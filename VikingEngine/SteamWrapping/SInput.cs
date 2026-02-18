using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using VikingEngine.HUD.RichBox;
using VikingEngine.Input;
using VikingEngine.ToGG.HeroQuest.HeroStrategy;

namespace VikingEngine.SteamWrapping
{
    class SInput
    {
        public static bool InputLayerChange = false;
        protected Callback<SteamInputConfigurationLoaded_t> m_InputConfigLoaded;

        public InputHandle_t[] controllerHandles = new InputHandle_t[Constants.STEAM_INPUT_MAX_COUNT];
        public SteamControllerInstance[] controllers = new SteamControllerInstance[Constants.STEAM_INPUT_MAX_COUNT];

        InputActionSetHandle_t[] actionSets;

        InputDigitalActionHandle_t[] digitalHandles;
        InputAnalogActionHandle_t[] analogHandles;
        bool isReady = false;
        bool lostFocus = false;

        private InputActionSetHandle_t[][] _previousLayers;
        private InputActionSetHandle_t[] _currentLayersBuffer = new InputActionSetHandle_t[16];
        public SInput()
        {
            SteamInput.Init(false);

            _previousLayers = new InputActionSetHandle_t[Constants.STEAM_INPUT_MAX_COUNT][];
            for (int i = 0; i < Constants.STEAM_INPUT_MAX_COUNT; i++)
            {
                _previousLayers[i] = new InputActionSetHandle_t[16];
            }
            m_InputConfigLoaded = Callback<SteamInputConfigurationLoaded_t>.Create(OnInputConfigLoaded);
        }
        private void OnInputConfigLoaded(SteamInputConfigurationLoaded_t pCallback)
        {
            if (!isReady)
            {
                for (int i = 0; i < controllers.Length; i++)
                {
                    controllers[i] = new SteamControllerInstance(i);
                }
            }

            actionSets = new InputActionSetHandle_t[(int)SteamActionSet.NUM];
            for (SteamActionSet aset = 0; aset < SteamActionSet.NUM; aset++)
            {
                actionSets[(int)aset] = SteamInput.GetActionSetHandle(aset.ToString());
            }

            // Fetch handles
            digitalHandles = new InputDigitalActionHandle_t[(int)SteamDigitalAction.NUM];
            for (SteamDigitalAction da = 0; da < SteamDigitalAction.NUM; da++)
            {
                digitalHandles[(int)da] = SteamInput.GetDigitalActionHandle(da.ToString());
            }

            analogHandles = new InputAnalogActionHandle_t[(int)SteamAnalogAction.NUM];
            for (SteamAnalogAction aa = 0; aa < SteamAnalogAction.NUM; aa++)
            {
                analogHandles[(int)aa] = SteamInput.GetAnalogActionHandle(aa.ToString());
            }
            isReady = true;
        }
        EInputActionOrigin[] origins = new EInputActionOrigin[Constants.STEAM_INPUT_MAX_ORIGINS];
        InputActionSetHandle_t[] activeLayers = new InputActionSetHandle_t[16];
        
        public SpriteName actionIcon(int controllerIx, SteamActionSet actionSet, SteamDigitalAction actionType)
        {
            if (isReady)
            {
                InputHandle_t controller = controllerHandles[controllerIx];
                int count = 0;

                // 1. Get all currently active layers for this controller
                int layercount = SteamInput.GetActiveActionSetLayers(controller, activeLayers);

                // 2. Check layers first, iterating backwards (top of the stack to the bottom)
                for (int i = layercount - 1; i >= 0; i--)
                {
                    count = SteamInput.GetDigitalActionOrigins(
                        controller,
                        activeLayers[i],
                        digitalHandles[(int)actionType],
                        origins
                    );

                    // If we found a mapping in this layer, stop searching
                    if (count > 0)
                    {
                        break;
                    }
                }

                // 3. If no layers had a mapping (or no layers were active), fall back to the base Action Set
                if (count == 0)
                {
                    count = SteamInput.GetDigitalActionOrigins(
                        controller,
                        actionSets[(int)actionSet],
                        digitalHandles[(int)actionType],
                        origins
                    );
                }

                // 4. If it's still 0, the action is entirely unmapped for the current input state
                if (count == 0)
                {
                    return SpriteName.NO_IMAGE;
                }

                // Return the primary bound origin (index 0)
                return actionOriginIcon(origins[0]);
            }
            else
            {
                return SpriteName.NO_IMAGE;
            }
        }

