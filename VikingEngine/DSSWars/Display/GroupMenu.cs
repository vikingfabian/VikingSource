using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Display
{
    class GroupMenu
    {
        SoldierGroup group;
        Players.LocalPlayer player;
        public GroupMenu(Players.LocalPlayer player, SoldierGroup group, RichBoxContent content) 
        {
            this.player = player;
            this.group = group;
            //var typeData = DssRef.unitsdata.Get(group.type);

            //bool hasCommand = player.commandPoints.Int() >= 1;

            //if (typeData.Command_Javelin)
            //{
            //    content.Add(new RichboxButton(
            //        new List<AbsRichBoxMember>
            //        {
            //            new HUD.RichBox.RichBoxText("Javelin throw"),
            //        },
            //        new RbAction(JavelinCommand, SoundLib.menuBuy),
            //        new RbAction(JavelinCommandTooltip),
            //        hasCommand));
            //}
        }

        //void JavelinCommand()
        //{
        //    if (player.commandPoints.pay(1, false))
        //    {
        //        var soldiersC = group.soldiers.counter();

        //        while (soldiersC.Next())
        //        {
        //            soldiersC.sel.bonusProjectiles += 1;
        //        }
        //        player.hud.needRefresh = true;
        //    }
        //}

        //void JavelinCommandTooltip()
        //{
        //    RichBoxContent content = beginCommandTooltip();

        //    content.text("Javelins on their next attack.");
        //    content.text("No time limit.");

        //    player.hud.tooltip.create(player, content, true);
        //}

        //RichBoxContent beginCommandTooltip()
        //{
        //    RichBoxContent content = new RichBoxContent();
        //    content.h1("Command");
        //    content.h2("Cost");
        //    HudLib.ResourceCost(content, SpriteName.WarsCommandSub,"Command points", 1, player.commandPoints.Int());
        //    content.newLine();
        //    content.h2("Gain");

        //    return content;
        //}
    }

   
}
