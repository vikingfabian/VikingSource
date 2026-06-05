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
        static readonly TimeLength CooldownTime = new TimeLength(TimeExt.MinuteInSeconds * 20);

        public GiftedAchievementsPlayerCollection()
        {
            recieved = null;
        }

        public void Add(GiftedAchievementType type, AbsHumanPlayer from)
        {
            if (recieved == null)
            {
                recieved = new List<GiftedAchievementsTypeAndTime>(3);
            }

            recieved.Add(new GiftedAchievementsTypeAndTime()
            {
                sender = from.networkPeer.peer.fullId,
                time = new TimeInGameCountdown(CooldownTime),
                type = type
            });
        }

        public void ToHud(RichBoxContent content, LocalPlayer player, RemotePlayer remotePlayer, NetSessionDisplay netSessionDisplay)
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
                netSessionDisplay.sendGiftTo = remotePlayer;
                player.hud.needRefresh = true;
            }), new RbTooltip(toolTip, player), remotePlayer != null && emptyCount > 0));
        }

        void toolTip(RichBoxContent content, object tag )
        {
            content.h1("Gifted achievements", HudLib.TitleColor_Head);

            //content.newParagraph();
            foreach (var m in recieved)
            {
                content.Add(new RbSeperationLine());

                var gift = GiftedAchievementCollection.Get(m.type);
                var p = ((PlayState)DssRef.state).GetPlayer(m.sender);
                if (p != null)
                {
                    content.newLine();
                    p.addNetGamerToHud(content, true, false);
                    content.hspace();
                    content.Add(new RbImage(SpriteName.cmdConvertArrow));
                }
                content.icontext(GiftedAchievement.DefaultIcon, gift.name);
                
            }
        }

        public HashSet<GiftedAchievementType> HasGiftsCollection()
        {
            HashSet<GiftedAchievementType> result = new HashSet<GiftedAchievementType>(recieved.Count);
            foreach (var m in recieved)
            {
                result.Add(m.type);
            }

            return result;
        }
    }
}
