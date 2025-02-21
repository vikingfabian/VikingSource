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
            return new IntVector2((pos.X - WorldData.SubTileHalfWidth) * WorldData.TileSubDivitions + WorldData.HalfTileSubDivitions , (pos.Z - WorldData.SubTileHalfWidth) * WorldData.TileSubDivitions + WorldData.HalfTileSubDivitions );
        }
        public static IntVector2 ToSubTilePos_Centered(IntVector2 tilePos)
        {
            return new IntVector2(tilePos.X * WorldData.TileSubDivitions + WorldData.HalfTileSubDivitions, tilePos.Y * WorldData.TileSubDivitions + WorldData.HalfTileSubDivitions);
        }

        public static IntVector2 ToSubTilePos_TopLeft(IntVector2 pos)
        {
            return new IntVector2(pos.X * WorldData.TileSubDivitions, pos.Y * WorldData.TileSubDivitions);
        }

        public static Vector3 SubtileToWorldPosXZ(IntVector2 subtilePos)
        {
            return new Vector3(subtilePos.X * WorldData.SubTileWidth - WorldData.TileHalfWidth, 0, subtilePos.Y * WorldData.SubTileWidth - WorldData.TileHalfWidth);
        }

        public static Vector3 SubtileToWorldPosXZ_Centered(IntVector2 subtilePos)
        {
            return new Vector3(
                subtilePos.X * WorldData.SubTileWidth - WorldData.TileHalfWidth + WorldData.SubTileHalfWidth, 
                0, 
                subtilePos.Y * WorldData.SubTileWidth - WorldData.TileHalfWidth + WorldData.SubTileHalfWidth);
        }

        public static Vector3 WorldPosToClosestSubtile_Centered(Vector3 worldPos)
        {
            var subtile = ToSubTilePos(worldPos);
            worldPos = SubtileToWorldPosXZ_Centered(subtile);
            worldPos.Y = DssRef.world.subTileGrid.Get(subtile).groundY;

            return worldPos;
        }

        public static Vector3 SubtileToWorldPosXZgroundY_Centered(IntVector2 subtilePos)
        {
            var result = new Vector3(
                subtilePos.X * WorldData.SubTileWidth - WorldData.TileHalfWidth + WorldData.SubTileHalfWidth,
                0,
                subtilePos.Y * WorldData.SubTileWidth - WorldData.TileHalfWidth + WorldData.SubTileHalfWidth);

            if (DssRef.world.subTileGrid.TryGet(subtilePos, out SubTile subTile))
            { 
                result.Y = subTile.groundY;
            }

            return result;
        }

        public static IntVector2 SubtileToTilePos(IntVector2 subtilePos)
        {
            subtilePos.X = (subtilePos.X) / WorldData.TileSubDivitions;
            subtilePos.Y = (subtilePos.Y) / WorldData.TileSubDivitions;
            return subtilePos;
        }

        public static Vector3 ToMapPos(IntVector2 tile)
        {
            return new Vector3(
                tile.X * TileDrawScale,
                DssRef.world.tileGrid.Get(tile).GroundY_aboveWater(),
                tile.Y * TileDrawScale);
        }

        public static Vector3 ToSubTileWP_Centered(IntVector2 tilePos)
        {
            return new Vector3(
                tilePos.X * WorldData.SubTileWidth + WorldData.SubTileHalfWidth,
                DssRef.world.subTileGrid.Get(tilePos).groundY,
                tilePos.Y * WorldData.SubTileWidth + WorldData.SubTileHalfWidth);
        }
        
        /// <summary>
        /// Picks ground height from subtiles, not bound safe
        /// </summary>
        public static float GroundY(Vector3 wp)
        {
            return DssRef.world.subTileGrid.array[
                Convert.ToInt32(wp.X * WorldData.TileSubDivitions + 3.5f),
                Convert.ToInt32(wp.Z * WorldData.TileSubDivitions + 3.5f)].groundY;
        }

        public static void Rotation1DToQuaterion(Graphics.Mesh mesh, float rotation)
        {
            mesh.Rotation.QuadRotation = Quaternion.CreateFromYawPitchRoll(MathHelper.TwoPi - rotation, 0, 0);            
        }

        public static void Rotation1DToQuaterion(Graphics.AbsVoxelObj mesh, float rotation)
        {
            mesh.Rotation.QuadRotation = Quaternion.Identity;
            mesh.Rotation.RotateWorldX(MathHelper.Pi - rotation);
        }

        public static RotationQuarterion ToQuaterion(float rotation)
        {
            RotationQuarterion rot = new RotationQuarterion(Quaternion.CreateFromYawPitchRoll(MathHelper.TwoPi - rotation, 0, 0));//RotationQuarterion.Identity;
            
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