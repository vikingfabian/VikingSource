using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using VikingEngine.DataLib;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars
{
    static class DssRef
    {
        public static Models models;

        public static WorldData world = null;
        public static MapSettings map = null;
        public static Diplomacy diplomacy = null;
        public static GameStorage storage;
        public static Achievements achieve = null;
        public static GameStats stats = null;
        public static AbsPlayState state;
        public static Ambience ambience;

        public static GameObject.AllUnits profile;
        public static GameTime time = new GameTime();
        public static Display.Translation.AbsLanguage lang;
        public static Display.Translation.TodoTranslation todoLang = new Display.Translation.TodoTranslation();
        public static PlaySettings settings;
        public static Difficulty difficulty = new Difficulty();
        //public static Data.Constants.Const Const = new Data.Constants.Const();
    }
}
