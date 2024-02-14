using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Engine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
//

namespace VikingEngine.LF2.Players
{
    partial class Player
    {
        public Engine.PlayerData localPData;
        public InputMap inputMap;

        //public override void BeginInputDialogueEvent(KeyboardInputValues keyInputValues)
        //{
        //    if (menu == null)
        //    {
        //        //openPage(MenuPageName.MainMenu);
        //        beginOpenMenu();
        //    }
        //    inputDialogue = new HUD.InputDialogue(menu, keyInputValues);
        //    if (voxelDesigner !=  null)
        //        inputDialogue.blockInput = 1;
        //}
        //        public override void BlockMenuValueOptionEvent(int link, double newValue, int playerIx)
        //        {
        //            if (mode == PlayerMode.Creation)
        //                voxelDesigner.BlockMenuValueOptionEvent(link, newValue, playerIx);
        //            else
        //            {
        //                switch ((Link)link)
        //                {
        //#if CMODE
        //                    case Link.RaceNumLaps:
        //                        settings.RaceNumLaps = (byte)newValue;
        //                        break;
        //#endif
        //                    case Link.FPviewHoriSpeed:
        //                        Settings.CameraSettings.SpeedX = (byte)newValue;
        //                        ControllerLink.view.Camera.Settings = Settings.CameraSettings;
        //                        break;
        //                    case Link.FPviewVertiSpeed:
        //                        Settings.CameraSettings.SpeedY = (byte)newValue;
        //                        ControllerLink.view.Camera.Settings = Settings.CameraSettings;
        //                        break;

        //                }
        //                SettingsChanged();
        //            }
        //        }
        //        public override void BlockMenuCheckboxEvent(int link, bool value, int playerIx)
        //        {
        //            if (mode == PlayerMode.Creation)
        //                voxelDesigner.BlockMenuCheckboxEvent(link, value, playerIx);
        //            else
        //            {
        //                switch ((Link)link)
        //                {
        //#if CMODE
        //                    case Link.CB_AutoSave:
        //                        settings.AutoSave = value;
        //                        break;
        //                    case Link.CB_ViewButtonHelp:
        //                        settings.ViewTutorial = value;
        //                        break;
        //                    case Link.CB_ZombieSpawn:
        //                        settings.ZombieHordes = value;
        //                        updateSettings();
        //                        if (!value)
        //                        {
        //                            LfRef.gamestate.ReduceZombieLevel();
        //                        }
        //                        break;
        //                    case Link.RaceAllowFire:
        //                        settings.RaceAllowFire = value;
        //                        break;
        //                    case Link.FlyingRCInvertPitch:
        //                        if (rcToy.RcCategory == GameObjects.Toys.RcCategory.Helicopter)
        //                        {
        //                            settings.HelicopterInvertPitch = value;
        //                        }
        //                        else
        //                        {
        //                            settings.AirPlaneInvertPitch = value;
        //                        }
        //                        break;
        //                    case Link.CB_DefaultBuildPermission:
        //                        settings.DefaulBuildPermission = value;
        //                        break;
        //                    //case Link.CB_RCHelpLine:
        //                    //    settings.FlyingRCHelpLine = value;
        //                    //    //update plane
        //                    //    if (rcToy.FlyingToy)
        //                    //    {
        //                    //        ((GameObjects.Toys.AbsFlyingRCtoy)rcToy).UpdateShowHelpLine();
        //                    //    }
        //                    //    break;
        //                    //case Link.CB_RCHelpStabilize:
        //                    //    settings.FlyingRCStabilize = value;
        //                    //    break;
        //                    //case Link.CB_RCHelpGas:
        //                    //    settings.FlyingRCAutoGas = value;
        //                    //    break;
        //                    case Link.CB_TerrainDamage:
        //                        settings.TerrainDamage = value;
        //                        updateSettings();
        //                        break;
        //#endif
        //                    //case Link.CB_UseCape:
        //                    //    settings.UseCape = value;
        //                    //    hero.UpdateAppearance(false);
        //                    //    settingsChanged = true;
        //                    //    appearanceChanged = true;
        //                    //    break;
        //                    case Link.CB_HorizontalSplit:
        //                        Engine.Draw.horizontalSplit = value;
        //                        LfRef.gamestate.UpdateSplitScreen();
        //                        break;

        //                    case Link.CB_ViewPlayerNames:
        //                        Settings.ViewPlayerNames = value;
        //                        break;
        //                    case Link.CB_InvertFPview:
        //                        Settings.CameraSettings.InvertY = value;
        //                        ControllerLink.view.Camera.Settings = Settings.CameraSettings;
        //                        break;

