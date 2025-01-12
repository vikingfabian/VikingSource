using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD;
using VikingEngine.LootFest.Data;
using VikingEngine.Input;
using VikingEngine.LootFest.Map.HDvoxel;
using System.ComponentModel.Design;

namespace VikingEngine.Voxels
{
    interface IVoxelDesigner
    {
        void SetVoxel(IntVector3 drawPoint, ushort material);
        ushort GetVoxel(IntVector3 drawPoint);
    }

    abstract class AbsVoxelDesigner : VikingEngine.AbsInput, IVoxelDesigner, IVoxelDesignerInterfaceParent
    {
        protected const float PencilMoveSpeedNomal = 0.01f;
        public VoxelDesignerInterface designerInterface;
       
        protected bool repeateOnAllFrames = true;
        protected bool stampEmpty = false;
        abstract public BlockHD SelectedMaterial { get; set; }
        abstract public BlockHD SecondaryMaterial { get; set; }
       
        
        public UndoList undolist = new UndoList();

        protected IntervalF ZoomBounds = new IntervalF(4, 600);
        protected Graphics.AbsDraw voxelObj = null;
        SelectionModel selectionModel = new SelectionModel();

        protected int playerIndex;
        protected EditorInputMap inputMap;
        public HUD.MenuInputMap menuInput;

        MouseToolHUD mouseToolHUD = null;
        protected EditorDrawTools drawTools;
        public LootFest.Map.WorldPosition worldPos = LootFest.Map.WorldPosition.EmptyPos;
        public IntervalIntV3 drawLimits;
        public const float BlockScale = 1f;
        protected Graphics.TopViewCamera camera;
        
        
        TextG frameInfo;

        public int lockFirstFrames = 0; 
        public CirkleCounter currentFrame = new CirkleCounter(0);
        public VoxelObjGridDataAnimHD animationFrames;
        public VoxelObjGridDataHD voxels
        {
            get { return animationFrames.Frames[currentFrame.Value]; }
            set { animationFrames.Frames[currentFrame.Value] = value; }
        }
        public VoxelObjListDataHD copiedVoxels = null;
        public VoxelObjListDataHD selectedVoxels = new VoxelObjListDataHD(new List<VoxelHD>());
        
        protected bool templateSent = false;
        protected AbsDesignMenuSystem absMenuSystem;
        public bool inGameEditor;

        public void ResetTemplateSent()
        {
            templateSent = false;
        }

        bool isDeleted = false;
        public override void DeleteMe()
        {
            
            if (voxelObj!= null)
                voxelObj.DeleteMe();
            
            frameInfo.DeleteMe();
            designerInterface.DeleteMe();

            isDeleted = true;
        }

        public AbsVoxelDesigner(IntervalIntV3 drawLimits, Vector3 offSet, EditorInputMap inputMap, HUD.MenuInputMap menuInput, 
            int playerIndex, bool transparentHelpLines, bool ingameEditor)
        {
            this.inGameEditor = ingameEditor;
            drawTools = new EditorDrawTools(this);

            this.inputMap = inputMap;
            this.menuInput = menuInput;
            this.playerIndex = playerIndex;

            if (ingameEditor == false)
            {
                animationFrames = new VoxelObjGridDataAnimHD();
                animationFrames.Frames = new List<VoxelObjGridDataHD> { new VoxelObjGridDataHD(drawLimits.Max) };
            }
            this.drawLimits = drawLimits;

            camera = new Graphics.TopViewCamera(40, new Vector2(MathHelper.PiOver2 - 0.14f, MathHelper.PiOver4));
            camera.UseTerrainCollisions = false;
            Ref.draw.Camera = camera;
            
            designerInterface = new VoxelDesignerInterface(this, inputMap, playerIndex, drawLimits.Max.Vec * PublicConstants.Half, offSet, 
                transparentHelpLines, drawLimits, camera);

            frameInfo = new TextG(LoadedFont.Regular, Engine.Screen.SafeArea.RightBottom,
                VectorExt.V2(1f), new Align(Dir8.SE), TextLib.EmptyString, Color.Black, ImageLayers.Foreground6);

            new AsynchUpdateable(asynchUpdate, "Voxel Editor Update");
            //Ref.asynchUpdate.AddUpdateThread(asynchUpdate, "Voxel Editor Update", 0);
        }

        bool asynchUpdate(int id, float time)
        {
            drawTools.update_asynch();
            return isDeleted;
        }

        public void RemoveCurrentFrame()
        {
            if (haveAnimation)
            {
                animationFrames.Frames.RemoveAt(currentFrame.Value);
                startUpdateVoxelObj(false);
                print("Removed Frame"); 
            }
        }

