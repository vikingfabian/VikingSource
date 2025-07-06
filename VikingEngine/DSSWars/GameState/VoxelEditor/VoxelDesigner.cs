using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameState;
using VikingEngine.Engine;


using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.Input;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.PJ;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars.GameState.VoxelEditor
{

    class VoxelDesigner : AbsVoxelDesigner
    {
        public MergeModelsOption mergeModelsOption = new MergeModelsOption().StandardInit();
        //public Players.Player parent = null;

        
        public static readonly IntVector3 StandardDrawlimit = new IntVector3(
           16,
           24,
           16) - 1;
        public static readonly IntervalIntV3 StandardDrawLimitRange =
            new IntervalIntV3(IntVector3.Zero, StandardDrawlimit);


        public BlockHD swapMaterialFrom;
        TextG infoText;

        //float sphereRadius = 2.5f;

        Mesh doorOutline;
        bool lockInputFirstFrame = true;

        ColorPicker colorPicker = null;
        //public bool loadOption_fromStorage = true;
        public bool loadOption_combine = false;
        public bool loadOption_preview = false;

        VoxelEditorMenu2 menusystem;
        VoxelEditorInputHelp inputHelp;
        public DesignerStorage storage;

        Timer.Basic autoSaveTimer = new Timer.Basic(TimeExt.MinutesToMS(1f), true);
        public InputMap dssInput;

        MessageGroup_Editor messages;

        public void pickColorLink(BlockHD col)
        {
            col.material = SelectedMaterial.material;

            SelectedMaterial = col;
            menusystem.closeMenu();
        }
        public void replaceAllMaterials()
        {
            storeUndoableAction(repeateOnAllFrames, repeateOnAllLayers);
            voxelProject.replaceAllMaterialProperties(SelectedMaterial.material, repeateOnAllFrames, repeateOnAllLayers);
            menusystem.closeMenu();
        }
        public void pickColorAndMaterialLink(BlockHD col)
        {
            //col.material = SelectedMaterial.material;

            SelectedMaterial = col;
            menusystem.closeMenu();
        }

        public void pickMaterialLink(MaterialProperty material)
        {
            Settings.Material.material = material;
            //menusystem.closeMenu();
        }

        override public BlockHD SelectedMaterial
        {
            get { return Settings.Material; }
            set
            {
                Settings.Material = value;
            }
        }
        override public BlockHD SecondaryMaterial
        {
            get { return Settings.SecondaryMaterial; }
            set
            {
                Settings.SecondaryMaterial = value;
            }
        }

        void storeSelectionAsTemplate(int category)
        {
            storage.beginStoreSelectionAsTemplate(category);
            menusystem.closeMenu();
            //visa save warning

        }

       
        public VoxelDesigner(bool controller, int playerIndex)
            : base(StandardDrawLimitRange, Vector3.Zero,
                 XGuide.LocalHost.inputMap.VoxelEditorInput(),
                 XGuide.LocalHost.inputMap.menuInput,
                 playerIndex, false, false)
        {

            messages = new MessageGroup_Editor();

            basicInit(new VectorRect(
                Screen.SafeArea.Position, new Vector2(300, Screen.SafeArea.Height)));
            Ref.draw.Camera.targetZoom = 40;

            inputHelp = new VoxelEditorInputHelp();

            setupNewInput(controller, playerIndex);
        }

        public void setupNewInput(bool controller, int playerIndex)
        {
            if (dssInput == null || dssInput.inputSource.IsController != controller)
            {
                dssInput = new InputMap(playerIndex);
                dssInput.setInputSource(controller? InputSourceType.XController : InputSourceType.KeyboardMouse, playerIndex);
                if (controller)
                {
                    dssInput.copyDataFrom(Ref.gamesett.controllerMap);
                }
                else
                {
                    dssInput.copyDataFrom(Ref.gamesett.keyboardMap);
                }

                inputMap = dssInput.VoxelEditorInput();
                if (menusystem.menu != null)
                {
                    menusystem.menu.needRefresh = true;
                }
                if (inputHelp.menu != null)
                {
                    inputHelp.menu.needRefresh = true;
                }
            }
        }

        override protected bool viewDrawLimitGrid { get { return true; } }

        override protected bool allowSelectAll { get { return true; } }

        public override void UpdateInput()
        {
            if (lockInputFirstFrame)
            {
                lockInputFirstFrame = false;
                return;
            }

            if (colorPicker != null)
            {
                if (colorPicker.update())
                {
                    SelectedMaterial = colorPicker.result;
                    colorPicker.DeleteMe();
                    colorPicker = null;

                    menusystem.closeMenu();
                }
            }
            else
            {
                base.UpdateInput();
            }

            if (autoSaveTimer.Update())
            {
                storage.saveBackUp();
            }

            if (XInput.AnyActivationKey_DownEvent(out int playerIx))
            {
                setupNewInput(true, playerIx);
            }
            else if (Keyboard.AnyActivationKey_DownEvent())
            {
                setupNewInput(false, 0);
            }
            else if (inputHelp.menu != null && inputHelp.menu.needRefresh)
            {
                inputHelp.refreshUpdate(this, prevInputState, dssInput);
            }

            bool mouseOverHud = false;
            messages.Update(ref mouseOverHud);
        }

        protected override bool openMenuInput()
        {
            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                //setupNewInput(false, 0);
                return true;
            }

            int playerIx;
            if (XInput.KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.Start, out playerIx))
            {
                //setupNewInput(true, playerIx);
                return true;
            }

            return false;
        }

        protected override void OnNewInputState(VoxelEditorInputState inputState)
        {
            base.OnNewInputState(inputState);
            inputHelp.refreshUpdate(this, inputState, dssInput);
        }

        void basicInit(VectorRect menuArea)
        {
            storage = new DesignerStorage(this);
            menusystem = new VoxelEditorMenu2(this);
            absMenuSystem = menusystem;

            infoText = new TextG(LoadedFont.Regular, new Vector2(menuArea.X, menuArea.Bottom - 30),
                VectorExt.V2(0.8f), Align.Zero, "info", Color.White, ImageLayers.Background6);
            designerInterface.hudElements.Add(infoText);

            bUpdateDrawLimits = true;
            UpdateDrawLimits();
        }

        public void openColorPicker()
        {
            colorPicker = new ColorPicker(SelectedMaterial, playerIndex);
        }

        void selectMaterial(BlockHD m)
        {
            SelectedMaterial = m;
            menusystem.closeMenu();
        }
        void selectSecondaryMaterial(BlockHD m)
        {
            SecondaryMaterial = m;
            menusystem.menu.menuBack();//.menu.PopLayout();
        }


        protected override void chagedTool(PaintFillType tool)
        {
            infoText.TextString = "Tool: " + tool.ToString();
        }

        void deleteArea(int frame, IntervalIntV3 volume)
        {
            ushort value = BlockHD.Empty.BlockValue;
            IntVector3 drawPoint = new IntVector3();

            for (drawPoint.Z = volume.Min.Z; drawPoint.Z <= volume.Max.Z; drawPoint.Z++)
            {
                for (drawPoint.Y = volume.Min.Y; drawPoint.Y <= volume.Max.Y; drawPoint.Y++)
                {
                    for (drawPoint.X = volume.Min.X; drawPoint.X <= volume.Max.X; drawPoint.X++)
                    {
                        SetVoxel(frame, drawPoint, value);
                    }
                }
            }
        }

        //public override void SetVoxel(IntVector3 drawPoint, ushort material)
        //{
        //    if (inGame)
        //    {
        //        Map.WorldPosition pos = worldPos;
        //        pos.WorldGrindex.Add(drawPoint);
        //        pos.SetBlock_IfOpen(material);
        //    }
        //    else
        //        base.SetVoxel(drawPoint, material);
        //}

        //public override void SetVoxel(Map.WorldPosition wp, ushort material)
        //{
        //    if (inGame)
        //    {
        //        wp.SetBlock_IfOpen(material);
        //    }
        //    else
        //        base.SetVoxel(wp, material);
        //}

        //public override ushort GetVoxel(IntVector3 drawPoint)
        //{
        //    if (inGame)
        //    {
        //        Map.WorldPosition pos = worldPos;
        //        pos.WorldGrindex.Add(drawPoint);
        //        return pos.GetBlock();

        //    }
        //    return base.GetVoxel(drawPoint);
        //}

        protected override bool selectionCut
        {
            get
            {
                return Settings.SelectionCut;
            }
        }

        //protected override void onSelect()
        //{
        //    base.onSelect();
        //    if (inGame)
        //    {
        //        parent.beginInputOverview();
        //    }
        //}

        override protected bool resetWhiteLines
        {
            get
            {
                return true;
            }
        }

        //const string DoorPart1Text = "Door (open)";
        //const string DoorPart2Text = "Door (closed)";

        //void newCharacterSizeAdjust()
        //{
        //    drawLimits.Max.AddDimension(Dimensions.X, 2);
        //    drawLimits.Max.AddDimension(Dimensions.Y, 2);
        //    drawLimits.Max.AddDimension(Dimensions.Z, 2);
        //    UpdateDrawLimits();

        //    moveAll(new IntVector3(1, 0, 1), false);

        //    updateVoxelObj();
        //}


        public bool WaitingForTextInput = false;


        public void LinkSetLimitsAfterSel()
        {
            if (HasSelection)
            {
                //lockFirstFrames = 0;
                dropSelection(false);
                //bool repeatSave = repeateOnAllFrames;
                //repeateOnAllFrames = true;
                moveAll(-designerInterface.selectionArea.Min, true, true, true);
                //repeateOnAllFrames = repeatSave;
                var limits = voxelProject.drawLimits;
                limits.Max = designerInterface.selectionArea.Add;
                voxelProject.setDrawLimit(limits);

                UpdateDrawLimits();
                updateVoxelObj();
            }
        }

        public void LinkStampOnFrames(int frame = -1)
        {
            //int current = voxelProject.currentFrame.Value;

            if (frame < 0)
            {
                for (int i = 0; i < voxelProject.AnimationFrames.Frames.Count; i++)
                {
                    //nextFrame(true);
                    //currentFrame.Value = i;
                    if (stampEmpty)
                    {
                        deleteArea(i, designerInterface.selectionArea);
                    }

                    stampSelection(false, frame);
                }
            }
            else
            {
                //currentFrame.Value = frame;
                if (stampEmpty)
                {
                    deleteArea(frame, designerInterface.selectionArea);
                }
                stampSelection(false, frame);
            }

            //currentFrame.Value = current;
            menusystem.closeMenu();
        }

        public void ClearSelectedAreaOnFrames(bool includeThisFrame)
        {
            //int current = currentFrame.Value;
            for (int i = 0; i < voxelProject.AnimationFrames.Frames.Count; i++)
            {
                //nextFrame(true);
                if (i != CurrentFrame || includeThisFrame)
                {
                    //currentFrame.Value = i;

                    deleteArea(i, designerInterface.selectionArea);

                }
            }

            //currentFrame.Value = current;
            if (includeThisFrame)
            {
                removeSelection();
                //UpdateVoxelObj(designerInterface.selectionArea);
            }
            menusystem.closeMenu();
        }

        public void LinkSelRotateLieDown()
        {
            templateSent = false;
        }

        public void LinkAnimUnlockFrame()
        {
            voxelProject.lockFirstFrames = 0;
            menusystem.closeMenu();
        }
        public void LinkAnimLockFrame(bool start)
        {
            //lockFirstFrames = currentFrame.Value;
            voxelProject.LockAnimation(start);
            menusystem.closeMenu();
        }

        //public void LinkAnimAddFrame()
        //{
        //    const int MaxFrames = 140;

        //    if (currentFrame.Max < MaxFrames)
        //    {
        //        AddFrame();
        //    }
        //    else
        //    {
        //        SoundLib.wrong.Play();
        //        //SoundLib.UnavailableActionSound.PlayFlat();
        //    }
        //}
        public void LinkHideHUD()
        {
            ShowHUD(false);
            menusystem.closeMenu();
            inputHelp.deleteMenu();
        }

        public void setBgCol(Color col)
        {
            Ref.draw.ClrColor = col;
        }

        public void LinkClearAll()
        {
            NewCanvas();
            storage.clearName();
            menusystem.closeMenu();
        }

        public void changeCanvasSize(IntVector3 add)
        {
            storeUndoableAction(true, true);
            var limits = voxelProject.drawLimits;
            limits.Max += add;
            voxelProject.setDrawLimit(limits);
           

            if (add.LargestSideLength_Abs() > 1)
            {
                bool storeRepeateOnAllFrames = repeateOnAllFrames;
                repeateOnAllFrames = true;
                moveAll(add / 2, false);

                repeateOnAllFrames = storeRepeateOnAllFrames;
            }

            UpdateDrawLimits();
            updateVoxelObj();
        }
        public void setCanvasSize(IntVector3 size)
        {
            //drawLimits.Size = size;
            voxelProject.setSize(size);
            UpdateDrawLimits();
            updateVoxelObj();
        }

        public void onFileNameChange(string result, object tag)
        {
            if (result != null)
            {
                storage.saveFileName = result;
            }
        }

        public void LinkEXIT()
        {
            new ExitGamePlay();
            //if (inGame)
            //    parent.EndCreationMode();
            //else
            //{
            //    if (Ref.gamestate.previousGameState == null)
            //        Ref.update.exitApplication = true;
            //    else
            //        Engine.StateHandler.PopGamestate();
            //}
        }

        public void linkFLattenArea()
        {
            //if (parent.ClientPermissions == Players.ClientPermissions.Full)
            //{
            //    designerInterface.selectionArea = new RangeIntV3(IntVector3.Zero, drawLimits.Max);

            //    designerInterface.selectionArea.Max.Y = designerInterface.drawCoord.Y - 1;
            //    storeUndoableAction();
            //    new ThreadedDrawAcion(ThreadedActionType.Rectangle, DrawTool.Rectangle, this, designerInterface.selectionArea, FillType.Fill, false);
            //    designerInterface.selectionArea.Max = drawLimits.Max;
            //    designerInterface.selectionArea.Min.Y = designerInterface.drawCoord.Y;
            //    storeUndoableAction();
            //    new ThreadedDrawAcion(ThreadedActionType.Rectangle, DrawTool.Rectangle, this, designerInterface.selectionArea, FillType.Delete, false);

            //}
            //else
            //{
            //    parent.Print("Need full permission");
            //    menusystem.closeMenu();
            //}
        }
        public void linkPickMaterial()
        {
            pickColor();
            menusystem.closeMenu();
        }

        public void LinkTemplateUse(FilePath path)
        {
            loadTemplateFile(path);
        }

        override public void LinkSelSaveTemplate()
        {
            storeSelectionAsTemplate(0);
        }

        public static string searchPattern(bool save)
        {
            return "*" + VoxelLib.VoxelObjByteArrayEnding;
        }

        public void voxelGridToSelection(VoxelObjGridDataHD grid)
        {
            selectedVoxels.Voxels = grid.GetVoxelArray();

            selectedVoxels.Move(designerInterface.drawCoord, voxelProject.drawLimits);
            refreshSelectionModel();
            menusystem.closeMenu();

            designerInterface.selectionArea = new IntervalIntV3(designerInterface.drawCoord, designerInterface.drawCoord + grid.Limits);
            designerInterface.refreshVolumeGui();
            //designerInterface.refreshSelectionGui();
            templateSent = false;
        }


        void loadTemplateFile(FilePath path)
        {
            templateSent = false;
            storage.beginLoadTemplate(path);
            menusystem.closeMenu();
        }
        public void clearSelectedArea()
        {
            IntervalIntV3 area = designerInterface.selectionArea;
            dropSelection(false);
            //drawInArea(PaintToolType.Delete, DrawShape.Rectangle, area, false);
        }
        //public void clearSelectedArea_AllFrames()
        //{
        //    for (int i = 0; i < animationFrames.Frames.Count; i++)
        //    {
        //        nextFrame(true);
        //        //drawInArea(PaintToolType.Delete, DrawShape.Rectangle, designerInterface.selectionArea, false);
        //    }
        //    clearSelectedArea();
        //}
        //public void clearSelectedArea_AllFramesButThis()
        //{
        //    IntervalIntV3 area = designerInterface.selectionArea;
        //    dropSelection(false);
        //    int protectedFrame = currentFrame.Value;
        //    for (int i = 0; i < animationFrames.Frames.Count - 1; i++)
        //    {
        //        nextFrame(true);
        //        if (currentFrame.Value != protectedFrame)
        //        { } //drawInArea(PaintToolType.Delete, DrawShape.Rectangle, area, false);
        //    }
        //}
        public void InsertLoadedTemplate(VoxelObjListDataHD selectedVoxels, IntervalIntV3 volume)
        {
            this.selectedVoxels = selectedVoxels;
            designerInterface.selectionArea = volume;

            refreshSelectionModel();
        }

        //void delete

        //public override void stampSelection(bool startThread)
        //{

        //    if (inGame)
        //    {
        //        if (HasSelection)
        //        {
        //            Music.SoundManager.PlayFlatSound(LoadedSound.block_place_1);
        //            storeUndoableAction(false);
        //            foreach (VoxelHD v in selectedVoxels.Voxels)
        //            {
        //                worldPos.GetNeighborPos(v.Position).SetBlock(v.Material);
        //            }
        //            UpdateVoxelObj(selectedVoxels.getMinMax());
        //            //NetworkWriteTemplate();
        //        }
        //    }
        //    else
        //    {
        //        base.stampSelection(startThread);
        //    }

        //}
        List<VoxelModelName> loadableInGameObjects()
        {
            List<VoxelModelName> loadable = new List<VoxelModelName>
            {
                //VoxelModelName.Apple,
                //VoxelModelName.ApplePie,
                //VoxelModelName.barrelX,
                //VoxelModelName.bee,
                //VoxelModelName.Character,
                //VoxelModelName.chest_open,
                //VoxelModelName.Coin,
                //VoxelModelName.cook,
                //VoxelModelName.crockodile1,
                //VoxelModelName.ent,
                //VoxelModelName.father,
                //VoxelModelName.fire_goblin,
                //VoxelModelName.frog1,
                //VoxelModelName.ghost,
                //VoxelModelName.granpa2,
                //VoxelModelName.grunt,
                //VoxelModelName.harpy,
                //VoxelModelName.hog_lvl1,
                //VoxelModelName.lizard1,
                //VoxelModelName.Lumberjack,
                //VoxelModelName.magician,
                //VoxelModelName.mommy,
                //VoxelModelName.orc_sword1,
                //VoxelModelName.Pig,
                //VoxelModelName.priest,
                //VoxelModelName.scorpion1,
                //VoxelModelName.sheep,
                //VoxelModelName.spider1,
                //VoxelModelName.squig_lvl1,
                //VoxelModelName.war_veteran,
                //VoxelModelName.white_hen,
                //VoxelModelName.wolf_lvl1,
                //VoxelModelName.zombie1,
            };


            return loadable;
        }

        //public void LinkTemplateDeleteOK(FilePath path)
        //{
        //    if (inGame)
        //    {
        //        new DataStream.RemoveFile(path, null, false);
        //    }
        //    parent.Print("Template Deleted");
        //    menusystem.listTemplates();
        //}

        const string ThrallordPath = LfLib.DataFolder + "Thrallords";
        const string RaceTrackPath = LfLib.DataFolder + "Data\\RaceTracks";

        protected override void pickColor()
        {
            base.pickColor();
            if (drawCoordMaterial.HasMaterial())
            {
                SelectedMaterial = drawCoordMaterial;
                //if (inGame)
                //{
                //    parent.Print("Picked: " + Settings.Material.ToString());
                //}
                //Debug.Log("Picked: " + Settings.Material.ToString() + ", blockVal:" + Settings.Material.BlockValue.ToString());

                RichBoxContent content = new RichBoxContent();
                HudLib.Label(content, DssRef.lang.Editor_PickedColor);
                content.newLine();
                content.Add(new RbImage(SpriteName.VoxelEditorColorCube, 1f, Settings.Material.color));
                content.hspace();
                content.Add(new RbText(string.Format(DssRef.lang.Editor_ColorRGBvalues, Settings.Material.color.R, Settings.Material.color.G, Settings.Material.color.B)));

                content.space(2);
                content.Add(new RbImage(SpriteName.VoxelEditorMaterialCube));
                content.hspace();
                content.Add(new RbText(Settings.Material.material.ToString()));

                print(content);
            }
        }

        //override protected ushort Get(IntVector3 pos)
        //{
            
        //        return curretVoxelGrid.Get(pos);
            
        //}

        protected override void UpdatePencilInfo()
        {
            if (voxelProject.drawLimits.pointInBounds(designerInterface.drawCoord))
            {
                //Map.WorldPosition wp = Map.WorldPosition.EmptyPos;
                drawCoordMaterial.BlockValue = Get(designerInterface.drawCoord);

                infoText.TextString = "X" + designerInterface.drawCoord.X.ToString() + " Y" + designerInterface.drawCoord.Y.ToString() + " Z" + designerInterface.drawCoord.Z.ToString();


                if (HasSelection || drawTools.currentDrawAction != null)
                {
                    IntVector3 size = designerInterface.selectionArea.Add + 1;
                    infoText.TextString += " W" + size.X.ToString() + " H" + size.Y.ToString() + " L" + size.Z.ToString();

                }
                if (drawCoordMaterial.HasMaterial())
                {
                    infoText.TextString += " " + drawCoordMaterial.ToString() + " (" + drawCoordMaterial.BlockValue.ToString() + ")";
                }

                {
                    base.UpdatePencilInfo();
                }


            }
        }

        override public void addLoadedModel(VoxelObjGridDataAnimHD loadedModel/*, bool combineLoading*/)
        {
            if (loadedModel.Frames == null)
            {
#if DEBUG
                throw new Exception();
#endif
                return;
            }

            storeUndoableAction(true, false);

            if (loadOption_combine)
            {
                //NEW: will add as new layer

                //if (loadedModel.Frames.Count == 1 && voxelProject.currentFrame.Length == 1)
                //{
                //    selectMergeOption(MergeFramesOptions.NewFirstOnOldFrames, loadedModel);
                //}
                //else
                //{

                //    //menusystem.mergeOptions(loadedModel);
                //}
            }
            else
            {
                //animationFrames = loadedModel;
                //drawLimits.Max = animationFrames.Frames[0].Limits;
                voxelProject = new VoxelProject(loadedModel);
                EventTriggerCallBack();
            }

            if (!loadOption_preview)
            {
                menusystem.closeMenu();
            }
        }

        public override void addLoadedProject(VoxelProject project)
        {
            voxelProject = project;
            EventTriggerCallBack();
        }


        //public void selectMergeOption(MergeFramesOptions opt, VoxelObjGridDataAnimHD loadedModel)
        //{
        //    mergeModelsOption.MergeFramesOptions = opt;
        //    animationFrames.Merge(loadedModel, mergeModelsOption);
        //    EventTriggerCallBack();
        //}

        //void FlipSelection(Dimensions dir)
        //{
        //    menusystem.closeMenu();
        //    curretVoxelGrid.FlipDir(dir, drawLimits, true);
        //    updateVoxelObj();

        //}

        public void EventTriggerCallBack()
        {
            //after loading a file
            updateFrameInfo();
            updateVoxelObj();
            UpdateDrawLimits();
        }

        override public void linkReplaceSelectionMaterials(ushort from)
        {
            swapMaterialFrom = new BlockHD(from);
            menusystem.openReColorTo();
            //RichBoxContent content = new RichBoxContent();
            //content.h1("Swap Material To", HudLib.TitleColor_Head);
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Empty") }, new RbAction1Arg<BlockHD>(replaceSelectionMaterialsTo, BlockHD.Empty)));

            //menusystem.Refresh(content);

            //GuiLayout layout = new GuiLayout("Swap Material To", menusystem.menu, GuiLayoutMode.MultipleColumns, null);
            //{
            //    new GuiTextButton("Empty", null, new GuiAction1Arg<BlockHD>(replaceSelectionMaterialsTo, VikingEngine.LootFest.Map.HDvoxel.BlockHD.Empty),
            //        true, layout);
            //    menusystem.colorPalette(layout, replaceSelectionMaterialsTo);
            //}
            //layout.End();
        }

        public void replaceSelectionMaterialsTo(BlockHD to)
        {
            storeUndoableAction(repeateOnAllFrames, repeateOnAllLayers);

            ushort swapTo = to.BlockValue;
            if (HasSelection)
            {
                swapMaterials(selectedVoxels, swapTo, true);

                if (repeateOnAllFrames)
                {
                    for (int i = 0; i < voxelProject.AnimationFrames.Frames.Count; i++)
                    {
                        if (i != voxelProject.currentFrame.Value)
                        {
                            voxelProject.AnimationFrames.Frames[i].ReplaceMaterial(swapMaterialFrom.BlockValue, swapTo, designerInterface.selectionArea);
                        }
                    }
                }
            }
            else
            {
                swapMaterials(voxelProject.CurrentVoxelGrid, swapTo);
            }
            templateSent = false;
            absMenuSystem.closeMenu();
        }

        public static void listMaterials(Gui menu, Action<BlockHD> callback, bool includeEmptySpace)
        {
            GuiLayout layout = new GuiLayout(SpriteName.NO_IMAGE, "Select Color", menu, GuiLayoutMode.MultipleColumns);
            {
                //DesignMenuSystem.BigPalette(layout, callback);
            }
            layout.End();


        }

        void swapMaterials(VoxelObjListDataHD voxelList, ushort swapTo, bool updateImage)
        {
            ushort from = swapMaterialFrom.BlockValue;

            if (from == swapTo)
                return;
            if (from == BlockHD.EmptyBlock)
            {
                IntVector3 pos = IntVector3.Zero;
                for (pos.Z = designerInterface.selectionArea.Min.Z; pos.Z <= designerInterface.selectionArea.Max.Z; pos.Z++)
                {
                    for (pos.Y = designerInterface.selectionArea.Min.Y; pos.Y <= designerInterface.selectionArea.Max.Y; pos.Y++)
                    {
                        for (pos.X = designerInterface.selectionArea.Min.X; pos.X <= designerInterface.selectionArea.Max.X; pos.X++)
                        {
                            if (voxelList.GetValue(pos) == BlockHD.EmptyBlock)
                            {
                                voxelList.Voxels.Add(new VoxelHD(pos, swapTo));
                            }
                        }

                    }
                }
            }
            else if (swapTo == BlockHD.EmptyBlock)
            {
                for (int i = voxelList.Voxels.Count - 1; i >= 0; i--)
                {
                    if (voxelList.Voxels[i].Material == from)
                    {
                        voxelList.Voxels.RemoveAt(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < voxelList.Voxels.Count; i++)
                {
                    if (voxelList.Voxels[i].Material == from)
                    {
                        VoxelHD v = voxelList.Voxels[i];
                        v.Material = swapTo;
                        voxelList.Voxels[i] = v;
                    }
                }
            }

            if (updateImage)
            {
                if (HasSelection)
                    refreshSelectionModel();
                else
                    startUpdateVoxelObj(false);

            }
        }

        public void swapMaterials(ushort from, ushort to)
        {
            swapMaterialFrom.BlockValue = from;
            designerInterface.selectionArea.Min = IntVector3.Zero;
            designerInterface.selectionArea.Max = voxelProject.CurrentVoxelGrid.Size - 1;

            swapMaterials(voxelProject.CurrentVoxelGrid, to);
        }

        void swapMaterials(VoxelObjGridDataHD grid, ushort swapTo)
        {
            ushort from = swapMaterialFrom.BlockValue;

            if (from == swapTo)
                return;
            IntVector3 pos = IntVector3.Zero;


            grid.ReplaceMaterial(from, swapTo, designerInterface.selectionArea);

            if (HasSelection)
                refreshSelectionModel();
            else
                startUpdateVoxelObj(false);
        }

        override protected void removeSelection()
        {
            //Merge the selected group of voxels with the original group
            if (HasSelection)
            {
                selectedVoxels.Voxels.Clear();
            }
            refreshSelectionModel();
        }

        protected override void LargeSelectionWarning()
        {
            //if (inGame)
            //{
            //    parent.Print("Large selection!");
            //}
        }

        public override void print(string text)
        {
            //if (inGame)
            //{
            //    parent.Print(text);
            //}
            messages.Add(text);
        }
        public override void print(RichBoxContent content)
        {
            messages.Add(content);
        }

        

        string exportName()
        {
            return storage.saveFileName + TextLib.Parentheses(TextLib.IndexToString(voxelProject.currentFrame.Value));
        }

        public void exportObjModel()
        {
            var frame = voxelProject.AnimationFrames.Frames[voxelProject.currentFrame.Value];
            ObjExporterScript.Export(frame, exportName());
            menusystem.mainMenu();
        }

        public FilePath ExportPath()
        {
            return ObjExporterScript.ExportPath(exportName());
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            menusystem.DeleteMe();
            infoText.DeleteMe();
            clearDoor();
        }
        void clearDoor()
        {
            if (doorOutline != null)
            {
                doorOutline.DeleteMe();
                doorOutline = null;
            }
        }

    }

    enum SaveCategory
    {
        non,
        dontKnow,
        temporary,
        art,
        terrain,
        house,
        castle,
        space,
        roadSign,
        animals,
        squares,
        veihcle,
        smiley,
        character,
        furniture,
        tools,
        NUM
    }
    enum MaterialCategory
    {
        Color,
        Texture,
        Text,
        Devs,
        Joints,
        NUM
    }
}
