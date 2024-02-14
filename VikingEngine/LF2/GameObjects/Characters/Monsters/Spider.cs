using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    class Spider: AbsMonster2
    {
        static readonly IntervalF ScaleRange = new IntervalF(3f, 3.2f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
        public Spider(Map.WorldPosition startPos, int level)
            : base(startPos, level)
        {
            setHealth(LootfestLib.SpiderHealth);
            NetworkShareObject();
        }
         public Spider(System.IO.BinaryReader r)
            : base(r)
        {
        }
         override protected void createBound(float imageScale)
         {
             CollisionBound = LF2.ObjSingleBound.QuickRectangleRotatedFromFeet(new Vector3(0.48f * imageScale, 0.2f * imageScale, imageScale * 0.42f), 0f);
             TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBoundFromFeetPos(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f, 0f);
         }
        protected override VoxelModelName imageName
        {
            get { return  areaLevel == 0?  VoxelModelName.spider1 : VoxelModelName.spider2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(6, 0.8f);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }

        

        const float WalkingSpeed = 0.008f;
        const float RunningSpeed = 0.014f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return RunningSpeed; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.blue_gray, Data.MaterialType.dark_gray, Data.MaterialType.black);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.blue, Data.MaterialType.dark_blue, Data.MaterialType.black);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return areaLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Spider; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Spider; }
        }
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return areaLevel == 0 ? MediumCollDamageLvl1 : MediumCollDamageLvl2;
            }
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
              Gadgets.GoodsType.Thread, 60, Gadgets.GoodsType.Poision_sting, 85, Gadgets.GoodsType.Nose_horn, 100);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }
    }
}
