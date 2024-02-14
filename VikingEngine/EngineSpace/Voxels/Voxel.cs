using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.Voxels
{
    struct Voxel : IBinaryIOobj
    {
        public static readonly Voxel Empty = new Voxel(IntVector3.Zero, 0);
        public IntVector3 Position;
        public byte Material;

        public Voxel(IntVector3 pos, byte material)
        {
            Position = pos;
            Material = material;
        }
        public bool EqualPos(Voxel other)
        {
            return (Position.Equals(other.Position));

        }
        public Voxel ChangeMaterial(byte toMaterial)
        {
            Material = toMaterial;
            return this;

        }
        public void write(System.IO.BinaryWriter w)
        {
            Position.write(w);
            w.Write(Material);
        }
        public void read(System.IO.BinaryReader r)
        {
            Position.read(r);
            Material = r.ReadByte();
        }
        public static Voxel FromStream(System.IO.BinaryReader r)
        {
            Voxel result = Voxel.Empty;
            result.read(r);
            return result;
        }
        public override string ToString()
        {
            return "Voxel " + Position.ToString() + ":" + Material.ToString();
        }
    }

    struct VoxelHD : IBinaryIOobj
    {
        public static readonly VoxelHD Empty = new VoxelHD(IntVector3.Zero, BlockHD.EmptyBlock);
        public IntVector3 Position;
        public ushort Material;

        public VoxelHD(IntVector3 pos, ushort material)
        {
            Position = pos;
            Material = material;
        }
        public bool EqualPos(Voxel other)
        {
            return (Position.Equals(other.Position));

        }
        public VoxelHD ChangeMaterial(ushort toMaterial)
        {
            Material = toMaterial;
            return this;

        }
        public void write(System.IO.BinaryWriter w)
        {
            Position.write(w);
            w.Write(Material);
        }
        public void read(System.IO.BinaryReader r)
        {
            Position.read(r);
            Material = r.ReadUInt16();
        }
        //public static ushort FromStream(System.IO.BinaryReader r)
        //{
        //    VoxelHD result = VoxelHD.Empty;
        //    result.ReadStream(r);
        //    return result;
        //}
        public override string ToString()
        {
            return "Voxel " + Position.ToString() + ":" + Material.ToString();
        }
    }
}