        public void NewBlockPosEvent(IntVector3 newCoord, IntVector3 posDiff)
        {
            drawTools.NewBlockPosEvent(newCoord, posDiff);
            
            if (HasSelection)
            {
                moveAll(posDiff, false);
            }
            UpdatePencilInfo();


            designerInterface.pencilShadow.update(this);
        }

        public List<ushort> materialsInUse(bool includePaintCol, out ushort selected)
        {
            List<ushort> result = new List<ushort>();
            bool[] inUse = new bool[ushort.MaxValue];

            if (animationFrames != null)
            {
                foreach (var m in animationFrames.Frames)
                {
                    var loop = new ForXYZLoop(m.Size);
                    while (loop.Next())
                    {
                        ushort mat = m.MaterialGrid[loop.Position.X, loop.Position.Y, loop.Position.Z];

                        if (mat != 0 && !inUse[mat])
                        {
                            inUse[mat] = true;
                            result.Add(mat);
                        }
                    }
                }
            }

            if (includePaintCol)
            {
                //if (!inUse[SelectedMaterial.BlockValue])
                //{
                //    result.Add(SelectedMaterial.BlockValue);
                //}
                selected = SelectedMaterial.BlockValue;

                if (!inUse[SecondaryMaterial.BlockValue])
                {
                    result.Add(SecondaryMaterial.BlockValue);
                }
            }
            else
            {
                selected = 0;
            }

            return result;
        }

        public List<ushort> materialsInSelection()
        {
            List<ushort> result = new List<ushort>();

            if (selectedVoxels != null)
            {
                bool[] inUse = new bool[ushort.MaxValue];

                foreach (var m in selectedVoxels.Voxels)
                {
                    if (!inUse[m.Material])
                    {
                        inUse[m.Material] = true;
                        result.Add(m.Material);
                    }
                }
            }

            return result;
        }

        public void moveFrame(MoveFrameType type)
        {
            VoxelObjGridDataHD current = animationFrames.Frames[currentFrame.Value];
            animationFrames.Frames.RemoveAt(currentFrame.Value);

            switch (type)
            {
                case MoveFrameType.Forward:
                    currentFrame.Next(1);
                    break;
                case MoveFrameType.Back:
                    currentFrame.Next(-1);
                    break;
                case MoveFrameType.ToStart:
                    currentFrame.Value = 0;
                    break;
                case MoveFrameType.ToEnd:
                    currentFrame.Value = currentFrame.Max;
                    break;
            }
            animationFrames.Frames.Insert(currentFrame.Value, current);
            updateFrameInfo();
        }

        public void RemoveAllFramesButThis()
        {
            animationFrames.Frames = new List<VoxelObjGridDataHD> { animationFrames.Frames[currentFrame.Value] };
            updateFrameInfo();
        }
        public void AddFrame()
        {
            animationFrames.Frames.Insert(currentFrame.Value, animationFrames.Frames[currentFrame.Value].Clone());
            int frame = currentFrame.Value;
            updateFrameInfo();
            currentFrame.Value = frame + 1;
            updateFrameInfo();
            print("Frame Added");
        }

        public void nextFrame(bool forward)
        {
            if (lockFirstFrames > currentFrame.Max)
            {
                lockFirstFrames = 0;
            }

            do
            {
                currentFrame.Next(lib.BoolToLeftRight(forward));
            } while (currentFrame.Value < lockFirstFrames);
            updateFrameInfo();
            updateVoxelObj();//startUpdateVoxelObj(false);
        }
        public bool haveAnimation
        {
            get { return currentFrame.Max > 0; }
        }
        public bool HasSelection
        {
            get { return selectedVoxels.Voxels.Count > 0; }
        }
        public void updateFrameInfo()
        {
            if (animationFrames != null)
            {
                if (currentFrame.Max != animationFrames.Frames.Count - 1)
                {
                    currentFrame = new CirkleCounter(currentFrame.Value, 0, animationFrames.Frames.Count - 1);
                    currentFrame.Next(1);
                    currentFrame.Next(-1);
                }

                frameInfo.Visible = haveAnimation;
                frameInfo.TextString = "Frame:" + (currentFrame.Value + 1).ToString() + "/" + (currentFrame.Max + 1).ToString();
            }
        }
        protected void ExpandDrawLimits(Dimensions dir, int add)
        {
            storeUndoableAction(true);

            switch (dir)
            {
                case Dimensions.X:
                    drawLimits.Max.X += add;
                    break;
                case Dimensions.Y:
                    drawLimits.Max.Y += add;
                    break;
                case Dimensions.Z:
                    drawLimits.Max.Z += add;
                    break;

            }
            UpdateDrawLimits();
        }

