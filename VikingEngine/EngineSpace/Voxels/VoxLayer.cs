using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.Voxels;

namespace VikingEngine.Voxels
{
    class VoxLayer
    {
        public string name = null;
        public bool visible = true;
        public bool animatedLayer = true;
        public VoxelObjGridDataAnimHD animationFrames;

        public void write(System.IO.BinaryWriter w)
        {
            StreamLib.WriteString(w, name);
            w.Write(visible);
            w.Write(animatedLayer);
            animationFrames.WriteBinaryStream(w);
        }
        public void read(System.IO.BinaryReader r, int version)
        {
            name = StreamLib.ReadString(r);
            visible = r.ReadBoolean();
            animatedLayer = r.ReadBoolean();
            animationFrames = new VoxelObjGridDataAnimHD();
            animationFrames.ReadBinaryStream(r);
        }

        public VoxLayer()
        { }

        public VoxLayer(IntVector3 size, bool animatedLayer, int frameCount)
        {
            this.animatedLayer = animatedLayer;
            animationFrames = new VoxelObjGridDataAnimHD(size, animatedLayer ? frameCount : 1);
        }

        public VoxLayer(VoxelObjGridDataHD loadedModel)
        {
            animationFrames = new VoxelObjGridDataAnimHD(new List<VoxelObjGridDataHD> { loadedModel });
        }

        public VoxLayer(VoxelObjGridDataAnimHD loadedModel)
        {
            animationFrames = loadedModel;
        }

        public void replaceAllMaterials(MaterialProperty toMaterial, bool allFrames, int currentFrame)
        {
            if (!animatedLayer || allFrames)
            {
                foreach (var frame in animationFrames.Frames)
                {
                    frame.SetMaterialProperty(toMaterial);
                }
            }
            else
            {
                animationFrames.Frames[currentFrame].SetMaterialProperty(toMaterial);
            }
        }

        public VoxLayer Clone()
        {
            var clone = new VoxLayer()
            {
                animatedLayer = animatedLayer,
                visible = visible,
                animationFrames = animationFrames.Clone()
            };

            if (!string.IsNullOrEmpty(name))
            {
                clone.name = name + "_c";
            }

            return clone;
        }

        public string Name(int layerIx)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Format(DssRef.lang.Editor_LayerNumber, TextLib.IndexToString(layerIx));
            }
            else
            {
                return name;
            }
        }

        public VoxelObjGridDataHD GetFrame(int frame)
        {
            if (animatedLayer)
            {
                return animationFrames.Frames[frame];
            }
            else
            {
                return animationFrames.Frames.First();
            }
        }

        public void SetFrame(int frame, VoxelObjGridDataHD grid)
        {
            if (animatedLayer)
            {
                animationFrames.Frames[frame] = grid;
            }
            else
            {
                animationFrames.Frames[0] = grid;
            }
        }

        public void refreshFrameCount(int frameCount)
        {
            if (animatedLayer)
            {
                while (animationFrames.Frames.Count < frameCount)
                {
                    AddFrame(animationFrames.Frames.Count - 1, true);
                }
            }
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

        public void merge_recieve(VoxLayer topLayer, int currentFrame)
        {
            if (!topLayer.animatedLayer && !animatedLayer)
            {
                //just merge one frame
                animationFrames.Frames[0].Merge(topLayer.GetFrame(currentFrame), true, true, IntVector3.Zero);
            }
            else
            {
                animatedLayer = true;
                int animationLength = Math.Max(animationFrames.Frames.Count, topLayer.animationFrames.Frames.Count);

                refreshFrameCount(animationLength);

                for (int frame = 0; frame < animationLength; frame++)
                {
                    animationFrames.Frames[frame].Merge(topLayer.GetFrame(frame), true, true, IntVector3.Zero);
                }
            }
        }

        
    }
}
