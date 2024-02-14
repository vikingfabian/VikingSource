using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class PopupDialogue : AbsPopupWindow
    {
        ToGG.HeroQuest.Display.Button button;
        Time inputDelay = new Time(1f, TimeUnit.Seconds);

        public PopupDialogue(SpriteName titleIcon, string titleText, List<AbsRichBoxMember> content)
        {
            RichBoxGroup richBox = new HUD.RichBox.RichBoxGroup(
                Vector2.Zero, HudLib.ToolTipWidth, 
                HudLib.TooltipLayer,
                HudLib.PopupRichBoxSett, content);
            images.Add(richBox);

            VectorRect area = richBox.maxArea;
            area.Position = Engine.Screen.CenterScreen - area.Size * 0.5f;
            
            richBox.Move(VectorExt.AddY(area.Position, titleHeight()));

            area.Height += Engine.Screen.BorderWidth + titleHeight();

            Vector2 buttonSz = new Vector2(area.Width * 0.5f, Engine.Screen.IconSize);
            VectorRect buttonAr = new VectorRect(
                area.Center.X - buttonSz.X * 0.5f,
                area.Bottom,
                buttonSz.X, buttonSz.Y);

            button = new HeroQuest.Display.Button(buttonAr, HudLib.TooltipLayer, 
                HeroQuest.Display.ButtonTextureStyle.Popup);
            
            button.addCenterText("OK", Color.Black, 0.8f);

            button.Visible = false;

            area.Height += button.area.Height + Engine.Screen.BorderWidth;
            area.AddRadius(HudLib.TooltipBorderEdgeSize);

            completeWindow(area, titleIcon, titleText, HudLib.TooltipBgLayer, false);
        }

        public bool update()
        {
            if (button.Visible)
            {
                if (button.update() || 
                    toggRef.inputmap.click.DownEvent || 
                    toggRef.inputmap.menuInput.openCloseInputEvent())
                {
                    return true;
                }
            }
            else if (inputDelay.CountDown())
            {
                button.Visible = true;
            }

            return false;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            button.DeleteMe();
        }
    }
}
