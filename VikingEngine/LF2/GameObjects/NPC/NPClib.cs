using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.NPC
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
    //        //goldsmith_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.goldsmith_station)[0];
    //        bow_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.bow_station)[0];
    //        armor_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.armour_station)[0];
    //        cook_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.cook_station)[0];
    //        vulcan_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.vulcan_station)[0];
    //        blacksmith_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.blacksmith_station)[0];
    //        healer_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.healer_station)[0];
    //        salesman_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.salesman_station)[0];
    //        lumberjack_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.lumberjack_station)[0];
    //        miner_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.miner_station)[0];

    //        weaponsmith_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.weaponsmith_station)[0];
    //        tailor_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.tailor_station)[0];
    //        wiselady_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.wiselady_cabin)[0];
    //        priest_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.priest_station)[0];

    //    }
    //    static Voxels.VoxelObjGridData WorkingStationFromType(GameObjects.EnvironmentObj.MapChunkObjectType npcType)
    //    {
    //        switch (npcType)
    //        {
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Armor_smith:
    //                return armor_station;
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Bow_maker:
    //                return bow_station;
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Cook:
    //                return cook_station;
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Healer:
    //                return healer_station;
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Lumberjack:
    //                return lumberjack_station;
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Miner:
    //                return miner_station;
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Salesman:
    //                return salesman_station;
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Weapon_Smith:
    //                return weaponsmith_station;
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Volcan_smith:
    //                return vulcan_station;

    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Blacksmith:
    //                return blacksmith_station;
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.High_priest:
    //                return priest_station;
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Wise_Lady:
    //                return wiselady_station;
    //            case GameObjects.EnvironmentObj.MapChunkObjectType.Tailor:
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
