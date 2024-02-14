using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.GO.Characters.Boss
{
    class StatueBoss : AbsEnemy
    {
        const int MaxSplitLevel = 3;

        int splitLevel;
        Map.WorldPosition center;
        BouncingObjPhysics2 bouncingPhys;

        Timer.Basic jumpTimer = new Timer.Basic(2000, true);
        Director.BossManager bossManager;

        public StatueBoss(GoArgs goArgs, BlockMap.AbsLevel level)
            : this(goArgs, level, goArgs.startWp, 0, int.MinValue)
        { }

        public StatueBoss(GoArgs args, BlockMap.AbsLevel level, Map.WorldPosition center, 
            int splitLevel, int startDir)
            :base(args)
        {
            this.center = center;
            WorldPos = args.startWp;
            
            if (args.reader != null)
            {
                this.splitLevel = args.reader.ReadByte();
            }
            else
            {
                this.splitLevel = splitLevel;
            }

            Health = 1;
            statueInit();

            if (args.LocalMember)
            {
                levelCollider.setLevel(level);

                bouncingPhys = (BouncingObjPhysics2)physics;
                bouncingPhys.Bounciness = 0.2f;
                bouncingPhys.maxBounces = 0;
                physics.Gravity *= 2 + 0.5f * splitLevel;


                if (splitLevel == 0)
                {
                    //First version
                    bossManager = new Director.BossManager(this, level, 
                        characterLevel == 0? Players.BabyLocation.Introduction : Players.BabyLocation.Barrels);
                }
                else
                {
                    immortalityTime.MilliSeconds = 500;
                    lastDamageLevel = float.MaxValue;
                    new Effects.DamageFlash(image, immortalityTime.MilliSeconds);
                }

                if (startDir >= 0)
                {
                    jump(startDir);
                }

                NetworkShareObject();
            }
        }

        void statueInit()
        {
            modelScale = 10 - 2 * splitLevel;

            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.statueboss,
                modelScale, 1f, false);

            WorldPos.SetAtClosestFreeY(1);
            image.position = WorldPos.PositionV3;

            float boundW = 0.18f * modelScale;
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickBoundingBox(new Vector3(boundW, boundW * 2f, boundW), boundW * 0.5f);
        }

        public override void netWriteGameObject(System.IO.BinaryWriter w)
        {
            base.netWriteGameObject(w);
            w.Write((byte)splitLevel);
        }

       

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (localMember && jumpTimer.Update())
            {
                const int MaxDistanceFromCenter = 12;
                UpdateWorldPos();
                IntVector2 centerDiff = new IntVector2(WorldPos.WorldGrindex.X - center.WorldGrindex.X, WorldPos.WorldGrindex.Z - center.WorldGrindex.Z);
                if (Math.Abs(centerDiff.X) > MaxDistanceFromCenter)
                {
                    jump(centerDiff.X > 0 ? 3 : 1);
                }
                else if (Math.Abs(centerDiff.Y) > MaxDistanceFromCenter)
                {
                    jump(centerDiff.Y > 0 ? 0 : 2);
                }
                else
                {
                    jump(-1);
                }
            }
        }
        public override void AsynchGOUpdate(UpdateArgs args)
        {
            base.AsynchGOUpdate(args);
            SolidBodyCheck(args.allMembersCounter);

            if (boundary != null)
            {
                boundary.update(this);
            }
        }

        void jump(int dir)
        {
            if (dir < 0)
            {
                dir = Ref.rnd.Int(4);
            }

            float speed = 0.01f;

            Velocity = new VikingEngine.Velocity(new Rotation1D(dir * MathHelper.PiOver2), speed);
            physics.SpeedY = speed * 3.6f;
            physics.WakeUp();


        }

        public override void onGroundPounce(float fallSpeed)
        {
            if (!localMember || bouncingPhys.numBounces == 0)
            {

                int smokeCount = 30;
                float smokeSpeed = 3; 

                float radiansStep = MathHelper.TwoPi / smokeCount;
                Rotation1D dir = Rotation1D.Random();
                var particles = new List<Graphics.ParticleInitData>(smokeCount);

                for (int i = 0; i < smokeCount; ++i)
                {
                    Vector2 planeDir = dir.Direction(1f);
                    Vector3 start = image.position + VectorExt.V2toV3XZ(image.Scale1D * 2f * planeDir);
                    start.Y += 0.1f;
                    particles.Add(new Graphics.ParticleInitData(start, VectorExt.V2toV3XZ(planeDir * smokeSpeed)));
                    dir.Radians += radiansStep;
                }

                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.RunningSmoke, particles);

                

                if (characterLevel > 0)//subLevel.WorldLevel is Map.LF3levels.ChallengeLevelZombies)
                {
                    Vector3 firePos = image.position;
                    firePos.Y -= 0.2f;

                    //fire four projectiles
                    Rotation1D fireDir = Rotation1D.D0;
                    for (int i = 0; i < 4; ++i)
                    {
                        new GO.WeaponAttack.TurretBullet(firePos, fireDir);
                        fireDir.Add(MathHelper.PiOver2);
                    }
                }

                if (localMember)
                {
                    NetworkWriteObjectState(AiState.Attacking);
                }
            }
        }

        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            //base.networkReadObjectState(state, r);
            onGroundPounce(1f);
        }

        

        

        protected override void moveImage(Velocity speed, float time)
        {
            physics.Update(time);
        }

        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            return updatesCount > 1 && base.willReceiveDamage(damage);
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);
            //StatueCount--;
            if (localMember)
            {
                if (splitLevel < MaxSplitLevel)
                {
                    int splitDir = Ref.rnd.Int(4);
                    for (int i = 0; i < 2; ++i)
                    {
                        var smallerStatue = new StatueBoss(new GoArgs(GameObjectType.NUM_NON, new Map.WorldPosition(image.position), characterLevel), Level, center, this.splitLevel + 1, splitDir);
                        bossManager.addBossObject(smallerStatue, false);
                        smallerStatue.bossManager = bossManager;

                        splitDir += 2;
                    }
                  
                }
            }
        }

        

        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            if (localMember)
            {
                if (physics.SpeedY < 0)
                {
                    base.handleCharacterColl(character, collisionData, usesMyDamageBound);
                    return false;
                }
            }
            else
            {
                if (clientGoalPosition.Y < image.position.Y)
                {
                    base.handleCharacterColl(character, collisionData, usesMyDamageBound);
                    return false;
                }
            }
            
            return true;
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.BouncingObj2;
            }
        }

        protected override void lootDrop()
        {
            //base.lootDrop();
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.StatueBoss; }
        }
        public override CardType CardCaptureType
        {
            get
            {
                return CardType.StatueBoss1;
            }
        }
        public override bool canBeCardCaptured
        {
            get
            {
                return splitLevel == MaxSplitLevel;
            }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.dirt_blue,
            Data.MaterialType.dirt_black,
            Data.MaterialType.gray_85);
        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }


        public override float LightSourceRadius
        {
            get
            {
                return modelScale * 0.5f;
            }
        }

        public override void onResetBoss()
        {
            this.DeleteMe();
        }
    }
}
