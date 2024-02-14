using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2.Editor;

namespace VikingEngine.LF2.GameObjects.PickUp
{
    /// <summary>
    /// Hero throws away a piece of food when healed up by eating
    /// </summary>
    class FoodScrap : AbsPickUp
    {
        VoxelObjName imgType = VoxelObjName.NUM_Empty; 

        public FoodScrap(GameObjects.Characters.Hero hero, GameObjects.Gadgets.GoodsType foodType)
            : base(Vector3.Zero)
        {
            scale = 1.4f;
            switch (foodType)
            {
                default:
                    scale = 1.2f;
                    imgType = VoxelObjName.apple_scrap; 
                    break;

                case GameObjects.Gadgets.GoodsType.Apple_pie:
                    scale = 1.6f;
                    imgType = VoxelObjName.applepie_scrap;
                    break;
                case GameObjects.Gadgets.GoodsType.Bread:
                    scale = 0.6f;
                    imgType = VoxelObjName.bread_scrap;
                    break;
                case GameObjects.Gadgets.GoodsType.Grapes:
                    imgType = VoxelObjName.grape_scrap;
                    break;
                case GameObjects.Gadgets.GoodsType.Meat:
                    goto case GameObjects.Gadgets.GoodsType.Grilled_meat;

                case GameObjects.Gadgets.GoodsType.Grilled_meat:
                    scale = 1.0f;
                    imgType = VoxelObjName.meat_scrap;
                    break;
            }

            Vector3 pos = hero.Position;
            pos.Y += 2;
            startSetup(pos);
        }

        protected override VoxelObjName imageType
        {
            get { return imgType; }
        }

        static readonly Data.TempBlockReplacementSett AppleScrapTempImage = new Data.TempBlockReplacementSett(new Color(255, 65, 27), new Vector3(0.2f, 1, 0.2f));
        static readonly Data.TempBlockReplacementSett ApplePieScrapTempImage = new Data.TempBlockReplacementSett(new Color(135,173,235), new Vector3(0.6f, 0.1f, 0.6f));
        static readonly Data.TempBlockReplacementSett BreadScrapTempImage = new Data.TempBlockReplacementSett(new Color(127,72,22), new Vector3(0.3f, 0.3f, 1f));
        static readonly Data.TempBlockReplacementSett GrapesScrapTempImage = new Data.TempBlockReplacementSett(new Color(63, 123, 40), new Vector3(0.2f, 0.6f, 0.2f));


        protected override Data.TempBlockReplacementSett tempImage
        {
            get
            {
                switch (imgType)
                {
                    case VoxelModelName.apple_scrap: return AppleScrapTempImage;
                    case VoxelModelName.applepie_scrap: return ApplePieScrapTempImage;
                    case VoxelModelName.bread_scrap: return BreadScrapTempImage;
                    case VoxelModelName.grape_scrap: return GrapesScrapTempImage;
                    case VoxelModelName.meat_scrap: return BoneTempImage;
                    
                }
                throw new NotImplementedException("Foodscrap temp img, " + imageType.ToString());
            }
        }

        float scale;
        override protected float imageScale { get { return scale; } }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GameObjects.NetworkShare.None;
            }
        }

        public override int UnderType
        {
            get { return (int)PickUpType.FoodScrap; }
        }

        const float PickUpLifeTime = 10000;
        override protected float pickUpLifeTime
        {
            get { return PickUpLifeTime; }
        }

        public override float LightSourceRadius
        {
            get
            {
                return scale * 1.2f;
            }
        }
    }
}