        public SpriteName actionIcon(int controllerIx, SteamActionSet actionSet, SteamAnalogAction actionType)
        {
            InputHandle_t controller = controllerHandles[controllerIx];
            int count = 0;

            // 1. Get all currently active layers for this controller
            int layercount = SteamInput.GetActiveActionSetLayers(controller, activeLayers);

            // 2. Check layers first, iterating backwards (top of the stack to the bottom)
            for (int i = layercount - 1; i >= 0; i--)
            {
                count = SteamInput.GetAnalogActionOrigins(
                    controller,
                    activeLayers[i],
                    analogHandles[(int)actionType],
                    origins
                );

                // If we found a mapping in this layer, stop searching
                if (count > 0)
                {
                    break;
                }
            }

            // 3. If no layers had a mapping (or no layers were active), fall back to the base Action Set
            if (count == 0)
            {
                count = SteamInput.GetAnalogActionOrigins(
                    controller,
                    actionSets[(int)actionSet],
                    analogHandles[(int)actionType],
                    origins
                );
            }

            // 4. If it's still 0, the action is entirely unmapped for the current input state
            if (count == 0)
            {
                return SpriteName.NO_IMAGE;
            }

            // Return the primary bound origin (index 0)
            return actionOriginIcon(origins[0]);
        }

