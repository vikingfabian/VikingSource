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

    }
}