using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.HUD;
using VikingEngine.Engine;
using Microsoft.Xna.Framework.Input;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.ToGG.Commander;

namespace VikingEngine.ToGG
{
    class MenuSystem
    {
        public static readonly Color DarkBrown = new Color(92, 62, 17);
        public static readonly Color BrightBeige = new Color(255, 235, 170);

        public Gui menu = null;
        public TextFormat skillDescFormat;
        Input.InputSource inputSource;
        bool optionsMenu;

        public MenuSystem(Input.InputSource inputSource)
        {
            this.inputSource = inputSource;    
            toggRef.menu = this;
        }

        public void OpenMenu(bool inGame)
        {
            Input.Mouse.LockToScreenArea = false;

            if (menu == null)
            {
                var style = new GuiStyle(
                    (Screen.PortraitOrientation ? Screen.SafeArea.Width : Screen.SafeArea.Height) * 0.61f,
                    5, SpriteName.LFMenuRectangleSelection);
                if (inGame)
                {
                    style.Tint = DarkBrown;
                    style.SliderColor = BrightBeige;
                    style.Mid_ButtonCol = ColorExt.ChangeBrighness(BrightBeige, -20);//new Color(255, 215, 150);
                    style.Dark_LabelColor = ColorExt.ChangeBrighness(BrightBeige, -40);

                    style.BackgroundColor = new Color(120, 81, 22);

                    style.ShineColor = new Color(253, 235, 205);
                    style.textFormat.Color = Color.Black;

                    style.tooltipFormat.Color = Color.White;
                    style.tooltipBgCol = Color.Black;
                    style.headBarTextColor = Color.White;
                    style.headBarColor = style.BackgroundColor;
                }
                if (Ref.gamestate is GameState.MainMenuState ||
                    Ref.gamestate is HeroQuest.Lobby.LobbyState)
                {
                    style.headCloseIcon = SpriteName.NO_IMAGE;
                }

                menu = new Gui(style, Screen.SafeArea, inGame? 0.5f : 0, ImageLayers.Foreground7, inputSource);
                menu.useAnyControllerInput = true;

                skillDescFormat = style.textFormat;
                skillDescFormat.Color = Color.LightGray;

                if (Ref.gamestate is AbsPlayState)
                {
                    toggRef.absPlayers.OnOpenCloseMenu(true);
                }
            }
        }
        public void CloseMenu()
        {
            Input.Mouse.LockToScreenArea = true;

            if (menu != null)
            {
                if (optionsMenu)
                {
                    optionsMenu = false;
                    Ref.gamesett.Save();
                }
                menu.DeleteMe();
                menu = null;

                toggRef.absPlayers?.OnOpenCloseMenu(false);
            }
        }

        /// <returns>Close me</returns>
        public bool Update()
        {
            if (menu.Update() || CloseMenuInput())
            {
                return true;
            }
            return false;
        }

        public bool Open { get { return menu != null; } }

        public bool OverrideInput { get { return menu != null; } }

        public void DeleteMe()
        {
            Ref.gamesett.Save();
            menu.DeleteMe();
        }

        public static bool CloseMenuInput()
        {
            return Input.Keyboard.KeyDownEvent(Keys.Escape) ||
                Input.XInput.KeyDownEvent(Buttons.Start) ||
                Input.XInput.KeyDownEvent(Buttons.Back);
        }

        public void MainMenu()
        {
            OpenMenu(true);

            GuiLayout layout = new GuiLayout("Main Menu", toggRef.menu.menu);
            {
                //if (toggRef.mode == GameMode.HeroQuest)
                //{
                //    heroQuestMain(layout);
                //}
                //else
                //{

                //}
                InGameMenu(layout);

                quitToMenuButton(layout);
            }
            layout.End();

        }

        virtual protected void InGameMenu(GuiLayout layout)
        {
            commanderMain(layout);
        }

        //void heroQuestMain(GuiLayout layout)
        //{
        //    if (PlatformSettings.DevBuild)
        //    {
        //        new GuiTextButton("Give awards", null, listGiftedAwards, true, layout);
        //    }

        //    new GuiTextButton("*DEBUG*", null, HeroQuest.hqRef.players.localHost.debugOptions, true, layout);
        //}

