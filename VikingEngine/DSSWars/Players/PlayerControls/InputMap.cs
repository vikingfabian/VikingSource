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
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.DataStream;

namespace VikingEngine.DSSWars
{
    class InputMap : PlayerInputMap, IRichboxGuiInputMap
    {
        IDirectionalMap wasd;
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
        //public IButtonMap Home;
        public IButtonMap Menu;

        public IButtonMap ToggleHudDetail;
        public IButtonMap GameSpeed;
        public IButtonMap PauseGame;

        public IButtonMap AutomationSetting;
        public IButtonMap FlagDesign_ToggleColor_Prev;
        public IButtonMap FlagDesign_ToggleColor_Next;
        public IButtonMap FlagDesign_PaintBucket;
        public IButtonMap Controller_FlagDesign_Colorpicker;

        public InputMap(bool keyboard)
        {
            if (keyboard)
            {
                keyboardSetup();
            }
            else
            {
                xboxSetup();
            }
        }

        public InputMap(int playerIx)
            : base(playerIx)
        {
            Engine.XGuide.GetPlayer(playerIx).inputMap = this;
        }

        public void copyDataFrom(InputMap other)
        {
            MemoryStreamHandler memoryStream = new MemoryStreamHandler();
            var w = memoryStream.GetWriter();
            other.write(w);

            var r = memoryStream.GetReader();
            this.read(r);
        }

        override public void keyboardSetup()
        {
            //ControllerCancel = new NoButtonMap();
            ControllerFocus = new NoButtonMap();
            ControllerMessageClick = new NoButtonMap();

            wasd = WASD;
            move = new AlternativeDirectionalMap(arrowKeys, wasd);

            var camAlts = new Alternative5DirectionalMap();
            camAlts.add(new KeyPlusDirectionalMap(new KeyboardButtonMap(Keys.LeftShift), arrowKeys));
            camAlts.add(new DirectionalButtonsMap(null, null, new KeyboardButtonMap(Keys.Q), new KeyboardButtonMap(Keys.E)));
            camAlts.add(new DirectionalMouseScrollMap());
            cameraTiltZoom = camAlts;

            Select = new MouseButtonMap(MouseButton.Left);
            Execute = new MouseButtonMap(MouseButton.Right);
            ControllerCancel = new MouseButtonMap(MouseButton.Right);
            DragPan = new MouseButtonMap(MouseButton.Middle);
            
            //Home = new KeyboardButtonMap(Keys.Home);
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

            menuInput?.keyboardSetup();
        }
        public override void xboxSetup()
        {
            //wasd = new DirectionalButtonsMap(null, null, null, null);
            move = new DirectionalXboxMap(ThumbStickType.Left, false, inputSource.controllerIndex);
            dpadMove = new DirectionalXboxMap(ThumbStickType.D, false, inputSource.controllerIndex);  
            cameraTiltZoom =new DirectionalXboxMap(ThumbStickType.Right, false, inputSource.controllerIndex);

            Select = new XboxButtonMap_TriggerAlts(Buttons.A, inputSource.controllerIndex);
            ControllerFocus = new XboxButtonMap_TriggerAlts(Buttons.X, inputSource.controllerIndex);
            ControllerCancel = new XboxButtonMap_TriggerAlts(Buttons.B, inputSource.controllerIndex);

            Stop = new XboxButtonMap(Buttons.DPadLeft, inputSource.controllerIndex);
            AutomationSetting = new XboxButtonMap(Buttons.Back, inputSource.controllerIndex);

            DragPan = new NoButtonMap();//new XboxButtonMap(Buttons.RightShoulder, inputSource.controllerIndex);
            //Home = new XboxButtonMap(Buttons.DPadRight, inputSource.controllerIndex);
            Menu = new XboxButtonMap(Buttons.Start, inputSource.controllerIndex);
            ToggleHudDetail = new XboxButtonMap_TriggerAlts(Buttons.Y, inputSource.controllerIndex);

            GameSpeed = new XboxButtonMap(Buttons.RightShoulder, inputSource.controllerIndex);
            PauseGame = new XboxButtonMap(Buttons.LeftShoulder, inputSource.controllerIndex);

            menuInput?.xboxSetup(inputSource.controllerIndex);

            NextCity = new XboxButtonMap_TriggerAlts(Buttons.A, inputSource.controllerIndex, true, false);
            NextArmy = new XboxButtonMap_TriggerAlts(Buttons.X, inputSource.controllerIndex, true, false);
            NextBattle = new XboxButtonMap_TriggerAlts(Buttons.Y, inputSource.controllerIndex, true, false);

            ControllerMessageClick = new XboxButtonMap_TriggerAlts(Buttons.A, inputSource.controllerIndex, true, true);

            FlagDesign_ToggleColor_Prev = new XboxButtonMap(Buttons.LeftShoulder, inputSource.controllerIndex);
            FlagDesign_ToggleColor_Next = new XboxButtonMap(Buttons.RightShoulder, inputSource.controllerIndex);
            Controller_FlagDesign_Colorpicker = new XboxButtonMap(Buttons.Y, inputSource.controllerIndex);
            FlagDesign_PaintBucket = new XboxButtonMap(Buttons.X, inputSource.controllerIndex);
        }

