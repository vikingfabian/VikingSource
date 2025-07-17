using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DataLib;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.Voxels
{
    struct PaintSettings
    {
        public PaintToolType drawTool = PaintToolType.Rectangle;
        public int pencilSize = 3;
        public float radiusTolerance = 0.05f;
        public bool roundPencil = true;
        public int roadEdgeSize = 1;
        public int roadPercentFill = 100;
        public int roadUpwardClear = 4;
        public int roadBelowFill = 2;
        public bool continiousFill = false;

        public PaintSettings()
        { }

        public void WriteStream(System.IO.BinaryWriter w)
        {
           
            w.Write((byte)pencilSize);
            w.Write(radiusTolerance);
            w.Write(roundPencil);
            w.Write((byte)roadEdgeSize);
            w.Write((byte)roadPercentFill);
            w.Write((byte)roadUpwardClear);
            w.Write((byte)roadBelowFill);
        }

        public void ReadStream(System.IO.BinaryReader r, int version)
        {            
            pencilSize = r.ReadByte();
            radiusTolerance = r.ReadSingle();
            roundPencil = r.ReadBoolean();
            roadEdgeSize = r.ReadByte();
            roadPercentFill = r.ReadByte();
            roadUpwardClear = r.ReadByte();
            roadBelowFill = r.ReadByte();
        }
    }

    class VoxelDesignerSettings
    {
        public BlockHD Material = new BlockHD(Color.LightBlue, MaterialProperty.Default);//new BlockHD(Color.Red);
        public BlockHD SecondaryMaterial = new BlockHD(Color.Yellow, MaterialProperty.Default);//new BlockHD(Color.Yellow);
        public bool SelectionCut = true;
        public bool ShowDrawCoord = true;
        public bool DrawFilled = true;//N
        public PaintSettings paintSettings = new PaintSettings();
        public float pencilMoveSpeed = 1;
        

        public FileSortSettings SortSettings = new FileSortSettings();

        public void WriteStream(System.IO.BinaryWriter w)
        {
            Material.write(w);
            SecondaryMaterial.write(w);
            
            w.Write(SelectionCut);
            w.Write(ShowDrawCoord);
            w.Write(DrawFilled);
            paintSettings.WriteStream(w);
            w.Write(pencilMoveSpeed);
        }

        public void ReadStream(System.IO.BinaryReader r, int version)
        {
            Material.read(r);
            SecondaryMaterial.read(r);

            SelectionCut = r.ReadBoolean();
            ShowDrawCoord = r.ReadBoolean();
            DrawFilled = r.ReadBoolean();
            paintSettings.ReadStream(r, version);

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
