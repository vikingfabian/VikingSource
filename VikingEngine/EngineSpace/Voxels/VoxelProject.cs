using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.Voxels
{

    class VoxelProject
    {
        /// <summary>
        /// All the data used and saved when working in the voxel editor
        /// </summary>

        public IntervalIntV3 drawLimits;

        public int lockFirstFrames = 0;
        public CirkleCounter currentFrame = new CirkleCounter(0);

        /// <summary>
        /// Low index = top layer = override bottom layers
        /// </summary>
        public ListWithSelection<VoxLayer> layers = new ListWithSelection<VoxLayer>(); 

        public VoxelObjGridDataHD CurretVoxelGrid
        {
            get { return layers.Selected().animationFrames.Frames[currentFrame.Value]; }
            //set { layers.Selected().animationFrames.Frames[currentFrame.Value] = value; }
        }

        public VoxelObjGridDataAnimHD AnimationFrames
        {
            get { return layers.Selected().animationFrames; }
        }

        public VoxelProject(IntervalIntV3 drawLimits)
        {
            this.drawLimits = drawLimits;
            layers = new ListWithSelection<VoxLayer>();
            addLayer(true);
            //animationFrames = new VoxelObjGridDataAnimHD();
            //animationFrames.Frames = new List<VoxelObjGridDataHD> { new VoxelObjGridDataHD(drawLimits.Max) };
        }

        public void addLayer(bool animatedLayer)
        { 
            layers.Add(new VoxLayer(drawLimits.Max, true, currentFrame.Length), animatedLayer);
        }

        public bool RemoveCurrentFrame()
        {
            if (HaveAnimation)
            {
                foreach (var layer in layers.list)
                {
                    layer.RemoveCurrentFrame(currentFrame.Value);
                }

                return true;
            }
            return false;
        }

        public void moveFrame(MoveFrameType type)
        {
            int fromIx = currentFrame.Value;
            
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

            int toIx = currentFrame.Value;
            //animationFrames.Frames.Insert(currentFrame.Value, current);
            //updateFrameInfo();

            foreach (var layer in layers.list)
            {
                layer.moveFrame(fromIx, toIx);
            }
        }
        public void AddFrame(bool copy)
        {
            foreach (var layer in layers.list)
            {
                layer.AddFrame(currentFrame.Value, copy);
            }
            //VoxelObjGridDataHD newFrame;
            //if (copy)
            //{
            //    newFrame = animationFrames.Frames[currentFrame.Value].Clone();
            //}
            //else
            //{
            //    newFrame = new VoxelObjGridDataHD(animationFrames.Size);
            //}
            int frame = currentFrame.Value + 1;
            

            updateFrameInfo();

            currentFrame.Value = frame;
            //print("Frame Added");
        }

        public void refreshFrameCount()
        {
            int length = 1;
            foreach (var layer in layers.list)
            {
                if (layer.animationFrames.Frames.Count > length)
                {
                    length = layer.animationFrames.Frames.Count;
                    break;
                }
            }

            if (currentFrame.Max != animationFrames.Frames.Count - 1)
            {
                currentFrame = new CirkleCounter(currentFrame.Value, 0, animationFrames.Frames.Count - 1);
                currentFrame.Next(1);
                currentFrame.Next(-1);
            }
        }

        public void RemoveAllFramesButThis()
        {
            foreach (var layer in layers.list)
            {
                layer.RemoveAllFramesButThis(currentFrame.Value);
            }
        }

        public bool HaveAnimation
        {
            get { return currentFrame.Max > 0; }
        }
    }

    class VoxLayer
    {
        public bool animatedLayer;
        public VoxelObjGridDataAnimHD animationFrames;

        public VoxLayer(IntVector3 size, bool animatedLayer, int frameCount)
        {
            this.animatedLayer = animatedLayer;
            animationFrames = new VoxelObjGridDataAnimHD(size, animatedLayer? frameCount : 1);
        }

        public void RemoveCurrentFrame(int index)
        {
            if (animatedLayer)
            {
                animationFrames.Frames.RemoveAt(index);
            }
        }

        public void moveFrame(int fromIx, int toIx)
        {
            if (animatedLayer)
            {
                VoxelObjGridDataHD current = arraylib.Pull(animationFrames.Frames, fromIx);
                animationFrames.Frames.Insert(toIx, current);
            }
        }

        public void AddFrame(int currentFrame, bool copy)
        {
            if (animatedLayer)
            {
                VoxelObjGridDataHD newFrame;
                if (copy)
                {
                    newFrame = animationFrames.Frames[currentFrame].Clone();
                }
                else
                {
                    newFrame = new VoxelObjGridDataHD(animationFrames.Size);
                }

                animationFrames.Frames.Insert(currentFrame + 1, newFrame);
            }
            //print("Frame Added");
        }

        public void RemoveAllFramesButThis(int keepIx)
        {
            if (animatedLayer)
            {
                var keep = animationFrames.Frames[keepIx];
                animationFrames.Frames.Clear();
                animationFrames.Frames.Add(keep);
            }
        }
    }
}
