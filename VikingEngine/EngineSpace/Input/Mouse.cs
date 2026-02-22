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
    static class Mouse
    {
        static MouseState previousMouseState;
        static MouseState currentMouseState;

        //static MainGame main;
        static bool swapLeftRightButtons = false;

        static MouseInstance mouse;
        public static List<MouseInstance> Instances = new List<MouseInstance>(4);
        
        /// <summary>
        /// Will display mouse even if unused, and everyone shares one pointer
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
            SetMenuMode(menu? SteamWrapping.SteamActionSet.MenuControls : SteamWrapping.SteamActionSet.InGameControls);
        }

        public static void SetMenuMode(SteamWrapping.SteamActionSet actionSet)
        { 
            MenuMode = actionSet == SteamWrapping.SteamActionSet.MenuControls;
            foreach (var ins in Instances)
            {
                ins.RefreshMouseVisible();
            }

            Ref.steam.input?.SetActionSet(actionSet);
            if (Input.Keyboard.Ctrl)
            {
                lib.DoNothing();
            }
        }

        public static void AddPlayer(PlayerData playerData, int playerCount, IDirectionalMap directionalMap1 = null, IDirectionalMap directionalMap2 = null)
        {
            //if (playerData.inputMap.inputSource.HasMouseInstance)
            //{
                if (playerData.inputMap.inputSource.HasMouse)
                {
                    mouse.SetPlayer(playerData, playerCount);
                    playerData.inputMap.mouse = mouse;
                }
                else
                {
                    MouseInstance instance = new MouseInstance(playerData, playerCount, directionalMap1, directionalMap2);
                    Instances.Add(instance);
                    playerData.inputMap.SetMouse(instance);
                }
            //}
        }

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

        static bool IsActive
        {
            get { return MainGame.GameIsActive && mouse.inBounds; }
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
