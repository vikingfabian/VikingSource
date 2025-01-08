using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class QuestBanner : AbsQuestBanner
    {
        public QuestBanner(float rightPos)
            : base(rightPos)
        { }

        protected override void onMouseEnter(bool enter)
        {
            base.onMouseEnter(enter);

            if (enter)
            {
                hqRef.setup.conditions.OnEvent(ToGG.Data.EventType.ReadingQuestMission, null);
            }
        }

        protected override void createToolTip()
        {
            base.createToolTip();

            var richbox = hqRef.setup.conditions.questDescription();
            HudLib.AddTooltipText(tooltip, richbox, Dir4.S, area,
                area, true, false);
        }
    }

    class DoomBanner : AbsHoverBanner
    {
        int firstInteractionTurn = int.MinValue;
        TimeStamp interactTime;

        public DoomBanner(float rightPos)
            :base(rightPos, SpriteName.DoomBanner, SpriteName.DoomBannerHighlight)
        { }

        protected override void onMouseEnter(bool enter)
        {
            base.onMouseEnter(enter);

            if (enter)
            {
                hqRef.playerHud.createDoomBar();

                if (firstInteractionTurn < 0 ||
                    firstInteractionTurn == hqRef.players.localHost.TurnsCount)
                {
                    var members = new List<HUD.RichBox.AbsRichBoxMember>
                    {
                        new HUD.RichBox.RbBeginTitle(),
                        new HUD.RichBox.RbText("Doom track"),
                        new HUD.RichBox.RbNewLine(false),
                    };

                    if (DoomTrack.ViewTime)
                    {
                        members.AddRange(new List<HUD.RichBox.AbsRichBoxMember>
                        {
                            new HUD.RichBox.RbImage(SpriteName.DoomClockIconGray),
                            new HUD.RichBox.RbImage(SpriteName.DoomClockIcon),
                            new HUD.RichBox.RbText(" Doom clock. Will increase each turn."),
                            new HUD.RichBox.RbNewLine(false),
                        });
                    }

                    members.AddRange(new List<HUD.RichBox.AbsRichBoxMember>
                    {
                        new HUD.RichBox.RbImage(SpriteName.DoomSkullGray),
                        new HUD.RichBox.RbImage(SpriteName.DoomSkull),
                        new HUD.RichBox.RbText(" Doom skull. A full bar will end the level in defeat."),
                        new HUD.RichBox.RbNewLine(false),

                        new HUD.RichBox.RbImage(SpriteName.GoldChestOpen),
                        new HUD.RichBox.RbImage(SpriteName.SilverChestOpen),
                        new HUD.RichBox.RbImage(SpriteName.BronzeChestOpen),
                        new HUD.RichBox.RbText(" Loot Chests. Complete the level before the chests are closed, to recieve extra rewards."),
                    });

                    //tooltip = new Graphics.ImageGroup();
                    HudLib.AddTooltipText(tooltip, members, Dir4.S, hqRef.playerHud.doombar.bgArea, null, true, false);

                    interactTime = TimeStamp.Now();
                }
            }
            else
            {
                //baseImage.SetSpriteName(SpriteName.DoomBanner);
                hqRef.playerHud.removeDoomBar();

                if (firstInteractionTurn < 0 && interactTime.msPassed(500))
                {
                    firstInteractionTurn = hqRef.players.localHost.TurnsCount;
                }
            }
        }

        protected override void createToolTip()
        {
            base.createToolTip();

        }
    }
}