        abstract protected bool viewDrawLimitGrid { get; } 
        protected bool bUpdateDrawLimits = true;
        virtual protected bool resetWhiteLines
        { get { return true; } }

        /// <returns>got a new size</returns>
        protected bool setDrawLimit(IntervalIntV3 newLimit)
        {
            if (newLimit != drawLimits)
            {
                drawLimits = newLimit;
                UpdateDrawLimits();
                updateVoxelObj();
                return true;
            }
            return false;
        }

        public void setUndoDrawLimit(IntVector3 size)
        {
            if (size != drawLimits.Size)
            {
                drawLimits.Size = size;
                animationFrames = null;
                UpdateDrawLimits();
            }
        }

        /// <summary>
        /// Make sure the limits are the same size as the vox model
        /// </summary>
        /// <returns>got a new size</returns>
        protected bool setDrawLimitFromModel()
        {
            return setDrawLimit(new IntervalIntV3(drawLimits.Min, drawLimits.Min + animationFrames.Frames[0].Limits));
        }

        protected void UpdateDrawLimits()
        {
            if (bUpdateDrawLimits)
            {
                designerInterface.SetDrawLimits(drawLimits, resetWhiteLines);

                if (viewDrawLimitGrid)
                {
                    designerInterface.createGrid(drawLimits);
                }
            }

            if (animationFrames != null)
            {
                foreach (VoxelObjGridDataHD frame in animationFrames.Frames)
                {
                    frame.Resize(drawLimits.Max + 1);
                }
            }
        }

        public void copySelectedVoxels(bool cut)
        {
            absMenuSystem.closeMenu();
            copiedVoxels = selectedVoxels.Clone();
            //används selection?
            if (cut)
            {
                selectedVoxels.Voxels.Clear();
                refreshSelectionModel();
            }
        }

        public void ShowHUD(bool show)
        {
            designerInterface.ShowHUD(show);
            //HUDelements.SetVisible(show);
            frameInfo.Visible = show && currentFrame.Max > 0;
        }

        
        void prevAndNextInput(bool next)
        {
           // bool right = dir > 0;
            if (HasSelection && !inputMap.toggleCameraMode.IsDown)
            {
                Rotate(next);
            }
            else
            {
                nextFrame(next);
               
            }
        }

        virtual protected void chagedTool(PaintFillType tool)
        { }

        Vector2 WASD = Vector2.Zero;
        void KeyDir(Dir4 dir, bool keyDown)
        {
            Vector2 vdir = conv.ToV2(dir, 1);
            if (keyDown)
                WASD += vdir;
            else
                WASD -= vdir;
            
        }
        virtual protected void NewCanvas()
        {
            animationFrames.Frames = new List<VoxelObjGridDataHD> { new VoxelObjGridDataHD(drawLimits.Size) };
            currentFrame = new CirkleCounter(0);
            updateVoxelObj();
        }
        
        
        public override void Time_Update(float time)
        {
            UpdateInput();

            selectionModel.update();
            designerInterface.Update(HasSelection, drawTools, inputMap.toggleCameraMode.IsDown);
            designerInterface.inputDisplay.update(HasSelection, undolist.Count, drawCoordMaterial.HasMaterial(), inputMap);

            Ref.draw.Camera.Time_Update(time);
        }

        virtual public void UpdateInput()
        {
            rotateCameraUpdate(inputMap.cameraRotation(absMenuSystem.InMenu, playerIndex));


            if (absMenuSystem.InMenu)
            {
                if (absMenuSystem.Update() || menuInput.openCloseInputEvent())
                {
                    absMenuSystem.closeMenu();
                }
            }
            else if (mouseToolHUD != null)
            {
                if (mouseToolHUD.update() || inputMap.mouseToolMenu.DownEvent)
                {
                    if (mouseToolHUD.selected != null)
                    {
                        inputMap.mouseTool = mouseToolHUD.selected.Value;
                    }
                    mouseToolHUD.DeleteMe();
                    mouseToolHUD = null;
                }
            }
            else
            {
                cameraZoom(inputMap.zoom());
                designerInterface.moveFreePencil(inputMap.pencilMovement(playerIndex, Settings.pencilMoveSpeed));
                
                if (inputMap.previous.DownEvent)
                { prevAndNextInput(false); }
                if (inputMap.next.DownEvent)
                { prevAndNextInput(true); }

                //Buttons
                if (HasSelection)
                {
                    if (inputMap.stampSelection())
                    {
                        drawTools.beginStampSelection(false);
                    }
                    if (inputMap.cancelSelection())
                    {
                        drawTools.beginStampSelection(true);
                    }
                    if (menuInput.openCloseInputEvent())
                    {
                        absMenuSystem.selectionMenu();
                    }

                    if (inputMap.mirrorX.DownEvent)
                    {
                        mirrorSelection();
                    }
                    if (inputMap.mirrorY.DownEvent)
                    {
                        if (inputMap.toggleCameraMode.IsDown)
                        {
                            flipLyingToStanding();
                        }
                        else
                        {
                            flip(Dimensions.Y);
                            templateSent = false;
                        }
                    }
                }
                else //No selection
                {
                    drawTools.UpdateInput(inputMap);

                    if (inputMap.colorPick.DownEvent)
                    {
                        pickColor();
                    }
                    else if (inputMap.undo.DownEvent)
                    {
                        //undo();
                        undolist.Undo(this);
                    }
                    else if (menuInput.openCloseInputEvent())
                    {
                        absMenuSystem.openMenu();
                    }

                    if (inputMap.useMouseInput)
                    {
                        if (inputMap.mouseToolMenu.DownEvent)
                        {
                            mouseToolHUD = new MouseToolHUD();
                        }
                    }
                }

                
            }
        }

