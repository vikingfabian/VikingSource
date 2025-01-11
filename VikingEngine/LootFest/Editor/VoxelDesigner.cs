using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Voxels;
//xna
using VikingEngine.DataStream;
using VikingEngine.HUD;
using VikingEngine.LootFest.Data;
using VikingEngine.Input;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.LootFest.Editor
{

    class VoxelDesigner : Voxels.AbsVoxelDesigner
    {
        public MergeModelsOption mergeModelsOption = new MergeModelsOption().StandardInit();
        public Players.Player parent = null;

        public bool inGame { get { return parent != null; } }

        public static readonly IntVector3 StandardDrawlimit = new IntVector3(
           Data.Block.NumObjBlocksPerTerrainBlock * 2,
           Data.Block.NumObjBlocksPerTerrainBlock * 3,
           Data.Block.NumObjBlocksPerTerrainBlock * 2) - 1;
        public static readonly IntervalIntV3 StandardDrawLimitRange =
            new IntervalIntV3(IntVector3.Zero, StandardDrawlimit);
        

        BlockHD swapMaterialFrom;
        TextG infoText;
        
        //float sphereRadius = 2.5f;

        Graphics.Mesh doorOutline;
        bool lockInputFirstFrame = true;

        Display.ColorPicker colorPicker = null;
        public bool combineLoading = false;

        DesignMenuSystem menusystem;
        public DesignerStorage storage;

        Timer.Basic autoSaveTimer = new Timer.Basic(TimeExt.MinutesToMS(1f), true);

        public void pickColorLink(BlockHD col)
        {
            col.material = SelectedMaterial.material;

            SelectedMaterial = col;
            menusystem.closeMenu();
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

        public static Map.WorldPosition HeroPosToCreationStartPos(IntVector2 heroScreen)
        {
            Map.WorldPosition result = Map.WorldPosition.EmptyPos;
            result.ChunkGrindex = heroScreen;
            result.ChunkGrindex -= Editor.VoxelDesigner.CreationChunkWidth / PublicConstants.Twice;

            result.LocalBlockGrindex = new IntVector3(1, 0, 1);
            return result;
        }

        public const int CreationChunkWidth = 5;
        public const int CreationXZSize = Map.WorldPosition.ChunkWidth * CreationChunkWidth - 2;

        public static readonly IntervalIntV3 CreationSizeLimit = new IntervalIntV3(IntVector3.Zero,
            new IntVector3(CreationXZSize, Map.WorldPosition.ChunkHeight, CreationXZSize) - 1);

        void storeSelectionAsTemplate(int category)
        {
            storage.beginStoreSelectionAsTemplate(category);
            menusystem.closeMenu();
            //visa save warning

            if (inGame)
            {
                parent.Print("Template Saved");
                //parent.Print(DataLib.AbsSaveToStorage.AbleToSave ?
                //    "Template Saved" : "Saving Failed");
            }
        }

        public VoxelDesigner(Map.WorldPosition voxelDesignerStartPos,
            Graphics.AbsCamera camera, float CamTopViewFOV, VectorRect menuArea, Players.Player parent)
            : base(CreationSizeLimit,
            new Vector3(voxelDesignerStartPos.PositionV3.X, 0, voxelDesignerStartPos.PositionV3.Z), 
            parent.inputMap.editorInput, parent.inputMap.menuInput, parent.PlayerIndex, true, true)
        { //IN GAME 
            bUpdateDrawLimits = false;
            ZoomBounds.Max = 200;
            this.parent = parent;
            designerInterface.settings = parent.Storage.voxelDesignerSettings;
            worldPos = voxelDesignerStartPos;
            basicInit(menuArea);

            //place the selection in center
            designerInterface.freePencilGridPos = new Vector3(
                drawLimits.Max.X * PublicConstants.Half, 
                parent.hero.HeadPosition.Y,
                drawLimits.Max.Z * PublicConstants.Half);
            designerInterface.moveFreePencil(Vector3.Up * 0.1f);

            base.camera.targetZoom = camera.targetZoom;
            base.camera.Tilt = camera.Tilt;
            base.camera.FieldOfView = CamTopViewFOV;
            base.camera.aspectRatio = camera.aspectRatio;
        }

        public VoxelDesigner(int playerIndex)
            : base(StandardDrawLimitRange, Vector3.Zero, 
                  ((LootFest.Players.InputMap)XGuide.GetPlayer(playerIndex).inputMap).editorInput,
                  ((LootFest.Players.InputMap)XGuide.GetPlayer(playerIndex).inputMap).menuInput, 
                  playerIndex, false, false)
        { //IN EDITOR
           
            basicInit(new VectorRect(
                Engine.Screen.SafeArea.Position, new Vector2(300, Engine.Screen.SafeArea.Height)));
            Ref.draw.Camera.targetZoom = 40;
        }
        override protected bool viewDrawLimitGrid { get { return !inGame; } }
        
        override protected bool allowSelectAll { get { return !inGame; } }

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
        }

        void basicInit(VectorRect menuArea)
        {
            storage = new DesignerStorage(this);
            menusystem = new DesignMenuSystem(this);
            absMenuSystem = menusystem;

            infoText = new TextG(LoadedFont.Regular, new Vector2(menuArea.X, menuArea.Bottom - 30),
                VectorExt.V2(0.8f), Align.Zero, "info", Color.White, ImageLayers.Background6);
            designerInterface.hudElements.Add(infoText);
                        
            bUpdateDrawLimits = true;
            UpdateDrawLimits();
        }

        public void openColorPicker()
        {
            colorPicker = new Display.ColorPicker(SelectedMaterial, playerIndex);
        }
        
        void selectMaterial(BlockHD m)
        {
            SelectedMaterial = m;
            menusystem.closeMenu();
        }
        void selectSecondaryMaterial(BlockHD m)
        {
            SecondaryMaterial = m;
            menusystem.menu.PopLayout();
        }


        protected override void chagedTool(PaintFillType tool)
        {
            infoText.TextString = "Tool: " + tool.ToString();
        }

        void deleteArea(IntervalIntV3 volume)
        {
            ushort value = BlockHD.Empty.BlockValue;
            IntVector3 drawPoint = new IntVector3();

            for (drawPoint.Z = volume.Min.Z; drawPoint.Z <= volume.Max.Z; drawPoint.Z++)
            {
                for (drawPoint.Y = volume.Min.Y; drawPoint.Y <= volume.Max.Y; drawPoint.Y++)
                {
                    for (drawPoint.X = volume.Min.X; drawPoint.X <= volume.Max.X; drawPoint.X++)
                    {
                        SetVoxel(drawPoint , value);
                    }
                }
            }
        }

        public override void SetVoxel(IntVector3 drawPoint, ushort material)
        {
            if (inGame)
            {
                Map.WorldPosition pos = worldPos;
                pos.WorldGrindex.Add(drawPoint);
                pos.SetBlock_IfOpen(material);
            }
            else
                base.SetVoxel(drawPoint, material);
        }

        public override void SetVoxel(Map.WorldPosition wp, ushort material)
        {
            if (inGame)
            {
                wp.SetBlock_IfOpen(material);
            }
            else
                base.SetVoxel(wp, material);
        }

        public override ushort GetVoxel(IntVector3 drawPoint)
        {
            if (inGame)
            {
                Map.WorldPosition pos = worldPos;
                pos.WorldGrindex.Add(drawPoint);
                return pos.GetBlock();

            }
            return base.GetVoxel(drawPoint);
        }
        
        protected override bool selectionCut
        {
            get
            {
                return Settings.SelectionCut;
            }
        }

        protected override void onSelect()
        {
            base.onSelect();
            if (inGame)
            {
                parent.beginInputOverview();
            }
        }
        
        override protected bool resetWhiteLines
        {
            get
            {
                return !inGame;
            }
        }
                                
        const string DoorPart1Text = "Door (open)";
        const string DoorPart2Text = "Door (closed)";
        
        void newCharacterSizeAdjust()
        {
            drawLimits.Max.AddDimension(Dimensions.X, 2);
            drawLimits.Max.AddDimension(Dimensions.Y, 2);
            drawLimits.Max.AddDimension(Dimensions.Z, 2);
            UpdateDrawLimits();

            moveAll(new IntVector3(1, 0, 1));

            updateVoxelObj();
        }
        
        
        public bool WaitingForTextInput = false;
        

        public void LinkSetLimitsAfterSel()
        {
            if (HasSelection)
            {
                lockFirstFrames = 0;
                dropSelection(false);
                bool repeatSave = repeateOnAllFrames;
                repeateOnAllFrames = true;
                moveAll(-designerInterface.selectionArea.Min);
                repeateOnAllFrames = repeatSave;
                drawLimits.Max = designerInterface.selectionArea.Add;
                UpdateDrawLimits();
                updateVoxelObj();
            }
        }
        
        public void LinkStampOnFrames(int frame = -1)
        {
            int current = currentFrame.Value;

            if (frame < 0)
            {
                for (int i = 0; i < animationFrames.Frames.Count; i++)
                {
                    //nextFrame(true);
                    currentFrame.Value = i;
                    if (stampEmpty)
                    {
                        deleteArea(designerInterface.selectionArea);
                    }
                    
                    stampSelection(false);
                }
            }
            else
            {
                currentFrame.Value = frame;
                if (stampEmpty)
                {
                    deleteArea(designerInterface.selectionArea);
                }
                stampSelection(false);
            }

            currentFrame.Value = current;
            menusystem.closeMenu();
        }

        public void ClearSelectedAreaOnFrames(bool includeThisFrame)
        {
            int current = currentFrame.Value;
            for (int i = 0; i < animationFrames.Frames.Count; i++)
            {
                //nextFrame(true);
                if (i != current || includeThisFrame)
                {
                    currentFrame.Value = i;
                    
                        deleteArea(designerInterface.selectionArea);
                    
                }
            }

            currentFrame.Value = current;
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
            lockFirstFrames = 0;
            menusystem.closeMenu();
       }
       public void LinkAnimLockFrame()
       {
           lockFirstFrames = currentFrame.Value;
           menusystem.closeMenu();
       }

        public void LinkAnimAddFrame()
        {
            const int MaxFrames = 140;

            if (currentFrame.Max < MaxFrames)
            {
                AddFrame();
            }
            else
            {
                SoundLib.UnavailableActionSound.PlayFlat();
            }
        }
        public void LinkHideHUD()
        {
            ShowHUD(false);
            menusystem.closeMenu();
        }        

        public void setBgCol(Color col)
        {
            Ref.draw.ClrColor = col;
        }

        public void LinkClearAll()
        {
            this.NewCanvas();
            storage.clearName();
            menusystem.closeMenu();
        }
        
        public void changeCanvasSize(IntVector3 add)
        {
            drawLimits.Max += add;
            UpdateDrawLimits();
            updateVoxelObj();

            if (add.LargestSideLength() > 1)
            {
                bool storeRepeateOnAllFrames = repeateOnAllFrames;
                repeateOnAllFrames = true;
                moveAll(add / 2);

                repeateOnAllFrames = storeRepeateOnAllFrames;
            }
        }
        public void setCanvasSize(IntVector3 size)
        {
            drawLimits.Size = size;
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
            if (inGame)
                parent.EndCreationMode();
            else
            {
                if (Ref.gamestate.previousGameState == null)
                    Ref.update.exitApplication = true;
                else
                    Engine.StateHandler.PopGamestate();
            }
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

        protected override void exportSelectionAsByteArray()
        {
            base.exportSelectionAsByteArray();
            if (inGame)
            {
                parent.Print("Exported");
            }
        }
        
        public static string searchPattern(bool save)
        {
            return "*" + Voxels.VoxelLib.VoxelObjByteArrayEnding;
        }
        
        public void voxelGridToSelection(VoxelObjGridDataHD grid)
        {
            selectedVoxels.Voxels = grid.GetVoxelArray();

            selectedVoxels.Move(designerInterface.drawCoord, drawLimits);
            refreshSelectionModel();
            menusystem.closeMenu();

            designerInterface.selectionArea = new IntervalIntV3(designerInterface.drawCoord, designerInterface.drawCoord + grid.Limits);
            designerInterface.refreshVolumeGui();
            //designerInterface.refreshSelectionGui();
            templateSent = false;
        }


        void loadTemplateFile(DataStream.FilePath path)
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
        public void clearSelectedArea_AllFrames()
        {
            for (int i = 0; i < animationFrames.Frames.Count; i++)
            {
                nextFrame(true);
                //drawInArea(PaintToolType.Delete, DrawShape.Rectangle, designerInterface.selectionArea, false);
            }
            clearSelectedArea();
        }
        public void clearSelectedArea_AllFramesButThis()
        {
            IntervalIntV3 area = designerInterface.selectionArea;
            dropSelection(false);
            int protectedFrame = currentFrame.Value;
            for (int i = 0; i < animationFrames.Frames.Count - 1; i++)
            {
                nextFrame(true);
                if (currentFrame.Value != protectedFrame)
                { } //drawInArea(PaintToolType.Delete, DrawShape.Rectangle, area, false);
            }
        }
        public void InsertLoadedTemplate(VoxelObjListDataHD selectedVoxels, IntervalIntV3 volume)
        {
            this.selectedVoxels = selectedVoxels;
            designerInterface.selectionArea = volume;
            
            refreshSelectionModel();
        }
        
        //void delete

        public override void stampSelection(bool startThread)
        {

            if (inGame)
            {
                if (HasSelection)
                {
                    Music.SoundManager.PlayFlatSound(LoadedSound.block_place_1);
                    storeUndoableAction();
                    foreach (VoxelHD v in selectedVoxels.Voxels)
                    {
                        worldPos.GetNeighborPos(v.Position).SetBlock(v.Material);
                    }
                    UpdateVoxelObj(selectedVoxels.getMinMax());
                    //NetworkWriteTemplate();
                }
            }
            else
            {
                base.stampSelection(startThread);
            }

        }
        List<VoxelModelName> loadableInGameObjects()
        {
            List<VoxelModelName> loadable = new List<VoxelModelName>
            {
                VoxelModelName.Apple,
                VoxelModelName.ApplePie,
                VoxelModelName.barrelX,
                VoxelModelName.bee,
                VoxelModelName.Character,
                VoxelModelName.chest_open,
                VoxelModelName.Coin,
                VoxelModelName.cook,
                VoxelModelName.crockodile1,
                VoxelModelName.ent,
                VoxelModelName.father,
                VoxelModelName.fire_goblin,
                VoxelModelName.frog1,
                VoxelModelName.ghost,
                VoxelModelName.granpa2,
                VoxelModelName.grunt,
                VoxelModelName.harpy,
                VoxelModelName.hog_lvl1,
                VoxelModelName.lizard1,
                VoxelModelName.Lumberjack,
                VoxelModelName.magician,
                VoxelModelName.mommy,
                VoxelModelName.orc_sword1,
                VoxelModelName.Pig,
                VoxelModelName.priest,
                VoxelModelName.scorpion1,
                VoxelModelName.sheep,
                VoxelModelName.spider1,
                VoxelModelName.squig_lvl1,
                VoxelModelName.war_veteran,
                VoxelModelName.white_hen,
                VoxelModelName.wolf_lvl1,
                VoxelModelName.zombie1,
            };


            return loadable;
        }

        public void LinkTemplateDeleteOK(FilePath path)
        {
            if (inGame)
            {
                new DataStream.RemoveFile(path, null, false);
            }
            parent.Print("Template Deleted");
            menusystem.listTemplates();
        }

        const string ThrallordPath = LfLib.DataFolder + "Thrallords";
        const string RaceTrackPath = LfLib.DataFolder + "Data\\RaceTracks";
       
        protected override void pickColor()
        {
            base.pickColor();
            if (drawCoordMaterial.HasMaterial())
            {
                SelectedMaterial = drawCoordMaterial;
                if (inGame)
                {
                    parent.Print("Picked: " + Settings.Material.ToString());
                }

                Debug.Log("Picked: " + Settings.Material.ToString() + ", blockVal:" + Settings.Material.BlockValue.ToString());
            }
        }

        override protected ushort Get(IntVector3 pos)
        {
            if (inGame)
            {
                Map.WorldPosition wp = Map.WorldPosition.EmptyPos;
                wp = worldPos.GetNeighborPos(pos);
                return wp.GetBlock();
            }
            else
            {
                return voxels.Get(pos);
            }
        }

        protected override void UpdatePencilInfo()
        {
            if (drawLimits.pointInBounds(designerInterface.drawCoord))
            {
                Map.WorldPosition wp = Map.WorldPosition.EmptyPos;
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

        public void addLoadedModel(VoxelObjGridDataAnimHD loadedModel, bool combineLoading)
        {
            storeUndoableAction();

            if (combineLoading)
            {
                if (loadedModel.Frames.Count == 1 && animationFrames.Frames.Count == 1)
                {
                    selectMergeOption(MergeFramesOptions.NewFirstOnOldFrames, loadedModel);
                }
                else
                {

                    menusystem.mergeOptions(loadedModel);
                }
            }
            else
            {
                animationFrames = loadedModel;
                drawLimits.Max = animationFrames.Frames[0].Limits;
                EventTriggerCallBack();
            }
        }


        public void selectMergeOption(MergeFramesOptions opt, VoxelObjGridDataAnimHD loadedModel)
        {
            mergeModelsOption.MergeFramesOptions = opt;
            this.animationFrames.Merge(loadedModel, mergeModelsOption);
            EventTriggerCallBack();
        }

        void FlipSelection(Dimensions dir)
        {
            menusystem.closeMenu();
            voxels.FlipDir(dir, drawLimits, true);
            updateVoxelObj();

        }

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

            GuiLayout layout = new GuiLayout("Swap Material To", menusystem.menu, GuiLayoutMode.MultipleColumns, null);
            {
                new GuiTextButton("Empty", null, new GuiAction1Arg<BlockHD>(replaceSelectionMaterialsTo, VikingEngine.LootFest.Map.HDvoxel.BlockHD.Empty),
                    true, layout);
                menusystem.colorPalette(layout, replaceSelectionMaterialsTo);
            } layout.End();
        }

        void replaceSelectionMaterialsTo(BlockHD to)
        {
            storeUndoableAction();

            ushort swapTo = to.BlockValue;
            if (HasSelection)
            {
                swapMaterials(selectedVoxels, swapTo, true);

                if (!inGame && repeateOnAllFrames)
                {
                    for (int i = 0; i < animationFrames.Frames.Count; i++)
                    {
                        if (i != currentFrame.Value)
                        {
                            animationFrames.Frames[i].ReplaceMaterial(swapMaterialFrom.BlockValue, swapTo, designerInterface.selectionArea);
                        }
                    }
                }
            }
            else
            {
                swapMaterials(voxels, swapTo);
            }
            templateSent = false;
            absMenuSystem.closeMenu();
        }

        public static void listMaterials(HUD.Gui menu, Action<BlockHD> callback, bool includeEmptySpace)
        {
            VikingEngine.HUD.GuiLayout layout = new HUD.GuiLayout(SpriteName.NO_IMAGE, "Select Color", menu, HUD.GuiLayoutMode.MultipleColumns);
            {
                DesignMenuSystem.BigPalette(layout, callback);
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
            designerInterface.selectionArea.Max = voxels.Size - 1;

            swapMaterials(voxels, to);
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
            if (inGame)
            {
                parent.Print("Large selection!");
            }
        }

        public static Map.WPRange NetworkReadTemplate(System.IO.BinaryReader r, Players.ClientPlayer sender)
        {
            if (!r.ReadBoolean())
            {
                sender.EditorTemplateSize = IntVector3.FromByteSzStream(r);
                VoxelObjGridData grid = new VoxelObjGridData(sender.EditorTemplateSize);

                int length = r.ReadInt16();
                byte[] array = new byte[length];
                r.Read(array, 0, length);
                List<byte> list = new List<byte>();
                list.AddRange(array);
                grid.FromCompressedData(list);

                sender.EditorTemplate = new VoxelObjListData(grid.GetVoxelArray());
            }
            Map.WorldPosition worldPos = Map.WorldPosition.EmptyPos;
            worldPos.WorldGrindex = IntVector3.FromStream(r);
            
            foreach (Voxel v in sender.EditorTemplate.Voxels)
            {
                worldPos.GetNeighborPos(v.Position).SetBlock(v.Material);
            }

            Map.WorldPosition max = worldPos;
            max.WorldGrindex += sender.EditorTemplateSize;
            return new Map.WPRange(worldPos, max);
        }
        

        public override void print(string text)
        {
            if (inGame)
            {
                parent.Print(text);
            }
        }
        
        public override void updateVoxelObj(IntervalIntV3 updateArea)
        {
            if (inGame)
            {
                updateArea.AddRadius(1);
                Map.WorldPosition minScreen = worldPos;
                minScreen.WorldGrindex.Add(updateArea.Min);
                Map.WorldPosition maxScreen = worldPos;
                maxScreen.WorldGrindex.Add(updateArea.Max);

                UpdateMapArea(minScreen, maxScreen);
            }
            else
            {
                base.updateVoxelObj(updateArea);
            }

        }

        public static void UpdateMapArea(Map.WorldPosition worldPos, IntVector3 size)
        {
            Map.WorldPosition maxScreen = worldPos;
            maxScreen.WorldGrindex += size;

            UpdateMapArea(worldPos, maxScreen);
        }

        public static void UpdateMapArea(Map.WorldPosition minScreen, Map.WorldPosition maxScreen)
        {
            Map.World.ReloadChunkMesh(minScreen, maxScreen, true);
            
        }

        public void exportObjModel()
        {
             menusystem.mainMenu();
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
