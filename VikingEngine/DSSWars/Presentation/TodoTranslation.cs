using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.Presentation
{
    abstract class AbsTranslation
    {
        abstract public string GameSettings_WideScrollbar { get; }
    }


    partial class TodoTranslation
    {
        public string Conscript_Parry => "Parry";
        public string Conscript_Parry_Description => "Higher parry will block some melee attacks";

        public string Conscript_Ability_AntiSpear => "Strong against spear";
        public string Conscript_Ability_AntiPlate => "Anti plate armor";
        public string Conscript_Ability_ArrowWeakness => "Weak to projectiles";
        //public string Conscript_Ability_DefenceBreak => "Breaks soldier block";


        public string PlayX => "Play {0}";
        public string Minigame_BlackBolt_Name => "Black Bolt Industries";
        public string Minigame_BlackBolt_Description => "Factory puzzle minigame. English only.";

        public string BuildingType_GravelRoad => "Gravel road";
        public string BuildingType_CobblestoneRoad => "Cobbled road";
        public string BuildingType_PavedRoad => "Paved road";

        public string BuildingType_ExpansionHall => "Lords seat";
        public string ExpandTerritory_Description => "Will expand you city territory";
        public string BuildingType_SoldiersQuarter => "Soldiers quarter";
        public string BuildingType_ => "";

        public string ArmySupplyCount => "Army supply count";
        public string ArmySupplyCount_ExpandByX => "Will expand army supply count by {0}";
    }
}