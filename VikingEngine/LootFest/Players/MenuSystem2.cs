using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

using VikingEngine.HUD;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest.Players;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.LootFest.GO;
using VikingEngine.SteamWrapping;
using VikingEngine.LootFest.GameState;
using VikingEngine.Input;
using VikingEngine.LootFest.GO.PlayerCharacter;
using VikingEngine.EngineSpace.Graphics.DeferredRendering;
using VikingEngine.EngineSpace.Maths;
//using SharpDX.DirectInput;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.Network;

namespace VikingEngine.LootFest
{
    /// <summary>
    /// Handler of all the in game menus (version 2) 
    /// </summary>
    class MenuSystem2
    {
        public Gui menu;
        Players.Player player;
#if LF2
        LF2.Players.Player lf2player;
#endif

        Color guiColor;

        
        public MenuSystem2(Players.Player player)
        {
            this.player = player;

            menu = new Gui(GuiStyle(), player.SafeScreenArea, 0f, LfLib.Layer_GuiMenu, Input.InputSource.DefaultPC);//player.PlayerIndex);

            Input.Mouse.Visible = true;

            //inputOverview = new Display.InputOverview(player.SafeScreenArea, player.inputMap);
        }

#if LF2
        public MenuSystem2(LF2.Players.Player player)
        {
            this.lf2player = player;

            menu = new Gui(GuiStyle(), player.SafeScreenArea, 0f, LfLib.Layer_GuiMenu, Input.InputSource.DefaultPC);//player.PlayerIndex);

            Input.Mouse.Visible = true;

            //inputOverview = new Display.InputOverview(player.SafeScreenArea, player.inputMap);
        }
#endif
        public static GuiStyle GuiStyle()
        {
            return new GuiStyle(
                (Screen.PortraitOrientation ? Screen.Width : Screen.Height) * 0.61f, 5, SpriteName.LFMenuRectangleSelection);
        }

        ushort vibration;
        float VibrationProperty(bool set, float value)
        {
            if (set)
                vibration = (ushort)value;
            return vibration;
        }

        //void Vibrate()
        //{
        //    player.pData.inputMap.Vibrate(ESteamControllerPad.k_ESteamControllerPad_Left, vibration);
        //}

        //void JoystickTest()
        //{
        //    GuiLayout layout = new GuiLayout("Test Joystick", menu);
        //    {
        //        //int pIx = player.PlayerIndex;
        //        int pIx = player.inputMap.controllerIndex;

        //        FloatGetSet gsx = (bool set, float val) => { return SharpDXInput.GetDpad(GenericControllerAxis.PointOfViewControllers0, pIx).X; };
        //        FloatGetSet gsy = (bool set, float val) => { return SharpDXInput.GetDpad(GenericControllerAxis.PointOfViewControllers0, pIx).Y; };
        //        new GuiFloatSlider(SpriteName.NO_IMAGE, "dpad x", gsx, new IntervalF(-1, 1), false, layout);
        //        new GuiFloatSlider(SpriteName.NO_IMAGE, "dpad y", gsy, new IntervalF(-1, 1), false, layout);

        //        for (int i = 0; i < 8; ++i)
        //        {
        //            GenericControllerAxis axis = (GenericControllerAxis)i;

        //            FloatGetSet getset = (bool set, float val) => { return SharpDXInput.GetFloatAxis(axis, pIx); };
        //            new GuiFloatSlider(SpriteName.NO_IMAGE, axis.ToString(), getset, new IntervalF(-1, 1), false, layout);
        //        }
        //        for (int i = 0; i < (int)GenericControllerButton.NUM; ++i)
        //        {
        //            GenericControllerButton btn = (GenericControllerButton)i;
        //            IntGetSet getset = (bool set, int val) => { return SharpDXInput.IsDown(btn, pIx) ? 1 : 0; };
        //            new GuiIntSlider(SpriteName.NO_IMAGE, btn.ToString(), getset, new IntervalF(0, 1), false, layout);
        //        }

        //        for (int i = (int)GenericControllerAxis.START2_MINUS1 + 1; i < (int)GenericControllerAxis.NUM2; ++i)
        //        {
        //            GenericControllerAxis axis = (GenericControllerAxis)i;

        //            FloatGetSet getset = (bool set, float val) => { return SharpDXInput.GetFloatAxis(axis, pIx); };
        //            new GuiFloatSlider(SpriteName.NO_IMAGE, axis.ToString(), getset, new IntervalF(-1, 1), false, layout);
        //        }
        //    }
        //    layout.End();
        //}

        public void RayPickMenu()
        {
            GuiLayout layout = new GuiLayout("Click an object", menu);
            {
                
            }
            layout.End();
            
            new RaycastPickedObjectMenu(player.localPData.view.Camera, menu);
        }

        public void MainMenu()
        {
            GuiLayout layout = new GuiLayout("Main Menu", menu, GuiLayoutMode.MultipleColumns, null);
            {
                if (PlatformSettings.DevBuild)
                {
                    // currently debugging
                    //new GuiTextButton("Update objs", null, new GuiAction1Arg<Gui>(Ref.update.UpdateListToFile, menu), true, layout);
                    //new GuiTextButton("Test KdTree", null, KdTree2Tester.Test , false, layout);
                    //new GuiTextButton("Ray Picking", null, RayPickMenu, true, layout);
                    //new GuiTextButton("Tree Tester", null, new GuiAction1Arg<Gui>(treeTester.Try, menu), true, layout);
                    
                    //new GuiTextButton("Joystick Test", null, JoystickTest, true, layout);
                }
                
                new GuiIcon(SpriteName.LFExpressHi, null, new GuiAction1Arg<VoxelModelName>(expressOption, VoxelModelName.express_hi), false, layout);
                new GuiIcon(SpriteName.LFExpressThumbsUp, null, new GuiAction1Arg<VoxelModelName>(expressOption, VoxelModelName.express_thumbup), false, layout);
                new GuiIcon(SpriteName.LFExpressLaugh, null, new GuiAction1Arg<VoxelModelName>(expressOption, VoxelModelName.express_laugh), false, layout);
                new GuiIcon(SpriteName.LFExpressTease, null, new GuiAction1Arg<VoxelModelName>(expressOption, VoxelModelName.express_teasing), false, layout);

                new GuiIcon(SpriteName.LFExpressAngry, null, new GuiAction1Arg<VoxelModelName>(expressOption, VoxelModelName.express_anger), false, layout);
                new GuiIcon(SpriteName.LFExpressSad, null, new GuiAction1Arg<VoxelModelName>(expressOption, VoxelModelName.express_sad1), false, layout);
                new GuiIcon(SpriteName.LFExpressLoot, null, new GuiAction1Arg<VoxelModelName>(expressOption, VoxelModelName.express_loot), false, layout);
                new GuiIcon(SpriteName.LFExpressDuck, null, new GuiAction1Arg<VoxelModelName>(expressOption, VoxelModelName.express_duck), false, layout);


                // basic & important
                new GuiTextButton("Resume", null, player.CloseMenu, false, layout);
                if (player.hero is GO.PlayerCharacter.HorseRidingHero)
                {
                    new GuiTextButton("Dismount", "Jump off your riding animal", new GuiAction(player.hero.dismount, player.CloseMenu), false, layout);
                }
                new GuiTextButton("Appearance", null, pageAppearance, true, layout);
                new GuiTextButton("Multiplayer", null, pageMultiplayer, true, layout);
                new GuiTextButton("Options", null, pageOptions, true, layout);
                   
                
                new GuiSectionSeparator(layout);
                {
                    // extra & fun stuff
                    if (LfRef.progress.GotUnlock(player.Storage, UnlockType.Cards) || PlatformSettings.DevBuild)
                    {
                        new GuiTextButton("Cards", null, pageCards, true, layout);
                    }
                    new GuiTextButton("Trophies", null, pageTrophies, true, layout);
                }
                new GuiSectionSeparator(layout);
                {
                    
                    new GuiTextButton("**Debug**", null, pageDebug, true, layout);
                    //new GuiTextButton("**Card Game**", "Try the draft version of the card minigame", startCardGame, false, layout);
                    //new GuiTextButton("**Terrain Editor**", null, new GuiActionOpenPage(LfRef.terrain.EditorMenu), false, layout);
                    //if (PlatformSettings.DevBuild)
                    //{
                    //    new GuiTextButton("**Scene maker**", null, startSceneMaker, false, layout);
                    //}
                    new GuiTextButton("**Voxel Editor**", null, startVoxelEditor, false, layout);

                    if (PlatformSettings.DevBuild)
                    {
                        new GuiTextButton("**BlockMap Editor**", null, startBlockMapEditor, false, layout);
                            

                    }
#if PCGAME
                    Ref.steam.debugToMenu(layout);
#endif
                }
                new GuiSectionSeparator(layout);
                {
                    if (PlatformSettings.DevBuild && LfRef.gamestate.localPlayers.Count < Engine.Draw.MaxScreenSplit)
                    {
                        new GuiTextButton("+Add local player", null, addLocalPlayer, false, layout);
                    }
                    // exit
                    string text = "";
                    if (LfRef.gamestate.LocalHostingPlayer == player)
                        text = "Save & Exit"; // exit game
                    else
                        text = "Drop Out"; // leave splitscreen
                    new GuiDialogButton(text, null, new GuiAction(exitLink), false, layout);
                }
            }
            layout.End();

            layout.MoveSteppingInput(IntVector2.PositiveY);
        }

        

        void restart()
        {
            new GameState.LoadingMap(new Data.WorldData(true));
        }

        void addLocalPlayer()
        {
            player.CloseMenu();
            LfRef.gamestate.addLocalPlayer();
        }
        
        void expressOption(VoxelModelName expression)
        {
            player.CloseMenu();
            player.hero.Express(expression);
        }

        void startBlockMapEditor()
        {
            new BlockMap.EditorState();
        }
        
        void pageMultiplayer()
        {
            GuiLayout layout = new GuiLayout("Multiplayer settings", menu);
            {
                new GuiLabel(Ref.netSession.HasInternet ? "Online" : "Offline", layout);
                if (Ref.netSession.InMultiplayerSession)
                {
                    new GuiLabel("In session, host: " + Ref.netSession.IsHost.ToString() + ")", layout);

                    //List active players, use to kick
                    List<Network.AbsNetworkPeer> gamers = Ref.netSession.RemoteGamers();//steam.P2PManager.remoteGamers;
                    foreach (var g in gamers)
                    {
                        new GuiTextButton(Engine.LoadContent.CheckCharsSafety(g.Gamertag, layout.gui.style.textFormat.Font), 
                            null, new GuiAction1Arg<AbsNetworkPeer>(viewSteamGamer, g), true, layout);
                    }
                }
            } layout.End();
        }

