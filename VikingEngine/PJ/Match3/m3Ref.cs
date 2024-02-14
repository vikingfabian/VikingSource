using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Match3
{
    static class m3Ref
    {
        public static Match3PlayState gamestate;
        public static Sounds sounds;
        public static float TileWidth;
        public static float ParticleSpeed;
        public static float Gravity;
        
        public static void ClearMem()
        {
            gamestate = null;
            sounds = null;
        }
    }
}
