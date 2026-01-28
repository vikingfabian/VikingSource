using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign.CardData
{
    /// <summary>
    /// All pools that are instanced to players and the bank
    /// </summary>
    class PlayerSupply
    {
        public Id id;

        public Dictionary<Id, CardPile> cardPileDic = new Dictionary<Id, CardPile>(2);
        public Dictionary<Id, ResourcePool> resources = new Dictionary<Id, ResourcePool>(2);

        public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu, string title)
        {

        }
    }
}
