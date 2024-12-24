using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Input;
using VikingEngine.LootFest.Map.HDvoxel;
using Microsoft.Xna.Framework;
using System.ComponentModel.Design;
using System.Collections.Concurrent;

namespace VikingEngine.Voxels
{
    class EditorDrawTools
    {
        AbsVoxelDesigner designer;
        public DrawQueAction currentDrawAction = null;

        ConcurrentQueue<DrawQueAction> drawQue = new ConcurrentQueue<DrawQueAction>();
        DottedDrawStroke drawStroke;

        public EditorDrawTools(AbsVoxelDesigner designer)
        {
            this.designer = designer;
        }

        public void UpdateInput(EditorInputMap inputMap)
        {
            bool keydown;

            if (inputMap.useMouseInput)
            {
                if (InputLib.ChangedEvent(inputMap.mouseUseButton, out keydown))
                {
                    drawKeyDown(keydown, inputMap.mouseTool);
                }
            }
            else
            {
                if (InputLib.ChangedEvent(inputMap.draw, out keydown))
                {
                    drawKeyDown(keydown, PaintFillType.Fill);
                }
                else if (InputLib.ChangedEvent(inputMap.erase, out keydown))
                {
                    drawKeyDown(keydown, PaintFillType.Delete);
                }
                else if (InputLib.ChangedEvent(inputMap.select, out keydown))
                {
                    drawKeyDown(keydown, PaintFillType.Select);
                }
            }
        }

        public void beginStampSelection(bool drop)
        {
            DrawQueAction stamp = new DrawQueAction(drop);
            
                drawQue.Enqueue(stamp);
            
        }

        public void NewBlockPosEvent(IntVector3 newCoord, IntVector3 posDiff)
        {
            if (currentDrawAction != null)
            {
                if (paintOnKeyDown(currentDrawAction.fill) && currentDrawAction.type == DrawQueType.Dot)
                {
                    //more than one block is selected
                    //tool that dots forward
                    beginPaintDot(designer.designerInterface.drawCoord, currentDrawAction.fill);
                }
                else
                {
                    designer.designerInterface.UpdateMultiSelectionPencil(posDiff);
                }
            }
        }

        public void update_asynch()
        {
            while (drawQue.TryDequeue(out var action))
            {
                //var action = drawQue[0];
                //lock (drawQue)
                //{
                //    drawQue.RemoveAt(0);
                //}

                if (action != null)
                {
                    IntervalIntV3 volume = action.volume;
                   // return;

                    switch (action.type)
                    {
                        case DrawQueType.FillVolume:
                            designer.undolist.add(new UndoAction(volume, designer, action.frame));
                            action.fillArea(designer);
                            designer.startUpdateVoxelObj(true);
                            designer.refreshSelectionModel();
                            break;
                        case DrawQueType.Dot:
                            float radius, edgeRadius;
                            volume = PaintDotVolume(action.keyDownDrawCoord, designer.drawLimits, action.shape,
                                action.material1 != BlockHD.EmptyBlock, action.pencilSize, action.roadEdge, out radius, out edgeRadius);

                            if (drawStroke == null)
                            {
                                drawStroke = new DottedDrawStroke();
                            }
                            drawStroke.add(volume, designer);

                            PaintDot(designer, action.pencilSize, action.radiusTolerance, action.roadEdge, action.keyDownDrawCoord, designer.drawLimits,
                                action.shape, action.fill == PaintFillType.Fill, action.material1, action.material2, action.roundPencil, action.roadPercentFill,
                                action.roadPercentFill, action.roadAboveClear);
                            designer.startUpdateVoxelObj(true, volume);
                            break;
                        case DrawQueType.EndDot:
                            volume = drawStroke.volume;
                            designer.undolist.add(new UndoAction(volume, drawStroke.undoData, action.frame));
                            drawStroke = null;
                            break;
                        case DrawQueType.StampSelection:
                            volume = designer.selectedVoxels.getMinMax();
                            volume = designer.drawLimits.keepValueInMyBounds(volume);
                            designer.undolist.add(new UndoAction(volume, designer, action.frame));
                            designer.stampSelection_Raw();
                            designer.startUpdateVoxelObj(true);
                            if (action.dropSelection)
                            {
                                designer.selectedVoxels.Voxels.Clear();
                                //designer.designerInterface.selectedVolumeLine.Visible = false;
                            }
                            designer.refreshSelectionModel();
                            break;
                    }

                    if (designer.inGameEditor)
                    {
                        volume.AddValue(designer.worldPos.WorldGrindex);
                        NetWriteVoxelEdit(volume);
                    }
                }
            }
        }