        void viewSteamGamer(AbsNetworkPeer gamer)
        {
            GuiLayout layout = new GuiLayout(Engine.LoadContent.CheckCharsSafety(gamer.Gamertag, menu.style.textFormat.Font), menu);
            {
                new GuiLabel("TODO: Ping, View gamer info", layout);
                if (Ref.netSession.IsHost)
                {
                    new GuiTextButton("Kick Player", null, new GuiAction1Arg<AbsNetworkPeer>(KickSteamNetworkPeer, gamer), false, layout);
                }
            }
            layout.End();
        }

        void KickSteamNetworkPeer(AbsNetworkPeer peer)
        {
            Ref.netSession.kickFromNetwork(peer);//peer.kickFromNetwork();
            //Ref.steam.P2PManager.RemovePeer(peer);
        }

        //void searchSessions()
        //{
        //    GuiLayout layout = new GuiLayout("Searching...", menu);
        //    {

        //    } layout.End();

        //    LfRef.net.ManualLobbySearch();
        //}

        void pageTrophies()
        {
            int gotTrophiesCount = 0;
            for (int i = 0; i < player.Storage.progress.achievements.Length; ++i)
            {
                if (player.Storage.progress.achievements[i]) gotTrophiesCount++;
            }

            GuiLayout layout = new GuiLayout(SpriteName.NO_IMAGE, "Trophies " + gotTrophiesCount.ToString() + "/" +
                player.Storage.progress.achievements.Length.ToString(), menu, GuiLayoutMode.MultipleColumns);
            {
                for (int i = 0; i < player.Storage.progress.achievements.Length; ++i)
                {
                    Data.AchievementIndex aIndex = (Data.AchievementIndex)i;
                    bool unlocked = player.Storage.progress.achievements[i];
                    TwoStrings name = Data.Achievements.NameAndDesc(aIndex);
                    SpriteName ic = Data.Achievements.icon(aIndex ,unlocked);
                    new GuiBigIcon(ic, name.String2, null, false, layout);
                }

                int unlockCount = (int)UnlockType.NUM_NONE;

                int gotUnlockCount = 0;
                for (UnlockType utype = 0; utype < UnlockType.NUM_NONE; utype++)
                {
                    if (LfRef.progress.GotUnlock(player.Storage, utype))
                    {
                        gotUnlockCount++;
                    }
                }

                new GuiSectionSeparator(layout);//--
                new GuiLabel("Secrets found " + gotUnlockCount.ToString()  + "/" + unlockCount.ToString(), layout);
                for (UnlockType utype = 0; utype < UnlockType.NUM_NONE; utype++)
                {
                    if (LfRef.progress.GotUnlock(player.Storage, utype))
                    {
                        new GuiTextButton(utype.ToString(), null, new GuiAction1Arg<UnlockType>(selectUnlockType, utype), true, layout);
                    }
                }
            }
            layout.End();
        }

        void selectUnlockType(UnlockType t)
        {
            switch (t)
            {
                case UnlockType.Cape:
                    pageAppearance();
                    break;
                case  UnlockType.Cards:
                    pageCards();
                    break;
            }
        }

        //public void listAvailableSessions(List<VikingEngine.Network.SteamAvailableSession> availableSessionsList)
        //{
        //    GuiLayout layout = new GuiLayout("Available network", menu);
        //    {
        //        if (availableSessionsList == null || availableSessionsList.Count == 0)
        //        {
        //            new GuiLabel("No available sessions", layout);
        //            new GuiTextButton("OK", null, new GuiAction1Arg<int>(menu.PopLayouts, 2), true, layout);
        //        }
        //        else
        //        {
        //            foreach (var sess in availableSessionsList)
        //            {
        //                new GuiTextButton("Join " + Engine.LoadContent.CheckCharsSafety(sess.hostName, layout.gui.style.textFormat.Font), 
        //                    "Join in a network co-op session",
        //                    new GuiAction1Arg<VikingEngine.Network.SteamAvailableSession>(LfRef.net.joinSessionLink, sess), false, layout);
        //            }
        //        }
        //    } layout.End();
        //}

        

        void pageCards()
        {
            //GuiLayout layout = new GuiLayout(SpriteName.NO_IMAGE, "Card collection", menu, GuiLayoutMode.MultipleColumns);
            //{
            //    new GuiTextButton("Tutorial", null, cardTutorial, false, layout);

            //    int totalCollectableTypes;
            //    int collectedTypes = player.Storage.progress.CardTypesCollectionCount(out totalCollectableTypes);
            //    new GuiLabel("Captured " + collectedTypes.ToString() + "/" + totalCollectableTypes.ToString(), layout);
            //    for (VikingEngine.CardType type = 0; type < CardType.NumNon; ++type)
            //    {
            //        int ix = (int)type;
            //        if (CardGame.cgLib.CardAvailableTypeList[ix] == CardGame.CardAvailableType.Collectable)
            //        {
            //            var captures = player.Storage.progress.cardCollection[ix];
            //            if (captures.TotalCount > 0 || PlatformSettings.DevBuild)
            //            {
            //                new Display.GuiCardCaptureIcon(type, captures.BaseCards, layout);
            //                //new GuiTextButton(type.ToString() + " " + captures.BaseCards.ToString(), null, new GuiNoAction(), false, layout);
            //            }
            //        }
            //    }
            //} layout.End();
        }

        

        void cardTutorial()
        {
            GuiLayout layout = new GuiLayout("Cards tutorial", menu, GuiLayoutMode.SingleColumn, true);
            {
                new GuiLabel("1. Get the card item", layout);
                new GuiLabel("2. Damage enemy down to 1HP", layout);
                new GuiLabel("3. Stun the enemy (if possible)", layout);
                new GuiLabel("4. Throw a card at them", layout);
                new GuiLabel("5. Pick up the captured enemy card", layout);

            } layout.End();
        }

        //void otherTitles()
        //{
        //    new GameState.OtherGamesView(menu.player);
        //}

        public void GameOverMenu()
        {
            GuiLayout layout = new GuiLayout("You were knocked out!", menu);
            {
                new GuiTextButton("Ok", null, player.restartFromKnockout, false, layout);
            }
            layout.End();
            //mFile.AddTitle("You were knocked out!");
            //mFile.AddTextLink("Ok", new HUD.ActionLink(restartFromKnockout));
        }

        /// <returns>Close menu</returns>
        public bool update()
        {
            return menu.Update();
        }

        void exitLink()
        {
            //if (Ref.netSession.Connected)
            //{
            //    Ref.netSession.BeginWritingPacket(Network.PacketType.PlayerDisconnected, Network.PacketReliability.Reliable);

            //}
            Ref.netSession.GracefulExitSession();

            //}
            //
            menu.PopAllLayouts();
            player.saveAndExitLink();
        }

        void pageAddAlternativeBtn(ButtonActionType map, bool addAlternative)
        {
            GuiLayout layout = new GuiLayout("Bind " + map.ToString(), menu);
            {
                new GuiLabel("Waiting for input...", layout);
            }
            layout.End();

            new BtnBindUpdateObj(menu, layout, map, player.hero, addAlternative);
        }

        void addFourButtonDirectionalMap(DirActionType map, bool addAlternative)
        {
            new FourButtonDirectionMapCreator(menu, map, player.hero, addAlternative);
        }

        //void pageAddAlternativeDir(DirActionType map, bool addAlternative)
        //{
        //    GuiLayout layout = new GuiLayout("Bind " + map.ToString(), menu);
        //    {
        //        new GuiTextButton("Mouse Move", null, new GuiAction5Arg<DirActionType, bool, GenericControllerAxis, IDirectionalMap, Dimensions>(
        //            addDirectionalBinding, map, addAlternative, GenericControllerAxis.NUM, new DirectionalMouseMap(), Dimensions.Y), false, layout);

        //        new GuiTextButton("Scroll Wheel", null, new GuiAction5Arg<DirActionType, bool, GenericControllerAxis, IDirectionalMap, Dimensions>(
        //            addDirectionalBinding, map, addAlternative, GenericControllerAxis.NUM, new DirectionalMouseScrollMap(), Dimensions.Y), false, layout);
        //        new GuiTextButton("4 Buttons", null, new GuiAction2Arg<DirActionType, bool>(addFourButtonDirectionalMap, map, addAlternative), true, layout);
        //        new GuiTextButton("Xbox Stick", null, new GuiAction2Arg<DirActionType, bool>(addXboxStickBinding, map, addAlternative), true, layout);
        //        new GuiTextButton("Xbox Triggers", null, new GuiAction5Arg<DirActionType, bool, GenericControllerAxis, IDirectionalMap, Dimensions>(
        //            addDirectionalBinding, map, addAlternative, GenericControllerAxis.NUM, new DirectionalXboxTriggerMap((int)player.PlayerIndex), Dimensions.Y), 
        //            false, layout);
        //        new GuiTextButton("Generic Dpad/HAT/POV", null, new GuiAction2Arg<DirActionType, bool>(addGenericDpadBinding, map, addAlternative), true, layout);
        //        new GuiTextButton("Generic Dual Axes", null, new GuiAction3Arg<DirActionType, bool, Dimensions>(addGenericDualAxesBindingStep, map, addAlternative, Dimensions.X), true, layout);
        //    }
        //    layout.End();
        //}

        bool invertY;
        bool invertYProperty(int index, bool set, bool value)
        {
            return GetSet.Do<bool>(set, ref invertY, value);
        }