        //                }
        //                SettingsChanged();
        //            }
        //        }
        //        public override float BlockMenuFloatValue(int playerIx, float value, bool get, int valueLink)
        //        {
        //            if (voxelDesigner != null)
        //                return voxelDesigner.BlockMenuFloatValue(playerIx, value, get, valueLink);
        //            return 0;
        //        }
        //        public override bool BlockMenuBoolValue(int playerIx, bool value, bool get, int valueIx)
        //        {
        //            if (voxelDesigner != null)
        //            {
        //                return voxelDesigner.BlockMenuBoolValue(playerIx, value, get, valueIx);
        //            }
        //            else
        //            {
        //                if (!get)
        //                    SettingsChanged();

        //                switch ((ValueLink)valueIx)
        //                {
        //#if CMODE
        //                    case ValueLink.bFlyingRCInvertPitch:
        //                        bool invert = settings.AirPlaneInvertPitch;
        //                        if (rcToy.RcCategory == GameObjects.Toys.RcCategory.Helicopter)
        //                        {
        //                            return getSetBool(ref settings.HelicopterInvertPitch, value, get);
        //                        }
        //                        else
        //                        {
        //                            return getSetBool(ref settings.AirPlaneInvertPitch, value, get);
        //                        }


        //                    case ValueLink.bDefaultBuildPermission:
        //                        return getSetBool(ref settings.DefaulBuildPermission, value, get);

        //                    case ValueLink.bRChelpLine:
        //                        getSetBool(ref settings.FlyingRCHelpLine, value, get);
        //                        if (!get && rcToy.FlyingToy)
        //                        {
        //                            ((GameObjects.Toys.AbsFlyingRCtoy)rcToy).UpdateShowHelpLine();
        //                        }
        //                        return settings.FlyingRCHelpLine;
        //                    case ValueLink.bRCplaneAutoGas:
        //                        return getSetBool(ref settings.FlyingRCAutoGas, value, get);
        //                    case ValueLink.bRCPlaneStabilizeHelp:
        //                        return getSetBool(ref settings.FlyingRCStabilize, value, get);
        //                    case ValueLink.bTerrainDamage:
        //                        return getSetBool(ref settings.TerrainDamage, value, get);
        //                    case ValueLink.bZombieHorde:
        //                        return getSetBool(ref settings.ZombieHordes, value, get);

        //#else
        //                    case ValueLink.bAutoEquip:
        //                        return getSetBool(ref Settings.AutoEquip, value, get);
        //#endif
        //                    case ValueLink.bHorizontalSplit:
        //                        bool result = getSetBool(ref Engine.Draw.horizontalSplit, value, get);
        //                        if (!get)
        //                        {
        //                            LfRef.gamestate.UpdateSplitScreen();
        //                        }
        //                        return result;
        //                    case ValueLink.bInvertLook:
        //                        return getSetBool(ref Settings.CameraSettings.InvertY, value, get);
        //                    case ValueLink.bApperanceCape:
        //                        value = getSetBool(ref Settings.UseCape, value, get);
        //                        if (!get)
        //                        {
        //                            hero.UpdateAppearance(false);
        //                            appearanceChanged = true;
        //                        }
        //                        return value;
        //                    case ValueLink.bAutoSave:
        //                        return getSetBool(ref Settings.AutoSave, value, get);
        //                    case ValueLink.bTutorial:
        //                        return getSetBool(ref Settings.ViewTutorial, value, get);
        //                    case ValueLink.bViewGamerTags:
        //                        return getSetBool(ref Settings.ViewPlayerNames, value, get);
        //                    case ValueLink.bQuickInput:
        //                        return getSetBool(ref Engine.XGuide.UseQuickInputDialogue, value, get);
        //                    case ValueLink.bSpawnAtPricateHome:
        //                        return getSetBool(ref Settings.SpawnAtPrivateHome, value, get);
        //                }

        //                return base.BlockMenuBoolValue(playerIx, value, get, valueIx);
        //            }
        //        }

        //        public override void TextInputEvent(PlayerIndex playerIndex, string input, int link)
        //        {
        //            if (voxelDesigner != null && voxelDesigner.WaitingForTextInput)
        //            {
        //                voxelDesigner.TextInputEvent(playerIndex, input, link);
        //                //return true;
        //            }
        //            else 
        //            {
        //                input = Engine.LoadContent.CheckCharsSafety(input, LoadedFont.Lootfest);
        //                input = TextLib.CheckBadLanguage(input);
        //                if (input.Length > Players.Player.ChatMessageMaxChars)
        //                {
        //                    input = input.Remove(Players.Player.ChatMessageMaxChars);
        //                }

        //                if (Network.Session.Connected)
        //                {
        //                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_Chat,
        //                        Network.PacketReliability.Chat, Index);
        //                    w.Write(input);
        //                }


        //                //print on your own screen
        //                const string You = "You";
        //                ChatMessageData m = new ChatMessageData(input, You);
        //                LfRef.gamestate.AddChat(m, true);
        //                PrintChat(m, LoadedSound.Dialogue_Neutral);
        //                CloseMenu();
        //            }

