using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using VikingEngine.LootFest.Editor;
using VikingEngine.LootFest.GO.PickUp;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.LootFest.Players;
using VikingEngine.PJ;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars.VoxelEditor
{
    class VoxelEditorMenu2 : AbsDesignMenuSystem_Base
    {
        const string Page_Canvas = "canvas";
        const string Page_Settings = "sett";
        const string Page_Loading = "loading";
        const string Page_ListFiles = "listfiles";
        bool listUserModels;

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
                case Page_Settings:
                    pageSettings();
                    break;
                case Page_ListFiles:
                    listFilesMenu();
                    break;
            }
        }

        public void mainMenu()
        {
            fileIndex = null;
            VoxelDesignerSettings sett = designer.Settings;

            RichBoxContent content = new RichBoxContent();

            //content.h1("Main Menu", HudLib.TitleColor_Head);
            
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary,
                HudLib.NextArrow(
                new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WhiteArea, 1, 0, 0, designer.SelectedMaterial.color),
                    new RbSpace(),
                    new RbText( "Color")
                })
                , new RbAction(/*selectColorMenu*/null)));

            content.newLine();
            HudLib.Label(content, "Tool");
            content.newLine();
            for (PaintToolType tool = 0; tool < PaintToolType.NUM; ++tool)
            {
                content.Add(new ArtOption(tool == sett.DrawTool,
                    new List<AbsRichBoxMember> { new RbImage(VoxelDesignerInterface.ToolIcon(tool)) },
                    new RbAction1Arg<PaintToolType>((PaintToolType toolType) => { designer.Settings.DrawTool = toolType; }, tool)));
            }

            const float TabLength = 0.3f;

            switch (sett.DrawTool)
            {
                case PaintToolType.Pencil:
                case PaintToolType.Road:
                case PaintToolType.ReColor:

                    content.newLine();
                    content.Add(new RbText("Pencil size" + ":", HudLib.TitleColor_Label));
                    content.Add(new RbTab(TabLength));
                    RbDragButton.RbDragButtonGroup(content, new List<float> { 1 }, new DragButtonSettings(1, 17, 1), pencilSizeProperty);

                    content.newLine();
                    content.Add(new RbText("Size tolerance" + ":", HudLib.TitleColor_Label));
                    content.Add(new RbTab(TabLength));
                    RbDragButton.RbDragButtonGroup(content, new List<float> { 0.1f, 0.5f }, new DragButtonSettings(-0.5f, 0.5f, 0.1f), radiusToleranceProperty);

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Round pencil") }, bRoundPencilProperty));

                    if (sett.DrawTool == PaintToolType.Road)
                    {
                        content.newLine();
                        content.Add(new RbText("Edge size" + ":", HudLib.TitleColor_Label));
                        content.Add(new RbTab(TabLength));
                        RbDragButton.RbDragButtonGroup(content, new List<float> { 1 }, new DragButtonSettings(0, 5, 1), RoadEdgeSizeProperty);

                        content.newLine();
                        content.Add(new RbText("Percent Fill" + ":", HudLib.TitleColor_Label));
                        content.Add(new RbTab(TabLength));
                        RbDragButton.RbDragButtonGroup(content, new List<float> { 1, 20 }, new DragButtonSettings(1, 100, 1), RoadPercentFillProperty);

                        content.newLine();
                        content.Add(new RbText("Clear above" + ":", HudLib.TitleColor_Label));
                        content.Add(new RbTab(TabLength));
                        RbDragButton.RbDragButtonGroup(content, new List<float> { 1, 10 }, new DragButtonSettings(0, 32, 1), RoadUpwardClearProperty);

                        content.newLine();
                        content.Add(new RbText("Fill below" + ":", HudLib.TitleColor_Label));
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
                content.Add(new ArtButton( RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Paste") }, new RbAction( designer.Paste)));
            }

            if (designer.drawCoordMaterial.HasMaterial())
            {
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { 
                    new RbImage(SpriteName.IconColorPick), new RbSpace(), new RbText(designer.drawCoordMaterial.ToString()) }, new RbAction(designer.linkPickMaterial)));
            }
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, 
                HudLib.NextArrow( new List<AbsRichBoxMember> { 
                    new RbImage(SpriteName.WarsHudIconOpen, DefaultIconScale), new RbSpace(), 
                    new RbText("User models") }), 
                new RbAction1Arg<bool>(beginListModelsPage, true), new RbTooltip_Text("Browse models you saved")));
            content.Add(new ArtButton(RbButtonStyle.Primary,
                HudLib.NextArrow(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconOpen, DefaultIconScale), new RbSpace(),
                    new RbText("Retail models") }),
                new RbAction1Arg<bool>(beginListModelsPage, false), new RbTooltip_Text("Load models from the game")));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("T") }, new RbAction(/*loadOptionsMenu*/null), new RbTooltip_Text("Templates for modding"), false));


            content.newLine();
            var editButton = new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.InterfaceTextInput) },
                        new RbAction(beginEditName), null);
            content.Add(editButton);
            content.Add(new RbText(designer.storage.saveFileName, Color.LightYellow));
            content.space();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { 
                new RbImage(SpriteName.LFSaveIcon, DefaultIconScale), new RbSpace(), 
                new RbText("Save") }, new RbAction(designer.storage.save), 
               new RbTooltip_Text(LoadContent.CheckCharsSafety(designer.storage.SavePath().CompletePath(true), LoadedFont.Regular))));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { 
                new RbImage(SpriteName.WarsHudIconExport, DefaultIconScale), new RbSpace(), 
                new RbText("OBJ") }, new RbAction(designer.exportObjModel), 
                new RbTooltip((RichBoxContent content, object tag)=> 
            {
                content.h2("Export as .OBJ", HudLib.TitleColor_Head);
                content.Add(new RbText(LoadContent.CheckCharsSafety(designer.ExportPath().CompletePath(true), LoadedFont.Regular), HudLib.InfoYellow_Light));
            })));

            content.Add(new RbSeperationLine());

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Select All") }, new RbAction(designer.selectAll)));
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.NextArrow(new List<AbsRichBoxMember> { new RbText("Canvas") }), new RbAction2Arg<string, StackOption>(menu.OpenMenu, Page_Canvas, StackOption.Stack)));

            content.newParagraph();
            HudLib.Label(content, "Animation");

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("-"), new RbImage(SpriteName.MissingImage) }, new RbAction(designer.RemoveCurrentFrame), new RbTooltip_Text("Remove current frame"), designer.haveAnimation));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+C"), new RbImage(SpriteName.MissingImage) }, new RbAction1Arg<bool>(designer.AddFrame, true), new RbTooltip_Text("Add frame as copy")));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+E"), new RbImage(SpriteName.MissingImage) }, new RbAction1Arg<bool>(designer.AddFrame, false), new RbTooltip_Text("Add empty frame")));
            
            if (designer.haveAnimation)
            {
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("<") }, new RbAction1Arg<bool>(designer.nextFrame, false)));
                for (int frame = 0; frame <= designer.currentFrame.Max; frame++)
                {
                    content.Add(new ArtButton(frame == designer.currentFrame.Value ? RbButtonStyle.OptionSelected : RbButtonStyle.OptionNotSelected,
                        new List<AbsRichBoxMember> { new RbText(TextLib.IndexToString(frame)) }, new RbAction1Arg<int>(designer.setFrame, frame)));
                }
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(">") }, new RbAction1Arg<bool>(designer.nextFrame, true)));

                content.newLine();
                //HudLib.Label(content, "Move current frame");
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("|<<") }, new RbAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.ToStart)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("|<") }, new RbAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.Back)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(">|") }, new RbAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.Forward)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(">>|") }, new RbAction1Arg<MoveFrameType>(designer.moveFrame, MoveFrameType.ToEnd)));
            }

            content.Add(new RbSeperationLine());


            content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.NextArrow(new List<AbsRichBoxMember> {
                 new RbImage(SpriteName.WarsHudIconSettings, DefaultIconScale), new RbSpace(), new RbText("Settings") }),
                new RbAction2Arg<string, StackOption>(menu.OpenMenu, Page_Settings, StackOption.Stack)));

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                 new RbImage(SpriteName.WarsHudIconExit, DefaultIconScale), new RbSpace(), new RbText("Exit") },
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

            HudLib.returnButton(content, menu);

            content.h1("Canvas", HudLib.TitleColor_Head);

            content.h2("Size", HudLib.TitleColor_Label);
            sizeOptions("X", IntVector3.PlusX);
            sizeOptions("Y", IntVector3.PlusY);
            sizeOptions("Z", IntVector3.PlusZ);

            void sizeOptions(string dimention, IntVector3 plusOne)
            {
                content.newLine();
                button(-2);
                button(-1);
                content.space(2);
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
                    sizePresets.AddOption(lim.ToString("*"), lim == designer.drawLimits.Size, false,
                        new RbAction1Arg<IntVector3>(designer.setCanvasSize, lim), null);
                }
            }
            sizePresets.Build(content, SpriteName.NO_IMAGE, "Size presets", menu);

            content.newParagraph();
            content.h2("Move", HudLib.TitleColor_Label);
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+" + "X") }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.PlusX, true)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("-" + "X") }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.NegativeX, true)));
            content.space(2);
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Up") }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.PlusY, true)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Down") }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.NegativeY, true)));
            content.space(2);
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+" + "Z") }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.PlusZ, true)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("-" + "Z") }, new RbAction2Arg<IntVector3, bool>(designer.moveAll, IntVector3.NegativeZ, true)));

            content.newParagraph();
            content.h2("Rotate/Flip", HudLib.TitleColor_Label);
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.RotateCW), new RbText("C") }, new RbAction(designer.LinkSelRotateC), new RbTooltip_Text("Rotate clockwise")));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("CC") }, new RbAction(designer.LinkSelRotateCC), new RbTooltip_Text("Rotate counter clockwise")));
            content.space(2);
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.FlipHori) }, new RbAction(designer.mirrorSelection), new RbTooltip_Text("Mirror")));
            content.space(2);
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.FlipVerti)}, new RbAction(designer.LinkSelFlipY), new RbTooltip_Text("Flip up and down")));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.MissingImage) }, new RbAction(designer.flipLyingToStanding), new RbTooltip_Text("Flip lying/standing")));
        
            content.newParagraph();
            content.Add(new ArtButton( RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Clear") }, new RbAction(designer.LinkClearAll), new RbTooltip_Text( "Removes all blocks and all frames")));


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

            //DSSSoldierPalette(layout, link);
               
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
        void pageSettings()
        {
            RichBoxContent content = new RichBoxContent();
            HudLib.returnButton(content, menu);

            content.newLine();
            content.Add(new RbText("Move speed" + ":", HudLib.TitleColor_Label));
            content.Add(new RbSpace());
            RbDragButton.RbDragButtonGroup(content, new List<float> { 0.1f, 1 }, new DragButtonSettings(0.1f, 4f, 0.1f), designer.Settings.moveSpeedProperty);

            content.newLine();
            content.Add(new RbText("Background color" + ":", HudLib.TitleColor_Label));
            content.Add(new RbSpace());
            List<Color> bgcolors = new List<Color>() { Color.White, Color.CornflowerBlue, Color.Black };
            foreach (Color color in bgcolors)
            {
                content.Add(new ArtOption(color == Ref.draw.ClrColor, new List<AbsRichBoxMember> { new RbImage(SpriteName.WhiteArea, 0.8f, 0, 0, color) },
                    new RbAction1Arg<Color>((Color col) => { Ref.draw.ClrColor = col; }, color)));
            }

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Hide HUD") },
                new RbAction(designer.LinkHideHUD)));

            Refresh(content);
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
            fileIndex = null;
            menu?.DeleteMe();
            menu = null;
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

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    closeMenu();
                }
            }
            return menu == null;
        }

        void beginListModelsPage(bool userModels)
        {
            RichBoxContent content = new RichBoxContent();

            HudLib.returnButton(content, menu);
            content.Add(new RbText(DssRef.lang.Hud_Loading, HudLib.InfoYellow_Light));
            menu.OpenMenu(content, Page_Loading);

            //--
            new Timer.AsynchActionTrigger(()=> {
                fileIndex = new FileIndex(userModels? DesignerStorage.UserVoxelObjFolder : LfLib.ModelsCategoryWars, 
                    VoxelDesigner.searchPattern(false), userModels, designer.Settings.SortSettings);
                
                Ref.update.AddSyncAction(new SyncAction1Arg<bool>((bool userModels) =>
                {
                    listUserModels = userModels;
                    listFilesMenu();
                }, userModels));

                Graphics.TopViewCamera modelView = null;

                FileIndex fileIndex_sp = fileIndex;
                if (fileIndex_sp != null)
                {
                    //Generate all icons
                    foreach (var file in fileIndex_sp.Files)
                    {
                        if (menu.CurrentMenuState == Page_ListFiles ||
                            menu.CurrentMenuState == Page_Loading)
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
                            DataStream.BeginReadWrite.BinaryIO(false, path, null, animationFrames.ReadBinaryStream, null, false);
                            if (animationFrames.Frames != null)
                            {
                                Graphics.VoxelModel voxelObj = LootFest.Editor.VoxelObjBuilder.BuildModelHD(animationFrames.Frames, Vector3.Zero);
                                Vector3 modelGridSz = animationFrames.Frames[0].Size.Vec;

                                new Timer.Action0ArgTrigger(renderModel);


                                void renderModel()
                                {
                                    const int Size = 32;

                                    RenderTargetImage target = new RenderTargetImage(Vector2.Zero, new Vector2(Size), ImageLayers.Foreground4, false);
                                    if (modelView == null)
                                    {
                                        modelView = new Graphics.TopViewCamera(22, new Vector2(MathHelper.PiOver2 - 0.8f, MathHelper.PiOver4 + 0.12f),
                                            Size, Size);
                                    }
                                    modelView.LookTarget = modelGridSz * 0.5f;
                                    modelView.Time_Update(0);
                                    modelView.RecalculateMatrices();

                                    target.Camera = modelView;
                                    target.DrawImagesToTarget(null, new List<AbsDraw> { voxelObj }, true, 0);

                                    file.Tag = target.renderTarget;

                                    menu.needRefresh = true;
                                    //iconImg.Texture = target.renderTarget;
                                    //iconImg.SetFullTextureSource();

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

            HudLib.returnButton(content, menu);

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
                    previewImage = new RbTexture((RenderTarget2D)file.Tag);
                }

                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            previewImage,
                            new RbSpace(),
                            new RbText(file.Name)
                        }, new RbAction4Arg<string, bool, bool, bool>(loadUserModelLink, file.Name, listUserModels, false, false),
                new RbTooltip_Text(LoadContent.CheckCharsSafety(file.Date.ToString(), LoadedFont.Regular))));

                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                            new RbImage(SpriteName.cmdSpyglass),
                        }, new RbAction4Arg<string, bool, bool, bool>(loadUserModelLink, file.Name, listUserModels, true, false),
                new RbTooltip_Text("Preview")));

                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                            new RbImage(SpriteName.cmdPlus),
                        }, new RbAction4Arg<string, bool, bool, bool>(loadUserModelLink, file.Name, listUserModels, false, true),
                new RbTooltip_Text("Combine with current model"), false));
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

        void loadUserModelLink(string name, bool userModel, bool preview, bool combine)
        {
            //designer.loadOption_fromStorage = userModel;
            designer.loadOption_combine = combine;
            designer.loadOption_preview = preview;

            if (userModel)
            {
                designer.storage.loadUserModel(name);
            }
            else
            {
                designer.storage.loadRetailModel(name);
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
            if (set) { designer.Settings.PencilSize = value; }
            return designer.Settings.PencilSize;
        }

        float radiusToleranceProperty(bool set, float value)
        {
            if (set) { designer.Settings.RadiusTolerance = value; }
            return designer.Settings.RadiusTolerance;
        }

        int RoadUpwardClearProperty(bool set, int value)
        {
            if (set) { designer.Settings.RoadUpwardClear = value; }
            return designer.Settings.RoadUpwardClear;
        }
        int RoadBelowFillProperty(bool set, int value)
        {
            if (set) { designer.Settings.RoadBelowFill = value; }
            return designer.Settings.RoadBelowFill;
        }
        int RoadEdgeSizeProperty(bool set, int value)
        {
            if (set) { designer.Settings.RoadEdgeSize = value; }
            return designer.Settings.RoadEdgeSize;
        }
        int RoadPercentFillProperty(bool set, int value)
        {
            if (set) { designer.Settings.RoadPercentFill = value; }
            return designer.Settings.RoadPercentFill;
        }

        bool bSelectionCutProperty(int index, bool set, bool value)
        {
            if (set) { designer.Settings.SelectionCut = value; }
            return designer.Settings.SelectionCut;
        }
        bool bRoundPencilProperty(int index, bool set, bool value)
        {
            if (set) { designer.Settings.RoundPencil = value; }
            return designer.Settings.RoundPencil;
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

        override public void selectionMenu()
        { 
        
        }

        public void DeleteMe()
        {
            closeMenu();

        }
    }
}
