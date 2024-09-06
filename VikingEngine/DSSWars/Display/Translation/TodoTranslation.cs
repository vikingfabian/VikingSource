using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    class TodoTranslation
    {
        public string CityMenu_SalePricesTitle => "Sale prices";

        public string Resource_TypeName_Water => "water";
        public string Resource_TypeName_Wood => "wood";
        public string Resource_TypeName_Stone => "stone";
        public string Resource_TypeName_RawFood => "raw food";
        public string Resource_TypeName_Food => "food";
        public string Resource_TypeName_Skin => "skin";
        public string Resource_TypeName_Ore => "ore";
        public string Resource_TypeName_Iron => "iron";


        public string MenuTab_Info => "Info";
        public string MenuTab_Work => "Work";
        public string MenuTab_Recruit => "Recruit";
        public string MenuTab_Resources => "Resources";
        public string MenuTab_Trade => "Trade";
        public string MenuTab_Build => "Build";

        public string Work_OrderPrioTitle => "Work priority";
        public string Work_OrderPrioDescription => "Priority goes from 1 (low) to 10 (high)";

        public string Work_Move => "Move items";

        public string Work_GatherXResource => "Gather {0}";
        public string Work_CraftX => "Craft {0}";
        public string Work_Farming => "Farming";
        public string Work_Mining => "Mining";
        public string Work_Trading => "Tradeing";

        public string Work_ExpandHousing => "Expand housing";
        public string Work_ExpandFarms => "Expand farms";

        public string Hud_ToggleFollowFaction => "Toggle follow faction settings";

        public string Hud_FollowFaction_Yes => "Is set to use faction global settings";
        public string Hud_FollowFaction_No => "Is set to use local settings (Global value is {0})";
    }
}