        //            deleteInputDialogue();
        //            //return false;
        //        }
        //        public override void TextInputCancelEvent(PlayerIndex playerIndex)
        //        {
        //            deleteInputDialogue();
        //        }
        //        void deleteInputDialogue()
        //        {
        //            if (inputDialogue != null)
        //            {
        //                inputDialogue.DeleteMe();
        //                inputDialogue = null;
        //                if (voxelDesigner != null)
        //                {
        //                    CloseMenu();
        //                    mode = PlayerMode.Creation;
        //                }
        //                else
        //                {
        //                    if (mFile == null)
        //                    {
        //                        exitCurrentMode();
        //                    }
        //                    else
        //                        OpenMenuFile();
        //                }
        //            }
        //        }

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
        //                            if (hero.InteractingWith == null)
        //                            {
        //                                menu.Back(e.PlayerIx);
        //                            }
        //                            else
        //                            {
        //                                if (hero.InteractingWith.InteractType != GameObjects.InteractType.SpeakDialogue ||
        //                                    menu.FileProperties.ParentPage < 0)
        //                                {
        //                                    CloseMenu();
        //                                }
        //                                else
        //                                {
        //                                    if (menu.FileProperties.ParentPage > 0)
        //                                    {
        //                                        hero.InteractingWith.Interact_LinkClick(new HUD.Link(menu.FileProperties.ParentPage), this.hero, menu);
        //                                    }
        //                                    else
        //                                        OpenMenuFile(hero.InteractingWith.Interact_TalkMenu(hero));
        //                                }
        //                                Music.SoundManager.PlayFlatSound(LoadedSound.MenuBack);
        //                            }
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

        //        bool altButtonDown
        //        {
        //            get
        //            {
        //                return Controller.IsButtonDown(Buttons.LeftTrigger);
        //            }
        //        }

        void updateInput()
        {
            if (inputMap.DownEvent(Input.ButtonActionType.GameEditorMode) && buttonTutorial == null)//e.Button == numBUTTON.Back && buttonTutorial == null)
            {
                //if (e.KeyDown)
                //{
                if (buttonLayOut == null)
                {
                    if (voxelDesigner != null)
                    {
                        buttonHelp(voxelDesigner.Mode);
                    }
                    else
                    {
                        buttonHelp(mode);
                    }
                    Music.SoundManager.PlayFlatSound(LoadedSound.MenuSelect);
                }
                else
                {
                    HideControls();
                    Music.SoundManager.PlayFlatSound(LoadedSound.MenuBack);
                }
                //}

            }
            else if (controlLock <= 0) //&& buttonLayOut == null)
            {
                if (!hero.Alive)
                { mode = PlayerMode.InMenu; }
                switch (mode)
                {
                    case PlayerMode.Play:
                        updateInGameInput();//playButtonInput(e);
                        break; //END PLAY MODE
                    default:

                        //if (e.KeyDown)
                        //{
                        //    switch (e.Button)
                        //    {
                        //        case numBUTTON.A:
                        //            CloseMenu();
                        //            break;
                        //        case numBUTTON.B:
                        //            CloseMenu();
                        //            break;
                        //        case numBUTTON.Start:
                        //            openPage(MenuPageName.MainMenu);
                        //            break;
                        //    }
                        //}
                        break;
                    case PlayerMode.ButtonTutorial:
                        if (buttonTutorial.args.button != null)
                        {
                            if (inputMap.DownEvent(buttonTutorial.args.button.Value))
                            {
                                beginButtonTutorial(ButtonTutorialArgs.EndTutuorial);
                                updateInGameInput();
                            }
                        }
                        else if (buttonTutorial.args.dirInput != null)
                        {
                            if (inputMap.Dir(buttonTutorial.args.dirInput.Value).Length() > 0.5f)
                            {
                                beginButtonTutorial(ButtonTutorialArgs.EndTutuorial);
                                updateInGameInput();
                            }
                        }

                        //if (e.KeyDown && e.Button == buttonTutorial.args.button)
                        //{
                        //    beginButtonTutorial(ButtonTutorialArgs.EndTutuorial);
                        //    if (e.Button != numBUTTON.A)
                        //        this.Button_Event(e);
                        //}
                        break;

                    //#if CMODE
                    case PlayerMode.Creation:
                        //voxelDesigner.Button_Event(e);
                        break;
                    //case PlayerMode.RCtoy:
                    //    if (e.Button == numBUTTON.X && e.KeyDown && altButtonDown)
                    //    {
                    //        exitRCmode();
                    //    }
                    //    else if (e.Button == numBUTTON.Start && e.KeyDown)
                    //    {
                    //        openPage(MenuPageName.RCPauseMenu);
                    //        rcToy.InMenu = true;
                    //    }
                    //    else
                    //        rcToy.Button_Event(e);
                    //    break;

                    //#else

                    case PlayerMode.Map:
                        minimapButtonInput();
                        break;
                    //#endif
                    case PlayerMode.InMenu:
                        //updateInGameInput();
                        break;
                    case PlayerMode.InDialogue:
                        if (inputMap.DownEvent(Input.ButtonActionType.GameInteract) ||
                            inputMap.DownEvent(Input.ButtonActionType.GameJump) ||
                            inputMap.DownEvent(Input.ButtonActionType.GameMainAttack) ||
                            inputMap.DownEvent(Input.ButtonActionType.GameAlternativeAttack) ||
                            inputMap.DownEvent(Input.ButtonActionType.GameInteract))
                        {
                            dialogue.Click();
                        }
                        break;
                }

            }//end control lock
        }