        //void addGenericDpadBinding(DirActionType dirType, bool addAlternative)
        //{
        //    GuiLayout layout = new GuiLayout("Bind " + dirType.ToString(), menu);
        //    {
        //        new GuiCheckbox("Invert Y", null, invertYProperty, layout);
        //        //new GuiTextButton("Dpad/HAT/POV 1", null,
        //        //    new GuiAction5Arg<DirActionType, bool, GenericControllerAxis, IDirectionalMap, Dimensions>(addDirectionalBinding, dirType, addAlternative,
        //        //        new DirectionalGenericDpadMap(GenericControllerAxis.PointOfViewControllers0, invertY, player.inputMap.controllerIndex)),
        //        //    false, layout);
        //        //new GuiTextButton("Dpad/HAT/POV 2", null,
        //        //    new GuiAction5Arg<DirActionType, bool, GenericControllerAxis, IDirectionalMap, Dimensions>(addDirectionalBinding, dirType, addAlternative,
        //        //        new DirectionalGenericDpadMap(GenericControllerAxis.PointOfViewControllers1, invertY, player.inputMap.controllerIndex)),
        //        //    false, layout);
        //        //new GuiTextButton("Dpad/HAT/POV 3", null,
        //        //    new GuiAction5Arg<DirActionType, bool, GenericControllerAxis, IDirectionalMap, Dimensions>(addDirectionalBinding, dirType, addAlternative,
        //        //        new DirectionalGenericDpadMap(GenericControllerAxis.PointOfViewControllers2, invertY, player.inputMap.controllerIndex)),
        //        //    false, layout);

        //        for (GenericControllerAxis axis = GenericControllerAxis.PointOfViewControllers0; axis <= GenericControllerAxis.PointOfViewControllers3; ++axis)
        //        {
        //            new GuiTextButton("Dpad/HAT/POV " + TextLib.IndexToString((int)(axis - GenericControllerAxis.PointOfViewControllers0)), null,
        //                new GuiAction5Arg<DirActionType, bool, GenericControllerAxis, IDirectionalMap, Dimensions>(
        //                    addDirectionalBinding, dirType, addAlternative, GenericControllerAxis.NUM,
        //                    new DirectionalGenericDpadMap(axis, invertY, player.inputMap.controllerIndex),
        //                    Dimensions.Y),
        //                false, layout);
        //        }
        //    }
        //    layout.End();
        //}

        //GenericControllerAxis storedXaxis = GenericControllerAxis.NUM;
        //void addDirectionalBinding(DirActionType dirType, bool addAlternative, GenericControllerAxis axis, IDirectionalMap newMap, Dimensions dimension)
        //{
        //    //if (dimension == Dimensions.X)
        //    //{
        //    //    storedXaxis = axis;
        //    //    addGenericDualAxesBindingStep(dirType, addAlternative, Dimensions.Y);
        //    //}
        //    //else
        //    //{
        //    //    int i = (int)dirType;
        //    //    if (addAlternative)
        //    //        player.hero.inputMap.directionalMappings[i] = new AlternativeDirectionalMap(player.hero.inputMap.directionalMappings[i], newMap);
        //    //    else
        //    //        player.hero.inputMap.directionalMappings[i] = newMap;

        //    //    menu.PopLayouts(4);
        //    //}
        //}

        //void addGenericDualAxesBindingStep(DirActionType dirType, bool addAlternative, Dimensions dimension)
        //{
            

        //    GuiLayout layout = new GuiLayout("Bind " + dirType.ToString(), menu);
        //    {
        //        new GuiLabel("Choose " + dimension.ToString() + " axis", layout);

        //        if (dimension == Dimensions.Y)
        //        {
        //            new GuiCheckbox("Invert Y", null, invertYProperty, layout);
        //        }

        //        new GuiSectionSeparator(layout);
        //        for (GenericControllerAxis axis = 0; axis < GenericControllerAxis.NUM2; ++axis)
        //        {
        //            if (axis == GenericControllerAxis.NUM)
        //            {
        //                axis = GenericControllerAxis.START2_MINUS1 + 1;
        //            }
        //            //GenericControllerAxis x = (GenericControllerAxis)i;

        //            //new GuiTextButton(x.ToString(), null,
        //            //    new GuiAction3Arg<DirActionType, bool, GenericControllerAxis>(
        //            //        addGenericDualAxesBindingStepY,
        //            //        map,
        //            //        addAlternative,
        //            //        x),
        //            //    false, layout);
        //            GenericControllerAxis x, y;
        //            if (dimension == Dimensions.X)
        //            {
        //                x = axis; y = GenericControllerAxis.NUM;
        //            }
        //            else
        //            {
        //                x = storedXaxis; y = axis; 
        //            }
        //            var dirmap = new DirectionalGenericDualAxesMap(x, y, false, player.inputMap.controllerIndex);

        //            new GuiDirectionalMapButton(axis.ToString(), dirmap,
        //                new GuiAction5Arg<DirActionType, bool, GenericControllerAxis, IDirectionalMap, Dimensions>(
        //                    addDirectionalBinding,
        //                    dirType,
        //                    addAlternative,
        //                    axis,
        //                    dirmap, 
        //                    dimension),

        //                false, layout);
        //        }

        //        //for (int i = (int)GenericControllerAxis.START2_MINUS1 + 1; i != (int)GenericControllerAxis.NUM2; ++i)
        //        //{
        //        //    GenericControllerAxis x = (GenericControllerAxis)i;

        //        //    var dirmap = new DirectionalGenericDualAxesMap(x, GenericControllerAxis.NUM, false, player.inputMap.controllerIndex);

        //        //    new GuiDirectionalMapButton(dirmap,
        //        //        new GuiAction3Arg<DirActionType, bool, GenericControllerAxis>(
        //        //            addGenericDualAxesBindingStepY,
        //        //            map,
        //        //            addAlternative,
        //        //            x),
        //        //        false, layout);


        //        //    //new GuiTextButton(x.ToString(), null,
        //        //    //    new GuiAction3Arg<DirActionType, bool, GenericControllerAxis>(
        //        //    //        addGenericDualAxesBindingStepY,
        //        //    //        map,
        //        //    //        addAlternative,
        //        //    //        x),
        //        //    //    false, layout);
        //        //}
        //    }
        //    layout.End();
        //}

        

        //void addGenericDualAxesBindingStepY(DirActionType map, bool addAlternative, GenericControllerAxis x)
        //{
        //    GuiLayout layout = new GuiLayout("Bind " + map.ToString(), menu);
        //    {
        //        new GuiLabel("Choose Y axis", layout);
        //        new GuiCheckbox("Invert Y", null, invertYProperty, layout);
        //        new GuiSectionSeparator(layout);
        //        for (int i = 0; i != (int)GenericControllerAxis.NUM; ++i)
        //        {
        //            GenericControllerAxis y = (GenericControllerAxis)i;
        //            new GuiTextButton(y.ToString(), null,
        //                new GuiAction3Arg<DirActionType, bool, IDirectionalMap>(
        //                    addDirectionalBinding,
        //                    map,
        //                    addAlternative,
        //                    new DirectionalGenericDualAxesMap(x, y, invertY, player.PlayerIndex)),
        //                false, layout);
        //        }

        //        for (int i = (int)GenericControllerAxis.START2_MINUS1 + 1; i != (int)GenericControllerAxis.NUM2; ++i)
        //        {
        //            GenericControllerAxis y = (GenericControllerAxis)i;
        //            new GuiTextButton(y.ToString(), null,
        //                new GuiAction3Arg<DirActionType, bool, IDirectionalMap>(
        //                    addDirectionalBinding,
        //                    map,
        //                    addAlternative,
        //                    new DirectionalGenericDualAxesMap(x, y, invertY, player.PlayerIndex)),
        //                false, layout);
        //        }
        //    }
        //    layout.End();
        //}

        //void addXboxStickBinding(DirActionType map, bool addAlternative)
        //{
        //    GuiLayout layout = new GuiLayout("Bind " + map.ToString(), menu);
        //    {
        //        new GuiCheckbox("Invert Y", null, invertYProperty, layout);
        //        new GuiTextButton("Left Stick", null,
        //            new GuiAction5Arg<DirActionType, bool, GenericControllerAxis, IDirectionalMap, Dimensions>(
        //                addDirectionalBinding, map, addAlternative, GenericControllerAxis.NUM, 
        //                new DirectionalXboxMap(ThumbStickType.Left, invertY, (int)player.PlayerIndex), Dimensions.Y),
        //            false, layout);
        //        new GuiTextButton("Right Stick", null,
        //           new GuiAction5Arg<DirActionType, bool, GenericControllerAxis, IDirectionalMap, Dimensions>(
        //                addDirectionalBinding, map, addAlternative, GenericControllerAxis.NUM, 
        //                new DirectionalXboxMap(ThumbStickType.Right, invertY, (int)player.PlayerIndex), Dimensions.Y),
        //            false, layout);
        //        new GuiTextButton("D-Pad", null,
        //            new GuiAction5Arg<DirActionType, bool, GenericControllerAxis, IDirectionalMap, Dimensions>(
        //                addDirectionalBinding, map, addAlternative, GenericControllerAxis.NUM, 
        //                new DirectionalXboxMap(ThumbStickType.D, invertY, (int)player.PlayerIndex), Dimensions.Y),
        //            false, layout);
        //    }
        //    layout.End();
        //}

       

        void pageBindButton(ButtonActionType map)
        {
            GuiLayout layout = new GuiLayout("Bind " + map.ToString(), menu);
            {
                new GuiTextButton("Replace", null, new GuiAction2Arg<ButtonActionType, bool>(pageAddAlternativeBtn, map, false), true, layout);
                new GuiTextButton("Add alternative", null, new GuiAction2Arg<ButtonActionType, bool>(pageAddAlternativeBtn, map, true), true, layout);
                //new GuiTextButton("Button + Dir", null, aoeuaoeuaoeu, true, layout); // KeyPlusDirectionalMap
            }
            layout.End();
        }
        
        //SpriteName BtnMapToSpriteName(object map)
        //{
        //    return player.hero.inputMap.buttonMappings[(int)map].Icon;
        //}

        //string BtnMapToString(object map)
        //{
        //    return player.hero.inputMap.buttonMappings[(int)map].ButtonName;
        //}

        //string DirMapToString(object map)
        //{
        //    return player.hero.inputMap.directionalMappings[(int)map].directionsName;
        //}

        void pageButtonMapBindings()
        {
            //GuiLayout layout = new GuiLayout("Button Mapping", menu);
            //{
            //    for (int i = 0; i < (int)ButtonActionType.NUM_NON; ++i)
            //    {
            //        ButtonActionType mapId = (ButtonActionType)i;

            //        IGuiAction action = new GuiAction1Arg<ButtonActionType>(pageBindButton, mapId);

            //        new GuiLabel(mapId.ToString(), layout); // TODO(Martin): Prettier formatting?
            //        new GuiIconTextButton(new DelayedGetCall1Arg<SpriteName>(BtnMapToSpriteName, mapId), new DelayedGetCall1Arg<string>(BtnMapToString, mapId), null, action, true, layout);
            //    }
            //}
            //layout.End();
        }

