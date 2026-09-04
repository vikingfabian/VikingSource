using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameState.MapEditor;
using VikingEngine.DSSWars.Interface.MapObjMenu;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Map.Terrain;
using VikingEngine.LootFest.Players;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.GameState.MapEditor2
{
    enum Map2GeneratorTab
    { 
        Setup,
        Nodes,
        
        Icon,
        
        Bioms,
        
        CityPlacements,

        Complete,
        NUM
    }

    enum Map2GeneratorSubTab
    {
        Generate,
        Paint,
        Heightmap,
        NUM
    }

    class MapEditor2Display
    {
        public Map2GeneratorTab tab = 0;
        RichMenu menu;
        MapEditor2_Scene state;
        public Vector2 topRight;
        public ImageGroup2D loadingDisplay;
        static readonly List<float> MapSizeAdd = new List<float> { 8, 64, 1024 };

        Map2GeneratorSubTab subTab = Map2GeneratorSubTab.Generate;

        public MapEditor2Display(MapEditor2_Scene state)
        { 
            this.state = state;
            var area = Screen.SafeArea;
            area.Width = Screen.IconSize * 8;

            topRight = area.RightTop;
            topRight.X += Engine.Screen.BorderWidth;

            menu = new RichMenu(HudLib.RbSettings, area, new Vector2(10), RichMenu.DefaultRenderEdge, ImageLayers.Top2, new PlayerData(PlayerData.AllPlayers));
            menu.addBackground(HudLib.HudMenuBackground, ImageLayers.Top2_Back);


            TextG loadingText = new TextG(LoadedFont.Regular, Engine.Screen.Area.PercentToPosition(0.5f, 0.2f), Screen.TextSizeV2 * 2f, Align.CenterAll, DssRef.lang.Hud_Loading, Color.White, ImageLayers.Top0_Front, true);
            var loadArea = loadingText.GetArea();
            loadArea.Size.X += Screen.IconSize * 0.5f;
            Graphics.Image loadingSpinner = new Image(SpriteName.WhiteArea, loadArea.RightCenter, Screen.IconSizeV2 * 0.6f, ImageLayers.Top1_Front, true);
            loadArea.Size.X += Screen.IconSize * 0.5f;
            loadArea.AddRadius(Screen.IconSize * 0.5f);

            Graphics.Image loadingBg = new Image(SpriteName.WhiteArea, loadArea.Position, loadArea.Size, ImageLayers.Top1_Back, false);
            loadingBg.ColorAndAlpha(Color.Black, 0.2f);

            new Motion2d(MotionType.ROTATE, loadingSpinner, new Vector2(MathHelper.Tau * 0.5f), MotionRepeate.Loop, 1000, true);

            loadingDisplay = new ImageGroup2D(new List<AbsDraw2D> { loadingText, loadingSpinner, loadingBg });
            loadingDisplay.Hide();

            refreshMenu();
        }

        public void update(ref bool mouseOver)
        {
            menu.updateMouseInput(ref mouseOver);
            if (menu.needRefresh)
            {
                refreshMenu();
            }
        }

        public void refreshMenu()
        {
            if (state.iconState)
            {
                iconMenu();
            }
        }

        void iconMenu()
        {
            RichBoxContent content = new RichBoxContent();
            content.h1("Map 2.0 - Icon editor", HudLib.TitleColor_Head);

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary,
                       new List<AbsRichBoxMember> { new RbText("Generate all") }, 
                       new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, 0, Map2Pass.NUM)));
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary,
                       new List<AbsRichBoxMember> { new RbText("Clear") },
                       new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, 0, Map2Pass.NewWorld)));

            content.newParagraph();

            var tabs = new List<ArtTabMember>();
            {
                for (Map2GeneratorTab tabType = 0; tabType < Map2GeneratorTab.NUM; tabType++)
                {
                    tabs.Add(new ArtTabMember(new List<AbsRichBoxMember> { new RbText(tabType.ToString()) }));
                }

                var tabGroup = new ArtTabgroup(tabs, (int)tab, (int ix) =>
                {
                    tab = (Map2GeneratorTab)ix;
                    
                }, null);

                content.Add(tabGroup);
            }
            state.tool?.refreshTools(tab);
            content.newLine();

            switch (tab)
            {
                case Map2GeneratorTab.Setup:
                    tab_setup(content);
                    break;

                case Map2GeneratorTab.Nodes:
                    tab_nodes(content);
                    break;

                case Map2GeneratorTab.Icon:
                    tab_Icon(content);
                    break;

                case Map2GeneratorTab.Bioms:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                       new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) },
                       new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, Map2Pass.Bioms, Map2Pass.Bioms), null, state.generator.currentPass < Map2Pass.ScaleUp));

                    if (state.generator.currentPass > Map2Pass.NodeGrid && state.generator.currentPass < Map2Pass.ScaleUp)
                    {
                        paintHud(content, true , false, false, true, false);
                    }
                    break;

                case Map2GeneratorTab.CityPlacements:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                       new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) },
                       new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, Map2Pass.IconCities, Map2Pass.IconCities), null, state.generator.currentPass < Map2Pass.ScaleUp));
                    break;

                case Map2GeneratorTab.Complete:
                    if (state.generator.currentPass < Map2Pass.ScaleUp)
                    {
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                           new List<AbsRichBoxMember> { new RbText("Post process map") },
                           new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, Map2Pass.ScaleUp, Map2Pass.PostNoise), null, state.generator.currentPass < Map2Pass.ScaleUp));
                    }
                    else
                    {
                        content.Add(new ArtButton(RbButtonStyle.Primary,
                           new List<AbsRichBoxMember> { new RbImage(SpriteName.Undo), new RbSpace(0.5f), new RbText("Revert to icon") },
                           new RbAction(state.revertToIconPass)));
                    }
                    content.newParagraph();
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                        new List<AbsRichBoxMember> { new RbText(DssRef.lang.Lobby_ExitGame) }, new RbAction(exit)));
                    break;
            }


            menu.Refresh(content);


        }

        private void tab_setup(RichBoxContent content)
        {
            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_Terrain_CustomSize) }, state.generateSettings.CustomSizeProperty));

            if (state.generateSettings.bCustomSize)
            {
                content.newLine();
                content.Add(new RbText(DssRef.lang.Hud_Vector_X + ":", HudLib.TitleColor_Label));
                content.space();
                RbDragButton.RbDragButtonGroup(content, MapSizeAdd, new DragButtonSettings(WorldData.CustomMapSize_Min, WorldData.CustomMapSize_Max, 8), state.generateSettings.MapXProperty, false);

                content.newLine();
                content.Add(new RbText(DssRef.lang.Hud_Vector_Y + ":", HudLib.TitleColor_Label));
                content.space();
                RbDragButton.RbDragButtonGroup(content, MapSizeAdd, new DragButtonSettings(WorldData.CustomMapSize_Min, WorldData.CustomMapSize_Max, 8), state.generateSettings.MapYProperty, false);
            }
            else
            {

                DropDownBuilder mapSzOptions = new DropDownBuilder("mapSz");
                {
                    for (MapSize sz = 0; sz < MapSize.NUM; ++sz)
                    {
                        mapSzOptions.AddOption(WorldData.SizeString(sz), sz == state.generateSettings.mapSize, sz == MapSize.Medium,
                            new RbAction1Arg<MapSize>((MapSize selected) =>
                            {
                                state.generateSettings.mapSize = selected;
                                state.generateSettings.customMapSize = WorldData.SizeDimentions(DssRef.storage.ruleset.mapSize);
                                menu.CloseDropDown();

                            }, sz), null);
                    }
                    mapSzOptions.Build(content, SpriteName.NO_IMAGE, DssRef.lang.Lobby_MapSizeTitle, menu);
                }
            }
        }

        private void tab_Icon(RichBoxContent content)
        {
            for (Map2GeneratorSubTab st = 0; st < Map2GeneratorSubTab.NUM; st++)
            {
                string caption;
                switch (st)
                {
                    default:
                    case Map2GeneratorSubTab.Generate:
                        caption = DssRef.lang.MapGenerator_GenerateAction;
                        break;
                    case Map2GeneratorSubTab.Paint:
                        caption = "Paint";
                        break;
                    case Map2GeneratorSubTab.Heightmap:
                        caption = "Height map";
                        break;

                }

                content.Add(new ArtButton(subTab ==  st? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected,
                new List<AbsRichBoxMember> { new RbText(caption) },
                new RbAction1Arg<Map2GeneratorSubTab>((Map2GeneratorSubTab selected) =>
                {
                    subTab = selected;
                }, st, RbSoundType.Tab)));
            }


            content.newParagraph();
            switch (subTab)
            {
                case Map2GeneratorSubTab.Generate:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                       new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) },
                       new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, Map2Pass.Icon, Map2Pass.IconNoise), null, state.generator.currentPass < Map2Pass.ScaleUp));
                    break;

                case Map2GeneratorSubTab.Paint:
                    paintHud(content, false, true, true, true, true);
                    break;

                case Map2GeneratorSubTab.Heightmap:
                    tab_icon_heightmap(content);
                    break;
            }
           
        }

        private void tab_icon_heightmap(RichBoxContent content)
        {
            var heightMap = state.generator.heightMapTexture;

            content.h2("Load a height map texture", HudLib.TitleColor_Head2);
            string explanation = "Supported files: " + string.Join(", ", StreamLib.ValidTextureExtensions) + ".";
            content.text(explanation);

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary,
               new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconOpen), new RbSpace(0.5f), new RbText("Select height map") },
               new RbAction(importSaves), null, state.generator.currentPass < Map2Pass.ScaleUp));

            if (heightMap != null)
            {
                content.text(heightMap.Name, HudLib.TitleColor_Name);
                content.text(LangLib.CanvasSize(heightMap.pixelTexture.Width.ToString(), heightMap.pixelTexture.Height.ToString()), HudLib.InfoYellow_Light);

                content.newParagraph();
                HudLib.Label(content, DssRef.lang.HUD_Scale);
                content.newLine();
                RbDragButton.EqualToButton(content, 2f, heightMap.scaleProperty);
                RbDragButton.EqualToButton(content, 1f, heightMap.scaleProperty);
                RbDragButton.EqualToButton(content, 0.5f, heightMap.scaleProperty);
                RbDragButton.EqualToButton(content, 0.25f, heightMap.scaleProperty);
                content.newLine();
                RbDragButton.RbDragButtonGroup(content, new List<float> { 0.5f, 0.1f, }, new DragButtonSettings(0.1f, 10f, 0.05f), heightMap.scaleProperty, false);

                content.newParagraph();
                HudLib.Label(content, "Offset X");
                content.newLine();
                offSetDragButton(heightMap.offSetXProperty, heightMap.pixelTexture.Width);

                content.newParagraph();
                HudLib.Label(content, "Offset Y");
                content.newLine();
                offSetDragButton(heightMap.offSetYProperty, heightMap.pixelTexture.Height);


                content.newParagraph();
                HudLib.Label(content, "Top height");
                content.newLine();
                RbDragButton.EqualToButton(content, Map2Generator.Height_MountainPeek, heightMap.topHeightProperty);
                content.newLine();
                RbDragButton.RbDragButtonGroup(content, new List<float> { 0.1f, 0.05f }, new DragButtonSettings(Map2Generator.Height_DefaultGround, Map2Generator.Height_MountainPeek + 0.2f, 0.01f), heightMap.topHeightProperty, false);

                content.newParagraph();
                HudLib.Label(content, "Bottom height");
                content.newLine();
                RbDragButton.EqualToButton(content, Map2Generator.Height_WaterBottom, heightMap.bottomHeightProperty);
                content.newLine();
                RbDragButton.RbDragButtonGroup(content, new List<float> { 0.1f, 0.05f }, new DragButtonSettings(Map2Generator.Height_WaterBottom, 0, 0.01f), heightMap.bottomHeightProperty, false);

                content.newParagraph();
                content.Add(new ArtButton(RbButtonStyle.Primary,
                   new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Apply) },
                   new RbAction(state.generateHeightMap), null, state.generator.currentPass < Map2Pass.ScaleUp)
                { fillWidth = true });

                void offSetDragButton(IntGetSetTag property, int textureSize)
                {
                    RbDragButton.EqualToButton(content, 0, property);
                    content.newLine();
                    var options = new List<float>(4);
                    options.Add(1);

                    if (textureSize >= 1000)
                    {
                        options.Add(1000);
                    }
                    if (textureSize >= 250)
                    {
                        options.Add(250);
                    }
                    if (textureSize >= 100)
                    {
                        options.Add(100);
                    }
                    if (textureSize >= 50)
                    {
                        options.Add(50);
                    }

                    RbDragButton.RbDragButtonGroup(content, options, new DragButtonSettings(-textureSize, textureSize, 1), property, true);

                }
            }
        }

        private void tab_nodes(RichBoxContent content)
        {
            content.newLine();
            HudLib.Label(content, "Fill");
            content.Add(new RbTab(0.25f));
            RbDragButton.RbDragButtonGroup(content, new List<float> { 10 }, new DragButtonSettings(5, 80, 5), state.generateSettings.nodeFillPercProperty, false);
            content.newLine();
            HudLib.Label(content, "Connect");
            content.Add(new RbTab(0.25f));
            RbDragButton.RbDragButtonGroup(content, new List<float> { 10 }, new DragButtonSettings(10, 90, 1), state.generateSettings.nodeConnectPercProperty, false);

            content.newParagraph();

            content.Add(new ArtButton(RbButtonStyle.Primary,
               new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) },
               new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, 0, Map2Pass.NodeGrid), null, true));
            

            if (state.generator.currentPass == Map2Pass.NodeGrid)
            {
                paintHud(content, false, true, true, false, false);
            }
        }

        private void paintHud(RichBoxContent content, bool bioms, bool clear, bool bAddType, bool bNoise, bool advancedPainting)
        {
            content.Add(new RbSeperationLine());

            content.h2("Paint tools", HudLib.TitleColor_Head2);

            if (bioms)
            {
                HudLib.Label(content, DssRef.lang.CityBiome_Title);
                content.newLine();
                for (BiomType tbiom = 0; tbiom < BiomType.NUM; tbiom++)
                {
                    content.Add(new ArtOption(tbiom == state.tool.biom, new List<AbsRichBoxMember> { new RbImage(SpriteName.WhiteArea_LFtiles, 0.9f, DssRef.map.bioms.bioms[(int)tbiom].colors_height[4].Color) },
                        new RbAction1Arg<BiomType>((BiomType selected) => { state.tool.biom = selected; }, tbiom)));
                }
                content.newParagraph();
            }

            
            content.newLine();
            HudLib.Label(content, "Canvas");
            content.Add(new RbTab(0.25f));
            content.Add(new ArtButton(RbButtonStyle.Primary,
               new List<AbsRichBoxMember> { new RbText("Fill") }, new RbAction(state.tool.fill), null, true));
            if (clear)
            {
                content.Add(new RbTab(0.25f));

                content.Add(new ArtButton(RbButtonStyle.Primary,
                   new List<AbsRichBoxMember> { new RbText("Clear") }, new RbAction(state.tool.clear), null, true));
            }
            HudLib.Label(content, "Pen size");
            content.Add(new RbTab(0.25f));
            RbDragButton.RbDragButtonGroup(content, new List<float> { 5 }, new DragButtonSettings(1, state.tool.toolSettings.maxPenSize, 1), state.tool.penSizeProperty, false);

            if (bAddType && (!advancedPainting || !state.tool.setHeightProperty(null, false, false)))
            {
                content.newLine();
                for (ToolAddType addType = 0; addType < ToolAddType.NUM_NONE; addType++)
                {
                    if (tab == Map2GeneratorTab.Icon && addType == ToolAddType.Toggle)
                    {
                        continue;
                    }

                    Ref.langOpt.ToolAddType(addType, out var addIcon, out var addCaption);
                    content.Add(new ArtOption(addType == state.tool.toolSettings.addType, new List<AbsRichBoxMember> { new RbImage(addIcon) },
                        new RbAction1Arg<ToolAddType>((ToolAddType selected) => { state.tool.toolSettings.addType = selected; }, addType),
                        new RbTooltip_Text(addCaption)));
                }
            }

            if (advancedPainting)
            {
                content.newLine();
                HudLib.Label(content, "Height"); content.Add(new RbTab(0.25f));
                RbDragButton.RbDragButtonGroup(content, new List<float> { 0.25f, 1f },  new DragButtonSettings(state.tool.setHeightProperty(null, false, false)? Map2Generator.Height_WaterBottom : 0.05f, Map2Generator.Height_MountainPeek, 0.05f),
                    state.tool.heightProperty, false);
                
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Set height") }, state.tool.setHeightProperty, null));

                content.newLine();
                HudLib.Label(content, "Flatness"); content.Add(new RbTab(0.25f));
                RbDragButton.RbDragButtonGroup(content, new List<float> { 25 }, new DragButtonSettings(0, 100, 5),
                    state.tool.flatnessProperty, false);
            }

            content.newLine();
            for (PencilShape shape = 0; shape < PencilShape.NUM; shape++)
            {
                content.Add(new ArtOption(shape == state.tool.toolSettings.pencilShape, new List<AbsRichBoxMember> { new RbText(shape.ToString()) },
                    new RbAction1Arg<PencilShape>((PencilShape selected) => { state.tool.toolSettings.pencilShape = selected; }, shape),
                    new RbTooltip_Text("Pen shape")));
            }

            if (bNoise)
            {
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Noise") },state.tool.noiseProperty, null));
            }
        }

        void exit()
        {
            new ExitToLobby(false);
        }

        bool importSavesMenu = false;
       
        void importSaves()
        {
            RichBoxContent content = new RichBoxContent();
            HudLib.returnButton(content, menu, true, null);


            //var saves = DssRef.storage.meta.ListHeightMaps();
            importSavesMenu = true;

            content.Add(new RbText(DssRef.lang.Hud_Loading, HudLib.InfoYellow_Light));

            menu.menuStack.Add("import");
            menu.Refresh(content);
            new Timer.AsynchActionTrigger(loadHeightmapList_async, true);


        }

        void loadHeightmapList_async()
        {
            var list = DssRef.storage.meta.ListHeightMaps();

            List<TwoStrings> nameAndPath = new List<TwoStrings>(list.Count);
            for (int i = 0; i < list.Count; ++i)
            {
                nameAndPath.Add(new TwoStrings(list[i].Split(Path.DirectorySeparatorChar).Last(), list[i]));
            }

            new Timer.Action1ArgTrigger<List<TwoStrings>>(listHeightMaps, nameAndPath);
        }

        void listHeightMaps(List<TwoStrings> name_Path)
        {
            RichBoxContent content = new RichBoxContent();
            HudLib.returnButton(content, menu, true, null);

            if (importSavesMenu)
            {
                for (int i = 0; i < name_Path.Count; ++i)
                {
                    var save = name_Path[i];
                    var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                                    new RbImage(SpriteName.WarsHudIconImport),
                                    new RbSpace(),
                                    new RbText(LoadContent.CheckCharsSafety(save.String1, LoadedFont.Regular)),

                                },
                new RbAction1Arg<string>(importHeightMap, save.String2));

                    btn.fillWidth = true;
                    content.Add(btn);
                    
                }

                if (name_Path.Count == 0)
                {
                    content.Add(new RbText(DssRef.lang.Hud_EmptyList, HudLib.InfoYellow_Light));
                }
            }
            menu.Refresh(content);
        }

        void importHeightMap(string path)
        {
            //SaveStateMeta meta = new SaveStateMeta();
            //meta.import = name;
            //meta.importedWorld = true;
            //loadGame = meta;
            //openPlayerSetupForMode(StartGameMode.Play);

            state.generator.addHeightMap(new HeightMapTexture(path));
            state.generateHeightMap();

        }

    }

}
