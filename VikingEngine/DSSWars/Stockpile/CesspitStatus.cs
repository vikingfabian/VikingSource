using System;
using System.Collections.Generic;

using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Stockpile;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Stockpile
{
    struct CesspitStatus
    {
        public int idAndPosition;
        public ItemResourceType type;
    }

}

namespace VikingEngine.DSSWars.GameObject
{
   
    partial class City
    {
        int selectedCessPit = -1;
        List<CesspitStatus> cesspits = null;

        public void cesspitToHud(LocalPlayer player, RichBoxContent content)
        {
            if (arraylib.HasMembers(cesspits))
            {
                if (arraylib.InBound(cesspits, selectedCessPit))
                {
                    //selected view

                }
                else
                {
                    //list all

                }
            }
            else
            { 
                //No cesspits
            }
        }

        public void addCesspit(IntVector2 subPos)
        {
            CesspitStatus status = new CesspitStatus()
            {
                idAndPosition = conv.IntVector2ToInt(subPos),
                type = ItemResourceType.NONE,
            };

            if (cesspits == null)
            {
                cesspits = new List<CesspitStatus>(4);
            }

            lock (cesspits)
            {
                cesspits.Add(status);
            }
        }
    }
}
