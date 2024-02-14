using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using Microsoft.Xna.Framework;
using VikingEngine.Engine;

namespace VikingEngine.LF2
{
    class MenuSystem
    {
        bool questLogTutorial = false;

        public Gui menu = null;
        Players.Player player;

        public MenuSystem(Players.Player player)
        {
            this.player = player;
        }

        public static GuiStyle GuiStyle()
        {
            return new GuiStyle(
                (Screen.PortraitOrientation ? Screen.Width : Screen.Height) * 0.61f, 5, SpriteName.LFMenuRectangleSelection);
        }

        public void open()
        {
            if (menu == null)
            {
                menu = new Gui(GuiStyle(), player.SafeScreenArea, 0f, LootFest.LfLib.Layer_GuiMenu, player.inputMap.inputSource);
                Input.Mouse.Visible = true;
            }
        }

        public void close()
        {
            if (menu != null)
            {
                menu.DeleteMe();
                menu = null;
                Input.Mouse.Visible = false;
            }
        }

        void mainMenu()
        {
            GuiLayout layout = new GuiLayout("Main Menu", menu, GuiLayoutMode.MultipleColumns, null);
            {
                new GuiTextButton(LanguageManager.Wrapper.Resume(), null, close, false, layout);
                new GuiIconTextButton(SpriteName.IconBackpack, "Backpack", null, new GuiAction(pageBackPack), true, layout);

                if (LfRef.gamestate.Progress.GeneralProgress >= Data.GeneralProgress.AskAround)
                {
                    const string QuestLog = "Quest log";
                    if (questLogTutorial)
                    {
                        questLogTutorial = false;
                        new GuiLargeIconTextButton(SpriteName.IconQuestLog, QuestLog, null,
                            new GuiAction1Arg<Gui>(LfRef.gamestate.Progress.QuestLog, menu), true, layout);
                    }
                    else
                        new GuiIconTextButton(SpriteName.IconQuestLog, QuestLog, null,
                            new GuiAction1Arg<Gui>(LfRef.gamestate.Progress.QuestLog, menu), true, layout);
                    //file.AddIconTextLink(SpriteName.IconQuestLog, QuestLog, (int)Link.QuestLog);
                }

                new GuiIconTextButton(SpriteName.LFIconMap, LanguageManager.Wrapper.GameMenuOpenMap(), 
                    null, openMap, false, layout);

                if (arraylib.HasMembers(player.messages))
                {
                    new GuiIconTextButton(SpriteName.LFIconLetter, "Messages(" + player.messages.Count.ToString() + ")", 
                        null, pageMessages, true, layout);
                }

                new GuiIconTextButton(SpriteName.LFIconMirror, LanguageManager.Wrapper.GameMenuAppearance(), 
                    null, pageAppearance, true, null);

                new GuiIconTextButton(SpriteName.IconMenuSettings, LanguageManager.Wrapper.GameMenuSettings(),
                    null, pageSettings, true, layout);

                new GuiIconTextButton(SpriteName.BoardIconRules, "Game manual", null, 
                    pageManual, true, layout);
            }
            layout.End();

            File file = new File();

            //if (Engine.XGuide.IsTrial)
            //{
            //    Engine.XGuide.UpdateTrialCheck();
            //    file.AddDescription("TRIAL MODE");
            //}
            //file.AddIconTextLink(SpriteName.LFIconGoBack, LanguageManager.Wrapper.Resume(), (int)Link.CloseMenu);

            //if (PlatformSettings.ViewUnderConstructionStuff)
            //{
            //    if (LfRef.gamestate.LocalHostingPlayer.ClientPermissions != ClientPermissions.Spectator &&
            //        lastInteraction != null && lastInteraction.InteractType == GameObjects.InteractType.Door)
            //    {
            //        file.AddTextLink("Remove door", (int)Link.RemoveDoor);
            //    }
            //}
            //file.AddIconTextLink(SpriteName.IconBackpack, "Backpack", new HUD.Link(LinkType.OpenPage, (int)MenuPageName.Backpack));
            //if (LfRef.gamestate.Progress.GeneralProgress >= Data.GeneralProgress.AskAround)
            //{
            //    const string QuestLog = "Quest log";
            //    if (questLogTutorial)
            //    {
            //        questLogTutorial = false;
            //        file.Add(new HUD.LiveTextLinkData(new List<string> { "**NEW**", QuestLog },
            //            null, new HUD.Link((int)Link.QuestLog), 900, 100));
            //    }
            //    else
            //        file.AddIconTextLink(SpriteName.IconQuestLog, QuestLog, (int)Link.QuestLog);
            //}
            //file.AddIconTextLink(SpriteName.LFIconMap, LanguageManager.Wrapper.GameMenuOpenMap(), (int)Link.OpenMap);

            //file.AddIconTextLink(SpriteName.BoardIconChat, "Chat history", new HUD.ActionLink(messageHistory));
            

           // if (!Engine.XGuide.IsTrial)
            //{
               
            //    file.AddIconTextLink(SpriteName.IconMenuMultiplayer, NetworkSettingsTitle, "Multiplayer and network settings", (int)Link.NetworkSettings);
            //}
            //file.AddIconTextLink(SpriteName.IconMenuSettings, LanguageManager.Wrapper.GameMenuSettings(), (int)Link.GameSettings);

            //file.AddIconTextLink(SpriteName.TrophyUnlocked, "Trophies", new HUD.ActionLink(pageThophies));
            //file.AddIconTextLink(SpriteName.BoardIconRules, "Game manual", new HUD.Link(LinkType.OpenPage, (int)MenuPageName.Manual));

            if (LfRef.gamestate.LocalHostingPlayer == this)
            {
                file.AddIconTextLink(SpriteName.IconMenuCloseGame, "Exit world", (int)Link.QuitGameQuest);
            }
            else
            {
                file.AddTextLink("Drop out", (int)Link.QuitGameQuest);
            }
            if (PlatformSettings.Debug <= BuildDebugLevel.TesterDebug_2 || Settings.DebugUnlocked)
            {

                file.AddTextLink("**Debug**", (int)Link.DEBUG);


            }
            return file;
        }