        //void pageBindDirection(DirActionType map)
        //{
        //    GuiLayout layout = new GuiLayout("Bind " + map.ToString(), menu);
        //    {
        //        new GuiTextButton("Replace", null, new GuiAction2Arg<DirActionType, bool>(pageAddAlternativeDir, map, false), true, layout);
        //        new GuiTextButton("Add alternative", null, new GuiAction2Arg<DirActionType, bool>(pageAddAlternativeDir, map, true), true, layout);
        //    }
        //    layout.End();
        //}

        void pageDirectionMapBindings()
        {
            //GuiLayout layout = new GuiLayout("Directions Mapping", menu);
            //{
            //    for (int i = 0; i < (int)DirActionType.NUM_NON; ++i)
            //    {
            //        DirActionType mapId = (DirActionType)i;
            //        IDirectionalMap map = player.hero.inputMap.directionalMappings[i];
            //        new GuiLabel(mapId.ToString(), layout); // TODO(Martin): Prettier formatting?
            //        //new GuiTextButton(new DelayedGetCall1Arg<string>(DirMapToString, mapId), null, 
            //        //    new GuiAction1Arg<DirActionType>(pageBindDirection, mapId), true, layout);
            //        new GuiDirectionalMapButton(mapId.ToString(), map, 
            //            new GuiAction1Arg<DirActionType>(pageBindDirection, mapId), true, layout);

            //    }
            //}
            //layout.End();
        }

        void pageKeyBindings()
        {
            //GuiLayout layout = new GuiLayout("Key bindings", menu);
            //{
            //    if (player.pData.inputMap.UsingSteamController)
            //    {
            //        new GuiTextButton("Steam Controller", null, player.pData.inputMap.OpenSteamControllerBindingGUI, true, layout);
            //    }
            //    new GuiTextButton("Buttons", null, pageButtonMapBindings, true, layout);
            //    new GuiTextButton("Directions", null, pageDirectionMapBindings, true, layout);
                
            //}
            //layout.End();
        }

        //bool systemLinkProperty(bool set, bool value)
        //{
        //    if (set) Ref.netSession.IsSystemLink = value;
        //    return Ref.netSession.IsSystemLink;
        //}

        float ssaoSampleRadius(bool set, float value)
        {
            SSAO ssao = ((DeferredRenderer)Ref.draw).ssao;
            if (set) ssao.SampleRadius = value;
            return ssao.SampleRadius;
        }

        float ssaoOcclusionPower(bool set, float value)
        {
            SSAO ssao = ((DeferredRenderer)Ref.draw).ssao;
            if (set) ssao.OcclusionPower = value;
            return ssao.OcclusionPower;
        }

        float ssaoAmount(bool set, float value)
        {
            DeferredRenderer draw = ((DeferredRenderer)Ref.draw);
            if (set) draw.SSAOAmount = value;
            return draw.SSAOAmount;
        }

        float lightMapAmount(bool set, float value)
        {
            DeferredRenderer draw = ((DeferredRenderer)Ref.draw);
            if (set) draw.LightMapAmount = value;
            return draw.LightMapAmount;
        }

        bool ssaoEnable(int index, bool set, bool value)
        {
            DeferredRenderer renderer = (DeferredRenderer)Ref.draw;
            if (set) renderer.EnableSSAO = value;
            return renderer.EnableSSAO;
        }

        bool ssaoDebug(int index, bool set, bool value)
        {
            DeferredRenderer renderer = (DeferredRenderer)Ref.draw;
            if (set) renderer.DebugSSAO = value;
            return renderer.DebugSSAO;
        }

        bool debugRenderTargets(int index, bool set, bool value)
        {
            DeferredRenderer renderer = (DeferredRenderer)Ref.draw;
            if (set) renderer.DebugTargets = value;
            return renderer.DebugTargets;
        }

        void ssaoOptions()
        {
            GuiLayout layout = new GuiLayout("Graphics Options", menu);
            {
                //new GuiTextButton("Reload Shaders", null, ((DeferredRenderer)Ref.draw).ReloadShaders, false, layout);
                new GuiCheckbox("DBG Render Targets", null, debugRenderTargets, layout);
                new GuiLabel("SSAO", layout);
                new GuiCheckbox("Enable SSAO", null, ssaoEnable, layout);
                new GuiCheckbox("Debug SSAO", null, ssaoDebug, layout);
                new GuiFloatSlider(SpriteName.NO_IMAGE, "Sample radius", ssaoSampleRadius, new IntervalF(0, 7), false, layout);
                new GuiFloatSlider(SpriteName.NO_IMAGE, "Occlusion Power", ssaoOcclusionPower, new IntervalF(0, 10), false, layout);
                new GuiFloatSlider(SpriteName.NO_IMAGE, "Ambience", ssaoAmount, new IntervalF(0, 1), false, layout);
                new GuiFloatSlider(SpriteName.NO_IMAGE, "Lighting", lightMapAmount, new IntervalF(0, 1), false, layout);
            }
            layout.End();
        }

        void monitorSelect(GuiLayout layout)
        {
            if (GraphicsAdapter.Adapters.Count > 1)
            {
                List<GuiOption<GraphicsAdapter>> options = new List<GuiOption<GraphicsAdapter>>();
                for (int i = 0; i < GraphicsAdapter.Adapters.Count; ++i)
                {
                    GraphicsAdapter monitor = GraphicsAdapter.Adapters[i];
                    options.Add(new GuiOption<GraphicsAdapter>((i + 1).ToString() + ": " + monitor.CurrentDisplayMode.Width.ToString() + "x" + monitor.CurrentDisplayMode.Height.ToString(), monitor));
                }
                new GuiOptionsList<GraphicsAdapter>(SpriteName.NO_IMAGE, "Monitor Select", options, monitorProperty, layout);
            }
        }

        //void resolutionOptions(GuiLayout layout)
        //{
        //    Ref.pc_gamesett.listMonitors(layout);

        //    var resoutionPercOptions = Engine.Screen.ResoutionPercOptions();

        //    List<GuiOption<int>> optionsList = new List<GuiOption<int>>();
        //    foreach (var m in resoutionPercOptions)
        //    {
        //        optionsList.Add(new GuiOption<int>(m.ToString() + "%", m));
        //    }

        //    new GuiOptionsList<int>("Resolution", optionsList, Ref.pc_gamesett.resolutionPercProperty, layout);
        //    new GuiCheckbox("Fullscreen", null, Ref.pc_gamesett.fullscreenProperty, layout);

        //    new GuiTextButton("Recording presets", null, recordingResolutionOptions, true, layout);
        //}
        //void recordingResolutionOptions()
        //{
        //    GuiLayout layout = new GuiLayout("Recording setup", menu);
        //    {
        //        var monitor = Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter;
        //        for (RecordingPresets rp = 0; rp < RecordingPresets.NumNon; ++rp)
        //        {
        //            IntVector2 sz = Engine.Screen.RecordingPresetsResolution(rp);
        //            if (sz.Y > monitor.CurrentDisplayMode.Height)
        //            {
        //                break;
        //            }
        //            else
        //            {
        //                if (rp == Engine.Screen.UseRecordingPreset)
        //                {
        //                    new GuiIconTextButton(SpriteName.LfCheckYes, rp.ToString(), null,
        //                        new GuiAction1Arg<RecordingPresets>(Ref.pc_gamesett.setRecordingPreset, rp), false, layout);
        //                }
        //                else
        //                {
        //                    new GuiTextButton(rp.ToString(), null,
        //                        new GuiAction1Arg<RecordingPresets>(Ref.pc_gamesett.setRecordingPreset, rp), false, layout);
        //                }
        //            }
        //        }
        //    }
        //    layout.End();
        //}

        void pageOptions()
        {
            GuiLayout layout = new GuiLayout("Options", menu);
            {
                // Controls
                Ref.gamesett.optionsMenu(layout);

                new GuiSectionSeparator(layout);

                if (DebugSett.EnableKeyrebindMenu)
                    new GuiTextButton("Key bindings", null, pageKeyBindings, true, layout);
                               
                
                // Graphics
                //resolutionOptions(layout);

                new GuiSectionSeparator(layout);

                if (LfRef.LocalHeroes.Count > 1)
                    new GuiCheckbox("Horizontal split", null, horiSplitProperty, layout);
                // other graphics
                new GuiTextButton("Camera", null, cameraSettings, true, layout);
                new GuiOptionsList<int>(SpriteName.NO_IMAGE, "Frame rate", new List<GuiOption<int>>
                {
                    new GuiOption<int>(30), new GuiOption<int>(60), new GuiOption<int>(90), 
                    new GuiOption<int>(100), new GuiOption<int>(120), new GuiOption<int>(144)
                }, frameRateProperty, layout);

                new GuiOptionsList<int>(SpriteName.NO_IMAGE, "Detail level", new List<GuiOption<int>>
                {
                    new GuiOption<int>("Low", 0), 
                    new GuiOption<int>("Medium", 1), 
                    new GuiOption<int>("High", 2),
                }, detailLevelProperty, layout);

                new GuiIntSlider(SpriteName.NO_IMAGE, "Chunk radius", chunkRadiusProperty, new IntervalF(2, Map.WorldPosition.MaxChunkRadiusGenerating), false, layout);

                if (Ref.draw is DeferredRenderer)
                    new GuiTextButton("Advanced Graphics", null, ssaoOptions, true, layout);
                
                
                new GuiSectionSeparator(layout);
                
                // Sounds
                //new GuiFloatSlider(SpriteName.NO_IMAGE, "Music Volume", musicVolProperty, new IntervalF(0, 1), false, layout);
                //new GuiFloatSlider(SpriteName.NO_IMAGE, "Sound Volume", soundVolProperty, new IntervalF(0, 1), false, layout);
                //new GuiTextButton("Next song", null, Ref.music.nextRandomSong, false, layout);
                //if (PlatformSettings.PlayMusic)
                //{ new GuiDelegateLabel(SongTitleProperty, layout); }
                
                new GuiSectionSeparator(layout);
                
                if (PlatformSettings.PC_platform)
                {
                    // other
                    //new GuiCheckbox("Auto join co-op levels", null, autoJoinLevelProperty, layout);
                }
                new GuiTextButton("Credits", null, credits, true, layout);
                
            }
            layout.End();
        }

