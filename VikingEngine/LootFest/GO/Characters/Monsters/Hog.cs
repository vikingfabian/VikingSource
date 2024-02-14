//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using VikingEngine.LootFest;
//using VikingEngine.Physics;

//namespace VikingEngine.LootFest.GO.Characters.Monsters
//{
//    class Hog: AbsMonster
//    {
//        const float ScaleToBound = 0.35f;


//        public Hog(GoArgs args)
//            : base(args)
//        {
//            setHealth(LfLib.LargeEnemyHealth);
//            if (args.LocalMember)
//            {
//                {       NetworkShareObject();
//            }
//        }

//        //public Hog(System.IO.BinaryReader r)
//        //    : base(r)
//        //{
//        //}

//       override protected void createBound(float imageScale)
//       {
//           Vector3 boundHalfSz = new Vector3(0.175f * imageScale, 0.24f * imageScale, imageScale * 0.375f);
//           CollisionAndDefaultBound = new Bounds.ObjectBound(new BoundData2(new Box1axisBound(new VectorVolume(Vector3.Zero, boundHalfSz), Rotation1D.D0), new Vector3(0, boundHalfSz.Y, imageScale * -0.088f)));
//           TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(imageScale * ScaleToBound, imageScale * 0.1f, 0f);
//       }

//       bool pumbaSecret = Ref.rnd.RandomChance(0.02f);
//        protected override VoxelModelName imageName
//        {
//            get { return pumbaSecret? VoxelModelName.pumba : VoxelModelName.hog_lvl1; }//characterLevel == 0? VoxelModelName.hog_lvl1 : VoxelModelName.hog_lvl2; }
//        }

//        static readonly Graphics.AnimationsSettings AnimSet = 
//            new Graphics.AnimationsSettings(5, 0.8f);
//        protected override Graphics.AnimationsSettings animSettings
//        {
//            get { return AnimSet; }
//        }

//        protected const float HogWalkingSpeed = 0.008f;
//        protected const float HogRunningSpeed = 0.014f;
//        protected const float RunningSpeedLvl2 = HogRunningSpeed * 1.4f;
//        protected override float walkingSpeed
//        {
//            get { return HogWalkingSpeed; }
//        }
//        protected override float runningSpeed
//        {
//            get { return characterLevel == 0? HogRunningSpeed : RunningSpeedLvl2; }
//        }
//        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
//            Data.MaterialType.darker_yellow_orange,
//            Data.MaterialType.darker_red,
//            Data.MaterialType.dark_cool_brown);
//        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);

//        public override Effects.BouncingBlockColors DamageColors
//        {
//            get
//            {
//                return characterLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
//            }
//        }
//        public override GameObjectType Type
//        {
//            get { return GameObjectType.Hog; }
//        }
//        protected override Monster2Type monsterType
//        {
//            get { return Monster2Type.Hog; }
//        }

//        static readonly IntervalF ScaleRange = new IntervalF(4f, 5.5f);
//        protected override IntervalF scaleRange
//        {
//            get { return ScaleRange; }
//        }

//        protected override WeaponAttack.DamageData contactDamage
//        {
//            get
//            {
//                return CollisionDamage;
//            }
//        }

//        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
//        {
//            base.HandleColl3D(collData, ObjCollision);
//        }

//        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
//        {
//            base.DeathEvent(local, damage);

//#if PCGAME
//            //if (pumbaSecret && PlatformSettings.SteamAPI)
//            //    Ref.instance.achievementsKeeper.SetAchievement(SteamWrapping.SteamAchievementIndex.ACH_HAKUNA_MATATA_1_1);
//#endif
//        }
//    }
//}
