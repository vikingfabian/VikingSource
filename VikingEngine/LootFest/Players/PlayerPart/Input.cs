using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Engine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
//xna
using VikingEngine.Input;

namespace VikingEngine.LootFest.Players
{
    partial class Player
    {
        public Engine.PlayerData localPData;
        public InputMap inputMap;
        Time cinematicMode = 0;

        void updateInput()
        {
            //if (Engine.XGuide.OverridePlayerInput)
            //    return;
            //if (Engine.Input.KeyBoardInput != null)
            //    return;

            if (controlLock <= 0)
            {
                if (menuSystem != null)
                {
                    updateMenuInput();
                }
                else if (cinematicMode.MilliSeconds > 0)
                {
                    cinematicMode.CountDown();
                    hero.updateMovement();
                    hero.UpdateAllChildObjects();

                    return;
                }
                else
                {
                    updatePlayInput();
                }
            }
            else
            {
                controlLock -= Ref.DeltaTimeMs;
                if (controlLock <= 0)
                {
                    hero.CheckIfUnderGround();
                }

            }

            if (Input.Keyboard.KeyDownEvent(Keys.D8))
            {
                if (debugTerrainGenDisplay == null)
                    debugTerrainGenDisplay = new Display.DebugTerrainGenDisplay(this);
                else
                {
                    debugTerrainGenDisplay.DeleteMe();
                    debugTerrainGenDisplay = null;
                }
            }
        }


        public void startCinematicMode(Time maxTime)
        {
            cinematicMode = maxTime;
        }
        public void endCinematicMode()
        {
            cinematicMode = 0;
        }

        
        void updatePlayInput()
        {
            //if (!InLoadingScene || PlatformSettings.Debug == BuildDebugLevel.DeveloperDebug_1)
            hero.UpdateInput();

            //if (pData.inputMap.Interact.DownEvent)//Controller.KeyDownEvent(Buttons.Y))
            //{
            //    interact();
            //}

            //if (pData.inputMap.DownEvent(ButtonActionType.GamePauseMenu) ||
            //   pData.inputMap.openCloseMenuKeyInput())//Controller.KeyDownEvent(Buttons.Start))
            if (inputMap.menuInput.openCloseInputEvent())
            {
                //if (DebugSett.MenuVer2)
                //{
                mainMenu();
                //}
                //else
                //{
                //    openPage(MenuPageName.MainMenu);
                //}
            }
            if (inputMap.chat.DownEvent)//pData.inputMap/.DownEvent(ButtonActionType.GameChat))
            {
                //XGuide.BeginKeyBoardInput(new KeyboardInputValues("", "", PlayerIndex, 0, chatInput));
            }

            updateCamInput();

            if (inputMap.firstPersonToggle.DownEvent)//.DownEvent(ButtonActionType.GamePlayCamMode))//Controller.KeyDownEvent(Buttons.DPadUp))
            {
                changeCamera();
            }
            if (inputMap.editorInput.OpenClose.DownEvent)//pData.inputMap.DownEvent(ButtonActionType.GameEditorMode))//Controller.KeyDownEvent(Buttons.DPadDown))
            {
                if (PlatformSettings.DebugOptions)
                {
                    //if (Controller.IsButtonDown(Buttons.LeftTrigger))
                    //{
                    BeginCreationMode();
                    //}
                    //else
                    //{
                    //    openMiniMap(true);
                    //}
                }
            }

            statusDisplay.updateInput();

        }
        void updateMenuInput()
        {
            if (menuSystem != null)
            {
                //inputMap.SetGameStateLayout(SteamWrapping.GamestateButtonLayout.MenuControls);
                if (!inputMap.inputSource.HasKeyBoard)
                    updateCamInput();

                if (menuSystem.update() || inputMap.menuInput.openCloseInputEvent())//.DownEvent(ButtonActionType.MenuReturnToGame))
                {
                    CloseMenu();
                }
            }

            //if (menu != null)
            //{
            //    //JoyStickValue leftStick = Controller.JoyStickValue(Stick.Left);
            //    menu.MoveSelection(pData.inputMap.MenuMovement);

            //    if (inputMap.inputSource != Input.PlayerInputSource.KeyboardMouse)
            //        updateCamInput();

            //    if (ValueDialogue != null)
            //    {
            //        ValueDialogue.UpdateInput();
            //    }
            //    else if (menu != null)
            //    {
            //        if (pData.inputMap.MenuClick.DownEvent)//Controller.KeyDownEvent(Buttons.A))
            //            menu.Click(Index, numBUTTON.A);
            //        //if (Controller.KeyDownEvent(Buttons.X))
            //        //    menu.Click(Controller.Index, numBUTTON.X);
            //        //if (Controller.KeyDownEvent(Buttons.Y))
            //        //    menu.Click(Controller.Index, numBUTTON.Y);


            //        if (pData.inputMap.MenuBack.DownEvent)//Controller.KeyDownEvent(Buttons.B))
            //        {
            //            menu.Back(Index);
                        
            //            return;
            //        }

            //        if (hero.Alive)
            //        {
            //            if (pData.inputMap.Escape.DownEvent)//Controller.KeyDownEvent(Buttons.Start))
            //            {
            //                CloseMenu();
            //                Music.SoundManager.PlayFlatSound(LoadedSound.MenuBack);
            //                return;
            //            }
            //        }
            //        //if (Controller.KeyDownEvent(Buttons.LeftShoulder))
            //        //{
            //        //    nextPage(false);
            //        //}
            //        //else if (Controller.KeyDownEvent(Buttons.RightShoulder))
            //        //{
            //        //    nextPage(true);
            //        //}
            //    }

            //}
        }

