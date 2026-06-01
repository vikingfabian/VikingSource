using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.Network;

namespace VikingEngine.DSSWars.Data
{
    struct GiftedAchievementsTypeAndTime
    {
        public ulong sender;
        public GiftedAchievementType type;
        public TimeInGameCountdown time;
    }

    struct GiftedAchievementsPlayerCollection
    {
        public const int MaxRecieveCount = 3;
        List<GiftedAchievementsTypeAndTime> recieved;

        public GiftedAchievementsPlayerCollection()
        {
            recieved = null;
        }

        public void ToHud(RichBoxContent content, LocalPlayer player, NetSessionDisplay netSessionDisplay)
        {
            if (recieved == null)
            {
                recieved = new List<GiftedAchievementsTypeAndTime>(3);
            }

            List<AbsRichBoxMember> buttonContent = new List<AbsRichBoxMember>(3);
            foreach (var m in recieved)
            {
                if (!m.time.TimeOut())
                {
                    buttonContent.Add(new RbImage(GiftedAchievement.DefaultIcon));
                }
            }

            int emptyCount = MaxRecieveCount - buttonContent.Count;
            for (int i = 0; i < emptyCount; i++)
            {
                buttonContent.Add(new RbImage(GiftedAchievement.EmptyIcon));
            }

            content.Add(new ArtButton(RbButtonStyle.Outline, buttonContent, new RbAction(() =>
            {
                netSessionDisplay.sendGiftMenu = true;
                player.hud.needRefresh = true;
            }), new RbTooltip(toolTip, player)));
        }

        void toolTip(RichBoxContent content, object tag )
        {
            content.text("Gifted achievements");
        }
    }
}