        public void listTransportLocations()
        {
            //player.createMenu();

            GuiLayout layout = new GuiLayout("Travel To", menu);
            {
                if (LfRef.AllHeroes.Count > 1)
                {
                    for (int i = 0; i < LfRef.AllHeroes.Count; ++i)
                    {
                        var h = LfRef.AllHeroes[i];
                        if (h != player.hero)
                        {
                            var button = new GuiIconTextButton(SpriteName.MissingImage, 
                                h.absPlayer.pData.PublicName(menu.style.textFormat.Font),
                                null, new GuiAction1Arg<AbsHero>(player.hero.teleportToPlayer, h),
                                false, layout);

                            new LoadGamerIcon(button.icon, h.absPlayer.pData.netPeer(), h.absPlayer.Local);
                        }
                    }
                    new GuiSectionSeparator(layout);
                }

                if (player.Storage.progress.StoredBabyLocations.Get(BabyLocation.Introduction))
                {

                    for (TeleportLocationId pos = 0; pos < TeleportLocationId.NUM_NON; ++pos)
                    {
                        var loc = LfLib.Location(pos);
                        if (loc.canSelectTravelTo && player.Storage.progress.canTravelTo(loc.location))
                        {
                            bool atLocation = false;

                            if (player.hero.isInLevel(loc.level))
                            {
                                if ((loc.wp.ChunkGrindex - player.hero.WorldPos.ChunkGrindex).SideLength() <= 2)
                                {
                                    atLocation = true;
                                }
                            }

                            if (atLocation == false)
                            {
                                new GuiTextButton(pos.ToString(), null, 
                                    new GuiAction1Arg<TeleportLocationId>(player.hero.teleportToLocation, pos), 
                                    false, layout);
                            }
                        }
                    }
                }
                else
                {
                    new GuiLabel("Must save 1 baby", layout);
                }
            } layout.End();
        }

        //void transportTo(TeleportLocationId to)
        //{
        //    player.hero.teleportToLocation
        //}

        public void SwapTargetResolutionCoordinates()
        {
            Engine.Screen.PcTargetResolution = new IntVector2(Engine.Screen.PcTargetResolution.Y, Engine.Screen.PcTargetResolution.X);
        }

        //string SongTitleProperty(bool set, string value)
        //{
        //    return "Playing: \n" + Ref.music.GetSongName();
        //}


        Color GuiColorPicker(bool set, Color value)
        {
            return GetSet.Do<Color>(set, ref guiColor, value);
        }

        GraphicsAdapter monitorProperty(bool set, GraphicsAdapter val)
        {
            if (set)
            {
                Screen.Monitor = val;
                Screen.ApplyScreenSettings();
            }

            return Screen.Monitor;
           // return GetSet.Do<GraphicsAdapter>(set, ref Screen.monitor, val);
        }

        void cameraSettings()
        {
            GuiLayout layout = new GuiLayout("Camera Settings", menu);
            {
                new GuiCheckbox("Inverted Y look", null, invertCamYProperty, layout);
                IntervalF camSpeedRange = new IntervalF(1, 10);
                new GuiFloatSlider(SpriteName.NO_IMAGE, "Horizontal speed", player.Storage.camSpeedXProperty, camSpeedRange, false, layout);
                new GuiFloatSlider(SpriteName.NO_IMAGE, "Vertical speed", player.Storage.camSpeedYProperty, camSpeedRange, false, layout);
                {
                    FloatGetSet fovProperty;
                    if (player.localPData.view.camType == Graphics.CameraType.TopView)
                        fovProperty = player.Storage.CamTopViewFOVProperty;
                    else
                        fovProperty = player.Storage.CamFirstPersonFOVProperty;
                    new GuiFloatSlider(SpriteName.NO_IMAGE, "Field Of View", fovProperty, Graphics.AbsCamera.PlayerSettingsFOVBounds, false, layout);
                }
                new GuiFloatSlider(SpriteName.NO_IMAGE, "Chase speed", cameraChaseSpeed, new IntervalF(Player.MinHeroCamChaseSpeed, Player.MaxHeroCamChaseSpeed), false, layout);

                if (player.localPData.view.Camera is VikingEngine.Graphics.TopViewCamera)
                {
                    new GuiCheckbox("Terrain collisions", "If checked, camera won't intersect terrain", cameraTerrainCollisionsProperty, layout);
                    new GuiCheckbox("Instant zoom in", "If unchecked, camera acts like a spring", cameraZoomInProperty, layout);
                    new GuiCheckbox("Instant zoom out", "If unchecked, camera acts like a spring", cameraZoomOutProperty, layout);
                }
            }
            layout.End();
        }

        float cameraChaseSpeed(bool set, float value)
        {
            if (set)
                player.localPData.view.Camera.positionChaseLengthPercentage = value;
            return player.localPData.view.Camera.positionChaseLengthPercentage;
        }

        bool cameraTerrainCollisionsProperty(int index, bool set, bool value)
        {
            return GetSet.Do<bool>(set, ref ((VikingEngine.Graphics.TopViewCamera)player.localPData.view.Camera).UseTerrainCollisions, value);
        }
        bool cameraZoomInProperty(int index, bool set, bool value)
        {
            return GetSet.Do<bool>(set, ref ((VikingEngine.Graphics.TopViewCamera)player.localPData.view.Camera).InstantZoomIn, value);
        }
        bool cameraZoomOutProperty(int index, bool set, bool value)
        {
            return GetSet.Do<bool>(set, ref ((VikingEngine.Graphics.TopViewCamera)player.localPData.view.Camera).InstantZoomOut, value);
        }

        private void credits()
        {
            GuiLayout layout = new GuiLayout("Credits", menu, GuiLayoutMode.SingleColumn, true);
            {
                LoadedFont font = menu.style.textFormat.Font;
                menu.style.textFormat.Font = LoadedFont.Regular;

                new GuiLabel("Lootfest for Steam Early Access, " + PlatformSettings.SteamVersion, layout);
                
                new GuiLabel("Art, Design & Lead Programmer:\n" + "Fabian \"Viking\" Jakobsson\n" + "www.vikingfabian.com", layout);
                new GuiLabel("Music, Sounds & Tech Programming:\n" + "Martin \"Akri\" Grönlund", layout);
                new GuiLabel("Main playtesters: \n" + "Pontus Bengtsson\n" + "Alex \"Smithy\" Hamelin", layout);


                menu.style.textFormat.Font = font;
            }
            layout.End();
        }

        int detailLevelProperty(bool set, int value)
        {
            return GetSet.Do<int>(set, ref Ref.gamesett.DetailLevel, value);
        }

        int frameRateProperty(bool set, int value)
        {
            if (set)
            {
                Ref.gamesett.FrameRate = value;
                Engine.Update.SetFrameRate(Ref.gamesett.FrameRate);
            }
            return Ref.gamesett.FrameRate;
        }

        int chunkRadiusProperty(bool set, int value)
        {
            if (set)
            {
                Ref.gamesett.ChunkLoadRadius = value;
                LfRef.world.RefreshChunkLoadRadius();
            }
            return Ref.gamesett.ChunkLoadRadius;
        }
        //float musicVolProperty(bool set, float value)
        //{
        //    if (set) Ref.music.SetVolume(value);
        //    return Sound.MusicPlayer.MusicMasterVolume;
        //}
        //float soundVolProperty(bool set, float value)
        //{
        //    return GetSet.Do<float>(set, ref Engine.Sound.SoundVolume, value);
        //}
        bool horiSplitProperty(int index, bool set, bool value)
        {
            if (set)
            {
                Engine.Draw.horizontalSplit = value;
                LfRef.gamestate.UpdateSplitScreen();
            }
            return Engine.Draw.horizontalSplit;
        }
        bool invertCamYProperty(int index, bool set, bool value)
        {
            if (set)
            {
                player.Storage.CamInvertY = value;
                player.refreshCamSettings();
            }
            return player.Storage.CamInvertY;
        }

        

        //bool fullscreenProperty(bool set, bool value)
        //{
        //    if (set)
        //    {
        //        Engine.Screen.PcTargetFullScreen = value;
        //        Engine.Screen.ApplyScreenSettings();
        //    }
        //    return Engine.Screen.PcTargetFullScreen;
        //}
        bool autoJoinLevelProperty(int index, bool set, bool value)
        {
            return GetSet.Do<bool>(set, ref Ref.gamesett.AutoJoinToCoopLevel, value);
        }

        //IntVector2 resolutionProperty(bool set, IntVector2 res)
        //{
        //    return GetSet.Do<IntVector2>(set, ref Engine.Screen.PcTargetResolution, res);
        //}

        //int resolutionPercProperty(bool set, int res)
        //{
        //    if (set)
        //    {
        //        Engine.Screen.RenderScalePerc = res;
        //        Engine.Screen.ApplyScreenSettings();
        //    }
        //    return Engine.Screen.RenderScalePerc;
        //}

        void steamSettings()
        {
#if PCGAME
            GuiLayout layout = new GuiLayout("Steam Debug", menu);
            {
                //new GuiDialogButton("Reset achievements", null,
                //    new GuiAction(Ref.instance.achievementsKeeper.ResetAllAchievements), 
                //    false, layout);
                if (DebugSett.DebugNetwork)
                {
                    new GuiTextButton("Create Lobby", "Creates a lobby on the Steam servers", Ref.steam.LobbyMatchmaker.CreateLobby, false, layout);
                    new GuiTextButton("Find Lobbies", "Searches Steam servers for lobbies", Ref.steam.LobbyMatchmaker.FindLobbies, false, layout);
                    new GuiTextButton("Send dbg_chat_msg", "Broadcasts a random debug chat message in the current lobby", Ref.steam.LobbyMatchmaker.SendDbgChat, false, layout);
                    new GuiTextButton("Send \"Hello!\" packet", "Sends a byte-encoded string to all connected peers", DebugSendHello, false, layout);
                    new GuiTextButton("Connect to self.", "", ConnectToSelf, false, layout);
                    new GuiTextButton("Invite user", "Opens a steam dialog to invite a user to your game", Ref.steam.LobbyMatchmaker.InviteSteamUserToLobbyDialog, false, layout);
                }
            }
            layout.End();
#endif
        }

        void DebugSendHello()
        {
#if PCGAME
            Ref.steam.P2PManager.SendReliable(SteamWrapping.SteamLobbyMatchmaker.EncodeString("Hello!"));
#endif
        }

