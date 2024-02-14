using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.PJ.Joust
{
    //static class JoustRef
    //{
    //    public static Sounds sounds;
	//
    //    public static void ClearMem()
    //    {
    //        sounds = null;
    //    }
    //}
    static class JoustRef
    {
        public static Level level;
        public static Sounds sounds;
        public static Joust.JoustGameState gamestate;

        public static void ClearMem()
        {
            sounds = null;
            gamestate = null;
            level = null;
        }
    }
}
