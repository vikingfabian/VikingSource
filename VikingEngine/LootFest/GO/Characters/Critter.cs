using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LootFest.GO.Physics;

namespace VikingEngine.LootFest.GO.Characters
{
    class Critter : AbsEnemy
    {
        static readonly IntervalF SheepScaleRange = new IntervalF(2.5f, 3.5f);
        static readonly IntervalF HenScaleRange = new IntervalF(2f, 2.2f);
        static readonly IntervalF PigScaleRange = new IntervalF(2.5f, 4f);
        static readonly IntervalF MinigPigScaleRange = new IntervalF(2f, 3.2f);
        static readonly IntervalF CowScaleRange = new IntervalF(5f, 6f);
        static readonly IntervalF PosAdd = new IntervalF(0.2f, Map.WorldPosition.ChunkWidth * 0.3f);
        bool walkingMode = false;
        float modeTimeLeft = 0;
        protected GameObjectType critterType;
        Time turnBackTimer = Time.Zero;
        
        public Critter(GoArgs args)
            : base(args)
        {
            critterType = args.type;
            Health = LfLib.CritterHealth;

            critterBasicInit();

            if (args.LocalMember)
            {
                //createAiPhys();
                NextMode();

                NetworkShareObject();

                if (critterType == GameObjectType.CritterWhiteHen)
                {
                    physics.SpeedY = 0.02f;
                }
            }
            else
            {
                image.position.Y += 2f;
            }
        }

        void critterBasicInit()
        {
            //Graphics.AnimationsSettings animSett;
            VoxelModelName imageName;
            
            if (critterType != GameObjectType.CritterPig)
            { TerrainInteractBound = CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(0.6f, 0.5f); }

            if (critterType == GameObjectType.CritterPig)
            {
                animSettings = new Graphics.AnimationsSettings(3, 1f);
                imageName = VoxelModelName.Pig;
                modelScale = PigScaleRange.GetRandom();
            }
            else if (critterType == GameObjectType.CritterHen || critterType == GameObjectType.CritterWhiteHen)
            {
                animSettings = new Graphics.AnimationsSettings(5, 1.6f, 1);
                imageName =critterType == GameObjectType.CritterHen? VoxelModelName.Hen : VoxelModelName.white_hen;
                modelScale = HenScaleRange.GetRandom();

                CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(0.26f * modelScale, 0.42f * modelScale, new Vector3(0, 0.24f * modelScale, 0.1f * modelScale));
            }
            else if (critterType == GameObjectType.CritterSheep)
            {
                animSettings = new Graphics.AnimationsSettings(3, 1f);
                imageName = VoxelModelName.sheep;
                modelScale = SheepScaleRange.GetRandom();
               // Health = 3;

                CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated(Vector3.One,
                    new Vector3(0.28f * modelScale, 0.24f * modelScale, 0.44f * modelScale),
                    new Vector3(0, 0.3f * modelScale, 0));
            }
            else if (critterType == GameObjectType.CritterMiningPig)
            {
                animSettings = new Graphics.AnimationsSettings(5, 1f);
                imageName = VoxelModelName.miner_pig;
                modelScale = MinigPigScaleRange.GetRandom();

                CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated(Vector3.One,
                    new Vector3(0.28f * modelScale, 0.28f * modelScale, 0.44f * modelScale),
                    new Vector3(0, 0.3f * modelScale, 0));
            }
            else if (critterType == GameObjectType.CritterMiningCow)
            {
                animSettings = new Graphics.AnimationsSettings(5, 1f);
                imageName = VoxelModelName.miner_cow;
                modelScale = CowScaleRange.GetRandom();

                CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated(Vector3.One,
                    new Vector3(0.22f * modelScale, 0.3f * modelScale, 0.44f * modelScale),
                    new Vector3(0, 0.3f * modelScale, 0));
            }
            else
            {
                throw new NotImplementedException();
            }

            
               

            image = LfRef.modelLoad.AutoLoadModelInstance(imageName,
                modelScale, 1, false);


            image.position = WorldPos.PositionV3;

            if (critterType == GameObjectType.CritterPig)
            {
                loadBounds();
            }
            else
            {
                Vector3 terrainBoundScale = new Vector3(CollisionAndDefaultBound.MainBound.halfSize.Z);
                TerrainInteractBound = new Bounds.ObjectBound(Bounds.BoundShape.BoundingBox, Vector3.Zero,
                    terrainBoundScale, Vector3.Up * terrainBoundScale.Y);
            }
            immortalityTime.Seconds = 0.5f;

            
        }

       

       
        static readonly IntervalF WalkingModeTime = new IntervalF(400, 1000);
        static readonly IntervalF WaitingModeTime = new IntervalF(400, 3000);

        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            base.AsynchGOUpdate(args);

            SolidBodyCheck(args.allMembersCounter);

            if (localMember)
            {   
                if (boundary != null)
                {
                    boundary.update(this);
                }
            }
        }