        public static void NetWriteVoxelEdit(IntervalIntV3 volume)
        {
            SteamWrapping.SteamWriter packet;
            var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.VoxelEdit, Network.PacketReliability.ReliableLasy, out packet);
            volume.Min.WriteUshortStream(w);
            volume.Size.WriteByteStream(w);

            LootFest.Map.WorldPosition pos = LootFest.Map.WorldPosition.EmptyPos;
            ForXYZLoop loop = new ForXYZLoop(volume);
            while (loop.Next())
            {
                pos.WorldGrindex = loop.Position;
                w.Write(pos.GetBlock());
            }

            packet.EndWrite_Asynch();
        }

        public static IntervalIntV3 NetReadVoxelEditVolume(Network.ReceivedPacket packet)
        {
            IntervalIntV3 volume = IntervalIntV3.Zero;
            volume.Min.ReadUshortStream(packet.r);
            volume.Max.ReadByteStream(packet.r);
            volume.Max += volume.Min - 1;

            return volume;
        }

        public static void NetReadVoxelEdit(Network.ReceivedPacket packet, IntervalIntV3 volume)
        {
            LootFest.Map.WorldPosition pos = LootFest.Map.WorldPosition.EmptyPos;
            ForXYZLoop loop = new ForXYZLoop(volume);
            while (loop.Next())
            {
                pos.WorldGrindex = loop.Position;
                ushort block = packet.r.ReadUInt16();
                pos.SetBlock_IfOpen(block);
            }
        }

        protected void drawKeyDown(bool keyDown, PaintFillType fillType)
        {
            if (keyDown)
            {
                if (paintOnKeyDown(fillType))
                {
                    beginPaintDot(designer.designerInterface.drawCoord, fillType);
                }
                else
                {
                    designer.designerInterface.toolDir = Dimensions.NON;
                    designer.designerInterface.keyDownDrawCoord = designer.designerInterface.drawCoord;
                    designer.designerInterface.selectionArea = new IntervalIntV3(designer.designerInterface.drawCoord, designer.designerInterface.drawCoord);
                    designer.selectedVoxels.Voxels.Clear();

                    currentDrawAction = new DrawQueAction(
                        designer.SelectedMaterial.BlockValue, 
                        fillType,
                        designer.Settings.DrawTool,
                        designer.designerInterface.keyDownDrawCoord,
                        designer.currentFrame.Value);
                }
            }
            else if (currentDrawAction != null) //Key UP
            {
                //draw
                designer.designerInterface.volumeGUI.hide();

                if (paintOnKeyDown(fillType))
                {
                    currentDrawAction = new DrawQueAction(DrawQueType.EndDot);
                    currentDrawAction.frame = designer.currentFrame.Value;

                    //lock (drawQue)
                    //{
                        drawQue.Enqueue(currentDrawAction);
                    //}
                    currentDrawAction = null;
                }
                else
                {
                    currentDrawAction.endDraw(designer.designerInterface.selectionArea, 
                        designer.designerInterface.toolDir, designer.designerInterface.mostRecentMoveXZ);
                    
                    
                        drawQue.Enqueue(currentDrawAction);
                    
                    currentDrawAction = null;
                }
                designer.designerInterface.drawSize = IntVector3.One;
            }
        }

        void beginPaintDot(IntVector3 pos, PaintFillType fill)
        {
            VoxelDesignerSettings sett = designer.Settings;
            currentDrawAction = new DrawQueAction(designer.SelectedMaterial.BlockValue, designer.SecondaryMaterial.BlockValue, fill,
                pos, sett.DrawTool, sett.PencilSize, sett.RadiusTolerance, sett.RoadEdgeSize, sett.RoundPencil, sett.RoadPercentFill, sett.RoadBelowFill, sett.RoadUpwardClear);
            
                drawQue.Enqueue(currentDrawAction);
            
        }

