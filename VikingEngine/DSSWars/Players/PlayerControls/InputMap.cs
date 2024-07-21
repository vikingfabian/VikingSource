﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Input;
using Microsoft.Xna.Framework.Input;
using VikingEngine.Graphics;
using VikingEngine.ToGG.HeroQuest.Data;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.PJ.MiniGolf.GO;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars
{
    class InputMap : PlayerInputMap, IRichboxGuiInputMap
    {
        public IDirectionalMap move;
        IDirectionalMap dpadMove;
        public IDirectionalMap cameraTiltZoom;

        public IButtonMap Select;
        public IButtonMap ControllerCancel;
        public IButtonMap ControllerFocus;
        public IButtonMap ControllerMessageClick;
        public IButtonMap Execute;
        public IButtonMap Stop;

        public IButtonMap DragPan;
        public IButtonMap NextArmy;
        public IButtonMap NextCity;
        public IButtonMap NextBattle;
        //public IButtonMap CardMenu;
        public IButtonMap Options;
        public IButtonMap Home;
        public IButtonMap Menu;

        public IButtonMap ToggleHudDetail;
        public IButtonMap GameSpeed;
        public IButtonMap PauseGame;

        public IButtonMap AutomationSetting;
        public IButtonMap FlagDesign_ToggleColor_Prev;
        public IButtonMap FlagDesign_ToggleColor_Next;
        public IButtonMap FlagDesign_PaintBucket;
        public IButtonMap Controller_FlagDesign_Colorpicker;



        public InputMap(int playerIx)
            : base(playerIx)
        {
            DssRef.input = this;

            Engine.XGuide.GetPlayer(playerIx).inputMap = this;
        }

        override public void keyboardSetup()
        {
            move = new AlternativeDirectionalMap(arrowKeys, WASD);

            var camAlts = new Alternative5DirectionalMap();
            camAlts.add(new KeyPlusDirectionalMap(new KeyboardButtonMap(Keys.LeftShift), arrowKeys));
            camAlts.add(new DirectionalButtonsMap(null, null, new KeyboardButtonMap(Keys.Q), new KeyboardButtonMap(Keys.E)));
            camAlts.add(new DirectionalMouseScrollMap());
            cameraTiltZoom = camAlts;

            Select = new MouseButtonMap(MouseButton.Left);
            Execute = new MouseButtonMap(MouseButton.Right);
            ControllerCancel = new MouseButtonMap(MouseButton.Right);
            DragPan = new MouseButtonMap(MouseButton.Middle);
            
            Home = new KeyboardButtonMap(Keys.Home);
            Stop = new KeyboardButtonMap(Keys.H);
            AutomationSetting = new KeyboardButtonMap(Keys.I);

            Menu = new KeyboardButtonMap(Keys.Escape);
            ToggleHudDetail = new KeyboardButtonMap(Keys.U);
            GameSpeed = new KeyboardButtonMap(Keys.Tab);
            PauseGame = new KeyboardButtonMap(Keys.Space);

            NextCity = new KeyboardButtonMap(Keys.D1);
            NextArmy = new KeyboardButtonMap(Keys.D2);
            NextBattle = new KeyboardButtonMap(Keys.D3);

            FlagDesign_ToggleColor_Prev = new TwoCombinedButtonsMap(new KeyboardButtonMap(Keys.LeftShift), new KeyboardButtonMap(Keys.Tab));
            FlagDesign_ToggleColor_Next = new KeyboardButtonMap(Keys.Tab);
            Controller_FlagDesign_Colorpicker = new NoButtonMap();
            FlagDesign_PaintBucket = new KeyboardButtonMap(Keys.LeftAlt);

            menuInput.keyboardSetup();
        }
        public override void xboxSetup()
        {
            move = new DirectionalXboxMap(ThumbStickType.Left, false, inputSource.controllerIndex);
            dpadMove = new DirectionalXboxMap(ThumbStickType.D, false, inputSource.controllerIndex);  
            cameraTiltZoom =new DirectionalXboxMap(ThumbStickType.Right, false, inputSource.controllerIndex);

            Select = new XboxButtonMap_TriggerAlts(Buttons.A, inputSource.controllerIndex);
            ControllerFocus = new XboxButtonMap_TriggerAlts(Buttons.X, inputSource.controllerIndex);
            ControllerCancel = new XboxButtonMap_TriggerAlts(Buttons.B, inputSource.controllerIndex);

            Stop = new XboxButtonMap(Buttons.DPadLeft, inputSource.controllerIndex);
            AutomationSetting = new XboxButtonMap(Buttons.Back, inputSource.controllerIndex);

            DragPan = new XboxButtonMap(Buttons.RightShoulder, inputSource.controllerIndex);
            Home = new XboxButtonMap(Buttons.DPadRight, inputSource.controllerIndex);
            Menu = new XboxButtonMap(Buttons.Start, inputSource.controllerIndex);
            ToggleHudDetail = new XboxButtonMap_TriggerAlts(Buttons.Y, inputSource.controllerIndex);

            GameSpeed = new XboxButtonMap(Buttons.RightShoulder, inputSource.controllerIndex);
            PauseGame = new XboxButtonMap(Buttons.LeftShoulder, inputSource.controllerIndex);

            menuInput.xboxSetup(inputSource.controllerIndex);

            

            NextCity = new XboxButtonMap_TriggerAlts(Buttons.A, inputSource.controllerIndex, true, false);
            NextArmy = new XboxButtonMap_TriggerAlts(Buttons.X, inputSource.controllerIndex, true, false);
            NextBattle = new XboxButtonMap_TriggerAlts(Buttons.Y, inputSource.controllerIndex, true, false);

            ControllerMessageClick = new XboxButtonMap_TriggerAlts(Buttons.A, inputSource.controllerIndex, true, true);

            FlagDesign_ToggleColor_Prev = new XboxButtonMap(Buttons.LeftShoulder, inputSource.controllerIndex);
            FlagDesign_ToggleColor_Next = new XboxButtonMap(Buttons.RightShoulder, inputSource.controllerIndex);
            Controller_FlagDesign_Colorpicker = new XboxButtonMap(Buttons.Y, inputSource.controllerIndex);
            FlagDesign_PaintBucket = new XboxButtonMap(Buttons.X, inputSource.controllerIndex);
        }

        public override void genericControllerSetup()
        {
            xboxSetup();
        }

        public float ZoomValue
        {
            get
            {
                float result = cameraTiltZoom.directionAndTime.Y;
                if (inputSource.HasMouse)
                {
                    result += lib.ToLeftRight(Input.Mouse.ScrollValue) * -10f;
                }

                return result;
            }
        }

        public IButtonMap RichboxGuiSelect => Select;
        public IntVector2 RichboxGuiMove() { return move.stepping + dpadMove.stepping; }
        public bool RichboxGuiUseMove => inputSource.IsController;
    }
}