        public void selectAll()
        {
            if (allowSelectAll)
            {
                absMenuSystem.closeMenu();
                drawTools.selectAll();
            }
        }

        void cameraZoom(float dir)
        {
            //const float ZoomSpeed = 0.05f;
            if (dir != 0f)
            {

                camera.targetZoom = Bound.Set(camera.targetZoom + dir * BlockScale, ZoomBounds.Min * BlockScale, ZoomBounds.Max * BlockScale);
            }
        }

        void rotateCameraUpdate(Vector2 dir)
        {
            if (Math.Abs(dir.Y) > Math.Abs(dir.X))
            {
                camera.TiltY += dir.Y;
            }
            else
            {
                camera.TiltX += dir.X;
            }
        }

        virtual protected void storeUndoableAction(bool allFrames)
        {
            this.undolist.add(new UndoAction(this, allFrames? -1 : currentFrame.Value));
        }

        virtual public void print(string text)
        { }
        protected void Rotate(bool clockWise)
        {
            byte dir = (byte)(clockWise ? 1 : 3);
            
            if (HasSelection)
            {
                IntVector3 add = designerInterface.selectionArea.Add;
                designerInterface.selectionArea.AddX = add.Z;
                designerInterface.selectionArea.AddZ = add.X;

                IntVector3 posdiff = designerInterface.selectionArea.Min - designerInterface.drawCoord;
                moveAll(-posdiff, true);


                selectedVoxels.Rotate(dir, designerInterface.selectionArea);

                templateSent = false;

                refreshSelectionModel();
            }
            else
            {
                voxels.Rotate(dir, drawLimits, true);

                UpdateVoxelObj();
            }
        }
        protected void flip(Dimensions dir)
        {
            if (HasSelection)
            {
                VoxelObjListDataHD v = selectedVoxels;
                v.FlipDir(dir, designerInterface.selectionArea);

                refreshSelectionModel();
            }
            else
            {
                VoxelObjGridDataHD v = voxels;
                v.FlipDir(dir, drawLimits, true);

                UpdateVoxelObj();
            }
        }
        virtual public void flipLyingToStanding()
        {
            if (HasSelection)
            {
                VoxelObjListDataHD v = selectedVoxels;

                IntVector3 add = designerInterface.selectionArea.Add;
                designerInterface.selectionArea.AddZ = add.Y;
                designerInterface.selectionArea.AddY = add.Z;
                v.FlipLyingToStanding(designerInterface.selectionArea);
                refreshSelectionModel();
                
                templateSent = false;
            }
        }
        virtual protected void removeSelection()
        {
            //Merge the selected group of voxels with the original group
            if (selectedVoxels.Voxels.Count > 0)
            {
                selectedVoxels.Voxels.Clear();
                startUpdateVoxelObj(false);
                refreshSelectionModel();
            }
            templateSent = false;
        }
        

        public void moveAll(IntVector3 dir, bool storeUndo)
        {
            if (dir != IntVector3.Zero)
            {
                if (HasSelection)
                {
                    selectedVoxels.Move(dir);
                    designerInterface.selectionArea.AddValue(dir);

                    moveSelectionModel(dir);
                }
                else
                {
                    if (storeUndo)
                    {
                        storeUndoableAction(repeateOnAllFrames);
                    }
                    if (repeateOnAllFrames)
                    {
                        for (int i = 0; i < animationFrames.Frames.Count; i++)
                        {
                            voxels.Move(dir, drawLimits);
                            nextFrame(true);
                        }
                    }
                    else
                    {
                        
                        voxels.Move(dir, drawLimits);
                    }

                    UpdateVoxelObj();
                }
            }
        }

