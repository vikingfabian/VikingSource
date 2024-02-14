//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SharpDX.DirectInput;
//using Microsoft.Xna.Framework;
////using System.Windows.Forms;

//namespace VikingEngine.Input
//{   
//    class GenericController
//    {
//        /* Fields */
//        Joystick device;
//        JoystickState state;
//        JoystickState prevState;
//        //bool isPlaystation3Controller;
//        public DeviceInstance instance;
//        public ControllerType type = ControllerType.Unknown;
//        public int deviceIndex;

//        public GenericThumbStick leftStick, rightStick;

//        /* Constructors */
//        public GenericController(DirectInput di, DeviceInstance instance, int deviceIndex)
//        {
//            this.deviceIndex = deviceIndex;
//            this.instance = instance;
//             device = new Joystick(di, instance.InstanceGuid);
//            device.Properties.AxisMode = DeviceAxisMode.Absolute;
//            device.Acquire();

//            // device.Capabilities.ButtonCount was found to be incorrect on one device, so let's not use it.

//            state = new JoystickState();
//            prevState = new JoystickState();
//            device.GetCurrentState(ref state);

//            type = SharpDXInput.GetType(instance);

//            leftStick = new GenericThumbStick(new DirectionalGenericDualAxesMap(
//                GenericControllerAxis.X, GenericControllerAxis.Y, false, deviceIndex));
//            rightStick = new GenericThumbStick(new DirectionalGenericDualAxesMap(
//                GenericControllerAxis.Z, GenericControllerAxis.RotationZ, false, deviceIndex));
            
//        }

       

//        /* Novelty Methods */
//        public void Update()
//        {
//            // copy state to previous state
//            prevState.AccelerationSliders[0] = state.AccelerationSliders[0];
//            prevState.AccelerationSliders[1] = state.AccelerationSliders[1];
//            prevState.AccelerationX = state.AccelerationX;
//            prevState.AccelerationY = state.AccelerationY;
//            prevState.AccelerationZ = state.AccelerationZ;
//            prevState.AngularAccelerationX = state.AngularAccelerationX;
//            prevState.AngularAccelerationY = state.AngularAccelerationY;
//            prevState.AngularAccelerationZ = state.AngularAccelerationZ;
//            prevState.AngularVelocityX = state.AngularVelocityX;
//            prevState.AngularVelocityY = state.AngularVelocityY;
//            prevState.AngularVelocityZ = state.AngularVelocityZ;
//            for (int j = 0; j < (int)GenericControllerButton.NUM; ++j)
//            {
//                prevState.Buttons[j] = state.Buttons[j];
//            }
//            prevState.ForceSliders[0] = state.ForceSliders[0];
//            prevState.ForceSliders[1] = state.ForceSliders[1];
//            prevState.ForceX = state.ForceX;
//            prevState.ForceY = state.ForceY;
//            prevState.ForceZ = state.ForceZ;
//            prevState.PointOfViewControllers[0] = state.PointOfViewControllers[0];
//            prevState.PointOfViewControllers[1] = state.PointOfViewControllers[1];
//            prevState.PointOfViewControllers[2] = state.PointOfViewControllers[2];
//            prevState.PointOfViewControllers[3] = state.PointOfViewControllers[3];
//            prevState.RotationX = state.RotationX;
//            prevState.RotationY = state.RotationY;
//            prevState.RotationZ = state.RotationZ;
//            prevState.Sliders[0] = state.Sliders[0];
//            prevState.Sliders[1] = state.Sliders[1];
//            prevState.TorqueX = state.TorqueX;
//            prevState.TorqueY = state.TorqueY;
//            prevState.TorqueZ = state.TorqueZ;
//            prevState.VelocitySliders[0] = state.VelocitySliders[0];
//            prevState.VelocitySliders[1] = state.VelocitySliders[1];
//            prevState.VelocityX = state.VelocityX;
//            prevState.VelocityY = state.VelocityY;
//            prevState.VelocityZ = state.VelocityZ;
//            prevState.X = state.X;
//            prevState.Y = state.Y;
//            prevState.Z = state.Z;

//            // get new state
//            try
//            {
//                device.GetCurrentState(ref state);
//            }
//            catch (Exception e)
//            {
//                Debug.LogError("Controller disconnected: " + e.Message);
//                SharpDXInput.controllers.Remove(this);
//            }
//        }

//        public bool IsDown(GenericControllerButton btn)
//        {
//            return state.Buttons[(int)btn];
//        }

//        public List<GenericControllerButton> listAllButtonDown()
//        {
//            List<GenericControllerButton> result = new List<GenericControllerButton>();
//            for (GenericControllerButton btn = 0; btn < GenericControllerButton.NUM; ++btn)
//            {
//                if (state.Buttons[(int)btn])
//                {
//                    result.Add(btn);
//                }
//            }

