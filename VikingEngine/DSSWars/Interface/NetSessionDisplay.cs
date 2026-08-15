using Microsoft.Xna.Framework;
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
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.LootFest.Players;
using VikingEngine.Network;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.DSSWars.Interface
{
    class NetSessionDisplay
    {
        public const string PAGE_BANWARNING = "ban warn";
        public const string PAGE_REQUESTBLOCK = "request block";
        public const string PAGE_KICK = "kick";
        public const string PAGE_BLOCK = "block";
        public const string PAGE_RECOLOR = "recolor";
        public const string PAGE_DEBUG = "debug";
        public RemotePlayer selectedPlayer = null;
        public RemotePlayer sendGiftTo = null;

        
        public bool ClientInteractDisplay => selectedPlayer != null;

        public void overviewToHud(LocalPlayer player, RichBoxContent content, RichMenu menu)
        {
            if (sendGiftTo != null)
            {
                giftMenu(player, content);
            }
            else
            {
                content.h2(DssRef.lang.Multiplayer_NetSession, HudLib.TitleColor_Head);

                gamerButton(player);
                content.newLine();
                content.Add(new RbSeperationLine());

                var remoteC = DssRef.state.remotePlayers.counter();
                while (remoteC.Next())
                {
                    gamerButton(remoteC.sel);

                    remoteC.sel.addNetPingToHud(content);
                }

                content.newLine();
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("*debug") }, new RbAction2Arg<string, StackOption>(menu.OpenMenu, PAGE_DEBUG, StackOption.Stack)));

                content.Add(new RbSeperationLine());
            }

            void gamerButton(AbsHumanPlayer gamer)
            {
                content.newLine();
                var settings = gamer.NetClientSettings();

                RichBoxContent flagContent = new RichBoxContent();
                gamer.addFlagToHud(flagContent);
                content.Add(new ArtButton(RbButtonStyle.Outline, flagContent,
                    new RbAction(() => {
                        gamer.pfaction.GetFaction()?.refreshMainCity();
                        var main = gamer.pfaction.GetFaction()?.mainCity;
                        if (main != null)
                        {
                            player.gameControls.map.cameraFocus = main;
                        }
                    }), new RbTooltip_Text(DssRef.lang.InputActionName_NextCity)));

                RichBoxContent buttonContent = new RichBoxContent();
                gamer.addNetGamerToHud(buttonContent, false, true);

                content.Add(new ArtButton(gamer.HasSupportDLC() ? RbButtonStyle.GoldOutline : RbButtonStyle.Outline, buttonContent, new RbAction1Arg<AbsHumanPlayer>(
                    (AbsHumanPlayer select) => { selectedPlayer = select as RemotePlayer; player.hud.needRefresh = true; }, gamer),
                    new RbTooltip_Text(DssRef.lang.Tutorial_SelectInput), gamer.IsRemotePlayer()));

                if (gamer.profile.casualControls)
                {
                    content.Add(new ArtButton(RbButtonStyle.HoverArea, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudCasualMode) },
                        null, new RbTooltip_Text(DssRef.lang.Settings_CasualControls)));
                }

                if (settings.clientSettings.recieveGifts == GiftRecieveOption.Allow ||
                    (settings.clientSettings.recieveGifts == GiftRecieveOption.FriendsOnly && gamer.IsFriend()))
                {
                    gamer.giftedAchievements.ToHud(content, player, gamer as RemotePlayer, this);
                }
            }
        }

        public void BanWarning(LocalPlayer player, RichBoxContent content, RichMenu menu)
        {
            HudLib.returnButton(content, menu, true, null);
            content.h1(string.Format(DssRef.lang.Hud_SendX, DssRef.lang.Multiplayer_BanWarning), HudLib.TitleColor_Head);
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
            content.h1(DssRef.lang.Multiplayer_RequestBlockPlayer, HudLib.TitleColor_Head);
            content.newLine();
            selectedPlayer.addNetGamerToHud(content, true, true);
            content.text(DssRef.lang.Multiplayer_SentToHost, HudLib.InfoYellow_Light);

            content.newParagraph();

            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Multiplayer_AddToOwnBlocks) },
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
                            new RbText(DssRef.lang.Multiplayer_Message_RequestSent)
                        });

                        menu.menuBack();

                    }, behaviourType)));
                content.newLine();
            }
        }
        public void Kick(LocalPlayer player, RichBoxContent content, RichMenu menu)
        {
            content.h1(DssRef.lang.Multiplayer_KickPlayer, HudLib.TitleColor_Head);
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
            content.h1(DssRef.lang.Multiplayer_BlockPlayer, HudLib.TitleColor_Head);
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

       

        public void recolor(LocalPlayer player, RichBoxContent content, RichMenu menu)
        {
            HudLib.returnButton(content, menu, true, null);
            content.h1(DssRef.lang.Editor_Color_Recolor, HudLib.TitleColor_Head);
            content.newLine();
            selectedPlayer.addNetGamerToHud(content, true, false);

            content.newParagraph();
            const int HueSteps = 20;
            const double StartHue = 0;

            double[] lightOptions = [0.2, 0.5, 0.8];
            double[] saturationOptions = [0.7];//0.4, 0.9];

            foreach (var lightness in lightOptions)
            {
                foreach (var saturation in saturationOptions)
                {
                    double hue = StartHue;
                    for (int i = 0; i < HueSteps; i++)
                    {
                        hue += 1.0 / HueSteps;
                        Color col = lib.HSL2RGB(hue, saturation, lightness);

                        content.Add(new ArtImageButton(new List<AbsRichBoxMember> { new RbImage(SpriteName.WhiteArea, 1, col) },
                            new RbAction1Arg<Color>(setColor, col))
                        { SpaceAfter = 0, });
                    }
                    content.newLine();
                }
            }

            void setColor(Color selected)
            {
                selectedPlayer.SetColor(selected, true);
                menu.menuBack();
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

            content.h2(DssRef.lang.GiftedAchievements, HudLib.TitleColor_Head);
            content.text(DssRef.lang.GiftedAchievements_Description, HudLib.InfoYellow_Light);

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
                            content.h2(string.Format( DssRef.lang.Hud_SendX, string.Empty), HudLib.TitleColor_Action);
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

            if (!DssRef.state.UpdateReady())
            {
                content.icontext(SpriteName.IconSandGlass, DssRef.lang.Hud_Loading, HudLib.InfoYellow_Light);
                return;
            }

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
                //player.hud.needRefresh = true;
            });
            content.Add(tabGroup);

            content.newLine();

            //TITLE
            selectedPlayer.addNetGamerToHud(content, true, true);

            selectedPlayer.addNetPingToHud(content);

            content.newParagraph();
            DiplomacyDisplay diplomacyDisplay = new DiplomacyDisplay(player);
            diplomacyDisplay.toHud(content, selectedPlayer.pfaction.GetFaction(), false);

            content.Add(new RbSeperationLine());

            content.newParagraph();
            HudLib.Label(content, SpriteName.VoiceSoundOn, DssRef.lang.VoiceTitle);
            content.hspace();
            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbImage(SpriteName.VoiceDisabled) },
                new RbAction(selectedPlayer.mute, RbSoundType.Stop),
                new RbTooltip_Text(DssRef.lang.VoiceMute)));
            content.Add(new RbDragButton(new DragButtonSettings(new IntervalF(0, 2f), 0.1f),
                selectedPlayer.voiceVolume, true));

            content.newLine();
            if (Ref.netSession.IsHost)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.VoxelEditorBucket),
                    new RbSpace(0.5f),
                    new RbText(DssRef.lang.Editor_Color_Recolor) },
                    new RbAction2Arg<string, StackOption>(menu.OpenMenu, PAGE_RECOLOR, StackOption.Stack)));
                
                
                content.newParagraph();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.cmdWarningTriangle),
                    new RbSpace(0.5f),
                    new RbText(string.Format(DssRef.lang.Hud_SendX, DssRef.lang.Multiplayer_BanWarning)) },
                     new RbAction2Arg<string, StackOption>(menu.OpenMenu, PAGE_BANWARNING, StackOption.Stack)));

                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconExit),
                    new RbSpace(0.5f),
                    new RbText(DssRef.lang.Multiplayer_KickPlayer) },
                    new RbAction2Arg<string, StackOption>(menu.OpenMenu, PAGE_KICK, StackOption.Stack)));

                content.newLine();
                content.Add(new ArtButton(Ref.netSession.IsHost ? RbButtonStyle.Primary : RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconBlockedPlayer),
                    new RbSpace(0.5f),
                    new RbText(DssRef.lang.Multiplayer_BlockPlayer) },
                    new RbAction2Arg<string, StackOption>(menu.OpenMenu, PAGE_BLOCK, StackOption.Stack)));
            }
            else
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconBlockedPlayer),
                    new RbSpace(0.5f),
                    new RbText(DssRef.lang.Multiplayer_RequestBlockPlayer) },
                     new RbAction2Arg<string, StackOption>(menu.OpenMenu, PAGE_REQUESTBLOCK, StackOption.Stack)));

            }
            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.SteamIcon),
                    new RbSpace(),
                    new RbText(DssRef.lang.Steam_UserProfile)
                }, new RbAction2Arg<string, CSteamID>(Steamworks.SteamFriends.ActivateGameOverlayToUser, "steamid", selectedPlayer.networkPeer.peer.SteamID),
               new RbTooltip_Text(DssRef.lang.Steam_OpenSteamOverlay)));

            
        }

        public void checkAlive()
        {
            if (selectedPlayer.isDeleted)
            {
                selectedPlayer = null;
            }
        }
    }
}
