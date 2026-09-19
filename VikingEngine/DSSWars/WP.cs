using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.MapData;

namespace VikingEngine.DSSWars
{
    /// <summary>
    /// Position helper
    /// </summary>
    static class WP
    {
        //public const float TileDrawScale = 1f;
        public static readonly Vector2 TileScaleV2 = new Vector2(MapChunkData8_8.ModelScale);
        static readonly Vector2 TileHalfScaleV2 = TileScaleV2 * PublicConstants.Half;

        //public bool InBound(Vector3 position)
        //{ 
        //    return DssRef.world.unitBounds.in
        //}
        public static float SubTileHeight(Vector3 wp)
        {
            return DssRef.world.subTileGrid.Get(ToSubTilePos(wp)).groundY;
        }

        public static Vector2 ToWorldPosXZ(IntVector2 tile)
        {
            return tile.Vec * TileScaleV2;
        }

        public static Vector3 ChunkToWorldPos(IntVector2 tile, float y = 0)
        {
            return new Vector3(tile.X * MapChunkData8_8.ModelScale, y, tile.Y * MapChunkData8_8.ModelScale);
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
            return new IntVector2((pos.X - MapTile1_1.SubTileHalfWidth) * Map.MapData.MapTile1_1.ModelScale_Inv + MapTile1_1.ModelScale_Inv_Half, (pos.Z - MapTile1_1.SubTileHalfWidth) * Map.MapData.MapTile1_1.ModelScale_Inv + MapTile1_1.ModelScale_Inv_Half);
        }
        public static IntVector2 ToSubTilePos_Centered(IntVector2 tilePos)
        {
            return new IntVector2(tilePos.X * Map.MapData.MapTile1_1.ModelScale_Inv + MapTile1_1.ModelScale_Inv_Half, tilePos.Y * Map.MapData.MapTile1_1.ModelScale_Inv + MapTile1_1.ModelScale_Inv_Half);
        }

        public static IntVector2 ToSubTilePos_TopLeft(IntVector2 pos)
        {
            return new IntVector2(pos.X * Map.MapData.MapTile1_1.ModelScale_Inv, pos.Y * Map.MapData.MapTile1_1.ModelScale_Inv);
        }

        public static Rectangle2 ToSubTilePos(Rectangle2 area)
        {
            area *= Map.MapData.MapTile1_1.ModelScale_Inv;
            return area;
        }

        public static Vector3 SubtileToWorldPosXZ(IntVector2 subtilePos)
        {
            return new Vector3(subtilePos.X * MapTile1_1.ModelScale - WorldData.TileHalfWidth, 0, subtilePos.Y * MapTile1_1.ModelScale - WorldData.TileHalfWidth);
        }

        public static Vector3 SubtileToWorldPosXZ_Centered(IntVector2 subtilePos)
        {
            return new Vector3(
                subtilePos.X * MapTile1_1.ModelScale - WorldData.TileHalfWidth + MapTile1_1.SubTileHalfWidth, 
                0, 
                subtilePos.Y * MapTile1_1.ModelScale - WorldData.TileHalfWidth + MapTile1_1.SubTileHalfWidth);
        }

        public static Vector3 WorldPosToClosestSubtile_Centered(Vector3 worldPos)
        {
            var subtile = ToSubTilePos(worldPos);
            worldPos = SubtileToWorldPosXZ_Centered(subtile);
            worldPos.Y = DssRef.world.subTileGrid.Get(subtile).groundY;

            return worldPos;
        }

        //public static Vector3 SubtileToWorldPosXZgroundY_Centered(IntVector2 subtilePos)
        //{
        //    var result = new Vector3(
        //        subtilePos.X * MapTile1_1.ModelScale - WorldData.TileHalfWidth + MapTile1_1.SubTileHalfWidth,
        //        0,
        //        subtilePos.Y * MapTile1_1.ModelScale - WorldData.TileHalfWidth + MapTile1_1.SubTileHalfWidth);

        //    if (DssRef.world.subTileGrid.TryGet(subtilePos, out MapTile1_1 subTile))
        //    { 
        //        result.Y = subTile.groundY;
        //    }

        //    return result;
        //}

        public static Vector3 SubtileToWorldPosXZgroundY_Centered(IntVector2 subtilePos)
        {
            var result = new Vector3(
                subtilePos.X * MapTile1_1.ModelScale + MapTile1_1.SubTileHalfWidth,
                0,
                subtilePos.Y * MapTile1_1.ModelScale + MapTile1_1.SubTileHalfWidth);

            if (DssRef.world.subTileGrid.TryGet(subtilePos, out MapTile1_1 subTile))
            {
                result.Y = subTile.groundY;
            }

            return result;
        }

        public static IntVector2 SubtileToTilePos(IntVector2 subtilePos)
        {
            subtilePos.X = (subtilePos.X) / Map.MapData.MapTile1_1.ModelScale_Inv;
            subtilePos.Y = (subtilePos.Y) / Map.MapData.MapTile1_1.ModelScale_Inv;
            return subtilePos;
        }