        public void write(System.IO.BinaryWriter w)
        {
            const int InputVersion = 1;
            w.Write(InputVersion);
            //inputSource.write(w);

            ControllerFocus.write(w);
            ControllerCancel.write(w);
            Stop.write(w);
            AutomationSetting.write(w);
            //Home.write(w);
            ToggleHudDetail.write(w);
            GameSpeed.write(w);
            PauseGame.write(w);
            NextCity.write(w);
            NextArmy.write(w);
            NextBattle.write(w);
            ControllerMessageClick.write(w);

            if (inputSource.HasKeyBoard)
            {
                wasd.write(w);
            }
        }
        public void read(System.IO.BinaryReader r)
        {
            int inputVersion = r.ReadInt32();
             //inputSource.read(r);

            ControllerFocus = MapRead.Button(r, inputSource.controllerIndex);
            ControllerCancel = MapRead.Button(r, inputSource.controllerIndex);
            Stop = MapRead.Button(r, inputSource.controllerIndex);
            AutomationSetting = MapRead.Button(r, inputSource.controllerIndex);
            //Home = MapRead.Button(r, inputSource.controllerIndex);
            ToggleHudDetail = MapRead.Button(r, inputSource.controllerIndex);
            GameSpeed = MapRead.Button(r, inputSource.controllerIndex);
            PauseGame = MapRead.Button(r, inputSource.controllerIndex);
            NextCity = MapRead.Button(r, inputSource.controllerIndex);
            NextArmy = MapRead.Button(r, inputSource.controllerIndex);
            NextBattle = MapRead.Button(r, inputSource.controllerIndex);
            ControllerMessageClick = MapRead.Button(r, inputSource.controllerIndex);

            if (inputSource.HasKeyBoard)
            {
                wasd = MapRead.Directional(r);
            }
        }

        public List<InputButtonType> listInputs(bool keyboard)
        {
            List<InputButtonType> result = new List<InputButtonType>
            {
                InputButtonType.Stop,
                InputButtonType.GameSpeed,
                InputButtonType.PauseGame,
                InputButtonType.AutomationSetting,
                //InputButtonType.Home,
                InputButtonType.NextCity,
                InputButtonType.NextArmy,
                InputButtonType.NextBattle,
                InputButtonType.ToggleHudDetail,
            };

            if (keyboard)
            {
                //result.AddRange(new List<InputButtonType>
                //{
                //    InputButtonType.Stop,
                //    InputButtonType.GameSpeed,
                //    InputButtonType.PauseGame,
                //    InputButtonType.AutomationSetting,
                //    InputButtonType.Home,
                //    InputButtonType.NextCity,
                //    InputButtonType.NextArmy,
                //    InputButtonType.NextBattle,
                //    InputButtonType.ToggleHudDetail,
                //});
            }
            else
            {
                result.AddRange(new List<InputButtonType>
                {
                    InputButtonType.ControllerFocus,
                    InputButtonType.ControllerCancel,
                    InputButtonType.ControllerMessageClick,
                });
            }

            return result;
        }

