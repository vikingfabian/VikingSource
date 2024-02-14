using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class FlashingTutorialArrow
    {
        Graphics.Image arrow;

        public FlashingTutorialArrow(Vector2 pointAt, Rotation1D pointDir)
        {
            arrow = new Graphics.Image(SpriteName.toggTutorialArrow, pointAt,
                Engine.Screen.IconSizeV2 * 1.6f, HudLib.TutorialArrowLayer, true);
            arrow.Rotation = pointDir.radians;

            arrow.position += pointDir.Direction(-arrow.Width * 0.6f);
            new Graphics.VisualFlash(arrow, int.MaxValue, 500);
        }


        public void DeleteMe()
        {
            arrow.DeleteMe();
        }
    }
}
