using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.BlockMap
{
    class MapDebugOverview : AbsUpdateable
    {
        Graphics.PixelImage img;
        Graphics.TextG exitText;

        public MapDebugOverview(AbsLevel level)
            : base(true)
        {
            img = new Graphics.PixelImage(
                Vector2.Zero, level.squares.Size.Vec * 2f, ImageLayers.Top3, false,
                level.squares.Size, true);

            level.squares.LoopBegin();
            while (level.squares.LoopNext())
            {
                img.pixelTexture.SetPixel(level.squares.LoopPosition, level.squares.LoopValueGet().debugColor());
            }

            img.pixelTexture.ApplyPixelsToTexture();

            exitText = new Graphics.TextG(LoadedFont.Regular, img.LeftBottom,
                new Vector2(Engine.Screen.TextSize * 2), Graphics.Align.Zero,
                "ESC: Close", Color.Yellow, ImageLayers.Top2);
        }

        public override void Time_Update(float time_ms)
        {
            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                this.DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            img.DeleteMe();
            exitText.DeleteMe();
        }
    }
}