        void openMap()
        {
            close();
            player.openMiniMap(true);
        }

        void pageBackPack()
        {
            //mFile.Properties.ParentPage = (int)MenuPageName.MainMenu;
            progress.BackpackPage(ref mFile, this);
        }

        void pageTalkingToNPC()
        {
            if (hero.InteractingWith != null && hero.InteractingWith.InteractType == GameObjects.InteractType.SpeakDialogue)
            {
                File talkMenu = hero.InteractingWith.Interact_TalkMenu(hero);//hero.InteractingWith.Interact_TalkMenu(hero);
                if (talkMenu != null)
                {
                    mFile = talkMenu;
                }
                else
                {
                    CloseMenu();
                    return;
                }
            }
            else
            {
                CloseMenu();
                return;
            }

        }

        void pageManual()
        {
            mFile.Properties.ParentPage = (int)MenuPageName.MainMenu;
            mFile.AddTitle("Manual");
            mFile.AddDescription("DON'T PANIC");
            mFile.AddTextLink("Controls", (int)Link.ManualControls);
            mFile.AddTextLink("Quests", (int)Link.ManualQuests);
            mFile.AddTextLink("Traveling", (int)Link.ManualTraveling);
            mFile.AddTextLink("Crafting", (int)Link.ManualCrafting);
            mFile.AddTextLink("Equip items", (int)Link.ManualEquipItems);
            mFile.AddTextLink("Alternative equip", new HUD.Link(ManualAlternativeEquiping));
            mFile.AddTextLink("Multiplayer", (int)Link.ManualMultiplayer);
        }

        void pageSettings()
        {
            mFile.Properties.ParentPage = (int)MenuPageName.MainMenu;



            mFile.Checkbox2("View tutorial", "Will promt which buttons you should use", (int)ValueLink.bTutorial);
            mFile.Checkbox2("Auto Equip", "Automatically fill empty equip slots with items you pick up", (int)ValueLink.bAutoEquip);


            if (Ref.netSession.IsHostOrOffline)
            {

                mFile.Checkbox2("Auto save", "Will save your progress every minute", (int)ValueLink.bAutoSave);//(int)Link.CB_AutoSave, settings.AutoSave);
                                                                                                               //mFile.AddTextLink("Save world", (int)Link.SaveWorldNow);
            }
            //file.AddCheckbox("Help text", "The button explanation in the bottom of the screen", (int)Link.CB_ViewButtonHelp, settings.ViewHelpButtons);
            if (Ref.netSession.InMultiplayer)
                mFile.Checkbox2("View gamer names", TextLib.EmptyString, (int)ValueLink.bViewGamerTags);//(int)Link.CB_ViewPlayerNames, settings.ViewPlayerNames);
            mFile.Checkbox2("Use quick typing", "Use the custom made text input typing, instead of the XBOX Guide standard one", (int)ValueLink.bQuickInput);
            Music.MusicLib.ToMenu(mFile);
            mFile.AddTextLink("Next song", new HUD.ActionLink(LfRef.gamestate.MusicDirector.NextSong));
            if (LfRef.LocalHeroes.Count > 1)
            {
                mFile.Checkbox2("Horizontal split", "Prefered split screen setup", (int)ValueLink.bHorizontalSplit);//(int)Link.CB_HorizontalSplit, Engine.Draw.horizontalSplit);
            }
            if (localPData.view.Camera.CamType == Graphics.CameraType.FirstPerson)
            {
                mFile.AddDescription("FP camera");
                mFile.Checkbox2("Inverted look", "Invert the up/down on FP-view", (int)ValueLink.bInvertLook);
                IntervalF camSpeedRange = new IntervalF(1, 10);
                mFile.AddValueOptionList("Horizontal speed", (int)Link.FPviewHoriSpeed, Settings.CameraSettings.SpeedX, camSpeedRange, 1);
                mFile.AddValueOptionList("Vertical speed", (int)Link.FPviewVertiSpeed, Settings.CameraSettings.SpeedY, camSpeedRange, 1);
            }
        }

