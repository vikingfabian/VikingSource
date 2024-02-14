using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Graphics
{
    struct Sprite
    {

        public Rectangle Source;
        public int textureIndex;
        /// <summary>
        /// Koordinaterna för 3d rendering
        /// </summary>
        public VectorRect SourceF; //must be values between 0-1

        public Vector2 SourcePolygonTopLeft, SourcePolygonTopRight, SourcePolygonLowLeft, SourcePolygonLowRight;

        public override string ToString()
        {
            return textureIndex.ToString() + ", SourceF:" + SourceF.ToString();
        }

        public Sprite(Rectangle source, LoadedTexture textureIx)
            : this()
        {
            Source = source;
            textureIndex = (int)textureIx;
            UpdateSourcePolygon(true, Engine.LoadContent.Textures[textureIndex]);

        }

        public Sprite(Texture2D tex)
            : this()
        {
            Source = new Rectangle(0, 0, tex.Width, tex.Height);
            textureIndex = 0;
            UpdateSourcePolygon(true, tex);
        }

        public void UpdateSourceF(bool pullEdges, Texture2D tex)
        {
            if (tex == null)
            {
                tex = Engine.LoadContent.Textures[textureIndex];
            }

            if (pullEdges)
            {
                const float PullSize = 1f;

                SourceF = new VectorRect(
                    (Source.X + PullSize) / tex.Width,
                    (Source.Y + PullSize) / tex.Height,
                    (Source.Width - PullSize * 2f) / tex.Width,
                    (Source.Height - PullSize * 2f) / tex.Height);
            }
            else
            {
                SourceF = new VectorRect(
                    (float)Source.X / tex.Width,
                    (float)Source.Y / tex.Height,
                    (float)Source.Width / tex.Width,
                    (float)Source.Height / tex.Height);
            }
        }



        public void UpdateSourcePolygon(bool pullEdges, Texture2D tex = null)
        {
            UpdateSourceF(pullEdges, tex);

            SourcePolygonTopLeft = SourceF.Position;
            SourcePolygonTopRight = SourceF.Position + new Vector2(SourceF.Size.X, 0);
            SourcePolygonLowLeft = SourceF.Position + new Vector2(0, SourceF.Size.Y);
            SourcePolygonLowRight = SourceF.Position + SourceF.Size;
        }

        public void FlipX()
        {
            Source.X += Source.Width;
            Source.Width *= -1;

            SourceF.Position.X += SourceF.Width;
            SourceF.Size.X *= -1;


            lib.Switch(ref SourcePolygonTopLeft, ref SourcePolygonTopRight);
            lib.Switch(ref SourcePolygonLowLeft, ref SourcePolygonLowRight);


            //UpdateSourcePolygon(true);
        }

        public Texture2D Texture()
        {
            return Engine.LoadContent.Textures[textureIndex];
        }

        public void SetCustomShaderParameters(ref Effect effect)
        {
            effect.Parameters[Graphics.TextureSourceLib.ColorPos].SetValue(SourceF.Position);
            effect.Parameters[Graphics.TextureSourceLib.ColorSz].SetValue(SourceF.Size);

        }

        public static Sprite FromeName(SpriteName name)
        {
            return DataLib.SpriteCollection.Sprites[(int)name];
        }
    }
}
