using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.Commander
{
    static class cmdRef
    {
        public static Display.PlayerHUD hud;
        public static CmdPlayState gamestate;
        public static Commander.Players.PlayerCollection players;
        public static UnitsData.AllUnits units;

        public static void ClearMEM()
        {
            hud = null;
            gamestate = null;
            players = null;
            units = null;
        }
    }
}
