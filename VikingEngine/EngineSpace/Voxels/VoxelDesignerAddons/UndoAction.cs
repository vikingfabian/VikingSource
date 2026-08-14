using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars;
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
                designer.UpdateDrawLimits();
                designer.updateVoxelObj(IntervalIntV3.Zero);
                designer.updateFrameInfo();
                
                //if (designer.inGameEditor)
                //{
                //    undo.selectionArea.AddValue(designer.worldPos.WorldGrindex);
                //    EditorDrawTools.NetWriteVoxelEdit(undo.selectionArea);
                //}

                designer.print(DssRef.lang.Hud_Undo + " " + undoActions.Count.ToString());
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
        public int frame;
        public int layer;
        VoxelObjGridDataAnimHD allFrames = null;
        VoxelObjGridDataHD oneFrame = null;
        ListWithSelection<VoxLayer> layers = null;

        public UndoAction()
        { }
        public UndoAction(AbsVoxelDesigner designer, int frame, int layer)
        {
            this.frame = frame;
            this.layer = layer;

            if (layer < 0)
            {
                layers = designer.voxelProject.CloneLayers();
            }
            else if (frame < 0)
            {
                allFrames = designer.voxelProject.AnimationFrames.Clone();
            }
            else
            {
                oneFrame = designer.voxelProject.layers.Selected().GetFrame(frame).Clone();
            }
        }

        public UndoAction(VoxelObjGridDataHD grid, int frame)
        {
            this.frame = frame;
            oneFrame = grid.Clone();
        }

        public void Undo(AbsVoxelDesigner designer)
        {
            if (layers != null)
            {
                designer.voxelProject.layers = layers;
            }
            else if (oneFrame != null)
            {
                designer.voxelProject.layers.Selected().SetFrame(frame, oneFrame);
            }
            else
            {
                designer.setUndoDrawLimit(allFrames.Frames.First().Size);
                designer.voxelProject.layers.list[layer].animationFrames = allFrames;
            }
            designer.voxelProject.drawLimits.Size = designer.voxelProject.layers.First.animationFrames.Size;
            designer.voxelProject.currentFrame.Value = frame;
        }
        
    }
    
}
