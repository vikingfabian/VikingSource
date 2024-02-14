using System;
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
        public IButtonMap Execute;

        public IButtonMap Stop;


        public IButtonMap DragPan;
        public IButtonMap NextArmy;
        public IButtonMap PrevBattle;
        public IButtonMap CardMenu;
        public IButtonMap Options;
        public IButtonMap Home;
        public IButtonMap Menu;

        public IButtonMap ToggleHudDetail;
        public IButtonMap GameSpeed;
        public IButtonMap PauseGame;
       
     
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
            //NextArmy = new KeyboardButtonMap(Keys.D1);
            //PrevBattle = new KeyboardButtonMap(Keys.D2);
            //CardMenu = new KeyboardButtonMap(Keys.Tab);
            //Options = new AlternativeButtonsMap(new KeyboardButtonMap(Keys.LeftControl), new KeyboardButtonMap(Keys.RightControl));
            Home = new KeyboardButtonMap(Keys.Home);

            Stop = new KeyboardButtonMap(Keys.H);

            Menu = new KeyboardButtonMap(Keys.Escape);
            ToggleHudDetail = new KeyboardButtonMap(Keys.U);
            GameSpeed = new KeyboardButtonMap(Keys.Tab);
            PauseGame = new KeyboardButtonMap(Keys.Space);
            menuInput.keyboardSetup();
        }
        public override void xboxSetup()
        {
            //throw new NotImplementedException();
            //source = InputSourceType.XController;
            move = new DirectionalXboxMap(ThumbStickType.Left, false, inputSource.controllerIndex);
            dpadMove = new DirectionalXboxMap(ThumbStickType.D, false, inputSource.controllerIndex);  
            cameraTiltZoom =new DirectionalXboxMap(ThumbStickType.Right, false, inputSource.controllerIndex);

            Select = new XboxButtonMap(Buttons.A, inputSource.controllerIndex);
            ControllerFocus = new XboxButtonMap(Buttons.X, inputSource.controllerIndex);
            ControllerCancel = new XboxButtonMap(Buttons.B, inputSource.controllerIndex);

            Stop = new XboxButtonMap(Buttons.DPadLeft, inputSource.controllerIndex);

            DragPan = new XboxButtonMap(Buttons.RightShoulder, inputSource.controllerIndex);
            Home = new XboxButtonMap(Buttons.DPadRight, inputSource.controllerIndex);
            Menu = new XboxButtonMap(Buttons.Start, inputSource.controllerIndex);
            ToggleHudDetail = new XboxButtonMap(Buttons.Y, inputSource.controllerIndex);

            GameSpeed = new XboxButtonMap(Buttons.RightShoulder, inputSource.controllerIndex);
            PauseGame = new XboxButtonMap(Buttons.LeftShoulder, inputSource.controllerIndex);

            menuInput.xboxSetup(inputSource.controllerIndex);

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
