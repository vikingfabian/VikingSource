using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.Players;
using VikingEngine.PJ.Match3;

namespace VikingEngine.HUD.RichMenu
{
    class RichTooltip
    {
        RichBoxGroup richBox;
        NineSplitAreaTexture bgTexture;
        RichBoxContent content;

        VectorRect buttonArea, playerScreen;
        RichBoxSettings settings;
        ImageLayers layer;
        public RichTooltip(RichBoxContent content, RichBoxSettings settings, VectorRect buttonArea, VectorRect playerScreen, ImageLayers layer) 
        {
            this.layer = layer;
            this.settings = settings;
            this.content = content;
            this.buttonArea = buttonArea;
            this.playerScreen = playerScreen;
            new Timer.TimedAction0ArgTrigger(view, 120);
        }

        public void DeleteMe()
        {
            if (content != null)
            {
                content = null;
            }
            else
            {
                richBox.DeleteAll();
                bgTexture.DeleteMe();
            }
        }

        public void view()
        {
            if (content != null)
            {
                const float ButtonSpaceing = 10;
                float borderW = settings.windowBackground.BorderWidth();
                float width = Engine.Screen.IconSize * 8;

                bool rightSide = buttonArea.Right + ButtonSpaceing + borderW * 2 + width < playerScreen.Right;
                Vector2 pos = new Vector2(0, buttonArea.Position.Y);
                if (rightSide)
                {
                    pos.X = buttonArea.Right + ButtonSpaceing + borderW;
                }
                else 
                {
                    pos.X = buttonArea.Position.X - ButtonSpaceing - width - borderW * 2;
                }
                richBox = new RichBoxGroup(pos, width, layer - 1, settings, content);

                Vector2 adjust = Vector2.Zero;

                if (!rightSide && richBox.maxArea.Width < width)
                {
                    adjust.X = width - richBox.maxArea.Width;
                }

                if (richBox.area.Bottom > playerScreen.Bottom)
                {
                    adjust.Y -= richBox.area.Bottom - playerScreen.Bottom;
                }

                if (VectorExt.HasValue(adjust))
                {
                    richBox.Move(adjust);
                }

                VectorRect bgArea = richBox.MaxAreaWithPosOffset();
                bgArea.AddRadius(borderW);
                bgTexture = new NineSplitAreaTexture(settings.windowBackground, bgArea, layer + 1);

                //Debug.Log("Mouse pos " + Input.Mouse.Position.ToString());

            }
            content = null;
        }
    }
}
