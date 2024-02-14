using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG
{
    /// <summary>
    /// Create a row with buttons and descriptions at the bottom of the screen
    /// </summary>
    class ButtonsOverview : Graphics.ImageGroup
    {
        VectorRect screenArea;

        public ButtonsOverview(VectorRect screenArea)
        {
            this.screenArea = screenArea;
        }

        public ButtonsOverview(List<HUD.ButtonDescriptionData> buttons, VectorRect screenArea)
        {
            this.screenArea = screenArea;
            Generate(buttons);
        }

        public void Generate(List<HUD.ButtonDescriptionData> buttons)
        {
            ImageLayers layer = HudLib.BgLayer;

            DeleteAll();

            float TextScale = Engine.Screen.TextSize;
            float Height = Engine.Screen.SmallIconSize;

            Vector2 currentPos = new Vector2(screenArea.X, screenArea.Bottom - Height);
            foreach (HUD.ButtonDescriptionData d in buttons)
            {
                TextG text = new TextG(LoadedFont.Regular, Vector2.Zero, VectorExt.V2(TextScale), Align.CenterHeight, d.Title, Color.White, layer);
                Add(text);
                float lenght = text.MeasureText().X + Height;
                if (d.Ctrl != SpriteName.NO_IMAGE)
                    lenght += 40;
                //check if the current row is to long
                if (lenght + currentPos.X > screenArea.Right)
                {
                    //new row
                    currentPos.X = screenArea.X;
                    currentPos.Y -= (Height + 8);
                }
                if (d.Ctrl != SpriteName.NO_IMAGE)
                {
                    Add(new Image(d.Ctrl, currentPos, VectorExt.V2(Height), layer));
                    currentPos.X += Height;
                    Add(new TextG(LoadedFont.Regular, currentPos, VectorExt.V2(TextScale), Align.Zero, "+", Color.White, layer));
                    currentPos.X += 10;
                }
                Add(new Image(d.Button, currentPos, VectorExt.V2(Height), layer));
                currentPos.X += Height;
                text.Position = VectorExt.AddY( currentPos, +Height * 0.5f);
                
                currentPos.X += lenght;
            }
        }

    }
}
