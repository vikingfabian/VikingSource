using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Voxels;

namespace VikingEngine.LootFest.Map.Terrain
{
    //static class TerrainLib
    //{
    //    static List<VoxelObjListData>[] areaHills = new List<VoxelObjListData>[(int)EnvironmentType.NUM_NON];
    //    const byte NumRotations = 4;
    //    const int NumWalls = 4;
    //    const int NumHillsPerAreaType = NumRotations * NumWalls;

    //    public static void Init()
    //    {
    //        //Create the labyrinth walls
    //        //areaHills[0] = new List<VoxelObjListData>
    //        //{
    //        //    Editor.VoxelObjDataLoader.VoxelObjListData(VoxelObjName.LabrinthWall1),
    //        //    Editor.VoxelObjDataLoader.VoxelObjListData(VoxelObjName.LabrinthWall2),
    //        //    Editor.VoxelObjDataLoader.VoxelObjListData(VoxelObjName.LabrinthWall3),
    //        //    Editor.VoxelObjDataLoader.VoxelObjListData(VoxelObjName.LabrinthWall4)
    //        //};
    //        //for (byte rotation = 1; rotation < NumRotations; rotation++)
    //        //{
    //        //    for (int wall = 0; wall < NumWalls; wall++)
    //        //    {
    //        //        VoxelObjListData rotatedClone = areaHills[0][wall].Clone();
    //        //        rotatedClone.Rotate(rotation, new RangeIntV3(IntVector3.Zero, Editor.VoxelDesigner.StandardDrawlimit));
    //        //        areaHills[0].Add(rotatedClone);
    //        //    }
    //        //}

    //        //for (int i = 1; i < (int)EnvironmentType.NUM; i++)
    //        //{
    //        //    Data.MaterialType changeDirtTo;
    //        //    Data.MaterialType changeGrassTo;

    //        //    switch ((EnvironmentType)i)
    //        //    {
    //        //        default:    //case EnvironmentType.Forrest
    //        //            changeDirtTo = Data.MaterialType.NO_MATERIAL;
    //        //            changeGrassTo = Data.MaterialType.ForestGround;
    //        //            break;
    //        //        case EnvironmentType.Burned:
    //        //            changeDirtTo = Data.MaterialType.BurntGround;
    //        //            changeGrassTo = Data.MaterialType.Stone;
    //        //            break;
    //        //        case EnvironmentType.Desert:
    //        //            changeDirtTo = Data.MaterialType.Sand;
    //        //            changeGrassTo = Data.MaterialType.Sand;
    //        //            break;
    //        //        case EnvironmentType.Swamp:
    //        //            changeDirtTo = Data.MaterialType.NO_MATERIAL;
    //        //            changeGrassTo = Data.MaterialType.NO_MATERIAL;
    //        //            break;

    //        //    }
    //        //    //areaHills[i] = new List<VoxelObjListData>();
    //        //    //for (int wall = 0; wall < NumHillsPerAreaType; wall++)
    //        //    //{
    //        //    //    VoxelObjListData clone = areaHills[0][wall].Clone();
    //        //    //    if (changeDirtTo != Data.MaterialType.NO_MATERIAL)
    //        //    //        clone.ReplaceMaterial((byte)Data.MaterialType.Dirt, (byte)changeDirtTo);
    //        //    //    if (changeGrassTo != Data.MaterialType.NO_MATERIAL)
    //        //    //        clone.ReplaceMaterial((byte)Data.MaterialType.Grass, (byte)changeGrassTo);
    //        //    //    areaHills[i].Add(clone);
    //        //    //}
    //        //    //adda här!!

    //        //}

    //    }
        
    //    //public static void GameStart()
    //    //{
    //    //    //clear memory
    //    //    // areaHills = null;
    //    //}
    //    //public static VoxelObjListData GetRandomWall(EnvironmentType env)
    //    //{
    //    //    return areaHills[(int)env][Data.WorldSeed.Instance.Next(NumHillsPerAreaType)];
    //    //}
    //}
    //struct AreaInfo
    //{
    //    public string Name;
    //    public IntVector2 position;
    //    public AreaInfo(IntVector2 position, string Name)
    //    {
    //        this.position = position;
    //        this.Name = Name;
    //    }
    //}
    //struct UsingFaces
    //{
    //    public bool top;
    //    public bool bottom;
    //    public bool front;
    //    public bool back;
    //    public bool left;
    //    public bool right;

    //    public UsingFaces(bool top, bool bottom, bool front, bool back, bool left, bool right)
    //    {
    //        this.top = top;
    //        this.bottom = bottom;
    //        this.front = front;
    //        this.back = back;
    //        this.left = left;
    //        this.right = right;
    //    }
    //    public static readonly UsingFaces Empty = new UsingFaces(false, false, false, false, false, false);
    //    public bool ShadowSide
    //    {
    //        get { return bottom || back || left; }
    //    }
    //}

    //struct EnvironmentMaterials
    //{
        

    //    public byte mainMaterial;
    //    public byte highGroundMaterial;
    //    public byte lowGroundMaterial;
    //    public byte belowSurfaceMaterial;

    //    public EnvironmentMaterials(byte mainMaterial, byte highGroundMaterial, byte lowGroundMaterial, byte belowSurfaceMaterial)
    //    {
    //        this.mainMaterial = mainMaterial;
    //        this.highGroundMaterial = highGroundMaterial;
    //        this.lowGroundMaterial = lowGroundMaterial;
    //        this.belowSurfaceMaterial = belowSurfaceMaterial;
    //    }
    //}

    //enum EnvironmentType
    //{
    //    Grassfield,
    //    Forest,
    //    Swamp,
    //    Desert,
    //    Burned,
    //    Mountains,
    //    NUM_NON,
    //}
    enum AreaType : byte
    {
        Empty,
        HomeBase,
        FlatEmptyAndMonsterFree,
        Village,
        City,
        Castle,
        FarmOutpost,
        SalesmanOutpost,
        SoldierOutpost,//har soldater, föremål och healer
        TravelOutpost,
        Lumberjack,
        Miner,
        ImError,

        EnemySpawn,
        EndTomb,
        Fort,
        PrivateHome,
        WiseLady,
        FreeBuild,
        DebugCity,
        NUM_NON
    }
}
