//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SharpDX.DirectInput;
//using Microsoft.Xna.Framework;

//namespace VikingEngine.Input
//{
//    static class SharpDXInput
//    {
//        public const bool UseSharpDX = false;
//        /* Static properties */
//        public static int ControllerCount { get { return controllers.Count; } }

//        /* Static fields */
//        public static List<GenericController> doublettes;
//        public static List<GenericController> controllers;
//        static DirectInput directInput;
//        public static SharpDXError error = null;
//        //public static string loadingFailure = null; 

//        /* Static methods */
//        public static void Initialize()
//        {
//            controllers = new List<GenericController>();
//            doublettes = new List<GenericController>();
//            directInput = new DirectInput();

//            UpdateControllerList();
//        }

//        public static void UpdateControllerList()
//        {
//            if (UseSharpDX)
//            {
//                try
//                {
//                    DeviceInstance[] devices = directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly).ToArray();

//                    if (devices.Length != controllers.Count)
//                    {
//                        controllers.Clear();
//                        doublettes.Clear();

//                        for (int i = 0; i < devices.Length; ++i)
//                        {
//                            DeviceInstance instance = devices[i];

//                            Debug.Log("Found controller: " + instance.InstanceName);

//                            GenericController c = new GenericController(directInput, instance, i);

//                            if (c.type == ControllerType.XboxOne ||
//                                c.type == ControllerType.PS4 ||
//                                c.type == ControllerType.Xbox360)
//                            {
//                                doublettes.Add(c);
//                            }
//                            else
//                            {
//                                controllers.Add(c);
//                            }
//                        }
//                    }

//                    //throw new Exception("testing");
//                }
//                catch (Exception e)
//                {
//                    controllers.Clear();
//                    error = new SharpDXError(e.Message);
//                }
//            }
//        }

//        public static string DevicesDebugString()
//        {
//            string result = "";
//            DeviceInstance[] devices = directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly).ToArray();

//            for (int i = 0; i < devices.Length; ++i)
//            {
//                result += devices[i].InstanceName + " (" + GetType(devices[i]).ToString() + ") " + "{" + devices[i].ProductGuid + "}" + Environment.NewLine + Environment.NewLine;
//            }

//            return result;
//        }


//        //fixed byte letter[256];

//        public static void UpdateAll()
//        {
//            for (int i = 0; i < controllers.Count; ++i)
//            {
//                GenericController controller = controllers[i];
//                if (controller != null)
//                    controller.Update();
//            }
//        }

//        public static void UpdateDoublettes()
//        {
//            foreach (var m in doublettes)
//            {
//                m.Update();
//            }
//        }



//        /// <summary>
//        /// Convert axis values from POV controllers and hat-switches to IntVector2 dpads.
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        static IntVector2 ToDpad(int value)
//        {
//            IntVector2 result = IntVector2.Zero;

//            // Values are provided in centigrades, with 36000 being one lap.
//            if (value == -1)
//            {
//                return result; // center
//            }
//            else if (value < 2250)
//            {
//                result = new IntVector2(0, -1); // up
//            }
//            else if (value < 6750)
//            {
//                result = new IntVector2(1, -1); // up right
//            }
//            else if (value < 11250)
//            {
//                result = new IntVector2(1, 0); // right
//            }
//            else if (value < 15750)
//            {
//                result = new IntVector2(1, 1); // right down
//            }
//            else if (value < 20250)
//            {
//                result = new IntVector2(0, 1); // down
//            }
//            else if (value < 24750)
//            {
//                result = new IntVector2(-1, 1); // down left
//            }
//            else if (value < 29250)
//            {
//                result = new IntVector2(-1, 0); // left
//            }
//            else if (value < 33750)
//            {
//                result = new IntVector2(-1, -1); // left up
//            }

//            return result;
//        }

//        /// <summary>
//        /// Converts normal axis values to floats in [-1, 1]
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        static float ToFloat(int value)
//        {
//            return (value - PublicConstants.UShortHalf) / (float)PublicConstants.UShortHalf;
//        }

//        static bool IsPOVAxis(GenericControllerAxis axis)
//        {
//            return axis >= GenericControllerAxis.PointOfViewControllers0 &&
//                   axis <= GenericControllerAxis.PointOfViewControllers3;
//        }

//        static int GetAxis(GenericControllerAxis axis, int controllerIndex)
//        {
//            //int ix = (int)controllerIndex;
//            if (axis == GenericControllerAxis.NUM)
//            {
//                return 0;
//            }

//            if (controllers != null && controllers.Count > controllerIndex && controllers[controllerIndex] != null)
//            {
//                return controllers[controllerIndex].GetAxis(axis);
//            }

//            if (IsPOVAxis(axis))
//                return -1; // maps to 0,0 with ToDpad

//            return ushort.MaxValue / 2; // maps to 0 with ToFloat
//        }

//        public static float GetFloatAxis(GenericControllerAxis axis, int controllerIndex)
//        {
//            if (IsPOVAxis(axis))
//                throw new NotImplementedException();
//            return ToFloat(GetAxis(axis, controllerIndex));
//        }

//        public static Vector2 GetFloatAxes(GenericControllerAxis x, GenericControllerAxis y, int controllerIndex)
//        {
//            return new Vector2(ToFloat(GetAxis(x, controllerIndex)), ToFloat(GetAxis(y, controllerIndex)));
//        }

//        public static IntVector2 GetDpad(GenericControllerAxis x, int controllerIndex)
//        {
//            if (!IsPOVAxis(x))
//                throw new NotImplementedException();

//            return ToDpad(GetAxis(x, controllerIndex));
//        }

