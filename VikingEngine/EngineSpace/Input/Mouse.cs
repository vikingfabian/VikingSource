using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Win32;

namespace VikingEngine.Input
{
    static class Mouse
    {
        static MouseState previousMouseState;
        static MouseState currentMouseState;

        static MainGame main;
        static bool swapLeftRightButtons = false;
        public static bool LockToScreenArea
        {
            set
            {
#if PCGAME
                if (!PlatformSettings.DevBuild)
                {
                    if (value)
                    {
                        var bounds = Ref.main.Window.ClientBounds;
                        //System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);// .form.RectangleToScreen(Ref.main.form.ClientRectangle);
                    }
                    else
                    {
                        //System.Windows.Forms.Cursor.Clip = System.Drawing.Rectangle.Empty;
                    }
                }
#endif
            }
        }

        public static void Init(MainGame _main)
        {
            main = _main;

//#if PCGAME
//            var key = Registry.CurrentUser.CreateSubKey("Control Panel\\Mouse\\");
//            var newValue = key.GetValue("SwapMouseButtons");
//            if (newValue != null)
//            {
//                swapLeftRightButtons = Convert.ToInt32(newValue) != 0;
//            }
//#endif
        }

        public static bool Visible
        {
            get {
                return PlatformSettings.PC_platform && main.IsMouseVisible;
            }
            set { 

                if (PlatformSettings.PC_platform && (PlatformSettings.Debug_HideMouse || !PlatformSettings.DevBuild))
                    main.IsMouseVisible = value; 
            }
        }

        public static Vector2 MoveDistance;
        public static Vector2 RealMoveDistance;
        
        public static bool bMoveInput
        {
            get
            {
                return RealMoveDistance.X != 0 || RealMoveDistance.Y != 0;//currentMouseState.X != previousMouseState.X || currentMouseState.Y != previousMouseState.Y;
            }
        }

        public static Vector2 Position, PrevPosition, PrevRealPosition, RealPosition;
        static int hiddenFramesCount = 0;

        public static void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            PrevPosition = Position;
            PrevRealPosition = RealPosition;

            RealPosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            Position = RealPosition * Engine.Screen.RenderScaleF;

            if (main.IsMouseVisible)
            {
                hiddenFramesCount = 0;
                RealMoveDistance = RealPosition - PrevRealPosition;
                MoveDistance = Position - PrevPosition;
            }
            else
            {
                if (++hiddenFramesCount > 2)
                {
                    RealMoveDistance = RealPosition - Engine.Screen.MonitorCenter.Vec;
                    MoveDistance = RealMoveDistance * Engine.Screen.RenderScaleF;
                }
                else
                {
                    RealMoveDistance = Vector2.Zero;
                    MoveDistance = Vector2.Zero;
                }
            }

            if (MainGame.GameIsActive)
            {
                if (!main.IsMouseVisible)
                {
                    SetPosition(Engine.Screen.MonitorCenter);
                }
            }
        }

        public static Vector2 EdgePush()
        {
            Vector2 result = Vector2.Zero;
            if (Engine.Screen.Area.IntersectPoint(Position))
            {
                if (Position.X < Engine.Screen.MousePushEdge.X &&
                    Position.X > Engine.Screen.MousePushEdgeMax.X)
                {
                    result.X = -1;
                }
                else if (Position.X > Engine.Screen.MousePushEdge.Right &&
                    Position.X < Engine.Screen.MousePushEdgeMax.Right)
                {
                    result.X = 1;
                }

                if (Position.Y < Engine.Screen.MousePushEdge.Y &&
                    Position.Y > Engine.Screen.MousePushEdgeMax.Y)
                {
                    result.Y = -1;
                }
                else if (Position.Y > Engine.Screen.MousePushEdge.Bottom &&
                    Position.Y < Engine.Screen.MousePushEdgeMax.Bottom)
                {
                    result.Y = 1;
                }
            }
            return result;
        }


        public static bool HasEdgePush()
        {
            return !Engine.Screen.MousePushEdge.IntersectPoint(Position);
        }


        public static void SetPosition(IntVector2 position)
        {
#if PCGAME
             Microsoft.Xna.Framework.Input.Mouse.SetPosition(position.X, position.Y);
#endif
        }

        static bool IsActive
        {
            get { return MainGame.GameIsActive && Engine.Screen.Area.IntersectPoint(Position); }
        }

        public static bool IsButtonDown(MouseButton button)
        {
            if (IsActive)
            {
                switch (button)
                {
                    case MouseButton.Left:
                        if (swapLeftRightButtons)
                            return currentMouseState.RightButton == ButtonState.Pressed;
                        else
                            return currentMouseState.LeftButton == ButtonState.Pressed;
                    case MouseButton.Right:
                        if (swapLeftRightButtons)
                            return currentMouseState.LeftButton == ButtonState.Pressed;
                        else
                            return currentMouseState.RightButton == ButtonState.Pressed;
                    case MouseButton.Middle:
                        return currentMouseState.MiddleButton == ButtonState.Pressed;
                    case MouseButton.X1:
                        return currentMouseState.XButton1 == ButtonState.Pressed;
                    case MouseButton.X2:
                        return currentMouseState.XButton2 == ButtonState.Pressed;
                }
                throw new NotImplementedException();
               
            }
            return false;
        }

        public static bool ButtonDownEvent(MouseButton button)
        {
            if (IsActive)
            {
                switch (button)
                {
                    case MouseButton.Left:
                        if (swapLeftRightButtons)
                        {
                            return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
                        }
                        else
                        {
                            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
                        }
                    case MouseButton.Right:
                        if (swapLeftRightButtons)
                        {
                            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
                        }
                        else
                        {
                            return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
                        }
                    case MouseButton.Middle:
                        return currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released;
                    case MouseButton.X1:
                        return currentMouseState.XButton1 == ButtonState.Pressed && previousMouseState.XButton1 == ButtonState.Released;
                    case MouseButton.X2:
                        return currentMouseState.XButton2 == ButtonState.Pressed && previousMouseState.XButton2 == ButtonState.Released;
                    case MouseButton.DoubleClick:
                        return false;
                }
                throw new NotImplementedException();
            }
            return false;
        }
        public static bool ButtonUpEvent(MouseButton button)
        {
            if (IsActive)
            {
                switch (button)
                {
                    case MouseButton.Left:
                        if (swapLeftRightButtons)
                        {
                            return currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed;
                        }
                        else
                        {
                            return currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
                        }
                    case MouseButton.Right:
                        if (swapLeftRightButtons)
                        {
                            return currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
                        }
                        else
                        {
                            return currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed;
                        }
                    case MouseButton.Middle:
                        return currentMouseState.MiddleButton == ButtonState.Released && previousMouseState.MiddleButton == ButtonState.Pressed;
                    case MouseButton.X1:
                        return currentMouseState.XButton1 == ButtonState.Released && previousMouseState.XButton1 == ButtonState.Pressed;
                    case MouseButton.X2:
                        return currentMouseState.XButton2 == ButtonState.Released && previousMouseState.XButton2 == ButtonState.Pressed;
                }
                throw new NotImplementedException();
            }
            return false;
        }

        public static bool Scroll
        {
            get { return currentMouseState.ScrollWheelValue != previousMouseState.ScrollWheelValue; }
        }

        public static int ScrollValue
        {
            get
            {
                return currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
            }
        }
    }
}
