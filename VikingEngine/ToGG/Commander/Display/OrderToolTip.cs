using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class OrderToolTip : AbsToolTip
    {
        public OrderToolTip(bool ordered, int ordersMade, int totalOrders, Commander.Players.LocalPlayer player)
            : base(player.mapControls)
        {
            RichBoxContent members = new RichBoxContent();
            bool red = false;

            if (ordered)
            {
                members.h2("Remove order");
            }
            else
            {
                if (ordersMade < totalOrders)
                {
                    members.h2("Order unit");
                }
                else
                {
                    members.h2("Out of orders").overrideColor = HudLib.UnavailableRedCol;
                    red = true;
                }
            }

            members.text("Pick which units to activate this turn");
            //members.add(SpriteName.cmdOrderCheckFlat, TextLib.Divition(ordersMade, totalOrders));
            members.newLine();
            members.Add(new RichBoxImage(SpriteName.cmdOrderCheckFlat));
            var orderCountText = new RichBoxText(TextLib.Divition(ordersMade, totalOrders));
            if (red)
            {
                orderCountText.overrideColor = HudLib.UnavailableRedCol;
            }
            members.Add(orderCountText);

            AddRichBox(members);
        }
    }

    class CantOrderTooltip : AbsToolTip
    {
        public CantOrderTooltip(CantOrderReason cantOrderReason, Commander.Players.LocalPlayer player)
            : base(player.mapControls)
        {
            RichBoxContent members = new RichBoxContent();
            members.h2("Resting Unit").overrideColor = HudLib.UnavailableRedCol;

            members.text("Ordered units must rest for one turn");

            AddRichBox(members);
        }
    }


}