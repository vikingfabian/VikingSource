using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Path;

namespace VikingEngine.DSSWars.Map.Path3
{
    class Path3Thread
    {
        public MoveCostLayer layer1_temp;
        public MoveCostLayer layer3_temp;

        public Path3Thread()
        {
            layer1_temp = new MoveCostLayer(1 ,2, new IntVector2(MoveCostLayer.Layer4TileWidth / 2));
            layer3_temp = new MoveCostLayer(3, 8, new IntVector2(2));
        }

        public LayerWalkingPath FindHighPath(Vector3 start, Vector3 goal, int startDir, bool startAsShip, bool endAsShip)
        {
            float tileLength = VectorExt.SideLength_XZ(start, goal);

            LayerWalkingPath lay4Path = null;
            if (tileLength > 32)
            {
                LayerPathFinding path = DssRef.world.GetLayerPath(4);
                path.FindPath(DssRef.world.layer4, 
            }
        }
        public DetailWalkingPath FindDetailPath(LayerWalkingPath parentPath, Vector3 start, Vector3 goal, int startDir, bool startAsShip, bool endAsShip)
        {

        }
    }

    
}
