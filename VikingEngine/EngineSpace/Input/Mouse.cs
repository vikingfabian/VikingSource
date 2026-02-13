using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Win32;
using VikingEngine.Engine;
using System.Runtime.CompilerServices;

namespace VikingEngine.Input
{
    class MouseInstance
    {
        public Vector2 Position, PrevPosition, MoveDistance;

        bool isMouse;
        bool inUse = false;
        Rectangle2 bounds;
        public IDirectionalMap directionalMap1, directionalMap2;
        public bool centerlockAndHide = false;
        bool hide = false;
        Graphics.Image customMousePointer = null;

        public MouseInstance()
        {
            bounds = Engine.Screen.Area.Rectangle2;
            isMouse = true;
        }

        public MouseInstance(PlayerData playerData, IDirectionalMap directionalMap1 = null, IDirectionalMap directionalMap2 = null)
        { 
            SetPlayer(playerData);
            isMouse = directionalMap1 == null;
            this.directionalMap1 = directionalMap1;
            this.directionalMap2 = directionalMap2;
        }

        public void SetPlayer(PlayerData playerData)
        {
            bounds.Rect = playerData.view.DrawArea;
            inUse = true;
        }


        public void Update()
        {
            if (isMouse)
            {
                Position = Mouse.Position;
                PrevPosition = Mouse.Position;
                MoveDistance = Mouse.MoveDistance;
            }
            else
            { 
                PrevPosition = Position;
                Position += directionalMap1.directionAndTime;
                if (directionalMap1 != null)
                {
                    Position += directionalMap2.directionAndTime;
                }
            }

            if (MainGame.GameIsActive)
            {
                if (centerlockAndHide)
                {
                    SetPosition(Engine.Screen.MonitorCenter);
                }
            }
        }

        public void Draw()
        {
            if (customMousePointer != null && RenderMouseCursor())
            {
                customMousePointer.position = Input.Mouse.Position;
                customMousePointer.Draw(0);
            }
        }

        public void SetPosition(IntVector2 position)
        {
#if PCGAME
            Position = position.Vec;
            if (isMouse)
            {
                Microsoft.Xna.Framework.Input.Mouse.SetPosition(position.X, position.Y);
            }
#endif
        }

        public void RestoreDefault()
        {
            centerlockAndHide = false;
            hide = false;
            RefreshMouseVisible();
        }

        public void Hide()
        {
            hide = true;
            RefreshMouseVisible();
        }

        public void View()
        {
            centerlockAndHide = false;
            hide = false;
            RefreshMouseVisible();
        }
        public void CenterLockAndHide()
        {
            centerlockAndHide = true;
            RefreshMouseVisible();
        }
        public bool RenderMouseCursor()
        {
            return !centerlockAndHide && !hide && (Mouse.MenuMode || inUse);
        }
        public void RefreshMouseVisible()
        {
            bool visible = RenderMouseCursor();

            if (isMouse && !Ref.gamesett.customCursor)
            {
                Ref.main.IsMouseVisible = visible;
            }
            else
            {   
                if (visible && customMousePointer == null) 
                {
                    customMousePointer = new Graphics.Image(SpriteName.cmdPointer, Vector2.Zero, Engine.Screen.IconSizeV2, ImageLayers.AbsoluteTopLayer, true, false);
                }

                if (customMousePointer != null)
                {
                    customMousePointer.Visible = visible;
                }
            }
        }

        //public void refreshCursor()
        //{
        //    customMousePointer = null;

        //    if (Ref.gamesett.customCursor)
        //    {
        //        customMousePointer = new Graphics.Image(SpriteName.cmdPointer, Vector2.Zero, Engine.Screen.IconSizeV2, ImageLayers.AbsoluteTopLayer, true, false);
        //    }
        //    RefreshMouseVisible();// = !Ref.gamesett.customMouse;
        //}
    }

    static class Mouse
    {
        static MouseState previousMouseState;
        static MouseState currentMouseState;

        //static MainGame main;
        static bool swapLeftRightButtons = false;

        static MouseInstance mouse;
        public static List<MouseInstance> Instances = new List<MouseInstance>(4);
        