        void ConnectToSelf()
        {
#if PCGAME
            //Ref.steam.P2PManager.AddPeer(SteamAPI.SteamUser().GetSteamID());
#endif
        }

        void startVoxelEditor()
        {
            new GameState.VoxelDesignState(player.PlayerIndex);
        }
        void startSceneMaker()
        {
            new Editor.SceneMaker(player.PlayerIndex);
        }

        //void HandMadeTerrainPage()
        //{
        //    GuiLayout layout = new GuiLayout("HandMadeTerrain models", menu);
        //    {
        //        new GuiTextButton("Load all HMT models", "Load all HandMadeTerrain models from disk.", LfRef.terrain.LoadAllHandmadeTerrain, false, layout);

        //        //new GuiTextButton("Export HMT model data", "Export all HandMadeTerrain model data to disk.", LfRef.terrain.ExportModelData, false, layout);

        //        //new GuiTextButton("Import HMT model data", "Import all HandMadeTerrain model data from disk.", LfRef.terrain.ImportModelData, false, layout);

        //        new GuiTextButton("Debug Print", "Print all imported HMT model data.", LfRef.terrain.DebugPrintAll, false, layout);
        //    }
        //    layout.End();
        //}

        //void TestOfTheDay()
        //{
        //    if (gtest == null)
        //    {
        //        gtest = new VikingEngine.LootFest.Map.Terrain.Generation.GTester();
        //    }
         
        //    gtest.ResetTest();
        //}

        void pageDebug()
        {
#if PCGAME
            LfRef.stats.debugMenuVisits.value++;
#endif
            GuiLayout layout = new GuiLayout("**Debug**", menu);
            {
                new GuiCheckbox("God mode", "Move fast, and be invincible", player.toggleDebugMode, layout);
                new GuiTextButton("Level Overview", null, debugLevelMap, false, layout);
                //new GuiTextButton("Jump to Lobby", null, new GuiAction1Arg<TeleportLocationId>(player.debugJumpTo, TeleportLocationId.Lobby), false, layout);
                //new GuiTextButton("Jump to Tutorial", null, new GuiAction1Arg<TeleportLocationId>(player.debugJumpTo, TeleportLocationId.TutorialStart), false, layout);
                new GuiTextButton("Restart", null, restart, false, layout);
                new GuiTextButton("Empty state", null, emptyState, false, layout);


                new GuiTextButton("Spawn monster", null, debugSpawnEnemyMenu, true, layout);
                new GuiTextButton("Spawn item", null, debugSpawnItem, true, layout);
                
                //new GuiTextButton("Bound manager", null, new GuiAction1Arg<Gui>(LfRef.bounds.ToMenu, menu), true, layout);
                //new GuiTextButton("Today's test", null, TestOfTheDay, false, layout);
                if (PlatformSettings.SteamAPI && PlatformSettings.Debug_SteamAPI)
                {
                    new GuiTextButton("Steam Debug", null, steamSettings, true, layout);
                }
                if (PlatformSettings.DevBuild)
                {
                    new GuiTextButton("All cards but 1", "Will miss a Hog capture", debugGetAllcardsButOne, false, layout);

                    //new GuiTextButton("HMT model data", "Debug import/export of HandMadeTerrain model data", HandMadeTerrainPage, true, layout);

                    new GuiTextButton("List hosted objects(" + LfRef.gamestate.GameObjCollection.LocalMembers.Count + ")",
                        "All things that move around in the game", new GuiAction1Arg<bool>(DebugListGameObjects, true), true, layout);
                    new GuiTextButton("List client objects(" + LfRef.gamestate.GameObjCollection.ClientMembers.Count + ")",
                        "All things that a network visitor spawn", new GuiAction1Arg<bool>(DebugListGameObjects, false), true, layout);

                    new GuiTextButton("Lights and shadows", "List all objects that has a light or shadow connected to it",
                        new GuiActionOpenPage(Director.LightsAndShadows.Instance.DebugInfo), true, layout);
                }
                    //DataStream.DataStreamLib.BrowseButton(mFile);
                new GuiTextButton("Crash game", "Test the blue screen", debugCrashGame, false, layout);
                new GuiTextButton("Take 1 damage", null, debugTake1Damage, false, layout);
                new GuiTextButton("Chunk status", "Orient the screen so -X is left", DebugChunkStatus, false, layout);
                new GuiTextButton("Remove Suit", null, debugRemoveSuit, false, layout);
                new GuiTextButton("Place position marker", null, debugPositionMark, false, layout);
                //new GuiTextButton("Unlock all levels", null, new GuiAction(player.Storage.debugUnlockAllLevels, player.CloseMenu), false, layout);
                new GuiTextButton("Unlock all achievements", null, debugUnlockAchievements, false, layout);
                new GuiTextButton("Model spammer", "Create models until the graphics buffer crashes", debugModelSpam, false, layout);
                new GuiTextButton("Models autoloader", null, debugModelsAutoLoader, false, layout);
                
                
                //new GuiTextButton("Add 100 coins", null, debugUnlockAchievements, false, layout);

                //mFile.AddTextLink("Change chunk", "add blocks to force the chunk to update and be stored", new HUD.ActionLink(debugChangeChunk));
                //mFile.AddTextLink("List changed chunks", "those have been changed and stored in mem", (int)Link.DeugListChangedChunks);
                //new GuiTextButton("Performance: Vox models", "All generated 3d models in render loop",
                //   new GuiActionOpenPage(Ref.draw.ListGeneratedModels), true, layout);
                //new GuiTextButton("Performance: Meshes", "All 3d models in render loop", 
                //    new GuiActionOpenPage(Ref.draw.ListMeshes), true, layout);
                //new GuiTextButton("Performance: Update", "All objects that is in update list", 
                //    new GuiActionOpenPage(Ref.update.UpdateListToFile), true, layout);
                new GuiTextButton("Hide terrain mesh", null, debugHideTerrain, false, layout);

#if PCGAME
                Ref.steam.debugInfoToMenu(layout);
#endif

                new GuiLabel("Hero pos:" + player.hero.Position.ToString(), false, layout.gui.style.textFormatDebug, layout);
                new GuiLabel("Chunk:" + player.hero.WorldPos.ChunkGrindex.ToString(), false, layout.gui.style.textFormatDebug, layout);
                new GuiLabel("Hero objIx:" + player.hero.ObjOwnerAndId.ToString(), false, layout.gui.style.textFormatDebug, layout);
                //new GuiLabel("World Seed:" + LfRef.levels.WorldSeed.seed.ToString(), false, layout.gui.style.textFormatDebug, layout);
                //if (player.hero.worldLevel != null)
                //{ new GuiLabel("Level Seed:" + player.hero.worldLevel.Seed.ToString(), layout.gui.style.textFormatDebug, layout); }
                //if (player.hero.subLevel != null)
                //{ new GuiLabel("Level Seed:" + player.hero.subLevel.WorldLevel.Seed.ToString(), layout.gui.style.textFormatDebug, layout); }
                //if (player.hero != null && player.hero.subLevel != null && player.hero.subLevel.WorldLevel != null)
                //{ new GuiLabel("Level Seed:" + player.hero.subLevel.WorldLevel.seed.ToString(), false, layout.gui.style.textFormatDebug, layout); }
            }
            layout.End();
        }

        void debugLevelMap()
        {
            if (player.hero.Level != null)
            {
                new VikingEngine.LootFest.BlockMap.MapDebugOverview(player.hero.Level);
            }
        }

        void emptyState()
        {
#if PCGAME
            //var performance = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
            //var memory = performance.NextValue();

            //Debug.Log("RAM (MB): " + (memory).ToString());
#endif
            new GameState.EmptyState();
        }

        void debugUnlockAchievements()
        {
            for (int i = 0; i < player.Storage.progress.achievements.Length; ++i)
            {
                player.Storage.progress.achievements[i] = true;

            }
            pageTrophies();
        }

        void debugGetAllcardsButOne()
        {
            for (int i = 0; i < player.Storage.progress.cardCollection.Length; ++i)
            {
                if (i == (int)CardType.Hog)
                {
                    player.Storage.progress.cardCollection[i] = new CardCaptures();
                }
                else
                {
                    player.Storage.progress.cardCollection[i] = player.Storage.progress.cardCollection[i].AddOne();
                }
            }
            pageCards();
        }

        void debugRemoveSuit()
        {
            player.Gear.dressInSuit(SuitType.Basic);
        }
        void debugPositionMark()
        {
            new Graphics.Mesh(LoadedMesh.cube_repeating, player.hero.Position, new Vector3(2, 92, 2),
                Graphics.TextureEffectType.Flat, SpriteName.WhiteArea, Color.Purple);
                //new Graphics.TextureEffect(Graphics.TextureEffectType.Flat, SpriteName.WhiteArea, Color.Purple),
                //, Vector3.Zero);
            player.CloseMenu();
        }

        void debugHideTerrain()
        {
            Ref.draw.DrawGround = !Ref.draw.DrawGround;
        }
        void DebugChunkStatus()
        {
            player.localPData.view.Camera.TiltX = MathHelper.PiOver2;
            IntVector2 pos = IntVector2.Zero;
        }

        void debugTake1Damage()
        {
            player.CloseMenu();
            player.hero.TakeDamage(new GO.WeaponAttack.DamageData(1f), true);
        }

        void debugCrashGame()
        {
            throw new Exception("Debug crash test");
        }

        void DebugListGameObjects(bool local)
        {
            LfRef.gamestate.GameObjCollection.DebugListGameObjects(local, menu);
        }
        

        void debugSpawnEnemyMenu()
        {
            GuiLayout layout = new GuiLayout("Spawn Enemy Menu", menu);
            {
                new GuiTextButton("Spawn All", null, debugSpawnEnemyAll, false, layout);
                foreach (GO.GameObjectType t in Director.GameObjectSpawn.availableMonsterTypes)
                {
                    int levels = Director.GameObjectSpawn.availableMonsterLevels(t);

                    for (int lvl = 0; lvl < levels; ++lvl)
                    {
                        new GuiTextButton(t.ToString() + TextLib.IndexToString(lvl), null, new HUD.GuiAction1Arg<GoArgs>(debugSpawnEnemyType, new GoArgs(t, lvl)), false, layout);
                    }
                }
            }
            layout.End();
        }

       