//        public static bool IsDown(GenericControllerButton btn, int controllerIndex)
//        {
//            int ix = controllerIndex;
//            if (controllers != null && controllers.Count > ix && controllers[ix] != null)
//            {
//                return controllers[ix].IsDown(btn);
//            }
//            return false;
//        }



//        public static bool DownEvent(GenericControllerButton btn, int controllerIndex)
//        {
//            if (controllers != null && controllers.Count > controllerIndex && controllers[controllerIndex] != null)
//            {
//                return controllers[controllerIndex].DownEvent(btn);
//            }
//            return false;
//        }

//        public static bool UpEvent(GenericControllerButton btn, int controllerIndex)
//        {
//            int ix = (int)controllerIndex;
//            if (controllers != null && controllers.Count > ix && controllers[ix] != null)
//            {
//                return controllers[ix].UpEvent(btn);
//            }
//            return false;
//        }


//        public static ControllerType GetType(DeviceInstance instance)
//        {
//            if (instance.ProductGuid == new Guid("05c4054c-0000-0000-0000-504944564944"))
//            {
//                return ControllerType.PS4;
//            }
//            else if (instance.InstanceName.ToLower().Contains("xbox 360")) //id == new Guid("028e045e-0000-0000-0000-504944564944"))
//            {
//                return ControllerType.Xbox360;
//            }
//            else if (instance.InstanceName.ToLower().Contains("xbox"))//id == new Guid("02ff045e-0000-0000-0000-504944564944"))
//            {
//                return ControllerType.XboxOne;
//            }
//            else
//            {
//                return ControllerType.Unknown;
//            }
//        }
//    }

//    class SharpDXError
//    {
//        string errorMessage;

//        public SharpDXError(string errorMessage)
//        {
//            this.errorMessage = errorMessage;
//        }

//        public void view()
//        {
//            //MessageBox.Show("Your controller may not work. Error message: " + errorMessage, "SharpDX failure",
//            //    MessageBoxButtons.OK,
//            //    MessageBoxIcon.Error,
//            //    MessageBoxDefaultButton.Button1);
//        }
//    }

//    // Add 48 to these value to get the JoystickOffset equivalent
//    enum GenericControllerButton
//    {
//        B0,
//        B1,
//        B2,
//        B3,
//        B4,
//        B5,
//        B6,
//        B7,
//        B8,
//        B9,
//        B10,
//        B11,
//        B12,
//        B13,
//        B14,
//        B15,
//        B16,
//        B17,
//        B18,
//        B19,
//        B20,
//        B21,
//        B22,
//        B23,
//        B24,
//        B25,
//        B26,
//        B27,
//        B28,
//        B29,
//        B30,
//        B31,
//        B32,
//        B33,
//        B34,
//        B35,
//        B36,
//        B37,
//        B38,
//        B39,
//        B40,
//        B41,
//        B42,
//        B43,
//        B44,
//        B45,
//        B46,
//        B47,
//        B48,
//        B49,
//        B50,
//        B51,
//        B52,
//        B53,
//        B54,
//        B55,
//        B56,
//        B57,
//        B58,
//        B59,
//        B60,
//        B61,
//        B62,
//        B63,
//        B64,
//        B65,
//        B66,
//        B67,
//        B68,
//        B69,
//        B70,
//        B71,
//        B72,
//        B73,
//        B74,
//        B75,
//        B76,
//        B77,
//        B78,
//        B79,
//        B80,
//        B81,
//        B82,
//        B83,
//        B84,
//        B85,
//        B86,
//        B87,
//        B88,
//        B89,
//        B90,
//        B91,
//        B92,
//        B93,
//        B94,
//        B95,
//        B96,
//        B97,
//        B98,
//        B99,
//        B100,
//        B101,
//        B102,
//        B103,
//        B104,
//        B105,
//        B106,
//        B107,
//        B108,
//        B109,
//        B110,
//        B111,
//        B112,
//        B113,
//        B114,
//        B115,
//        B116,
//        B117,
//        B118,
//        B119,
//        B120,
//        B121,
//        B122,
//        B123,
//        B124,
//        B125,
//        B126,
//        B127,
//        NUM
//    }

//    // Multiply these values by 4 to get the JoystickOffset equivalent
//    enum GenericControllerAxis
//    {
//        X = 0,
//        Y,
//        Z,
//        RotationX,
//        RotationY,
//        RotationZ,
//        Sliders0,
//        Sliders1,
//        PointOfViewControllers0,
//        PointOfViewControllers1,
//        PointOfViewControllers2,
//        PointOfViewControllers3,
//        NUM,
//        /*
//         * JoystickOffset contains buttons in [48, 176).
//         * Divided by four the interval is this one.
//         * [12, 44)
//         */
//        START2_MINUS1 = 43,
//        VelocityX = 44,
//        VelocityY,
//        VelocityZ,
//        AngularVelocityX,
//        AngularVelocityY,
//        AngularVelocityZ,
//        VelocitySliders0,
//        VelocitySliders1,
//        AccelerationX,
//        AccelerationY,
//        AccelerationZ,
//        AngularAccelerationX,
//        AngularAccelerationY,
//        AngularAccelerationZ,
//        AccelerationSliders0,
//        AccelerationSliders1,
//        ForceX,
//        ForceY,
//        ForceZ,
//        TorqueX,
//        TorqueY,
//        TorqueZ,
//        ForceSliders0,
//        ForceSliders1,
//        NUM2
//    }
//    enum ControllerType
//    {
//        Unknown,
//        PS4,
//        XboxOne,
//        Xbox360,
//        XInput,
//    }
//}
