using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Display
{
    class LostControllerDisplay
    {
        List<ControllerIcon> controllerIcons;
        bool disconnectFlash = true;
        IntervalTimer disconnectFlashTimer = new IntervalTimer(800);
        public TimeStamp created;
        Graphics.Image bgOverLay = null;

        public LostControllerDisplay()
        {
            Vector2 controllerSz; int viewCount;
            controllerLayout(out controllerSz, out viewCount);
            Vector2 center = Engine.Screen.CenterScreen;
            init(center, controllerSz, viewCount);

            bgOverLay = HudLib.DarkBgOverlay(HudLib.LayPopupBg);
        }

        public LostControllerDisplay(PauseMenu pause)
        {
            pause.hidePauseIcon();
            Vector2 controllerSz; int viewCount;// = new Vector2(Engine.Screen.IconSize * 1.6f);
            controllerLayout(out controllerSz, out viewCount);

            Vector2 center = Engine.Screen.CenterScreen;
            center.Y = pause.buttonsY - controllerSz.Y;

            init(center, controllerSz, viewCount);
        }

        void controllerLayout(out Vector2 controllerSz, out int viewCount)
        {
            int connected = Input.XInput.MaxIndex() + 1;
            if (connected >= 5)
            {
                viewCount = Input.XInput.controllers.Count;
            }
            else if (connected >= 3)
            {
                viewCount = 6;
            }
            else
            {
                viewCount = 4;
            }

            controllerSz = new Vector2(Engine.Screen.IconSize *
                (viewCount <= 4? 1.6f : 2f));
        }

        void init(Vector2 center, Vector2 controllerSz, int viewCount)
        {
            created = TimeStamp.Now();
            controllerIcons = new List<ControllerIcon>(viewCount);

            for (int i = 0; i < viewCount; ++i)
            {
                var area = Table.CellPlacement(center, true, i, viewCount, controllerSz,
                    new Vector2(Engine.Screen.BorderWidth));
                controllerIcons.Add(new ControllerIcon(area, Input.XInput.controllers[i]));
            }

            update();
        }

        public bool update()
        {
            bool hasDisconnectedUser = false;

            if (disconnectFlashTimer.CountDown())
            {
                lib.Invert(ref disconnectFlash);
            }

            foreach (var m in controllerIcons)
            {
                m.refresh(ref hasDisconnectedUser, disconnectFlash);
            }

            return hasDisconnectedUser == false;
        }

        public void DelteMe()
        {
            bgOverLay?.DeleteMe();

            foreach (var m in controllerIcons)
            {
                m.DeleteMe();
            }
        }

        class ControllerIcon
        {
            Graphics.Image image, disconnect;
            Input.XController controller;

            public ControllerIcon(VectorRect area, Input.XController controller)
            {
                this.controller = controller;
                image = new Graphics.Image(SpriteName.NO_IMAGE, area.Position, area.Size, HudLib.LayPopup);
                image.SetSpriteName(SpriteName.PixController1, controller.Index);

                disconnect = new Graphics.Image(SpriteName.DisconnectSquare, area.Position, area.Size, ImageLayers.AbsoluteBottomLayer);
                disconnect.LayerAbove(image);
                
            }

            public void refresh(ref bool hasDisconnectedUser, bool disconnectFlash)
            {
                image.Opacity = 1f;
                disconnect.Visible = false;

                if (controller.hasUser)
                {
                    image.Color = Color.White;
                    if (controller.Connected)
                    {
                        //image.Color = Color.White;
                    }
                    else
                    {
                        disconnect.Visible = disconnectFlash;
                        //image.Color = Color.Red;
                        hasDisconnectedUser = true;
                    }
                }
                else
                {
                    if (controller.Connected)
                    {
                        image.Color = ColorExt.VeryDarkGray;
                    }
                    else
                    {
                        image.Color = Color.Black;
                        image.Opacity = 0.1f;
                    }
                }
            }

            public void DeleteMe()
            {
                image.DeleteMe();
                disconnect.DeleteMe();
            }
        }
    }
}
