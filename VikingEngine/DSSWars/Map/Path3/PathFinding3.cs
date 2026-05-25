using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Path3
{
    class Path3Thread
    {
        public MoveCostLayer layer1_temp;
        public MoveCostLayer layer3_temp;

        public Path3Thread()
        {
            layer1_temp = new MoveCostLayer(2, new IntVector2(MoveCostLayer.Layer4TileWidth / 2));
            layer3_temp = new MoveCostLayer(8, new IntVector2(2));
        }

        public LayerWalkingPath FindPath(Vector3 start, Vector3 goal, int startDir, bool startAsShip, bool endAsShip)
        {

        }
    }

    
}