        SpriteName actionOriginIcon(EInputActionOrigin origin)
        {
            // Switch on the physical button to decide which of YOUR textures to draw
            switch (origin)
            {
                // ========================================================================
                // FACE BUTTONS (Mapped by Label)
                // ========================================================================
                // Note: Switch A is Physically Right (East), Xbox A is Physically Down (South).
                // Steam Input handles the logical input swapping, you just show the label.

                case EInputActionOrigin.k_EInputActionOrigin_Switch_A:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_A:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_A:
                    return SpriteName.ButtonA;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_B:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_B:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_B:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_B:
                    return SpriteName.ButtonB;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_X:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_X:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_X:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_X:
                    return SpriteName.ButtonX;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_Y:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_Y:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_Y:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_Y:
                    return SpriteName.ButtonY;


                // ========================================================================
                // BUMPERS & TRIGGERS
                // ========================================================================

                // Switch L / Xbox LB
                case EInputActionOrigin.k_EInputActionOrigin_Switch_LeftBumper:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_LeftBumper:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_LeftBumper:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_L1:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_LeftBumper:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_LeftBumper:
                    return SpriteName.ButtonLB;

                // Switch R / Xbox RB
                case EInputActionOrigin.k_EInputActionOrigin_Switch_RightBumper:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_RightBumper:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_RightBumper:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_R1:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_RightBumper:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_RightBumper:
                    return SpriteName.ButtonRB;

                // Xbox LT
                
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_LeftTrigger_Pull:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_LeftTrigger_Pull:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_L2:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_LeftTrigger_Pull:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_LeftTrigger_Pull:
                    return SpriteName.ButtonLT;

                // Xbox RT
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_RightTrigger_Pull:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_RightTrigger_Pull:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_R2:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_RightTrigger_Pull:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_RightTrigger_Pull:
                    return SpriteName.ButtonRT;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_LeftTrigger_Pull:
                    return SpriteName.ButtonLZ;
                case EInputActionOrigin.k_EInputActionOrigin_Switch_RightTrigger_Pull:
                    return SpriteName.ButtonRZ;


                // ========================================================================
                // STICKS & CLICKS
                // ========================================================================

                case EInputActionOrigin.k_EInputActionOrigin_Switch_LeftStick_Move:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_LeftStick_Move:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_LeftStick_Move:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_LeftStick_Move:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_LeftStick_Move:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_LeftStick_Move:
                    return SpriteName.LeftStick;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_LeftStick_Click:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_LeftStick_Click:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_LeftStick_Click:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_L3:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_LeftStick_Click:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_LeftStick_Click:
                    return SpriteName.LSClick;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_RightStick_Move:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_RightStick_Move:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_RightStick_Move:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_RightStick_Move:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_RightStick_Move:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_RightStick_Move:
                    return SpriteName.RightStick;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_RightStick_Click:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_RightStick_Click:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_RightStick_Click:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_R3:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_RightStick_Click:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_RightStick_Click:
                    return SpriteName.RSClick;


                // ========================================================================
                // D-PAD
                // ========================================================================

                case EInputActionOrigin.k_EInputActionOrigin_Switch_DPad_North:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_DPad_North:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_DPad_North:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_DPad_North:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_DPad_North:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_DPad_North:
                    return SpriteName.DpadUp;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_DPad_South:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_DPad_South:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_DPad_South:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_DPad_South:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_DPad_South:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_DPad_South:
                    return SpriteName.DpadDown;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_DPad_West:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_DPad_West:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_DPad_West:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_DPad_West:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_DPad_West:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_DPad_West:
                    return SpriteName.DpadLeft;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_DPad_East:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_DPad_East:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_DPad_East:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_DPad_East:
                case EInputActionOrigin.k_EInputActionOrigin_PS4_DPad_East:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_DPad_East:
                    return SpriteName.DpadRight;


                // ========================================================================
                // MENU / SYSTEM BUTTONS
                // ========================================================================

                // Start / Menu / Plus (+)
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_Menu:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_Start:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_Menu:
                    return SpriteName.ButtonMENU;

                case EInputActionOrigin.k_EInputActionOrigin_PS4_Options:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_Option:
                    return SpriteName.PsButtonOptions;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_Plus:
                    return SpriteName.ButtonSwitchPlus;

                // Back / View / Minus (-)

                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_View:
                case EInputActionOrigin.k_EInputActionOrigin_XBox360_Back:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_View:
                    return SpriteName.ButtonVIEW;

                case EInputActionOrigin.k_EInputActionOrigin_PS4_Share:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_Create:
                    return SpriteName.PsButtonShare;

                case EInputActionOrigin.k_EInputActionOrigin_Switch_Minus:
                    return SpriteName.ButtonSwitchMinus;

                

                // ========================================================================
                // PLAYSTATION SPECIFICS (Unique shapes)
                // ========================================================================

                case EInputActionOrigin.k_EInputActionOrigin_PS4_X:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_X:
                    return SpriteName.PsButtonCross;

                case EInputActionOrigin.k_EInputActionOrigin_PS4_Circle:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_Circle:
                    return SpriteName.PsButtonCirkle;

                case EInputActionOrigin.k_EInputActionOrigin_PS4_Triangle:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_Triangle:
                    return SpriteName.PsButtonTriangle;

                case EInputActionOrigin.k_EInputActionOrigin_PS4_Square:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_Square:
                    return SpriteName.PsButtonSquare;

                case EInputActionOrigin.k_EInputActionOrigin_PS4_CenterPad_Touch:
                case EInputActionOrigin.k_EInputActionOrigin_PS5_CenterPad_Touch:
                    return SpriteName.PsTouchPad;


                // ========================================================================
                // STEAM DECK SPECIFICS
                // ========================================================================

                // Gyro
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_Gyro_Move:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_Gyro_Pitch:
                    return SpriteName.GyroPitch;
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_Gyro_Yaw:
                    return SpriteName.GyroYaw;
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_Gyro_Roll:
                    return SpriteName.GyroRoll;

                // Back Grips
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_L4:
                    return SpriteName.ButtonL4;
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_R4:
                    return SpriteName.ButtonR4;
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_L5:
                    return SpriteName.ButtonL5;
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_R5:
                    return SpriteName.ButtonR5;

                case EInputActionOrigin.k_EInputActionOrigin_SteamController_LeftGrip:
                    return SpriteName.ButtonLG;
                case EInputActionOrigin.k_EInputActionOrigin_SteamController_RightGrip:
                    return SpriteName.ButtonRG;

                // Trackpads
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_LeftPad_Touch:
                    return SpriteName.DeckTouchL;

                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_LeftPad_Swipe:
                    return SpriteName.DeckTouchL_Right;

                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_LeftPad_Click:
                    return SpriteName.DeckTouchL_Click;

                case EInputActionOrigin.k_EInputActionOrigin_LenovoLegionGo_LeftPad_DPadWest:
                    return SpriteName.DeckTouchL_Right;
                case EInputActionOrigin.k_EInputActionOrigin_LenovoLegionGo_LeftPad_DPadEast:
                    return SpriteName.DeckTouchL_Left;
                case EInputActionOrigin.k_EInputActionOrigin_LenovoLegionGo_LeftPad_DPadNorth:
                    return SpriteName.DeckTouchL_Up;
                case EInputActionOrigin.k_EInputActionOrigin_LenovoLegionGo_LeftPad_DPadSouth:
                    return SpriteName.DeckTouchL_Down;

                case EInputActionOrigin.k_EInputActionOrigin_LenovoLegionGo_RightPad_DPadWest:
                    return SpriteName.DeckTouchR_Right;
                case EInputActionOrigin.k_EInputActionOrigin_LenovoLegionGo_RightPad_DPadEast:
                    return SpriteName.DeckTouchR_Left;
                case EInputActionOrigin.k_EInputActionOrigin_LenovoLegionGo_RightPad_DPadNorth:
                    return SpriteName.DeckTouchR_Up;
                case EInputActionOrigin.k_EInputActionOrigin_LenovoLegionGo_RightPad_DPadSouth:
                    return SpriteName.DeckTouchR_Down;

                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_LeftTrigger_Click:
                    return SpriteName.ButtonLT;


                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_RightTrigger_Click:
                    return SpriteName.ButtonRT;

                // --- Sticks ---

                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_Share:
                    return SpriteName.PsButtonShare; // Mapping Xbox Share to your generic Share sprite

                // --- Elite Controller Paddles (Grips) ---
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_LeftGrip_Lower:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_LeftGrip_Upper:
                    return SpriteName.ButtonLG; // Left Grip

                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_RightGrip_Lower:
                case EInputActionOrigin.k_EInputActionOrigin_XBoxOne_RightGrip_Upper:
                    return SpriteName.ButtonRG; // Right Grip


                // ========================================================================
                // STEAM DECK
                // ========================================================================



                // Left Pad used as D-Pad (Directional)
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_LeftPad_DPadNorth:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_LeftPad_DPadSouth:
                    return SpriteName.TouchSurface1UpDown; // Visual cue to swipe/press up/down on pad

                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_LeftPad_DPadWest:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_LeftPad_DPadEast:
                    return SpriteName.TouchSurface1LeftRight;


                // Right Pad used as D-Pad
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_RightPad_DPadNorth:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_RightPad_DPadSouth:
                    return SpriteName.TouchSurface2UpDown;

                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_RightPad_DPadWest:
                case EInputActionOrigin.k_EInputActionOrigin_SteamDeck_RightPad_DPadEast:
                    return SpriteName.TouchSurface2LeftRight;

            }
            return SpriteName.MissingImage;
        }

