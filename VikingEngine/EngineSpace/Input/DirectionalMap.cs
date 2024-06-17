using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace VikingEngine.Input
{
    enum DirectionalMapType
    {//Index får inte förändras
        AlternativeDirectionalMap,
        KeyPlusDirectionalMap,
        FourButtons,
        MouseMove,
        MouseScroll,
        Xbox,
        XboxTriggers,
        GenericDualAxes,
        GenericDpad,
    }

    interface IDirectionalMap
    {
        bool plusKeyIsDown { get; }
        Vector2 direction { get; }
        Vector2 directionAndTime { get; }
        IntVector2 stepping { get; }
        string directionsName { get; }
        InputSourceType inputSource { get; }
        int ControllerIndex { get; set; }
        SpriteName Icon { get; }
        void ListIcons(List<SpriteName> list, out SpriteName plusKey, bool includeAlternative);

        void write(System.IO.BinaryWriter w);
        void read(System.IO.BinaryReader r);
    }

    struct AlternativeDirectionalMap : IDirectionalMap
    {
        IDirectionalMap dirMap1, dirMap2;

        public AlternativeDirectionalMap(IDirectionalMap dirMap1, IDirectionalMap dirMap2)
        {
            this.dirMap1 = dirMap1; this.dirMap2 = dirMap2;
        }

        public Vector2 direction { get { return dirMap2 == null ? dirMap1.direction : dirMap1.direction + dirMap2.direction; } }
        public Vector2 directionAndTime { get { return dirMap2 == null ? dirMap1.directionAndTime : dirMap1.directionAndTime + dirMap2.directionAndTime; } }
        public IntVector2 stepping { get { return dirMap2 == null ? dirMap1.stepping : dirMap1.stepping + dirMap2.stepping; } }
        public bool plusKeyIsDown { get { return dirMap2 == null ? dirMap1.plusKeyIsDown : dirMap1.plusKeyIsDown || dirMap2.plusKeyIsDown; ; } }

        public string directionsName { get { return dirMap1.directionsName + ", " + dirMap2.directionsName; } }

        public InputSourceType inputSource { get { return dirMap1.inputSource; } }
        public int ControllerIndex
        {
            get { return dirMap1.ControllerIndex; }
            set { dirMap1.ControllerIndex = value; dirMap2.ControllerIndex = value; }
        }

        public SpriteName Icon { get { return dirMap1.Icon; } }

        public void ListIcons(List<SpriteName> list, out SpriteName plusKeyIcon, bool includeAlternative)
        {
            dirMap1.ListIcons(list, out plusKeyIcon, includeAlternative);

            if (includeAlternative)
            {
                //SpriteName non;
                dirMap2.ListIcons(list, out _, includeAlternative);
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)DirectionalMapType.AlternativeDirectionalMap);
            dirMap1.write(w);
            dirMap2.write(w);
        }
        public void read(System.IO.BinaryReader r)
        {
            dirMap1 = MapRead.Directional(r);
            dirMap2 = MapRead.Directional(r);
        }
    }

    struct Alternative5DirectionalMap : IDirectionalMap
    {
        IDirectionalMap dirMap1, dirMap2, dirMap3, dirMap4, dirMap5;
        int count;
        //public Alternative5DirectionalMap(IDirectionalMap dirMap1, IDirectionalMap dirMap2)
        //{
        //    this.dirMap1 = dirMap1; this.dirMap2 = dirMap2;
        //}

        public void add(IDirectionalMap map)
        {
            count++;
            switch (count)
            {
                case 1: dirMap1 = map; break;
                case 2: dirMap2 = map; break;
                case 3: dirMap3 = map; break;
                case 4: dirMap4 = map; break;
                case 5: dirMap5 = map; break;
            }
        }

        public Vector2 direction
        {
            get
            {
                switch (count)
                {
                    default: return dirMap1.direction;
                    case 2: return dirMap1.direction + dirMap2.direction;
                    case 3: return dirMap1.direction + dirMap2.direction + dirMap3.direction;
                    case 4: return dirMap1.direction + dirMap2.direction + dirMap3.direction + dirMap4.direction;
                    case 5: return dirMap1.direction + dirMap2.direction + dirMap3.direction + dirMap4.direction + dirMap5.direction;

                }
            }
        }
        // return dirMap2 == null ? dirMap1.direction : dirMap1.direction + dirMap2.direction; } }
        public Vector2 directionAndTime { get { return direction * Ref.DeltaTimeMs; } }//return dirMap2 == null ? dirMap1.directionAndTime : dirMap1.directionAndTime + dirMap2.directionAndTime; } }
        public IntVector2 stepping
        {
            get
            {
                switch (count)
                {
                    default: return dirMap1.stepping;
                    case 2: return dirMap1.stepping + dirMap2.stepping;
                    case 3: return dirMap1.stepping + dirMap2.stepping + dirMap3.stepping;
                    case 4: return dirMap1.stepping + dirMap2.stepping + dirMap3.stepping + dirMap4.stepping;
                    case 5: return dirMap1.stepping + dirMap2.stepping + dirMap3.stepping + dirMap4.stepping + dirMap5.stepping;

                }
            }
        }
        public Input.InputSourceType activeSource()
        {//
            if (dirMap2 != null && dirMap2.direction != Vector2.Zero)
            {
                return dirMap2.inputSource;
            }
            if (dirMap3 != null && dirMap3.direction != Vector2.Zero)
            {
                return dirMap3.inputSource;
            }
            if (dirMap4 != null && dirMap4.direction != Vector2.Zero)
            {
                return dirMap4.inputSource;
            }
            if (dirMap5 != null && dirMap5.direction != Vector2.Zero)
            {
                return dirMap5.inputSource;
            }
            return dirMap1.inputSource;
        }

        //get { return dirMap2 == null ? dirMap1.stepping : dirMap1.stepping + dirMap2.stepping; } }
        public bool plusKeyIsDown { get { return dirMap1.plusKeyIsDown; } }

        public string directionsName { get { return dirMap1.directionsName; } }

        public InputSourceType inputSource { get { return dirMap1.inputSource; } }
        public int ControllerIndex { get { return dirMap1.ControllerIndex; } set { } }

        public SpriteName Icon { get { return dirMap1.Icon; } }

        public void ListIcons(List<SpriteName> list, out SpriteName plusKeyIcon, bool includeAlternative)
        {
            dirMap1.ListIcons(list, out plusKeyIcon, includeAlternative);

            if (includeAlternative)
            {
                if (dirMap2 != null)
                {
                    dirMap2.ListIcons(list, out _, includeAlternative);
                }
                if (dirMap3 != null)
                {
                    dirMap3.ListIcons(list, out _, includeAlternative);
                }
                if (dirMap4 != null)
                {
                    dirMap4.ListIcons(list, out _, includeAlternative);
                }
                if (dirMap5 != null)
                {
                    dirMap5.ListIcons(list, out _, includeAlternative);
                }

            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            //w.Write((byte)DirectionalMapType.AlternativeDirectionalMap);
            //dirMap1.write(w);
            //dirMap2.write(w);
        }
        public void read(System.IO.BinaryReader r)
        {
            //dirMap1 = MapRead.Directional(r);
            //dirMap2 = MapRead.Directional(r);
        }
    }

    struct DirectionalButtonsMap : IDirectionalMap
    {
        IButtonMap up, down, left, right;
        public DirectionalButtonsMap(IButtonMap up, IButtonMap down, IButtonMap left, IButtonMap right)
        {
            this.up = up; this.down = down;
            this.left = left; this.right = right;
        }

        public Vector2 direction
        {
            get
            {
                Vector2 result = Vector2.Zero;
                if (up != null && up.IsDown) result.Y = -up.Value;
                if (down != null && down.IsDown) result.Y = down.Value;
                if (left != null && left.IsDown) result.X = -left.Value;
                if (right != null && right.IsDown) result.X = right.Value;

                return VectorExt.SafeNormalizeV2(result);
            }
        }
        public Vector2 directionAndTime { get { return direction * Ref.DeltaTimeMs; } }

        public IntVector2 stepping
        {
            get
            {
                IntVector2 result = IntVector2.Zero;
                if (up.DownEvent) result.Y = -1;
                if (down.DownEvent) result.Y = 1;
                if (left.DownEvent) result.X = -1;
                if (right.DownEvent) result.X = 1;

                return result;
            }
        }
        public bool plusKeyIsDown { get { return false; } }
        public string directionsName { get { return up.ButtonName + "/" + down.ButtonName + " " + left.ButtonName + "/" + right.ButtonName; } }
        public InputSourceType inputSource { get { return up.inputSource; } }
        public SpriteName Icon { get { return up.Icon; } }

        public int ControllerIndex
        {
            get { return up.ControllerIndex; }
            set { up.ControllerIndex = value; down.ControllerIndex = value; left.ControllerIndex = value; right.ControllerIndex = value; }
        }

        public void ListIcons(List<SpriteName> list, out SpriteName plusKeyIcon, bool includeAlternative)
        {
            list.Add(up.Icon); list.Add(left.Icon); list.Add(down.Icon); list.Add(right.Icon);
            plusKeyIcon = SpriteName.NO_IMAGE;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)DirectionalMapType.FourButtons);
            up.write(w);
            down.write(w);
            left.write(w);
            right.write(w);
        }
        public void read(System.IO.BinaryReader r)
        {
            up = MapRead.Button(r);
            down = MapRead.Button(r);
            left = MapRead.Button(r);
            right = MapRead.Button(r);
        }
    }

    struct DirectionalXboxMap : IDirectionalMap
    {
        int controllerIx;
        ThumbStickType stick;
        bool invertY;

        public DirectionalXboxMap(ThumbStickType stick, bool invertY, int controllerIx)
        {
            this.controllerIx = controllerIx;
            this.stick = stick;
            this.invertY = invertY;
        }

        public Vector2 direction
        {
            get
            {
                Vector2 result = Input.XInput.Instance(controllerIx).JoyStickValue(stick).Direction;
                if (invertY) result.Y = -result.Y;

                return result;
            }
        }
        public Vector2 directionAndTime { get { return direction * Ref.DeltaTimeMs; } }
        public IntVector2 stepping { get { return Input.XInput.Instance(controllerIx).JoyStickValue(stick).Stepping; } }
        public bool plusKeyIsDown { get { return false; } }

        public string directionsName { get { return stick.ToString() + (stick == ThumbStickType.D ? "-Pad" : " Stick") + (invertY ? " (inv)" : ""); } }
        public InputSourceType inputSource { get { return InputSourceType.XController; } }
        public int ControllerIndex { get { return controllerIx; } set { controllerIx = value; } }

        public SpriteName Icon
        {
            get
            {
                switch (stick)
                {
                    default: return SpriteName.LeftStick;
                    case ThumbStickType.Right: return SpriteName.RightStick;
                    case ThumbStickType.D: return SpriteName.Dpad;
                }
            }
        }

        public void ListIcons(List<SpriteName> list, out SpriteName plusKeyIcon, bool includeAlternative)
        {
            list.Add(Icon);
            plusKeyIcon = SpriteName.NO_IMAGE;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)DirectionalMapType.Xbox);
            w.Write((byte)stick);
            w.Write(invertY);
        }
        public void read(System.IO.BinaryReader r)
        {
            stick = (ThumbStickType)r.ReadByte();
            invertY = r.ReadBoolean();
        }
    }

    //struct DirectionalGenericDualAxesMap : IDirectionalMap
    //{
    //    /* Properties */
    //    public Vector2 direction
    //    {
    //        get
    //        {
    //            Vector2 result = SharpDXInput.GetFloatAxes(x, y, controllerIx);
    //            if (Math.Abs(result.X) < ControlSense.AnalogBuffert)
    //                result.X = 0;
    //            if (Math.Abs(result.Y) < ControlSense.AnalogBuffert)
    //                result.Y = 0;
    //            else if (invertY)
    //                result.Y = -result.Y;
    //            return result;
    //        }
    //    }
    //    public Vector2 directionAndTime { get { return direction * Ref.DeltaTimeMs; } }
    //    public IntVector2 stepping { get { return new IntVector2(direction); } }
    //    public bool plusKeyIsDown { get { return false; } }

    //    public string directionsName { get { return "Dual(" + x.ToString() + ", " + y.ToString() + ")"; } }

    //    /* Fields */
    //    GenericControllerAxis x;
    //    GenericControllerAxis y;
    //    int controllerIx;
    //    bool invertY;

    //    /* Constructors */
    //    public DirectionalGenericDualAxesMap(GenericControllerAxis x, GenericControllerAxis y, bool invertY, int controllerIx)
    //    {
    //        this.x = x;
    //        this.y = y;
    //        this.controllerIx = controllerIx;
    //        this.invertY = invertY;
    //    }
    //    public SpriteName Icon { get { return SpriteName.RightStick; } }

    //    public void ListIcons(List<SpriteName> list, out SpriteName plusKeyIcon, bool includeAlternative)
    //    {
    //        list.Add(Icon);
    //        plusKeyIcon = SpriteName.NO_IMAGE;
    //    }

    //    public PlayerInputSource inputSource { get { return PlayerInputSource.GenericController; } }
    //    public int ControllerIndex { get { return controllerIx; } set { controllerIx = value; } }
    //    /* Methods */
    //    public void write(System.IO.BinaryWriter w)
    //    {
    //        w.Write((byte)DirectionalMapType.GenericDualAxes);
    //        w.Write((byte)x);
    //        w.Write((byte)y);
    //        w.Write(invertY);
    //    }
    //    public void read(System.IO.BinaryReader r)
    //    {
    //        x = (GenericControllerAxis)r.ReadByte();
    //        y = (GenericControllerAxis)r.ReadByte();
    //        invertY = r.ReadBoolean();
    //    }
    //}

    //struct DirectionalGenericDpadMap : IDirectionalMap
    //{
    //    /* Properties */
    //    public Vector2 direction { get { return stepping.Vec; } }
    //    public Vector2 directionAndTime { get { return direction * Ref.DeltaTimeMs; } }
    //    public IntVector2 stepping
    //    {
    //        get
    //        {
    //            IntVector2 result = SharpDXInput.GetDpad(axis, controllerIx);
    //            if (invertY)
    //                result.Y = -result.Y;
    //            return result;
    //        }
    //    }
    //    public bool plusKeyIsDown { get { return false; } }
    //    public string directionsName { get { return "D-pad " + ((int)(axis - GenericControllerAxis.PointOfViewControllers0 + 1)).ToString(); } }

    //    /* Fields */
    //    bool invertY;
    //    GenericControllerAxis axis;
    //    int controllerIx;

    //    /* Constructors */
    //    public DirectionalGenericDpadMap(GenericControllerAxis axis, bool invertY, int controllerIx)
    //    {
    //        this.axis = axis;
    //        this.invertY = invertY;
    //        this.controllerIx = controllerIx;
    //    }
    //    public PlayerInputSource inputSource { get { return PlayerInputSource.GenericController; } }
    //    public int ControllerIndex { get { return controllerIx; } set { controllerIx = value; } }

    //    public SpriteName Icon { get { return SpriteName.Dpad; } }
    //    public void ListIcons(List<SpriteName> list, out SpriteName plusKeyIcon, bool includeAlternative)
    //    {
    //        list.Add(Icon);
    //        plusKeyIcon = SpriteName.NO_IMAGE;
    //    }
    //    /* Methods */
    //    public void write(System.IO.BinaryWriter w)
    //    {
    //        w.Write((byte)DirectionalMapType.GenericDpad);
    //        w.Write((byte)axis);
    //        w.Write(invertY);
    //    }
    //    public void read(System.IO.BinaryReader r)
    //    {
    //        axis = (GenericControllerAxis)r.ReadByte();
    //        invertY = r.ReadBoolean();
    //    }
    //}

    struct DirectionalXboxTriggerMap : IDirectionalMap
    {
        int controllerIx;

        public DirectionalXboxTriggerMap(int controllerIx)
        {
            this.controllerIx = (int)controllerIx;
        }

        public Vector2 direction
        {
            get
            {
                Vector2 result = Vector2.Zero;
                result.Y = Input.XInput.Instance(controllerIx).RightTrigger - Input.XInput.Instance(controllerIx).LeftTrigger;
                return result;
            }
        }
        public Vector2 directionAndTime { get { return direction * Ref.DeltaTimeMs; } }
        public IntVector2 stepping { get { return new IntVector2(0, direction.Length() > 0.05f ? 1 : 0); } }
        public bool plusKeyIsDown { get { return false; } }
        public string directionsName { get { return "Triggers"; } }

        public InputSourceType inputSource { get { return InputSourceType.XController; } }
        public int ControllerIndex { get { return controllerIx; } set { controllerIx = value; } }

        public SpriteName Icon { get { return SpriteName.ButtonRT; } }

        public void ListIcons(List<SpriteName> list, out SpriteName plusKeyIcon, bool includeAlternative)
        {
            list.Add(SpriteName.ButtonLT); list.Add(SpriteName.ButtonRT);
            plusKeyIcon = SpriteName.NO_IMAGE;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)DirectionalMapType.XboxTriggers);
        }
        public void read(System.IO.BinaryReader r) { }
    }

    struct DirectionalMouseMap : IDirectionalMap
    {
        public Vector2 direction
        {
            get
            {
                return VectorExt.SafeNormalizeV2(Input.Mouse.MoveDistance);
            }
        }
        public Vector2 directionAndTime { get { return Input.Mouse.MoveDistance; } }
        public IntVector2 stepping
        {
            get
            {
                return new IntVector2(0, Input.Mouse.ScrollValue);
            }
        }
        public bool plusKeyIsDown { get { return false; } }
        public string directionsName { get { return "Mouse Move"; } }

        public SpriteName Icon { get { return SpriteName.MouseAllDir; } }

        public void ListIcons(List<SpriteName> list, out SpriteName plusKeyIcon, bool includeAlternative)
        {
            list.Add(SpriteName.MouseAllDir);
            plusKeyIcon = SpriteName.NO_IMAGE;
        }

        public InputSourceType inputSource { get { return InputSourceType.Mouse; } }
        public int ControllerIndex { get { return -1; } set { } }
        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)DirectionalMapType.MouseMove);
        }
        public void read(System.IO.BinaryReader r) { }
    }

    struct DirectionalMouseScrollMap : IDirectionalMap
    {
        public Vector2 direction
        {
            get
            {
                return new Vector2(0f, lib.ToLeftRight(-Mouse.ScrollValue));
            }
        }
        public Vector2 directionAndTime { get { return direction * Ref.DeltaTimeMs; } }
        public IntVector2 stepping
        {
            get
            {
                return new IntVector2(0, lib.ToLeftRight(-Input.Mouse.ScrollValue));
            }
        }
        public bool plusKeyIsDown { get { return false; } }
        public string directionsName { get { return "Scroll Wheel"; } }

        public InputSourceType inputSource { get { return InputSourceType.Mouse; } }
        public int ControllerIndex { get { return -1; } set { } }

        public SpriteName Icon { get { return SpriteName.MouseScroll; } }

        public void ListIcons(List<SpriteName> list, out SpriteName plusKeyIcon, bool includeAlternative)
        {
            list.Add(Icon);
            plusKeyIcon = SpriteName.NO_IMAGE;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)DirectionalMapType.MouseScroll);
        }
        public void read(System.IO.BinaryReader r) { }
    }
}
