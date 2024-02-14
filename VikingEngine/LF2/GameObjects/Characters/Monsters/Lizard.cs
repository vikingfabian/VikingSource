using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    class Lizard: AbsMonster2
    {
        static readonly IntervalF ScaleRange = new IntervalF(4f, 5f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
        public Lizard(Map.WorldPosition startPos, int level)
            : base(startPos, level)
        {
            setHealth(LootfestLib.LizardHealth);
            NetworkShareObject();
        }
         public Lizard(System.IO.BinaryReader r)
            : base(r)
        {
        }
         override protected void createBound(float imageScale)
         {
             CollisionBound = LF2.ObjSingleBound.QuickRectangleRotatedFromFeet(new Vector3(0.22f * imageScale, 0.15f * imageScale, imageScale * 0.48f), 0f);
             TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBoundFromFeetPos(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f, 0f);
         }
        protected override VoxelModelName imageName
        {
            get { return areaLevel == 0? VoxelModelName.lizard1 : VoxelModelName.lizard2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(6, 0.8f);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }


        const float WalkingSpeed = 0.008f;
        const float WalkingSpeedLvl2 = WalkingSpeed * 1.4f;

        const float RunningSpeed = 0.014f;
        protected override float walkingSpeed
        {
            get { return areaLevel==0? WalkingSpeed : WalkingSpeedLvl2; }
        }
        protected override float runningSpeed
        {
            get { return RunningSpeed; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.orc_skin, Data.MaterialType.orange, Data.MaterialType.mossy_green);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.violet, Data.MaterialType.cyan, Data.MaterialType.blue);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return areaLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Lizard; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Lizard; }
        }
        public override float LightSourceRadius
        {
            get
            {
                return image.scale.X * 10;
            }
        }
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return areaLevel == 0 ? HighCollDamageLvl1 : HighCollDamageLvl2;
            }
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
              Gadgets.GoodsType.Scaley_skin, 60, Gadgets.GoodsType.Sharp_teeth, 85, Gadgets.GoodsType.Monster_egg, 100);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }
    }
}