        void updateInGameInput()
        {
            if (inputMap.DownEvent(Input.ButtonActionType.GamePauseMenu))
            {
                openPage(MenuPageName.MainMenu);
            }

            if (inputMap.IsDown(Input.ButtonActionType.GameAltButton))
            {
                if (inputMap.DownEvent(Input.ButtonActionType.GameJump))
                {
                    openPage(MenuPageName.Backpack);
                }
                if (inputMap.DownEvent(Input.ButtonActionType.GameAlternativeAttack))
                {
                    openPage(MenuPageName.NetworkSettings);
                }
                if (inputMap.DownEvent(Input.ButtonActionType.GameMainAttack))
                {
                    openPage(MenuPageName.Backpack);
                }
            }
            else
            {
                useEquipmentUpdate(Input.ButtonActionType.GameJump, EquipSlot.ButtonA);
                useEquipmentUpdate(Input.ButtonActionType.GameAlternativeAttack, EquipSlot.ButtonB);
                useEquipmentUpdate(Input.ButtonActionType.GameMainAttack, EquipSlot.ButtonX);
                useEquipmentUpdate(Input.ButtonActionType.GameUseItem, EquipSlot.ButtonRB);

                if (inputMap.DownEvent(Input.ButtonActionType.GameAltUseItem))
                {
                    healingButtonPress();
                }
                if (inputMap.DownEvent(Input.ButtonActionType.GameInteract))
                {
                    interact();
                }
            }

            moveCamera(inputMap.DirTime(Input.DirActionType.GameCamera));
            IntVector2 dpad = inputMap.Stepping(Input.DirActionType.EditorCamXY) + inputMap.Stepping(Input.DirActionType.EditorCamZoom);

            if (dpad.Y > 0)
            {
                if (mode == PlayerMode.InMenu || mode == PlayerMode.Play)
                {
                    if (mode == PlayerMode.InMenu)
                        CloseMenu();

                    if (PlatformSettings.DevBuild && inputMap.IsDown(Input.ButtonActionType.GameAltButton))
                    {
                        BeginCreationMode();
                    }
                    else
                    {
                        openMiniMap(true);
                    }
                }
            }
            else if (dpad.Y < 0)
            {
                changeCamera();
            }

        }

        void useEquipmentUpdate(Input.ButtonActionType button, EquipSlot slot)
        {
            if (inputMap.DownEvent(button))
            {
                progress.Equipped.Use(slot, true, hero, progress.Items);
            }
            else if (inputMap.UpEvent(button))
            {
                progress.Equipped.Use(slot, false, hero, progress.Items);
            }
        }


        //void playButtonInput(ButtonValue e)
        //{
        //            switch (e.Button)
        //            {

        //#if !CMODE
        //                default:

        //                    break;
        //                case numBUTTON.Start:
        //                    if (e.KeyDown)
        //                    {
        //                        openPage(MenuPageName.MainMenu);
        //                    }
        //                    break;
        //                case numBUTTON.A:
        //                    if (altButtonDown)
        //                    {
        //                        if (e.KeyDown && !Controller.IsButtonDown(Buttons.RightTrigger))
        //                            openPage(MenuPageName.Backpack);
        //                    }
        //                    else
        //                    {
        //                        progress.Equipped.Use(EquipSlot.ButtonA, e.KeyDown, hero, progress.Items);
        //                    }
        //                    break;
        //                case numBUTTON.B:
        //                    if (altButtonDown)
        //                    {
        //                        if (e.KeyDown && !Controller.IsButtonDown(Buttons.RightTrigger))
        //                            openPage(MenuPageName.NetworkSettings);
        //                    }
        //                    else
        //                    {
        //                        progress.Equipped.Use(EquipSlot.ButtonB, e.KeyDown, hero, progress.Items);
        //                    }
        //                    break;
        //                case numBUTTON.X:
        //                    if (altButtonDown)
        //                    {
        //                        if (e.KeyDown && !Controller.IsButtonDown(Buttons.RightTrigger))
        //                            openPage(MenuPageName.Backpack);
        //                    }
        //                    else
        //                    {
        //                        progress.Equipped.Use(EquipSlot.ButtonX, e.KeyDown, hero, progress.Items);
        //                    }
        //                    break;
        //                case numBUTTON.Y:
        //                    if (e.KeyDown && !Controller.IsButtonDown(Buttons.RightTrigger))
        //                    {
        //                        interact();
        //                    }
        //                    break;
        //                case LootfestLib.InputHealUp://numBUTTON.LB:
        //                    if (e.KeyDown) healingButtonPress();

