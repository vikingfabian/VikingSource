using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.Interface
{
    class NetSessionDisplay
    {
        RemotePlayer selectedPlayer = null;
        public RemotePlayer sendGiftTo = null;

        public bool ClientInteractDisplay => selectedPlayer != null;

        public void overviewToHud(LocalPlayer player, RichBoxContent content)
        {
            if (sendGiftTo != null)
            {
                giftMenu(player, content);
            }
            else
            {
                content.h2(".Net session", HudLib.TitleColor_Head);

                gamerButton(player);
                content.newLine();
                content.Add(new RbSeperationLine());

                var remoteC = DssRef.state.remotePlayers.counter();
                while (remoteC.Next())
                {
                    gamerButton(remoteC.sel);
                    //content.newLine();

                    //RichBoxContent buttonContent = new RichBoxContent();
                    //remoteC.sel.addNetGamerToHud(buttonContent, true);

                    //content.Add(new ArtButton(RbButtonStyle.Outline, buttonContent, new RbAction1Arg<RemotePlayer>(
                    //    (RemotePlayer select) => { selectedPlayer = select; player.hud.needRefresh = true; }, remoteC.sel),
                    //    new RbTooltip_Text(DssRef.lang.Tutorial_SelectInput)));

                    //remoteC.sel.giftedAchievements.ToHud(content, player, remoteC.sel, this);

                    remoteC.sel.addNetPingToHud(content);
                }
                content.Add(new RbSeperationLine());
            }

            void gamerButton(AbsHumanPlayer gamer)
            {
                content.newLine();

                RichBoxContent buttonContent = new RichBoxContent();
                gamer.addNetGamerToHud(buttonContent, true);

                content.Add(new ArtButton(RbButtonStyle.Outline, buttonContent, new RbAction1Arg<AbsHumanPlayer>(
                    (AbsHumanPlayer select) => { selectedPlayer = select as RemotePlayer; player.hud.needRefresh = true; }, gamer),
                    new RbTooltip_Text(DssRef.lang.Tutorial_SelectInput), gamer.IsRemotePlayer()));

                gamer.giftedAchievements.ToHud(content, player, gamer as RemotePlayer, this);
            }
        }

        void giftMenu(LocalPlayer player, RichBoxContent content)
        {
            var hasGifts = sendGiftTo.giftedAchievements.HasGiftsCollection();

            content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> {
                    new RbImage( SpriteName.WarsHudIconReturn, 0.8f),
                    new RbSpace(),
                    new RbText(DssRef.lang.Hud_ReturnToPrevious)
                    }, new RbAction(() =>
                    {
                        sendGiftTo = null;
                        player.hud.needRefresh = true;
                    }, RbSoundType.Back)));

            content.h2("Gifted achievements", HudLib.TitleColor_Head);
            content.text("Reward your friends bad behaiviour", HudLib.InfoYellow_Light);

            foreach (var category in GiftedAchievementCollection.Categories)
            {
                content.newParagraph();
                foreach (var type in category)
                {
                    var gift = GiftedAchievementCollection.Get(type);

                    //content.newLine();
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbImage(GiftedAchievement.DefaultIcon),
                        new RbSpace(0.5f),
                        new RbText(gift.name)
                        }, new RbAction1Arg<GiftedAchievementType>((GiftedAchievementType selected) =>
                        {
                            ((PlayState)DssRef.state).sendGiftedAchievement(selected, sendGiftTo);

                            sendGiftTo = null;
                            player.hud.needRefresh = true;

                        }, type), new RbTooltip((RichBoxContent content, object tag) =>
                        {

                            var info = GiftedAchievementCollection.Get((GiftedAchievementType)tag);
                            content.h2("Send", HudLib.TitleColor_Action);
                            content.newParagraph();
                            content.text(info.description, HudLib.InfoYellow_Light);

                        }, type), !hasGifts.Contains(type)));
                }

            }
        }


        public void clientToHud(LocalPlayer player, RichBoxContent content)
        {

            content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> {
                    new RbImage( SpriteName.WarsHudIconReturn, 0.8f),
                    new RbSpace(),
                    new RbText(DssRef.lang.Hud_ReturnToPrevious)
                    }, new RbAction(() =>
                    { selectedPlayer = null; player.hud.needRefresh = true; }, RbSoundType.Back)));

            content.newLine();

            //TABS
            var tabs = new List<ArtTabMember>(DssRef.state.remotePlayers.Count);
            var remoteC = DssRef.state.remotePlayers.counter();
            int index = 0;
            int sel = 0;
            while (remoteC.Next())
            {
                tabs.Add(new ArtTabMember(new List<AbsRichBoxMember>
                    {
                       new RbText(remoteC.sel.Name)
                    }));
                if (selectedPlayer == remoteC.sel)
                {
                    sel = index;
                }
                index++;
            }

            var tabGroup = new ArtTabgroup(tabs, sel, (int select) =>
            {
                var gamer = DssRef.state.remotePlayers.GetIndex_Safe(select);
                if (gamer != null)
                {
                    selectedPlayer = gamer;
                }
                player.hud.needRefresh = true;
            });
            content.Add(tabGroup);

            content.newLine();

            //TITLE
            selectedPlayer.addNetGamerToHud(content, true);
            //selectedPlayer.giftedAchievements.ToHud(content, player, this);

            selectedPlayer.addNetPingToHud(content);

            //var diplomacy = player.GetOrCreateToPlayerDiplomacy(selected);
            //diplomacy.
            content.newParagraph();
            DiplomacyDisplay diplomacyDisplay = new DiplomacyDisplay(player);
            diplomacyDisplay.toHud(content, selectedPlayer.faction, false);

        }


    }
}
