using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Moba
{
    static class MobaRef
    {
        public static Map map;
        public static MobaPlayState gamestate;
        public static List<LocalGamer> gamers;
        public static GO.ObjectCollection objects;

        public static void ClearMem()
        {
            map = null;
            gamers = null;
            gamers = null;
        }
    }
}
