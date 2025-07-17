using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;
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
        public string name = "VoxProj" + Ref.rnd.Int(9999).ToString();
        //public VoxelObjGridDataAnimHD mergedLayers = null;

        /// <summary>
        /// Low index = top layer = override bottom layers
        /// </summary>
        public ListWithSelection<VoxLayer> layers = new ListWithSelection<VoxLayer>(); 

        public VoxelObjGridDataHD CurrentVoxelGrid
        {
            get { return layers.Selected().GetFrame(currentFrame.Value); }
            //set { layers.Selected().animationFrames.Frames[currentFrame.Value] = value; }
        }

        public VoxelObjGridDataAnimHD AnimationFrames
        {
            get { return layers.Selected().animationFrames; }
        }

        public VoxelProject()
        { }

        public VoxelProject(IntervalIntV3 drawLimits)
        {
            this.drawLimits = drawLimits;
            layers = new ListWithSelection<VoxLayer>();
            addLayer(true, false);
            //animationFrames = new VoxelObjGridDataAnimHD();
            //animationFrames.Frames = new List<VoxelObjGridDataHD> { new VoxelObjGridDataHD(drawLimits.Max) };
        }

        public VoxelProject(VoxelObjGridDataAnimHD loadedModel)
        {
            this.drawLimits = new IntervalIntV3(IntVector3.Zero, loadedModel.Size - 1);
            layers = new ListWithSelection<VoxLayer>();
            addLayer(loadedModel);
        }

        public void replaceAllMaterialProperties(MaterialProperty toMaterial, bool allFrames, bool allLayers)
        {
            if (allLayers)
            {
                foreach (var layer in layers.list)
                {
                    layer.replaceAllMaterials(toMaterial, allFrames, currentFrame.Value);
                }
            }
            else
            {
                layers.Selected().replaceAllMaterials(toMaterial, allFrames, currentFrame.Value);
            }
        }
        
        public List<VoxLayer> LayersCopy()
        { 
             var layersCopy = new List<VoxLayer>(layers.Count);
            lock (layers.list)
            {
                layersCopy.AddRange(layers.list);
            }

            return layersCopy;
        }

        public VoxelObjGridDataAnimHD refreshMerged(bool allFrames)
        {
            //TODO try catch
            MergeModelsOption mergeOpt = new MergeModelsOption()
            {
                KeepOldGridSize = true,
                NewBlocksReplaceOld = true,
                MergeFramesOptions = MergeFramesOptions.FrameByFrame,
            };

            int frame = currentFrame.Value;
            var layersCopy = LayersCopy();
            //bool firstVisibleLayer = true;
            VoxelObjGridDataAnimHD result = null;

            for (int i = layersCopy.Count - 1; i >= 0; i--)
            {
                var layer = layersCopy[i];
                if (layer.visible)
                {
                    if (result == null)
                    {
                        if (allFrames)
                        {
                            result = layer.animationFrames.Clone();
                        }
                        else
                        {
                            result = new VoxelObjGridDataAnimHD(new List<VoxelObjGridDataHD> { layer.GetFrame(frame).Clone() });
                        }
                    }
                    else
                    {
                        if (allFrames)
                        {
                            result.Merge(layer.animationFrames, mergeOpt);
                        }
                        else
                        {
                            result.Merge(new VoxelObjGridDataAnimHD(new List<VoxelObjGridDataHD> { layer.GetFrame(frame) }), mergeOpt);
                        }
                    }
                }
            }

            if (result == null)
            {
                result = new VoxelObjGridDataAnimHD(drawLimits.Size, 1);
            }

            //mergedLayers = result;
            return result;
        }

        public void addLayer(bool animatedLayer, bool copy)
        {
            lock (layers.list)
            {

                VoxLayer layer;

                if (copy && arraylib.HasMembers(layers.list))
                {
                    layer = layers.Selected().Clone();
                }
                else
                {
                    layer = new VoxLayer(drawLimits.Size, animatedLayer, currentFrame.Length);
                }

                if (layers.Count == 0)
                {
                    layers.Add(layer, true);
                }
                else
                {
                    layers.AddBefore(layer, true);
                }
            }

            refreshFrameCount();
        }

        public void addLayer(VoxelObjGridDataAnimHD loadedModel)
        {
            lock (layers.list)
            {
                layers.Add(new VoxLayer(loadedModel), true);
            }
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

        public void convertAnimationToLayers()
        {
            var animation = layers.Pull();
            foreach (var frame in animation.animationFrames.Frames)
            {
                layers.AddAfter(new VoxLayer(frame), true);
            }
            refreshFrameCount();
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
            ushort fromColor = CurrentVoxelGrid.Get(pos);
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

        public void moveLayer(bool down)
        {
            //designer.voxelProject.layers.selectedIndex = layer;
            layers.MoveSelected(lib.BoolToLeftRight(down));
        }
        public void layerMergeDown()
        {

        }

        public void toggleLayerAnimated(int layer)
        {
            lib.Invert(ref layers.list[layer].animatedLayer);
            layers.list[layer].refreshFrameCount(currentFrame.Length);
        }

        const int Version = 1;
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);
            w.Write(layers.Count);
            foreach (var layer in layers.list)
            { 
                layer.write(w);
            }

            w.Write(layers.selectedIndex);
            w.Write(currentFrame.Value);
        }

        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
            if (version > Version) { return; }

            int layerCount = r.ReadInt32();
            for (int i = 0; i < layerCount; i++)
            {
                VoxLayer layer = new VoxLayer();
                layer.read(r, version);
                layers.list.Add(layer);
            }
            
            layers.selectedIndex = r.ReadInt32();
            refreshFrameCount();
            currentFrame.Value = r.ReadInt32();

            drawLimits.Size = CurrentVoxelGrid.Size;
        }
    }

    

}