//            return result;
//        }

//        public bool DownEvent(GenericControllerButton btn)
//        {
//            return state.Buttons[(int)btn] && !prevState.Buttons[(int)btn];
//        }

//        public bool UpEvent(GenericControllerButton btn)
//        {
//            return !state.Buttons[(int)btn] && prevState.Buttons[(int)btn];
//        }

//        public int GetAxis(GenericControllerAxis axis)
//        {
//            switch (axis)
//            {
//                case GenericControllerAxis.AccelerationSliders0:
//                    return state.AccelerationSliders[0];
//                case GenericControllerAxis.AccelerationSliders1:
//                    return state.AccelerationSliders[1];

//                case GenericControllerAxis.AccelerationX:
//                    return state.AccelerationX;
//                case GenericControllerAxis.AccelerationY:
//                    return state.AccelerationY;
//                case GenericControllerAxis.AccelerationZ:
//                    return state.AccelerationZ;

//                case GenericControllerAxis.AngularAccelerationX:
//                    return state.AngularAccelerationX;
//                case GenericControllerAxis.AngularAccelerationY:
//                    return state.AngularAccelerationY;
//                case GenericControllerAxis.AngularAccelerationZ:
//                    return state.AngularAccelerationZ;

//                case GenericControllerAxis.AngularVelocityX:
//                    return state.AngularVelocityX;
//                case GenericControllerAxis.AngularVelocityY:
//                    return state.AngularVelocityY;
//                case GenericControllerAxis.AngularVelocityZ:
//                    return state.AngularVelocityZ;

//                case GenericControllerAxis.ForceSliders0:
//                    return state.ForceSliders[0];
//                case GenericControllerAxis.ForceSliders1:
//                    return state.ForceSliders[1];

//                case GenericControllerAxis.ForceX:
//                    return state.ForceX;
//                case GenericControllerAxis.ForceY:
//                    return state.ForceY;
//                case GenericControllerAxis.ForceZ:
//                    return state.ForceZ;

//                case GenericControllerAxis.PointOfViewControllers0:
//                    return state.PointOfViewControllers[0];
//                case GenericControllerAxis.PointOfViewControllers1:
//                    return state.PointOfViewControllers[1];
//                case GenericControllerAxis.PointOfViewControllers2:
//                    return state.PointOfViewControllers[2];
//                case GenericControllerAxis.PointOfViewControllers3:
//                    return state.PointOfViewControllers[3];

//                case GenericControllerAxis.RotationX:
//                    return state.RotationX;
//                case GenericControllerAxis.RotationY:
//                    return state.RotationY;
//                case GenericControllerAxis.RotationZ:
//                    return state.RotationZ;

//                case GenericControllerAxis.Sliders0:
//                    return state.Sliders[0];
//                case GenericControllerAxis.Sliders1:
//                    return state.Sliders[1];

//                case GenericControllerAxis.TorqueX:
//                    return state.TorqueX;
//                case GenericControllerAxis.TorqueY:
//                    return state.TorqueY;
//                case GenericControllerAxis.TorqueZ:
//                    return state.TorqueZ;

//                case GenericControllerAxis.VelocitySliders0:
//                    return state.VelocitySliders[0];
//                case GenericControllerAxis.VelocitySliders1:
//                    return state.VelocitySliders[1];

//                case GenericControllerAxis.VelocityX:
//                    return state.VelocityX;
//                case GenericControllerAxis.VelocityY:
//                    return state.VelocityY;
//                case GenericControllerAxis.VelocityZ:
//                    return state.VelocityZ;

//                case GenericControllerAxis.X:
//                    return state.X;
//                case GenericControllerAxis.Y:
//                    return state.Y;
//                case GenericControllerAxis.Z:
//                    return state.Z;

//                default:
//                    return 0;
//            }
//        }

//        public override string ToString()
//        {
//            string name = type == ControllerType.Unknown ? instance.InstanceName : type.ToString();

//            return name + "(" + TextLib.IndexToString(deviceIndex) + ")";
//        }
//    }

//    class GenericThumbStick
//    {
//        DirectionalGenericDualAxesMap map;
//        StepBuffert stepX = new StepBuffert();
//        StepBuffert stepY = new StepBuffert();
//        public IntVector2 stepping;

//        public GenericThumbStick(DirectionalGenericDualAxesMap map)
//        {
//            this.map = map;
//        }

//        public IntVector2 UpdateStepBuffer()
//        {
//            Vector2 dir = map.direction;

//            if (dir != Vector2.Zero)
//            {
//                lib.DoNothing();
//            }
//            stepping.X = stepX.Update(dir.X);
//            stepping.Y = stepY.Update(dir.Y);

//            return stepping;
//        }
//    }
   
//}
