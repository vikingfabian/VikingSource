using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//

namespace VikingEngine.LF2
{

    static class NetworkLib
    {
        public static readonly string[] QuickMessages = new string[]
        {
            "Take this",
            "Follow me",
            "Head towards my flag (see map)",
            "Need food",
	
            "Will head out for raiding now",
            "I'm heading back for town",
	
            "I'll be afk for a moment",
            "I need to quit soon",
        };

        public static readonly SpriteName[] QuickMessageIcons = new SpriteName[]
        {
            SpriteName.LFIconDiscardItem,
            SpriteName.IconMapFlag,
            SpriteName.LFIconMap,
            SpriteName.GoodsApple,

            SpriteName.TrophySword,
            SpriteName.IconTown,

            SpriteName.IconSandGlass,
            SpriteName.IconMenuCloseGame,
        };

        static List<string> BlockedGamers = new List<string>();
        public const string ProgressNetProperty = "Progress";

        //public static void GamerJoinedEvent(Network.AbsNetworkPeer gamer)
        //{
        //    if (BlockedGamers.Contains(gamer.Gamertag))
        //    {
        //        Ref.netSession.RemovePlayer(gamer);
        //        LfRef.gamestate.LocalHostingPlayerPrint(gamer.Gamertag + " blocked");
        //    }
        //}
        public static void BlockGamer(Network.AbsNetworkPeer gamer)
        {
            if (!BlockedGamers.Contains(gamer.Gamertag))
                BlockedGamers.Add(gamer.Gamertag);
        }
    }

    //enum NetworkProperties
    //{
    //    Progress = Network.Session.NumReservedProperties,
    //    NUM
    //}
}
