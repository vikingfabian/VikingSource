using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ
{
    class DlcButton2 : HUD.ImageButton
    {
        Graphics.TextG info;
        Graphics.Image infoBG;

        public DlcButton2(SpriteName image, VectorRect area, string toolTip)
            :base(image, area, HudLib.LayButtons, new HUD.ButtonGuiSettings(Color.White, 2f, Color.Black, Color.Black))
        {
            info = new Graphics.TextG(LoadedFont.Regular, area.CenterTop, Engine.Screen.TextBreadScale, Graphics.Align.CenterAll,
                toolTip, Color.White, ImageLayers.Background3);

            Vector2 infoSz = info.MeasureText();

            info.Ypos -= infoSz.Y;

            infoBG = new Graphics.Image(SpriteName.WhiteArea, info.Position, infoSz * 1.2f, ImageLayers.Background4, true);
            infoBG.Color = Color.Black;
            

            info.Visible = false;
            infoBG.Visible = false;
        }

        protected override void onMouseEnter(bool enter)
        {
            base.onMouseEnter(enter);

            info.Visible = enter;
            infoBG.Visible = enter;
        }
    }

    class DlcButton
    {
        Graphics.Image button, highlight, buyIcon;
        Graphics.TextG info;
        Graphics.Image infoBG;

        VectorRect area;
        bool owned;
        int dlcIndex;

        public DlcButton(float xpos, SpriteName trueTile, SpriteName falseTile, bool owned, string infotext, int dlcIndex)
        {
            this.dlcIndex = dlcIndex;
            this.owned = owned;
            Vector2 size = new Vector2(2, 3) * 0.9f * Engine.Screen.IconSize;
            button = new Graphics.Image(owned ? trueTile : falseTile,
                new Vector2(xpos, Engine.Screen.SafeArea.Bottom - size.Y), size, ImageLayers.Background2);
            highlight = new Graphics.Image(SpriteName.WhiteArea, button.Position, button.Size, ImageLayers.Background3);
            highlight.Visible = false;

            area = button.Area;

            info = new Graphics.TextG(LoadedFont.Regular, button.CenterTop, new Vector2(Engine.Screen.TextSize * 0.6f), Graphics.Align.CenterAll,
                infotext, Color.White, ImageLayers.Background3);
            info.Ypos -= Engine.Screen.IconSize * 0.6f;

            infoBG = new Graphics.Image(SpriteName.WhiteArea, info.Position, info.MeasureText() * 1.2f, ImageLayers.Background4, true);
            infoBG.Color = Color.Black;


            buyIcon = new Graphics.Image(SpriteName.birdShopButton, button.BottomRight, new Vector2(area.Width * 0.7f), ImageLayers.Background2);
            buyIcon.LayerAbove(button);
            buyIcon.Position -= buyIcon.Size * 1.1f;

            info.Visible = false;
            infoBG.Visible = false;
            buyIcon.Visible = false;

            if (!owned)
            {
                button.Visible = false;
            }
        }

        public void update()
        {
            if (owned)
            {
                if (area.IntersectPoint(Input.Mouse.Position))
                {
                    if (!owned)
                    {
                        highlight.Visible = true;
                        buyIcon.Visible = true;

                        if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                        {
                            PjLib.TryStartDlcPurchase(dlcIndex);
                            //if (Ref.steam != null && Ref.steam.DLC != null)
                            //{
                            //    Ref.steam.DLC.OpenDlcStore(dlcIndex);
                            //}
                        }
                    }
                    info.Visible = true;
                    infoBG.Visible = true;
                }
                else
                {
                    highlight.Visible = false;
                    info.Visible = false;
                    infoBG.Visible = false;
                    buyIcon.Visible = false;
                }
            }
        }


        public float Right()
        {
            return button.Right;
        }
    }
}
