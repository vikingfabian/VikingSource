﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Voxels;

namespace VikingEngine.Voxels
{
    class UndoList
    {
        List<Voxels.UndoAction> undoActions = new List<Voxels.UndoAction>();

        public void add(Voxels.UndoAction undo)
        {
            lock (undoActions)
            {
                undoActions.Add(undo);

                const int MaxUndo = 10;
                if (undoActions.Count > MaxUndo)
                {
                    undoActions.RemoveAt(0);
                }
            }
        }

        public bool Undo(AbsVoxelDesigner designer)
        {
            if (undoActions.Count > 0)
            {
                UndoAction undo;
                lock (undoActions)
                {
                    undo = arraylib.PullLastMember(undoActions);
                }

                undo.Undo(designer);
                designer.updateVoxelObj(IntervalIntV3.Zero);
                designer.updateFrameInfo();
                //if (designer.inGameEditor)
                //{
                //    undo.selectionArea.AddValue(designer.worldPos.WorldGrindex);
                //    EditorDrawTools.NetWriteVoxelEdit(undo.selectionArea);
                //}

                designer.print("Undo " + undoActions.Count.ToString());
                return true;
            }
            else
            {
                LootFest.Music.SoundManager.PlayFlatSound(LoadedSound.out_of_ammo);
                return false;
            }
        }

        public int Count { get { return undoActions.Count; } }
    }

    class UndoAction
    {
        //List<ushort> gridCompressed;
        //public IntervalIntV3 selectionArea;
        public int frame;
        VoxelObjGridDataAnimHD allFrames = null;
        VoxelObjGridDataHD oneFrame = null;

        public UndoAction()
        { }
        public UndoAction(/*IntervalIntV3 selectionArea, */AbsVoxelDesigner designer, int frame)
        {
            this.frame = frame;
            //this.selectionArea = selectionArea;

            if (frame < 0)
            {
                allFrames = designer.animationFrames.Clone();
            }
            else
            {
                oneFrame = designer.animationFrames.Frames[frame].Clone();
            }
            

            //var grid = new VoxelObjGridDataHD(selectionArea.Size);

            //ForXYZLoop loop = new ForXYZLoop(grid.Size);
            //while (loop.Next())
            //{
            //    grid.Set(loop.Position, designer.GetVoxel(loop.Position + selectionArea.Min));
            //}

            //gridCompressed = new List<ushort>();
            //VoxelLib.CompressToList(grid.MaterialGrid, gridCompressed);
        }

        public UndoAction(VoxelObjGridDataHD grid, int frame)
        {
            this.frame = frame;
            oneFrame = grid.Clone();
            //this.selectionArea = selectionArea;

            //gridCompressed = new List<ushort>();
            //VoxelLib.CompressToList(grid.MaterialGrid, gridCompressed);
        }

        public void Undo(AbsVoxelDesigner designer)
        {
           

            if (oneFrame != null)
            {
                designer.animationFrames.Frames[frame] = oneFrame;
            }
            else
            {
                designer.setUndoDrawLimit(allFrames.Frames.First().Size);
                designer.animationFrames = allFrames;
            }

            designer.currentFrame.Value = frame;

            //var grid = new VoxelObjGridDataHD(selectionArea.Size);
            //VoxelLib.DeCompressList(gridCompressed, grid.MaterialGrid);

            //ForXYZLoop loop = new ForXYZLoop(grid.Size);
            //while (loop.Next())
            //{
            //    designer.SetVoxel(loop.Position + selectionArea.Min, grid.Get(loop.Position));
            //}
        }
        
    }
    
}
