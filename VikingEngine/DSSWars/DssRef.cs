using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using VikingEngine.DataLib;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars
{
    static class DssRef
    {
        public static Models models;
        public static InputMap input;
        public static WorldData world = null;
        public static Diplomacy diplomacy = null;
        public static Players.GameStorage storage;
        public static Achievements achieve = null;
        public static PlayState state;
        public static Map.UnitDetailMap detailMap;
        public static GameObject.AllUnits unitsdata;
        public static GameTime time;
        public static Display.Translation.AbsLanguage lang;

        public static int Faction_SouthHara;
        public static int Faction_GreenWood;
    }
}