        public override void BeginInputDialogueEvent(KeyboardInputValues keyInputValues)
        {
            //if (menu == null)
            //{
            //    //openPage(MenuPageName.MainMenu);
            //    beginOpenMenu();
            //}
            //inputDialogue = new HUD.InputDialogue(menu, keyInputValues);
            //if (voxelDesigner !=  null)
            //    inputDialogue.blockInput = 1;
        }
        //public override void BlockMenuValueOptionEvent(int link, double newValue, int playerIx)
        //{
        //    if (inEditor)
        //        voxelDesigner.BlockMenuValueOptionEvent(link, newValue, playerIx);
        //    else
        //    {
        //        switch ((Link)link)
        //        {

        //            case Link.CamLookSpeedX:
        //                Storage.CamSpeedX = (byte)newValue;
        //                //pData.view.Camera.Settings = Storage.CameraSettings;
        //                refreshCamSettings();
        //                break;
        //            case Link.CamLookSpeedY:
        //                Storage.CamSpeedY = (byte)newValue;
        //                refreshCamSettings();
        //                //pData.view.Camera.Settings = Storage.CameraSettings;
        //                break;
        //            case Link.CamFOV:
        //                if (pData.view.camType == Graphics.CameraType.TopView)
        //                {
        //                    Storage.CamTopViewFOV = (float)newValue;
        //                }
        //                else
        //                {
        //                    Storage.CamFirstPersonFOV = (float)newValue;
        //                }
        //                refreshCamSettings();
        //                //pData.view.Camera.Settings = Storage.CameraSettings;
        //                break;
        //            //case Link.ChunkUpdateRadius:
        //            //    Ref.pc_gamesett.ChunkLoadRadius = (int)newValue;
        //            //    break;
        //        }
        //        // SettingsChanged();
        //    }
        //}

        //public override void BlockMenuCheckboxEvent(int link, bool value, int playerIx)
        //{
        //    if (inEditor)//mode == PlayerMode.Creation)
        //        voxelDesigner.BlockMenuCheckboxEvent(link, value, playerIx);
        //    else
        //    {
        //        switch ((Link)link)
        //        {

        //            //case Link.CB_UseCape:
        //            //    settings.UseCape = value;
        //            //    hero.UpdateAppearance(false);
        //            //    settingsChanged = true;
        //            //    appearanceChanged = true;
        //            //    break;
        //            case Link.CB_HorizontalSplit:
        //                Engine.Draw.horizontalSplit = value;
        //                LfRef.gamestate.UpdateSplitScreen();
        //                break;

