using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Tanks
{
    static class tankRef
    {
        public static TankPlayState state;
        public static Level level;
        public static GoColl objects;

        public static void ClearMem()
        {
            state = null;
            level = null;
        }
    }
}
