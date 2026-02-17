using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using VikingEngine.DataStream;
using VikingEngine.HUD.RichBox;
using VikingEngine.Input;
using VikingEngine.SteamWrapping;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
    class InputMap : PlayerInputMap, IRichboxGuiInputMap
    {
        IButtonMap wasd_up, wasd_down, wasd_left, wasd_right;
        IButtonMap cameraTiltLeft, cameraTiltRight;
        public IButtonMap cameraTiltUp;
        public IDirectionalMap move; //Do not save
        public IDirectionalMap moveCursor;
        IDirectionalMap dpadMove; //Do not save
        public IDirectionalMap cameraStick; //Do not save
        public IDirectionalMap cameraTiltUpSmooth;

        public IButtonMap zoomInKey, zoomOutKey;

        //public IButtonMap ControllerSelect;
        public IButtonMap CancelKey;
        public IButtonMap QuickSelect;
        public IButtonMap Controller_ObjectMenuToggle;
        public IButtonMap Controller_Faction;
        public IButtonMap ControllerMessageClick;
        //public IButtonMap Execute;
        public IButtonMap StopStart;
        public IButtonMap Copy;
        public IButtonMap Paste;
        public IButtonMap Build;
        public IButtonMap PinAndPing = new KeyboardButtonMap(Keys.P);

        public IButtonMap mousePan; //Do not save
        public IButtonMap mouseSelect; //Do not save
        public IButtonMap mouseOrder; //Do not save
        public IButtonMap mouseCancel; //Do not save
        public bool hasPanOrderMix; //Do not save

        MouseButtonAction leftMouseAction = MouseButtonAction.Select;
        MouseButtonAction rightMouseAction = MouseButtonAction.PanAndOrder;
        MouseButtonAction middleMouseAction = MouseButtonAction.Pan;

        MouseButtonAction X1MouseAction = MouseButtonAction.None;
        MouseButtonAction X2MouseAction = MouseButtonAction.None;


        public IButtonMap NextArmy;
        public IButtonMap NextCity;
        public IButtonMap NextWar;
        public IButtonMap Options;
        public IButtonMap Menu;

        public IButtonMap ToggleHudDetail;
        public IButtonMap ToggleMinimap;
        public IButtonMap GameSpeed;
        public IButtonMap PauseGame;

        public IButtonMap FlagDesign_ToggleColor_Prev;
        public IButtonMap FlagDesign_ToggleColor_Next;
        public IButtonMap FlagDesign_PaintBucket;
        public IButtonMap Controller_FlagDesign_Colorpicker;
        public IButtonMap Controller_TabLeft, Controller_TabRight;
        //public IButtonMap Controller_SubTabLeft, Controller_SubTabRight;

        public Voxels.EditorInputMap editorInput = new Voxels.EditorInputMap();
        

        public MouseButtonAction GetMouseAction(MouseButton MouseButton)
        {
            switch (MouseButton)
            {
                case MouseButton.Left:
                    return leftMouseAction;
                case MouseButton.Right:
                    return rightMouseAction;
                case MouseButton.Middle:
                    return middleMouseAction;
                case MouseButton.X1:
                    return X1MouseAction;
                case MouseButton.X2:
                    return X2MouseAction;
            }

            throw new ArgumentOutOfRangeException();
        }

        public void SetMouseAction(MouseButton MouseButton, MouseButtonAction action)
        {
            switch (MouseButton)
            {
                case MouseButton.Left:
                    leftMouseAction = action;
                    break;
                case MouseButton.Right:
                    rightMouseAction = action;
                    break;
                case MouseButton.Middle:
                    middleMouseAction = action;
                    break;
                case MouseButton.X1:
                    X1MouseAction = action;
                    break;
                case MouseButton.X2:
                    X2MouseAction = action;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(MouseButton), MouseButton, null);
            }

            refreshMouseInput();
        }

        public InputMap(bool keyboard)
        {
            menuInput = new HUD.MenuInputMap();
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

        public override IButtonMap MenuClick => mouseSelect;

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
            Controller_ObjectMenuToggle = new NoButtonMap();
            ControllerMessageClick = new NoButtonMap();

            wasd_up = new KeyboardButtonMap(Keys.W);
            wasd_down = new KeyboardButtonMap(Keys.S);
            wasd_left = new KeyboardButtonMap(Keys.A);
            wasd_right = new KeyboardButtonMap(Keys.D);

            cameraTiltLeft = new KeyboardButtonMap(Keys.Q);
            cameraTiltRight = new KeyboardButtonMap(Keys.E);
            cameraTiltUp = new KeyboardButtonMap(Keys.R);
            cameraTiltUpSmooth = null;

            //ControllerSelect = new MouseButtonMap(MouseButton.Left);
            //Execute = new MouseButtonMap(MouseButton.Right);
            CancelKey = new KeyboardButtonMap(Keys.Back);
            QuickSelect = new KeyboardButtonMap(Keys.Enter);

            //DragPan = new MouseButtonMap(MouseButton.Middle);

            //Home = new KeyboardButtonMap(Keys.Home);
            StopStart = new KeyboardButtonMap(Keys.H);
            Copy = new KeyboardButtonMap(Keys.C);
            Paste = new KeyboardButtonMap(Keys.V);
            Build = new KeyboardButtonMap(Keys.B);
            //AutomationSetting = new KeyboardButtonMap(Keys.I);

            Menu = new KeyboardButtonMap(Keys.Escape);
            ToggleHudDetail = new KeyboardButtonMap(Keys.U);
            ToggleMinimap  = new KeyboardButtonMap(Keys.M);
            GameSpeed = new KeyboardButtonMap(Keys.Tab);
            PauseGame = new KeyboardButtonMap(Keys.Space);

            zoomInKey = new KeyboardButtonMap(Keys.PageUp);
            zoomOutKey = new KeyboardButtonMap(Keys.PageDown);


            NextCity = new KeyboardButtonMap(Keys.D1);
            NextArmy = new KeyboardButtonMap(Keys.D2);
            NextWar = new KeyboardButtonMap(Keys.D3);

            FlagDesign_ToggleColor_Prev = new TwoCombinedButtonsMap(new KeyboardButtonMap(Keys.LeftShift), new KeyboardButtonMap(Keys.Tab));
            FlagDesign_ToggleColor_Next = new KeyboardButtonMap(Keys.Tab);
            Controller_FlagDesign_Colorpicker = new NoButtonMap();
            FlagDesign_PaintBucket = new KeyboardButtonMap(Keys.LeftAlt);

            menuInput?.keyboardSetup();
            refreshMouseInput();
            refreshKeyBoardInput();

            menuInput.keyboardSetup();
            editorInput.keyboardSetup();
        }
        void refreshKeyBoardInput()
        {
            var wasd = new DirectionalButtonsMap(
                wasd_up,
                wasd_down,
                wasd_left,
                wasd_right
            );;
            move = new AlternativeDirectionalMap(arrowKeys, wasd);

            var camAlts = new Alternative5DirectionalMap();
            camAlts.add(new DirectionalButtonsMap(null, null, cameraTiltLeft, cameraTiltRight));
            camAlts.add(new DirectionalMouseScrollMap());
            cameraStick = camAlts;
        }
        void refreshMouseInput()
        {

            mousePan = new NoButtonMap();
            
            mouseCancel = new NoButtonMap();
            hasPanOrderMix = false;

            
            if (inputSource.HasMouse && !inputSource.IsSteamInput)
            {
                mouseSelect = new NoButtonMap();

                checkButton(new MouseButtonMap(MouseButton.Left), leftMouseAction);
                checkButton(new MouseButtonMap(MouseButton.Right), rightMouseAction);
                checkButton(new MouseButtonMap(MouseButton.Middle), middleMouseAction);
                checkButton(new MouseButtonMap(MouseButton.X1), X1MouseAction);
                checkButton(new MouseButtonMap(MouseButton.X2), X2MouseAction);


                void checkButton(IButtonMap button, MouseButtonAction action)
                {
                    switch (action)
                    {
                        case MouseButtonAction.Select:
                            mouseSelect = InputLib.CombineButtons(mouseSelect, button);
                            break;
                        case MouseButtonAction.Pan:
                            mousePan = InputLib.CombineButtons(mousePan, button);
                            break;
                        case MouseButtonAction.PanAndCancel:
                            mousePan = InputLib.CombineButtons(mousePan, button);
                            mouseCancel = InputLib.CombineButtons(mouseCancel, button);
                            break;
                        case MouseButtonAction.PanAndOrder:
                            mousePan = InputLib.CombineButtons(mousePan, button);
                            mouseOrder = InputLib.CombineButtons(mousePan, button);
                            hasPanOrderMix = true;
                            break;
                        case MouseButtonAction.PanAndOrderAndCancel:
                            mousePan = InputLib.CombineButtons(mousePan, button);
                            mouseOrder = InputLib.CombineButtons(mousePan, button);
                            hasPanOrderMix = true;
                            mouseCancel = InputLib.CombineButtons(mouseCancel, button);
                            break;
                        case MouseButtonAction.Order:
                            mouseOrder = InputLib.CombineButtons(mousePan, button);
                            break;
                        case MouseButtonAction.Cancel:
                            mouseCancel = InputLib.CombineButtons(mouseCancel, button);
                            break;

                    }
                }
            }
        }
        public override void steamSetup()
        {
            int idx = inputSource.controllerIndex;

            // --- Movement & Camera ---
            move = new SteamAnalogMap( SteamActionSet.InGameControls, false, SteamAnalogAction.PanCamera, idx);
            moveCursor = new SteamAnalogMap(SteamActionSet.InGameControls, true, SteamAnalogAction.MoveCursor, idx);

            //if (inputSource.ControllerMode)
            //{ 
            //    move = new AlternativeDirectionalMap(move, moveCursor);
            //}

            cameraStick = new SteamAnalogMap(SteamActionSet.InGameControls, false, SteamAnalogAction.CameraStick, idx);
            // Note: cameraTiltUpSmooth is handled by the Steam Input config (e.g. Chorded Press) 
            // so you just map it to the intended resulting action.
            cameraTiltUpSmooth = new SteamAnalogMap(SteamActionSet.InGameControls, false, SteamAnalogAction.CameraTilt, idx);

            // --- Core Gameplay ---
            mouseSelect = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.select, idx);
            mouseOrder = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.order, idx);
            CancelKey = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.cancel, idx);
            StopStart = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.stop_start, idx);

            // --- UI & Windows ---
            Menu = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.open_menu, idx);
            ToggleHudDetail = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.toggle_hud_detail, idx);
            ToggleMinimap = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.toggle_minimap, idx);
            Controller_TabLeft = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.tab_left, idx);
            Controller_TabRight = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.tab_right, idx);

            // --- Strategy / RTS Actions ---
            Build = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.build, idx);
            Copy = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.copy, idx);
            Paste = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.paste, idx);
            GameSpeed = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.gamespeed, idx);
            PauseGame = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.pause, idx);
            QuickSelect = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.quick_select, idx);

            // --- Cycling / Focus ---
            NextCity = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.next_city, idx);
            NextArmy = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.next_army, idx);
            NextWar = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.next_war, idx);

            Controller_ObjectMenuToggle = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.controller_focus, idx);
            Controller_Faction = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.controller_faction, idx);
            ControllerMessageClick = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.controller_message, idx);

            // --- Flag Design / Editor ---
            // Note: If these share buttons in Steam, they use the same DigitalAction keys
            FlagDesign_ToggleColor_Prev = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.tab_left, idx);
            FlagDesign_ToggleColor_Next = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.tab_right, idx);
            Controller_FlagDesign_Colorpicker = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.controller_focus, idx);
            FlagDesign_PaintBucket = new SteamButtonMap(SteamActionSet.InGameControls, SteamDigitalAction.build, idx);

            // --- Menus & Editors ---
            // Make sure your MenuControls set is handled in the sub-set setups
            menuInput?.steamSetup(idx);
            editorInput?.steamSetup(idx, move, cameraStick);

            refreshMouseInput();
        }
        public override void xboxSetup()
        {
            //wasd = new DirectionalButtonsMap(null, null, null, null);
            move = new DirectionalXboxMap(ThumbStickType.Left, false, inputSource.controllerIndex);
            dpadMove = new DirectionalXboxMap(ThumbStickType.D, false, inputSource.controllerIndex);  
            
            cameraStick =new DirectionalXboxMap(ThumbStickType.Right, false, inputSource.controllerIndex);
            
            cameraTiltUpSmooth = new KeyPlusDirectionalMap(new XboxButtonMap(Buttons.LeftTrigger, inputSource.controllerIndex), new DirectionalXboxMap(ThumbStickType.Right, false, inputSource.controllerIndex));

            mouseSelect = new XboxButtonMap_TriggerAlts(Buttons.A, inputSource.controllerIndex);
            mouseOrder = new XboxButtonMap_TriggerAlts(Buttons.X, inputSource.controllerIndex);
            Controller_ObjectMenuToggle = new XboxButtonMap_TriggerAlts(Buttons.Y, inputSource.controllerIndex);
            Controller_Faction = new XboxButtonMap_TriggerAlts(Buttons.Back, inputSource.controllerIndex);
            CancelKey = new XboxButtonMap_TriggerAlts(Buttons.B, inputSource.controllerIndex);


            StopStart = new XboxButtonMap_TriggerAlts(Buttons.Start, inputSource.controllerIndex, true);
            
            Menu = new XboxButtonMap_TriggerAlts(Buttons.Start, inputSource.controllerIndex);
            //ToggleHudDetail = new XboxButtonMap_TriggerAlts(Buttons.Y, inputSource.controllerIndex);
            ToggleHudDetail = new XboxButtonMap_TriggerAlts(Buttons.DPadDown, inputSource.controllerIndex);//new NoButtonMap();
            ToggleMinimap = new XboxButtonMap_TriggerAlts(Buttons.DPadDown, inputSource.controllerIndex, true);

            Controller_TabLeft = new XboxButtonMap(Buttons.LeftShoulder, inputSource.controllerIndex);
            Controller_TabRight = new XboxButtonMap(Buttons.RightShoulder, inputSource.controllerIndex);
            //Controller_SubTabLeft = new XboxButtonMap(Buttons.LeftTrigger, inputSource.controllerIndex);
            //Controller_SubTabRight = new XboxButtonMap(Buttons.RightTrigger, inputSource.controllerIndex);
            QuickSelect = new XboxButtonMap_TriggerAlts(Buttons.A, inputSource.controllerIndex, true);
            Build = new XboxButtonMap_TriggerAlts(Buttons.X, inputSource.controllerIndex, true);
            Copy = new XboxButtonMap_TriggerAlts(Buttons.Y, inputSource.controllerIndex, true);
            Paste = new XboxButtonMap_TriggerAlts(Buttons.B, inputSource.controllerIndex, true);

            GameSpeed = new NoButtonMap();//
            PauseGame = new NoButtonMap();//

            menuInput?.xboxSetup(inputSource.controllerIndex);

            NextCity = new XboxButtonMap_TriggerAlts(Buttons.DPadLeft, inputSource.controllerIndex);// new XboxButtonMap_TriggerAlts(Buttons.A, inputSource.controllerIndex, true, false);
            NextArmy = new XboxButtonMap(Buttons.DPadRight, inputSource.controllerIndex);//new XboxButtonMap_TriggerAlts(Buttons.X, inputSource.controllerIndex, true, false);
            NextWar = new XboxButtonMap_TriggerAlts(Buttons.DPadLeft, inputSource.controllerIndex, true);
            //NextBattle = new XboxButtonMap_TriggerAlts(Buttons.Y, inputSource.controllerIndex, true, false);

            ControllerMessageClick = new XboxButtonMap(Buttons.DPadUp, inputSource.controllerIndex);//new XboxButtonMap_TriggerAlts(Buttons.A, inputSource.controllerIndex, true, true);

            FlagDesign_ToggleColor_Prev = new XboxButtonMap(Buttons.LeftShoulder, inputSource.controllerIndex);
            FlagDesign_ToggleColor_Next = new XboxButtonMap(Buttons.RightShoulder, inputSource.controllerIndex);
            Controller_FlagDesign_Colorpicker = new XboxButtonMap(Buttons.Y, inputSource.controllerIndex);
            FlagDesign_PaintBucket = new XboxButtonMap(Buttons.X, inputSource.controllerIndex);
            refreshMouseInput();

            menuInput.xboxSetup(inputSource.controllerIndex);
            editorInput.xboxSetup(inputSource.controllerIndex, move, cameraStick);
        }

        public void write(System.IO.BinaryWriter w)
        {
            const int InputVersion = 7;
            w.Write(InputVersion);


            if (inputSource.HasKeyBoard)
            {
                CancelKey.write(w);
                StopStart.write(w);

                ToggleHudDetail.write(w);
                ToggleMinimap.write(w);
                GameSpeed.write(w);
                PauseGame.write(w);
                NextCity.write(w);
                NextArmy.write(w);
                NextWar.write(w);

                wasd_up.write(w);
                wasd_down.write(w);
                wasd_left.write(w);
                wasd_right.write(w);

                cameraTiltLeft.write(w);
                cameraTiltRight.write(w);
                cameraTiltUp.write(w);

                zoomInKey.write(w);
                zoomOutKey.write(w);

                Copy.write(w);
                Paste.write(w);
                Build.write(w);
            }

            if (inputSource.HasMouse)
            {
                w.Write((byte)leftMouseAction);
                w.Write((byte)rightMouseAction);
                w.Write((byte)middleMouseAction);
                w.Write((byte)X1MouseAction);
                w.Write((byte)X2MouseAction);
            }

            if (inputSource.IsXnaController)
            {
                //Controller_FlagDesign_Colorpicker.write(w);
                //mouseSelect.write(w);
            }

            refreshMouseInput();

            Debug.WriteCheck(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            int inputVersion = r.ReadInt32();
            if (inputSource.IsXnaController)
            {
                xboxSetup();
            }
           
            if (inputSource.HasKeyBoard)
            {
                CancelKey = MapRead.Button(r, inputSource.controllerIndex);
                StopStart = MapRead.Button(r, inputSource.controllerIndex);

                //Home = MapRead.Button(r, inputSource.controllerIndex);
                ToggleHudDetail = MapRead.Button(r, inputSource.controllerIndex);
                if (inputVersion >= 7)
                { 
                    ToggleMinimap = MapRead.Button(r, inputSource.controllerIndex);
                }
                GameSpeed = MapRead.Button(r, inputSource.controllerIndex);
                PauseGame = MapRead.Button(r, inputSource.controllerIndex);
                NextCity = MapRead.Button(r, inputSource.controllerIndex);
                NextArmy = MapRead.Button(r, inputSource.controllerIndex);
                NextWar = MapRead.Button(r, inputSource.controllerIndex);

                wasd_up = MapRead.Button(r, inputSource.controllerIndex);
                wasd_down = MapRead.Button(r, inputSource.controllerIndex);
                wasd_left = MapRead.Button(r, inputSource.controllerIndex);
                wasd_right = MapRead.Button(r, inputSource.controllerIndex);

                cameraTiltLeft = MapRead.Button(r, inputSource.controllerIndex);
                cameraTiltRight = MapRead.Button(r, inputSource.controllerIndex);
                cameraTiltUp = MapRead.Button(r, inputSource.controllerIndex);

                zoomInKey = MapRead.Button(r, inputSource.controllerIndex);
                zoomOutKey = MapRead.Button(r, inputSource.controllerIndex);

                Copy = MapRead.Button(r, inputSource.controllerIndex);
                Paste = MapRead.Button(r, inputSource.controllerIndex);
                Build = MapRead.Button(r, inputSource.controllerIndex);
               

                refreshKeyBoardInput();
            }

            if (inputSource.HasMouse)
            {                
                leftMouseAction = (MouseButtonAction)r.ReadByte();
                rightMouseAction = (MouseButtonAction)r.ReadByte();
                middleMouseAction = (MouseButtonAction)r.ReadByte();
                X1MouseAction = (MouseButtonAction)r.ReadByte();
                X2MouseAction = (MouseButtonAction)r.ReadByte();

                if (inputVersion < 6)
                {
                    checkOld(ref leftMouseAction);
                    checkOld(ref rightMouseAction);
                    checkOld(ref middleMouseAction);
                    checkOld(ref X1MouseAction);
                    checkOld(ref X2MouseAction);
                }

                void checkOld(ref MouseButtonAction action)
                {
                    if (action == MouseButtonAction.PanAndCancel || action == MouseButtonAction.PanAndOrderAndCancel)
                    {
                        action++;
                    }
                }

                refreshMouseInput();
            }

            if (inputSource.IsXnaController)
            {
                //Controller_FlagDesign_Colorpicker = MapRead.Button(r, inputSource.controllerIndex);
                //mouseSelect = MapRead.Button(r, inputSource.controllerIndex);
            }

            Debug.ReadCheck(r);
        }

        public List<InputActionType> listInputs(bool keyboard)
        {
            List<InputActionType> result = new List<InputActionType>();
                       

            if (keyboard)
            {
                result.AddRange(new List<InputActionType>
                {
                    InputActionType.WASD_UP,
                    InputActionType.WASD_DOWN,
                    InputActionType.WASD_LEFT,
                    InputActionType.WASD_RIGHT,

                    InputActionType.CameraTiltLeft, InputActionType.CameraTiltRight, InputActionType.CameraTiltUp,

                    InputActionType.ZoomInKey,
                    InputActionType.ZoomOutKey,
                    InputActionType.Build,

                    InputActionType.Copy,
                    InputActionType.Paste,
                    InputActionType.Build,
                });
            }
            else
            {
                result.AddRange(new List<InputActionType>
                {
                    InputActionType.ControllerFocus,
                    InputActionType.ControllerCancel,
                    InputActionType.ControllerMessageClick,

                    //InputActionType.ControllerSelect,
                });
            }

            result.AddRange(new List<InputActionType>()
            {
                InputActionType.StopStart,
                InputActionType.GameSpeed,
                InputActionType.PauseGame,
                InputActionType.NextCity,
                InputActionType.NextArmy,
                InputActionType.NextWar,
                InputActionType.ToggleHudDetail,
                InputActionType.ToggleMiniMap,

            });

            return result;
        }

        public List<InputActionType> listEditorInputs(bool keyboard)
        {
            List<InputActionType> result = new List<InputActionType>();
            result.AddRange(new List<InputActionType>
                {

                    InputActionType.FlagDesign_ToggleColor_Prev,
                    InputActionType.FlagDesign_ToggleColor_Next,
                    InputActionType.FlagDesign_PaintBucket,
                });

            if (keyboard)
            {
            }
            else
            {
                result.AddRange(new List<InputActionType>
                {
                    InputActionType.Controller_FlagDesign_Colorpicker,
                });
            }


            return result;
        }

        public void getset(InputActionType type, ref IButtonMap buttonMap, bool set)
        {
            if (set)
            {
                Ref.gamesett.settingsHasChanged = true;
            }

            switch (type)
            {
                case InputActionType.StopStart:
                    if (set)
                    {
                        StopStart = buttonMap;
                    }
                    else
                    {
                        buttonMap = StopStart;
                    }
                    break;
                case InputActionType.ToggleMiniMap:
                    if (set)
                    {
                        ToggleMinimap = buttonMap;
                    }
                    else
                    {
                        buttonMap = ToggleMinimap;
                    }
                    break;

                case InputActionType.ToggleHudDetail:
                    if (set)
                    {
                        ToggleHudDetail = buttonMap;
                    }
                    else
                    {
                        buttonMap = ToggleHudDetail;
                    }
                    break;

                case InputActionType.GameSpeed:
                    if (set)
                    {
                        GameSpeed = buttonMap;
                    }
                    else
                    {
                        buttonMap = GameSpeed;
                    }
                    break;

                case InputActionType.PauseGame:
                    if (set)
                    {
                        PauseGame = buttonMap;
                    }
                    else
                    {
                        buttonMap = PauseGame;
                    }
                    break;

                case InputActionType.NextCity:
                    if (set)
                    {
                        NextCity = buttonMap;
                    }
                    else
                    {
                        buttonMap = NextCity;
                    }
                    break;

                case InputActionType.NextArmy:
                    if (set)
                    {
                        NextArmy = buttonMap;
                    }
                    else
                    {
                        buttonMap = NextArmy;
                    }
                    break;

                case InputActionType.NextWar:
                    if (set)
                    {
                        NextWar = buttonMap;
                    }
                    else
                    {
                        buttonMap = NextWar;
                    }
                    break;

                case InputActionType.ControllerFocus:
                    if (set)
                    {
                        Controller_ObjectMenuToggle = buttonMap;
                    }
                    else
                    {
                        buttonMap = Controller_ObjectMenuToggle;
                    }
                    break;

                case InputActionType.ControllerCancel:
                    if (set)
                    {
                        CancelKey = buttonMap;
                    }
                    else
                    {
                        buttonMap = CancelKey;
                    }
                    break;

                case InputActionType.ControllerMessageClick:
                    if (set)
                    {
                        ControllerMessageClick = buttonMap;
                    }
                    else
                    {
                        buttonMap = ControllerMessageClick;
                    }
                    break;

                case InputActionType.Build:
                    if (set) Build = buttonMap;
                    else buttonMap = Build;
                    break;
                case InputActionType.Copy:
                    if (set) Copy = buttonMap;
                    else buttonMap = Copy;
                    break;
                case InputActionType.Paste:
                    if (set) Paste = buttonMap;
                    else buttonMap = Paste;
                    break;
                case InputActionType.Menu:
                    if (set) Menu = buttonMap;
                    else buttonMap = Menu;
                    break;
                case InputActionType.FlagDesign_ToggleColor_Prev:
                    if (set) FlagDesign_ToggleColor_Prev = buttonMap;
                    else buttonMap = FlagDesign_ToggleColor_Prev;
                    break;
                case InputActionType.FlagDesign_ToggleColor_Next:
                    if (set) FlagDesign_ToggleColor_Next = buttonMap;
                    else buttonMap = FlagDesign_ToggleColor_Next;
                    break;
                case InputActionType.FlagDesign_PaintBucket:
                    if (set) FlagDesign_PaintBucket = buttonMap;
                    else buttonMap = FlagDesign_PaintBucket;
                    break;
                case InputActionType.Controller_FlagDesign_Colorpicker:
                    if (set) Controller_FlagDesign_Colorpicker = buttonMap;
                    else buttonMap = Controller_FlagDesign_Colorpicker;
                    break;
                //case InputActionType.ControllerSelect:
                //    if (set) ControllerSelect = buttonMap;
                //    else buttonMap = ControllerSelect;
                //    break;
                case InputActionType.WASD_UP:
                    if (set) wasd_up = buttonMap;
                    else buttonMap = wasd_up;
                    break;
                case InputActionType.WASD_DOWN:
                    if (set) wasd_down = buttonMap;
                    else buttonMap = wasd_down;
                    break;
                case InputActionType.WASD_LEFT:
                    if (set) wasd_left = buttonMap;
                    else buttonMap = wasd_left;
                    break;
                case InputActionType.WASD_RIGHT:
                    if (set) wasd_right = buttonMap;
                    else buttonMap = wasd_right;
                    break;
                case InputActionType.CameraTiltLeft:
                    if (set) cameraTiltLeft = buttonMap;
                    else buttonMap = cameraTiltLeft;
                    break;
                case InputActionType.CameraTiltRight:
                    if (set) cameraTiltRight = buttonMap;
                    else buttonMap = cameraTiltRight;
                    break;
                case InputActionType.CameraTiltUp:
                    if (set) cameraTiltUp = buttonMap;
                    else buttonMap = cameraTiltUp;
                    break;
                case InputActionType.ZoomInKey:
                    if (set) zoomInKey = buttonMap;
                    else buttonMap = zoomInKey;
                    break;
                case InputActionType.ZoomOutKey:
                    if (set) zoomOutKey = buttonMap;
                    else buttonMap = zoomOutKey;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public bool cancelDownEvent()
        { 
            return CancelKey.DownEvent || mouseCancel.DownEvent;
        }
        public bool cancelDownEvent_anyInstance()
        {
            return CancelKey.DownEvent_AnyInstance || mouseCancel.DownEvent;
        }

        public List<SpriteName> cancelIcons()
        {
            List<SpriteName> icons = new List<SpriteName>(2);
            CancelKey.ListIcons(icons);
            mouseCancel.ListIcons(icons);

            return icons;
        }

        public override void genericControllerSetup()
        {
            xboxSetup();
        }

        public bool anyActionKeyDown(bool includeCancel)
        {
            return mouseSelect.DownEvent || mouseOrder.DownEvent || mouseCancel.DownEvent || (includeCancel && cancelDownEvent());
        }


        const float KeyZoomSpeed = 10;
        public float ZoomValue()
        {

            float result = InputLib.OnlyOneDimentionOut(cameraStick.directionAndTime).Y * Ref.gamesett.scrollWheelSensitivity_game;
            if (inputSource.HasMouse)
            {
                result += lib.ToLeftRight(Input.Mouse.ScrollValue) * -10f * Ref.gamesett.scrollWheelSensitivity_game;
            }

            if (zoomInKey.IsDown)
            {
                result -= KeyZoomSpeed * Ref.gamesett.scrollWheelSensitivity_game;
            }
            if (zoomOutKey.IsDown)
            {
                result += KeyZoomSpeed * Ref.gamesett.scrollWheelSensitivity_game;
            }
            return result;            
        }

        public IButtonMap RichboxGuiSelect => mouseSelect;
        public IntVector2 RichboxGuiMove() { return move.stepping + dpadMove.stepping; }
        public bool RichboxGuiUseMove => inputSource.ControllerMode;

        public override EditorInputMap VoxelEditorInput()
        {
            return editorInput;
        }
    }

    enum InputActionType
    {
        StopStart,
        ToggleHudDetail,
        ToggleMiniMap,
        GameSpeed,
        PauseGame,
        NextCity,
        NextArmy,
        NextWar,
        Build,
        Copy,
        Paste,
        Menu,
        FlagDesign_ToggleColor_Prev,//"Previous color"
        FlagDesign_ToggleColor_Next,
        FlagDesign_PaintBucket,
        Controller_FlagDesign_Colorpicker,

        ControllerFocus, //"Focus"
        ControllerCancel,
        ControllerMessageClick,
        //ControllerSelect,

        WASD_UP,//"Up"
        WASD_DOWN,
        WASD_LEFT,
        WASD_RIGHT,

        CameraTiltLeft,
        CameraTiltRight,
        CameraTiltUp,

        ZoomInKey,
        ZoomOutKey,

        NUM,
    }

    enum MouseButtonAction
    { 
        None,
        Select,
        Pan,
        PanAndCancel,
        PanAndOrder,
        PanAndOrderAndCancel,
        Order,
        Cancel,
        NUM
    }
}
