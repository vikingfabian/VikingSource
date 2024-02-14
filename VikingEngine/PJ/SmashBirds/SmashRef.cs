using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.SmashBirds
{
    static class SmashRef
    {
        public static SmashGameState gamestate;
        public static List2<Gamer> gamers;
        public static Map map;
        public static ObjectCollection objects;

        public static void ClearMem()
        {
            gamestate = null;
            gamers = null;
            map = null;
            objects = null;
        }
    }
}
