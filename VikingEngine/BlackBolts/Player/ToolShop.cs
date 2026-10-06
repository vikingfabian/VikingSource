using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.GO;

namespace VikingEngine.Core.BlackBolts.Player
{
    class ToolShop
    {
        //public Dir4 toolDir = Dir4.S;
        //public GameObjectType selectedObjectType = GameObjectType.Worker;
        //public ResourceType selectedResourceType = ResourceType.Box;
        public PlaceObjectData  placementData = new PlaceObjectData() { 
            factoryObjectType = FactoryObjectType.GoblinWorker, resourceType = ResourceType.Box };

        public void selectTool(FactoryObjectType objectType)
        {
            placementData.factoryObjectType = objectType;
            checkToolDir();
        }

        public void selectResource(ResourceType res)
        {
            placementData.resourceType = res;
        }

        public void checkToolDir()
        {
            if (placementData.factoryObjectType == FactoryObjectType.Spin_plate)
            {
                if (placementData.mapPlacement.direction != Dir4.W && placementData.mapPlacement.direction != Dir4.E)
                {
                    rotate();
                }
            }
        }
        public void RotateTool()
        {
            rotate();
            checkToolDir();
        }

        void rotate()
        {
            placementData.mapPlacement.direction++;
            if (placementData.mapPlacement.direction >= Dir4.NUM_NON)
            {
                placementData.mapPlacement.direction = Dir4.N;
            }
        }
    }
}