        //                    //progress.Equipped.Use(EquipSlot.ButtonLB, e.KeyDown, hero, progress.Items);
        //                    //if (e.KeyDown)
        //                    //    changeEquipSetup(false);
        //                    break;
        //                case numBUTTON.RB:

        //                    progress.Equipped.Use(EquipSlot.ButtonRB, e.KeyDown, hero, progress.Items);
        //                    //if (e.KeyDown)
        //                    //    changeEquipSetup(true);
        //                    break;
        //                //case numBUTTON.RT:
        //                //    //use healing

        //                //    break;
        //#else
        //                            case numBUTTON.A:
        //                                if (LTkeyDown)
        //                                {
        //                                    if (e.KeyDown)
        //                                        beginSendMessage();
        //                                }
        //                                else
        //                                {
        //                                    hero.Attack(sword, e.KeyDown, EquipSlot.Primary, EquippedAlternativeUse.Standard);
        //                                }
        //                                break;
        //                            case numBUTTON.B:
        //                                if (e.KeyDown)
        //                                {
        //                                    if (LTkeyDown)
        //                                    {
        //                                        openPage(MenuPageName.NetworkSettings);
        //                                    }
        //                                    else
        //                                    {

        //                                    }
        //                                }
        //                                break;
        //                            case numBUTTON.X:
        //                                if (e.KeyDown)
        //                                {
        //                                    if (LTkeyDown)
        //                                    {

        //                                        beginRCmode(lastToyType);
        //                                    }
        //                                    else
        //                                    {
        //                                        throwSpear();

        //                                    }
        //                                }
        //                                break;
        //                            case numBUTTON.Y:
        //                                if (e.KeyDown)
        //                                {
        //                                    if (LTkeyDown)
        //                                    {
        //                                        spawnCritter(false);
        //                                    }
        //                                    else
        //                                    {
        //                                        interact();
        //                                    }
        //                                }
        //                                break;
        //                            case numBUTTON.Start:
        //                                if (e.KeyDown)
        //                                {
        //                                    //(MenuType.PauseMenu);
        //                                    openPage(MenuPageName.MainMenu);
        //                                }
        //                                break;

        //                            case numBUTTON.RB:
        //                                if (LTkeyDown)
        //                                {

        //                                }
        //                                else
        //                                {
        //                                    hero.Attack(sword, e.KeyDown, EquipSlot.Primary, EquippedAlternativeUse.Standard);
        //                                }
        //                                break;
        //                            //case numBUTTON.LT:
        //                            //    LTkeyDown = e.KeyDown;
        //                            //    break;
        //                            case numBUTTON.RT:
        //                                if (e.KeyDown)
        //                                {
        //                                    if (LTkeyDown)
        //                                    {
        //                                        buyPlayerHealth();
        //                                    }
        //                                    else
        //                                    {
        //                                        throwSpear();
        //                                    }
        //                                }
        //                                break;
        //#endif
        //            }

        //}

        void healingButtonPress()
        {
            if (hero.PercentHealth < 1)
            {
                GameObjects.Gadgets.FoodHealingPower result = progress.Items.EatHealing(hero.PercentHealth,
                    progress.GotSkill(GameObjects.Magic.MagicRingSkill.Apple_lover), progress.GotSkill(GameObjects.Magic.MagicRingSkill.Hobbit_skill));
                UseHealing(result);
            }
        }

