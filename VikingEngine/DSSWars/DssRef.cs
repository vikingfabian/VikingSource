using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using VikingEngine.DataLib;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars
{
    static class DssRef
    {
        public static Models models;
        public static InputMap input;
        public static WorldData world = null;
        public static MapSettings map = null;
        public static Diplomacy diplomacy = null;
        public static GameStorage storage;
        public static Achievements achieve = null;
        public static PlayState state;
        
        public static GameObject.AllUnits unitsdata;
        public static GameTime time;
        public static Display.Translation.AbsLanguage lang;
        public static PlaySettings settings;

        
    }
}
