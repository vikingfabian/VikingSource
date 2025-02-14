using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.GO.Gadgets;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class MapGeneratorDisplay
    {
        static readonly DragButtonSettings BuildDigLoopBounds = new DragButtonSettings(1, 10, 1);
        static readonly DragButtonSettings StrokeCountBounds = new DragButtonSettings(0f, 10, 0.01f);
        static readonly List<float> MapSizeAdd = new List<float> { 8, 64, 1024 };

        RichMenu menu;
        MapEditor_Generator state;
        MapGeneratorTab tab=0;
        public Vector2 topRight;
        public MapGeneratorDisplay(MapEditor_Generator state) 
        { 
            this.state = state;

            var area = Screen.SafeArea;
            area.Width = Screen.IconSize * 8;
            
            state.GenerateSettings.customMapSize = WorldData.SizeDimentions(DssRef.storage.mapSize);

            topRight = area.RightTop;
            topRight.X += Engine.Screen.BorderWidth;

            menu = new RichMenu(HudLib.RbSettings, area, new Vector2(10), RichMenu.DefaultRenderEdge, ImageLayers.Top1, new PlayerData(PlayerData.AllPlayers));
            menu.addBackground(HudLib.HudMenuBackground, ImageLayers.Top1_Back);

            refreshMenu();
        }

        public void refreshMenu()
        {
            RichBoxContent content = new RichBoxContent();
            content.h1("Map editor - generate", HudLib.TitleColor_Head);

            content.newLine();

            var tabs = new List<ArtTabMember>();
            {
                for (MapGeneratorTab tabType = 0; tabType < MapGeneratorTab.NUM; tabType++)
                {
                    tabs.Add(new ArtTabMember(new List<AbsRichBoxMember> { new RbText(tabType.ToString()) }));
                }

                var tabGroup = new ArtTabgroup(tabs, (int)tab, (int ix) =>
                {
                    tab = (MapGeneratorTab)ix;
                }, null);

                content.Add(tabGroup);
            }

            content.newLine();

            switch (tab)
            {
                case MapGeneratorTab.Ground:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                        new List<AbsRichBoxMember> { new RbText("Generate") }, new RbAction1Arg< GenerateMapPass>(state.generatePass, GenerateMapPass.AllTerrain)));

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Custom size") }, state.GenerateSettings.CustomSizeProperty));

                    if (state.GenerateSettings.bCustomSize)
                    {
                        content.newLine();
                        content.Add(new RbText("X:", HudLib.TitleColor_Label));
                        content.space();
                        RbDragButton.RbDragButtonGroup(content, MapSizeAdd, new DragButtonSettings(WorldData.CustomMapSize_Min, WorldData.CustomMapSize_Max, 8), state.GenerateSettings.MapXProperty);

                        content.newLine();
                        content.Add(new RbText("Y:", HudLib.TitleColor_Label));
                        content.space();
                        RbDragButton.RbDragButtonGroup(content, MapSizeAdd, new DragButtonSettings(WorldData.CustomMapSize_Min, WorldData.CustomMapSize_Max, 8), state.GenerateSettings.MapYProperty);
                    }
                    else
                    {
                        GameStorage defaultOptions = new GameStorage();
                        DropDownBuilder mapSzOptions = new DropDownBuilder("mapSz");
                        {
                            for (MapSize sz = 0; sz < MapSize.NUM; ++sz)
                            {
                                mapSzOptions.AddOption(WorldData.SizeString(sz), DssRef.storage.mapSize == sz, defaultOptions.mapSize == sz,
                                    new RbAction1Arg<MapSize>(setMapSize, sz), null);
                            }
                            mapSzOptions.Build(content, DssRef.lang.Lobby_MapSizeTitle, menu);
                        }
                    }

                    DropDownBuilder startAs = new DropDownBuilder("start");
                    {
                        for (MapStartAs mapStartAs = 0; mapStartAs < MapStartAs.NUM; ++mapStartAs)
                        {
                            startAs.AddOption(mapStartAs.ToString(), mapStartAs == Sett.StartAs, mapStartAs == 0,
                                new RbAction1Arg<MapStartAs>((MapStartAs value) =>
                                {
                                    Sett.StartAs = value;
                                    menu.CloseDropDown();
                                }, mapStartAs), null);
                        }
                        startAs.Build(content, "Start as", menu);                        
                    }

                    if (Adv)
                    {
                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember> { new RbText("Run Clear Pass") }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.Clear)));
                    }

                    content.newLine();

                    HudLib.Label(content, "Build-Dig loop count");
                    content.space();
                    RbDragButton.RbDragButtonGroup(content, new List<float> { 1 }, BuildDigLoopBounds,
                        (bool set, int value) =>
                        {

                            if (set)
                            {
                                this.Sett.repeatBuildDigCount = value;
                            }
                            return this.Sett.repeatBuildDigCount;
                        });

                    content.newLine();

                    HudLib.Label(content, "Build strokes count");
                    content.space();
                    RbDragButton.RbDragButtonGroup(content, new List<float> { 0.1f }, StrokeCountBounds,
                        (bool set, float value) =>
                        {
                            if (set)
                            {
                                Sett.BuildChainsCount_per100Tiles = value;
                            }
                            return Sett.BuildChainsCount_per100Tiles;
                        }, false);
                    content.space();
                    HudLib.InfoButton(content, new RbTooltip_Text("Measured in paint strokes per 100 tiles"));

                    if (Adv)
                    {
                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember> { new RbText("Run Build Pass") }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.Build), null, state.canRunPass(GenerateMapPass.Build)));
                    }
                    content.newLine();

                    HudLib.Label(content, "Dig strokes count");
                    content.space();
                    RbDragButton.RbDragButtonGroup(content, new List<float> { 0.1f }, StrokeCountBounds,
                        (bool set, float value) =>
                        {
                            if (set)
                            {
                                this.Sett.DigChainsCount_per100Tiles = value;
                            }
                            return this.Sett.DigChainsCount_per100Tiles;
                        }, false);
                    content.space();
                    HudLib.InfoButton(content, new RbTooltip_Text("Measured in paint strokes per 100 tiles"));

                    if (Adv)
                    {
                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember> { new RbText("Run Dig Pass") }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.Dig), null, state.canRunPass(GenerateMapPass.Dig)));
                    }

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Cleanup of single tiles") }, state.GenerateSettings.CleanUpProperty));

                    if (Adv)
                    {
                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember> { new RbText("Run cleanup Pass") }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.CleanUp), null, state.canRunPass(GenerateMapPass.CleanUp)));
                    }

                    break;
                //case MapGeneratorTab.Step:

                //    break;
                case MapGeneratorTab.Populate:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                        new List<AbsRichBoxMember> { new RbText("Generate") }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.AllPopulation), null, state.canRunPass(GenerateMapPass.AllPopulation)));


                    if (Adv)
                    {
                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember> { new RbText("Run Clear pass") }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.ClearPopulation), null, state.canRunPass(GenerateMapPass.ClearPopulation)));
                    }
                    break;
                case MapGeneratorTab.Complete:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                        new List<AbsRichBoxMember> { new RbText(DssRef.lang.Settings_NewGame) }, new RbAction(state.startNewGame), null, DssRef.world != null && DssRef.world.generatePassCompleted >= GenerateMapPass.Countries));
                    break;

            }

            
            content.newParagraph();
            content.Add(new RbSeperationLine());
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Advanced settings") }, state.storage.ViewAdvancedProperty));
            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary,
                new List<AbsRichBoxMember> { new RbText(DssRef.lang.Lobby_ExitGame) }, new RbAction(exit)));

            menu.Refresh(content);
        }

        public void setMapSize(MapSize value)
        {
            DssRef.storage.mapSize = value;
            state.GenerateSettings.customMapSize = WorldData.SizeDimentions(DssRef.storage.mapSize);
            menu.CloseDropDown();
        }

        public void update(ref bool mouseOver)
        {
            menu.updateMouseInput(ref mouseOver);
            if (menu.needRefresh)
            {
                refreshMenu();
            }
        }

        void exit()
        {
            new ExitGamePlay();
        }

        
        Map.Generate.MapGenerateSettings Sett => state.GenerateSettings;

        bool Adv => state.storage.viewAdvancedSettings;

        
    }
    enum MapGeneratorTab
    {
        Ground,
        //Step,
        Populate,
        Complete,
        NUM
    }
}