        void minimapButtonInput()
        {
            if (miniMap != null)
            {
                if (inputMap.DownEvent(Input.ButtonActionType.GameJump))
                {
                    if (PlatformSettings.TravelEverywhere && inputMap.IsDown(Input.ButtonActionType.GameAltButton))
                    {
                        hero.JumpTo(miniMap.PointingAtChunk());
                        openMiniMap(false);
                    }
                    else
                    {
                        if (miniMap.Travel)
                        {
                            hero.InteractingWith = null;
                            mFile = new File();
                            IMiniMapLocation location = miniMap.TravelLocation;
                            travelGoal = location.TravelEntrance;
                            openMiniMap(false);

                            if ((travelGoal - hero.MapLocationChunk).Length() > 2)
                            {
                                MyMoneyToMenu(mFile);
                                mFile.AddTitle(location.MapLocationName);
                                //mFile.AddDescription("Travel cost " + LootfestLib.TravelCost.ToString() + LootfestLib.MoneyText);
                                mFile.Add(new LargeShopButtonData(SpriteName.IconTravel, "Go now", null,
                                    LootfestLib.TravelCost, hero.Player.Progress.Items.Money, new HUD.Link((int)Link.UseHorseTravelOK)));
                                //mFile.AddIconTextLink(SpriteName.LFIconCoins, "Pay", (int)Link.UseHorseTravelOK);
                                mFile.AddIconTextLink(SpriteName.LFIconGoBack, "Cancel", (int)Link.CloseMenu);
                            }
                            else
                            {
                                mFile.AddDescription("You are here already!");
                                mFile.AddIconTextLink(SpriteName.LFIconGoBack, "Ok", (int)Link.CloseMenu);
                            }

                            OpenMenuFile(mFile);
                        }
                        else //set flag
                        {
                            miniMap.PlaceFlag(ref progress.mapFlag);
                            progress.NetworkWriteMapFlag();
                        }
                    }
                }

                if (inputMap.DownEvent(Input.ButtonActionType.GameAlternativeAttack))
                {
                    openMiniMap(false);
                }
                if (inputMap.DownEvent(Input.ButtonActionType.GamePauseMenu))
                {
                    openMiniMap(false);
                    openPage(MenuPageName.MainMenu);
                }

                miniMap.Scale(inputMap.DirTime(Input.DirActionType.GameCamera).Y);//e.DirAndTime.Y);
                miniMap.Scroll(inputMap.DirTime(Input.DirActionType.GamePlayerMovement));

                if (inputMap.Stepping(Input.DirActionType.EditorCamXY).HasValue())//DpadUp && e.Direction.Y != 0)
                {
                    openMiniMap(false);
                }
            }

            //if (e.KeyDown)
            //{
            //    switch (e.Button)
            //    {
            //        case numBUTTON.A:
            //            if (miniMap != null)
            //            {

            //                if (PlatformSettings.TravelEverywhere && altButtonDown)
            //                {
            //                    hero.JumpTo(miniMap.PointingAtChunk());
            //                    openMiniMap(false);
            //                }
            //                else

            //                {
            //                    if (miniMap.Travel)
            //                    {
            //                        hero.InteractingWith = null;
            //                        mFile = new File();
            //                        IMiniMapLocation location = miniMap.TravelLocation;
            //                        travelGoal = location.TravelEntrance;
            //                        openMiniMap(false);

            //                        if ((travelGoal - hero.MapLocationChunk).Length() > 2)
            //                        {
            //                            MyMoneyToMenu(mFile);
            //                            mFile.AddTitle(location.MapLocationName);
            //                            //mFile.AddDescription("Travel cost " + LootfestLib.TravelCost.ToString() + LootfestLib.MoneyText);
            //                            mFile.Add(new LargeShopButtonData(SpriteName.IconTravel, "Go now", null,
            //                                LootfestLib.TravelCost, hero.Player.Progress.Items.Money, new HUD.Link((int)Link.UseHorseTravelOK)));
            //                            //mFile.AddIconTextLink(SpriteName.LFIconCoins, "Pay", (int)Link.UseHorseTravelOK);
            //                            mFile.AddIconTextLink(SpriteName.LFIconGoBack, "Cancel", (int)Link.CloseMenu);
            //                        }
            //                        else
            //                        {
            //                            mFile.AddDescription("You are here already!");
            //                            mFile.AddIconTextLink(SpriteName.LFIconGoBack, "Ok", (int)Link.CloseMenu);
            //                        }

            //                        OpenMenuFile(mFile);
            //                    }
            //                    else //set flag
            //                    {
            //                        miniMap.PlaceFlag(ref progress.mapFlag);
            //                        progress.NetworkWriteMapFlag();
            //                    }
            //                }
            //            }
            //            break;
            //        case numBUTTON.B:
            //            openMiniMap(false);
            //            break;
            //        case numBUTTON.Start:
            //            openMiniMap(false);
            //            openPage(MenuPageName.MainMenu);
            //            break;
            //    }
            //}
        }

        //public override void Button_Event(ButtonValue e)
        //{
        //if (e.Button == numBUTTON.LT)
        //{
        //    altButtonDown = e.KeyDown;
        //}
        //System.Diagnostics.Debug.WriteLine("Player button: " + e.ToString());
        //if (inputDialogue != null)
        //{
        //    inputDialogue.Button_Event(e);
        //}
        //else 
        //            if (e.Button == numBUTTON.Back && buttonTutorial == null)
        //            {
        //                if (e.KeyDown)
        //                {
        //                    if (buttonLayOut == null)
        //                    {
        //                        if (voxelDesigner != null)
        //                        {
        //                            buttonHelp(voxelDesigner.Mode);
        //                        }
        //                        else
        //                        {
        //                            buttonHelp(mode);
        //                        }
        //                        Music.SoundManager.PlayFlatSound(LoadedSound.MenuSelect);
        //                    }
        //                    else
        //                    {
        //                        HideControls();
        //                        Music.SoundManager.PlayFlatSound(LoadedSound.MenuBack);
        //                    }
        //                }

