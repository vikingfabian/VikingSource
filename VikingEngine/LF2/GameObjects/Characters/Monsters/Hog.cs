using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LF2;
using VikingEngine.Physics;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    class Hog: AbsMonster2
    {
        const float ScaleToBound = 0.35f;

        public Hog(Map.WorldPosition startPos, int level)
            : base(startPos, level)
        {
            setHealth(LootfestLib.HogHealth);
            NetworkShareObject();
        }

       public Hog(System.IO.BinaryReader r)
            : base(r)
        {
        }
       override protected void createBound(float imageScale)
       {
           Vector3 boundHalfSz = new Vector3(0.175f * imageScale, 0.24f * imageScale, imageScale * 0.375f);
           CollisionBound = new ObjSingleBound(new BoundData2(new Box1axisBound(new VectorVolume(Vector3.Zero, boundHalfSz), Rotation1D.D0), new Vector3(0, boundHalfSz.Y, imageScale * -0.088f)));
           TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBoundFromFeetPos(imageScale * ScaleToBound, imageScale * 0.1f, 0f);
       }
        protected override VoxelModelName imageName
        {
            get { return areaLevel == 0? VoxelModelName.hog_lvl1 : VoxelModelName.hog_lvl2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(5, 0.8f);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }

        protected const float WalkingSpeed = 0.008f;
        protected const float RunningSpeed = 0.014f;
        protected const float RunningSpeedLvl2 = RunningSpeed * 1.4f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return areaLevel == 0? RunningSpeed : RunningSpeedLvl2; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.leather, Data.MaterialType.dark_gray, Data.MaterialType.bone);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.red_brown, Data.MaterialType.red, Data.MaterialType.bone);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return areaLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Hog; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Hog; }
        }

        static readonly IntervalF ScaleRange = new IntervalF(4f, 5.5f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }

        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return areaLevel == 0 ? MediumCollDamageLvl1 : MediumCollDamageLvl2;
            }
        }

        public override void HandleColl3D(Collision3D collData, AbsUpdateObj ObjCollision)
        {
            base.HandleColl3D(collData, ObjCollision);
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
              Gadgets.GoodsType.Tusks, 60, Gadgets.GoodsType.Skin, 85, Gadgets.GoodsType.Meat, 100);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }
    }
}
