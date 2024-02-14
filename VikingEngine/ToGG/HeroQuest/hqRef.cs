using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.ToGG.HeroQuest
{
    static class hqRef
    {
        public static ItemManager items;
        public static HeroQuestPlay gamestate;
        public static NetManager netManager;
        public static Players.PlayerCollection players;
        public static Data.LootManager loot;
        public static Data.AllUnitsData unitsdata;
        public static Data.LocalPlayersSetup localPlayers;
        public static Data.QuestSetup setup;
        public static EventManager events;
        public static PlayerHUD playerHud { get { return players.localHost.hud; } }

        public static void ClearMEM()
        {
            items = null;
            gamestate = null;
            netManager = null;
            
            players = null;
            unitsdata = null;
            //localPlayers = null;

            setup = null;
            events = null;
        }
    }
}