        void debugSpawnEnemyAll()
        {
            foreach (GO.GameObjectType t in Director.GameObjectSpawn.availableMonsterTypes)
            {
                int levels = Director.GameObjectSpawn.availableMonsterLevels(t);

                for (int lvl = 0; lvl < levels; ++lvl)
                {
                    debugSpawnEnemyType(new GoArgs(t, lvl));
                }
            }
        }
        void debugSpawnEnemyType(GoArgs args)
        {
            args.startWp = player.spawnNextToHeroPos;
            Director.GameObjectSpawn.SpawnMonster(args);
        }

        
        void debugSpawnItem()
        {
            GO.GameObjectType[] availablePickup = new GO.GameObjectType[]
            {
                 GO.GameObjectType.Coin,
                 GO.GameObjectType.HealUp,
                 GO.GameObjectType.SpecialAmmo1,
                GO.GameObjectType.SpecialAmmoFull,

            };

            GuiLayout layout = new GuiLayout("Spawn Item", menu);
            {
                for (VikingEngine.LootFest.GO.SuitType suit = GO.SuitType.Archer; suit < LootFest.GO.SuitType.NUM_NON; ++suit)
                {
                    new GuiTextButton("Suit " + suit.ToString(), null, new GuiAction1Arg<LootFest.GO.SuitType>(debugSpawnSuit, suit), false, layout);
                }

                foreach (var p in availablePickup)
                {
                    new GuiTextButton("Pickup: " + p.ToString(), null, new GuiAction1Arg<GO.GameObjectType>(debugSpawnPickup,p), false, layout);
                }

                for (VikingEngine.LootFest.GO.Gadgets.ItemType itype = 0; itype < VikingEngine.LootFest.GO.Gadgets.ItemType.NUM_NON; ++itype)
                {
                    new GuiTextButton("Item box: " + itype.ToString(), null, new GuiAction1Arg<VikingEngine.LootFest.GO.Gadgets.ItemType>(debugSpawnItemBox, itype), false, layout);
                }
            }
            layout.End();
        }

        void debugModelSpam()
        {
            new ModelSpammer(player.HeroPos);
            player.CloseMenu();
        }

        void debugSpawnSuit(LootFest.GO.SuitType type)
        {
            new GO.PickUp.SuitBox(new GoArgs( player.spawnItemOnHeroPos), type);
        }

        void debugSpawnPickup(GO.GameObjectType type)
        {
            switch (type)
            {
                case GO.GameObjectType.CardThrow:
                    new GO.PickUp.ItemBox(new GoArgs(player.spawnItemOnHeroPos),  GO.Gadgets.ItemType.Card);
                    break;
                case GO.GameObjectType.Coin:
                    new GO.PickUp.Coin(new GoArgs(player.spawnItemOnHeroPos));
                    break;
                case GO.GameObjectType.HealUp:
                    new GO.PickUp.HealUp(new GoArgs(player.spawnItemOnHeroPos));
                    break;
                case GO.GameObjectType.SpecialAmmo1:
                    new GO.PickUp.SpecialAmmoAdd1(new GoArgs(player.spawnItemOnHeroPos));
                    break;
                case GO.GameObjectType.SpecialAmmoFull:
                    new GO.PickUp.SpecialAmmoFullAdd(new GoArgs(player.spawnItemOnHeroPos));
                    break;
            }
        }
        void debugSpawnItemBox(VikingEngine.LootFest.GO.Gadgets.ItemType item)
        {
            new GO.PickUp.ItemBox(new GoArgs(player.spawnItemOnHeroPos), item);
        }

        void pageAppearance()
        {
            GuiLayout layout = new GuiLayout("Appearance", menu);
            {
                //if (player.hero is GO.PlayerCharacter.HorseRidingHero)
                //{
                //    new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.HorseMainColor),
                //        "Horse Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, horseColorCallback), true, layout);
                //    new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.HorseHairColor),
                //        "Horse hair Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, horseHairColorCallback), true, layout);
                //    new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.HorseNoseColor),
                //        "Horse nose Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, horseNoseColorCallback), true, layout);
                //    new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.HorseHoofColor),
                //        "Horse hoof Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, horseHoofColorCallback), true, layout);
                //}
                var appear = player.SuitAppearance;


                new GuiIconTextButton(SpriteName.IconApperanceHat, "Hat", null, selectHatMenu, true, layout);
                ////new GuiIconTextButton(Data.BlockTextures.MaterialTile(appear.HatMainColor),
                //    appearanceColorButton(appear.HatMainColor, "Hat Main Color", new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, hatMainColorCallback), layout);
                ////new GuiIconTextButton(Data.BlockTextures.MaterialTile(appear.HatDetailColor),
                //    appearanceColorButton(appear.HatDetailColor,"Hat Detail Color", new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, hatDetailColorCallback), layout);
                appearanceColorButton(appear.HatMainColor, "Hat Main Color", hatMainColorCallback, layout);
                appearanceColorButton(appear.HatDetailColor, "Hat Detail Color", hatDetailColorCallback, layout);
                
                new GuiIconTextButton(SpriteName.IconApperanceHair, "Hair", null, selectHairMenu, true, layout);
                //new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.hairColor),
                appearanceColorButton(player.Storage.hairColor, "Hair Color", hairColorCallback, layout);

                new GuiIconTextButton(SpriteName.IconApperanceMouth, "Mouth", null, selectMouthMenu, true, layout);
                new GuiIconTextButton(SpriteName.IconApperanceEyes, "Eyes", null, selectEyesMenu, true, layout);

              
                
                //if (player.Gear.suit.availableBeardTypes() != null)
                //{
                //    new GuiIconTextButton(SpriteName.IconApperanceBeard, "Beard", null, selectBeardMenu, true, layout);
                //    new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.BeardColor),
                //        "Beard Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, beardColorCallback), true, layout);
                //}
               

                //new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.SkinColor),
                //    "Skin Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, skinColorCallback), true, layout);
                  appearanceColorButton(player.Storage.SkinColor, "Skin Color", skinColorCallback, layout);

                //if (LfRef.progress.GotUnlock(player.Storage, UnlockType.Cape))
                //{
                //    new GuiCheckbox("Use cape", null, useCapeProperty, layout);
                //    new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.SkinColor),
                //        "Cape Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, capeColorCallback), true, layout);
                //}
                //new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.ClothColor),
                //    "Cloth Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, clothColorCallback), true, layout);
                  appearanceColorButton(player.Storage.ClothColor, "Cloth Color", clothColorCallback, layout);
                ////todo bälte
                //new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.BeltColor),
                //    "Belt Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, beltColorCallback), true, layout);
                //new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.BeltBuckleColor),
                //    "Belt Buckle Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, beltBuckleColorCallback), true, layout);
                  appearanceColorButton(player.Storage.BeltColor, "Belt Color", beltColorCallback, layout);
                  appearanceColorButton(player.Storage.BeltBuckleColor, "Belt Buckle Color", beltBuckleColorCallback, layout);
                ////legs
                //new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.PantsColor),
                //    "Pants Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, pantsColorCallback), true, layout);
                //new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.ShoeColor),
                //    "Shoe Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, shoeColorCallback), true, layout);
                  appearanceColorButton(player.Storage.PantsColor, "Pants Color", pantsColorCallback, layout);

                  appearanceColorButton(player.Storage.ShoeColor, "Shoe Color",  shoeColorCallback, layout);

                //if (player.Gear.suit.shield != null)
                //{
                //    new GuiIconTextButton(SpriteName.LFHudShield, "Shield", null, selectShieldMenu, true, layout);
                //    new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.ShieldMainColor),
                //        "Shield Main Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, shieldMainColorCallback), true, layout);
                //    new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.ShieldDetailColor),
                //        "Shield Detail Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, shieldDetailColorCallback), true, layout);
                //    new GuiIconTextButton(Data.BlockTextures.MaterialTile(player.Storage.ShieldEdgeColor),
                //        "Shield Edge Color", null, new GuiAction1Arg<Action<Data.MaterialType>>(listMaterials, shieldEdgeColorCallback), true, layout);
                //}

            }
            layout.End();
        }

        void appearanceColorButton(ushort color, string text, Action<BlockHD> link, GuiLayout layout)
        {
            var button = new GuiIconTextButton(SpriteName.WhiteArea,
                    text, null, new GuiAction1Arg<Action<BlockHD>>(listMaterials, link), true, layout);
            button.icon.Color = VikingEngine.LootFest.Map.HDvoxel.BlockHD.ToColor(color);
        }

        bool useCapeProperty(bool set, bool value)
        {
            if (set)
            {
                player.Storage.UseCape = value;
                player.hero.UpdateAppearance(false);
                player.appearanceChanged = true;
            }
            return player.Storage.UseCape;
        }

        void selectShieldMenu()
        {
            GuiLayout layout = new GuiLayout("Shield", menu);
            {
                List<ShieldType> options = new List<ShieldType>
                {
                    ShieldType.Round1, 
                    ShieldType.Round2, 
                    ShieldType.Round3, 
                    ShieldType.Round4,
                    ShieldType.Spartan1, 
                    ShieldType.Spartan2, 
                    ShieldType.Spartan3, 
                    ShieldType.Keit1,
                };

                foreach (var opt in options)
                {
                    new GuiTextButton(opt.ToString(), null, new HUD.GuiAction1Arg<ShieldType>(selectShield, opt), false, layout);
                }
            }
            layout.End();
        }

        void selectShield(ShieldType type)
        {
            player.Storage.shieldType = type;
            refreshShield();
        }

        void selectHatMenu()
        {
            GuiLayout layout = new GuiLayout("Hat", menu);
            {
                var classHatArray = player.Gear.suit.availableHatTypes();
                
                List<HatType> otherOptions = new List<HatType>
                {
                    HatType.Cap,
                    HatType.Witch,
                    HatType.Football,
                    HatType.Pirate1,
                    HatType.Pirate2,
                    HatType.Pirate3,
                    HatType.Santa1,
                    HatType.Santa2,
                    HatType.Santa3,
                    HatType.Baby1,
                    HatType.Baby2,

                    HatType.Arrow, 
                    HatType.Bucket, 
                    HatType.Coif1, 
                    HatType.Coif2, 
                    HatType.High1, 
                    HatType.High2,
                    HatType.Hunter1, 
                    HatType.Hunter2, 
                    HatType.Hunter3,
                    HatType.Low1, 
                    HatType.Low2, 
                    HatType.Mini1, 
                    HatType.Mini2, 
                    HatType.Mini3,
                    HatType.Turban1, 
                    HatType.Turban2,
                    HatType.Headband1, 
                    HatType.Headband2, 
                    HatType.Headband3,
                    HatType.crown1, HatType.crown2, HatType.crown3, 
                    HatType.princess1, HatType.princess2,
                    HatType.MaskTurtle1, 
                    HatType.MaskZorro1, 
                    HatType.MaskZorro2, 
                    HatType.Zelda,
                };

                if (classHatArray != null)
                {
                    otherOptions.InsertRange(0, classHatArray);
                }
                
                otherOptions.Insert(0, HatType.None);

                List<HatType> unlockedHats = new List<HatType>(otherOptions.Count);
                player.Storage.lootBoxes.unlockedItemsToList(Data.LootBoxType.Hat, otherOptions, unlockedHats);

                foreach (var opt in unlockedHats)
                {
                    new GuiTextButton(opt.ToString(), null, new HUD.GuiAction1Arg<HatType>(selectHat, opt), false, layout);
                }
            }
            layout.End();
        }

        
        void selectHat(HatType type)
        {
            var appear = player.SuitAppearance;
            appear.hat = type;
            player.SuitAppearance = appear;
            player.hero.UpdateAppearance(false);
            player.appearanceChanged = true;
        }