        void commanderMain(GuiLayout layout)
        {
            if (toggRef.gamestate.gameSetup.category == GameCategory.Tutorial)
            {
                new GuiLargeTextButton("Retry", "Start over from the beginning", new GuiAction(retryLevel), false, layout);
            }
            if (toggRef.gamestate.gameSetup.category == GameCategory.Story1)
            {
                new GuiTextButton("*auto win*", "debug option", toggRef.gamestate.debugAutoWin, false, layout);
            }
        }
        
        void retryLevel()
        {
            new Commander.LevelSetup.ChallengeLevelsData().startMission(toggRef.gamestate.gameSetup.level);
        }

        public void quitToMenuButton(GuiLayout layout)
        {
            new GuiTextButton("Quit to Menu", null, quitToMenu, false, layout);


        }

        public void quitToMenu()
        {
            new GameState.ExitState();
            //new GameState.MainMenuState();
        }
        public void options()
        {
            optionsMenu = true;

            GuiLayout layout = new GuiLayout("Options", menu);
            {
                //if (PlatformSettings.DevBuild)
                {
                    new GuiTextButton("*Crash game*", "Debug blue screen", crash, false, layout);
                    //new GuiTextButton("*Unlock All*", "Will set all progress to completed", lib.Combine(cmdRef.storage.debugUnlockAll, resetGameAndSave), false, layout);
                    //new GuiTextButton("*Reset progress*", "Will remove all progress", lib.Combine(cmdRef.storage.debugResetProgress, resetGameAndSave), false, layout);
                }
                new GuiLabel("*There is currently no Sound or Music", layout);
                Ref.gamesett.optionsMenu(layout);
                if (Ref.gamesett.bannedPeers.HasMembers)
                {
                    new GuiTextButton("Banned players", null, listBlockedPlayers, true, layout);
                }
                //resolutionOptions(layout);

            }
            layout.End();

            layout.OnDelete += closingOptionsMenu;
        }

        void listBlockedPlayers()
        {
            GuiLayout layout = new GuiLayout("Blocked players", menu);
            {
                Ref.gamesett.bannedPeers.toMenu(layout, onBanRemove);
                //new GuiLabel("Click to remove block", layout);
                //foreach (var m in Ref.netSession.blockedUsers.members)
                //{
                //    new GuiTextButton(m.name, "Unblock: " + m.ToString(),
                //           new GuiAction1Arg<SteamWrapping.SteamUser>(unblockPlayer, m), true, layout);
                //}
            }
            layout.End();
        }

        void onBanRemove()
        {
            menu.PopLayouts(2);

            Ref.gamesett.Save();
        }

        void resetGameAndSave()
        {
            var state = new VikingEngine.ToGG.GameState.MainMenuState();
            toggRef.storage.saveLoad(true);
        }