        /// <summary>
        /// Will display mouse even if unused
        /// </summary>
        public static bool MenuMode = true;



        public static void Reset()
        {
            mouse = new MouseInstance();
            Instances.Clear();
            Instances.Add(mouse);
            Ref.main.IsMouseVisible = true;
        }

        public static void SetMenuMode(bool menu)
        { 
            MenuMode = menu;
            foreach (var ins in Instances)
            {
                ins.RefreshMouseVisible();
            }
        }

        public static void AddPlayer(PlayerData playerData, IDirectionalMap directionalMap1 = null, IDirectionalMap directionalMap2 = null)
        {
            if (playerData.inputMap.inputSource.HasMouseInstance)
            {
                if (playerData.inputMap.inputSource.HasMouse)
                {
                    mouse.SetPlayer(playerData);
                    playerData.inputMap.mouse = mouse;
                }
                else
                {
                    MouseInstance instance = new MouseInstance(playerData, directionalMap1, directionalMap2);
                    Instances.Add(instance);
                    playerData.inputMap.mouse = instance;
                }
            }
        }

//        public static bool LockToScreenArea
//        {
//            set
//            {
//#if PCGAME
//                if (!PlatformSettings.DevBuild)
//                {
//                    if (value)
//                    {
//                        var bounds = Ref.main.Window.ClientBounds;
//                        //System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);// .form.RectangleToScreen(Ref.main.form.ClientRectangle);
//                    }
//                    else
//                    {
//                        //System.Windows.Forms.Cursor.Clip = System.Drawing.Rectangle.Empty;
//                    }
//                }
//#endif
//            }
//        }

        //        public static void Init(MainGame _main)
        //        {
        //            //main = _main;

        ////#if PCGAME
        ////            var key = Registry.CurrentUser.CreateSubKey("Control Panel\\Mouse\\");
        ////            var newValue = key.GetValue("SwapMouseButtons");
        ////            if (newValue != null)
        ////            {
        ////                swapLeftRightButtons = Convert.ToInt32(newValue) != 0;
        ////            }
        ////#endif
        //        }

        //public static bool Visible
        //{
        //    get {
        //        return PlatformSettings.PC_platform && Ref.main.IsMouseVisible && !Ref.gamesett.customMouse;
        //    }
        //    set { 

        //        if (PlatformSettings.PC_platform && (PlatformSettings.Debug_HideMouse || !PlatformSettings.DevBuild))
        //            Ref.main.IsMouseVisible = value; 
        //    }
        //}
        public static void CenterLockAndHideAll()
        {
            foreach (var ins in Instances)
            {
                ins.CenterLockAndHide();
            }
        }
        public static void ViewAll()
        {
            foreach (var ins in Instances)
            {
                ins.View();
            }
        }
        public static void Hide()
        {
            foreach (var ins in Instances)
            {
                ins.Hide();
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
            if (Ref.update.textInput != null)
            {
                currentMouseState = new MouseState();
                previousMouseState = currentMouseState;
                return;
            }

            previousMouseState = currentMouseState;
            currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            PrevPosition = Position;
            PrevRealPosition = RealPosition;

            RealPosition = new Vector2(currentMouseState.X, currentMouseState.Y);
            Position = RealPosition * Engine.Screen.WindowScaleF;

            if (!mouse.centerlockAndHide)//Ref.main.IsMouseVisible)
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
                    MoveDistance = RealMoveDistance * Engine.Screen.WindowScaleF;
                }
                else
                {
                    RealMoveDistance = Vector2.Zero;
                    MoveDistance = Vector2.Zero;
                }
            }

            foreach (MouseInstance ins in Instances)
            {
                ins.Update();
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
            return Engine.Screen.PcDisplayMode != Engine.WindowDisplayMode.Windowed && !Engine.Screen.MousePushEdge.IntersectPoint(Position);
        }


//        public static void SetPosition(IntVector2 position)
//        {
//#if PCGAME
//            Position = position.Vec;
//            Microsoft.Xna.Framework.Input.Mouse.SetPosition(position.X, position.Y);
//#endif
//        }

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