        public void update()
        {
            InputLayerChange = false;

            if (Ref.main.IsActive)
            {
                if (lostFocus)
                {
                    lostFocus = false;
                    SteamInput.RunFrame();
                }

                if (isReady)
                {
                    int count = SteamInput.GetConnectedControllers(controllerHandles);
                    for (int controllerIx = 0; controllerIx < count; controllerIx++)
                    {
                        InputHandle_t controllerHandle = controllerHandles[controllerIx];

                        // Skip disconnected controllers
                        if (controllerHandle.m_InputHandle == 0) continue;

                        //INPUT
                        var ins = controllers[controllerIx];

                        //LAYERS
                        // Get the current layers for this specific controller
                        int layerCount = SteamInput.GetActiveActionSetLayers(controllerHandle, _currentLayersBuffer);
                        if (layerCount != ins.layerCount)
                        {
                            InputLayerChange = true;
                            ins.layerCount = layerCount;                            
                        }

                        SteamInput.ActivateActionSet(controllerHandle, actionSets[(int)ins.actionSet]);

                        for (int daIx = 0; daIx < digitalHandles.Length; daIx++)
                        {
                            ins.digital_isDown_previous[daIx] = ins.digital_isDown_current[daIx];
                            InputDigitalActionData_t actionData = SteamInput.GetDigitalActionData(controllerHandle, digitalHandles[daIx]);
                            ins.digital_isDown_current[daIx] = actionData.bState == 1 && actionData.bActive == 1;
                        }

                        if (ins.muteKeyChange > 0)
                        {
                            ins.muteKeyChange -= Ref.DeltaTimeMs;
                            //Kill all key change events
                            for (int daIx = 0; daIx < digitalHandles.Length; daIx++)
                            {
                                ins.digital_isDown_previous[daIx] = ins.digital_isDown_current[daIx];
                            }
                        }

                        for (int aaIx = 0; aaIx < analogHandles.Length; aaIx++)
                        {
                            InputAnalogActionData_t analogData = SteamInput.GetAnalogActionData(controllerHandle, analogHandles[aaIx]);

                            ins.analog_current[aaIx] = analogData;
                        }
                    }
                }
            }
            else
            {
                lostFocus = true;
            }
        }