        void closingOptionsMenu()
        {
            Ref.gamesett.Save();
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

        void recordingResolutionOptions()
        {
            GuiLayout layout = new GuiLayout("Recording setup", menu);
            {
                var monitor = Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter;
                for (RecordingPresets rp = 0; rp < RecordingPresets.NumNon; ++rp)
                {
                    IntVector2 sz = Engine.Screen.RecordingPresetsResolution(rp);
                    if (sz.Y > monitor.CurrentDisplayMode.Height)
                    {
                        break;
                    }
                    else
                    {
                        if (rp == Engine.Screen.UseRecordingPreset)
                        {
                            new GuiIconTextButton(SpriteName.LfCheckYes, rp.ToString(), null,
                                new GuiAction1Arg<RecordingPresets>(Ref.gamesett.setRecordingPreset, rp), false, layout);
                        }
                        else
                        {
                            new GuiTextButton(rp.ToString(), null,
                                new GuiAction1Arg<RecordingPresets>(Ref.gamesett.setRecordingPreset, rp), false, layout);
                        }
                    }
                }
            }
            layout.End();
        }

        void crash()
        {
            throw new Exception("Test crash");
        }

        public void openManualButton(GuiLayout layout)
        {
            new GuiTextButton("Manual", null, openManual, true, layout);
        }

        void openManual()
        {
#if PCGAME
            GuiLayout layout = new GuiLayout("Loading Manual", toggRef.menu.menu);
            {
                new GuiLabel("Attempting to open manual in a seperate window", layout);
                new GuiTextButton("Ok", null, toggRef.menu.menu.PopLayout, true, layout);
            }
            layout.End();
            string contentPath = Environment.CurrentDirectory + DataStream.FilePath.Dir + Ref.main.Content.RootDirectory + DataStream.FilePath.Dir + toggLib.ContentFolder + "Manual.rtf";
            System.Diagnostics.Process.Start(contentPath);
#endif
        }

        void exitToMainMenu()
        {
            new GameState.MainMenuState();
        }

        void ListUnitsMenu()
        {

            GuiLayout layout = new GuiLayout("Units", toggRef.menu.menu);
            {
                for (UnitType type = 0; type < UnitType.NUM_NONE; ++type)
                {
                    var data = cmdRef.units.GetUnit(type);
                    new GuiIconTextButton(data.modelSettings.image, data.name, null, new GuiAction2Arg<UnitType, AbsUnit>(UnitInfoMenu, type, null),
                        true, layout);
                }
            }
            layout.End();
        }
        public void UnitInfoMenu(UnitType type, AbsUnit unit)
        {
            UnitInfoMenu(type, unit, null);
        }
        public void UnitInfoMenu(UnitType type, AbsUnit unit, Action<UnitType, GuiLayout> insertLayout)
        {
            OpenMenu(false);

            AbsUnitData data;
            int health;
            string title;

            if (unit == null)
            {
                data = cmdRef.units.GetUnit(type);
                health = data.startHealth;
                title = data.Name;
            }
            else
            {
                data = unit.Data;
                health = unit.health.Value;
                title = data.Name + " (" + unit.UnitId.ToString() + ")";
            }

            GuiLayout layout = new GuiLayout(title , toggRef.menu.menu);
            {
                if (insertLayout != null)
                {
                    insertLayout(type, layout);
                }

                var icon = new GuiBigIcon(data.modelSettings.image, null, null, false, layout);
                icon.iconScale(1.6f);

                new GuiIconTextButton(SpriteName.cmdStatsMove, "Movement: " + data.move.ToString(), null, new GuiNoAction(), false, layout);
                new GuiIconTextButton(SpriteName.cmdStatsMelee, "Melee attacks: " + data.wep.meleeStrength.ToString(), null, new GuiNoAction(), false, layout);
                
                hitChanceLabel("Melee", toggLib.CloseCombatHitChance, toggLib.CloseCombatRetreatChance, layout);

                if (data.WeaponStats.HasProjectileAttack)
                {
                    new GuiIconTextButton(SpriteName.cmdStatsRangedLength, "Projectile range: " + data.wep.projectileRange.ToString(), null, new GuiNoAction(), false, layout);
                    new GuiIconTextButton(SpriteName.cmdStatsRanged, "Projectile attacks: " + data.wep.projectileStrength.ToString(), null, new GuiNoAction(), false, layout);

                    hitChanceLabel("Range", toggLib.RangedCombatHitChance, toggLib.RangedCombatRetreatChance, layout);
                }
                new GuiIconTextButton(SpriteName.cmdStatsHealth, "Health: " + health.ToString(), null, new GuiNoAction(), false, layout);

                new GuiSectionSeparator(layout);

                //var propList = data.properties.ToList((int)UnitPropertyType.Num_Non);
                foreach (var prop in data.properties.members)
                {
                    new GuiLabel(prop.Name + ":", layout);
                    new GuiLabel(prop.Desc, true, layout.gui.style.textFormat, layout);
                }

                if (unit != null && unit.progress != null)
                {
                    int pointsCollected, pointsNeeded;
                    if (unit.progress.nextLevelProgress(out pointsCollected, out pointsNeeded))
                    {
                        new GuiSectionSeparator(layout);
                        new GuiLabel("Next Level: ", layout);
                        new GuiLabel(pointsCollected.ToString() + "/" + pointsNeeded.ToString() + " Victory Points", layout);
                    }
                }

                new GuiSectionSeparator(layout);
                openManualButton(layout);

               
            }
            layout.End();
        }
        
        void hitChanceLabel(string attackType, float hit, float retreat, GuiLayout layout)
        {
            new GuiLabel("Basic " + attackType + " attack: " +
                        TextLib.PercentText(hit) + " Hit, " +
                        TextLib.PercentText(retreat) + " Retreat chance", true, layout.gui.style.textFormat, layout); 
        }

        void ListTerrainMenu()
        {
            GuiLayout layout = new GuiLayout("Units", toggRef.menu.menu);
            {
                for (SquareType type = 0; type < SquareType.NUM_NON; ++type)
                {
                    var terrainData = toggRef.sq.Get(type);
                   
                    new GuiIconTextButton(terrainData.LabelImage(), terrainData.Terrain().Name, null,
                        new GuiAction1Arg<SquareType>(TerrainInfoMenu, type), true, layout);
                }
            }
            layout.End();
        }

        public void squareInfoMenu(IntVector2 sq)
        {
            var selectedTile = toggRef.board.tileGrid.Get(sq);

            if (selectedTile.unit == null)
            {
                toggRef.menu.TerrainInfoMenu(selectedTile.squareType);
            }
            else
            {
                toggRef.menu.UnitInfoMenu(selectedTile.unit.cmd().data.Type, selectedTile.unit);
            }
        }

        public void TerrainInfoMenu(SquareType type)
        {
            TerrainInfoMenu(type, null);
        }
        public void TerrainInfoMenu(SquareType type, Action<SquareType, GuiLayout> insertLayout)
        {
            OpenMenu(false);

            var terrainData = toggRef.sq.Get(type);
            GuiLayout layout = new GuiLayout(terrainData.Terrain().Name, toggRef.menu.menu);
            {
                if (insertLayout != null)
                {
                    insertLayout(type, layout);
                }
                new GuiBigIcon(terrainData.LabelImage(), null, null, false, layout);

                //var propList = terrainData.Terrain.properties.ToList((int)TerrainPropertyType.Num_Non);
                
                foreach (var p in terrainData.Terrain().properties)
                {
                    new GuiLabel(p.Name + ":", layout);
                    new GuiLabel(p.Desc, true, toggRef.menu.skillDescFormat, layout);
                }
                
            }
            layout.End();
        }

        public void credits()
        {
            GuiLayout layout = new GuiLayout("Credits", toggRef.menu.menu);
            layout.scrollOnly = true;
            {
                var oldFormat = menu.style.textFormat;
                menu.style.textFormat.Font = LoadedFont.Console;
                menu.style.textFormat.size *= 1.6f;

                new GuiLabel(PlatformSettings.GameTitle + " - " + PlatformSettings.SteamVersion, layout);

                new GuiLabel("Art, Design & Programming:" + Environment.NewLine +
                    "Fabian \"Viking\" Jakobsson", layout);

                new GuiLabel("Music:" + Environment.NewLine +
                    "Martin \"Akri\" Grönlund", layout);

                new GuiLabel("Main playtesters:" + Environment.NewLine +
                    "Samuel Reneskog" + Environment.NewLine +
                    "Pontus Bengtsson" + Environment.NewLine +
                    "Niklas \"Kelthar\"" + Environment.NewLine +
                    "Anders \"Candypimp\" Lindberg" + Environment.NewLine +
                    "Martin \"Taur\" Sundquist"
                    , layout);

                new GuiLabel("Winner of the Creative Coast \"Game Concept Challenge\" 2018 Award", layout);

                new GuiSectionSeparator(layout);

                new GuiLabel("vikingfabian.com", layout);

                menu.style.textFormat = oldFormat;
            }
            layout.End();
        }

        void someTipsMenu()
        {
            GuiLayout layout = new GuiLayout("Tips", toggRef.menu.menu);
            {
                new GuiLabel("Victory Points (VP)", layout);
                new GuiLabel("-Reach " + toggLib.WinnerScore.ToString() + "VP to win", layout);
                new GuiLabel("-Strategic points will give you " + toggLib.VP_TacticalBanner.ToString() + "VP at the end of each turn you occupy it with one unit", layout);
                new GuiLabel("-Destroy an enemy unit to gain " + toggLib.VP_DestroyUnit.ToString() + "VP", layout);

                new GuiSectionSeparator(layout);
                new GuiLabel("Battle", layout);
                new GuiLabel("-Supporters: Each friendly unit that can attack will help out during combat and give +1 attack", layout);
                new GuiLabel("-Units will always retreat in a direction direct opposite to the attacker", layout);
                new GuiLabel("-Retreats can't be made to a square with Terrain, other units or off the board", layout);
                new GuiLabel("-Units that can't retreat will take one hit", layout);
                new GuiLabel("-A unit with two friendly adjacent units will ignore one retreat", layout);
                new GuiLabel("-The battle contains managed randomness, you can't have bad luck in the long run", layout);
            }
            layout.End();
        }
    }
}