        void pageNetworkSettings()
        {
            mFile.Properties.ParentPage = (int)MenuPageName.MainMenu;

            mFile.AddTextLink("Send message", (int)Link.NetSendMessage);

            mFile.AddIcon(SpriteName.IconHand, Color.White, new HUD.Link(LinkType.ActionAndExit, (int)Link.Express,
                (int)VoxelModelName.express_hi), "Express hi!");
            mFile.AddIcon(SpriteName.IconLaugh, Color.White, new HUD.Link(LinkType.ActionAndExit, (int)Link.Express,
                (int)VoxelModelName.express_laugh), "Express laugh");
            mFile.AddIcon(SpriteName.IconThumbUp, Color.White, new HUD.Link(LinkType.ActionAndExit, (int)Link.Express,
                (int)VoxelModelName.express_thumbup), "Express thumbs up");
            mFile.AddIcon(SpriteName.IconSour, Color.White, new HUD.Link(LinkType.ActionAndExit, (int)Link.Express,
                (int)VoxelModelName.express_anger), "Express anger");
            mFile.AddIcon(SpriteName.IconTeasing, Color.White, new HUD.Link(LinkType.ActionAndExit, (int)Link.Express,
                (int)VoxelModelName.express_teasing), "Express teasing");

            const string Say = "Say: ";
            for (int i = 0; i < NetworkLib.QuickMessages.Length; ++i)
            {
                mFile.AddIcon(NetworkLib.QuickMessageIcons[i], Color.White,
                    new HUD.ActionIndexLink(quickMessage, i),
                    Say + NetworkLib.QuickMessages[i]);
            }

            if (localPData.OnlineSessionsPrivilege)
            {

                if (Ref.netSession.IsHostOrOffline)
                {
                    mFile.AddDescription("Allow gamers to join");
                    mFile.AddTextOptionList("Network type", (int)Link.NetworkSessionType, (int)Ref.netSession.SessionOpenType,
                        Network.NetLib.ListNetworkCanJoinTypes(), Network.NetLib.ListNetworkCanJoinTypesDescriptions());
                    mFile.AddTextLink("Invite", "Ask friends to join your world, if they don't own the game they will get a buy option", (int)Link.Invite);
                }

                const string NetStatus = "Status: ";
                if (Network.Session.Connected)
                {
                    mFile.AddDescription(NetStatus + "Online " + (Map.World.RunningAsHost ? "host" : "client"));
                }
                else
                {
                    mFile.AddDescription(NetStatus + "Offline");
                }
            }
            else
            {
                mFile.AddDescription("Your profile does not allow online multiplayer");
            }
            LfRef.gamestate.ListLocalGamers(mFile, (int)Link.SelectLocalGamer_dialogue, this);
            Ref.netSession.ListRemoteGamers(mFile);
            //DEBUG 
            if (PlatformSettings.Debug <= BuildDebugLevel.TesterDebug_2)
            {
                mFile.TextBoxTitle("Debug info:");
                if (Ref.netSession.networkSession == null)
                {
                    mFile.TextBoxBread("No session");
                }
                else
                {
                    mFile.TextBoxBread("Session type:" + Ref.netSession.networkSession.SessionType.ToString());

                    int?[] properties = Ref.netSession.networkSession.SessionProperties.ToArray() as int?[];
                    for (int i = 0; i < properties.Length; ++i)
                    {
                        mFile.TextBoxBread("Property" + i.ToString() + ": " + properties[i].ToString());
                    }
                }
            }
        }

