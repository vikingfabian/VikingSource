using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.DataLib
{
    static class SpriteCollection
    {
        public static Graphics.Sprite[] Sprites;
        //public static List<Graphics.Sprite> imageFiles = new List<VikingEngine.Graphics.Sprite>(); 
        //public static Dictionary<SpriteName, int> imagesNames = new Dictionary<SpriteName, int>();

        /// <returns>imageFiles index</returns>
        //public static int AddSprite(SpriteName name, Graphics.Sprite file)
        //{
        //    int index;
        //    if (imagesNames.ContainsKey(name))
        //    {
        //        index = imagesNames[name];
        //        imageFiles[index] = file;
        //    }
        //    else
        //    {
        //        index = imageFiles.Count;
        //        imagesNames.Add(name, index);
        //        imageFiles.Add(file);
        //    }
        //    return index;
        //}

        public static void Init()
        {
            const int NoImageSize = 8;

            Sprites = new Sprite[(int)SpriteName.NUM];
            Set(SpriteName.NO_IMAGE, new VikingEngine.Graphics.Sprite(new Rectangle(0, 0, NoImageSize, NoImageSize), (int)LoadedTexture.NO_TEXTURE)); 
        }
       
        public static void PullInEdgesOnTile(SpriteName name, int pullIn)
        {
            //int ix = imagesNames[name];
            Sprite sprite = Get(name);
            sprite.Source.X += pullIn;
            sprite.Source.Y += pullIn;
            sprite.Source.Width -= pullIn * PublicConstants.Twice;
            sprite.Source.Height -= pullIn * PublicConstants.Twice;
            Set(name, sprite);
        }

        public static Graphics.Sprite Get(SpriteName name)
        {            
            return Sprites[(int)name];
        }

        public static Vector2 TexureSize(SpriteName sprite)
        {
            Rectangle rect = Get(sprite).Source;
            return new Vector2(rect.Width, rect.Height);
        }

        public static float Ratio(SpriteName sprite, bool widthToHeight)
        {
            //w * X = h
            Vector2 tex = TexureSize(sprite);
            if (widthToHeight)
            {
                return tex.Y / tex.X;
            }
            else
            {
                return tex.X / tex.Y;
            }
        }

        public static Vector2 RatioFromLargestSide(SpriteName sprite)
        {
            Vector2 tex = TexureSize(sprite);
            if (tex.X > tex.Y)
            {
                return new Vector2(1f, tex.Y / tex.X);
            }
            else
            {
                return new Vector2(tex.X / tex.Y, 1f);
            }
        }

        public static void Set(SpriteName name, Sprite sprite)
        {
            Sprites[(int)name] = sprite;
        }
        
    }
}