        //            case Link.CB_ViewPlayerNames:
        //                Storage.ViewPlayerNames = value;
        //                break;
        //            case Link.CB_InvertFPview:
        //                Storage.CamInvertY = value;
        //                refreshCamSettings();
        //                //pData.view.Camera.Settings = Storage.CameraSettings;
        //                break;

        //        }
        //        //SettingsChanged();
        //    }
        //}
        //public override float BlockMenuFloatValue(int playerIx, float value, bool get, int valueLink)
        //{
        //    if (voxelDesigner != null)
        //        return voxelDesigner.BlockMenuFloatValue(playerIx, value, get, valueLink);

        //    switch ((Link)valueLink)
        //    {

        //        case Link.ChunkUpdateRadius:
        //            if (!get)
        //            {
        //                Ref.pc_gamesett.ChunkLoadRadius = (int)value;
        //                LfRef.worldMap.RefreshChunkLoadRadius();
        //            }
        //            return Ref.pc_gamesett.ChunkLoadRadius;
        //        case Link.FrameRate:
        //            if (!get)
        //            {
        //                Ref.pc_gamesett.FrameRate = (int)value;
        //                Engine.Update.SetFrameRate(Ref.pc_gamesett.FrameRate);
        //            }
        //            return Ref.pc_gamesett.FrameRate;


        //    }

        //    return 0;
        //}
        //public override bool BlockMenuBoolValue(int playerIx, bool value, bool get, int valueIx)
        //{
        //    if (voxelDesigner != null)
        //    {
        //        return voxelDesigner.BlockMenuBoolValue(playerIx, value, get, valueIx);
        //    }
        //    else
        //    {
        //        //if (!get)
        //        //    SettingsChanged();

        //        switch ((ValueLink)valueIx)
        //        {

        //            //case ValueLink.bAutoEquip:
        //            //    return getSetBool(ref Settings.AutoEquip, value, get);

        //            case ValueLink.bHorizontalSplit:
        //                bool result = getSetBool(ref Engine.Draw.horizontalSplit, value, get);
        //                if (!get)
        //                {
        //                    LfRef.gamestate.UpdateSplitScreen();
        //                }
        //                return result;
        //            case ValueLink.bInvertLook:
        //                return getSetBool(ref Storage.CamInvertY, value, get);
        //            case ValueLink.bApperanceCape:
        //                value = getSetBool(ref Storage.UseCape, value, get);
        //                if (!get)
        //                {
        //                    hero.UpdateAppearance(false);
        //                    appearanceChanged = true;
        //                }
        //                return value;
        //            case ValueLink.bFullScreen:
        //                if (!get)
        //                {
        //                    Engine.Screen.PcTargetFullScreen = value;
        //                }
        //                return Engine.Screen.PcTargetFullScreen;
        //            //case ValueLink.bAutoSave:
        //            //    return getSetBool(ref Settings.AutoSave, value, get);
        //            //case ValueLink.bTutorial:
        //            //    return getSetBool(ref Settings.ViewTutorial, value, get);
        //            //case ValueLink.bViewGamerTags:
        //            //    return getSetBool(ref Settings.ViewPlayerNames, value, get);
        //            //case ValueLink.bQuickInput:
        //            //    return getSetBool(ref Engine.XGuide.UseQuickInputDialogue, value, get);
        //            //case ValueLink.bSpawnAtPricateHome:
        //            //    return getSetBool(ref Settings.SpawnAtPrivateHome, value, get);
        //        }

        //        return base.BlockMenuBoolValue(playerIx, value, get, valueIx);
        //    }
        //}