        void pageGameover()
        {
            mFile.AddTitle(LanguageManager.Wrapper.DeathTitle());
            mFile.AddDescription("You lost " + LootfestLib.DeathCost.ToString() + LootfestLib.MoneyText);
            mFile.AddTextLink("Ok", (int)Link.YouDiedOk);
            if (Settings.UnlockedPrivateHome)
            {
                mFile.Checkbox2("To private home", "Relocate at your private home", (int)ValueLink.bSpawnAtPricateHome);
            }
        }

        void pageMessages()
        {
            mFile.Properties.ParentPage = (int)MenuPageName.MainMenu;
            mFile.AddTitle("Messages");
            for (int i = 0; i < messages.Count; i++)
            {
                mFile.AddDescription(messages[i].Sender + ":");
                mFile.AddTextLink(messages[i].Subject, new HUD.Link((int)Players.Link.OpenMessage_dialogue, i));//i, (int)Players.Link.OpenMessage_dialogue);
            }
        }

        void pageTravel()
        {
            mFile.Properties.ParentPage = (int)MenuPageName.MainMenu;
            mFile.AddTitle("Travel");
            mFile.AddTextLink("To Spawn point", (int)Link.WarpToSpawn);
            //mFile.AddTextLink("Set Spawn point", "Will set the position you are standing at as your new spawn point", (int)Link.WarpToSetHere, -1);
            setSpawnPointMember(mFile);
            if (Network.Session.Connected || LfRef.LocalHeroes.Count > 1)
                mFile.AddTextLink("To player", (int)Link.NetworkSettings);
            mFile.AddTextLink("To World Center", "The center is the position new players will enter at", (int)Link.WarpToCenter);
            //if (Data.WorldsSummaryColl.CurrentWorld.NamedAreas.Count > 0)
            //{
            //    mFile.AddDescription("Named locations");
            //    Link d = Map.World.RunningAsHost ? Link.NamedLocation_dialogue : Link.TravelToNamedLocation_dialogue;
            //    for (int i = 0; i < Data.WorldsSummaryColl.CurrentWorld.NamedAreas.Count; i++)
            //    {
            //        mFile.AddTextLink("To " + Data.WorldsSummaryColl.CurrentWorld.NamedAreas[i].Name, new HUD.Link((int)d, i));// i, (int)d);
            //    }
            //}
            if (Map.World.RunningAsHost)
            {
                mFile.AddTextLink("Add location", "Add a name to this position so you can easily find it again", (int)Link.NameThisLocation);
            }
        }

        

