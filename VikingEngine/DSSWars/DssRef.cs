using Steamworks;
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
using VikingEngine.DSSWars.Presentation;
using VikingEngine.SteamWrapping;

namespace VikingEngine.DSSWars
{
    static class DssRef
    {
        public static Models models;

        public static WorldData world = null;
        public static MapSettings map = null;
        //public static Diplomacy diplomacy = null;
        public static GameStorage storage;
        public static Achievements achieve = null;
        public static GameStats stats = null;
        public static AbsPlayState state;
        public static Ambience ambience;

        public static GameObject.AllUnits units;
        public static GameTime time = new GameTime();
        public static Presentation.AbsLanguage lang;
        public static TodoTranslation todoLang = new Presentation.TodoTranslation();
        public static PlaySettings settings;
        public static Difficulty difficulty = new Difficulty();

        public static LeaderBoardType LastLeaderBoardUpload = LeaderBoardType.NUM_NONE;
        //public static Data.Constants.Const Const = new Data.Constants.Const();

        public static DlcDescriptor DlcSupporter, DlcBloodAndGore, FromGloryToGoo;

        public static void InitDLC()
        {
            DlcSupporter = new DlcDescriptor(new AppId_t(4820280));
            DlcBloodAndGore = new DlcDescriptor(new AppId_t(4820290));
            FromGloryToGoo = new DlcDescriptor(new AppId_t(2607060));
        }
    }
}
