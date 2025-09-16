using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.LootFest.GO.Gadgets;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class MapGeneratorDisplay
    {
        static readonly DragButtonSettings BuildDigLoopBounds = new DragButtonSettings(1, 10, 1);
        static readonly DragButtonSettings StrokeCountBounds = new DragButtonSettings(0f, 10, 0.01f);
        static readonly List<float> MapSizeAdd = new List<float> { 8, 64, 1024 };

        RichMenu menu;
        MapEditor_GeneratorScene state;
        MapGeneratorTab tab=0;
        public Vector2 topRight;

        public ImageGroup2D loadingDisplay;
        

        public MapGeneratorDisplay(MapEditor_GeneratorScene state) 
        { 
            this.state = state;

            var area = Screen.SafeArea;
            area.Width = Screen.IconSize * 8;
            
            state.GenerateSettings.customMapSize = WorldData.SizeDimentions(DssRef.storage.gameRuleset.mapSize);

            topRight = area.RightTop;
            topRight.X += Engine.Screen.BorderWidth;

            menu = new RichMenu(HudLib.RbSettings, area, new Vector2(10), RichMenu.DefaultRenderEdge, ImageLayers.Top2, new PlayerData(PlayerData.AllPlayers));
            menu.addBackground(HudLib.HudMenuBackground, ImageLayers.Top2_Back);


            TextG loadingText = new TextG(LoadedFont.Regular, Engine.Screen.Area.PercentToPosition(0.5f, 0.2f), Screen.TextSizeV2 * 2f, Align.CenterAll, DssRef.lang.Hud_Loading, Color.White, ImageLayers.Top0_Front, true);
            var loadArea = loadingText.GetArea();
            loadArea.Size.X += Screen.IconSize * 0.5f;
            Graphics.Image loadingSpinner =new Image(SpriteName.WhiteArea, loadArea.RightCenter, Screen.IconSizeV2 * 0.6f, ImageLayers.Top1_Front, true);
            loadArea.Size.X += Screen.IconSize * 0.5f;
            loadArea.AddRadius(Screen.IconSize * 0.5f);

            Graphics.Image loadingBg = new Image(SpriteName.WhiteArea, loadArea.Position, loadArea.Size, ImageLayers.Top1_Back, false);
            loadingBg.ColorAndAlpha(Color.Black, 0.2f);

            new Motion2d(MotionType.ROTATE, loadingSpinner, new Vector2(MathHelper.Tau * 0.5f), MotionRepeate.Loop, 1000, true);

            loadingDisplay = new ImageGroup2D(new List<AbsDraw2D> { loadingText, loadingSpinner, loadingBg });
            loadingDisplay.Hide();

            refreshMenu();
        }

        public void refreshMenu()
        {
            RichBoxContent content = new RichBoxContent();
            content.h1(DssRef.lang.MapGenerator_Name, HudLib.TitleColor_Head);

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
                        new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) }, new RbAction1Arg< GenerateMapPass>(state.generatePass, GenerateMapPass.AllTerrain)));

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_Terrain_CustomSize) }, state.GenerateSettings.CustomSizeProperty));

                    if (state.GenerateSettings.bCustomSize)
                    {
                        content.newLine();
                        content.Add(new RbText(DssRef.lang.Hud_Vector_X + ":", HudLib.TitleColor_Label));
                        content.space();
                        RbDragButton.RbDragButtonGroup(content, MapSizeAdd, new DragButtonSettings(WorldData.CustomMapSize_Min, WorldData.CustomMapSize_Max, 8), state.GenerateSettings.MapXProperty, false);

                        content.newLine();
                        content.Add(new RbText(DssRef.lang.Hud_Vector_Y + ":", HudLib.TitleColor_Label));
                        content.space();
                        RbDragButton.RbDragButtonGroup(content, MapSizeAdd, new DragButtonSettings(WorldData.CustomMapSize_Min, WorldData.CustomMapSize_Max, 8), state.GenerateSettings.MapYProperty, false);
                    }
                    else
                    {
                        GameStorage defaultOptions = new GameStorage();
                        DropDownBuilder mapSzOptions = new DropDownBuilder("mapSz");
                        {
                            for (MapSize sz = 0; sz < MapSize.NUM; ++sz)
                            {
                                mapSzOptions.AddOption(WorldData.SizeString(sz), DssRef.storage.gameRuleset.mapSize == sz, defaultOptions.gameRuleset.mapSize == sz,
                                    new RbAction1Arg<MapSize>(setMapSize, sz), null);
                            }
                            mapSzOptions.Build(content, SpriteName.NO_IMAGE, DssRef.lang.Lobby_MapSizeTitle, menu);
                        }
                    }

                    DropDownBuilder startAs = new DropDownBuilder("start");
                    {
                        for (MapStartAs mapStartAs = 0; mapStartAs < MapStartAs.NUM; ++mapStartAs)
                        {
                            string caption = null;
                            switch (mapStartAs)
                            {
                                case MapStartAs.Water:
                                    caption = DssRef.lang.MapStartAs_Water;
                                    break;
                                case MapStartAs.Land:
                                    caption = DssRef.lang.MapStartAs_Land;
                                    break;
                                case MapStartAs.Circle:
                                    caption = DssRef.lang.MapStartAs_Circle;
                                    break;
                            }

                            startAs.AddOption(mapStartAs.ToString(), mapStartAs == Sett.StartAs, mapStartAs == 0,
                                new RbAction1Arg<MapStartAs>((MapStartAs value) =>
                                {
                                    Sett.StartAs = value;
                                    menu.CloseDropDown();
                                }, mapStartAs), null);
                        }
                        startAs.Build(content, SpriteName.NO_IMAGE, DssRef.lang.MapGenerator_Terrain_StartAs, menu);                        
                    }

                    if (Adv)
                    {
                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_Terrain_ClearPass) }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.Clear)));
                    }

                    content.newLine();

                    HudLib.Label(content, DssRef.lang.MapGenerator_Terrain_BuildDigLoops);
                    content.space();
                    RbDragButton.RbDragButtonGroup(content, new List<float> { 1 }, BuildDigLoopBounds,
                        (bool set, int value) =>
                        {

                            if (set)
                            {
                                this.Sett.repeatBuildDigCount = value;
                            }
                            return this.Sett.repeatBuildDigCount;
                        }, false);

                    content.newLine();

                    HudLib.Label(content, DssRef.lang.MapGenerator_Terrain_BuildStrokes);
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
                    HudLib.InfoButton(content, new RbTooltip_Text(DssRef.lang.MapGenerator_Terrain_BuildStrokes_Description));

                    if (Adv)
                    {
                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_Terrain_BuildPass) }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.Build), null, state.canRunPass(GenerateMapPass.Build)));
                    }
                    content.newLine();

                    HudLib.Label(content, DssRef.lang.MapGenerator_Terrain_DigStrokes);
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
                    HudLib.InfoButton(content, new RbTooltip_Text(DssRef.lang.MapGenerator_Terrain_BuildStrokes_Description));

                    if (Adv)
                    {
                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_Terrain_DigPass) }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.Dig), null, state.canRunPass(GenerateMapPass.Dig)));
                    }

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_Terrain_CleanUp_Option) }, state.GenerateSettings.CleanUpProperty));

                    if (Adv)
                    {
                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_Terrain_CleanUpPass) }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.CleanUp), null, state.canRunPass(GenerateMapPass.CleanUp)));
                    }

                    break;
                //case MapGeneratorTab.Step:

                //    break;
                case MapGeneratorTab.Populate:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                        new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.AllPopulation), null, state.canRunPass(GenerateMapPass.AllPopulation)));


                    if (Adv)
                    {
                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                            new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_Terrain_ClearPass) }, new RbAction1Arg<GenerateMapPass>(state.generatePass, GenerateMapPass.ClearPopulation), null, state.canRunPass(GenerateMapPass.ClearPopulation)));
                    }
                    break;
                case MapGeneratorTab.Complete:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                        new List<AbsRichBoxMember> { new RbText(DssRef.lang.Settings_NewGame) }, new RbAction(state.startNewGame), null, DssRef.world != null && DssRef.world.generatePassCompleted >= GenerateMapPass.Countries));

