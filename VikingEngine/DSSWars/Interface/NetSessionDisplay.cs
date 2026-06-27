using Steamworks;
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
using VikingEngine.Network;

namespace VikingEngine.DSSWars.Interface
{
    class NetSessionDisplay
    {
        public const string PAGE_BANWARNING = "ban warn";
        public const string PAGE_REQUESTBLOCK = "request block";
        public const string PAGE_KICK = "kick";
        public const string PAGE_BLOCK = "block";
        public RemotePlayer selectedPlayer = null;
        public RemotePlayer sendGiftTo = null;

        public bool ClientInteractDisplay => selectedPlayer != null;

        public void BanWarning(LocalPlayer player, RichBoxContent content, RichMenu menu)
        {
            HudLib.returnButton(content, menu, true, null);
            content.h1("Send ban warning", HudLib.TitleColor_Head);
            content.newLine();
            selectedPlayer.addNetGamerToHud(content, true, true);

            content.newParagraph();
            for (BadBehaviourType behaviourType = 0; behaviourType < BadBehaviourType.NUM; behaviourType++)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbText(behaviourType.ToString())
                    }, new RbAction1Arg<BadBehaviourType>((BadBehaviourType selected)=>
                    {
                        var w = Ref.netSession.BeginWritingPacket(PacketType.WarnPlayer, PacketReliability.Reliable,  SendPacketTo.OneSpecific, selectedPlayer.networkPeer.peer.fullId, null);
                        w.Write((byte)selected);

                        ((PlayState)DssRef.state).BanWarning(DssRef.state.LocalHost(), selectedPlayer, selected);

                        selectedPlayer.networkPeer.peer.storedData.ban = BanStatus.Warning;

                    }, behaviourType)));
                content.newLine();
            }
        }
        public void RequestBlock(LocalPlayer player, RichBoxContent content, RichMenu menu)
        {
            HudLib.returnButton(content, menu, true, null);
            content.h1("Request block", HudLib.TitleColor_Head);
            content.newLine();
            selectedPlayer.addNetGamerToHud(content, true, true);
            content.text("Will be sent to the host", HudLib.InfoYellow_Light);

            content.newParagraph();

            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Add to your own block list") },
                Ref.netsett.alsoBlockOnRequestProperty));

            content.newParagraph();
            for (BadBehaviourType behaviourType = 0; behaviourType < BadBehaviourType.NUM; behaviourType++)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbText(behaviourType.ToString())
                    }, new RbAction1Arg<BadBehaviourType>((BadBehaviourType selected) =>
                    {
                        var w = Ref.netSession.BeginWritingPacket(PacketType.RequestPlayerBan, PacketReliability.Reliable, SendPacketTo.Host, 0, null);
                        selectedPlayer.networkPeer.writeNetID(w);
                        w.Write((byte)selected);
                        if (Ref.netsett.alsoBlockOnRequest)
                        {
                            selectedPlayer.networkPeer.peer.storedData.ban = BanStatus.Banned;
                            Ref.netsett.setUpdatedStoredGamer(selectedPlayer.networkPeer.peer.storedData);
                        }

                        DssRef.state.LocalHost().hud.messages.Add(new RichBoxContent()
                        {
                            new RbText("Request sent")
                        });

                        menu.menuBack();

                    }, behaviourType)));
                content.newLine();
            }
        }
        public void Kick(LocalPlayer player, RichBoxContent content, RichMenu menu)
        {
            content.h1("Kick player", HudLib.TitleColor_Head);
            selectedPlayer.addNetGamerToHud(content, true, false);

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbText(Ref.langOpt.Hud_OK)
            }, new RbAction(() =>
            {
                ((PlayState)DssRef.state).KickPlayer(selectedPlayer.networkPeer.peer);
                menu.menuBack();
            })));

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                new RbText(Ref.langOpt.Hud_Cancel)
            }, new RbAction(() =>
            {
                menu.menuBack();
            })));
        }
        public void Block(LocalPlayer player, RichBoxContent content, RichMenu menu)
        {
            content.h1("Block player", HudLib.TitleColor_Head);
            selectedPlayer.addNetGamerToHud(content, true, false);

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbText(Ref.langOpt.Hud_OK)
            }, new RbAction(() =>
            {
                ((PlayState)DssRef.state).BlockPlayer(selectedPlayer.networkPeer.peer);
                menu.menuBack();
            })));

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                new RbText(Ref.langOpt.Hud_Cancel)
            }, new RbAction(() =>
            {
                menu.menuBack();
            })));
        }

        public void invite(RichBoxContent content)
        {
            content.newLine();

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.SteamIcon),
                new RbSpace(),
                new RbImage(SpriteName.WarsHudIconNetwork),
                new RbSpace(),
                new RbText("Invite")
            }, new RbAction(Ref.netSession.Invite), new RbTooltip_Text("Open Steam overlay")));
        }

        public void overviewToHud(LocalPlayer player, RichBoxContent content)
        {
            if (sendGiftTo != null)
            {
                giftMenu(player, content);
            }
            else
            {
                content.h2("Net session", HudLib.TitleColor_Head);

                gamerButton(player);
                content.newLine();
                content.Add(new RbSeperationLine());

                var remoteC = DssRef.state.remotePlayers.counter();
                while (remoteC.Next())
                {
                    gamerButton(remoteC.sel);
                    
                    remoteC.sel.addNetPingToHud(content);
                }
                content.Add(new RbSeperationLine());
            }

            void gamerButton(AbsHumanPlayer gamer)
            {
                content.newLine();
                var settings = gamer.NetClientSettings();

                RichBoxContent buttonContent = new RichBoxContent();
                gamer.addNetGamerToHud(buttonContent, true, true);

               

                content.Add(new ArtButton(RbButtonStyle.Outline, buttonContent, new RbAction1Arg<AbsHumanPlayer>(
                    (AbsHumanPlayer select) => { selectedPlayer = select as RemotePlayer; player.hud.needRefresh = true; }, gamer),
                    new RbTooltip_Text(DssRef.lang.Tutorial_SelectInput), gamer.IsRemotePlayer()));

                if (settings.clientSettings.recieveGifts == GiftRecieveOption.Allow ||
                    (settings.clientSettings.recieveGifts == GiftRecieveOption.FriendsOnly && gamer.IsFriend()))
                {
                    gamer.giftedAchievements.ToHud(content, player, gamer as RemotePlayer, this);
                }
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

#if DEBUG
            bool[] included = new bool[(int)GiftedAchievementType.NUM];
#endif

            foreach (var category in GiftedAchievementCollection.Categories)
            {
                content.newParagraph();
                foreach (var type in category)
                {
                    var gift = GiftedAchievementCollection.Get(type);

#if DEBUG
                    included[(int)type] = true;
                    
#endif

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

#if DEBUG
            for (int i = 0; i < included.Length; i++)
            {
                if (!included[i])
                {
                    Debug.Log("missing " + ((GiftedAchievementType)i).ToString());
                }
            }

#endif
        }


        public void clientToHud(LocalPlayer player, RichBoxContent content, RichMenu menu)
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
            selectedPlayer.addNetGamerToHud(content, true, true);

            selectedPlayer.addNetPingToHud(content);

            content.newParagraph();
            DiplomacyDisplay diplomacyDisplay = new DiplomacyDisplay(player);
            diplomacyDisplay.toHud(content, selectedPlayer.faction, false);

            content.Add(new RbSeperationLine());
            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.SteamIcon),
                    new RbSpace(),
                    new RbText("User profile")
                }, new RbAction2Arg<string, CSteamID>(Steamworks.SteamFriends.ActivateGameOverlayToUser, "steamid", selectedPlayer.networkPeer.peer.SteamID),
               new RbTooltip_Text("Open Steam overlay")));

            content.newLine();
            if (Ref.netSession.IsHost)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Send ban warning") },
                     new RbAction2Arg<string, StackOption>(menu.OpenMenu, PAGE_BANWARNING, StackOption.Stack)));

                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Kick player") },
                    new RbAction2Arg<string, StackOption>(menu.OpenMenu, PAGE_KICK, StackOption.Stack)));

                content.newLine();
                content.Add(new ArtButton(Ref.netSession.IsHost ? RbButtonStyle.Primary : RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText("Block player") },
                    new RbAction2Arg<string, StackOption>(menu.OpenMenu, PAGE_BLOCK, StackOption.Stack)));
            }
            else
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Request block") },
                     new RbAction2Arg<string, StackOption>(menu.OpenMenu, PAGE_REQUESTBLOCK, StackOption.Stack)));

                //content.newLine();
                //content.Add(new ArtButton(Ref.netSession.IsHost ? RbButtonStyle.Primary : RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText("Block player") },
                //    new RbAction2Arg<string, StackOption>(menu.OpenMenu, PAGE_BLOCK, StackOption.Stack), new RbTooltip_Text("")));
            }

            
        }
    }
}
