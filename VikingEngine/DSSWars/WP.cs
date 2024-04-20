using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars
{
    /// <summary>
    /// Position helper
    /// </summary>
    static class WP
    {
        public const float TileDrawScale = 1f;
        public static readonly Vector2 TileScaleV2 = new Vector2(TileDrawScale);
        static readonly Vector2 TileHalfScaleV2 = TileScaleV2 * PublicConstants.Half;

        public static Vector2 ToWorldPosXZ(IntVector2 tile)
        {
            return tile.Vec * TileScaleV2;
        }

        public static Vector3 ToWorldPos(IntVector2 tile, float y = 0)
        {
            return new Vector3(tile.X * TileDrawScale, y, tile.Y * TileDrawScale);
        }

        public static IntVector2 ToTilePos(Vector3 pos)
        {
            return new IntVector2(pos.X, pos.Z);
        }
        public static IntVector2 ToTilePos(Vector2 pos)
        {
            return new IntVector2(pos.X, pos.Y);
        }
        public static IntVector2 ToSubTilePos(Vector3 pos)
        {
            return new IntVector2(pos.X * WorldData.TileSubDivitions, pos.Z * WorldData.TileSubDivitions);
        }

        public static IntVector2 ToSubTilePos_TopLeft(IntVector2 pos)
        {
            return new IntVector2(pos.X * WorldData.TileSubDivitions, pos.Y * WorldData.TileSubDivitions);
        }

        public static Vector3 ToMapPos(IntVector2 tile)
        {
            return new Vector3(
                tile.X * TileDrawScale,
                DssRef.world.tileGrid.Get(tile).GroundY_aboveWater(),
                tile.Y * TileDrawScale);
        }

        public static Vector3 ToSubTilePos_Centered(IntVector2 subtile)
        {
            return new Vector3(
                subtile.X * WorldData.SubTileWidth + WorldData.SubTileHalfWidth,
                DssRef.world.subTileGrid.Get(subtile).groundY,
                subtile.Y * WorldData.SubTileWidth + WorldData.SubTileHalfWidth);
        }

        public static void Rotation1DToQuaterion(Graphics.Mesh mesh, float rotation)
        {
            mesh.Rotation.QuadRotation = Quaternion.CreateFromYawPitchRoll(MathHelper.TwoPi - rotation, 0, 0);
            //    Quaternion.Identity;
            //Vector3 rot = Vector3.Zero;
            //rot.X = MathHelper.TwoPi - rotation;
            //mesh.Rotation.RotateWorld(rot);
        }

        public static void Rotation1DToQuaterion(Graphics.AbsVoxelObj mesh, float rotation)
        {
            mesh.Rotation.QuadRotation = Quaternion.Identity;
            mesh.Rotation.RotateWorldX(MathHelper.Pi - rotation);
        }

        public static RotationQuarterion ToQuaterion(float rotation)
        {
            RotationQuarterion rot = new RotationQuarterion(Quaternion.CreateFromYawPitchRoll(MathHelper.TwoPi - rotation, 0, 0));//RotationQuarterion.Identity;
            //rot.RotateWorldX(MathHelper.Pi - rotation);
            return rot;
        }

        public static float birdDistance(AbsMapObject obj1, IntVector2 tilePos2)
        {
            return (obj1.tilePos - tilePos2).Length();
        }
        public static float birdDistance(AbsMapObject obj1, AbsMapObject obj2)
        {
            return (obj1.tilePos - obj2.tilePos).Length();
        }

        public static void writeTilePos(System.IO.BinaryWriter w, IntVector2 position)
        {
            position.writeUshort(w);
        }

        public static IntVector2 readTilePos(System.IO.BinaryReader r)
        {
            var result = IntVector2.Zero;
            result.readUshort(r);
            return result;
        }

        public static void writePosXZ(System.IO.BinaryWriter w, Vector3 position)
        {
            w.Write((Half)position.X);
            w.Write((Half)position.Z);
        }
        public static void readPosXZ(System.IO.BinaryReader r, out Vector3 position, out IntVector2 tilePos)
        {
            position = Vector3.Zero;
            position.X = (float)r.ReadHalf();
            position.Z = (float)r.ReadHalf();

            tilePos = new IntVector2(position.X, position.Z);
        }
    }
}