#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Steamworks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Input;
using Valve.Steamworks;

namespace VikingEngine.SteamWrapping
{
    class ControllerActionSet
    {
        /* Fields */
        public ControllerActionSetType type;
        public ulong handle;

        DigitalActionCollection digital;
        AnalogActionCollection analog;

        /* Constructors */
        public ControllerActionSet(ControllerActionSetType setName,
            DigitalActionCollection digital,
            AnalogActionCollection analog)
        {
            this.type = setName;
            handle = 0;

            this.digital = digital;
            this.analog = analog;
        }

        /* Methods */
        public void Init()
        {
            handle = SteamAPI.SteamController().GetActionSetHandle(type.ToString());

            foreach (var a in digital.actions)
            {
                a.Init();
            }

            foreach (var a in analog.actions)
            {
                a.Init();
            }
        }

        public void Update(ulong controllerHandle)
        {
            SteamAPI.SteamController().ActivateActionSet(controllerHandle, handle);

            foreach (var a in digital.actions)
            {
                a.Update(handle, controllerHandle);
            }

            foreach (var a in analog.actions)
            {
                a.Update(handle, controllerHandle);
            }
        }

        public DigitalControllerAction GetAction(ButtonActionType actionName)
        {
            foreach (var a in digital.actions)
            {
                if (a.actionName == actionName)
                {
                    return a;
                }
            }

            return null;
        }

