using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    static class GolfRef
    {
        public static MinigolfState gamestate;
        public static Field field;
        public static Editor editor = null;
        public static GO.ObjectCollection objects;
        public static Sounds sounds;

        static List<LevelName> NextLevels = null;

        public static LevelName NextRandomLevel()
        {
            LevelName next;
            if (NextLevels == null)
            {
                NextLevels = new List<LevelName>();
            }
            if (NextLevels.Count == 0)
            {
                NextLevels.AddRange(FieldStorage.RetailLevels);
            }

            next = arraylib.RandomListMemberPop(NextLevels);
            return next;
        }

        public static void ClearMemory()
        {
            gamestate = null;
            field = null;
            editor = null;
            objects = null;
            NextLevels = null;
            sounds = null;
        }
    }
}
