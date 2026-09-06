using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct VertexVoxelInstance
    {
        // 64 Bytes: Full 4x4 World Transform Matrix (Rows 0..3 mapped to TEXCOORD1..4)
        public Vector4 WorldRow0; // (M11, M12, M13, M14)
        public Vector4 WorldRow1; // (M21, M22, M23, M24)
        public Vector4 WorldRow2; // (M31, M32, M33, M34)
        public Vector4 WorldRow3; // (M41, M42, M43, M44 - Translation in XYZ, W=1)

        // 16 Bytes: Instance Metadata: X = ColorR/Team, Y = ColorG, Z = ColorB, W = DamageFlash
        public Vector4 InstanceData;

        public static readonly VertexDeclaration VertexDeclaration;

        static VertexVoxelInstance()
        {
            var elements = new VertexElement[]
            {
                // World Matrix Rows mapped to TEXCOORD1..4
                new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1),
                new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 2),
                new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 3),
                new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 4),

                // Instance Color / Flags mapped to COLOR1
                new VertexElement(64, VertexElementFormat.Vector4, VertexElementUsage.Color, 1)
            };

            VertexDeclaration = new VertexDeclaration(elements);
        }

        public VertexVoxelInstance(ref Matrix world, Vector4 instanceData)
        {
            WorldRow0 = new Vector4(world.M11, world.M12, world.M13, world.M14);
            WorldRow1 = new Vector4(world.M21, world.M22, world.M23, world.M24);
            WorldRow2 = new Vector4(world.M31, world.M32, world.M33, world.M34);
            WorldRow3 = new Vector4(world.M41, world.M42, world.M43, world.M44);
            InstanceData = instanceData;
        }

        public void Set(ref Matrix world, Vector4 instanceData)
        {
            WorldRow0.X = world.M11; WorldRow0.Y = world.M12; WorldRow0.Z = world.M13; WorldRow0.W = world.M14;
            WorldRow1.X = world.M21; WorldRow1.Y = world.M22; WorldRow1.Z = world.M23; WorldRow1.W = world.M24;
            WorldRow2.X = world.M31; WorldRow2.Y = world.M32; WorldRow2.Z = world.M33; WorldRow2.W = world.M34;
            WorldRow3.X = world.M41; WorldRow3.Y = world.M42; WorldRow3.Z = world.M43; WorldRow3.W = world.M44;
            InstanceData = instanceData;
        }
    }
}