        //            }
        //            else if (controlLock <= 0) //&& buttonLayOut == null)
        //            {
        //                if (!hero.Alive)
        //                { mode = PlayerMode.InMenu; }
        //                switch (mode)
        //                {
        //                    case PlayerMode.Play:
        //                        playButtonInput(e);
        //                        break; //END PLAY MODE
        //                    default:
        //                        if (e.KeyDown)
        //                        {
        //                            switch (e.Button)
        //                            {
        //                                case numBUTTON.A:
        //                                    CloseMenu();
        //                                    break;
        //                                case numBUTTON.B:
        //                                    CloseMenu();
        //                                    break;
        //                                case numBUTTON.Start:
        //                                    openPage(MenuPageName.MainMenu);
        //                                    break;
        //                            }
        //                        }
        //                        break;
        //                    case PlayerMode.ButtonTutorial:
        //                        if (e.KeyDown && e.Button == buttonTutorial.args.button)
        //                        {
        //                            beginButtonTutorial(ButtonTutorialArgs.EndTutuorial);
        //                            if (e.Button != numBUTTON.A)
        //                                this.Button_Event(e);
        //                        }
        //                        break;

        ////#if CMODE
        //                    case PlayerMode.Creation:
        //                        voxelDesigner.Button_Event(e);
        //                        break;
        //                    //case PlayerMode.RCtoy:
        //                    //    if (e.Button == numBUTTON.X && e.KeyDown && altButtonDown)
        //                    //    {
        //                    //        exitRCmode();
        //                    //    }
        //                    //    else if (e.Button == numBUTTON.Start && e.KeyDown)
        //                    //    {
        //                    //        openPage(MenuPageName.RCPauseMenu);
        //                    //        rcToy.InMenu = true;
        //                    //    }
        //                    //    else
        //                    //        rcToy.Button_Event(e);
        //                    //    break;

        ////#else

        //                    case PlayerMode.Map:
        //                        minimapButtonInput(e);
        //                        break;
        ////#endif
        //                    case PlayerMode.InMenu:
        //                        menuButtonInput(e);
        //                        break;
        //                    case PlayerMode.InDialogue:
        //                        if (e.KeyDown)
        //                        {
        //                            dialogue.Click();
        //                        }
        //                        break;
        //                }

        //            }//end control lock
    }



    //        public override void Pad_Event(JoyStickValue e)
    //        {
    //            bool inBuildSelection = false;

    //            if (controlLock <= 0)
    //            {
    //                switch (mode)
    //                {
    //                    default:
    //                        switch (e.Stick)
    //                        {

    //                            //case Stick.Left:
    //                            //    moveCharacter(e.Direction);
    //                            //    break;

    //                            //case Stick.Right:
    //                            //    moveCamera(e.DirAndTime);
    //                            //    break;
    //                            //case Stick.D:
    //                            //    if (DpadUp)
    //                            //    {
    //                            //        if (e.Stepping.Y > 0)
    //                            //        {
    //                            //            if (mode == PlayerMode.InMenu || mode == PlayerMode.Play)
    //                            //            {
    //                            //                if (mode == PlayerMode.InMenu)
    //                            //                    CloseMenu();

    //                            //                if (PlatformSettings.Debug <= BuildDebugLevel.TesterDebug_2 && Controller.IsButtonDown(Buttons.LeftTrigger))
    //                            //                {
    //                            //                    BeginCreationMode();
    //                            //                }
    //                            //                else
    //                            //                {
    //                            //                    openMiniMap(true);
    //                            //                }
    //                            //            }
    //                            //        }
    //                            //        else if (e.Stepping.Y < 0)
    //                            //        {
    //                            //            changeCamera();
    //                            //        }
    //                            //        DpadUp = false;
    //                            //    }
    //                            //    break;
    //                        }
    //                        break;
    //                    case PlayerMode.ButtonTutorial:
    //                        if (buttonTutorial == null)
    //                            exitCurrentMode();
    //                        else
    //                        {
    //                            if (e.Stick == buttonTutorial.args.stick)
    //                            {
    //                                beginButtonTutorial(ButtonTutorialArgs.EndTutuorial);
    //                                this.Pad_Event(e);
    //                            }
    //                        }
    //                        break;

