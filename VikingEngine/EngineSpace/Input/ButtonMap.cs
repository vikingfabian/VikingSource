using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.Input
{
    enum ButtonMapType
    { //Index får inte förändras
        NoButtonMap,
        AlternativeButtonsMap,
        CombinedButtonsMap,
        Keyboard,
        Mouse,
        XController,
        GenericController,
        XController_NoAlt,
    }
    
    interface IButtonMap
    {
        bool IsDown { get; }
        bool DownEvent { get; }
        bool UpEvent { get; }
        float Value { get; }
        SpriteName Icon { get; }
        void ListIcons(List<SpriteName> list);
        string ButtonName { get; }
        bool IsMouse { get; }

        int buttonIndex { get; }
        int ControllerIndex { get; set; }

        InputSourceType inputSource { get; }

        void write(System.IO.BinaryWriter w);
        void read(System.IO.BinaryReader r);
    }

    static class MapRead
    {
        public static IButtonMap Button(System.IO.BinaryReader r)
        {
            IButtonMap result = null;
            ButtonMapType type = (ButtonMapType)r.ReadByte();
            switch (type)
            {
                case ButtonMapType.AlternativeButtonsMap:
                    result = new AlternativeButtonsMap();
                    break;
                case ButtonMapType.Keyboard:
                    result = new KeyboardButtonMap();
                    break;
                case ButtonMapType.Mouse:
                    result = new MouseButtonMap();
                    break;
                case ButtonMapType.NoButtonMap:
                    result = new NoButtonMap();
                    break;
                case ButtonMapType.XController:
                    result = new XboxButtonMap();
                    break;
                //case ButtonMapType.GenericController:
                //    result = new GenericControllerButtonMap();
                //    break;
                default:
                    throw new Exception("Could not read button type: " + type.ToString());
            }

            result.read(r);
            return result;
        }
        public static IDirectionalMap Directional(System.IO.BinaryReader r)
        {
            IDirectionalMap result;
            DirectionalMapType type = (DirectionalMapType)r.ReadByte();
            switch (type)
            {
                case DirectionalMapType.AlternativeDirectionalMap:
                    result = new AlternativeDirectionalMap();
                    break;
                case DirectionalMapType.FourButtons:
                    result = new DirectionalButtonsMap();
                    break;
                case DirectionalMapType.KeyPlusDirectionalMap:
                    result = new KeyPlusDirectionalMap();
                    break;
                case DirectionalMapType.MouseMove:
                    result = new DirectionalMouseMap();
                    break;
                case DirectionalMapType.MouseScroll:
                    result = new DirectionalMouseScrollMap();
                    break;
                case DirectionalMapType.Xbox:
                    result = new DirectionalXboxMap();
                    break;
                case DirectionalMapType.XboxTriggers:
                    result = new DirectionalXboxTriggerMap();
                    break;
                //case DirectionalMapType.GenericDualAxes:
                //    result = new DirectionalGenericDualAxesMap();
                //    break;
                //case DirectionalMapType.GenericDpad:
                //    result = new DirectionalGenericDpadMap();
                //    break;
                default:
                    throw new Exception("Could not read direction type: " + type.ToString());
            }

            result.read(r);
            return result;
        }
    }

    struct NoButtonMap : IButtonMap
    {
        public bool IsDown { get { return false; } }
        public bool DownEvent { get { return false; } }
        public bool UpEvent { get { return false; } }
        public float Value { get { return 0f; } }
        public SpriteName Icon { get { return SpriteName.MissingImage; } }
        public int buttonIndex { get { return -1; } }
        public int ControllerIndex { get { return -1; } set { } }
        
        public void ListIcons(List<SpriteName> list)
        {
            list.Add(Icon);
        }
        public string ButtonName { get { return ""; } }
        public bool IsMouse { get { return false; } }

        public InputSourceType inputSource { get { return InputSourceType.Num_Non; } }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)ButtonMapType.NoButtonMap);
        }
        public void read(System.IO.BinaryReader r)
        {
        }
    }

    struct AlternativeButtonsMap : IButtonMap
    {
        IButtonMap button1, button2;
        //SpriteName[] icons;

        public AlternativeButtonsMap(IButtonMap button1, IButtonMap button2)
        {
            this.button1 = button1;
            this.button2 = button2;
            //icons = button1.Icons.Concat(button2.Icons).ToArray();
        }

        public bool IsDown { get { return button2 == null ? button1.IsDown : button1.IsDown || button2.IsDown; } }
        public bool DownEvent { get { return button2 == null ? button1.DownEvent : button1.DownEvent || button2.DownEvent; } }
        public bool UpEvent { get { return button2 == null ? button1.UpEvent : button1.UpEvent || button2.UpEvent; } }
        public float Value { get { return button2 == null ? button1.Value : button1.Value + button2.Value; } }
        public int buttonIndex { get { return button1.buttonIndex; } }
        public int ControllerIndex { get { return button1.ControllerIndex; } 
            set { button1.ControllerIndex = value; button2.ControllerIndex = value; } }

        public InputSourceType inputSource { get { return button1.inputSource; } }

        public SpriteName Icon { get { return button1.Icon; } }
        public void ListIcons(List<SpriteName> list)
        {
            button1.ListIcons(list);
            button2.ListIcons(list);
        }
        public string ButtonName { get { return button1.ButtonName + ", " + button2.ButtonName; } }
        public bool IsMouse { get { return button1.IsMouse || button2.IsMouse; } }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)ButtonMapType.AlternativeButtonsMap);
            button1.write(w);
            button2.write(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            button1 = MapRead.Button(r);
            button2 = MapRead.Button(r);
        }
    }

    struct Alternative5ButtonsMap : IButtonMap
    {
        IButtonMap button1, button2, button3, button4, button5;
        int count;
        //SpriteName[] icons;

        //public Alternative5ButtonsMap()
        //{
        //    button1 = null;
        //    button2 = null;
        //    button3 = null;
        //    button4 = null;
        //    button5 = null;
        //    count = 0;
        //}

        public void add(IButtonMap button)
        {
            count++;
            switch (count)
            {
                case 1: button1 = button; break;
                case 2: button2 = button; break;
                case 3: button3 = button; break;
                case 4: button4 = button; break;
                case 5: button5 = button; break;
            }
        }


        public bool IsDown
        {
            get
            {
                if (button1 == null) return false;

                switch (count)
                {
                    default:
                        return button1.IsDown;
                    case 2:
                        return button1.IsDown || button2.IsDown;
                    case 3:
                        return button1.IsDown || button2.IsDown || button3.IsDown;
                    case 4:
                        return button1.IsDown || button2.IsDown || button3.IsDown || button4.IsDown;
                    case 5:
                        return button1.IsDown || button2.IsDown || button3.IsDown || button4.IsDown || button5.IsDown;
                }
            }
        }
        public bool DownEvent { 
             get
            {
                switch (count)
                {
                    default:
                        return button1.DownEvent;
                    case 2:
                        return button1.DownEvent || button2.DownEvent;
                    case 3:
                        return button1.DownEvent || button2.DownEvent || button3.DownEvent;
                    case 4:
                        return button1.DownEvent || button2.DownEvent || button3.DownEvent || button4.DownEvent;
                    case 5:
                        return button1.DownEvent || button2.DownEvent || button3.DownEvent || button4.DownEvent || button5.DownEvent;
                }
            }
        }

        public Input.InputSourceType activeSource()
        {
            if (button2 != null && button2.IsDown)
            {
                return button2.inputSource;
            }
            if (button3 != null && button3.IsDown)
            {
                return button3.inputSource;
            }
            if (button4 != null && button4.IsDown)
            {
                return button4.inputSource;
            }
            if (button5 != null && button5.IsDown)
            {
                return button5.inputSource;
            }
            return button1.inputSource;
        }
   
        //    get { return button2 == null ? button1.DownEvent : button1.DownEvent || button2.DownEvent; } }
        //public bool UpEvent { get { return button2 == null ? button1.UpEvent : button1.UpEvent || button2.UpEvent; } }
        public bool UpEvent
        {
            get
            {
                switch (count)
                {
                    default:
                        return button1.UpEvent;
                    case 2:
                        return button1.UpEvent || button2.UpEvent;
                    case 3:
                        return button1.UpEvent || button2.UpEvent || button3.UpEvent;
                    case 4:
                        return button1.UpEvent || button2.UpEvent || button3.UpEvent || button4.UpEvent;
                    case 5:
                        return button1.UpEvent || button2.UpEvent || button3.UpEvent || button4.UpEvent || button5.UpEvent;
                }
            }
        }

        public IButtonMap GetFromSource(Input.InputSourceType fromSource)
        {
            if (button2 != null && button2.inputSource == fromSource)
            {
                return button2;
            }
            if (button3 != null && button3.inputSource == fromSource)
            {
                return button3;
            }
            if (button4 != null && button4.inputSource == fromSource)
            {
                return button4;
            }
            if (button5 != null && button5.inputSource == fromSource)
            {
                return button5;
            }
            return button1;
        }


        public float Value { get { return IsDown? 1 : 0; } }
        public int buttonIndex { get { return button1.buttonIndex; } }
        public int ControllerIndex { get { return button1.ControllerIndex; } set { lib.DoNothing(); } }

        public InputSourceType inputSource { get { return button1.inputSource; } }

        public SpriteName Icon { get { return button1.Icon; } }
        public void ListIcons(List<SpriteName> list)
        {
            button1.ListIcons(list);
            //button2.ListIcons(list);
        }
        public string ButtonName { get { return button1.ButtonName; } }
        public bool IsMouse { get { return button1.IsMouse; } }

        public void write(System.IO.BinaryWriter w)
        {
            //w.Write((byte)ButtonMapType.AlternativeButtonsMap);
            //button1.write(w);
            //button2.write(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            //button1 = MapRead.Button(r);
            //button2 = MapRead.Button(r);
        }
    }
 



    struct KeyPlusDirectionalMap : IDirectionalMap
    {
        IButtonMap plusKey;
        IDirectionalMap directional;

        public KeyPlusDirectionalMap(IButtonMap plusKey, IDirectionalMap directional)
        {
            this.plusKey = plusKey;
            this.directional = directional;
        }
        public Vector2 direction { get { return plusKey.IsDown ? directional.direction : Vector2.Zero; } }
        public Vector2 directionAndTime { get { return plusKey.IsDown ? directional.directionAndTime : Vector2.Zero; } }
        public IntVector2 stepping { get { return plusKey.IsDown ? directional.stepping : IntVector2.Zero; } }
        public bool plusKeyIsDown { get { return plusKey.IsDown; } }

        public string directionsName { get { return plusKey.ButtonName + " + " + directional.directionsName; } }

        public InputSourceType inputSource { get { return directional.inputSource; } }
        public int ControllerIndex { get { return directional.ControllerIndex; } set { lib.DoNothing(); } }

        public SpriteName Icon { get { return directional.Icon; } }

        public void ListIcons(List<SpriteName> list, out SpriteName plusKeyIcon, bool includeAlternative)
        {
            directional.ListIcons(list, out plusKeyIcon, includeAlternative);

            plusKeyIcon = plusKey.Icon;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)DirectionalMapType.KeyPlusDirectionalMap);
            plusKey.write(w);
            directional.write(w);
        }
        public void read(System.IO.BinaryReader r)
        {
            plusKey = MapRead.Button(r);
            directional = MapRead.Directional(r);
        }
    }

    struct TwoCombinedButtonsMap : IButtonMap
    {
        IButtonMap altKey;
        IButtonMap key;

        public TwoCombinedButtonsMap(IButtonMap altKey, IButtonMap key)
        {
            this.altKey = altKey;
            this.key = key;
        }

        public SpriteName Icon { get { return SpriteName.MissingImage; } }

        public void ListIcons(List<SpriteName> list)
        {
            list.Add(altKey.Icon);
            list.Add(key.Icon);
        }

        public bool IsDown { get { return altKey.IsDown && key.IsDown; } }
        public bool DownEvent
        {
            get
            {
                return (altKey.IsDown && key.IsDown) && (altKey.DownEvent || key.DownEvent);
            }
        }
        public bool UpEvent { get { return (altKey.IsDown != key.IsDown) && (altKey.UpEvent || key.UpEvent); ; } }
        public float Value { get { return IsDown ? 1f : 0f; } }
        //public SpriteName[] Icons { get { return icons; } }
        public string ButtonName { get { return altKey.ButtonName + " + " + key.ButtonName; } }
        public bool IsMouse { get { return key.IsMouse; } }
        public InputSourceType inputSource { get { return key.inputSource; } }
        public int buttonIndex { get { return key.buttonIndex; } }
        public int ControllerIndex { get { return key.ControllerIndex; } set { } }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)ButtonMapType.CombinedButtonsMap);
            altKey.write(w);
            key.write(w);
        }
        public void read(System.IO.BinaryReader r)
        {
            altKey = MapRead.Button(r);
            key = MapRead.Button(r);
        }
    }



    struct KeyboardButtonMap : IButtonMap
    {        
        /* Static */
        static private SpriteName GetKeyTile(Keys key)
        {
            switch (key)
            {
                default: return SpriteName.KeyUnknown;

                case Keys.A: return SpriteName.KeyA;
                case Keys.B: return SpriteName.KeyB;
                case Keys.C: return SpriteName.KeyC;
                case Keys.D: return SpriteName.KeyD;
                case Keys.E: return SpriteName.KeyE;
                case Keys.F: return SpriteName.KeyF;
                case Keys.G: return SpriteName.KeyG;
                case Keys.H: return SpriteName.KeyH;
                case Keys.I: return SpriteName.KeyI;
                case Keys.J: return SpriteName.KeyJ;
                case Keys.K: return SpriteName.KeyK;
                case Keys.L: return SpriteName.KeyL;
                case Keys.M: return SpriteName.KeyM;
                case Keys.N: return SpriteName.KeyN;
                case Keys.O: return SpriteName.KeyO;
                case Keys.P: return SpriteName.KeyP;
                case Keys.Q: return SpriteName.KeyQ;
                case Keys.R: return SpriteName.KeyR;
                case Keys.S: return SpriteName.KeyS;
                case Keys.T: return SpriteName.KeyT;
                case Keys.U: return SpriteName.KeyU;
                case Keys.V: return SpriteName.KeyV;
                case Keys.W: return SpriteName.KeyW;
                case Keys.X: return SpriteName.KeyX;
                case Keys.Y: return SpriteName.KeyY;
                case Keys.Z: return SpriteName.KeyZ;

                case Keys.F1: return SpriteName.KeyF1;
                case Keys.F2: return SpriteName.KeyF2;
                case Keys.F3: return SpriteName.KeyF3;
                case Keys.F4: return SpriteName.KeyF4;
                case Keys.F5: return SpriteName.KeyF5;
                case Keys.F6: return SpriteName.KeyF6;
                case Keys.F7: return SpriteName.KeyF7;
                case Keys.F8: return SpriteName.KeyF8;
                case Keys.F9: return SpriteName.KeyF9;
                case Keys.F10: return SpriteName.KeyF10;
                case Keys.F11: return SpriteName.KeyF11;
                case Keys.F12: return SpriteName.KeyF12;

                case Keys.Escape: return SpriteName.KeyEsc;
                case Keys.Enter: return SpriteName.KeyEnter;
                case Keys.Space: return SpriteName.KeySpace;
                case Keys.Tab: return SpriteName.KeyTab;

                case Keys.Back: return SpriteName.KeyBack;
                case Keys.Delete: return SpriteName.KeyDelete;

                case Keys.D0:
                case Keys.NumPad0: return SpriteName.Key0;
                case Keys.D1:
                case Keys.NumPad1: return SpriteName.Key1;
                case Keys.D2:
                case Keys.NumPad2: return SpriteName.Key2;
                case Keys.D3:
                case Keys.NumPad3: return SpriteName.Key3;
                case Keys.D4:
                case Keys.NumPad4: return SpriteName.Key4;
                case Keys.D5:
                case Keys.NumPad5: return SpriteName.Key5;
                case Keys.D6:
                case Keys.NumPad6: return SpriteName.Key6;
                case Keys.D7:
                case Keys.NumPad7: return SpriteName.Key7;
                case Keys.D8:
                case Keys.NumPad8: return SpriteName.Key8;
                case Keys.D9:
                case Keys.NumPad9: return SpriteName.Key9;

                case Keys.LeftShift: // fall through;
                case Keys.RightShift: return SpriteName.KeyShift;

                case Keys.RightControl: // fall through;
                case Keys.LeftControl: return SpriteName.KeyCtrl;

                case Keys.LeftAlt: // fall through;
                case Keys.RightAlt: return SpriteName.KeyAlt;

                case Keys.Up: return SpriteName.KeyArrowUp;
                case Keys.Down: return SpriteName.KeyArrowDown;
                case Keys.Left: return SpriteName.KeyArrowLeft;
                case Keys.Right: return SpriteName.KeyArrowRight;

                case Keys.CapsLock: return SpriteName.KeyCapsLock;

                case Keys.OemComma:
                case Keys.Decimal: return SpriteName.KeyComma;
                case Keys.OemSemicolon: return SpriteName.KeySemiColon;

                case Keys.Divide: return SpriteName.KeySlash;
                case Keys.End: return SpriteName.KeyEnd;
                case Keys.Home: return SpriteName.KeyHome;
                case Keys.Insert: return SpriteName.KeyInsert;
                case Keys.Multiply: return SpriteName.KeyStar;
                case Keys.OemBackslash: return SpriteName.KeyBackslash;
                case Keys.OemOpenBrackets: return SpriteName.KeyOpenBracket;
                case Keys.OemCloseBrackets: return SpriteName.KeyCloseBracket;
                
                case Keys.Subtract:
                case Keys.OemMinus: return SpriteName.KeyMinus;

                case Keys.Separator:
                case Keys.OemPeriod: return SpriteName.KeyPeriod;
                
                case Keys.OemPipe: return SpriteName.KeyPipe;
                case Keys.OemPlus: return SpriteName.KeyPlus;
                case Keys.OemQuestion: return SpriteName.KeyQuestionmark;
                case Keys.OemQuotes: return SpriteName.KeyQuote;
                case Keys.OemTilde: return SpriteName.KeyTilde;
                case Keys.PageDown: return SpriteName.KeyPgDown;
                case Keys.PageUp: return SpriteName.KeyPgUp;
                case Keys.PrintScreen: return SpriteName.KeyPrintScreen;

            }
        }

        /* Fields */
        Keys key;
        //SpriteName[] icons;

        /* Constructors */
        public KeyboardButtonMap(Keys key)
        {
            this.key = key;
            //icons = new SpriteName[] { GetKeyTile(key) };
        }

        /* Methods */
        public SpriteName Icon { get { return GetKeyTile(key); } }
        public void ListIcons(List<SpriteName> list)
        {
            list.Add(GetKeyTile(key));
        }
        
        public bool IsDown { get { return Keyboard.IsKeyDown(key); } }
        public bool DownEvent { get { 
            if (key == Keys.Tab && Input.Keyboard.Shift) 
            {return false;}

            return Keyboard.KeyDownEvent(key); 
        } }
        public bool UpEvent { get { return Keyboard.KeyUpEvent(key); } }
        public float Value { get { return IsDown ? 1f : 0f; } }
        //public SpriteName[] Icons { get { return icons; } }
        public string ButtonName { get { return key.ToString(); } }
        public bool IsMouse { get { return false; } }
        public InputSourceType inputSource { get { return InputSourceType.Keyboard; } }
        public int buttonIndex { get { return (int)key; } }
        public int ControllerIndex { get { return -1; } set { } }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)ButtonMapType.Keyboard);
            w.Write((byte)key);
        }
        public void read(System.IO.BinaryReader r)
        {
            key = (Keys)r.ReadByte();
        }

        public override string ToString()
        {
            return "Keyboard " + key.ToString() + 
                (Keyboard.IsKeyDown(key)? "(Down)" : "(Up)");
        }
    }

    struct MouseButtonMap : IButtonMap
    {
        /* Properties */
        public bool IsDown { get { return Input.Mouse.IsButtonDown(button); } }
        public bool DownEvent { get { return Input.Mouse.ButtonDownEvent(button); } }
        public bool UpEvent { get { return Input.Mouse.ButtonUpEvent(button); } }
        public float Value { get { return Input.Mouse.IsButtonDown(button) ? 1f : 0f; } }
        public string ButtonName { get { return button.ToString() + "Click"; } }
        public bool IsMouse { get { return true; } }
        public InputSourceType inputSource { get { return InputSourceType.Mouse; } }
        public int buttonIndex { get { return (int)button; } }
        public int ControllerIndex { get { return -1; } set { } }
        /* Fields */
        MouseButton button;
        //SpriteName[] icons;

        /* Constructors */
        public MouseButtonMap(MouseButton button)
        {
            this.button = button;
            
        }

        public SpriteName Icon { get {
            switch (button)
            {
                case MouseButton.Left:
                    return SpriteName.MouseButtonLeft;
                case MouseButton.Middle:
                    return SpriteName.MouseButtonMiddle;
                case MouseButton.Right:
                    return SpriteName.MouseButtonRight;
                case MouseButton.X1:
                    return SpriteName.MouseButtonX1;
                case MouseButton.X2:
                    return SpriteName.MouseButtonX2;
                default:
                    throw new NotImplementedException("Unknown mouse button.");
            }
            
        } }
        public void ListIcons(List<SpriteName> list)
        {
            list.Add(Icon);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)ButtonMapType.Mouse);
            w.Write((byte)button);
        }
        public void read(System.IO.BinaryReader r)
        {
            button = (MouseButton)r.ReadByte();
        }
    }
    
    struct XboxButtonMap : IButtonMap
    {
        public bool IsDown { get { return Input.XInput.Instance(controllerIx).IsButtonDown(button); } }
        public bool DownEvent { get { return Input.XInput.Instance(controllerIx).KeyDownEvent(button); } }
        public bool UpEvent { get { return Input.XInput.Instance(controllerIx).KeyUpEvent(button); } }
        public float Value
        {
            get
            {
                if (button == Buttons.LeftTrigger)
                    return Input.XInput.Instance(controllerIx).LeftTrigger;
                else if (button == Buttons.RightTrigger)
                    return Input.XInput.Instance(controllerIx).RightTrigger;
                return IsDown ? 1f : 0f;
            }
        }

        public bool IsMouse { get { return false; } }
        public string ButtonName { get { return button.ToString(); } }
        public InputSourceType inputSource { get { return InputSourceType.XController; } }
        public int buttonIndex { get { return (int)button; } }
        public int ControllerIndex { get { return controllerIx; } set { controllerIx = value; } }

        int controllerIx;
        Buttons button;
        
        public XboxButtonMap(Buttons button, int controllerIx)
        {
            this.button = button;
            this.controllerIx = controllerIx;
        }

        public SpriteName Icon
        {
            get
            {
                //if (Input.XInput.Instance(controllerIx).controllerType == ControllerType.PS4)
                //{
                //    return PS4InputLib.ButtonSprite(button);
                //}
                return XboxInputLib.ButtonSprite(button);
            }
        }
        public void ListIcons(List<SpriteName> list)
        {
            list.Add(Icon);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)ButtonMapType.XController);
            w.Write((int)button);
        }
        public void read(System.IO.BinaryReader r)
        {
            button = (Buttons)r.ReadInt32();
        }
    }

    struct XboxButtonMap_NoAlt : IButtonMap
    {
        public bool IsDown { get { return Input.XInput.Instance(controllerIx).IsButtonDown(button) && !altIsDown();  } }
        public bool DownEvent { get { return Input.XInput.Instance(controllerIx).KeyDownEvent(button) && !altIsDown(); } }
        public bool UpEvent { get { return Input.XInput.Instance(controllerIx).KeyUpEvent(button); } }

        bool altIsDown()
        {
            var ins = Input.XInput.Instance(controllerIx);
            return ins.IsButtonDown(Buttons.LeftTrigger) || ins.IsButtonDown(Buttons.RightTrigger);
        }
        public float Value
        {
            get
            {
                if (button == Buttons.LeftTrigger)
                    return Input.XInput.Instance(controllerIx).LeftTrigger;
                else if (button == Buttons.RightTrigger)
                    return Input.XInput.Instance(controllerIx).RightTrigger;
                return IsDown ? 1f : 0f;
            }
        }

        public bool IsMouse { get { return false; } }
        public string ButtonName { get { return button.ToString(); } }
        public InputSourceType inputSource { get { return InputSourceType.XController; } }
        public int buttonIndex { get { return (int)button; } }
        public int ControllerIndex { get { return controllerIx; } set { controllerIx = value; } }

        int controllerIx;
        Buttons button;

        public XboxButtonMap_NoAlt(Buttons button, int controllerIx)
        {
            this.button = button;
            this.controllerIx = controllerIx;
        }

        public SpriteName Icon
        {
            get
            {
                return XboxInputLib.ButtonSprite(button);
            }
        }
        public void ListIcons(List<SpriteName> list)
        {
            list.Add(Icon);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)ButtonMapType.XController_NoAlt);
            w.Write((int)button);
        }
        public void read(System.IO.BinaryReader r)
        {
            button = (Buttons)r.ReadInt32();
        }
    }
    //struct GenericControllerButtonMap : IButtonMap
    //{
    //    /* Static */
    //    private static SpriteName GetSpriteName(GenericControllerButton button)
    //    {
    //        if (button < GenericControllerButton.B15)
    //        {
    //            return (SpriteName)((int)button + (int)SpriteName.GenericButton0);
    //        }
    //        return SpriteName.GenericButtonAny;            
    //    }

    //    /* Fields */
    //    int controllerIx;
    //    GenericControllerButton button;

    //    /* Constructors */
    //    public GenericControllerButtonMap(GenericControllerButton button, int controllerIx)
    //    {
    //        this.button = button;
    //        this.controllerIx = controllerIx;
    //    }

    //    /* Properties */
    //    public SpriteName Icon
    //    {
    //        get
    //        {
    //            GenericController controller;
    //            if (arraylib.TryGet<GenericController>(SharpDXInput.controllers, controllerIx, out controller))
    //            {
    //                if (controller.type == ControllerType.PS4)
    //                {
    //                    return PS4InputLib.GetSpriteName(button);
    //                }
    //                else
    //                {
    //                    return GetSpriteName(button);
    //                }
    //            }
    //            else
    //            {
    //                return SpriteName.GenericButtonAny;    
    //            }
    //        }
    //    }            

    //    public bool IsDown { get { return SharpDXInput.IsDown(button, controllerIx); } }
    //    public bool DownEvent { get { return SharpDXInput.DownEvent(button, controllerIx); } }
    //    public bool UpEvent { get { return SharpDXInput.UpEvent(button, controllerIx); } }
    //    public float Value { get { return IsDown ? 1f : 0f; } }
    //    public bool IsMouse { get { return false; } }
    //    public string ButtonName { get { return button.ToString(); } }
    //    public PlayerInputSource inputSource { get { return PlayerInputSource.GenericController; } }
    //    public int buttonIndex { get { return (int)button; } }
    //    public int ControllerIndex { get { return controllerIx; } set { controllerIx = value; } }


    //    /* Novelty Methods */
    //    public void ListIcons(List<SpriteName> list)
    //    {
    //        list.Add(Icon);
    //    }

    //    public void write(System.IO.BinaryWriter w)
    //    {
    //        w.Write((byte)ButtonMapType.GenericController);
    //        w.Write((int)button);
    //    }
    //    public void read(System.IO.BinaryReader r)
    //    {
    //        button = (GenericControllerButton)r.ReadInt32();
    //    }
    //}






}
