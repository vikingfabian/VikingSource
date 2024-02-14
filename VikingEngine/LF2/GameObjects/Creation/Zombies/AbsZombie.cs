using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Creation.Zombies
{
    abstract class AbsZombie : GameObjects.Characters.AbsEnemy 
    {
        protected static List<AbsZombie> leaders = new List<AbsZombie>();

        protected Timer.Basic stateTime;

        protected static readonly IntervalF CharacterScale = new IntervalF(0.15f, 0.17f);
        protected abstract IntervalF ScaleRange { get; }
        protected abstract float StartHealth { get; }
        protected GameObjects.Characters.Hero targetHero;

        public AbsZombie(int areaLevel)
            : base(areaLevel)
        {
            basicInit(ScaleRange.GetRandom());

            //updateTarget();
            this.targetHero = GetRandomHero();
            float appearRadius = Map.WorldPosition.ChunkWidth * 1.5f + lib.RandomDifferance(6);
            Vector2 addPos = lib.AngleToV2(lib.RandomRotation(), appearRadius);
            image.Position = targetHero.Position + Map.WorldPosition.V2toV3(addPos);
            WorldPosition = new Map.WorldPosition(image.Position);
            image.Position.Y = LfRef.chunks.GetScreen(WorldPosition).GetGroundY(WorldPosition) + 0.5f;
            stateTime = new Timer.Basic(10);
            rotation = Rotation1D.Random;
            setImageDirFromRotation();
            Health = StartHealth;
            NetworkShareObject();

            if (isLeader)
            {
                leaders.Add(this);
            }
#if CMODE
            LfRef.gamestate.NumZombies++;
#endif
        }
        public AbsZombie(System.IO.BinaryReader packetReader)
            : base(packetReader)
        {
            Health = StartHealth;
            basicInit(packetReader.ReadSingle());
            stateTime.Set(BasicStateTime);

            if (PlatformSettings.DebugOptions)
            {
                image.Scale.Y *= 1.6f;
            }
        }
        //public override void ReceiveDamageCollision(GameObjects.Weapons.DamageData damage, bool local)
        //{
        //    base.ReceiveDamageCollision(damage, local);
        //    //bool test = this.localMember;
        //    //bool willDamage = GameObjects.Weapons.WeaponLib.WillCollide(this.WeaponTargetType, damage.User);
        //}
        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write(image.Scale.X);
        }
        protected override void clientSpeed(float speed)
        {
            image.UpdateAnimation(speed, Ref.DeltaTimeMs);
        }

        
        ////public override void ClientTimeUpdate(float time, List<AbsUpdateObj> args.localMembersCounter, List<AbsUpdateObj> active)
        //{
            

        //    base.ClientTimeUpdate(time, args.localMembersCounter, active);
        //}


        protected abstract VoxelModelName VoxelObjName { get; }

        void basicInit(float scale)
        {
            this.image = mesh(VoxelObjName);
            image.Scale = lib.V3(scale);
            CollisionBound = LootFest.ObjSingleBound.QuickBoundingBox(6 * scale);
        }


        Graphics.VoxelModelInstance mesh(VoxelModelName objName)
        {
            return new Graphics.VoxelModelInstance(
                    LootfestLib.Images.StandardAnimatedVoxelObjects[objName],
                animationsSettings);
        }
        abstract protected Graphics.AnimationsSettings animationsSettings { get; }
        

        virtual protected bool isLeader { get { return false; } }
        virtual protected bool willFlee { get { return false; } }
        virtual protected float fleeRange { get { throw new NotImplementedException(); } }
        abstract protected IntervalF SpeedRange { get; } //static readonly RangeF SpeedRange = new RangeF(0.005f, 0.007f);
        virtual protected IntervalF RunningSpeedRange { get { throw new NotImplementedException(); } } //static readonly RangeF RunningSpeedRange = new RangeF(0.01f, 0.014f);
        abstract protected IntervalF BasicStateTime { get; } //static readonly RangeF BasicStateTime = new RangeF(1000, 3000);
        abstract protected int ChanceToMove { get; }//50
        abstract protected float WalkingToTargetRndAngleAdd { get; } //MathHelper.Pi * 0.8f
        abstract protected AttackType attackType { get; }
        virtual protected IntervalF attackDistance { get { throw new NotImplementedException(); } }
        virtual protected Range attackReloadTimeInStates { get { throw new NotImplementedException(); } }
        virtual protected void fireProjectile() { throw new NotImplementedException(); } 
        int attackReloadInStates = 0;

        
        virtual protected LoadedSound MoanSound { get { return LoadedSound.zombie_moan_1; } }

        void maybeSound()
        {
            if (lib.PercentChance(2))
            {
                Music.SoundManager.PlaySound(MoanSound, image.Position);
            }
        }

        bool bFireProjectile = false;
        //public override void Time_Update(float time, float halfUpdateTime, List<GameObjects.AbsUpdateObj> active, bool halfUpdate)
        //{
        public override void  Time_Update(GameObjects.UpdateArgs args)
        {
 	        base.Time_Update(args);
//}
//            base.Time_Update(args);

            if (localMember)
            {
                if (bFireProjectile)
                {
                    bFireProjectile = false;
                    fireProjectile();
                }
            }
            else
            {
                if (stateTime.Update(args.time))
                {
                    stateTime.Set(BasicStateTime);
                    maybeSound();
                }

                checkCharacterCollision(args.localMembersCounter);
            }
        }
        //public override void ClientTimeUpdate(float time, List<GameObjects.AbsUpdateObj> args.localMembersCounter, List<GameObjects.AbsUpdateObj> active)
        //{
        //    if (stateTime.Update(time))
        //    {
        //        stateTime.Set(BasicStateTime);
        //        maybeSound();
        //    }

        //    checkCharacterCollision(args.localMembersCounter);
        //    base.ClientTimeUpdate(time, args.localMembersCounter, active);
        //}

        public override void  AIupdate(GameObjects.UpdateArgs args)
        {
            if (localMember)
            {
                if (stateTime.Update(args.time))
                {
                    maybeSound();

                    GameObjects.Characters.Hero fleefrom = flee();
                    if (fleefrom != null)
                    {
                        rotation.Radians = this.AngleDirToObject(fleefrom) + lib.RandomDifferance(0.2f) + MathHelper.Pi;
                        this.AngleToSpeed(rotation, RunningSpeedRange.GetRandom());
                    }
                    else if (attackType != AttackType.NONE && attackReloadInStates <= 0 && attackDistance.IsWithinRange(this.PositionDiff3D(targetHero).Length()))
                    {
                        attackReloadInStates = attackReloadTimeInStates.GetRandom();
                        //use projectile
                        if (attackType == AttackType.Projectile)
                        {
                            //fireProjectile();
                            bFireProjectile = true;
                        }
                        else
                        {
                            rotation.Radians = this.AngleDirToObject(targetHero);
                            this.AngleToSpeed(rotation, RunningSpeedRange.GetRandom());
                        }
                    }
                    else
                    {
                        updateTarget();
                        if (lib.PercentChance(ChanceToMove))
                        {
                            rotation.Radians = this.AngleDirToObject(targetHero) + lib.RandomDifferance(WalkingToTargetRndAngleAdd);
                            this.AngleToSpeed(rotation, SpeedRange.GetRandom());
                        }
                        else
                        {
                            Velocity.SetZeroPlaneSpeed();
                        }
                    }
                    stateTime.Set(BasicStateTime);

                    attackReloadInStates--;
                }
                CheckOutSideWorldsBounds();
                
            }
            checkCharacterCollision(args.localMembersCounter);
        }

        

        void updateTarget()
        {
            float closest = float.MaxValue;
            List<GameObjects.Characters.Hero> heroes = LfRef.gamestate.AllHeroes();
            foreach (GameObjects.Characters.Hero h in heroes)
            {
                float l = PositionDiff(h).Length();
                if (l < closest)
                {
                    closest = l;
                    targetHero = h;
                }
            }
        }

        GameObjects.Characters.Hero flee()
        {
            if (willFlee)
            {
                List<GameObjects.Characters.Hero> heroes = LfRef.gamestate.AllHeroes();
                foreach (GameObjects.Characters.Hero h in heroes)
                {
                    if (PositionDiff(h).Length() <= fleeRange)
                    {
                        return h;
                    }
                }
            }
            return null;
        }

        public override void  HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
 	        base.HandleColl3D(collData, ObjCollision);
            stateTime.goalTime *= 0.5f;
        }
        

        protected override void  DeathEvent(bool local, VikingEngine.LootFest.GameObjects.WeaponAttack.DamageData damage)
        {
 	         base.DeathEvent(local, damage);
             //drop coin
             if (local)
             {
                 int numCoins = 1;
                 if (LfRef.gamestate.TerrainDestruction && lib.PercentChance(50))
                     numCoins = 2;
                 for (int i = 0; i < numCoins; i++)
                 {
                     LootDrop();
                 }
             }
             //LfRef.gamestate.DeadEnemy(CharacterType);
        }

        virtual protected void LootDrop()
        {

        }

        static readonly Effects.BouncingBlockColors ZombieDamageColors = new Effects.BouncingBlockColors(Data.MaterialType.zombie_skin, Data.MaterialType.violet, Data.MaterialType.blue_gray);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return ZombieDamageColors;
            }
        }
        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (isLeader)
            {
                leaders.Remove(this);
            }

        }
        
        protected override bool pushable
        {
            get
            {
                return false;
            }
        }
    }
    enum AttackType
    {
        NONE,
        Charge,
        Projectile,
    }
}
