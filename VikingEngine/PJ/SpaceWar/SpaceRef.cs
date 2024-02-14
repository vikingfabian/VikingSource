using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.SpaceWar
{
    static class SpaceRef
    {
        public static SpacePlayState gamestate = null;
        public static GameObjects.ObjectCollection go;

        public static void ClearMem()
        {
            go = null;
            gamestate = null;
        }
    }
}