        //public static Vector3 ChunkToMapPos(IntVector2 tile)
        //{
        //    return new Vector3(
        //        tile.X * TileDrawScale,
        //        DssRef.world.tileGrid.Get(tile).co .GroundY_aboveWater(),
        //        tile.Y * TileDrawScale);
        //}

        public static Vector3 ToSubTileWP_Centered(IntVector2 tilePos)
        {
            return new Vector3(
                tilePos.X * MapTile1_1.ModelScale + MapTile1_1.SubTileHalfWidth,
                DssRef.world.subTileGrid.Get(tilePos).groundY,
                tilePos.Y * MapTile1_1.ModelScale + MapTile1_1.SubTileHalfWidth);
        }
        
        /// <summary>
        /// Picks ground height from subtiles, not bound safe
        /// </summary>
        public static float GroundY(Vector3 wp)
        {
            return DssRef.world.subTileGrid.Get(
                Convert.ToInt32(wp.X * Map.MapData.MapTile1_1.ModelScale_Inv + 3.5f),
                Convert.ToInt32(wp.Z * Map.MapData.MapTile1_1.ModelScale_Inv + 3.5f)).groundY;
        }

        public static void Rotation1DToQuaterion(Graphics.Mesh mesh, float rotation)
        {
            if (mesh != null)
            {
                mesh.Rotation.QuadRotation = Quaternion.CreateFromYawPitchRoll(MathHelper.TwoPi - rotation, 0, 0);
            }
        }

        public static void Rotation1DToQuaterion(Graphics.AbsVoxelObj mesh, float rotation)
        {
            if (mesh != null)
            {
                mesh.Rotation.QuadRotation = Quaternion.Identity;
                mesh.Rotation.RotateWorldX(MathHelper.Pi - rotation);
            }
        }

        public static RotationQuarterion ToQuaterion(float rotation)
        {
            RotationQuarterion rot = new RotationQuarterion(Quaternion.CreateFromYawPitchRoll(MathHelper.TwoPi - rotation, 0, 0));//RotationQuarterion.Identity;
            
            return rot;
        }

        public static float birdDistance(AbsMapObject obj1, IntVector2 tilePos2)
        {
            return (obj1.maptilePos - tilePos2).Length();
        }
        public static float birdDistance(AbsMapObject obj1, AbsMapObject obj2)
        {
            return (obj1.maptilePos - obj2.maptilePos).Length();
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

        public static void writeSubTilePos(System.IO.BinaryWriter w, IntVector2 position)
        {
            position.WriteUInt24(w);
        }

        public static IntVector2 readSubTilePos(System.IO.BinaryReader r)
        {
            var result = IntVector2.Zero;
            result.ReadUInt24(r);
            return result;
        }

        public static void readPosXZ_old(System.IO.BinaryReader r, out Vector3 position, out IntVector2 tilePos)
        {
            position = Vector3.Zero;
            position.X = (float)r.ReadHalf();
            position.Z = (float)r.ReadHalf();

            tilePos = new IntVector2(position.X, position.Z);
        }

        public static void WritePosXZPercentU16(BinaryWriter w, Vector3 position)
        {
            StreamLib.WriteFloatAsPercentU16(w, position.X, DssRef.world.Size.X);
            StreamLib.WriteFloatAsPercentU16(w, position.Z, DssRef.world.Size.Y);
        }

        public static void ReadPosXZPercentU16(BinaryReader r, out Vector3 position, out IntVector2 maptilePos)
        {
            position = Vector3.Zero;
            position.X = StreamLib.ReadFloatFromPercentU16(r, DssRef.world.Size.X);
            position.Z = StreamLib.ReadFloatFromPercentU16(r, DssRef.world.Size.Y);

            maptilePos = new IntVector2(position.X * MapTile1_1.ModelScale_Inv, position.Z * MapTile1_1.ModelScale_Inv);
        }

        public static bool ReadPosXZPercentU16_ZeroCheck(BinaryReader r, out Vector3 position, out IntVector2 maptilePos)
        {
            position = Vector3.Zero;
            position.X = StreamLib.ReadFloatFromPercentU16(r, DssRef.world.Size.X);
            position.Z = StreamLib.ReadFloatFromPercentU16(r, DssRef.world.Size.Y);

            maptilePos = new IntVector2(position.X * MapTile1_1.ModelScale_Inv, position.Z * MapTile1_1.ModelScale_Inv);

            return position.X > 0;
        }

        public static IntVector2 MaptileToSumTile(IntVector2 mapTilePos)
        {
            return mapTilePos / SumTile4_4.TileWidth;
        }
        public static IntVector2 MaptileToSumTile_centered(IntVector2 mapTilePos)
        {
            return (mapTilePos + (SumTile4_4.TileWidth / 2)) / SumTile4_4.TileWidth;
        }
        public static IntVector2 MaptileToChunk(IntVector2 mapTilePos)
        {
            return mapTilePos / MapChunkData8_8.TileWidth;
        }
    }
}