        protected void drawInArea(PaintFillType fill, PaintToolType tool, IntervalIntV3 drawArea, bool isAsynch)
        {
            switch (fill)
            {
                default:
                    VikingEngine.LootFest.Music.SoundManager.PlayFlatSound(LoadedSound.block_place_1);
                    break;
                case PaintFillType.Delete:
                    VikingEngine.LootFest.Music.SoundManager.PlayFlatSound(LoadedSound.tool_dig);
                    break;
                case PaintFillType.Select:
                    VikingEngine.LootFest.Music.SoundManager.PlayFlatSound(LoadedSound.tool_select);
                    break;

            }
        }

         

        public static void FillArea(DrawQueAction drawAction, VoxelObjListDataHD selectedVoxels, bool selectionCut, IVoxelDesigner parent)
        //ushort blockValue, PaintToolType fill, Dimensions toolDir, DrawShape drawTool,
        //RangeIntV3 selectionArea, IntVector3 keyDownDrawCoord, VoxelObjListDataHD selectedVoxels, bool selectionCut, IVoxelDesigner parent)
        {

            ushort blockValue = drawAction.material1;

            if (drawAction.fill == PaintFillType.Delete)
                blockValue = BlockHD.EmptyBlock;

            if (drawAction.toolDir == Dimensions.NON)
                drawAction.toolDir = Dimensions.Y;

            IntVector3 drawPoint = new IntVector3();

            Vector3 center = drawAction.volume.Center;
            Vector3 halfSz = drawAction.volume.Add.Vec * PublicConstants.Half;

            float height = 0;
            if (drawAction.shape == PaintToolType.Pyramid || drawAction.shape == PaintToolType.Cone)
            {
                center = VectorExt.SetDim(center, drawAction.toolDir, drawAction.keyDownDrawCoord.GetDimension(drawAction.toolDir));
                height = drawAction.volume.Add.GetDimension(drawAction.toolDir);
            }

            IntVector3 volumeSz = drawAction.volume.Size;

            for (drawPoint.Z = drawAction.volume.Min.Z; drawPoint.Z <= drawAction.volume.Max.Z; drawPoint.Z++)
            {
                for (drawPoint.Y = drawAction.volume.Min.Y; drawPoint.Y <= drawAction.volume.Max.Y; drawPoint.Y++)
                {
                    for (drawPoint.X = drawAction.volume.Min.X; drawPoint.X <= drawAction.volume.Max.X; drawPoint.X++)
                    {
                        //see if there already exists one block
                        if (drawAction.fill == PaintFillType.Select)
                        {
                            ushort m = parent.GetVoxel(drawPoint);
                            if (m != BlockHD.EmptyBlock)
                            {
                                selectedVoxels.Voxels.Add(new VoxelHD(drawPoint, m));
                                if (selectionCut)
                                    parent.SetVoxel(drawPoint, BlockHD.EmptyBlock);
                            }
                        }
                        else
                        {
                            bool draw = true;
                            switch (drawAction.shape)
                            {
                                case PaintToolType.Sphere:
                                    Vector3 diff = drawPoint.Vec - center;
                                    diff /= halfSz;
                                    draw = diff.Length() <= 1.1f;
                                    break;
                                case PaintToolType.Cylinder:
                                    Vector3 diff2 = drawPoint.Vec - center;
                                    diff2 /= halfSz;
                                    diff2 = VectorExt.SetDim(diff2, drawAction.toolDir, 0);
                                    draw = diff2.Length() <= 1.1f;
                                    break;
                                case PaintToolType.Pyramid:
                                    Vector3 diff3 = drawPoint.Vec - center;
                                    float radius = 1 - (Math.Abs(VectorExt.GetDim(diff3, drawAction.toolDir)) / height) + 0.1f;
                                    diff3 = VectorExt.SetDim(diff3, drawAction.toolDir, 0);
                                    diff3 /= halfSz;
                                    draw = VectorExt.SideLength(diff3) <= radius;
                                    break;
                                case PaintToolType.Cone:
                                    Vector3 diff4 = drawPoint.Vec - center;
                                    float radius2 = 1 - (Math.Abs(VectorExt.GetDim(diff4, drawAction.toolDir)) / height) + 0.1f;
                                    diff4 = VectorExt.SetDim(diff4, drawAction.toolDir, 0);
                                    diff4 /= halfSz;
                                    draw = diff4.Length() <= radius2;
                                    break;
                                case PaintToolType.Triangle:
                                    double percentPos = 1.0;
                                    if (drawAction.mostRecentMoveXZ.X != 0)
                                    {
                                        percentPos = (double)(drawPoint.X - drawAction.volume.Min.X) / volumeSz.X;
                                        if (drawAction.mostRecentMoveXZ.X < 0)
                                        {
                                            percentPos = 1.0 - percentPos;
                                        }
                                    }
                                    else if (drawAction.mostRecentMoveXZ.Z != 0)
                                    {
                                        percentPos = (double)(drawPoint.Z - drawAction.volume.Min.Z) / volumeSz.Z;
                                        if (drawAction.mostRecentMoveXZ.Z < 0)
                                        {
                                            percentPos = 1.0 - percentPos;
                                        }
                                    }

                                    int maxY = drawAction.volume.Min.Y + (int)(percentPos * volumeSz.Y - 0.5);
                                    draw = drawPoint.Y <= maxY;
                                    break;
                            }

                            if (draw)
                            {
#if LOOTFEST
                                ++LootFest.LfRef.stats.editorBlocksPlaced.value;
#endif
                                parent.SetVoxel(drawPoint, blockValue);
                            }
                        }
                        //}
                    }
                }
            }
        }

