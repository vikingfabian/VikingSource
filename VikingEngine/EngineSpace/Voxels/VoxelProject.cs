using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.Voxels
{

    class VoxelProject
    {
        /// <summary>
        /// All the data used and saved when working in the voxel editor
        /// </summary>

        public IntervalIntV3 drawLimits;

        public int lockFirstFrames = 0;
        public int lockEndFrames = 0;
        public CircleCounter currentFrame = new CircleCounter(0);

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

        public VoxelProject(VoxelObjGridDataAnimHD loadedModel)
        {
            this.drawLimits = new IntervalIntV3(IntVector3.Zero, loadedModel.Size - 1);
            layers = new ListWithSelection<VoxLayer>();
            addLayer(loadedModel);
        }

        public void addLayer(bool animatedLayer)
        { 
            layers.Add(new VoxLayer(drawLimits.Size, animatedLayer, currentFrame.Length), true);
        }

        public void addLayer(VoxelObjGridDataAnimHD loadedModel)
        {
            layers.Add(new VoxLayer(loadedModel), true);
        }

        public void LockAnimation(bool start)
        {
            if (start)
            {
                lockFirstFrames = currentFrame.Value;
                lockEndFrames = Bound.Min(lockEndFrames, lockFirstFrames);
            }
            else
            { 
                lockEndFrames = currentFrame.Value;
                lockFirstFrames = Bound.Max(lockFirstFrames, lockEndFrames);
            }
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

            refreshFrameCount();
            //updateFrameInfo();

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

            if (currentFrame.Length != length)
            {
                currentFrame = new CircleCounter(currentFrame.Value, 0, length - 1);
            }
        }

        public void RemoveAllFramesButThis()
        {
            foreach (var layer in layers.list)
            {
                layer.RemoveAllFramesButThis(currentFrame.Value);
            }
        }

        public bool setSize(IntVector3 size)
        {
            var limits = drawLimits;
            limits.Size = size;
            return setDrawLimit(limits);
        }

        public ListWithSelection<VoxLayer> CloneLayers()
        { 
            var clone = new ListWithSelection<VoxLayer>(layers.Count);
            foreach (var layer in layers.list)
            {
                clone.list.Add(layer.Clone());
            }

            clone.selectedIndex = layers.selectedIndex;
            return clone;
        }

        public bool setDrawLimit(IntervalIntV3 newLimit)
        {
            if (newLimit != drawLimits)
            {
                drawLimits = newLimit;
                var sz = drawLimits.Size;
                foreach (var layer in layers.list)
                {
                    layer.Resize(sz);
                }
                return true;
            }
            return false;
        }

        public void Rotate(int rotationSteps, bool allFrames, bool allLayers)
        {
            if (allLayers)
            {
                foreach (var layer in layers.list)
                {
                    layer.Rotate(rotationSteps, allFrames, currentFrame.Value);
                }
            }
            else
            { 
                layers.Selected().Rotate(rotationSteps, allFrames, currentFrame.Value);
            }
        }

        public void flip(Dimensions dir, bool allFrames, bool allLayers)
        {
            if (allLayers)
            {
                foreach (var layer in layers.list)
                {
                    layer.flip(dir, drawLimits, allFrames, currentFrame.Value);
                }
            }
            else
            {
                layers.Selected().flip(dir, drawLimits, allFrames, currentFrame.Value);
            }
        }

        public void moveAll(IntVector3 dir, bool allFrames, bool allLayers)
        {
            if (allLayers)
            {
                foreach (var layer in layers.list)
                {
                    layer.moveAll(dir, drawLimits, allFrames, currentFrame.Value);
                }
            }
            else
            {
                layers.Selected().moveAll(dir, drawLimits, allFrames, currentFrame.Value);
            }
        }

        public void BucketFill(IntVector3 pos, ushort toColor, bool continous, bool allFrames, int frame, bool allLayers, int layerIx)
        {
            ushort fromColor = CurretVoxelGrid.Get(pos);
            if (allLayers)
            {
                foreach (var layer in layers.list)
                {
                    layer.BucketFill(pos, fromColor, toColor, continous, allFrames, frame);
                }
            }
            else
            {
                if (arraylib.TryGet(layers.list, layerIx, out var layer))
                {
                    layer.BucketFill(pos, fromColor, toColor, continous, allFrames, frame);
                }
            }
            //animationFrames.BucketFill(action.keyDownDrawCoord, action.frame, action.fill == PaintFillType.Delete ? BlockHD.EmptyBlock : action.material1, action.paintSettings.continiousFill, action.allFrames, );
        }

        public bool HaveAnimation
        {
            get { return currentFrame.Max > 0; }
        }
    }

    class VoxLayer
    {
        public bool visible = true;
        public bool animatedLayer = true;
        public VoxelObjGridDataAnimHD animationFrames;

        public VoxLayer()
        { }

        public VoxLayer(IntVector3 size, bool animatedLayer, int frameCount)
        {
            this.animatedLayer = animatedLayer;
            animationFrames = new VoxelObjGridDataAnimHD(size, animatedLayer? frameCount : 1);
        }

        public VoxLayer(VoxelObjGridDataAnimHD loadedModel)
        {
            animationFrames = loadedModel;
        }

        public VoxLayer Clone()
        {
            var clone = new VoxLayer()
            {
                animatedLayer = animatedLayer,
                visible = visible,
                animationFrames = animationFrames.Clone()
            };

            return clone;
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

        public void Resize(IntVector3 size)
        {            
            foreach (VoxelObjGridDataHD frame in animationFrames.Frames)
            {
                frame.Resize(size);
            }            
        }

        public void Rotate(int rotationSteps, bool allFrames, int currentFrame)
        {
            if (!animatedLayer || allFrames)
            {
                foreach (var frame in animationFrames.Frames)
                {
                    frame.Rotate(rotationSteps, true);
                }
            }
            else
            {
                animationFrames.Frames[currentFrame].Rotate(rotationSteps, true);
            }
        }

        public void flip(Dimensions dimention, IntervalIntV3 drawLimits, bool allFrames, int currentFrame)
        {
            if (!animatedLayer || allFrames)
            {
                foreach (var frame in animationFrames.Frames)
                {
                    frame.FlipDir(dimention, drawLimits, true);
                }
            }
            else
            {
                animationFrames.Frames[currentFrame].FlipDir(dimention, drawLimits, true);
            }
        }

        public void moveAll(IntVector3 dir, IntervalIntV3 drawLimits, bool allFrames, int currentFrame)
        {
            if (!animatedLayer || allFrames)
            {
                foreach (var frame in animationFrames.Frames)
                {
                    frame.Move(dir, drawLimits);
                }
            }
            else
            {
                animationFrames.Frames[currentFrame].Move(dir, drawLimits);
            }
        }

        public void BucketFill(IntVector3 pos, ushort fromColor, ushort toColor, bool continous, bool allFrames, int currentFrame)
        {
            if (!animatedLayer || allFrames)
            {
                foreach (var frame in animationFrames.Frames)
                {
                    frame.BucketFill(pos, fromColor, toColor, continous);
                }
            }
            else
            {
                animationFrames.Frames[currentFrame].BucketFill(pos, fromColor, toColor, continous);
            }
            //animationFrames.BucketFill(action.keyDownDrawCoord, action.frame, action.fill == PaintFillType.Delete ? BlockHD.EmptyBlock : action.material1, action.paintSettings.continiousFill, action.allFrames, );
        }
    }

}
