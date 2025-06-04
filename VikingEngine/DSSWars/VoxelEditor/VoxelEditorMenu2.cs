using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.Engine;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.LootFest.Players;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars.VoxelEditor
{
    class VoxelEditorMenu2 : AbsDesignMenuSystem_Base
    {
        RichMenuControllerPointer controllerPointer = null;
        VoxelDesigner designer;
        public RichMenu menu;
        public VoxelEditorMenu2(VoxelDesigner designer)
        { 
            this.designer = designer;
        }

        override public void openMenu()
        {
            if (menu == null)
            {

                var objectMenuArea = Engine.Screen.SafeArea;
                objectMenuArea.Width = HudLib.HeadDisplayWidth;

                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, XGuide.LocalHost);
                var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));


                mainMenu();
                designer.ShowHUD(true);
            }
        }

        public void Refresh(RichBoxContent content)
        {
            openMenu();
            menu.Refresh(content, controllerPointer);
        }

        public void mainMenu()
        {
           
            VoxelDesignerSettings sett = designer.Settings;

            RichBoxContent content = new RichBoxContent();

            content.h1("Main Menu", HudLib.TitleColor_Head);

            //GuiLayout layout = new GuiLayout("Main Menu", menu);
            //{
            //    if (PlatformSettings.DevBuild)
            //    {
            //        // new GuiTextButton("*Quick convert materials", null, quickConvertAll, false, layout);
            //    }

            //    var colorButton = new GuiIconTextButton(SpriteName.WhiteArea, "Color", null, selectColorMenu, true, layout);
            //    colorButton.icon.Color = designer.SelectedMaterial.color;

            //    //List<GuiOption<PaintToolType>> toolsOptList = new List<GuiOption<PaintToolType>>();

            //    //for (PaintToolType tool = 0; tool < PaintToolType.NUM; ++tool)
            //    //{
            //    //    toolsOptList.Add(new GuiOption<PaintToolType>(tool.ToString(), tool));
            //    //}
            //    //new GuiOptionsList<PaintToolType>("Tool", toolsOptList, toolProperty, layout);
            //    new GuiIconTextButton(VoxelDesignerInterface.ToolIcon(designer.Settings.DrawTool), "Tool", null, selectTool, true, layout);

            //    if (sett.DrawTool == PaintToolType.Pencil || sett.DrawTool == PaintToolType.Road || sett.DrawTool == PaintToolType.ReColor)
            //    {
            //        new GuiIntSlider(SpriteName.NO_IMAGE, "Pencil size", pencilSizeProperty, new IntervalF(1, 17), false, layout);
            //        new GuiFloatSlider(SpriteName.NO_IMAGE, "Size tolerance", radiusToleranceProperty, new IntervalF(-0.5f, 0.5f), false, layout);
            //        new GuiCheckbox("Round pencil", null, bRoundPencilProperty, layout);
            //        if (sett.DrawTool == PaintToolType.Road)
            //        {
            //            new GuiIntSlider(SpriteName.NO_IMAGE, "Edge size", RoadEdgeSizeProperty, new IntervalF(0, 5), false, layout);
            //            //new GuiIconTextButton(Data.BlockTextures.MaterialTile(secondaryMaterial), "Edge Material", 
            //            //    null, listSecondaryMaterialsLink, true, layout);
            //            new GuiIntSlider(SpriteName.NO_IMAGE, "Percent Fill", RoadPercentFillProperty, new IntervalF(1, 100), false, layout);

            //            new GuiIntSlider(SpriteName.NO_IMAGE, "Clear above", RoadUpwardClearProperty, new IntervalF(0, 32), false, layout);
            //            new GuiIntSlider(SpriteName.NO_IMAGE, "Fill below", RoadBelowFillProperty, new IntervalF(0, 32), false, layout);
            //        }
            //    }

            //    if (designer.copiedVoxels != null)
            //    {
            //        new GuiTextButton("Paste", null, designer.Paste, false, layout);
            //    }
            //    //if (PlatformSettings.DevBuild)
            //    //{ new GuiTextButton("New char adj", null, newCharacterSizeAdjust, false, layout); }

            //    if (designer.drawCoordMaterial.HasMaterial())
            //    {
            //        new GuiIconTextButton(SpriteName.IconColorPick, designer.drawCoordMaterial.ToString(),
            //            null, designer.linkPickMaterial, false, layout);
            //    }

            //    if (designer.inGame)
            //    {
            //        new GuiTextButton("Flatten area",
            //            "Will fill everything to the level of the cursor, with the selected material, and remove all above it",
            //            designer.linkFLattenArea, false, layout);

            //    }

            //    new GuiTextButton("Load", null, loadOptionsMenu, true, layout);
            //    if (designer.inGame)
            //    {
            //        new GuiCheckbox("Selection Cut", "Removes blocks you select with RT", bSelectionCutProperty, layout);//(int)ValueLink.SelectionCut);

            //        //if (PlatformSettings.ViewUnderConstructionStuff)

            //        //    file.AddIconTextLink(SpriteName.IconInfo, "Create door", (int)Link.CreateDoorInfo);
            //        //
            //        //new GuiTextButton("Letter blocks", "Add a row of blocks with letters on them", designer.linkTypeTextBlocks, false, layout);
            //        new GuiTextButton("Exit Creation", null, designer.LinkEXIT, false, layout);
            //    }
            //    else
            //    {


            //        new GuiTextButton("Save", null, SaveMenu, true, layout);

            //        new GuiTextButton("Select All", null, designer.selectAll, false, layout);
            //        new GuiTextButton("Canvas size", "Change the draw limits", LinkCanvasSize, true, layout);

            //        //if (PlatformSettings.RunningWindows)
            //        //new GuiTextButton("Expand Draw Limits", null, LinkExpandLimits, true, layout);

            //        new GuiTextButton("Rotate/Flip", "Rotate or flip the whole model", LinkRotateFlip, true, layout);
            //        new GuiTextButton("Move everything", null, MoveAllMenu, true, layout);

            //        new GuiTextButton("Animation", null, animationMenu, true, layout);


            //        new GuiTextButton("Clear all", "Removes all blocks and all frames", designer.LinkClearAll, false, layout);
            //        new GuiTextButton("Settings", null, pageSettings, true, layout);

            //        //file.AddDescription(LfLib.ViewBackText + " for help");

            //        //if (editTerrain == null)
            //        //{
            //        new GuiTextButton("Exit", "Exit to main menu", designer.LinkEXIT, false, layout);
            //        //}
            //        //else
            //        //{
            //        //    new GuiTextButton("Save & Exit", "Store model and return to game", exitTerrainEdit, false, layout);
            //        //}
            //    }

            //}
            //layout.End();
            Refresh(content);
        }

        public void colorPalette(GuiLayout layout, Action<BlockHD> link)
        {
            var inUse = designer.materialsInUse(true, out ushort selected);

            if (selected != 0)
            {
                ColorButton(BlockHD.ToColor(selected), layout, link, true);
                new GuiSectionSeparator(layout);
            }
            foreach (var m in inUse)
            {
                ColorButton(BlockHD.ToColor(m), layout, link);
            }

            new GuiSectionSeparator(layout);

            Color[] MainColors = new Color[]
                {
                    Color.White,
                    Color.LightGray,
                    Color.DarkGray,
                    new Color(100, 100, 100),
                    ColorExt.VeryDarkGray,
                    Color.Black,

                    Color.Red,
                    Color.Orange,
                    Color.Yellow,
                    Color.Green,
                    Color.Purple,
                    Color.Blue,
                };

            foreach (var m in MainColors)
            {
                ColorButton(VikingEngine.LootFest.Map.HDvoxel.BlockHD.FilterColor(m), layout, link);
            }

            new GuiSectionSeparator(layout);

            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                DSSSoldierPalette(layout, link);
                //#if RTS
                //                appearanceMaterials(DSSWars.ProfileData.SkinCol, "Skin", layout, link);
                //                appearanceMaterials(DSSWars.ProfileData.HairCol, "Hair", layout, link);
                //                appearanceMaterials(DSSWars.ProfileData.MainCol, "Main", layout, link);
                //                appearanceMaterials(DSSWars.ProfileData.AltMainCol, "Alt Main", layout, link);
                //                appearanceMaterials(DSSWars.ProfileData.DetailCol1, "Detail1", layout, link);
                //                appearanceMaterials(DSSWars.ProfileData.DetailCol2, "Detail2", layout, link);
                //#endif
            }
            else
            {
                appearanceMaterials(AppearanceMaterial.Material1, "1", layout, link);
                appearanceMaterials(AppearanceMaterial.Material2, "2", layout, link);
                appearanceMaterials(AppearanceMaterial.Material3, "3", layout, link);
                appearanceMaterials(AppearanceMaterial.Material4, "4", layout, link);
                appearanceMaterials(AppearanceMaterial.Material5, "5", layout, link);
            }
            new GuiSectionSeparator(layout);


            const int HueCount = 16;
            const int LightnessCount = 16;

            double[] Saturations = new double[] { 0.5, 0.25 };

            foreach (var saturate in Saturations)
            {
                for (int hue = 0; hue < HueCount; ++hue)
                {
                    for (int light = LightnessCount - 1; light >= 1; --light)
                    {
                        Color col = lib.HSL2RGB((double)hue / HueCount, saturate, (double)light / LightnessCount);
                        col = VikingEngine.LootFest.Map.HDvoxel.BlockHD.FilterColor(col);
                        ColorButton(col, layout, link);
                    }
                }
            }
        }


        static void ColorButton(Color col, GuiLayout layout, Action<BlockHD> link, bool bigIcon = false)
        {
            GuiIcon icon;

            if (bigIcon)
            {
                icon = new GuiIcon(SpriteName.WhiteArea, col.ToString(), new GuiAction1Arg<BlockHD>(link, new BlockHD(col)), false, layout);
            }
            else
            {
                icon = new GuiSmallIcon(SpriteName.WhiteArea, col.ToString(), new GuiAction1Arg<BlockHD>(link, new BlockHD(col)), false, layout);
            }
            icon.iconImage.Color = col;
        }

        override public void closeMenu() 
        {
            menu?.DeleteMe();
            menu = null;
        }
        override public bool InMenu { get; }

        /// <returns>Exit</returns>
        override public bool Update()
        {
            bool mouseOverHud = false;
            menu?.updateMouseInput(ref mouseOverHud);
            return menu == null;
        }

        
        override public void selectionMenu()
        { 
        
        }

        public void DeleteMe()
        {
            closeMenu();

        }
    }
}