    //                    case PlayerMode.Creation:
    //                        if (e.Stick == Stick.D && voxelDesigner.Mode != PlayerMode.CreationSelection && voxelDesigner.Mode != PlayerMode.InMenu)
    //                        {
    //                            if (DpadUp)
    //                            {
    //                                if (e.Stepping.Y > 0)
    //                                {
    //                                    EndCreationMode();
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            voxelDesigner.Pad_Event(e);
    //                            inBuildSelection = voxelDesigner.HaveSelection;
    //                        }
    //                        break;
    //#if CMODE
    //                    case PlayerMode.RCtoy:
    //                        if (e.Stick == Stick.D)
    //                        {
    //                            if (settings.GetRCCamType(rcToy.FlyingToy) == GameObjects.Toys.RCCameraType.Pilot)
    //                            {
    //                                GameObjects.Toys.AbsFlyingRCtoy flyToy = (GameObjects.Toys.AbsFlyingRCtoy)rcToy;
    //                                flyToy.BehindPos = Bound.Set(flyToy.BehindPos + e.DirAndTime.Y * 0.02f, 4, 24);
    //                            }
    //                            else
    //                            {
    //                                currentZoom += e.DirAndTime.Y * ChangeZoomSpeed;
    //                                updateCamZoom();
    //                            }
    //                        }
    //                        else
    //                            rcToy.Pad_Event(e);
    //                        break;
    //#endif
    //                    case PlayerMode.InMenu:
    //                        if (ValueDialogue != null)
    //                        {
    //                            ValueDialogue.Pad_Event(e);
    //                        }
    //                        else
    //                        {
    //                            if (e.Stick == Stick.Right)
    //                            {
    //                                moveCamera(e.DirAndTime);
    //                            }
    //                            else
    //                            {
    //                                if (menu != null)
    //                                    menu.MoveSelection(e);
    //                            }
    //                        }
    //                        break;
    //                    case PlayerMode.Map:
    //                        if (e.Stick == Stick.Right)
    //                        {
    //                            miniMap.Scale(e.DirAndTime.Y);
    //                        }
    //                        else if (e.Stick == Stick.Left)
    //                        {
    //                            miniMap.Scroll(e.DirAndTime);
    //                        }
    //                        else
    //                        {
    //                            if (DpadUp && e.Direction.Y != 0)
    //                            {
    //                                openMiniMap(false);
    //                            }
    //                        }
    //                        break;
    //                }
    //            }
    //        }
    //        public override void PadUp_Event(Stick padIx, int contolIx)
    //        {
    //            if (padIx == Stick.D)
    //                DpadUp = true;
    //            //explainControlsGroup.PadUp_Event(padIx, contolIx);
    //            //if (mode == PlayerMode.RCtoy)
    //            //    rcToy.PadUp_Event(padIx, contolIx);
    //            else if (mode == PlayerMode.InMenu && menu != null)
    //            {
    //                menu.PadUpEvent(padIx);
    //            }
    //            else if (voxelDesigner != null)
    //            {
    //                voxelDesigner.PadUp_Event(padIx, contolIx);
    //            }

    //        }
    //        public override void KeyPressEvent(Keys key, bool keydown)
    //        {
    //            if (inputDialogue != null)
    //            {
    //                inputDialogue.KeyPressEvent(key, keydown);
    //            }
    //#if PCGAME
    //            else
    //            {

    //                switch (mode)
    //                {
    //                    case PlayerMode.Play:
    //                        switch (key)
    //                        {
    //                            case Keys.W:
    //                                WASDdir.Y = -lib.BoolToInt01(keydown);
    //                                break;
    //                            case Keys.S:
    //                                WASDdir.Y = lib.BoolToInt01(keydown);
    //                                break;
    //                            case Keys.A:
    //                                WASDdir.X = -lib.BoolToInt01(keydown);
    //                                break;
    //                            case Keys.D:
    //                                WASDdir.X = lib.BoolToInt01(keydown);
    //                                break;
    //                            case Keys.Tab:
    //                                if (keydown)
    //                                    beginCreationMode(true);
    //                                //startCreationMo(true);
    //                                break;
    //                            case Keys.C:
    //                                if (keydown)
    //                                    changeCamera();
    //                                break;
    //                            case Keys.Escape:
    //                                if (keydown)
    //                                    openPage(MenuPageName.MainMenu);
    //                                break;
    //                            case Keys.Y:
    //                                if (keydown)
    //                                    interact();
    //                                break;

    //                        }
    //                        break;
    //                    case PlayerMode.InMenu:
    //                        if (keydown)
    //                            CloseMenu();
    //                        break;
    //                    case PlayerMode.Creation:
    //                        voxelDesigner.KeyPressEvent(key, keydown);
    //                        break;

    //                }
    //            }
    //#endif
    //        }

    //bool LTkeyDown = false;



}