        public override void TextInputEvent(string input, object tag)
        {
            if (voxelDesigner != null && voxelDesigner.WaitingForTextInput)
            {
                voxelDesigner.TextInputEvent(input, tag);
                //return true;
            }
            else
            {
                input = Engine.LoadContent.CheckCharsSafety(input, LoadedFont.Regular);
                input = TextLib.CheckBadLanguage(input);
                if (input.Length > Players.Player.ChatMessageMaxChars)
                {
                    input = input.Remove(Players.Player.ChatMessageMaxChars);
                }

                if (Ref.netSession.InMultiplayerSession)
                {
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.Chat,
                        Network.PacketReliability.Chat, PlayerIndex);
                    w.Write(input);
                }


                //print on your own screen
                const string You = "You";
                HUD.ChatMessageData m = new HUD.ChatMessageData(input, You);
                LfRef.gamestate.AddChat(m, true);
                PrintChat(m, true, LoadedSound.Dialogue_Neutral);
                CloseMenu();
            }

            deleteInputDialogue();
            //return false;
        }

        void chatInput(int user, string result, int index)
        {
            if (result != null)
            {
                result = Engine.LoadContent.CheckCharsSafety(result, LoadedFont.Regular);
                //result = TextLib.CheckBadLanguage(result);
                if (result.Length > Players.Player.ChatMessageMaxChars)
                {
                    result = result.Remove(Players.Player.ChatMessageMaxChars);
                }

                if (Ref.netSession.HasInternet)
                {
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.Chat,
                        Network.PacketReliability.ReliableLasy, PlayerIndex);
                    w.Write(result);
                }


                //print on your own screen
                const string You = "You";
                HUD.ChatMessageData m = new HUD.ChatMessageData(result, You);
                LfRef.gamestate.AddChat(m, true);
                PrintChat(m, true, LoadedSound.Dialogue_Neutral);
            }
        }

        public override void TextInputCancelEvent(int playerIndex)
        {
            deleteInputDialogue();
        }
        void deleteInputDialogue()
        {
            //if (inputDialogue != null)
            //{
            //    inputDialogue.DeleteMe();
            //    inputDialogue = null;
            //    if (voxelDesigner != null)
            //    {
            //        CloseMenu();
            //        mode = PlayerMode.Creation;
            //    }
            //    else
            //    {
            //        if (mFile == null)
            //        {
            //            exitCurrentMode();
            //        }
            //        else
            //            OpenMenuFile();
            //    }
            //}
        }

//        void menuButtonInput(ButtonValue e)
//        {
//            if (ValueDialogue != null)
//            {
//                ValueDialogue.Button_Event(e);
//            }
//            else
//            {
//                if (menu != null && e.KeyDown)
//                {
//                    switch (e.Button)
//                    {
//                        //case numBUTTON.A:
//                        default:
//                            if (e.Button == numBUTTON.A || e.Button == numBUTTON.X || e.Button == numBUTTON.Y)
//                                menu.Click(e.PlayerIx, e.Button);
//                            break;
//                        case numBUTTON.B:
//                            //if (hero.InteractingWith == null)
//                            //{
//                                menu.Back(e.PlayerIx);
//                            //}
//                            //else
//                            //{
//                            //    if (hero.InteractingWith.InteractType != GO.GameObjectType.SpeakDialogue ||
//                            //        menu.FileProperties.ParentPage < 0)
//                            //    {
//                            //        CloseMenu();
//                            //    }
//                            //    else
//                            //    {
//                            //        //if (menu.FileProperties.ParentPage > 0)
//                            //        //{
//                            //        //    hero.InteractingWith.Interact_LinkClick(new HUD.Link(menu.FileProperties.ParentPage), this.hero, menu);
//                            //        //}
//                            //        //else
//                            //        //    OpenMenuFile(hero.InteractingWith.Interact_TalkMenu(hero));
//                            //    }
//                            //    Music.SoundManager.PlayFlatSound(LoadedSound.MenuBack);
//                            //}
//                            break;
//                        case numBUTTON.Start:
//                            CloseMenu();
//                            Music.SoundManager.PlayFlatSound(LoadedSound.MenuBack);
//                            break;
//#if !CMODE
//                        case numBUTTON.LB:
//                            nextPage(false);

//                            break;
//                        case numBUTTON.RB:
//                            nextPage(true);
//                            break;
//#endif
//                    }
//                }
//            }
//        }
    }
}