        const float WalkingSpeed = 0.007f;
        void NextMode()
        {
            walkingMode = !walkingMode;
            if (walkingMode)
            {
                Rotation = Rotation1D.Random();
                Velocity.Set(rotation, WalkingSpeed);
                modeTimeLeft = WalkingModeTime.GetRandom();
                setImageDirFromRotation();
            }
            else
            {
                Velocity.SetZeroPlaneSpeed();
                modeTimeLeft = WaitingModeTime.GetRandom();
            }
        }
        public override void Time_LasyUpdate(ref float time)
        {
            base.Time_LasyUpdate(ref time);
            modeTimeLeft -= time;
            turnBackTimer.CountDown();
            if (modeTimeLeft <= 0)
            {
                NextMode();
            }
        }
        public override void Time_Update(UpdateArgs args)
        {
            
            base.Time_Update(args);
        }

        protected override void activeCheckUpdate()
        {
            base.activeCheckUpdate();
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);
            if (localMember &&
                (critterType == GameObjectType.CritterMiningCow || critterType == GameObjectType.CritterMiningPig))
            {
                bool foundMinerCritter = false;
                var objects = LfRef.gamestate.GameObjCollection.localMembersUpdateCounter.IClone();
                while (objects.Next())
                {
                    if (objects.GetSelection is Critter && objects.GetSelection != this)
                    {
                        foundMinerCritter = true;
                        break;
                    }
                }

                if (!foundMinerCritter)
                {
                    //lib.DoNothing();
                    if (!BlockMap.Level.Lobby.spiderBossWp.WorldGrindex.IsZero)
                    {
                        new MinerSpider(new GoArgs(BlockMap.Level.Lobby.spiderBossWp));
                    }
                }
            }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            return true;//do nothing
        }
        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            HandleObsticle(true, ObjCollision);
        }
        public override void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
        {
            if (walkingMode)
            {
                if (turnBackTimer.TimeOut)
                {
                    rotation.Add(MathHelper.Pi + Ref.rnd.Plus_MinusF(1f));
                    Velocity = new Velocity(rotation, WalkingSpeed);
                    setImageDirFromRotation();
                    turnBackTimer.Seconds = 1f;
                }
            }
        }

        public override GameObjectType Type
        {
            get { return critterType; }
        }
        public override CardType CardCaptureType
        {
            get
            {
                switch (critterType)
                {
                    case GameObjectType.CritterPig:
                        return CardType.CritterPig;
                    case GameObjectType.CritterWhiteHen:
                        return CardType.CritterHen;
                    case GameObjectType.CritterSheep:
                        return CardType.CritterSheep;
                    default:
                        return CardType.UnderConstruction;

                }
            }
        }

        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Critter;
            }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsPig = new Effects.BouncingBlockColors(
            Data.MaterialType.pastel_red_orange,
            Data.MaterialType.dark_yellow_orange,
            Data.MaterialType.gray_75);
        public static readonly Effects.BouncingBlockColors DamageColorsMiningPig = new Effects.BouncingBlockColors(
            Data.MaterialType.light_magenta_red, 
            Data.MaterialType.pastel_magenta);
        static readonly Effects.BouncingBlockColors DamageColorsHen = new Effects.BouncingBlockColors(
            Data.MaterialType.gray_10, 
            Data.MaterialType.gray_45, 
            Data.MaterialType.light_red_orange);
        public static readonly Effects.BouncingBlockColors DamageColorsWhiteHen = new Effects.BouncingBlockColors(
            Data.MaterialType.gray_10,
            Data.MaterialType.gray_45,
            Data.MaterialType.light_red_orange);
        public static readonly Effects.BouncingBlockColors DamageColorsSheep = new Effects.BouncingBlockColors(
            Data.MaterialType.gray_25,
            Data.MaterialType.gray_80,
            Data.MaterialType.dark_red_orange);
        public static readonly Effects.BouncingBlockColors DamageColorsCow = new Effects.BouncingBlockColors(
            Data.MaterialType.darker_cool_brown, 
            Data.MaterialType.gray_35, 
            Data.MaterialType.gray_70);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                switch (critterType)
                {
                    default:
                        return DamageColorsSheep;
                    case GameObjectType.CritterHen:
                        return DamageColorsHen;
                    case GameObjectType.CritterWhiteHen:
                        return DamageColorsWhiteHen;

                    case GameObjectType.CritterMiningPig:
                        return DamageColorsMiningPig;
                    case GameObjectType.CritterPig:
                       return DamageColorsPig;
                    case GameObjectType.CritterMiningCow:
                       return DamageColorsCow;
                }
            }
        }
        public override Vector3 HeadPosition
        {
            get
            {
                Vector3 result = image.position;
                result.Y += CollisionAndDefaultBound.MainBound.halfSize.Y * 1.6f;
                return result;
            }
        }
        public override float ExspectedHeight
        {
            get
            {
                return 2;
            }
        }
    }

}