        public void getset(InputButtonType type, ref IButtonMap buttonMap, bool set)
        {
            switch (type)
            {
                case InputButtonType.Stop:
                    if (set)
                    {
                        Stop = buttonMap;
                    }
                    else
                    {
                        buttonMap = Stop;
                    }
                    break;

                case InputButtonType.AutomationSetting:
                    if (set)
                    {
                        AutomationSetting = buttonMap;
                    }
                    else
                    {
                        buttonMap = AutomationSetting;
                    }
                    break;

                //case InputButtonType.Home:
                //    if (set)
                //    {
                //        Home = buttonMap;
                //    }
                //    else
                //    {
                //        buttonMap = Home;
                //    }
                //    break;

                case InputButtonType.ToggleHudDetail:
                    if (set)
                    {
                        ToggleHudDetail = buttonMap;
                    }
                    else
                    {
                        buttonMap = ToggleHudDetail;
                    }
                    break;

                case InputButtonType.GameSpeed:
                    if (set)
                    {
                        GameSpeed = buttonMap;
                    }
                    else
                    {
                        buttonMap = GameSpeed;
                    }
                    break;

                case InputButtonType.PauseGame:
                    if (set)
                    {
                        PauseGame = buttonMap;
                    }
                    else
                    {
                        buttonMap = PauseGame;
                    }
                    break;

                case InputButtonType.NextCity:
                    if (set)
                    {
                        NextCity = buttonMap;
                    }
                    else
                    {
                        buttonMap = NextCity;
                    }
                    break;

                case InputButtonType.NextArmy:
                    if (set)
                    {
                        NextArmy = buttonMap;
                    }
                    else
                    {
                        buttonMap = NextArmy;
                    }
                    break;

                case InputButtonType.NextBattle:
                    if (set)
                    {
                        NextBattle = buttonMap;
                    }
                    else
                    {
                        buttonMap = NextBattle;
                    }
                    break;

                case InputButtonType.ControllerFocus:
                    if (set)
                    {
                        ControllerFocus = buttonMap;
                    }
                    else
                    {
                        buttonMap = ControllerFocus;
                    }
                    break;

                case InputButtonType.ControllerCancel:
                    if (set)
                    {
                        ControllerCancel = buttonMap;
                    }
                    else
                    {
                        buttonMap = ControllerCancel;
                    }
                    break;

                case InputButtonType.ControllerMessageClick:
                    if (set)
                    {
                        ControllerMessageClick = buttonMap;
                    }
                    else
                    {
                        buttonMap = ControllerMessageClick;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public string Name(InputButtonType type)
        {
            switch (type)
            {

            case InputButtonType.Stop:
                return DssRef.lang.ArmyOption_Halt;
            case InputButtonType.AutomationSetting:
                return DssRef.lang.Automation_Title;
            //case InputButtonType.Home:
            //    return;
            case InputButtonType.ToggleHudDetail:
                return DssRef.lang.Input_ToggleHudDetail;
            case InputButtonType.GameSpeed:
                return  DssRef.lang.Input_GameSpeed;
            case InputButtonType.PauseGame:
                return DssRef.lang.Input_Pause;
            case InputButtonType.NextCity:
                return DssRef.lang.Input_NextCity;
            case InputButtonType.NextArmy:
                return DssRef.lang.Input_NextArmy;
            case InputButtonType.NextBattle:
                return DssRef.lang.Input_NextBattle;

            case InputButtonType.ControllerFocus:
                return DssRef.lang.Input_ToggleHudFocus;
            case InputButtonType.ControllerCancel:
                return Ref.langOpt.Hud_Cancel;
            case InputButtonType.ControllerMessageClick:
                return DssRef.lang.Input_ClickMessage;

                case InputButtonType.WASD_UP:
                    return DssRef.lang.Input_Up;
                case InputButtonType.WASD_DOWN:
                    return DssRef.lang.Input_Down;
                case InputButtonType.WASD_LEFT:
                    return DssRef.lang.Input_Left;
                case InputButtonType.WASD_RIGHT:
                    return DssRef.lang.Input_Right;

                default:
                    return "ERR";

            }
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

    enum InputButtonType
    {
        Stop,
        AutomationSetting,
        //Home,
        ToggleHudDetail,
        GameSpeed,
        PauseGame,
        NextCity,
        NextArmy,
        NextBattle,

        ControllerFocus,
        ControllerCancel,
        ControllerMessageClick,

        WASD_UP,
        WASD_DOWN,
        WASD_LEFT,
        WASD_RIGHT,

        NUM,
    }
}
