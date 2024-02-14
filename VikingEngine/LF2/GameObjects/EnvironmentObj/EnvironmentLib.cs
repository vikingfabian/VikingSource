using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    enum EnvironmentType
    {
        BloodFingerHerb,
        FrogHeartHerb,
        FireStarHerb,
        BlueRoseHerb,
        MinigSpot,
        BeeHive,
        NUM
    }

    enum MapChunkObjectType
    {
        Door,
        CritterSpawn,
        BossLock,
        EndTombLock,
        BasicNPC,
        Salesman,
        Lumberjack,
        Miner,
        Father,
        Messenger,
        Granpa,
        War_veteran,
        Healer,
        Guard,
        Guard_Captain,
        Weapon_Smith,
        Volcan_smith,
        Armor_smith,
        REMOVED1,//Gold_smith,
        REMOVED2, //removed, but kept to not change saves
        Bow_maker,
        Cook,
        Horse_Transport,
        Chest,
        DiscardPile,
        ImError,
        ImGlitch,
        ImBug,

        Mother,
        Blacksmith,
        Tailor,
        Wise_Lady,
        High_priest,
        Banker,
        Builder,

        DebugNPC,
        LootCrate,
        NUM_NONE,

        Home,
    }
}
