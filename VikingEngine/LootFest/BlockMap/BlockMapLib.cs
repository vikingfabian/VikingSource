using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.LootFest
{
    static class BlockMapLib
    {
        public const int SquaresPerChunkW = 4;
        public const int SquareBlockWidth = Map.WorldPosition.ChunkWidth / SquaresPerChunkW;
        public const int SquareHalfWidth = SquareBlockWidth / 2;

        public static readonly Rectangle2 SquareArea = new Rectangle2(IntVector2.Zero, new IntVector2(SquareBlockWidth));
    }

    public struct HeightMapMaterials
    {
        public ushort topMaterial, firstLayerMaterial, bottomMaterial;

        public int firstLayerHeight;

        public HeightMapMaterials(
            BlockPatternMaterial top,
            BlockPatternMaterial firstLayer, int firstLayerHeight,
           BlockPatternMaterial bottom)
        {
            this.topMaterial = BlockHD.ToBlockValue(top);
            this.firstLayerMaterial = BlockHD.ToBlockValue(firstLayer);
            this.firstLayerHeight = firstLayerHeight;
            this.bottomMaterial = BlockHD.ToBlockValue(bottom);
        }

        public static readonly HeightMapMaterials Test = new HeightMapMaterials(BlockPatternMaterial.Grass, BlockPatternMaterial.Sand, 9, BlockPatternMaterial.Dirt);
        
    }

    public struct TerrainModel
    {
        public Map.WorldPosition position;
        public VoxelModelNameAndRotation name;
        public bool fillUpTheGround;

        public TerrainModel(VoxelModelNameAndRotation name, Map.WorldPosition position, bool fillUpTheGround)
        {
            this.fillUpTheGround = fillUpTheGround;
            this.name = name;
            this.position = position;
        }
    }

    public struct ModelJoint
    {
        public int joint;
        public Map.WorldPosition wp;

        public ModelJoint(Voxels.Voxel vox, Map.WorldPosition origo)
        {
            joint = vox.Material - Voxels.VoxelLib.JointStart;
            wp = origo;
            wp.WorldGrindex += vox.Position;
        }
    }

    public class LevelChunk
    {
        public List<TerrainModel> models = new List<TerrainModel>();
    }

    public enum PaintType
    {
        BlockType,
        SpecialType,
    }

    public enum MapBlockType : byte
    {
        Occupied,
        Open,
        Road,
        Wall,
        Water,
        OverrideToOccupied,
        NUM
    }

    public enum MapBlockSpecialType : byte
    {
        None,
        Entrance,
        SpawnPos,
        TerrainModel,
        SpecialPoint,
        SpecialModel,
        Item,
        Landmark,
        NUM
    }

    public enum SegmentHeadType : byte
    {
        Normal,
        NormalLarge,
        None,
        Specific_use,
        NormalEnter,
        BossRoom,
        NormalNpcRoom,
        Castle,
        CastleLarge,
        CastleEnter,
        NormalGuardRoom,
        CastleGuardRoom,
        NormalRoad,
        CastleRoad,
        NormalMazeSegment,
        NUM
    }
}
