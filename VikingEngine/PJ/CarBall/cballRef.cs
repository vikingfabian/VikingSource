using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.CarBall
{
    static class cballRef
    {
        public static CarBallPlayState state;
        public static Sounds sounds;
        public static float ballScale;

        public static void ClearMem()
        {
            state = null;
            sounds = null;
        }
    }
}
