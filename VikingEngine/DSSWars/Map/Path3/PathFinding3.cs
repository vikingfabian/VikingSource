using Microsoft.CodeAnalysis;
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
        //public MoveCostLayer layer1_temp;
        //public MoveCostLayer layer3_temp;

        public Path3Thread()
        {
            //layer1_temp = new MoveCostLayer(1 ,2, new IntVector2(MoveCostLayer.Layer4TileWidth / 2));
            //layer3_temp = new MoveCostLayer(3, 8, new IntVector2(2));
        }

        public LayerWalkingPath FindHighPath(Vector3 start, Vector3 goal, int startDir, bool startAsShip, bool endAsShip)
        {
            float tileLength = VectorExt.SideLength_XZ(start, goal);

            LayerWalkingPath lay4Path = null;
            if (tileLength > MoveCostLayer.Layer4TileWidth * 2)
            {
                LayerPathFinding path = DssRef.world.GetLayerPath(4);
                lay4Path = path.FindPath(DssRef.world.layer4, MoveCostLayer.WpToLay4(start), startDir, MoveCostLayer.WpToLay4(goal), startAsShip, endAsShip);
            }

            LayerWalkingPath lay2Path = null;
            //if (tileLength > MoveCostLayer.Layer2TileWidth * 2)
            {
                LayerPathFinding path = DssRef.world.GetLayerPath(2);
                path.ApplyParentPath(lay4Path);
                lay2Path = path.FindPath(DssRef.world.layer2, MoveCostLayer.WpToLay2(start), startDir, MoveCostLayer.WpToLay2(goal), startAsShip, endAsShip);
            }

            return lay2Path;
        }
        public DetailWalkingPath FindDetailPath(LayerWalkingPath parentPath, Vector3 start, Vector3 goal, Rotation1D startDir, bool startAsShip, bool endAsShip, bool isTravelNode)
        {
            DetailPathFinding detailPath = DssRef.world.detailPathFindingPool.GetPf();
            detailPath.ApplyParentPath(parentPath);

            var result = detailPath.FindPath(WP.ToSubTilePos(start), startDir, WP.ToSubTilePos(goal),
                    startAsShip, endAsShip, isTravelNode);
            return result;
        }
    }

    
}