        protected void dropSelection(bool threaded)
        {
            stampSelection(threaded);
            removeSelection();
        }

        virtual protected bool allowSelectAll { get { return true; } }
        
        public void AfterDraw()
        {
            
            startUpdateVoxelObj(true);

            UpdatePencilInfo();
        }

        virtual protected void pickColor()
        { }

        virtual protected bool selectionCut
        { get { return true; } }
        
        //virtual protected void drawInArea(PaintToolType fill, DrawShape tool, RangeIntV3 drawArea, bool isAsynch)
        //{
        //    designerInterface.drawSize = IntVector3.One;

        //    FillArea(SelectedMaterial, fill, designerInterface.toolDir, tool, drawArea,
        //        designerInterface.keyDownDrawCoord, selectedVoxels, selectionCut, this);

        //    if (fill == PaintToolType.Select)
        //    {
        //        designerInterface.selectionArea = drawArea;
        //    }
        //}

        //public static void FillArea(BlockHD selectedMaterial, PaintToolType fill, Dimensions toolDir, DrawShape drawTool,
        //    RangeIntV3 selectionArea, IntVector3 keyDownDrawCoord, VoxelObjListDataHD selectedVoxels, bool selectionCut, IVoxelDesigner parent)
        //{
        //    BlockHD material = selectedMaterial;
        //    if (fill == PaintToolType.Delete)
        //        material = new BlockHD();
        //    if (toolDir == Dimensions.NON)
        //        toolDir = Dimensions.Y;

        //    IntVector3 drawPoint = new IntVector3();

        //    Vector3 center = selectionArea.Center;
        //    Vector3 halfSz = selectionArea.Add.Vec * PublicConstants.Half;

        //    float height = 0;
        //    if (drawTool == DrawShape.Pyramid || drawTool == DrawShape.Cone)
        //    {
        //        center = lib.SetDimention3(center, toolDir, keyDownDrawCoord.GetDimension(toolDir));
        //        height = selectionArea.Add.GetDimension(toolDir);
        //    }
            
        //    ushort blockValue = material.BlockValue;

        //    for (drawPoint.Z = selectionArea.Min.Z; drawPoint.Z <= selectionArea.Max.Z; drawPoint.Z++)
        //    {
        //        for (drawPoint.Y = selectionArea.Min.Y; drawPoint.Y <= selectionArea.Max.Y; drawPoint.Y++)
        //        {
        //            for (drawPoint.X = selectionArea.Min.X; drawPoint.X <= selectionArea.Max.X; drawPoint.X++)
        //            {
        //                    //see if there already exists one block
        //                    if (fill == PaintToolType.Select)
        //                    {
        //                        ushort m = parent.GetVoxel(drawPoint);
        //                        if (m != BlockHD.EmptyBlock)
        //                        {
        //                            selectedVoxels.Voxels.Add(new VoxelHD(drawPoint, m));
        //                            if (selectionCut)
        //                                parent.SetVoxel(drawPoint, BlockHD.EmptyBlock);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        bool draw = true;
        //                        switch (drawTool)
        //                        {
        //                            case DrawShape.Sphere:
        //                                Vector3 diff = drawPoint.Vec - center;
        //                                diff /= halfSz;
        //                                draw = diff.Length() <= 1.1f;
        //                                break;
        //                            case DrawShape.Cylinder:
        //                                Vector3 diff2 = drawPoint.Vec - center;
        //                                diff2 /= halfSz;
        //                                diff2 = lib.SetDimention3(diff2, toolDir, 0);
        //                                draw = diff2.Length() <= 1.1f;
        //                                break;
        //                            case DrawShape.Pyramid:
        //                                Vector3 diff3 = drawPoint.Vec - center;
        //                                float radius = 1 - (Math.Abs(lib.GetDimention3(diff3, toolDir)) / height) + 0.1f;
        //                                diff3 = lib.SetDimention3(diff3, toolDir, 0);
        //                                diff3 /= halfSz;
        //                                draw = lib.GetLongestAbsV3Dimention(diff3) <= radius;
        //                                break;
        //                            case DrawShape.Cone:
        //                                Vector3 diff4 = drawPoint.Vec - center;
        //                                float radius2 = 1 - (Math.Abs(lib.GetDimention3(diff4, toolDir)) / height) + 0.1f;
        //                                diff4 = lib.SetDimention3(diff4, toolDir, 0);
        //                                diff4 /= halfSz;
        //                                draw = diff4.Length() <= radius2;
        //                                break;
        //                        }

        //                        if (draw)
        //                        {

