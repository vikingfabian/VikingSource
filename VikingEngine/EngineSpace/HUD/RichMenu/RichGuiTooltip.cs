using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.HUD.RichMenu
{
    abstract class RichGuiTooltip
    {
        protected Graphics.ImageGroup images = new Graphics.ImageGroup(128);

        public bool refresh = false;
        protected Vector2 size;
        public static int tooltip_id = int.MinValue;
        public static float tooltip_id_timestampsec;

        protected bool current_menuToolTip;

        protected void basecreate(List<AbsRichBoxMember> content, ImageLayers layer, bool menuToolTip, int tooltip_id = -1)
        {
            images.DeleteAll();

            if (content.Count > 0)
            {

                current_menuToolTip = menuToolTip;

                float edge = 8;
                float width = Engine.Screen.IconSize * 8;

                RichBoxGroup richBox = new RichBoxGroup(new Vector2(edge),
                    width, layer/*HudLib.MapToolTipLayer*/, HudLib.RbSettings, content);

                var area = richBox.maxArea;
                area.AddRadius(edge);
                //var backgroundTextures = new NineSplitAreaTexture(HudLib.TooltipSettings.windowBackground, area, HudLib.MapToolTipLayer + 2);
                //images.Add(backgroundTextures.images);
                createBackground(area, layer + 2);
                size = area.Size;

                images.Add(richBox);

                //baseUpdate(player, menuToolTip, false);

            }
        }

        protected void baseUpdate(Vector2 mousePos/*Players.LocalPlayer player*/, bool menuToolTip, bool aboveMouse)
        {
            if (images.HasMembers)
            {

                Vector2 offset = mousePos;
                offset.X += Engine.Screen.IconSize;
                if (aboveMouse)
                {
                    offset.Y -= 5 + size.Y;
                }
                else
                {
                    offset.Y += Engine.Screen.SmallIconSize;
                }


                Vector2 maxPos = offset + size;

                if (maxPos.X > Engine.Screen.SafeArea.Right)
                {
                    offset.X = mousePos.X - (Engine.Screen.IconSize + size.X);
                }

                if (offset.Y < Engine.Screen.SafeArea.Y)
                {
                    offset.Y = Engine.Screen.SafeArea.Y;
                }
                else if (maxPos.Y > Engine.Screen.SafeArea.Bottom)
                {
                    offset.Y = Engine.Screen.SafeArea.Bottom - size.Y;
                }


                images.SetOffset(offset);

            }
        }

        abstract protected void createBackground(VectorRect area, ImageLayers layer);

        public void clear()
        {
            images.DeleteAll();
        }
    }
}
