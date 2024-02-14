using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using VikingEngine.Graphics;

namespace VikingEngine.LF2.Data
{
    static class Block
    {
        public const int FaceTopDir = 1;
        public const int FaceFrontDir = 1;
        public const int FaceRightDir = -1;
        public const float TerrainBlockScale = 1;
        public static readonly Vector3 TerrainBlockScaleV3 = lib.V3(TerrainBlockScale);
        public static readonly Vector3 TerrainBlockHalfScaleV3 = lib.V3(TerrainBlockScale * PublicConstants.Half);
        public const int NumObjBlocksPerTerrainBlock = 8;
        public const float ObjectBlockScale = 1;
        static Face[] terrainFaces;
        static Face[] voxelObjFaces;

        public static void Init()
        {
            const float NormTopLength = 1.1f;
            const float NormSideLength = 0.8f;
            const float NormBottomLength = 0.6f;

            terrainFaces = PolygonLib.createFaces(TerrainBlockScale);
            terrainFaces[(int)CubeFace.Ypositive].Normal = terrainFaces[(int)CubeFace.Ypositive].Normal * NormTopLength;
            terrainFaces[(int)CubeFace.Xpositive].Normal = terrainFaces[(int)CubeFace.Xpositive].Normal * NormSideLength;
            terrainFaces[(int)CubeFace.Xnegative].Normal = terrainFaces[(int)CubeFace.Xnegative].Normal * NormSideLength;
            terrainFaces[(int)CubeFace.Ynegative].Normal = terrainFaces[(int)CubeFace.Ynegative].Normal * NormBottomLength;

            voxelObjFaces = PolygonLib.createFaces(ObjectBlockScale);
        }
        public static Face GetTerrainFace(IntVector3 pos, CubeFace face)
        {
            Face result = terrainFaces[(int)face];
            result.Move(pos.Vec);
            return result;
        }

        public static Face GetVoxelObjFace(Vector3 pos, CubeFace face)
        {
            Face result = voxelObjFaces[(int)face];
            result.Move(pos);
            return result;
        }

        public static Face GetVoxelObjFace(IntVector3 pos, CubeFace face)
        {
            Face result = voxelObjFaces[(int)face];
            result.Move(pos);
            return result;
        }

        public static Face GetVoxelObjFace(IntVector3 pos, int face)
        {
            Face result = voxelObjFaces[face];
            result.Move(pos);
            return result;
        }

        public static Face GetTerrainFace(IntVector3 pos, int face)
        {
            Face result = terrainFaces[face];
            result.Move(pos.Vec);
            return result;
        }
    }
}