        public bool AnyKeyDownEvent()
        {
            if (isReady)
            {
                foreach (var c in controllers)
                {
                    if (c.AnyKeyDownEvent())
                    { 
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsActive(int controllerIx, SteamDigitalAction actionType)
        {
            InputHandle_t controllerHandle = controllerHandles[controllerIx];
            InputDigitalActionData_t actionData = SteamInput.GetDigitalActionData(controllerHandle, digitalHandles[(int)actionType]);
            return actionData.bActive == 1;
        }

        public void ListConneted(List<InputSource> sources)
        {
            int count = SteamInput.GetConnectedControllers(controllerHandles);
            for (int controllerIx = 0; controllerIx < count; controllerIx++)
            {
                var handle = controllerHandles[controllerIx];
                var type = SteamInput.GetInputTypeForHandle(handle);
                sources.Add(new InputSource(InputSourceType.SteamInput, controllerIx, type));

            }
        }

        public void SetActionSet(SteamActionSet actionSet)
        {
            if (isReady)
            {
                foreach (var c in controllers)
                {
                    if (c.actionSet != actionSet)
                    {
                        c.actionSet = actionSet;
                        c.muteKeyChange = 120;
                    }
                }
            }
        }
        public static void UnusedLayerToRichContent(RichBoxContent content)
        {
                content.Add(new RbText("(ALT)", Color.DarkGray));
                content.hspace();           
        }
    }

    enum SteamActionSet
    {
        InGameControls,
        MenuControls,
        EditorControls,
        NUM
    }

    enum SteamDigitalAction
    {
        // InGameControls & Shared
        select,
        order,
        quick_select,
        cancel,
        stop_start,
        open_menu,
        toggle_hud_detail,
        toggle_minimap,
        tab_left,
        tab_right,
        build,
        copy,
        paste,
        gamespeed,
        pause,
        next_city,
        next_army,
        next_war,
        controller_focus,
        controller_faction,
        controller_message,
        zoomInKey,
        zoomOutKey,

        // MenuControls
        close_menu,

        // EditorControls
        editor_draw,
        editor_erase,
        editor_select,
        editor_cancel,
        editor_colorPick,
        editor_undo,
        editor_YmovementToggle,
        editor_tab_left,
        editor_tab_right,

        editor_selection_mirrorX,
        editor_selection_mirrorY,
        editor_selection_rotateCCW,
        editor_selection_rotateCW,
        NUM
    }

    enum SteamAnalogAction
    {
        // InGameControls
        PanCamera,
        CameraStick,
        CameraTilt,
        MoveCursor,

        // MenuControls
        Scroll,

        // EditorControls
        editor_moveXZ,
        editor_cameraXMoveY,
        editor_cameraZoom,

        NUM
    }

    class SteamControllerInstance
    {
        public int index;
        public int layerCount  =0;
        public float muteKeyChange = 0;

        public bool[] digital_isDown_previous;
        public bool[] digital_isDown_current;
        public InputAnalogActionData_t[] analog_current;
        public SteamActionSet actionSet = SteamActionSet.MenuControls;
        

        public SteamControllerInstance(int index)
        {
            this.index = index;
            digital_isDown_previous = new bool[(int)SteamDigitalAction.NUM];
            digital_isDown_current = new bool[(int)SteamDigitalAction.NUM];

            analog_current = new InputAnalogActionData_t[(int)SteamAnalogAction.NUM];
        }

        public bool AnyKeyDownEvent()
        {
            for (int i = 0; i < digital_isDown_current.Length; i++)
            {
                if (digital_isDown_current[i] && !digital_isDown_previous[i])
                {
                    return true;
                }
            }
            return false;
        }
    }

}
