using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    class Moth : AbsMonster2
    {
        
        public Moth(Map.WorldPosition startPos, int level)
            : base(startPos, level)
        {
            Health = 1;
            NetworkShareObject();
        }
        public Moth(System.IO.BinaryReader r)
            : base(r)
        {
            basicInit();
        }

        override protected void createBound(float imageScale)
        {
            CollisionBound = LF2.ObjSingleBound.QuickRectangleRotated(new Vector3(0.3f * imageScale, 0.3f * imageScale, imageScale * 0.4f));
            TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.FlyingObj;
            }
        }


        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Bee; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.yellow, Data.MaterialType.dark_gray, Data.MaterialType.white);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.black, Data.MaterialType.red_brown, Data.MaterialType.red);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return areaLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Bee; }
        }
        public override float ExspectedHeight
        {
            get
            {
                return 0.5f;
            }
        }

        const float WalkingSpeed = 0.011f;
        const float WalkingSpeedLvlAdd = 0.002f;

        protected override float walkingSpeed
        {
            get { return WalkingSpeed + WalkingSpeedLvlAdd * areaLevel; }
        }
        protected override float runningSpeed
        {
            get { return 0; }
        }

        static readonly Graphics.AnimationsSettings AnimSet =
            new Graphics.AnimationsSettings(2, 0.8f, 0);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }

        protected override VoxelModelName imageName
        {
            get { return areaLevel == 0 ? VoxelModelName.bee : VoxelModelName.bee2; }
        }
        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
              Gadgets.GoodsType.NONE, 0, Gadgets.GoodsType.NONE, 0, Gadgets.GoodsType.NONE, 0);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }
        static readonly IntervalF ScaleRange = new IntervalF(1.6f, 2f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
    }
}
