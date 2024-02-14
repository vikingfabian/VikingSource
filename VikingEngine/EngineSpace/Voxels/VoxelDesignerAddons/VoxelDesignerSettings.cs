using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map.HDvoxel;
using Microsoft.Xna.Framework;

namespace VikingEngine.Voxels
{
    class VoxelDesignerSettings
    {
        public BlockHD Material = new BlockHD(Color.LightBlue, MaterialProperty.Unknown);//new BlockHD(Color.Red);
        public BlockHD SecondaryMaterial = new BlockHD(Color.Yellow, MaterialProperty.Unknown);//new BlockHD(Color.Yellow);
        public bool SelectionCut = true;
        public bool ShowDrawCoord = true;
        public bool DrawFilled = true;//N
        public PaintToolType DrawTool = PaintToolType.Rectangle;
        public int PencilSize = 3;
        public float RadiusTolerance = 0.05f;
        public bool RoundPencil = true;
        public int RoadEdgeSize = 1;
        public int RoadPercentFill = 100;
        public int RoadUpwardClear = 4;
        public int RoadBelowFill = 2;
        public float pencilMoveSpeed = 1;

        public void WriteStream(System.IO.BinaryWriter w)
        {
            Material.write(w);
            SecondaryMaterial.write(w);
            
            w.Write(SelectionCut);
            w.Write(ShowDrawCoord);
            w.Write(DrawFilled);
            w.Write((byte)PencilSize);
            w.Write(RadiusTolerance);
            w.Write(RoundPencil);
            w.Write((byte)RoadEdgeSize);
            w.Write((byte)RoadPercentFill);
            w.Write((byte)RoadUpwardClear);
            w.Write((byte)RoadBelowFill);
            w.Write(pencilMoveSpeed);
        }

        public void ReadStream(System.IO.BinaryReader r, int version)
        {
            Material.read(r);
            SecondaryMaterial.read(r);

            SelectionCut = r.ReadBoolean();
            ShowDrawCoord = r.ReadBoolean();
            DrawFilled = r.ReadBoolean();
            PencilSize = r.ReadByte();
            RadiusTolerance = r.ReadSingle();
            RoundPencil = r.ReadBoolean();
            RoadEdgeSize = r.ReadByte();
            RoadPercentFill = r.ReadByte();
            RoadUpwardClear = r.ReadByte();
            RoadBelowFill = r.ReadByte();

            pencilMoveSpeed = r.ReadSingle();
        }

        public float moveSpeedProperty(bool set, float value)
        {
            if (set)
            {
                pencilMoveSpeed = value;
            }
            return pencilMoveSpeed;
        }
    }
}
