using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Display.Component
{
    class ProgressQue
    {
        public void toHud(LocalPlayer player, RichBoxContent content, Action<int> queClick, int currentQue)
        {
            HudLib.Label(content, DssRef.lang.Hud_Queue);
            content.space();
            HudLib.InfoButton(content, new RbAction(()=> {
                {
                    RichBoxContent content = new RichBoxContent();

                    content.text(DssRef.todoLang.Automation_queue_description);

                    player.hud.tooltip.create(player, content, true);
                }
            }));

            content.newLine();
            content.Add(new RichBoxImage(player.input.Stop.Icon));
            content.space();
            for (int length = 0; length <= BarracksStatus.MaxQue; length++)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember>{
                       new RichBoxText( length.ToString())
                    }, new RbAction1Arg<int>(queClick, length, length == 0? SoundLib.menuStop : SoundLib.menuStart));
                button.setGroupSelectionColor(HudLib.RbSettings, length == currentQue);
                content.Add(button);
                content.space();
            }
            {
                var button = new RichboxButton(new List<AbsRichBoxMember>{
                       new RichBoxText(DssRef.lang.Hud_NoLimit)
                    }, new RbAction1Arg<int>(queClick, 255, SoundLib.menuStart));
                button.setGroupSelectionColor(HudLib.RbSettings, currentQue > BarracksStatus.MaxQue);
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
