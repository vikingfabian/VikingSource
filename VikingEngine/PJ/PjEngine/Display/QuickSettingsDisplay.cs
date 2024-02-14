using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Display
{
    //class QuickSettingsDisplay
    //{
    //    public QuickSettingsDisplay(VectorRect area)
    //    {
    //        Vector2 winmodeSz = Engine.Screen.IconSizeV2 * 2f;

    //        Vector2 topCenter = new Vector2((area.Width - winmodeSz.X) * 0.5f, Engine.Screen.SafeArea.Y);

    //        HUD.ImageGroupButton winmodeButton = new HUD.ImageGroupButton(new VectorRect(topCenter, winmodeSz), ImageLayers.Lay2, new HUD.ButtonGuiSettings(new Color(38, 74, 113), Engine.Screen.BorderWidth, Color.White, Color.Gray));
    //        VectorRect winIconArea = winmodeButton.area;

    //        winIconArea.AddPercentRadius(-0.1f);
    //        Graphics.Image innerCol = new Graphics.Image(SpriteName.WhiteArea, winIconArea.Position, winIconArea.Size, ImageLayers.Lay1, false, true);
    //        innerCol.Color = new Color(63, 109, 156);
    //        winmodeButton.imagegroup.Add(innerCol);

    //        winIconArea.AddPercentRadius(-0.08f);
    //        Graphics.Image winIcon = new Graphics.Image(SpriteName.MenuIconLargeMonitorArrowsOut, winIconArea.Center, winIconArea.Size, ImageLayers.Lay0, true);
    //        winmodeButton.imagegroup.Add(winIcon);
    //    }
    //}
}