#if DEBUG
                    content.text("Finns inget sätt att ladda!");

                    content.newParagraph();
                    var editButton = new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.InterfaceTextInput) },
                       new RbAction(beginEditName), null);
                    content.Add(editButton);
                    content.space();
            
                    var nameText = new RbText(state.mapStorage.Name);
                    nameText.overrideColor = Color.LightYellow;
                    content.Add(nameText);


                    content.newLine();
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Save) }, new RbAction(state.saveMap), null, DssRef.world != null));
#endif
                    break;

            }

            
            content.newParagraph();
            content.Add(new RbSeperationLine());
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_AdvancedSettings) }, state.userStorage.ViewAdvancedProperty));
            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary,
                new List<AbsRichBoxMember> { new RbText(DssRef.lang.Lobby_ExitGame) }, new RbAction(exit)));

            menu.Refresh(content);
        }

        public void beginEditName()
        {
            new TextInput(state.mapStorage.Name, NameEditEvent, null);
        }

        virtual protected void NameEditEvent(string result, object tag)
        {
            if (result != null)
            {
                state.mapStorage.customName = TextLib.checkFileName(result);
            }
            refreshMenu();
        }
        public void setMapSize(MapSize value)
        {
            DssRef.storage.gameRuleset.mapSize = value;
            state.GenerateSettings.customMapSize = WorldData.SizeDimentions(DssRef.storage.gameRuleset.mapSize);
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
            new ExitToLobby(false);
        }

        
        Map.Generate.MapGenerateSettings Sett => state.GenerateSettings;

        bool Adv => state.userStorage.viewAdvancedSettings;

        
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
