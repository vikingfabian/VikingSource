using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.Mission;

namespace VikingEngine.Core.BlackBolts.Player
{
    class ToolShop
    {
        public PlaceObjectData  placementData = new PlaceObjectData() { 
            component = new ToolSetupComponent(), resourceType = ResourceType.Box };


        public ToolShop()
        {
            placementData.component = BlackRef.missionSetup.componentList.First();
            placementData.spawn = FactoryObjectType.WhiteKnight;
        }

        public void selectTool(ToolSetupComponent objectType)
        {
            
            placementData.machineId.hasValue = false;
            placementData.component = objectType;
            
            checkToolDir();
        }

        public void selectResource(ResourceType res)
        {
            placementData.resourceType = res;
        }

        public void checkToolDir()
        {

            if (placementData.component.properties().rotationType == RotationType.LeftRight)
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

        public bool IncludeItemProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                placementData.includeItem = value;
            }
            return placementData.includeItem;
        }
    }
}
