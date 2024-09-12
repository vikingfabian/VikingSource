using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Display.Translation
{
    static class LangLib
    {
        public static string Tab(MenuTab tab)
        {
            switch (tab)
            {
                case MenuTab.Info:
                    return DssRef.todoLang.MenuTab_Info;
                case MenuTab.Build:
                    return DssRef.todoLang.MenuTab_Build;
                case MenuTab.Recruit:
                    return DssRef.todoLang.MenuTab_Recruit;
                case MenuTab.Trade:
                    return DssRef.todoLang.MenuTab_Trade;
                case MenuTab.Resources:
                    return DssRef.todoLang.MenuTab_Resources;
                case MenuTab.Work:
                    return DssRef.todoLang.MenuTab_Work;
                case MenuTab.Automation:
                    return DssRef.lang.Automation_Title;
                default:
                    throw new NotImplementedException();
            }
        }

        public static string BuildingDescription(TerrainBuildingType buildingType)
        {
            switch (buildingType)
            { 
                case TerrainBuildingType.Tavern:
                    return "Workers may eat here";
                case TerrainBuildingType.PigPen:
                    return "Produces pigs, which give food and skin";
                case TerrainBuildingType.HenPen:
                    return "Produces hens and eggs, which give food";
                case TerrainBuildingType.WorkerHut:
                    return string.Format( "Expands worker limit with {0}", GameObject.CityDetail.WorkersPerHut);

                default:
                    throw new NotImplementedException();
            }
        }


        public static string Item(ItemResourceType item)
        {
            switch (item)
            {

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
