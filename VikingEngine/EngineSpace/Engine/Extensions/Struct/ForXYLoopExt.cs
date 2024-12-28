using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    class ForXYEdgeLoopRandomPicker
    {
        List<IntVector2> positions = new List<IntVector2>(16);
        ForXYEdgeLoop loop;
        public IntVector2 Position;

        public void start(Rectangle2 area)
        { 
            positions.Clear();

            loop = new ForXYEdgeLoop(area);
            while (loop.Next())
            {
                positions.Add(loop.Position);
            }
        }

        public bool Next()
        {
            if (positions.Count == 0) return false;

            Position = arraylib.RandomListMemberPop(positions);
            return true;
        }

    }
}
