using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class NextPhaseWarningPopup2 : AbsPopupWindow
    {
        Time viewTime = new Time(2f, TimeUnit.Seconds);

        public NextPhaseWarningPopup2()
        {
            RichBoxGroup descRB = new HUD.RichBox.RichBoxGroup(
                Vector2.Zero, HudLib.ToolTipWidth,
                HudLib.TooltipLayer,
                HudLib.PopupRichBoxSett, 
                new List<AbsRichBoxMember>
                {
                    //new RichBoxImage(SpriteName.cmdWarningTriangle),
                    new RbText("You still got more available actions")
                });
            images.Add(descRB);

            VectorRect area = descRB.area;
            area.Position = Engine.Screen.CenterScreen - area.Size * 0.5f;

            descRB.Move(VectorExt.AddY(area.Position, titleHeight()));
            area.Height += Engine.Screen.BorderWidth + titleHeight();


            var sett = HudLib.PopupRichBoxSett;
            sett.titleIconHeight = MathExt.Round(sett.titleIconHeight * 1.4f);

            RichBoxGroup pressAgainRB = new HUD.RichBox.RichBoxGroup(
                Vector2.Zero, HudLib.ToolTipWidth,
                HudLib.TooltipLayer,
                sett,
                new List<AbsRichBoxMember>
                {
                    new RbBeginTitle(),
                    
                    new RbText("Press ", Color.Black),
                    new RbImage(toggRef.inputmap.nextPhase.Icon),
                    new RbText(" again", Color.Black),
                });
            images.Add(pressAgainRB);
            //pressAgainRB.maxWidth

            Vector2 pos = area.CenterBottom;
            pos.X -= pressAgainRB.maxWidth * 0.5f;
            pressAgainRB.Move(pos);

            area.Height += pressAgainRB.area.Height;

            area.AddRadius(HudLib.TooltipBorderEdgeSize);

            completeWindow(area, SpriteName.cmdWarningTriangle, "End turn?", 
                HudLib.TooltipBgLayer, false);
        }

        public bool Update()
        {
            return viewTime.CountDown();
        }
    }
}
