using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.MapEditor;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
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

        PostProcessing,
        NUM
    }

    class MapEditor2Display
    {
        Map2GeneratorTab tab = 0;
        RichMenu menu;
        MapEditor2_Scene state;
        public Vector2 topRight;
        public ImageGroup2D loadingDisplay;
        static readonly List<float> MapSizeAdd = new List<float> { 8, 64, 1024 };

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

            content.newLine();

            switch (tab)
            {
                case Map2GeneratorTab.Setup:
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
                                mapSzOptions.AddOption(WorldData.SizeString(sz),  sz == state.generateSettings.mapSize,  sz == MapSize.Medium,
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
                    break;

                case Map2GeneratorTab.Nodes:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                       new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) },
                       new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, 0, Map2Pass.NodeGrid), null, true));
                    break;

                case Map2GeneratorTab.Icon:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                       new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) },
                       new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, Map2Pass.Icon, Map2Pass.IconNoise), null, state.generator.currentPass < Map2Pass.ScaleUp));
                    break;

                case Map2GeneratorTab.Bioms:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                       new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) },
                       new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, Map2Pass.Bioms, Map2Pass.Bioms), null, state.generator.currentPass < Map2Pass.ScaleUp));
                    break;

                case Map2GeneratorTab.CityPlacements:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                       new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) },
                       new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, Map2Pass.IconCities, Map2Pass.IconCities), null, state.generator.currentPass < Map2Pass.ScaleUp));
                    break;

                case Map2GeneratorTab.PostProcessing:
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                       new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapGenerator_GenerateAction) },
                       new RbAction2Arg<Map2Pass, Map2Pass>(state.generatePass, Map2Pass.ScaleUp, Map2Pass.PostNoise)));
                    break;
            }


            menu.Refresh(content);


        }
    }

}