        //RangeIntV3 paintDot(IntVector3 center, IntVector3 posDiff, bool add)
        //{
        //    return dottedLineMovement.Add(posDiff.SByteVector);
        //    //new ThreadedDrawAcion(ThreadedActionType.DottedLine, DrawTool, this, new RangeIntV3(center, IntVector3.Zero), add ? FillType.Fill : FillType.Delete, true);
        //}
        static IntervalIntV3 PaintDotVolume(IntVector3 center, IntervalIntV3 drawLimits, PaintToolType drawTool, bool add, 
            int pencilSize, int roadEdge,
            out float radius, out float edgeRadius)
        {
            radius = pencilSize * 0.5f;
            edgeRadius = radius + roadEdge;
            IntervalIntV3 selectionArea = new IntervalIntV3(center, 
                (int)Math.Ceiling((drawTool == PaintToolType.Pencil || !add) ? radius : edgeRadius));
            selectionArea = drawLimits.keepValueInMyBounds(selectionArea);
            //selectionArea.Min = drawLimits.keepValueInMyBounds(selectionArea.Min, false);
            //selectionArea.Max = drawLimits.keepValueInMyBounds(selectionArea.Max, false);

            return selectionArea;
        }

        public static IntervalIntV3 PaintDot(AbsVoxelDesigner voxels, int pencilSize, float radiusTolerance, int roadEdge,
            IntVector3 center, IntervalIntV3 drawLimits, PaintToolType drawTool, 
            bool add, ushort material1, ushort material2, bool roundPencil, int roadPercentFill, int roadBelowFill, int roadAboveClear)
        {
            
            //const float CirkleRadius = 1.05f;
            //float radius = pencilSize * 0.5f;
            //float edgeRadius = radius + roadEdge;
            //RangeIntV3 selectionArea = new RangeIntV3(center, 
            //    (int)Math.Ceiling((drawTool == DrawTool.Pencil || !add) ? radius : edgeRadius));
            //selectionArea.Min = drawLimits.SetBounds(selectionArea.Min, false);
            //selectionArea.Max = drawLimits.SetBounds(selectionArea.Max, false);
            // = material1 != BlockHD.Empty;
            float radius, edgeRadius;

            var selectionArea = PaintDotVolume(center, drawLimits, drawTool, add, pencilSize, roadEdge, out radius, out edgeRadius);
            radius += radiusTolerance;
            edgeRadius += radiusTolerance;
            //LootFest.Map.WorldPosition wpCopy;

            if (drawTool == PaintToolType.Road)
            {
                //flat the area above and fill below
                int chance = roadPercentFill;
                selectionArea.Min.Y = Bound.Min(center.Y - roadBelowFill, drawLimits.Min.Y);
                selectionArea.Max.Y = center.Y;
                //IntVector3 seedPos;
                bool notBottom = center.Y > drawLimits.Min.Y;
                IntVector2 intPlaneCenter = VectorExt.V3XZtoV2(center);
                Vector2 planeCenter = VectorExt.V3XZtoV2(center.Vec);

                ForXYLoop loop = new ForXYLoop(VectorExt.V3XZtoV2(selectionArea.Min), VectorExt.V3XZtoV2(selectionArea.Max));
                if (!roundPencil)
                {
                    edgeRadius -= PublicConstants.Half;
                }

                while (!loop.Done)
                {
                    IntVector2 planePos = loop.Next_Old();
                    IntVector3 pos = VectorExt.V2toV3XZ(planePos, center.Y);
                    bool draw = true;
                    ushort material = material1;
                    if (roundPencil)
                    {
                        float length = (planeCenter - planePos.Vec).Length();
                        if (length <= radius)/// radius <= CirkleRadius)
                        { }
                        else if (length <= edgeRadius) //<= CirkleRadi)
                        {
                            draw = drawRoadEdge(voxels, material1, notBottom, pos);
                            material = material2;
                        }
                        else
                        {
                            draw = false;
                        }
                    }
                    else
                    {
                        int length = (intPlaneCenter - planePos).SideLength();
                        if (length >= edgeRadius)
                        {
                            draw = drawRoadEdge(voxels, material1, notBottom, pos);
                            material = material2;
                        }
                    }

                    if (draw)
                    {

                        //seedPos = wp.WorldGrindex + pos;
                        //Random rnd = new Random(pos.X * 3 + pos.Y * 11 + pos.Z * 17);
                        if (add && Ref.rnd.Chance(chance))
                        {
                            for (pos.Y = selectionArea.Min.Y; pos.Y <= selectionArea.Max.Y; pos.Y++)
                            {
                                //wpCopy = wp;
                                //wpCopy.WorldGrindex += pos;
                                voxels.SetVoxel(pos, material);
                            }
                        }
                        for (pos.Y = Bound.Max(center.Y + roadAboveClear, drawLimits.Max.Y); pos.Y > selectionArea.Max.Y; pos.Y--)
                        {
                            //wpCopy = wp;
                            //wpCopy.WorldGrindex += pos;
                            voxels.SetVoxel(pos, BlockHD.EmptyBlock);
                        }
                    }
                }
            }
            else
            {
                ushort material = add ? material1 : BlockHD.EmptyBlock;
                Vector3 v3Center = center.Vec;
                ForXYZLoop loop = new ForXYZLoop(selectionArea);
                while (!loop.Done)
                {
                    IntVector3 pos = loop.Next_Old();

                    if (!roundPencil ||
                        ((v3Center - pos.Vec).Length() <= radius))/// radius) <= CirkleRadius)
                    {
                        //wpCopy = wp;
                        //wpCopy.WorldGrindex += pos;
                        if (drawTool == PaintToolType.Pencil ||
                            voxels.GetVoxel(pos) != BlockHD.EmptyBlock)//.HasMaterial)
                        {
                            voxels.SetVoxel(pos, material);
                        }
                    }
                }
            }
            return selectionArea;
        }