        void pageThophies()
        {
            mFile = new File();
            mFile.Properties.ParentPage = (int)MenuPageName.MainMenu;
            int numUnlocked = 0;
            mFile.AddTitle("Challenge trophies");

            const byte Dark = 50;
            Color veryDarkGray = new Color(Dark, Dark, Dark);

            for (Trophies t = (Trophies)0; t < Trophies.NUM; t++)
            {
                bool isUnlocked = Settings.UnlockedThrophy[(int)t];

                mFile.Add(new HUD.IconData(LootfestLib.TrophyIcon(t),
                    isUnlocked ? SpriteName.TrophyUnlocked : SpriteName.TrophyLocked, isUnlocked ? Color.White : veryDarkGray, null, LootfestLib.TrophyDescription(t)));

                if (isUnlocked)
                {
                    numUnlocked++;
                }
            }
            mFile.AddDescription("Unlocked " + numUnlocked.ToString() + "/" + ((int)Trophies.NUM).ToString());
            if (LfRef.gamestate.Progress.GeneralProgress < Data.GeneralProgress.GameComplete)
            {
                mFile.AddDescription("Player knock outs in this world: " + LfRef.gamestate.Progress.NumHeroDeaths.ToString());
            }
            mFile.AddTitle("Monsters killed");
            for (int i = 0; i < (int)GameObjects.Characters.Monster2Type.NUM; i++)
            {
                mFile.AddDescription(((GameObjects.Characters.Monster2Type)i).ToString() + ": " + Settings.KilledMonsterTypes[i].ToString());
            }
            OpenMenuFile();
        }
        void pageAppearance()
        {
            mFile = new File();
            //if (rcToy != null)
            //{
            //    mFile.Properties.ParentPage = (int)MenuPageName.RCPauseMenu;
            //    List<string> colors = RCcolorScheme.ColorNames(rcToy.RcCategory);
            //    for (int i = 0; i < colors.Count; i++)
            //    {
            //        mFile.AddTextLink(colors[i], new HUD.Link((int)Link.ListRCcolors_dialogue, i));//i, (int)Link.ListRCcolors_dialogue);
            //    }
            //}
            //else
            //{
            mFile.Properties.ParentPage = (int)MenuPageName.MainMenu;
            //head


            //check unlocked pet
            if (Settings.UnlockedAllTrophies)
            {
                mFile.AddIconTextLink(SpriteName.TrophyUnlocked, "Pet Type", new HUD.Link(appearanceSelectPetTypeMenu));

            }
            if (hero.HasFlyingPet)
                mFile.AddIconTextLink(SpriteName.BoardIconDelete, "Remove Pet", new HUD.Link(removePet));

            mFile.AddIconTextLink(SpriteName.IconApperanceHat, LanguageManager.Wrapper.AppearanceHatTitle(), (int)Link.ChangeHat);
            mFile.AddIconTextLink(Data.MaterialBuilder.MaterialTile(Settings.HatMainColor),
                "Hat Main Color", (int)Link.HatMainColor);
            mFile.AddIconTextLink(Data.MaterialBuilder.MaterialTile(Settings.HatDetailColor),
                "Hat Detail Color", (int)Link.HatDetailColor);
            mFile.AddIconTextLink(Data.MaterialBuilder.MaterialTile(Settings.hairColor),
               "Hair Color", (int)Link.HairColor);
            mFile.AddIconTextLink(Data.MaterialBuilder.MaterialTile(Settings.SkinColor),
                "Skin Color", (int)Link.SkinnColor);
            mFile.AddIconTextLink(SpriteName.IconApperanceEyes, "Eyes", (int)Link.ChangeEyes);
            mFile.AddIconTextLink(SpriteName.IconApperanceMouth, "Mouth", (int)Link.ChangeMouth);

            const string Beard = "Facial Hair";
            if (Settings.BeardType == BeardType.Shaved)
            {
                mFile.AddIconTextLink(SpriteName.IconApperanceBeard, Beard, (int)Link.Beard);
            }
            else
            {
                mFile.AddIconTextLink(Data.MaterialBuilder.MaterialTile(Settings.BeardColor), Beard, (int)Link.Beard);
            }
            //body
            mFile.AddIconTextLink(Data.MaterialBuilder.MaterialTile(Settings.ClothColor),
                "Cloth Color", (int)Link.ClothColor);
            mFile.AddIconTextLink(SpriteName.IconApperanceBelt, "Belt", (int)Link.BeltType);
            mFile.AddIconTextLink(Data.MaterialBuilder.MaterialTile(Settings.BeltColor),
                "Belt Color", (int)Link.BeltColor);
            mFile.AddIconTextLink(Data.MaterialBuilder.MaterialTile(Settings.BeltBuckleColor),
                "Belt Buckle Color", (int)Link.BeltBuckleColor);

            //legs
            mFile.AddIconTextLink(Data.MaterialBuilder.MaterialTile(Settings.PantsColor),
                "Pants Color", (int)Link.PantsColor);
            mFile.AddIconTextLink(Data.MaterialBuilder.MaterialTile(Settings.ShoeColor),
                "Shoe Color", (int)Link.ShoeColor);

            mFile.Checkbox2("Cape", TextLib.EmptyString, (int)ValueLink.bApperanceCape);
            mFile.AddIconTextLink(Data.MaterialBuilder.MaterialTile(Settings.CapeColor), "Cape Color", (int)Link.CapeColor);



            //}
            menu.File = mFile;
        }

