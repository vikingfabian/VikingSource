using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ
{
    class HatImage : IDeleteable
    {
        public Graphics.Image image; Graphics.Image animalImg;
        Vector2 offset;

        public HatImage(Hat type, Graphics.Image animalImg, AnimalSetup animalSetup)
        {
            //AnimalSetup animalSetup = AnimalSetup.Get(animal);

            const float MaskOffsetYAdj = 0.04f;
            this.offset = animalSetup.hatOffset;
            this.animalImg = animalImg;

            SpriteName tile;
            switch (type)
            {
                case Hat.BlingCap:
                    tile = SpriteName.hatBlingCap;
                    break;
                case Hat.Bow:
                    tile = SpriteName.hatBow;
                    break;
                case Hat.Bunny:
                    tile = SpriteName.hatBunny;
                    this.offset.Y -= 0.02f;
                    break;
                case Hat.Butterix:
                    tile = SpriteName.hatButterix;
                    break;
                case Hat.ChildCap:
                    tile = SpriteName.hatChidCap1;
                    break;
                case Hat.Cowboy:
                    tile = SpriteName.hatCowboy;
                    break;
                case Hat.English:
                    tile = SpriteName.hatEnglish;
                    break;
                case Hat.Fez:
                    tile = SpriteName.hatFez;
                    break;
                case Hat.Frank:
                    tile = SpriteName.hatFrank;
                    this.offset.Y += MaskOffsetYAdj;
                    break;
                case Hat.GooglieEyes:
                    tile = SpriteName.hatGooglieEyes;
                    break;
                case Hat.HeartEyes:
                    tile = SpriteName.hatLoveEyes;
                    break;
                case Hat.Halo:
                    tile = SpriteName.hatHalo;
                    break;
                case Hat.High:
                    tile = SpriteName.hatHigh;
                    break;
                case Hat.Indian:
                    tile = SpriteName.hatIndian;
                    break;
                case Hat.Pirate:
                    tile = SpriteName.hatPirate;
                    break;
                case Hat.RobinHood:
                    tile = SpriteName.hatRobinHood;
                    break;
                case Hat.Viking:
                    tile = SpriteName.hatViking;
                    break;
                case Hat.Vlc:
                    tile = SpriteName.hatVlc;
                    break;


                case Hat.HighRainbow:
                    tile = SpriteName.hatRainbow;
                    break;
                case Hat.King:
                    tile = SpriteName.hatKing;
                    break;
                case Hat.Princess:
                    tile = SpriteName.hatPrincess;
                    break;
                case Hat.Riddler:
                    tile = SpriteName.hatRiddler;
                    this.offset.Y += MaskOffsetYAdj;
                    break;
                case Hat.Scotish:
                    tile = SpriteName.hatScottish;
                    break;
                case Hat.Shades:
                    tile = SpriteName.hatShades;
                    break;
                case Hat.SkyMask:
                    tile = SpriteName.hatSkyMask;
                    this.offset.Y += MaskOffsetYAdj;
                    break;
                case Hat.UniHorn:
                    tile = SpriteName.hatUniHorn;
                    break;              
                

                default:
                    tile = SpriteName.NO_IMAGE;
                    break;
            }

            image = new Graphics.Image(tile, Vector2.Zero, Vector2.One, ImageLayers.AbsoluteBottomLayer, true);
            image.LayerAbove(animalImg);
            update();
        }

        public void DeleteMe()
        {
            image.DeleteMe();
        }
        public bool IsDeleted
        {
            get { return image.IsDeleted; }
        }

        public void update()
        {
            image.Size = animalImg.Size * 0.5f;

            Vector2 off = offset * animalImg.Size;
            if (image.spriteEffects == SpriteEffects.FlipHorizontally)
            {
                off.X = -off.X;
            }

            image.Position = animalImg.Position + VectorExt.RotateVector(off, animalImg.Rotation);
            image.spriteEffects = animalImg.spriteEffects;
            image.Rotation = animalImg.Rotation;
        }
    }
}