        //void dottetLineKeyUp(bool add)
        //{
        //    dottedLineMovement.Clear();
        //}

        static bool drawRoadEdge(AbsVoxelDesigner voxels, ushort selectedMaterial, bool notBottom, IntVector3 pos)
        {
            bool draw = voxels.GetVoxel(pos) != selectedMaterial;
            if (notBottom)
            {
                pos.Y--;
                draw = draw && voxels.GetVoxel(pos) != selectedMaterial;
            }
            return draw;
        }

        bool paintOnKeyDown(PaintFillType fillType)
        {
            if (fillType == PaintFillType.Select) 
                return false;

            PaintToolType tool = designer.Settings.DrawTool;

            return tool == PaintToolType.Pencil ||
                tool == PaintToolType.Road ||
                tool == PaintToolType.ReColor;
        }

        public void selectAll()
        {
            designer.designerInterface.selectionArea = designer.drawLimits;
            designer.designerInterface.UpdateMultiSelectionPencil(IntVector3.Zero);
            //select all
            DrawQueAction selectAction = new DrawQueAction(
                        designer.SelectedMaterial.BlockValue,
                        PaintFillType.Select,
                        PaintToolType.Rectangle,
                        IntVector3.Zero,
                        designer.currentFrame.Value);
           
            selectAction.endDraw(designer.drawLimits,Dimensions.NON, IntVector3.Zero);

            drawQue.Enqueue(selectAction);
            

            //gotPencilKeyDown = true;
            //selectionArea = drawLimits;
            //drawTools.fillType = FillType.Select;
            //drawKeyDown(false, fillType);


        }
    }
}