        //                            parent.SetVoxel(drawPoint, blockValue);
        //                        }
        //                    }
        //                //}
        //            }
        //        }
        //    }
        //}

        virtual public ushort GetVoxel(IntVector3 drawPoint)
        {
            return voxels.Get(drawPoint);
        }
        virtual public void SetVoxel(IntVector3 drawPoint, ushort material)
        {
            voxels.Set(drawPoint, material);
        }

        virtual public ushort GetVoxel(LootFest.Map.WorldPosition wp)
        {
            return voxels.Get(wp.WorldGrindex);
        }
        virtual public void SetVoxel(LootFest.Map.WorldPosition wp, ushort material)
        {
            voxels.Set(wp.WorldGrindex, material);
        }
        
        public bool removeBlock(IntVector3 pos)
        {
            if (voxels.Get(pos) != BlockHD.EmptyBlock)
            {
                voxels.Set(pos, BlockHD.EmptyBlock);
                return true;
            }
            
            return false;
        }        

        
        public void SetPencilBounds(ref Vector3 freePencilGridPos)
        {
            freePencilGridPos.X = Bound.Set(freePencilGridPos.X, drawLimits.Min.X, drawLimits.Max.X);
            freePencilGridPos.Y = Bound.Set(freePencilGridPos.Y, drawLimits.Min.Y, drawLimits.Max.Y);
            freePencilGridPos.Z = Bound.Set(freePencilGridPos.Z, drawLimits.Min.Z, drawLimits.Max.Z);
        }
        
        virtual protected bool roundPencil { get { return true; } }
        

        virtual protected void lostSelectionWhenMoving()
        { }

       public BlockHD drawCoordMaterial = new BlockHD();
       abstract protected ushort Get(IntVector3 pos);

        virtual protected void UpdatePencilInfo()
        {
            int state_empty0_contact1_inside2 = 0;

            //update color
            if (drawLimits.pointInBounds(designerInterface.drawCoord))
            {
                //GetVoxel(

                    //wp = worldPos;
               // wp.WorldGrindex += designerInterface.drawCoord;

                //int state_empty0_contact1_inside2 = 0;

                if (drawCoordMaterial.HasMaterial())
                {
                    state_empty0_contact1_inside2 = 2;
                }
                else if (checkSelectionContact(designerInterface.drawCoord))
                {
                    state_empty0_contact1_inside2 = 1;
                }
                designerInterface.setPencilColor(state_empty0_contact1_inside2);

                //designerInterface.pencilCube.Color = EmptySelection;

                //if (voxels.GetSafe(designerInterface.drawCoord) == BlockHD.EmptyBlock)
                //{
                //    for (int i = 0; i < 2; i++)
                //    {
                //        int lookdir = i == 0 ? 1 : -1;

                //        IntVector3 x = designerInterface.drawCoord;
                //        x.X += lookdir;
                //        IntVector3 y = designerInterface.drawCoord;
                //        y.Y += lookdir;
                //        IntVector3 z = designerInterface.drawCoord;
                //        z.Z += lookdir;

                //        for (Dimensions d = Dimensions.X; d <= Dimensions.Z; d++)
                //        {
                //            IntVector3 sidePos = designerInterface.drawCoord;
                //            sidePos.AddDimension(d, lookdir);
                //            if (voxels.GetSafe(sidePos) != BlockHD.EmptyBlock)
                //            {
                //                //designerInterface.pencilCube.Color = ContactSelection;
                //                state_empty0_contact1_inside2 = 1;
                //                return;
                //            }
                //        }

                //    }
                //}
                //else
                //{
                //    state_empty0_contact1_inside2 = 2;
                //    //designerInterface.pencilCube.Color = InsideSelection;
                //}
            }
            else
            {
                state_empty0_contact1_inside2 = -1;
                drawCoordMaterial = BlockHD.Empty;
                //designerInterface.pencilCube.Color = Color.Red;
            }

            designerInterface.setPencilColor(state_empty0_contact1_inside2);
        }