        void selectHairMenu()
        {
            GuiLayout layout = new GuiLayout("Hair", menu);
            {
                List<HairType> options = new List<HairType>
                {
                    HairType.NoHair,
                    HairType.Normal,
                    HairType.Spiky1,
                    HairType.Spiky2,
                    HairType.Rag1,
                    HairType.Rag2,
                    HairType.Bald1,
                    HairType.Bald2,
                    HairType.GirlyShort1,
                    HairType.GirlyShort2,
                    HairType.GirlyLong1,
                    HairType.GirlyLong2,
                };

                if (player.Gear.suit.Type == SuitType.Emo)
                {
                    options.Add(HairType.Emo1);
                    options.Add(HairType.Emo2);
                    options.Add(HairType.Emo3);
                }

                foreach (var opt in options)
                {
                    new GuiTextButton(opt.ToString(), null, new HUD.GuiAction1Arg<HairType>(selectHair, opt), false, layout);
                }
            }
            layout.End();
        }

        void selectHair(HairType type)
        {
            var appear = player.SuitAppearance;
            appear.hair = type;
            player.SuitAppearance = appear;
            //player.Storage.hairType = type;
            player.hero.UpdateAppearance(false);
            player.appearanceChanged = true;
        }

        void selectMouthMenu()
        {
            GuiLayout layout = new GuiLayout("Mouth", menu);
            {
                //mFile = new File();
                var options = new MouthType[]
                {
                    MouthType.NoMouth,
                    MouthType.Smile,
                    MouthType.BigSmile,
                    MouthType.SideSmile1,
                    MouthType.SideSmile2,
                    MouthType.WideSmile,
                    MouthType.open_smile,
                    MouthType.Loony,
                    MouthType.Straight,
                    MouthType.teeth1,
                    MouthType.teeth2,
                    MouthType.teeth3,
                    MouthType.OMG,
                    MouthType.Hmm,
                    MouthType.Sour,
                    MouthType.souropen,
                    MouthType.Orc,
                    MouthType.vampire,
                    MouthType.Gasp,
                    MouthType.Laugh,
                    MouthType.Girly1,
                    MouthType.Girly2,
                    MouthType.Pirate,
                    MouthType.Baby1,
                    MouthType.Baby2,
                };

                foreach (var opt in options)
                {
                    new GuiTextButton(opt.ToString(), null, new HUD.GuiAction1Arg<MouthType>(selectMouth, opt), false, layout);
                }
            }
            layout.End();
            //OpenMenuFile();
        }
        void selectMouth(MouthType type)
        {
            //var appear = player.AbsHero.SuitAppearance;
            player.Storage.mouth = type;
            //player.AbsHero.SuitAppearance = appear;
            player.hero.UpdateAppearance(false);
            player.appearanceChanged = true;
            //pageAppearance();
        }


        void selectEyesMenu()
        {
            GuiLayout layout = new GuiLayout("Eyes", menu);
            {
                //mFile = new File();
                var options = new EyeType[]
                {
                    EyeType.NoEyes,
                    EyeType.Normal,
                    EyeType.Sunshine,
                    EyeType.Wink,
                    EyeType.Loony,
                    EyeType.Slim,
                    EyeType.Frown,
                    EyeType.HardShut,
                    EyeType.Evil,
                    EyeType.Red,
                    EyeType.Sleepy,
                    EyeType.SleepyCross,
                    EyeType.Cross,
                    EyeType.Crossed1, EyeType.Crossed2, EyeType.Crossed3, EyeType.Crossed4, EyeType.Crossed5,
                    EyeType.Sad1, EyeType.Sad2, EyeType.Sad3, EyeType.Sad4, EyeType.Sad5, 
                    EyeType.Cyclops,
                    EyeType.Vertical,
                    EyeType.Girly1, EyeType.Girly2, EyeType.Girly3, 
                    EyeType.Pirate,
                    EyeType.SunGlasses1,
                    EyeType.SunGlasses2,
                    EyeType.Emo1,
                    EyeType.Emo2,
                };

                foreach (var opt in options)
                {
                    new GuiTextButton(opt.ToString(), null, new HUD.GuiAction1Arg<EyeType>(selectEyes, opt), false, layout);
                }
            }
            layout.End();
            //OpenMenuFile();
        }
        void selectEyes(EyeType type)
        {
            //var appear = player.AbsHero.SuitAppearance;
            player.Storage.eyes = type;
           // player.AbsHero.SuitAppearance = appear;
            player.hero.UpdateAppearance(false);
            player.appearanceChanged = true;
            //pageAppearance();
        }


        void selectBeardMenu()
        {
            GuiLayout layout = new GuiLayout("Hat", menu);
            {
                // mFile = new File();
                var options = player.Gear.suit.availableBeardTypes();

                foreach (var opt in options)
                {
                    new GuiTextButton(opt.ToString(), null, new HUD.GuiAction1Arg<BeardType>(selectBeard, opt), false, layout);
                }
            }
            layout.End();
            //OpenMenuFile();
        }

        void selectBeard(BeardType type)
        {
            var appear = player.SuitAppearance;
            appear.beard = type;
            player.SuitAppearance = appear;
            player.hero.UpdateAppearance(false);
            player.appearanceChanged = true;
        }

        public void DeleteMe()
        {
            menu.DeleteMe();
            Input.Mouse.Visible = false;

            //inputOverview.DeleteMe();
        }

        void listMaterials(Action<BlockHD> link)
        {
            Editor.VoxelDesigner.listMaterials(menu, link, false);//mFile, (int)d, false, Storage, (int)Link.ShowHideMaterialNames, 0);
        }

        void refreshShield()
        {
            if (player.Gear.suit.shield != null)
            {
                player.Gear.suit.shield.refreshModel(player.Storage);
            }
        }

        //void shieldMainColorCallback(Color material)
        //{
        //    player.Storage.ShieldMainColor = BlockHD.ToBlockValue(material, BlockHD.UnknownMaterial);
        //    refreshShield();
        //}
        //void shieldDetailColorCallback(Color material)
        //{
        //    player.Storage.ShieldDetailColor = BlockHD.ToBlockValue(material, BlockHD.UnknownMaterial);
        //    refreshShield();
        //}
        //void shieldEdgeColorCallback(Color material)
        //{
        //    player.Storage.ShieldEdgeColor = BlockHD.ToBlockValue(material, BlockHD.UnknownMaterial);
        //    refreshShield();
        //}


        //void horseColorCallback(Color material)
        //{
        //    player.Storage.HorseMainColor = BlockHD.ToBlockValue(material, BlockHD.UnknownMaterial);
        //    onAppearanceColorChange();
        //}
        //void horseHairColorCallback(Color material)
        //{
        //    player.Storage.HorseHairColor = BlockHD.ToBlockValue(material, BlockHD.UnknownMaterial);
        //    onAppearanceColorChange();
        //}
        //void horseNoseColorCallback(Color material)
        //{
        //    player.Storage.HorseNoseColor = BlockHD.ToBlockValue(material, BlockHD.UnknownMaterial);
        //    onAppearanceColorChange();
        //}
        //void horseHoofColorCallback(Color material)
        //{
        //    player.Storage.HorseHoofColor = BlockHD.ToBlockValue(material, BlockHD.UnknownMaterial);
        //    onAppearanceColorChange();
        //}

        void hairColorCallback(BlockHD material)
        {
            player.Storage.hairColor = material.BlockValue;
            onAppearanceColorChange();
        }
        void skinColorCallback(BlockHD material)
        {
            player.Storage.SkinColor = material.BlockValue;
            onAppearanceColorChange();
        }
        void beardColorCallback(BlockHD material)
        {
            player.Storage.BeardColor = material.BlockValue;
            onAppearanceColorChange();
        }
        //void capeColorCallback(BlockHD material)
        //{
        //    player.Storage.CapeColor = material.BlockValue;
        //    onAppearanceColorChange();
        //}
        void clothColorCallback(BlockHD material)
        {
            player.Storage.ClothColor = material.BlockValue;
            onAppearanceColorChange();
        }
        void pantsColorCallback(BlockHD material)
        {
            player.Storage.PantsColor = material.BlockValue;
            onAppearanceColorChange();
        }
        void shoeColorCallback(BlockHD material)
        {
            player.Storage.ShoeColor = material.BlockValue;
            onAppearanceColorChange();
        }
        void hatMainColorCallback(BlockHD material)
        {
            var appear = player.SuitAppearance;
            appear.HatMainColor = material.BlockValue;
            player.SuitAppearance = appear;
            onAppearanceColorChange();
        }
        void hatDetailColorCallback(BlockHD material)
        {
            var appear = player.SuitAppearance;
            appear.HatDetailColor = material.BlockValue;
            player.SuitAppearance = appear;
            onAppearanceColorChange();
        }

        void beltColorCallback(BlockHD material)
        {
            player.Storage.BeltColor = material.BlockValue;
            onAppearanceColorChange();
        }
        void beltBuckleColorCallback(BlockHD material)
        {
            player.Storage.BeltBuckleColor = material.BlockValue;
            onAppearanceColorChange();
        }

        void onAppearanceColorChange()
        {
            player.hero.UpdateAppearance(false);
            player.appearanceChanged = true;
        }


        void debugModelsAutoLoader()
        {
            LfRef.modelLoad.debugMenu(menu);
        }
    }

}
