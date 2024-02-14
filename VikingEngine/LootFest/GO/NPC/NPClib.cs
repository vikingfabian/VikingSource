using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.NPC
{
    static class NPClib
    {
    //    //protected static Voxels.VoxelObjGridData goldsmith_station;
    //    protected static Voxels.VoxelObjGridData bow_station;
    //    protected static Voxels.VoxelObjGridData armor_station;
    //    protected static Voxels.VoxelObjGridData cook_station;
    //    protected static Voxels.VoxelObjGridData vulcan_station;
    //    //protected static Voxels.VoxelObjGridData sheild_station;
    //    protected static Voxels.VoxelObjGridData blacksmith_station;
    //    protected static Voxels.VoxelObjGridData healer_station;
    //    protected static Voxels.VoxelObjGridData salesman_station;
    //    protected static Voxels.VoxelObjGridData lumberjack_station;
    //    protected static Voxels.VoxelObjGridData miner_station;

    //    protected static Voxels.VoxelObjGridData weaponsmith_station;
    //    protected static Voxels.VoxelObjGridData tailor_station;
    //    protected static Voxels.VoxelObjGridData wiselady_station;
    //    protected static Voxels.VoxelObjGridData priest_station;

    //    public static void LoadStationImages()
    //    {
    //        //goldsmith_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.goldsmith_station)[0];
    //        bow_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.bow_station)[0];
    //        armor_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.armour_station)[0];
    //        cook_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.cook_station)[0];
    //        vulcan_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.vulcan_station)[0];
    //        blacksmith_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.blacksmith_station)[0];
    //        healer_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.healer_station)[0];
    //        salesman_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.salesman_station)[0];
    //        lumberjack_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.lumberjack_station)[0];
    //        miner_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.miner_station)[0];

    //        weaponsmith_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.weaponsmith_station)[0];
    //        tailor_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.tailor_station)[0];
    //        wiselady_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.wiselady_cabin)[0];
    //        priest_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelObjName.priest_station)[0];

    //    }
    //    static Voxels.VoxelObjGridData WorkingStationFromType(GO.EnvironmentObj.MapChunkObjectType npcType)
    //    {
    //        switch (npcType)
    //        {
    //            case GO.EnvironmentObj.MapChunkObjectType.Armor_smith:
    //                return armor_station;
    //            case GO.EnvironmentObj.MapChunkObjectType.Bow_maker:
    //                return bow_station;
    //            case GO.EnvironmentObj.MapChunkObjectType.Cook:
    //                return cook_station;
    //            case GO.EnvironmentObj.MapChunkObjectType.Healer:
    //                return healer_station;
    //            case GO.EnvironmentObj.MapChunkObjectType.Lumberjack:
    //                return lumberjack_station;
    //            case GO.EnvironmentObj.MapChunkObjectType.Miner:
    //                return miner_station;
    //            case GO.EnvironmentObj.MapChunkObjectType.Salesman:
    //                return salesman_station;
    //            case GO.EnvironmentObj.MapChunkObjectType.Weapon_Smith:
    //                return weaponsmith_station;
    //            case GO.EnvironmentObj.MapChunkObjectType.Volcan_smith:
    //                return vulcan_station;

    //            case GO.EnvironmentObj.MapChunkObjectType.Blacksmith:
    //                return blacksmith_station;
    //            case GO.EnvironmentObj.MapChunkObjectType.High_priest:
    //                return priest_station;
    //            case GO.EnvironmentObj.MapChunkObjectType.Wise_Lady:
    //                return wiselady_station;
    //            case GO.EnvironmentObj.MapChunkObjectType.Tailor:
    //                return tailor_station;

    //            default:
    //                return null;
    //        }
    //    }
        
    }
    
    enum TalkingNPCTalkLink
    {
        //Back,
        Exit,
        PickGift,
        TakeQuest1,
        TakeQuest2,
        TakeQuest3,
        TakeQuest4,
        TakeQuest5,
        WhatsNext,
        // Travel,
        Main,
        Gossip1,


        WorkerChooseIngredient,
        WorkerChooseQuality,
        WorkerCraftEndDialogue,

        SelectItem,
        SelectItemOK,
        CraftBluePrint,
        SelectBluePrint,
        CraftOK,
        WorkerChooseIngredient_dialogue,
        WorkerChooseQuality_dialogue,

        QuestTip,
        Special,
    }
}