        bool checkSelectionContact(IntVector3 pos)
        {
            for (Dimensions d = Dimensions.X; d <= Dimensions.Z; d++)
            {
                for (int dir = 0; dir < 2; dir++)
                {
                    IntVector3 npos = pos;
                    npos.AddDimension(d, lib.BoolToLeftRight(dir == 0));

                    //bool nMateriel = GetVoxel(npos) != BlockHD.EmptyBlock;
                    if (drawLimits.pointInBounds(npos) &&
                        GetVoxel(npos) != BlockHD.EmptyBlock)//wp.GetNeighborPos(pos).BlockHasMaterial())
                    {

                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdateImageAfterThread(IntervalIntV3 updateArea)
        {
            UpdateVoxelObj(updateArea);

            if (HasSelection)
            {
                onSelect();
            }
        }

        virtual protected void onSelect()
        { }

        public void startUpdateVoxelObj(bool isasynch)
        {
            this.startUpdateVoxelObj(isasynch, designerInterface.selectionArea);
        }
        public void startUpdateVoxelObj(bool isasynch, IntervalIntV3 updateArea)
        {
            if (isasynch)
            {
                Debug.CrashIfMainThread();
                new Timer.Action1ArgTrigger<IntervalIntV3>(UpdateVoxelObj, updateArea);
            }
            else
            {
                UpdateVoxelObj(updateArea);
            }
        }
       
        
       protected void UpdateVoxelObj()
       {
           UpdateVoxelObj(designerInterface.selectionArea);
       }
       protected void UpdateVoxelObj(IntervalIntV3 updateArea)
       {
            updateFrameInfo();
            if (voxelObj != null)
            {
                voxelObj.DeleteMe();
            }
            updateVoxelObj(updateArea);
       }

       protected void updateVoxelObj()
       {
           this.updateVoxelObj(designerInterface.selectionArea);
       }
       virtual public void updateVoxelObj(IntervalIntV3 updateArea)
       {
           if (voxelObj != null)
           {
               Ref.draw.RemoveFromRenderList(voxelObj);
           }

           voxelObj = LootFest.Editor.VoxelObjBuilder.BuildModelHD(new List<VoxelObjGridDataHD>{ animationFrames.Frames[currentFrame.Value] }, Vector3.Zero);
           if (voxelObj != null)
           {
               Ref.draw.AddToRenderList(voxelObj);
           }
       }


       void moveSelectionModel(IntVector3 move)
       {
           selectionModel.move(move);
           designerInterface.refreshVolumeGui();
        }
        
       public void refreshSelectionModel()
       {
           if (HasSelection)
           {
               if (designerInterface.selectionArea.Min.X < 0)
               {
                    designerInterface.selectionArea = selectedVoxels.getMinMax();
               }

                designerInterface.refreshVolumeGui();
               //designerInterface.refreshSelectionGui();
               selectionModel.refresh(selectedVoxels, drawLimits.Size, designerInterface.offSet);
           }
           else
           {
               selectionModel.clear();
                designerInterface.volumeGUI.hide();
           }
       }
       virtual protected void LargeSelectionWarning()
       {

       }

       public void stampSelection_Raw()
       {
            for (int i = 0; i < selectedVoxels.Voxels.Count; ++i)
            {
                if (drawLimits.pointInBounds(selectedVoxels.Voxels[i].Position))
                {
                    SetVoxel(selectedVoxels.Voxels[i].Position, selectedVoxels.Voxels[i].Material);
                }
            }
       }

       virtual public void stampSelection(bool startThread)
        {
            if (HasSelection)
            {
                storeUndoableAction(false);

                if (startThread)
                    new ThreadedTemplateStamp(this, selectedVoxels.Clone());
                else
                    MakeThreadedStamp(selectedVoxels, designerInterface.selectionArea);
            }
        }

        public void MakeThreadedStamp(VoxelObjListDataHD selectedVoxels, IntervalIntV3 updateArea)
        {
            voxels.SafeAddVoxels(selectedVoxels.Voxels);
        }

        virtual public void linkReplaceSelectionMaterials(ushort from)
        {
            throw new NotImplementedException();
        }

        virtual public void LinkSelSaveTemplate() {  }
        
        virtual protected void exportSelectionAsByteArray()
        {
        }

        public VoxelObjGridDataHD SelectionToGrid()
        {
            IntVector3 gridSize = designerInterface.selectionArea.Add + 1;
            var grid = new VoxelObjGridDataHD(gridSize, selectedVoxels.Voxels, -designerInterface.selectionArea.Min);
            return grid;
        }

       
        
        public void LinkSelRotateC()
        {
            rotateHeader(true);
            templateSent = false;
        }
        public void LinkSelRotateCC()
        {
            rotateHeader(false);
            templateSent = false;
        }
        public void rotateHeader(bool clockwise)
        {
            if (repeateOnAllFrames)
            {
                for (int i = 0; i < animationFrames.Frames.Count; i++)
                {
                    Rotate(clockwise);
                    nextFrame(true);
                }
            }
            else
            {
                Rotate(clockwise);
            }
        }
        public void LinkSelFlipY()
        {
            flip(Dimensions.Y);
            templateSent = false;
        }

        public void mirrorSelection()
        {
            flip(cameraInZdir() ? Dimensions.X : Dimensions.Z);
            templateSent = false;
        }
        protected bool cameraInZdir()
        {
            Vector2 dir = lib.AngleToV2(camera.TiltX, 1);
            return Math.Abs(dir.Y) < Math.Abs(dir.X);
        }
        public void Paste()
        {   
            if (haveCopiedVoxels)
            {
                absMenuSystem.closeMenu();

                selectedVoxels = copiedVoxels.Clone();
                designerInterface.UpdateMultiSelectionPencil(IntVector3.Zero);
                refreshSelectionModel();
            }
        }
        public bool bRepeateOnAllFramesProperty(int index, bool set, bool value)
        {
            if (set) { repeateOnAllFrames = value; }
            return repeateOnAllFrames;
        }

        public bool bStampEmptyProperty(int index, bool set, bool value)
        {
            if (set) { stampEmpty = value; }
            return stampEmpty;
        }
        protected bool haveCopiedVoxels
        {
            get { return copiedVoxels != null && copiedVoxels.Voxels.Count > 0; }
        }

        public VikingEngine.Voxels.VoxelDesignerSettings Settings
        {
            get { return designerInterface.settings; }
        }
    }

    enum MoveFrameType
    {
        Forward,
        Back,
        ToStart,
        ToEnd,
    }
    enum PaintToolType
    {
        Rectangle,
        Cylinder,
        Sphere,

        Triangle,
        Pyramid,
        Cone,

        Pencil,
        ReColor,
        Road,

        NUM,
    }

    enum PaintFillType
    {
        Fill,
        Select,
        Delete,
    }

    //class DrawInAreaTimer : AbsUpdateable
    //{
    //    AbsVoxelDesigner designer;
    //    RangeIntV3 selectionArea;
    //    RangeIntV3 drawLimits;
    //    PaintFillType fill;
    //    VoxelObjListData voxels;
    //    VoxelObjListData selectedVoxels;
    //    byte selectedMaterial;
    //    Graphics.Mesh pencilMultiSelection;

    //    public DrawInAreaTimer(AbsVoxelDesigner designer, RangeIntV3 selectionArea, RangeIntV3 drawLimits, PaintFillType fill, 
    //        VoxelObjListData voxels, VoxelObjListData selectedVoxels, byte selectedMaterial, Graphics.Mesh pencilMultiSelection)
    //        : base(true)
    //    {
    //         this.designer= designer;
    //         this.selectionArea= selectionArea;
    //         this.drawLimits= drawLimits;
    //         this.fill= fill;
    //         this.voxels= voxels;
    //         this.selectedVoxels= selectedVoxels;
    //         this.selectedMaterial= selectedMaterial;
    //         drawPoint.Z = selectionArea.Min.Z;
    //         drawPoint.Y = selectionArea.Min.Y;

    //         this.pencilMultiSelection = (Graphics.Mesh)pencilMultiSelection.CloneMe();
    //         this.pencilMultiSelection.Color = Color.Black;
    //         this.pencilMultiSelection.Visible = selectionArea.Add != IntVector3.Zero;

    //    }

    //    IntVector3 drawPoint = new IntVector3();
    //    public override void Time_Update(float time)
    //    {
            
    //        for (drawPoint.X = selectionArea.Min.X; drawPoint.X <= selectionArea.Max.X; drawPoint.X++)
    //        {
    //            if (drawLimits.WithinRange(drawPoint))
    //            {
    //                //see if there already exists one block
    //                if (fill == PaintFillType.Select)//e.Button == selectionButton)
    //                {
    //                    //select voxel
    //                    int v = voxels.selectedVoxel(drawPoint);
    //                    if (v != -1)
    //                    {
    //                        selectedVoxels.Voxels.Add(voxels.Voxels[v]);
    //                        designer.removeBlock(drawPoint);
    //                    }
    //                }
    //                else
    //                {
    //                    bool draw = true;

    //                    if (draw)
    //                    {
    //                        designer.removeBlock(drawPoint);
    //                        if (fill != PaintFillType.Delete)
    //                            voxels.Voxels.Add(new Voxel(drawPoint, selectedMaterial));
    //                    }
    //                }
    //            }
    //        }

    //        drawPoint.Y++;
    //        if (drawPoint.Y > selectionArea.Max.Y)
    //        {
    //            drawPoint.Y = selectionArea.Min.Y;

    //            drawPoint.Z++;
    //            if (drawPoint.Z > selectionArea.Max.Z)
    //            {
    //                DeleteMe();
    //                designer.AfterDraw();
    //            }
    //        }
    //    }
    //    public override void DeleteMe()
    //    {
    //        base.DeleteMe();
    //        pencilMultiSelection.DeleteMe();
    //    }
    //}

    
}