        void pageDebug()
        {
            //mFile = new File();
            //mFile.Properties.ParentPage = (int)MenuPageName.MainMenu;
            //HUD.File debugMenu = new HUD.File();

            ////string chunkText = 


            //mFile.TextBoxDebug(debugChunkText());

            //mFile.AddTextLink("Quick startup", "get basic weapons and equipment, unlock all locations and skip initial tutorial", new HUD.ActionLink(debugQuickStartUp));
            //mFile.AddTextLink("Unlock all locations", "the map won't have any undiscovered black areas", (int)Link.DebugUnlockLocations);
            //mFile.AddTextLink("List hosted objects(" + LfRef.gamestate.GameObjCollection.LocalMembers.Count + ")", "all things that move around in the game", (int)Link.DebugListHostedObj);
            //mFile.AddTextLink("List client objects(" + LfRef.gamestate.GameObjCollection.ClientMembers.Count + ")", "all things that a network visitor spawn", (int)Link.DebugListClientObj);
            //mFile.AddTextLink("Used IDs", "Deleted gameobjects must return their ID or the game will crash", new HUD.ActionLink(debugUsedGameObjectIDs));
            //mFile.AddTextLink("Light and shadows", "list all objects that has a light or shadow connected to it", new HUD.ActionLink(debugLightAndShadows));
            //mFile.AddTextLink("Add 100gold", (int)Link.Get100g);
            //mFile.AddTextLink("Run debug mode", "move fast, and be invincible", (int)Link.StartDebugMode);
            ////mFile.AddTextLink("Spawn enemies", new HUD.ActionLink(debugSpawnEnemies));

            //mFile.AddTextLink("Spawn...", "Spawn a game object", new HUD.ActionLink(debugSpawn));

            //mFile.AddTextLink("Get all weapons", new HUD.ActionLink(debugGetAllWeapons));
            //mFile.AddTextLink("Get all magic rings", (int)Link.DebugGetAllMagicRings);
            //mFile.AddTextLink("Pick Item", new HUD.ActionLink(debugPickItem));//(int)Link.DebugGetAllGoods);
            //mFile.AddTextLink("Unlock all bosses", "get all info plus keys to bosses", new HUD.Link(LfRef.gamestate.Progress.DebugUnlockAllBosses, CloseMenu));
            //DataStream.DataStreamLib.BrowseButton(mFile);
            //mFile.AddTextLink("Crash game", "test the blue screen", (int)Link.DebugCrash);



            //mFile.AddDescription("Music vol:" + Engine.Sound.MusicVolume);
            //mFile.AddTextLink("Test song", new HUD.ActionLink(debugMusic));
            //mFile.AddTextLink("Chunk status", "orient the screen so -X is left", (int)Link.DebugChunkStatus);
            //mFile.AddTextLink("Creation mode", new HUD.ActionLink(BeginCreationMode));
            //mFile.AddTextLink("Change chunk", "add blocks to force the chunk to update and be stored", new HUD.ActionLink(debugChangeChunk));
            //mFile.AddTextLink("List changed chunks", "those have been changed and stored in mem", (int)Link.DeugListChangedChunks);
            //mFile.AddTextLink("Performance: Vox models", "all generated 3d models in render loop", new HUD.ActionLink(debugPerformanceVoxModels));
            //mFile.AddTextLink("Performance: Meshes", "all 3d models in render loop", (int)Link.DebugListMeshes);
            //mFile.AddTextLink("Performance: Update", "all objects that is in update list", (int)Link.DebugListUpdate);
            //mFile.AddTextLink("Jump", "move away quickly", (int)Link.DebugJump);
            //mFile.AddTextLink("Hide terrain mesh", new HUD.ActionLink(debugHideTerrain));

            //mFile.AddTextLink("Set mission", "jump to any position in the main story", (int)Link.DebugSetMission);
            //mFile.AddTextLink("Clear inventory", "all your items will be removed", new HUD.ActionLink(progress.DebugClearInvetory));

            //mFile.AddTextLink("Unlock Trophies", "get all trophies", new HUD.ActionLink(TestAllTophiesUnlock));
            //mFile.AddTextLink("99% monster kill", "get all kills needed for the trophy except one hog", new HUD.ActionLink(debug99PercMonsterKills));
            //mFile.AddTextLink("Clear Trophies", "lock all trophies", new HUD.ActionLink(debugClearthrophies));

            //mFile.AddTextLink("Reset knock outs", "set knock out counter to zero", new HUD.ActionLink(LfRef.gamestate.Progress.debugResetHeroDeaths));
            //mFile.AddDescription(monsterSpawn.ToString());


            //if (Map.World.RunningAsHost)
            //{
            //    mFile.AddTextLink("Corrupt this chunk", "fuck up the memory for this chunk", (int)Link.DebugCorruptChunk);
            //}
            //mFile.AddTextLink("Clear settings file", new HUD.ActionLink(debugClearSettings));
            //mFile.AddDescription("hero pos:" + hero.Position);
            //mFile.AddDescription("hero objIx:" + hero.ObjOwnerAndId);
            //mFile.AddDescription(Data.WorldsSummaryColl.CurrentWorld.FolderPath);
            //mFile.AddDescription("World seed:" + Data.WorldsSummaryColl.CurrentWorld.SeedName());
            //OpenMenuFile();
        }
    }
}
