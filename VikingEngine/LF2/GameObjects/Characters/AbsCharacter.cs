using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters
{
    abstract class AbsCharacter : AbsGameObject
    {
        protected static readonly Data.TempVoxelReplacementSett TempCharacterImage = new Data.TempVoxelReplacementSett(VoxelModelName.Character, true);
        protected AiState aiState = AiState.Waiting;
        protected Time aiStateTimer = 0;
        protected IntVector2 spawnChunk;

        Int32Bits conditions = new Int32Bits();
        public bool GetCondition(Condition.ConditionType type)
        {
            return conditions.Get((int)type);
        }
        public void SetCondition(Condition.ConditionType type, bool value)
        {
            conditions.Set((int)type, value);
        }

        public float StunnedSpeedModifier = 1;

        
        public override ObjectType Type
        {
            get { return ObjectType.Character; }
        }
        public override int  UnderType
        {
	        get { return (int)CharacterType; }
        }
        abstract public CharacterUtype CharacterType { get; }

        public AbsCharacter(System.IO.BinaryReader r)
            :base(r)
        {

        }
        public AbsCharacter()
            :base()
        {
            
        }

        
        
        virtual public void UpdateMissioni() { }
        protected AbsCharacter getClosestCharacter(float maxDistance, ISpottedArrayCounter<AbsUpdateObj> objects, WeaponAttack.WeaponUserType friendOrFoe)
        {
            LowestValue lowestDist = new LowestValue(true);
            objects.Reset();
            while (objects.Next())
            {
                if (objects.GetMember.Type == ObjectType.Character && (WeaponAttack.WeaponLib.IsFoeTarget(friendOrFoe, objects.GetMember.WeaponTargetType, true)))
                {
                    float dist = distanceToObject(objects.GetMember);
                    if (dist <= maxDistance)
                    {
                        lowestDist.Next(dist, objects.CurrentIndex);
                    }
                }
            }


            if (!lowestDist.hasValue)
                return null;

            return (AbsCharacter)objects.GetFromIndex(lowestDist.LowestMemberIndex);
            
        }

        protected AbsCharacter getRndCharacter(float maxDistance, ISpottedArrayCounter<GameObjects.AbsUpdateObj> counter, WeaponAttack.WeaponUserType ofType)
        {
            List<AbsUpdateObj> inView = null;
            counter.Reset();
            while (counter.Next())
            {
                if (counter.GetMember.Type == ObjectType.Character &&
                    counter.GetMember.WeaponTargetType == ofType &&
                    distanceToObject(counter.GetMember) <= maxDistance)
                {
                    if (inView == null)
                    {
                        inView = new List<AbsUpdateObj> { counter.GetMember };
                    }
                    else
                    {
                        inView.Add(counter.GetMember);
                    }
                }

            }

            if (inView == null)
            {
                return null;
            }
            else
            {
                return (AbsCharacter)arraylib.RandomListMemeber(inView);
            }
        }

        protected AbsCharacter getRndCharacterWithinView(float maxDistance, float maxAngle, ISpottedArrayCounter<GameObjects.AbsUpdateObj> objects, WeaponAttack.WeaponUserType friendOrFoe)
        {
            List<AbsUpdateObj> inView = null;
            
            objects.Reset();
            while (objects.Next())
            {
                if (objects.GetMember.Type == ObjectType.Character &&
                    WeaponAttack.WeaponLib.IsFoeTarget(friendOrFoe, objects.GetMember.WeaponTargetType, true) &&
                    distanceToObject(objects.GetMember) <= maxDistance &&
                    LookingAtObject(objects.GetMember, maxAngle)
                    )
                {
                    if (inView == null)
                    {
                        inView = new List<AbsUpdateObj> { objects.GetMember };
                    }
                    else
                    {
                        inView.Add(objects.GetMember);
                    }
                }
            }

            if (inView == null)
            {
                return null;
            }
            else
            {
                return (AbsCharacter)arraylib.RandomListMemeber(inView);
            }
        }

        protected AbsUpdateObj GetClosestObjectType(ISpottedArrayCounter<GameObjects.AbsUpdateObj> objects, ObjectType objType, List<int> undertypes, float maxDistance)
        {
            LowestValue lowestDist = new LowestValue(true);

            objects.Reset();
            while (objects.Next())
            {
                if (objects.GetMember.Type == objType && undertypes.Contains(objects.GetMember.UnderType))
                {
                    float dist = distanceToObject(objects.GetMember);
                    if (dist <= maxDistance)
                    {
                        lowestDist.Next(dist, objects.CurrentIndex);
                    }
                }
            }

            if (!lowestDist.hasValue)
                return null;
            return objects.GetFromIndex(lowestDist.LowestMemberIndex);
        }

        protected void checkCharacterCollision(ISpottedArrayCounter<GameObjects.AbsUpdateObj> counter)
        {
            counter.Reset();
            while (counter.Next())
            {
                if (counter.GetMember.Type == ObjectType.Character && WeaponAttack.WeaponLib.IsFoeTarget(this.WeaponTargetType, counter.GetMember.WeaponTargetType, false))
                {
                    if (CollisionBound != null)
                    {
                        LF2.ObjBoundCollData collisionData = CollisionBound.Intersect2(counter.GetMember.CollisionBound);
                        if (collisionData != null)
                        {
                            handleCharacterColl((AbsCharacter)counter.GetMember, collisionData);
                        }
                    }
                }
            }
        }

        override public NetworkShare NetworkShareSettings
        {
            get
            {
                if (localMember)
                {
                    NetworkShare NetShare = GameObjects.NetworkShare.Full;
                    NetShare.Update = !Velocity.ZeroPlaneSpeed || updatePositionToNewbie > 0 || Ref.rnd.RandomChance(20);
                    return NetShare;
                }
                else
                {
                    return  GameObjects.NetworkShare.Full;
                }
            }
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            if (WeaponTargetType == WeaponAttack.WeaponUserType.Enemy)
            {
                Data.WorldsSummaryColl.CurrentWorld.KilledEnemy();
            }
            base.DeathEvent(local, damage);
        }

        //protected void damageText(float damage)
        //{
        //    if (Ref.draw.ActivePlayerScreens.Count == 1)
        //    {
        //        const float DamTextTime = 1600;
        //        Graphics.TextG damText = new TextG(LoadedFont.Lootfest, Ref.draw.Camera.From3DToScreenPos(image.Position),
        //            Vector2.One * 1.2f, Align.CenterAll, Convert.ToInt32(damage).ToString(), damageTextCol, ImageLayers.AbsoluteTopLayer);
        //        new Graphics.Motion2d(MotionType.MOVE, damText, Vector2.UnitY * -100, MotionRepeate.NO_REPEATE, DamTextTime, true);
        //        new Graphics.Motion2d(MotionType.TRANSPARENSY, damText, new Vector2NegativeOne, MotionRepeate.NO_REPEATE, DamTextTime, true);
        //        new Timer.Terminator(DamTextTime, damText);
        //    }
        //}

        virtual protected Color damageTextCol { get { return Color.White; } }

        protected void characterCritiqalUpdate(bool outsideCheck)
        {

            immortalityTime.CountDown();
            updateHealthBar();
            UpdateBound();

            if (outsideCheck && localMember && checkOutsideUpdateArea_ActiveChunk())
            {
                DeleteMe();
            }
        }
        
        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
            if (!damage.Secondary && local)
            {
                switch (damage.Magic)
                {
                    case Magic.MagicElement.Poision:
                        if (!GetCondition(Condition.ConditionType.Poisioned))
                            new Condition.Poision(this, damage);
                        break;
                    case Magic.MagicElement.Fire:
                        if (!GetCondition(Condition.ConditionType.Burning))
                            new Condition.Burning(this, damage);
                        break;
                    case Magic.MagicElement.Evil:
                        Vector3 smokeCenter = image.position; smokeCenter.Y += 2;
                        Engine.ParticleHandler.AddParticleArea(ParticleSystemType.Smoke, smokeCenter, 1, 6);
                        break;
                }
            }
            //blood
            BlockSplatter();
        }
        
        public override void Time_Update(UpdateArgs args)
        {
            if (localMember || !NetworkShareSettings.Update)
            {
                moveImage(Velocity, args.time);
            }

            updateAnimation();
            base.Time_Update(args);
        }

        virtual protected void updateAnimation()
        {
            image.UpdateAnimation(localMember ? Velocity.PlaneLength() : clientSpeedLength, Ref.DeltaTimeMs);
        }
        
        public override void Time_LasyUpdate(ref float time)
        {
            base.Time_LasyUpdate(ref time);
            
            updatePositionToNewbie--;
        }
        protected override void moveImage(Velocity speed, float time)
        {
            if (localMember)
            {
                speed.Value *= StunnedSpeedModifier;
                image.position = physics.UpdateMovement();
            }
            else
                base.moveImage(speed, time);
        }
        protected override bool autoMoveImage
        {
            get
            {
                return false;
            }
        }

        virtual public void HandleCastleRoomCollision()
        { }
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return GameObjects.ObjPhysicsType.CharacterSimple;
            }
        }

        public Vector3 Scale
        {
            get { return image.scale; }
        }

        override public bool SolidBody
        {
            get { return true; }
        }
        public override bool IsWeaponTarget
        { get { return true; } }

        protected override RecieveDamageType recieveDamageType
        {
            get { return RecieveDamageType.ReceiveDamage; }
        }
       
        override public LightParticleType LightSourceType { get { return Graphics.LightParticleType.Shadow; } }
    }
    enum CharacterUtype
    {
        NON,
        Player,
        NPC,
        Dummie,
        Craeture,
        FlyingPet,
        CritterPig,
        CritterHen,
        CritterSheep,

        Crocodile,
        Ent,
        FireGoblin,
        Frog,
        Harpy,
        Hog,
        OldSwine,
        Lizard,
        Scorpion,
        Spider,
        Wolf,
        Squig,
        SquigSpawn,

        Ogre,
        Humanioid,
        Grunt,
        Magician,
        EggNest,

        Mummy,
        Ghost,
        Fly,
        Bat,

        ShootingTurret,
        TrapRotating,
        TrapBackNforward,

        EndBossMount,
        EvilSpider,

        Bee,
        BeeHive,
        MiningSpot,

        Zombie,
        Skeleton,
        LeaderZombie,
        BabyZombie,
        DogZombie,
        FatZombie,

        CritterWhiteHen,
    }
}
