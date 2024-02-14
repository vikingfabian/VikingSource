using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Display
{
    class SaveIcon
    {
        const float FadeSpeed = 6f;
        Graphics.Image icon;
        Time visibleTimer;

        public SaveIcon(Players.Player p)
        {
            Vector2 loadingWheelSz = new Vector2(Engine.Screen.IconSize * 2f);
            icon = new Graphics.Image(SpriteName.LFSaveIcon, p.localPData.view.safeScreenArea.RightBottom - loadingWheelSz, loadingWheelSz, ImageLayers.Foreground2, false);
            icon.Opacity = 0f;
            icon.Visible = false;
        }

        public void Update()
        {
            if (visibleTimer.CountDown())
            {//hide
                if (icon.Visible)
                {
                    icon.Opacity -= FadeSpeed * Ref.DeltaTimeSec;
                    if (icon.Opacity <= 0)
                    {
                        icon.Visible = false;
                    }
                }
            }
            else
            {//show
                icon.Opacity += FadeSpeed * Ref.DeltaTimeSec;
            }
        }

        public void OnSave()
        {
            icon.Visible = true;
            visibleTimer.Seconds = 0.6f;
        }

        public void DeleteMe()
        {
            icon.DeleteMe();
        }
    }
}
