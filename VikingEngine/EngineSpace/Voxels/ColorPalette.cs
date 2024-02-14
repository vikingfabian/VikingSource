using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Voxels
{
    class CreateTexState : Engine.GameState
    {
        public CreateTexState()
            : base()
        {
            ColorPalette palette = new ColorPalette();
            palette.CreateTexture();
            draw.ClrColor = Color.Green;
        }
    }

    class ColorPalette
    {
        const int TexSize = 2048;

        const int StepsR = 2;
        const int StepsG = 2;
        const int StepsB = 1;

        public void CreateTexture()
        {
            Graphics.PixelTexture tex = new Graphics.PixelTexture(new IntVector2(TexSize));

            IntVector2 texGrindex = IntVector2.Zero;

            for (int g = 0; g <= byte.MaxValue; g += StepsG)
            {

                for (int b = 0; b <= byte.MaxValue; b += StepsB)
                {
                    for (int r = 0; r <= byte.MaxValue; r += StepsR)
                    {
                        tex.SetPixel(texGrindex, new Color(r, g, b));
                        ++texGrindex.X;
                        if (texGrindex.X >= TexSize)
                        {
                            texGrindex.X = 0;
                            ++texGrindex.Y;
                        }
                    }
                }
            }

            tex.ApplyPixelsToTexture();

            DataStream.FilePath path = new DataStream.FilePath(null, "colorpalette", "", true);
            string save = path.CompletePath(true);
            tex.SaveAsPNG(save);

            Graphics.Text2 resultText = new Graphics.Text2(save, LoadedFont.Console,
                Engine.Screen.CenterScreen, Engine.Screen.SmallIconSize, Color.Black, ImageLayers.Lay0);
            resultText.OrigoAtCenter();
        }
        
    }
}