        public AnalogControllerAction GetAction(DirActionType actionName)
        {
            foreach (var a in analog.actions)
            {
                if (a.actionName == actionName)
                {
                    return a;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// This class's only purpose is to ease creation of an array.
    /// </summary>
    class DigitalActionCollection
    {
        /* Fields */
        public DigitalControllerAction[] actions;

        /* Constructors */
        public DigitalActionCollection(params ButtonActionType[] actions)
        {
            int N = actions.Length;
            this.actions = new DigitalControllerAction[N];
            for (int i = 0; i < N; ++i)
            {
                this.actions[i] = new DigitalControllerAction(actions[i]);
            }
            actions = null;
        }
    }

    /// <summary>
    /// This class's only purpose is to ease creation of an array.
    /// </summary>
    class AnalogActionCollection
    {
        /* Fields */
        public AnalogControllerAction[] actions;

        /* Constructors */
        public AnalogActionCollection(params DirActionType[] actions)
        {
            int N = actions.Length;
            this.actions = new AnalogControllerAction[N];
            for (int i = 0; i < N; ++i)
            {
                this.actions[i] = new AnalogControllerAction(actions[i]);
            }
            actions = null;
        }
    }

    class DigitalControllerAction
    {
        /* Fields */
        public ButtonActionType actionName;
        public ulong handle;

        public EControllerActionOrigin[] origins;
        public int currentlyUsedOrigins;

        ControllerDigitalActionData_t state;
        ControllerDigitalActionData_t prevState;

        /* Constructors */
        public DigitalControllerAction(ButtonActionType actionName)
        {
            this.actionName = actionName;
            handle = 0;

            origins = new EControllerActionOrigin[SteamGamepad.STEAM_CONTROLLER_MAX_ORIGINS];
            for (int i = 0; i < SteamGamepad.STEAM_CONTROLLER_MAX_ORIGINS; ++i)
            {
                origins[i] = EControllerActionOrigin.k_EControllerActionOrigin_None;
            }
            currentlyUsedOrigins = 0;

            state = new ControllerDigitalActionData_t();
            prevState = new ControllerDigitalActionData_t();
        }

        /* Methods */
        public void Init()
        {
            handle = SteamAPI.SteamController().GetDigitalActionHandle(actionName.ToString());
        }

        public void Update(ulong actionSetHandle, ulong controllerHandle)
        {
            prevState = state;
            state = SteamAPI.SteamController().GetDigitalActionData(controllerHandle, handle);
            currentlyUsedOrigins = SteamAPI.SteamController().GetDigitalActionOrigins(controllerHandle, actionSetHandle, handle, origins);
        }

        public bool Get()
        {
            return state.bActive != 0 && state.bState != 0;
        }

        public bool GetDown()
        {
            return state.bActive != 0 && prevState.bActive != 0 &&
                   state.bState != 0  && prevState.bState == 0;
        }

        public bool GetUp()
        {
            return state.bActive != 0 && prevState.bActive != 0 && 
                   state.bState == 0 && prevState.bState != 0;
        }
    }

    class AnalogControllerAction
    {
        /* Fields */
        public DirActionType actionName;
        public ulong handle;

        public EControllerActionOrigin[] origins;
        public int currentlyUsedOrigins;

        ControllerAnalogActionData_t state;

        /* Constructors */
        public AnalogControllerAction(DirActionType actionName)
        {
            this.actionName = actionName;
            handle = 0;

            origins = new EControllerActionOrigin[SteamGamepad.STEAM_CONTROLLER_MAX_ORIGINS];
            for (int i = 0; i < SteamGamepad.STEAM_CONTROLLER_MAX_ORIGINS; ++i)
            {
                origins[i] = EControllerActionOrigin.k_EControllerActionOrigin_None;
            }
            currentlyUsedOrigins = 0;

            state = new ControllerAnalogActionData_t();
        }

        /* Methods */
        public void Init()
        {
            handle = SteamAPI.SteamController().GetAnalogActionHandle(actionName.ToString());
        }

        public void Update(ulong actionSetHandle, ulong controllerHandle)
        {
            state = SteamAPI.SteamController().GetAnalogActionData(controllerHandle, handle);
            currentlyUsedOrigins = SteamAPI.SteamController().GetDigitalActionOrigins(controllerHandle, actionSetHandle, handle, origins);
        }

        public Vector2 GetAnalogState()
        {
            if (state.bActive == 0)
            {
                return Vector2.Zero;
            }

            return new Vector2(state.x, -state.y);
        }

        public EControllerSourceMode GetAnalogMode()
        {
            return state.eMode;
        }
    }

    struct ControllerOrigin
    {
        /* Fields */
        public SpriteName texture;
        public string name;

        /* Constructors */
        public ControllerOrigin(EControllerActionOrigin origin)
        {
            switch (origin)
            {
                case EControllerActionOrigin.k_EControllerActionOrigin_None:
                texture = SpriteName.ButtonDisabledCross;
                name = "None";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_A:
                texture = SpriteName.ButtonA;
                name = "A";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_B:
                texture = SpriteName.ButtonB;
                name = "B";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_X:
                texture = SpriteName.ButtonX;
                name = "X";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_Y:
                texture = SpriteName.ButtonY;
                name = "Y";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftBumper:
                texture = SpriteName.ButtonLB;
                name = "Left Shoulder";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightBumper:
                texture = SpriteName.ButtonRB;
                name = "Right Shoulder";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftGrip:
                texture = SpriteName.ButtonLG;
                name = "Left Grip";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightGrip:
                texture = SpriteName.ButtonRG;
                name = "Right Grip";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_Start:
                texture = SpriteName.ButtonSTART;
                name = "Start";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_Back:
                texture = SpriteName.ButtonBACK;
                name = "Back";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftPad_Touch:
                texture = SpriteName.TouchSurface1;
                name = "Touch";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftPad_Swipe:
                texture = SpriteName.TouchSurface1;
                name = "Swipe";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftPad_Click:
                texture = SpriteName.TouchSurface1;
                name = "Click";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftPad_DPadNorth:
                texture = SpriteName.DpadUp;
                name = "Left Pad Dpad North";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftPad_DPadSouth:
                texture = SpriteName.DpadDown;
                name = "Left Pad Dpad South";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftPad_DPadWest:
                texture = SpriteName.DpadLeft;
                name = "Left Pad Dpad West";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftPad_DPadEast:
                texture = SpriteName.DpadRight;
                name = "Left Pad Dpad East";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_Touch:
                texture = SpriteName.TouchSurface2;
                name = "Right Pad Touch";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_Swipe:
                texture = SpriteName.TouchSurface2;
                name = "Right Pad Swipe";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_Click:
                texture = SpriteName.TouchSurface2;
                name = "Right Pad Click";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_DPadNorth:
                texture = SpriteName.TouchSurface2UpDown;
                name = "Right Pad Dpad North";
                break;


                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_DPadSouth:
                texture = SpriteName.TouchSurface2UpDown;
                name = "Right Pad Dpad South";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_DPadWest:
                texture = SpriteName.TouchSurface2LeftRight;
                name = "Right Pad Dpad West";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_DPadEast:
                texture = SpriteName.TouchSurface2LeftRight;
                name = "Right Pad Dpad East";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftTrigger_Pull:
                texture = SpriteName.ButtonLT;
                name = "Left Trigger Pull";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftTrigger_Click:
                texture = SpriteName.ButtonLT;
                name = "Left Trigger Click";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightTrigger_Pull:
                texture = SpriteName.ButtonRT;
                name = "Right Trigger Pull";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightTrigger_Click:
                texture = SpriteName.ButtonRT;
                name = "Right Trigger Click";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_Move:
                texture = SpriteName.LeftStick;
                name = "Left Stick Move";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_Click:
                texture = SpriteName.LSClick;
                name = "Left Stick Click";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_DPadNorth:
                texture = SpriteName.LeftStick_UD;
                name = "Left Stick Dpad North";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_DPadSouth:
                texture = SpriteName.LeftStick_UD;
                name = "Left Stick Dpad South";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_DPadWest:
                texture = SpriteName.LeftStick_LR;
                name = "Left Stick Dpad West";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_DPadEast:
                texture = SpriteName.LeftStick_LR;
                name = "Left Stick Dpad East";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_Gyro_Move:
                texture = SpriteName.GyroMove;
                name = "Gyro Move";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_Gyro_Pitch:
                texture = SpriteName.GyroPitch;
                name = "Gyro Pitch";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_Gyro_Yaw:
                texture = SpriteName.GyroYaw;
                name = "Gyro Yaw";
                break;

                case EControllerActionOrigin.k_EControllerActionOrigin_Gyro_Roll:
                texture = SpriteName.GyroRoll;
                name = "Gyro Roll";
                break;

                default:
                throw new NotImplementedException();
            }
        }
    }

    class SteamGamepad // Name chosen to not conflict with Steam API's SteamController.
    {
        /* Constants */
        public const int STEAM_CONTROLLER_MAX_COUNT = 16;  // actually a define in Steam's C++ sdk, so this could change
        public const int STEAM_CONTROLLER_MAX_ORIGINS = 8; // ^

        /* Static Fields */
        public static int controllerCount;
        public static ControllerOrigin[] origins;

        static SteamGamepad[] controllers;
        static ControllerActionSet[] actionSets;

        /* Static Methods */
        public static void Initialize(ControllerActionSet[] sets)
        {
            // Init
            SteamAPI.SteamController().Init();

            // Get controllers
            ulong[] controllerHandles = new ulong[STEAM_CONTROLLER_MAX_COUNT];
            controllerCount = SteamAPI.SteamController().GetConnectedControllers(controllerHandles);
            controllers = new SteamGamepad[controllerCount];
            for (int i = 0; i < controllerCount; ++i)
            {
                controllers[i] = new SteamGamepad(controllerHandles[i]);
            }

            // Setup actions
            actionSets = sets;
            foreach (var set in actionSets)
            {
                set.Init();
            }

            // Setup origins
            origins = new ControllerOrigin[(int)EControllerActionOrigin.k_EControllerActionOrigin_Count];
            for (int i = 0; i < origins.Length; ++i)
            {
                origins[i] = new ControllerOrigin((EControllerActionOrigin)i);
            }
        }

        public static void Update()
        {
            // Check for new controller count
            ulong[] controllerHandles = new ulong[STEAM_CONTROLLER_MAX_COUNT];
            controllerCount = SteamAPI.SteamController().GetConnectedControllers(controllerHandles);

            if (controllerCount > controllers.Length)
            {
                controllers = new SteamGamepad[controllerCount];
                for (int i = 0; i < controllerCount; ++i)
                {
                    controllers[i] = new SteamGamepad(controllerHandles[i]);
                }
            }

            foreach (SteamGamepad controller in controllers)
            {
                if (controller.actionSet != null)
                {
                    controller.actionSet.Update(controller.handle);
                }
            }
        }

        public static SteamGamepad GetController(int ix)
        {
            if (ix < controllerCount)
                return controllers[ix];
            return null;
        }

        public static void Shutdown()
        {
            SteamAPI.SteamController().Shutdown();
        }

        public static void SetActionSet_All(ControllerActionSetType actionSet)
        {
            for (int i = 0; i < controllerCount; ++i)
            {
                var pad = GetController(i);
                if (pad != null)
                {
                    pad.setActionSet(ControllerActionSetType.MenuControls);
                }
            }
        }

        /* Fields */
        ControllerActionSetType currentActionSetName;
        ControllerActionSet actionSet;
        ulong handle;

        /* Constructors */
        public SteamGamepad(ulong handle)
        {
            this.handle = handle;
            setActionSet(ControllerActionSetType.InGameControls);
        }

        public void ShowBindingGUI()
        {
            if (!SteamAPI.SteamController().ShowBindingPanel(handle))
            {
                // Overlay is disabled / unavailable or user is not running Big Picture mode.
            }
        }

        public void setActionSet(ControllerActionSetType actionSet)
        {
            currentActionSetName = actionSet;
            foreach (var set in actionSets)
            {
                if (set.type == actionSet)
                    this.actionSet = set;
            }
        }

        public void Vibrate(ESteamControllerPad pad, ushort duration)
        {
            SteamAPI.SteamController().TriggerHapticPulse(handle, pad, duration);
        }

        public bool IsDown(ButtonActionType action)
        {
            var a = actionSet.GetAction(action);
            if (a == null) return false;
            return a.Get();
        }
        public bool DownEvent(ButtonActionType action)
        {
            var a = actionSet.GetAction(action);
            if (a == null) return false;
            return a.GetDown();
        }
        public bool UpEvent(ButtonActionType action)
        {
            var a = actionSet.GetAction(action);
            if (a == null) return false;
            return a.GetUp();
        }
        public Vector2 GetAnalogState(DirActionType action)
        {
            var a = actionSet.GetAction(action);
            if (a == null) return Vector2.Zero;
            return a.GetAnalogState();
        }
        public EControllerSourceMode GetAnalogMode(DirActionType action)
              
        {
            var a = actionSet.GetAction(action);
            if (a == null) return EControllerSourceMode.k_EControllerSourceMode_None;
            return a.GetAnalogMode();
        }
       
        public int GetIcons(ButtonActionType actionName, List<SpriteName> modify)
        {
            var action = actionSet.GetAction(actionName);
            if (action == null)
            {
                modify.Add(SpriteName.ButtonDisabledCross);
                return 1;
            }

            EControllerActionOrigin[] usedOriginsIndices = action.origins;
            int count = action.currentlyUsedOrigins;

            modify.Clear();
            for (int i = 0; i < count; ++i)
            {
                modify.Add(origins[(int)usedOriginsIndices[i]].texture);
            }

            return count;
        }

        public int GetIcons(DirActionType actionName, List<SpriteName> modify)
        {
            var action = actionSet.GetAction(actionName);
            EControllerActionOrigin[] usedOriginsIndices = action.origins;
            int count = action.currentlyUsedOrigins;

            modify.Clear();
            for (int i = 0; i < count; ++i)
            {
                modify.Add(origins[(int)usedOriginsIndices[i]].texture);
            }

            return count;
        }

        public SpriteName GetIcon(ButtonActionType actionName)
        {
            var action = actionSet.GetAction(actionName);
            EControllerActionOrigin[] usedOriginsIndices = action.origins;

            return origins[(int)usedOriginsIndices[0]].texture;
        }

        public string GetNames(ButtonActionType actionName)
        {
            var action = actionSet.GetAction(actionName);
            EControllerActionOrigin[] usedOriginsIndices = action.origins;
            int count = action.currentlyUsedOrigins;

            string result = "";
            for (int i = 0; i < count - 1; ++i)
            {
                result += origins[(int)usedOriginsIndices[i]].name + ", ";
            }
            result += origins[count - 1].name;
            return result;
        }
        
        
        public string GetNames(DirActionType actionName)
        {
            var action = actionSet.GetAction(actionName);
            EControllerActionOrigin[] usedOriginsIndices = action.origins;
            int count = action.currentlyUsedOrigins;

            string result = "";
            for (int i = 0; i < count - 1; ++i)
            {
                result += origins[(int)usedOriginsIndices[i]].name + ", ";
            }
            result += origins[count - 1].name;
            return result;
        }
    }
}
#endif