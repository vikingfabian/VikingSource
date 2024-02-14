using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.MoonFall
{
    static class moonRef
    {
        //public static bool inEditor;
        public static Map map;
        public static MoonFallState playState;
        public static Players.Players players;

        public static void ClearMem()
        {
            map = null;
            playState = null;
            players = null;
        }
    }
}
