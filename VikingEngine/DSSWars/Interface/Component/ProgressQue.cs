using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Interface.Component
{
    class ProgressQue
    {
        public const int NoLimit = 255;
        public void labelToHud(RichBoxContent content)
        {
            HudLib.Label(content, DssRef.lang.Hud_ProductionQueue);
            content.space();
            HudLib.InfoButton(content, new RbTooltip_Text(DssRef.lang.Automation_queue_description));
        }

        public void buttonsToHud(LocalPlayer player, RichBoxContent content, Action<int> queClick, int currentQue, int maxQue, bool noLimitOption)
        {
            content.newLine();
            player.gameControls.input.StopStart.ToRichContent(content);
            //content.Add(new RbImage(player.gameControls.input.StopStart.Icon));
            content.space();
            for (int length = 0; length <= maxQue; length++)
            {
                var button = new ArtToggle(length == currentQue, new List<AbsRichBoxMember>{
                       new RbText( length.ToString())
                    }, new RbAction1Arg<int>(queClick, length, length == 0 ? RbSoundType.Stop : RbSoundType.Start));
                //button.setGroupSelectionColor(HudLib.RbSettings, length == currentQue);
                content.Add(button);
                //content.space();
            }

            if (noLimitOption)
            {
                var button = new ArtToggle(currentQue > maxQue, new List<AbsRichBoxMember>{
                       new RbText(DssRef.lang.Hud_NoLimit)
                    }, new RbAction1Arg<int>(queClick, 255, RbSoundType.Start));
                //button.setGroupSelectionColor(HudLib.RbSettings, currentQue > maxQue);
                content.Add(button);
            }
        }

        public void singleToHud(LocalPlayer player, RichBoxContent content, Action<int> queClick, int currentQue, int maxQue, bool noLimitOption)
        {
            labelToHud(content);
            buttonsToHud(player, content, queClick, currentQue, maxQue, noLimitOption);
        }
        public void listToHud(LocalPlayer player, RichBoxContent content, Action<int> queAllClick, bool noLimit)
        {
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("=0") }, new RbAction1Arg<int>(queAllClick, 0, RbSoundType.Stop)));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+1") }, new RbAction1Arg<int>(queAllClick, 1, RbSoundType.Start)));
            if (noLimit)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_NoLimit) }, new RbAction1Arg<int>(queAllClick, ProgressQue.NoLimit, RbSoundType.Start)));
            }
        }
    }
}
