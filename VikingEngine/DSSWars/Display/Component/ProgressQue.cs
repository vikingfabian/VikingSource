using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Display.Component
{
    class ProgressQue
    {
        public void toHud(LocalPlayer player, RichBoxContent content, Action<int> queClick, int currentQue, int maxQue, bool noLimitOption)
        {
            HudLib.Label(content, DssRef.lang.Hud_Queue);
            content.space();
            HudLib.InfoButton(content, new RbAction(()=> {
                {
                    RichBoxContent content = new RichBoxContent();

                    content.text(DssRef.lang.Automation_queue_description);

                    player.hud.tooltip.create(player, content, true);
                }
            }));

            content.newLine();
            content.Add(new RbImage(player.input.Stop.Icon));
            content.space();
            for (int length = 0; length <= maxQue; length++)
            {
                var button = new RbButton(new List<AbsRichBoxMember>{
                       new RbText( length.ToString())
                    }, new RbAction1Arg<int>(queClick, length, length == 0? SoundLib.menuStop : SoundLib.menuStart));
                button.setGroupSelectionColor(HudLib.RbSettings, length == currentQue);
                content.Add(button);
                content.space();
            }

            if (noLimitOption)
            {
                var button = new RbButton(new List<AbsRichBoxMember>{
                       new RbText(DssRef.lang.Hud_NoLimit)
                    }, new RbAction1Arg<int>(queClick, 255, SoundLib.menuStart));
                button.setGroupSelectionColor(HudLib.RbSettings, currentQue > maxQue);
                content.Add(button);
            }
        }


        //void queInfo()
        //{
        //    RichBoxContent content = new RichBoxContent();

        //    content.text("Will keep repeating until the que is empty");

        //    player.hud.tooltip.create(player, content, true);
        //}
    }
}
