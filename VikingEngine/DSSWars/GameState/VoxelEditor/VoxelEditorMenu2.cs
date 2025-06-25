using Microsoft.CodeAnalysis.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VikingEngine.DataLib;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Data;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.LootFest;
using VikingEngine.LootFest.GO.PickUp;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.LootFest.Players;
using VikingEngine.PJ;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars.GameState.VoxelEditor
{
    class VoxelEditorMenu2 : AbsDesignMenuSystem_Base
    {
        const string Page_Canvas = "canvas";
        const string Page_Settings = "sett";
        const string Page_Color = "color";
        const string Page_material = "material";
        const string Page_Loading = "loading";
        const string Page_ListFiles = "listfiles";
        const string Page_Selection = "selection";
        const string Page_Recolor = "recolor";
        const string Page_RecolorTo = "recolor_to";
        const string Page_Layers = "layers";
        int listModels_0proj_1user_2retail;

        const float DefaultIconScale = 0.8f;
        RichMenuControllerPointer controllerPointer = null;
        VoxelDesigner designer;
        public RichMenu menu;
        FileIndex fileIndex = null;
        public VoxelEditorMenu2(VoxelDesigner designer)
        { 
            this.designer = designer;
        }

        override public void openMenu()
        {
            if (menu == null)
            {

                var objectMenuArea = Screen.SafeArea;
                objectMenuArea.Width = HudLib.HeadDisplayWidth;

                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, XGuide.LocalHost);
                var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));


                mainMenu();
                designer.ShowHUD(true);

                updateMouseVisible();
            }
        }

        public void Refresh(RichBoxContent content)
        {
            openMenu();
            menu.Refresh(content, controllerPointer);
        }

        void refreshPage()
        {
            switch (menu.menuStack.LastOrDefault())
            {
                default:
                    mainMenu();
                    break;
                case Page_Canvas:
                    LinkCanvasSize();
                    break;
                case Page_Color:
                    selectColorMenu();
                    break;
                case Page_material:
                    selectMaterialMenu();
                    break;
                case Page_Settings:
                    pageSettings();
                    break;
                case Page_ListFiles:
                    listFilesMenu();
                    break;
                case Page_Selection:
                    selectionOptionsMenu();
                    break;
                case Page_Recolor:
                    linkFindReplaceSelectionMaterials();
                    break;
                case Page_RecolorTo:
                    reColorToMenu();
                    break;

                case Page_Layers:
                    layersMenu();
                    break;
            }
        }



        public void mainMenu()
        {
            fileIndex = null;
            VoxelDesignerSettings sett = designer.Settings;

            RichBoxContent content = new RichBoxContent();

            HudLib.returnButton(content, menu, false, closeMenu);

            if (designer.HasSelection)
            {
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.NextArrow(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.IconBuildSelection),
                    new RbSpace(),
                    new RbText(DssRef.lang.Editor_SelectOptionsMenu) }),
                    new RbAction2Arg<string, StackOption>(menu.OpenMenu, Page_Selection, StackOption.Stack)));
            }

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary,
                HudLib.NextArrow(
                new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.VoxelEditorColorCube, 1, designer.SelectedMaterial.color),
                    new RbSpace(),
                    new RbText(DssRef.lang.Editor_Color)
                }), new RbAction2Arg<string, StackOption>(menu.OpenMenu, Page_Color, StackOption.Stack)));

            content.Add(new ArtButton(RbButtonStyle.Primary,
                HudLib.NextArrow(
                new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.VoxelEditorMaterialCube),
                    new RbSpace(),
                    new RbText(DssRef.lang.Editor_Material),
                    new RbSpace(),
                    new RbText(TextLib.Parentheses(designer.SelectedMaterial.material.ToString()), HudLib.TitleColor_TypeName_Dark)
                }), new RbAction2Arg<string, StackOption>(menu.OpenMenu, Page_material, StackOption.Stack)));

            content.newLine();
            HudLib.Label(content, DssRef.lang.Editor_Tool);
            content.newLine();
            for (PaintToolType tool = 0; tool < PaintToolType.NUM; ++tool)
            {
                content.Add(new ArtOption(tool == sett.paintSettings.drawTool,
                    new List<AbsRichBoxMember> { new RbImage(VoxelDesignerInterface.ToolIcon(tool)) },
                    new RbAction1Arg<PaintToolType>((toolType) => { designer.Settings.paintSettings.drawTool = toolType; }, tool)));
            }

            const float TabLength = 0.3f;

            switch (sett.paintSettings.drawTool)
            {
                case PaintToolType.Bucket:
                    allFramesChkBox(content);

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                        new RbText(DssRef.lang.Editor_Continous) }, bContiniousProperty));
                    break;

                case PaintToolType.Pencil:
                case PaintToolType.Road:
                case PaintToolType.ReColor:

                    content.newLine();
                    content.Add(new RbText(DssRef.lang.Editor_Tool_PencilSize + ":", HudLib.TitleColor_Label));
                    content.Add(new RbTab(TabLength));
                    RbDragButton.RbDragButtonGroup(content, new List<float> { 1 }, new DragButtonSettings(1, 17, 1), pencilSizeProperty);

                    content.newLine();
                    content.Add(new RbText(DssRef.lang.Editor_Tool_SizeTolerance + ":", HudLib.TitleColor_Label));
                    content.Add(new RbTab(TabLength));
                    RbDragButton.RbDragButtonGroup(content, new List<float> { 0.1f, 0.5f }, new DragButtonSettings(-0.5f, 0.5f, 0.1f), radiusToleranceProperty);

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Editor_Tool_RoundPencil) }, bRoundPencilProperty));

                    if (sett.paintSettings.drawTool == PaintToolType.Road)
                    {
                        content.newLine();
                        content.Add(new RbText(DssRef.lang.Editor_Tool_EdgeSize + ":", HudLib.TitleColor_Label));
                        content.Add(new RbTab(TabLength));
                        RbDragButton.RbDragButtonGroup(content, new List<float> { 1 }, new DragButtonSettings(0, 5, 1), RoadEdgeSizeProperty);

                        content.newLine();
                        content.Add(new RbText(DssRef.lang.Editor_Tool_PercentFill + ":", HudLib.TitleColor_Label));
                        content.Add(new RbTab(TabLength));
                        RbDragButton.RbDragButtonGroup(content, new List<float> { 1, 20 }, new DragButtonSettings(1, 100, 1), RoadPercentFillProperty);

                        content.newLine();
                        content.Add(new RbText(DssRef.lang.Editor_Tool_ClearAbove + ":", HudLib.TitleColor_Label));
                        content.Add(new RbTab(TabLength));
                        RbDragButton.RbDragButtonGroup(content, new List<float> { 1, 10 }, new DragButtonSettings(0, 32, 1), RoadUpwardClearProperty);

                        content.newLine();
                        content.Add(new RbText(DssRef.lang.Editor_Tool_FillBelow + ":", HudLib.TitleColor_Label));
                        content.Add(new RbTab(TabLength));
                        RbDragButton.RbDragButtonGroup(content, new List<float> { 1, 10 }, new DragButtonSettings(0, 32, 1), RoadBelowFillProperty);

                    }
                    break;

            }
            content.Add(new RbSeperationLine());
            content.newParagraph();
            if (designer.copiedVoxels != null)
            {
                content.newLine();
                content.Add(new ArtButton( RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Paste) }, new RbAction( designer.Paste)));
            }

            if (designer.drawCoordMaterial.HasMaterial())
            {
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { 
                    new RbImage(SpriteName.IconColorPick), new RbSpace(),
                    new RbImage(SpriteName.VoxelEditorColorCube, 1, designer.drawCoordMaterial.color), new RbSpace(), 
                    new RbText(designer.drawCoordMaterial.ToString()) }, new RbAction(designer.linkPickMaterial)));
            }
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary,
               HudLib.NextArrow(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconOpen, DefaultIconScale), new RbSpace(),
                    new RbText(DssRef.todoLang.Editor_Projects) }),
               new RbAction(beginListProjectFiles), new RbTooltip_Text(DssRef.lang.Editor_UserModels_Description)));
            content.Add(new ArtButton(RbButtonStyle.Primary, 
                HudLib.NextArrow( new List<AbsRichBoxMember> { 
                    new RbImage(SpriteName.WarsHudIconOpen, DefaultIconScale), new RbSpace(), 
                    new RbText(DssRef.lang.Editor_UserModels) }), 
                new RbAction1Arg<bool>(beginListModelsPage, true), new RbTooltip_Text(DssRef.lang.Editor_UserModels_Description)));
            content.Add(new ArtButton(RbButtonStyle.Primary,
                HudLib.NextArrow(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconOpen, DefaultIconScale), new RbSpace(),
                    new RbText(DssRef.lang.Editor_RetailModels) }),
                new RbAction1Arg<bool>(beginListModelsPage, false), new RbTooltip_Text(DssRef.lang.Editor_RetailModels_Description)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsBluePrint) }, new RbAction(/*loadOptionsMenu*/null), new RbTooltip_Text("Templates for modding"), false));


            content.newLine();
            var editButton = new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.InterfaceTextInput) },
                        new RbAction(beginEditName), null);
            content.Add(editButton);
            content.Add(new RbText(designer.storage.saveFileName, Color.LightYellow));
            content.space();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { 
                new RbImage(SpriteName.WarsHudIconSave, DefaultIconScale), new RbSpace(), 
                new RbText(DssRef.lang.Hud_Save) }, new RbAction(((Action)designer.storage.save) + closeMenu), 
               new RbTooltip_Text(LoadContent.CheckCharsSafety(designer.storage.SavePath().CompletePath(true), LoadedFont.Regular))));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { 
                new RbImage(SpriteName.WarsHudIconExport, DefaultIconScale), new RbSpace(), 
                new RbText("OBJ") }, new RbAction(((Action)designer.exportObjModel) + closeMenu), 
                new RbTooltip((content, tag)=> 
            {
                content.h2(DssRef.lang.Editor_ExportAsOBJ, HudLib.TitleColor_Head);
                content.Add(new RbText(LoadContent.CheckCharsSafety(designer.ExportPath().CompletePath(true), LoadedFont.Regular), HudLib.InfoYellow_Light));
            })));

            content.Add(new RbSeperationLine());

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Editor_SelectAll) }, new RbAction(designer.selectAll)));
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.NextArrow(new List<AbsRichBoxMember> { 
                new RbText(DssRef.lang.Editor_Canvas_Title),  new RbSpace(),  new RbText(designer.voxelProject.drawLimits.Size.ToString("*")),
            }), new RbAction2Arg<string, StackOption>(menu.OpenMenu, Page_Canvas, StackOption.Stack)));

            content.newParagraph();
            HudLib.Label(content, DssRef.lang.Editor_Animation);

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorFrameRemove) }, new RbAction(designer.RemoveCurrentFrame), new RbTooltip_Text(DssRef.lang.Editor_Animation_RemoveCurrentFrame), designer.voxelProject.HaveAnimation));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorFrameAddCopy) }, new RbAction1Arg<bool>(designer.AddFrame, true), new RbTooltip_Text(DssRef.lang.Editor_Animation_AddFrameCopy)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorFrameAddEmpty) }, new RbAction1Arg<bool>(designer.AddFrame, false), new RbTooltip_Text(DssRef.lang.Editor_Animation_AddEmptyFrame)));
            
            if (designer.voxelProject.HaveAnimation)
            {
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorFramePrevious) }, new RbAction1Arg<bool>(designer.nextFrame, false)));
                for (int frame = 0; frame <= designer.voxelProject.currentFrame.Max; frame++)
                {
                    SpriteName frameicon;
                    RbButtonStyle buttonStyle;
                    if (frame == designer.voxelProject.currentFrame.Value)
                    {
                        buttonStyle = RbButtonStyle.OptionSelected;
                        frameicon = SpriteName.VoxelEditorFrameSelected;
                    }
                    else
                    {
                        buttonStyle = RbButtonStyle.OptionNotSelected;
                        frameicon = SpriteName.VoxelEditorFrame;
                    }

                    content.Add(new ArtButton(buttonStyle,
                        new List<AbsRichBoxMember> { new RbText(TextLib.IndexToString(frame)), new RbImage(frameicon) }, new RbAction1Arg<int>(designer.setFrame, frame)));
                }
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorFrameNext) }, new RbAction1Arg<bool>(designer.nextFrame, true)));

                content.newLine();
                const float MoveFrameIconSz = 1.4f;

                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorMoveFrameToEndL, MoveFrameIconSz) }, new RbAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.ToStart), new RbTooltip_Text(DssRef.lang.Editor_Animation_MoveDescription)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorMoveFrameL, MoveFrameIconSz) }, new RbAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.Back)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorMoveFrameR, MoveFrameIconSz) }, new RbAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.Forward)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorMoveFrameToEndR, MoveFrameIconSz) }, new RbAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.ToEnd)));
            }

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.Editor_Layers_Titel, HudLib.TitleColor_Label) },
                 new RbAction2Arg<string, StackOption>(menu.OpenMenu, Page_Layers, StackOption.Stack)));

            content.newLine();
            for (int layerIx = 0; layerIx < designer.voxelProject.layers.list.Count; ++layerIx)
            {
                content.Add(new ArtToggle(layerIx == designer.voxelProject.layers.selectedIndex, new List<AbsRichBoxMember> {
                    new RbText(TextLib.IndexToString(layerIx)) },
                    new RbAction1Arg<int>(selectLayer, layerIx)));
            }

            content.Add(new RbSeperationLine());

            content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.NextArrow(new List<AbsRichBoxMember> {
                 new RbImage(SpriteName.WarsHudIconSettings, DefaultIconScale), new RbSpace(), new RbText( DssRef.lang.Editor_SettingsMenu) }),
                new RbAction2Arg<string, StackOption>(menu.OpenMenu, Page_Settings, StackOption.Stack)));

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                 new RbImage(SpriteName.WarsHudIconExit, DefaultIconScale), new RbSpace(), new RbText(DssRef.lang.Hud_Exit) },
                new RbAction(designer.LinkEXIT)));
           
            Refresh(content);
        }

        public void beginEditName()
        {
            new TextInput(designer.storage.saveFileName, NameEditEvent, null);
        }
        void NameEditEvent(string result, object tag)
        {
            designer.onFileNameChange(result, tag);
            menu.needRefresh = true;
        }

        public void LinkCanvasSize()
        {
            RichBoxContent content = new RichBoxContent();

            HudLib.returnButton(content, menu, true, closeMenu);

            content.h1(DssRef.lang.Editor_Canvas_Title, HudLib.TitleColor_Head);

            content.h2(DssRef.lang.Editor_Canvas_Size, HudLib.TitleColor_Label);
            content.text(designer.voxelProject.drawLimits.Size.ToString("*"));
            sizeOptions(DssRef.lang.Editor_Canvas_Dimension_X, SpriteName.width, IntVector3.PlusX);
            sizeOptions(DssRef.lang.Editor_Canvas_Dimension_Y, SpriteName.height, IntVector3.PlusY);
            sizeOptions(DssRef.lang.Editor_Canvas_Dimension_Z, SpriteName.length, IntVector3.PlusZ);

            void sizeOptions(string dimention, SpriteName dimIcon, IntVector3 plusOne)
            {
                content.newLine();
                button(-2);
                button(-1);
                content.Add(new RbImage(dimIcon));
                content.space();
                button(1);
                button(2);

                void button(int value)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                        new List<AbsRichBoxMember> { new RbText(TextLib.PlusMinus(value) + dimention) }, new RbAction1Arg<IntVector3>(designer.changeCanvasSize, plusOne * value)));
                }
            }

            content.newParagraph();

            IntVector3[] suggestedDrawLimits = new IntVector3[]
                {
                    new IntVector3(16, 24, 16),
                    new IntVector3(24),
                    new IntVector3(24, 32, 24),
                    new IntVector3(32),
                    new IntVector3(32, 48, 32),
                    new IntVector3(64, 32, 64),
                };
            DropDownBuilder sizePresets = new DropDownBuilder("sz preset");
            {
                foreach (IntVector3 lim in suggestedDrawLimits)
                {
                    sizePresets.AddOption(lim.ToString("*"), lim == designer.voxelProject.drawLimits.Size, false,
                        new RbAction1Arg<IntVector3>(designer.setCanvasSize, lim), null);
                }
            }
            sizePresets.Build(content, SpriteName.NO_IMAGE, DssRef.lang.Editor_Canvas_SizePresets, menu);

            content.newParagraph();
            content.h2(DssRef.lang.Editor_Canvas_Move, HudLib.TitleColor_Label);
            content.newLine();
            content.Add(new RbImage(SpriteName.Xdir));
            content.space();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+" + DssRef.lang.Editor_Canvas_Dimension_X) }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.PlusX, true)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("-" + DssRef.lang.Editor_Canvas_Dimension_X) }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.NegativeX, true)));
            content.space();
            //content.newLine();
            content.Add(new RbImage(SpriteName.Ydir));
            content.space();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Editor_Canvas_Move_Up) }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.PlusY, true)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Editor_Canvas_Move_Down) }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.NegativeY, true)));
            content.space();
            //content.newLine();
            content.Add(new RbImage(SpriteName.Zdir));
            content.space();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+" + DssRef.lang.Editor_Canvas_Dimension_Z) }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.PlusZ, true)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("-" + DssRef.lang.Editor_Canvas_Dimension_Z) }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.NegativeZ, true)));

            content.newParagraph();
            content.h2(DssRef.lang.Editor_Canvas_RotateFlip_Title, HudLib.TitleColor_Label);
            content.newLine();
            rotateFlipToHud(content);
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.RotateCW) }, new RbAction(designer.LinkSelRotateC), new RbTooltip_Text(DssRef.lang.Editor_Canvas_RotateClockwise)));
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.RotateCCW) }, new RbAction(designer.LinkSelRotateCC), new RbTooltip_Text(DssRef.lang.Editor_Canvas_RotateCounterClockwise)));
            //content.space(2);
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.FlipHori) }, new RbAction(designer.mirrorSelection), new RbTooltip_Text(DssRef.lang.Editor_Canvas_Mirror)));
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.FlipVerti)}, new RbAction(designer.LinkSelFlipY), new RbTooltip_Text(DssRef.lang.Editor_Canvas_FlipVertical)));
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorFlipLyingStanding) }, new RbAction(designer.flipLyingToStanding), new RbTooltip_Text(DssRef.lang.Editor_Canvas_FlipOrientation)));

            content.newParagraph();
            content.Add(new ArtButton( RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Editor_Canvas_Clear) }, new RbAction(designer.LinkClearAll), new RbTooltip_Text(DssRef.lang.Editor_Canvas_ClearAll_Description)));


            Refresh(content);
        }

        void linkFindReplaceSelectionMaterials()
        {
            RichBoxContent content = new RichBoxContent();

            HudLib.returnButton(content, menu, false, closeMenu);
            var used = designer.materialsInSelection();

            content.h1(DssRef.lang.Editor_Color_Recolor, HudLib.TitleColor_Head);
            
            content.newLine();
            HudLib.Label(content,DssRef.lang.Editor_ColorsInUseLabel);
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.VoxelEditorEmptyCube), new RbSpace(), new RbText(DssRef.lang.Editor_NoColor)},
                new RbAction1Arg<ushort>(designer.linkReplaceSelectionMaterials, BlockHD.EmptyBlock)));

            content.newLine();
            foreach (var m in used)
            {
                Color col = VikingEngine.LootFest.Map.HDvoxel.BlockHD.ToColor(m);

                content.Add(new ArtImageButton(new List<AbsRichBoxMember> { new RbImage(SpriteName.WhiteArea, 1, col) },
                    new RbAction1Arg<ushort>(designer.linkReplaceSelectionMaterials, m)));
            }

            content.newParagraph();
            allFramesChkBox(content);

            Refresh(content);
            //var layout = new GuiLayout(SpriteName.NO_IMAGE, "Model materials", menu, HUD.GuiLayoutMode.MultipleColumns);
            //{
            //    new GuiTextButton("Empty", null, new GuiAction1Arg<ushort>(designer.linkReplaceSelectionMaterials, VikingEngine.LootFest.Map.HDvoxel.BlockHD.EmptyBlock),
            //        true, layout);

            //    foreach (var m in used)
            //    {
            //        Color col = VikingEngine.LootFest.Map.HDvoxel.BlockHD.ToColor(m);

            //        var icon = new GuiIcon(SpriteName.WhiteArea, col.ToString(),
            //            new GuiAction1Arg<ushort>(designer.linkReplaceSelectionMaterials, m), false, layout);
            //        icon.iconImage.Color = col;
            //    }

            //    allFramesChkBox(layout);
            //}
            //layout.End();
        }

        public void layersMenu()
        {
            RichBoxContent content = new RichBoxContent();

            HudLib.returnButton(content, menu, true, closeMenu);
            content.h1(DssRef.todoLang.Editor_Layers_Titel, HudLib.TitleColor_Head);

            for (int layerIx = 0; layerIx < designer.voxelProject.layers.list.Count; ++layerIx)
            {
                content.newLine();
                var layer = designer.voxelProject.layers.list[layerIx];
                content.Add(new ArtToggle(layer.visible, new List<AbsRichBoxMember> { new RbImage(SpriteName.lineofsightEye) },
                     new RbAction1Arg<int>(toggleLayerVisible, layerIx), new RbTooltip_Text(DssRef.todoLang.Editor_ToggleVisible)));
                content.Add(new ArtOption(layerIx == designer.voxelProject.layers.selectedIndex, new List<AbsRichBoxMember> {
                    new RbText(string.Format( DssRef.todoLang.Editor_LayerNumber, TextLib.IndexToString(layerIx))) },
                    new RbAction1Arg<int>(selectLayer, layerIx)));
                
            }

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.pjNumPlus) },
                    new RbAction(layerMergeDown), new RbTooltip_Text(DssRef.todoLang.Editor_Layer_MergeDown)));
            content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsIncreaseArrowUp) },
                new RbAction1Arg<bool>(moveLayer, false),
                new RbTooltip_Text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Editor_Canvas_Move, DssRef.lang.Editor_Canvas_Move_Up))));
            content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsDecreaseArrowDown) },
                new RbAction1Arg<bool>(moveLayer, true),
                new RbTooltip_Text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Editor_Canvas_Move, DssRef.lang.Editor_Canvas_Move_Down))));

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.Editor_Layer_AddCopy) },
                new RbAction1Arg<bool>(addLayer, true)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.Editor_Layer_AddEmpty) },
                new RbAction1Arg<bool>(addLayer, false)));

            Refresh(content);
        }

        void toggleLayerVisible(int layer)
        {
            lib.Invert(ref designer.voxelProject.layers.list[layer].visible);
            designer.updateVoxelObj();
        }
        void selectLayer(int layer)
        { 
            designer.voxelProject.layers.selectedIndex = layer;
        }
        void moveLayer(bool down)
        {
            //designer.voxelProject.layers.selectedIndex = layer;
        }
        void layerMergeDown()
        {

        }

        void addLayer(bool copy)
        {
            designer.voxelProject.addLayer(false, copy);
        }
        
        public void openReColorTo()
        {
            menu.OpenMenu(Page_RecolorTo, StackOption.Stack);
        }

        public void reColorToMenu()
        {
            RichBoxContent content = new RichBoxContent();

            HudLib.returnButton(content, menu, false, closeMenu);
            var from = designer.swapMaterialFrom;

            content.Add(new RbBeginTitle());
            if (from.IsEmpty())
            {
                content.Add(new RbImage(SpriteName.VoxelEditorEmptyCube));
            }
            else
            {
                content.Add(new RbImage(SpriteName.VoxelEditorColorCube, 1, from.color));
            }
            content.space();
            content.Add(new RbText(DssRef.lang.Editor_Color_RecolorTo, HudLib.TitleColor_Head));

            if (from.HasMaterial())
            {
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.VoxelEditorEmptyCube), new RbSpace(), new RbText(DssRef.lang.Editor_NoColor)},
                    new RbAction1Arg<BlockHD>(designer.replaceSelectionMaterialsTo, BlockHD.Empty)));
            }

            content.newLine();
            colorPalette(content, designer.replaceSelectionMaterialsTo);

            Refresh(content);

            //RichBoxContent content = new RichBoxContent();
            //content.h1("Swap Material To", HudLib.TitleColor_Head);
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Empty") }, new RbAction1Arg<BlockHD>(replaceSelectionMaterialsTo, BlockHD.Empty)));



            //GuiLayout layout = new GuiLayout("Swap Material To", menusystem.menu, GuiLayoutMode.MultipleColumns, null);
            //{
            //    new GuiTextButton("Empty", null, new GuiAction1Arg<BlockHD>(replaceSelectionMaterialsTo, VikingEngine.LootFest.Map.HDvoxel.BlockHD.Empty),
            //        true, layout);
            //    menusystem.colorPalette(layout, replaceSelectionMaterialsTo);
            //}
            //layout.End();
        }

        void allFramesChkBox(RichBoxContent content)
        {
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                new RbImage(SpriteName.VoxelEditorAllFrames), new RbSpace(),
                new RbText(DssRef.lang.Editor_Animation_AllFrames) },
                designer.bRepeateOnAllFramesProperty, new RbTooltip_Text(DssRef.lang.Editor_Animation_AllFrames_ActionDescription)));
        }

        void rotateFlipToHud(RichBoxContent content)
        {
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.RotateCW) }, new RbAction(designer.LinkSelRotateC), new RbTooltip_Text(DssRef.lang.Editor_Canvas_RotateClockwise)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.RotateCCW) }, new RbAction(designer.LinkSelRotateCC), new RbTooltip_Text(DssRef.lang.Editor_Canvas_RotateCounterClockwise)));
            content.space(2);
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.FlipHori) }, new RbAction(designer.mirrorSelection), new RbTooltip_Text(DssRef.lang.Editor_Canvas_Mirror)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.FlipVerti) }, new RbAction(designer.LinkSelFlipY), new RbTooltip_Text(DssRef.lang.Editor_Canvas_FlipVertical)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoxelEditorFlipLyingStanding) }, new RbAction(designer.flipLyingToStanding), new RbTooltip_Text(DssRef.lang.Editor_Canvas_FlipOrientation)));
        }

        public override void selectionMenu()
        {
            openMenu();
            menu.OpenMenu(Page_Selection, StackOption.Stack);
        }

        void selectionOptionsMenu()
        {
            RichBoxContent content = new RichBoxContent();

            HudLib.returnButton(content, menu, true, closeMenu);

            content.h1(DssRef.lang.Editor_SelectOptionsMenu, HudLib.TitleColor_Head);

            content.newLine();
            Color current = BlockHD.FilterColor(designer.SelectedMaterial.color);
            content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.NextArrow(new List<AbsRichBoxMember> {
                new RbImage(SpriteName.VoxelEditorColorCube, 1f, current), new RbSpace(),
                new RbText(DssRef.lang.Editor_Color_Recolor)
            }), new RbAction2Arg<string, StackOption>(menu.OpenMenu, Page_Recolor, StackOption.Stack)));

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.NextArrow(new List<AbsRichBoxMember> {
                new RbImage(SpriteName.VoxelEditorMaterialCube), new RbSpace(),
                new RbText(DssRef.lang.Editor_Material_Set)
            }), new RbAction2Arg<string, StackOption>(menu.OpenMenu, Page_Recolor, StackOption.Stack), null, false));

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.WarsHudIconCut) }, new RbAction1Arg<bool>(designer.copySelectedVoxels, true),
                new RbTooltip_Text(DssRef.lang.Hud_Cut)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.WarsHudIconCopy) }, new RbAction1Arg<bool>(designer.copySelectedVoxels, false),
                new RbTooltip_Text(DssRef.lang.Hud_Copy)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.IconBuildStamp) }, new RbAction1Arg<bool>(designer.stampSelection, true),
                new RbTooltip_Text(DssRef.lang.Editor_Stamp)));
            content.space();
            rotateFlipToHud(content);
                        
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.IconBuildRemove), new RbText(DssRef.lang.Editor_Canvas_Clear) }, new RbAction(designer.clearSelectedArea)));

            if (designer.voxelProject.currentFrame.Length > 1)
            {
                content.newLine();
                HudLib.Label(content, DssRef.lang.Editor_StampOtherFrames);
                content.newLine();
                for (int frame = 0; frame <= designer.voxelProject.currentFrame.Max; frame++)
                {
                    SpriteName frameicon;
                    RbButtonStyle buttonStyle;
                    bool enabled = true;
                    if (frame == designer.voxelProject.currentFrame.Value)
                    {
                        buttonStyle = RbButtonStyle.OptionSelected;
                        frameicon = SpriteName.VoxelEditorFrameSelected;
                        enabled = false;
                    }
                    else
                    {
                        buttonStyle = RbButtonStyle.OptionNotSelected;
                        frameicon = SpriteName.VoxelEditorFrame;
                    }

                    content.Add(new ArtButton(buttonStyle,
                        new List<AbsRichBoxMember> { new RbText(TextLib.IndexToString(frame)), new RbImage(frameicon) }, new RbAction1Arg<int>(designer.LinkStampOnFrames, frame), new RbTooltip_Text(DssRef.lang.Editor_StampOtherFrames_Description), enabled));
                }


                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.IconBuildRemove), new RbText(DssRef.lang.Editor_ClearAllFrames) }, new RbAction1Arg<bool>(designer.ClearSelectedAreaOnFrames, true)));

                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.IconBuildRemove), new RbText(DssRef.lang.Editor_ClearOtherFrames) }, new RbAction1Arg<bool>(designer.ClearSelectedAreaOnFrames, false)));

            }

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.MissingImage), new RbText(DssRef.todoLang.Editor_CropSelection) }, new RbAction(designer.LinkSetLimitsAfterSel)));
            
            Refresh(content);
        }

        //void stampFramesOptions()
        //{
        //    GuiLayout layout = new GuiLayout("Stamp options", menu);
        //    {
        //        new GuiCheckbox("Include empty", null, designer.bStampEmptyProperty, layout);
        //        new GuiTextButton("All frames", null, new GuiAction1Arg<int>(designer.LinkStampOnFrames, -1), false, layout);
        //        for (int i = 0; i < designer.animationFrames.Frames.Count; i++)
        //        {
        //            if (i != designer.currentFrame.Value)
        //            {
        //                new GuiTextButton("Frame " + TextLib.IndexToString(i), null, new GuiAction1Arg<int>(designer.LinkStampOnFrames, i), false, layout);
        //            }
        //        }
        //    }
        //    layout.End();
        //}

        public void colorPalette(RichBoxContent content, Action<BlockHD> link)
        {

            var inUse = designer.materialsInUse(true, out ushort selected);

            //if (selected != 0)
            //{
            //    ColorButton(BlockHD.ToColor(selected), layout, link, true);
            //    new GuiSectionSeparator(layout);
            //}
            content.newLine();
            HudLib.Label(content, DssRef.lang.Editor_ColorsInUseLabel);
            content.newLine();
            foreach (var m in inUse)
            {
                ColorButton(BlockHD.ToColor(m), content, link);
            }

            content.Add(new RbSeperationLine());

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
                ColorButton(BlockHD.FilterColor(m), content, link);
            }

            //new GuiSectionSeparator(layout);

            DSSSoldierPalette(content, link);

            //new GuiSectionSeparator(layout);
            content.Add(new RbSeperationLine());

            const int HueCount = 16;
            const int LightnessCount = 16;

            double[] Saturations = new double[] { 1, 0.75, 0.5, 0.25 };

            foreach (var saturate in Saturations)
            {
                for (int hue = 0; hue < HueCount; ++hue)
                {
                    for (int light = LightnessCount - 1; light >= 1; --light)
                    {
                        Color col = lib.HSL2RGB((double)hue / HueCount, saturate, (double)light / LightnessCount);
                        col = BlockHD.FilterColor(col);
                        ColorButton(col, content, link);
                    }
                }
            }
        }

        public void DSSSoldierPalette(RichBoxContent content, Action<BlockHD> link)
        {
            
            content.newLine();
            HudLib.Label(content, DssRef.todoLang.ProfileEditor_ProfileColors_Label);
            content.newLine();
            //new GuiTitle("DSS soldier color mapping", layout);
            //SkinCol, HairCol, MainCol, AltMainCol, DetailCol1, DetailCol2;
            appearanceMaterials(FlagAndColor.SkinCol, DssRef.lang.ProfileEditor_SkinColor, content, link);
            appearanceMaterials(FlagAndColor.HairCol, DssRef.lang.ProfileEditor_HairColor, content, link);
            appearanceMaterials(FlagAndColor.MainCol, DssRef.lang.ProfileEditor_MainColor, content, link);
            appearanceMaterials(FlagAndColor.AltMainCol, DssRef.lang.ProfileEditor_AltMain, content, link);
            appearanceMaterials(FlagAndColor.DetailCol1, DssRef.lang.ProfileEditor_Detail1Color, content, link);
            appearanceMaterials(FlagAndColor.DetailCol2, DssRef.lang.ProfileEditor_Detail2Color, content, link);

            appearanceMaterials(FlagAndColor.TunicCol, DssRef.todoLang.ProfileEditor_TunicColor, content, link);
            appearanceMaterials(FlagAndColor.PantsCol, DssRef.todoLang.ProfileEditor_PantsColor, content, link);
            appearanceMaterials(FlagAndColor.LeaderCol, DssRef.todoLang.ProfileEditor_LeaderColor, content, link);
                
            content.newLine();
            appearanceMaterialsButton(true, BlockHD.JointUp, "Joint up", content, link);
            appearanceMaterialsButton(true, BlockHD.JointForward, "Joint forward", content, link);

        }
        void appearanceMaterials(AppearanceMaterial mat, string type, RichBoxContent content, Action<BlockHD> link)
        {
            string materialName = string.Format(DssRef.todoLang.ProfileEditor_ReplaceMaterial, type);
            appearanceMaterialsButton(true, mat.baseColor,  materialName, content, link);
            appearanceMaterialsButton(false, mat.brighter, materialName + ", " + DssRef.lang.Editor_Color_Brighter, content, link);
            appearanceMaterialsButton(false, mat.darker, materialName + ", " + DssRef.lang.Editor_Color_Darker, content, link);
            if (mat.redTint != BlockHD.EmptyBlockMaterial)
            {
                appearanceMaterialsButton(false, mat.redTint, materialName + ", " + DssRef.lang.Editor_Color_RedTint, content, link);
            }
        }


        void pageSettings()
        {
            RichBoxContent content = new RichBoxContent();
            HudLib.returnButton(content, menu, true, closeMenu);

            content.newLine();
            content.Add(new RbText(DssRef.lang.Editor_Settings_MoveSpeed + ":", HudLib.TitleColor_Label));
            content.Add(new RbSpace());
            RbDragButton.RbDragButtonGroup(content, new List<float> { 0.1f, 1 }, new DragButtonSettings(0.1f, 4f, 0.1f), designer.Settings.moveSpeedProperty);

            content.newLine();
            content.Add(new RbText(DssRef.lang.Editor_Settings_BackgroundColor + ":", HudLib.TitleColor_Label));
            content.Add(new RbSpace());
            List<Color> bgcolors = new List<Color>() { Color.White, Color.CornflowerBlue, Color.Black };
            foreach (Color color in bgcolors)
            {
                content.Add(new ArtOption(color == Ref.draw.ClrColor, new List<AbsRichBoxMember> { new RbImage(SpriteName.WhiteArea, 0.8f, color) },
                    new RbAction1Arg<Color>((col) => { Ref.draw.ClrColor = col; }, color)));
            }

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Editor_Settings_HideHUD) },
                new RbAction(designer.LinkHideHUD)));

            Refresh(content);
        }

        void selectColorMenu()
        {
            Color current = BlockHD.FilterColor(designer.SelectedMaterial.color);

            RichBoxContent content = new RichBoxContent();
            HudLib.returnButton(content, menu, true, closeMenu);

            content.Add(new RbBeginTitle(1));
            content.Add(new RbImage(SpriteName.VoxelEditorColorCube,1, current));
            content.space();
            content.Add(new RbText(DssRef.lang.Editor_Color, HudLib.TitleColor_Head));

            content.newLine();
            HudLib.Label(content, DssRef.lang.Editor_Color_Tint); content.newLine();
            
            Color brighter = ColorExt.ChangeColor(current, BlockHD.ColorStep, BlockHD.ColorStep, BlockHD.ColorStep);
            Color darker = ColorExt.ChangeColor(current, -BlockHD.ColorStep, -BlockHD.ColorStep, -BlockHD.ColorStep);
            Color brighter2 = ColorExt.ChangeColor(current, BlockHD.ColorStep * 2, BlockHD.ColorStep * 2, BlockHD.ColorStep * 2);
            Color darker2 = ColorExt.ChangeColor(current, -BlockHD.ColorStep * 2, -BlockHD.ColorStep * 2, -BlockHD.ColorStep * 2);
            Color redTint = ColorExt.ChangeColor(current, BlockHD.ColorStep, -BlockHD.ColorStep, -BlockHD.ColorStep);
            Color greenTint = ColorExt.ChangeColor(current, -BlockHD.ColorStep, BlockHD.ColorStep, -BlockHD.ColorStep);
            Color blueTint = ColorExt.ChangeColor(current, -BlockHD.ColorStep, -BlockHD.ColorStep, BlockHD.ColorStep);
            Color yellowTint = ColorExt.ChangeColor(current, BlockHD.ColorStep, BlockHD.ColorStep, -BlockHD.ColorStep);
            Color purpleTint = ColorExt.ChangeColor(current, BlockHD.ColorStep, -BlockHD.ColorStep, BlockHD.ColorStep);

            colorTintButton(brighter2, Color.White, false, SpriteName.MissingImage, DssRef.lang.Editor_Color_BrighterPlus, content, designer.pickColorLink);
            colorTintButton(brighter, Color.White, false, SpriteName.MissingImage, DssRef.lang.Editor_Color_Brighter, content, designer.pickColorLink);

            //colorTintButton(current, curr, true, SpriteName.MissingImage, "Current", content, designer.pickColorLink);

            colorTintButton(darker, Color.Black, false, SpriteName.MissingImage, DssRef.lang.Editor_Color_Darker, content, designer.pickColorLink);
            colorTintButton(darker2, Color.Black, false, SpriteName.MissingImage, DssRef.lang.Editor_Color_DarkerPlus, content, designer.pickColorLink);

            colorTintButton(redTint, Color.Red, false, SpriteName.MissingImage, DssRef.lang.Editor_Color_RedTint, content, designer.pickColorLink);
            colorTintButton(greenTint, Color.Green, false, SpriteName.MissingImage, DssRef.lang.Editor_Color_GreenTint, content, designer.pickColorLink);
            colorTintButton(blueTint, Color.Blue, false, SpriteName.MissingImage, DssRef.lang.Editor_Color_BlueTint, content, designer.pickColorLink);
            colorTintButton(yellowTint, Color.Yellow, false, SpriteName.MissingImage, DssRef.lang.Editor_Color_YellowTint, content, designer.pickColorLink);
            colorTintButton(purpleTint, Color.Purple, false, SpriteName.MissingImage, DssRef.lang.Editor_Color_PurpleTint, content, designer.pickColorLink);


            //new GuiTextButton("Pick Hue", null, designer.openColorPicker, false, layout);

            //new GuiIntSlider(SpriteName.NO_IMAGE, "R", redProperty, RGBrange, false, layout);
            //new GuiIntSlider(SpriteName.NO_IMAGE, "G", greenProperty, RGBrange, false, layout);
            //new GuiIntSlider(SpriteName.NO_IMAGE, "B", blueProperty, RGBrange, false, layout);
            //new GuiSectionSeparator(layout);

            colorPalette(content, designer.pickColorLink);

            Refresh(content);
        }

        void selectMaterialMenu()
        {
            //Color current = BlockHD.FilterColor(designer.SelectedMaterial.ma);

            RichBoxContent content = new RichBoxContent();
            HudLib.returnButton(content, menu, true, closeMenu);

            content.Add(new RbBeginTitle(1));
            content.Add(new RbImage(SpriteName.VoxelEditorMaterialCube));
            content.space();
            content.Add(new RbText(DssRef.lang.Editor_Material, HudLib.TitleColor_Head));

            for (MaterialProperty material = MaterialProperty.Default; material <= MaterialProperty.Layer_BelowAll; material++)
            {
                content.newLine();
                content.Add(new ArtOption(material == designer.SelectedMaterial.material,
                    new List<AbsRichBoxMember> { new RbText(material.ToString()) }, new RbAction1Arg<MaterialProperty>(designer.pickMaterialLink, material)));

            }

            Refresh(content);
        }

        void colorTintButton(Color col, Color tint, bool currentCol, SpriteName icon, string text, RichBoxContent content, Action<BlockHD> link)
        {
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>
            {
                 new RbImage(SpriteName.VoxelEditorTint, 1, tint),new RbImage(SpriteName.VoxelEditorColorCube, 1, col)
            }, new RbAction1Arg<BlockHD>(link, new BlockHD(col)), new RbTooltip_Text(text)));
        }

        static void ColorButton(Color col, RichBoxContent content, Action<BlockHD> link, bool bigIcon = false)
        {
            content.Add(new ArtImageButton(new List<AbsRichBoxMember> { new RbImage(SpriteName.WhiteArea, 1, col) },
                new RbAction1Arg<BlockHD>(link, new BlockHD(col)))
                { SpaceAfter = 0, });
            
        }

        void appearanceMaterialsButton(bool bigButton, ushort col, string name, RichBoxContent content, Action<BlockHD> link)
        {
            BlockHD color = new BlockHD(col);
            //GuiIcon icon;
            //icon = new GuiIcon(SpriteName.WhiteArea, name, new GuiAction1Arg<BlockHD>(link, color), false, layout);

            //icon.iconImage.Color = color.color;
            //if (!bigIcon)
            //{
            //    icon.iconImage.Size *= 0.7f;
            //}

            content.Add(new ArtImageButton(new List<AbsRichBoxMember> { new RbImage(SpriteName.WhiteArea, bigButton? 1 : 0.8f, color.color) },
               new RbAction1Arg<BlockHD>(link, color), new RbTooltip_Text(name))
            { SpaceAfter = 0, });
        }

        override public void closeMenu() 
        {
            fileIndex = null;
            menu?.DeleteMe();
            menu = null;

            updateMouseVisible();
        }

        void updateMouseVisible()
        {
            Input.Mouse.Visible = menu != null;
        }

        override public bool InMenu { get { return menu != null; } }

        /// <returns>Exit</returns>
        override public bool Update()
        {
            bool mouseOverHud = false;
            if (menu != null)
            {
                menu.updateMouseInput(ref mouseOverHud);
                if (menu != null && menu.needRefresh)
                {
                    refreshPage();
                    menu.needRefresh = false;
                }

                if (Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    closeMenu();
                }
            }
            return menu == null;
        }

        void beginListProjectFiles()
        {

            RichBoxContent content = new RichBoxContent();

            HudLib.returnButton(content, menu, true, closeMenu);
            content.Add(new RbText(DssRef.lang.Hud_Loading, HudLib.InfoYellow_Light));
            menu.OpenMenu(content, Page_Loading);

            //--
            new Timer.AsynchActionTrigger(() => {
                fileIndex = new FileIndex(DesignerStorage.VoxelProjectFolder,
                    "*" + VoxelLib.VoxelProjectEnding, true, designer.Settings.SortSettings)
                { 
                    projectType = true,
                };

                Ref.update.AddSyncAction(new SyncAction(() =>
                {
                    listModels_0proj_1user_2retail = 0;
                    listFilesMenu();
                }));

                //TopViewCamera modelView = null;

                FileIndex fileIndex_sp = fileIndex;
                var menu_sp = menu;
                if (fileIndex_sp != null &&
                    menu_sp != null)
                {
                    //Generate all icons
                    foreach (var file in fileIndex_sp.Files)
                    {
                        if (menu != null &&
                            (menu_sp.CurrentMenuState == Page_ListFiles || menu_sp.CurrentMenuState == Page_Loading)
                            )
                        {
                            FilePath path  = DesignerStorage.VoxelProjectPath(file.Name);
                            path.FileEnd = ".png";

                            if (path.Exists())
                            {
                                using (FileStream stream = new FileStream(path.CompleteLocalPath(false), FileMode.Open))
                                {
                                    var texture = Texture2D.FromStream(Draw.graphicsDeviceManager.GraphicsDevice, stream);
                                    file.Tag = texture;

                                    menu_sp.needRefresh = true;
                                }
                            }
                            //VoxelObjGridDataAnimHD animationFrames = new VoxelObjGridDataAnimHD();
                            //BeginReadWrite.BinaryIO(false, path, null, animationFrames.ReadBinaryStream, null, false);
                            //if (animationFrames.Frames != null)
                            //{
                            //    VoxelModel voxelObj = VoxelObjBuilder.BuildModelHD(animationFrames.Frames, Vector3.Zero);
                            //    Vector3 modelGridSz = animationFrames.Frames[0].Size.Vec;

                            //    new Timer.Action0ArgTrigger(renderModel);


                            //    void renderModel()
                            //    {
                            //        const int Size = 32;

                            //        RenderTargetImage target = new RenderTargetImage(Vector2.Zero, new Vector2(Size), ImageLayers.Foreground4, false);
                            //        if (modelView == null)
                            //        {
                            //            modelView = new TopViewCamera(22, new Vector2(MathHelper.PiOver2 - 0.8f, MathHelper.PiOver4 + 0.12f),
                            //                Size, Size);
                            //        }
                            //        modelView.LookTarget = modelGridSz * 0.5f;
                            //        modelView.Time_Update(0);
                            //        modelView.RecalculateMatrices();

                            //        target.Camera = modelView;
                            //        target.DrawImagesToTarget(null, new List<AbsDraw> { voxelObj }, true, 0);

                            //        file.Tag = target.renderTarget;

                            //        menu_sp.needRefresh = true;
                            //    }
                            //}
                        }
                        else
                        {
                            return;
                        }
                    }
                }

            }, true);
        }

        void beginListModelsPage(bool userModels)
        {
            RichBoxContent content = new RichBoxContent();

            HudLib.returnButton(content, menu, true, closeMenu);
            content.Add(new RbText(DssRef.lang.Hud_Loading, HudLib.InfoYellow_Light));
            menu.OpenMenu(content, Page_Loading);

            //--
            new Timer.AsynchActionTrigger(()=> {
                fileIndex = new FileIndex(userModels? DesignerStorage.VoxelModelFolder : LfLib.ModelsCategoryWars, 
                    VoxelDesigner.searchPattern(false), userModels, designer.Settings.SortSettings);
                
                Ref.update.AddSyncAction(new SyncAction1Arg<bool>((userModels) =>
                {
                    listModels_0proj_1user_2retail = userModels? 1 : 2;
                    listFilesMenu();
                }, userModels));

                TopViewCamera modelView = null;

                FileIndex fileIndex_sp = fileIndex;
                var menu_sp = menu;
                if (fileIndex_sp != null &&
                 menu_sp != null)
                {
                    //Generate all icons
                    foreach (var file in fileIndex_sp.Files)
                    {
                        if (menu != null &&
                            (menu_sp.CurrentMenuState == Page_ListFiles || menu_sp.CurrentMenuState == Page_Loading)
                            )
                        {
                            FilePath path;
                            if (userModels)
                            {
                                path = DesignerStorage.CustomVoxelObjPath(file.Name);
                            }
                            else
                            {
                                path = DesignerStorage.InGameVoxelObjPath(file.Name);
                            }
                            VoxelObjGridDataAnimHD animationFrames = new VoxelObjGridDataAnimHD();
                            BeginReadWrite.BinaryIO(false, path, null, animationFrames.ReadBinaryStream, null, false);
                            if (animationFrames.Frames != null)
                            {
                                VoxelModel voxelObj = VoxelObjBuilder.BuildModelHD(animationFrames.Frames, Vector3.Zero);
                                Vector3 modelGridSz = animationFrames.Frames[0].Size.Vec;

                                new Timer.Action0ArgTrigger(renderModel);


                                void renderModel()
                                {
                                    const int Size = 32;

                                    RenderTargetImage target = new RenderTargetImage(Vector2.Zero, new Vector2(Size), ImageLayers.Foreground4, false);
                                    if (modelView == null)
                                    {
                                        modelView = new TopViewCamera(22, new Vector2(MathHelper.PiOver2 - 0.8f, MathHelper.PiOver4 + 0.12f),
                                            Size, Size);
                                    }
                                    modelView.LookTarget = modelGridSz * 0.5f;
                                    modelView.Time_Update(0);
                                    modelView.RecalculateMatrices();

                                    target.Camera = modelView;
                                    target.DrawImagesToTarget(null, new List<AbsDraw> { voxelObj }, true, 0);

                                    file.Tag = target.renderTarget;

                                    menu_sp.needRefresh = true;
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }

            }, true);
            //new Process.AsynchMenuPage<List<string>, int>(asynchListUserModelsPage, int.MinValue, endListUserModelsPage, menu);
        }

        void listFilesMenu()
        {
            RichBoxContent content = new RichBoxContent();

            HudLib.returnButton(content, menu, true, closeMenu);

            content.Add(new RichBoxScale(1.2f));

            foreach (var file in fileIndex.Files)
            {
                content.newLine();

                AbsRichBoxMember previewImage;

                if (file.Tag == null)
                {
                    previewImage = new RbImage(SpriteName.IconSandGlass);
                }
                else
                { 
                    previewImage = new RbTexture((Texture2D)file.Tag);
                }

                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            previewImage,
                            new RbSpace(),
                            new RbText(file.Name)
                        }, new RbAction4Arg<string, int, bool, bool>(loadUserModelLink, file.Name, listModels_0proj_1user_2retail, false, false),
                new RbTooltip_Text(LoadContent.CheckCharsSafety(file.Date.ToString(), LoadedFont.Regular))));

                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                            new RbImage(SpriteName.cmdSpyglass),
                        }, new RbAction4Arg<string, int, bool, bool>(loadUserModelLink, file.Name, listModels_0proj_1user_2retail, true, false),
                new RbTooltip_Text(DssRef.lang.Editor_Preview)));

                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                            new RbImage(SpriteName.cmdPlus),
                        }, new RbAction4Arg<string, int, bool, bool>(loadUserModelLink, file.Name, listModels_0proj_1user_2retail, false, true),
                new RbTooltip_Text(DssRef.lang.Editor_CombineWithCurrent), false));
            }

            menu.menuStack.Clear();
            menu.OpenMenu(content, Page_ListFiles);
        }

        //List<string> asynchListUserModelsPage(int none)
        //{
        //    return DataLib.SaveLoad.FilesInStorageDir(DesignerStorage.UserVoxelObjFolder, VoxelDesigner.searchPattern(false));
        //}

        //void endListUserModelsPage(List<string> files2, int none)
        //{
        //    var layout = new GuiLayout(SpriteName.NO_IMAGE, "User models", menu, HUD.GuiLayoutMode.MultipleColumns);
        //    {
        //        if (files2.Count == 0)
        //        {
        //            new GuiIconTextButton(menu.style.headReturnIcon, "No models", null, menu.PopLayout, false, layout);
        //        }

        //        for (int i = 0; i < files2.Count; i++)
        //        {
        //            if (menuModelIcons)
        //            {
        //                new GuiVoxelModelIcon(SpriteName.IconSandGlass, DesignerStorage.CustomVoxelObjPath(files2[i]),
        //                    "Load \"" + files2[i] + "\"", new GuiAction1Arg<string>(loadUserModelLink, files2[i]), layout);
        //            }
        //            else
        //            {
        //                new GuiTextButton(files2[i], null, new GuiAction1Arg<string>(loadUserModelLink, files2[i]), false, layout);
        //            }
        //        }


        //    }
        //    layout.End();
        //}

        void loadUserModelLink(string name, int type_0proj_1user_2retail, bool preview, bool combine)
        {
            //designer.loadOption_fromStorage = userModel;
            designer.loadOption_combine = combine;
            designer.loadOption_preview = preview;

            switch (type_0proj_1user_2retail)
            {
                case 0:
                    {
                        designer.storage.loadProject(name);
                    }
                    break;
                case 1:
                    {
                        designer.storage.loadUserModel(name);
                    }
                    break;
                case 2:
                    {
                        designer.storage.loadRetailModel(name);
                    }
                    break;
            }
        }


        int redProperty(bool set, int value)
        {
            return colorProperty(set, Dimensions.X, value);
        }
        int greenProperty(bool set, int value)
        {
            return colorProperty(set, Dimensions.Y, value);
        }
        int blueProperty(bool set, int value)
        {
            return colorProperty(set, Dimensions.Z, value);
        }

        int colorProperty(bool set, Dimensions dim, int value)
        {
            if (set)
            {
                designer.SelectedMaterial.SetColor(dim, (byte)value);
            }
            return designer.SelectedMaterial.GetColor(dim);
        }

        int pencilSizeProperty(bool set, int value)
        {
            if (set) { designer.Settings.paintSettings.pencilSize = value; }
            return designer.Settings.paintSettings.pencilSize;
        }

        float radiusToleranceProperty(bool set, float value)
        {
            if (set) { designer.Settings.paintSettings.radiusTolerance = value; }
            return designer.Settings.paintSettings.radiusTolerance;
        }

        int RoadUpwardClearProperty(bool set, int value)
        {
            if (set) { designer.Settings.paintSettings.roadUpwardClear = value; }
            return designer.Settings.paintSettings.roadUpwardClear;
        }
        int RoadBelowFillProperty(bool set, int value)
        {
            if (set) { designer.Settings.paintSettings.roadBelowFill = value; }
            return designer.Settings.paintSettings.roadBelowFill;
        }
        int RoadEdgeSizeProperty(bool set, int value)
        {
            if (set) { designer.Settings.paintSettings.roadEdgeSize = value; }
            return designer.Settings.paintSettings.roadEdgeSize;
        }
        int RoadPercentFillProperty(bool set, int value)
        {
            if (set) { designer.Settings.paintSettings.roadPercentFill = value; }
            return designer.Settings.paintSettings.roadPercentFill;
        }

        bool bSelectionCutProperty(int index, bool set, bool value)
        {
            if (set) { designer.Settings.SelectionCut = value; }
            return designer.Settings.SelectionCut;
        }
        bool bRoundPencilProperty(int index, bool set, bool value)
        {
            if (set) { designer.Settings.paintSettings.roundPencil = value; }
            return designer.Settings.paintSettings.roundPencil;
        }
        bool bCombineLoadedModelProperty(int index, bool set, bool value)
        {
            if (set) { designer.loadOption_combine = value; }
            return designer.loadOption_combine;
        }
        bool bMergeKeepSizeProperty(int index, bool set, bool value)
        {
            if (set) { designer.mergeModelsOption.KeepOldGridSize = value; }
            return designer.mergeModelsOption.KeepOldGridSize;
        }
        bool bMergeNewOverrideProperty(int index, bool set, bool value)
        {
            if (set) { designer.mergeModelsOption.NewBlocksReplaceOld = value; }
            return designer.mergeModelsOption.NewBlocksReplaceOld;
        }

        bool bContiniousProperty(int index, bool set, bool value)
        {
            if (set) { designer.Settings.paintSettings.continiousFill = value; }
            return designer.Settings.paintSettings.continiousFill;
        }


        public void DeleteMe()
        {
            closeMenu();

        }
    }
}
