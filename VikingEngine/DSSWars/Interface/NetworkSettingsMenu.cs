using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Network;

namespace VikingEngine.DSSWars.Interface
{
    class NetworkSettingsMenu
    {
        public const string MultiplayerSettings = "net sett";
        const string UnlockPublic = "net sett unlock public";
        const string UnlockPublic_Sure = "net sett unlock public_sure";
        const string UnlockPvp = "net sett unlock pvp";
        const string UnlockPvp_Sure = "net sett unlock pvp_sure";
        public const string BlockList = "blocklist";

        static readonly RelationType[] DefaultRelationsOptions = { RelationType.RelationType0_Neutral, RelationType.RelationType3_Ally, RelationType.RelationTypeN4_War };

        RichMenu menu;
        bool bMainMenu;
        public NetworkSettingsMenu(RichMenu menu, bool mainMenu)
        {
            this.menu = menu;
            this.bMainMenu = mainMenu;
        }

        public void refresh()
        {
            switch (menu.menuStack.LastOrDefault())
            {
                case MultiplayerSettings:
                    multiplayerSettingsMenu();
                    break;
                case UnlockPublic:
                    {
                        RichBoxContent content = new RichBoxContent();

                        content.h1(string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Hud_Unlock, DssRef.todoLang.Unlock_PublicGames), HudLib.TitleColor_Head);
                        content.h2(SpriteName.cmdWarningTriangle, DssRef.lang.Lobby_WarningTitle, HudLib.NotAvailableColor);

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.todoLang.UnlockPublic_Warning1));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.todoLang.UnlockPublic_Warning2));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.todoLang.Unlock_WarningBadExperience));

                        content.newParagraph();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbText(DssRef.todoLang.Hud_Accept)
                        }, new RbAction2Arg<string, StackOption>(openmenu, UnlockPublic_Sure, StackOption.ReplaceLast))
                        { fillWidth = true });

                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                            new RbText(DssRef.lang.Hud_Cancel)
                        }, new RbAction(menu.menuBack, RbSoundType.Back))
                        { fillWidth = true });

                        menu.Refresh(content);
                    }
                    break;
                case UnlockPublic_Sure:
                    unlockMultiplayer_Sure(true);
                    break;

                case UnlockPvp:
                    {
                        RichBoxContent content = new RichBoxContent();

                        content.h1(string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Hud_Unlock, DssRef.todoLang.Unlock_PlayerVersusPlayer), HudLib.TitleColor_Head);
                        content.h2(SpriteName.cmdWarningTriangle, DssRef.lang.Lobby_WarningTitle, HudLib.NotAvailableColor);

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.todoLang.UnlockPvp_Warning1));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.todoLang.UnlockPvp_Warning2));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.todoLang.Unlock_WarningBadExperience));

                        content.newParagraph();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbText(DssRef.todoLang.Hud_Accept)
                        }, new RbAction2Arg<string, StackOption>(openmenu, UnlockPvp_Sure, StackOption.ReplaceLast))
                        { fillWidth = true });

                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                            new RbText(DssRef.lang.Hud_Cancel)
                        }, new RbAction(menu.menuBack, RbSoundType.Back))
                        { fillWidth = true });

                        menu.Refresh(content);
                    }
                    break;
                case UnlockPvp_Sure:
                    {
                        unlockMultiplayer_Sure(false);
                    }
                    break;
                case BlockList:
                    {
                        
                        RichBoxContent content = new RichBoxContent();
                        blockList(content);
                        menu.Refresh(content);
                    }
                    break;
            }
        }

        public int blockList(RichBoxContent content)
        {
            int count = 0;
            HudLib.returnButton(content, menu, true, null);

            content.h1(DssRef.todoLang.BlockedPlayersTitle, HudLib.TitleColor_Head);

            for (int i = 0; i < Ref.netsett.storedGamers.Count; i++)
            {
                if (Ref.netsett.storedGamers.array[i].ban == BanStatus.Banned)
                {
                    count++;
                    content.newLine();
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                        new List<AbsRichBoxMember> { new RbText(Ref.netsett.storedGamers.array[i].name) },
                        new RbAction1Arg<int>((int selected) =>
                        {
                            var m = Ref.netsett.storedGamers.array[selected];
                            m.ban = BanStatus.None;
                            Ref.netsett.storedGamers.array[selected] = m;
                            DssRef.storage.Save(null);
                        }, i), new RbTooltip_Text(DssRef.todoLang.ClickToRemoveBan)));
                }
            }
            if (count == 0)
            {
                content.text(DssRef.lang.Hud_EmptyList, HudLib.InfoYellow_Light);
            }

            return count;
        }

        public void openmenu(string menuName, StackOption stack)
        {
            menu.OpenMenu(menuName, stack);
        }

        void multiplayerSettingsMenu()
        {
            RichBoxContent content = new RichBoxContent();
            multiplayerSettingsMenuContent(content);
            menu.Refresh(content);
        }
        public void multiplayerSettingsMenuContent(RichBoxContent content)
        {
            

            if (bMainMenu)
            {
                if (menu.menuStack.Count > 1)
                {
                    HudLib.returnButton(content, menu, true, null);
                    content.newLine();
                }

                content.h1(DssRef.todoLang.Lobby_Category_MultiplayerSettings, HudLib.TitleColor_Head);

                content.h2(SpriteName.WarsHudIconHost, DssRef.todoLang.HostSettingsTitle, HudLib.TitleColor_Head2);

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.Network_PlayOffline) },
                Ref.netsett.OfflineProperty));

                if (!Ref.netsett.hostNetwork)
                {
                    return;
                }
                var publicityOptions = new DropDownBuilder("lobby public");
                {
                    publicityOptions.AddOption(DssRef.todoLang.JoinPermission_Private, Ref.netsett.lobbyPublicity == LobbyPublicity.Private, false,
                        new RbAction1Arg<LobbyPublicity>(setLobbyPublicity, LobbyPublicity.Private), null);
                    publicityOptions.AddOption(DssRef.todoLang.JoinPermission_FriendsOnly, Ref.netsett.lobbyPublicity == LobbyPublicity.FriendsOnly, false,
                                        new RbAction1Arg<LobbyPublicity>(setLobbyPublicity, LobbyPublicity.FriendsOnly), null);
                    var publicOpt = publicityOptions.AddOption(DssRef.todoLang.JoinPermission_Public, Ref.netsett.lobbyPublicity == LobbyPublicity.Public, false,
                                        new RbAction1Arg<LobbyPublicity>(setLobbyPublicity, LobbyPublicity.Public), null);
                    publicOpt.enabled = Ref.netsett.unlockPublicGames;

                }
                publicityOptions.Build(content, SpriteName.NO_IMAGE, DssRef.todoLang.JoinPermission_Title, menu);

                content.newLine();
                HudLib.Label(content, SpriteName.WarsHudIconPlayerCount, DssRef.todoLang.MaxPlayerCount);
                content.hspace();
                RbDragButton.RbDragButtonGroup(content, new List<float> { 10 },
                    new DragButtonSettings(2, 64, 1), Ref.netsett.MaxPlayerCountProperty, false);

                content.newLine();
                HudLib.Label(content, SpriteName.WarsHudIconDistanceOnMap, DssRef.todoLang.DistanceBetweenPlayers);
                content.hspace();
                content.Add(new RbDragButton(new DragButtonSettings(0, 8, 1), Ref.netsett.PlayerSpacingProperty));


                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconHandicap),
                    new RbSpace(0.5f),
                    new RbText(DssRef.todoLang.AllowHandicap) },
                    Ref.netsett.allowHandicapProperty));

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudCasualMode),
                    new RbSpace(0.5f),
                    new RbText(DssRef.todoLang.AllowCasualControls) },
                    Ref.netsett.allowCasualControlsProperty, new RbTooltip_Text(DssRef.lang.Settings_CasualControls_Description)));

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.VoxelEditorBucket),
                    new RbSpace(0.5f),
                    new RbText(DssRef.todoLang.AutoRecolorPlayerFlags) },
                    Ref.netsett.autoRecolorFlagsProperty, null));

                content.newLine();

            }
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconBlockedPlayer),
                    new RbSpace(0.5f),
                    new RbText(DssRef.todoLang.BlockedPlayersTitle) },
                new RbAction2Arg<string, StackOption>(openmenu, BlockList, StackOption.Stack)));

            content.newParagraph();
            if (bMainMenu)
            {
                content.h2(DssRef.todoLang.PlayerInteractionTitle, HudLib.TitleColor_Label);

                var defDiplomacyOptions = new DropDownBuilder("def diplomacy");
                {
                    foreach (var relation in DefaultRelationsOptions)
                    {
                        IconName.Relation(relation, out var dipIcon, out var dipName);
                        defDiplomacyOptions.AddOption(dipIcon, dipName, relation == Ref.netsett.hostSettings.startDiplomacy,
                            relation == RelationType.RelationType0_Neutral,
                            new RbAction1Arg<RelationType>((RelationType rel) =>
                            {
                                Ref.netsett.hostSettings.startDiplomacy = relation;
                                Ref.netsett.settingsHasChanged = true;
                            }, relation), null);
                    }
                }
                defDiplomacyOptions.Build(content, SpriteName.WarsDiplomaticPoint, DssRef.todoLang.DefaultDiplomacy, menu);

                playerInteractSettings(content, true);



                //"Join Permissions"}
                content.Add(new RbSeperationLine());

                content.newParagraph();
                content.h2(SpriteName.WarsHudIconClient, DssRef.todoLang.ClientSettingsTitle, HudLib.TitleColor_Head2);

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconHandicap),
                    new RbSpace(0.5f),
                    new RbText(DssRef.todoLang.UseHandicap) },
                    Ref.netsett.useHandicapProperty));

                if (Ref.netsett.clientSettings.useHandicap)
                {
                    var aggroOptions = new DropDownBuilder("handicap aggro");
                    {
                        for (HandicapLevel lvl = HandicapLevel.High; lvl <= HandicapLevel.None; lvl++)
                        {
                            string caption;
                            switch (lvl)
                            {
                                case HandicapLevel.High:
                                    caption = DssRef.lang.Hud_High;
                                    break;
                                default:
                                case HandicapLevel.Default:
                                    caption = DssRef.todoLang.Hud_Default;
                                    break;
                                case HandicapLevel.Low:
                                    caption = DssRef.lang.Hud_Low;
                                    break;
                                case HandicapLevel.None:
                                    caption = DssRef.lang.Settings_Mode_Peaceful;
                                    break;

                            }
                            aggroOptions.AddOption(caption,
                                lvl == Ref.netsett.clientSettings.handicap_botAggression,
                                lvl == HandicapLevel.Default,
                                new RbAction1Arg<HandicapLevel>((HandicapLevel selected) =>
                                {
                                    menu.CloseDropDown();
                                    Ref.netsett.clientSettings.handicap_botAggression = selected;
                                    Ref.netsett.settingsHasChanged = true;
                                }, lvl), null);
                        }
                    }
                    aggroOptions.Build(content, SpriteName.NO_IMAGE, DssRef.todoLang.DifficultyDescription_BotAggression, menu);

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.WarsUnitIcon_Honorguard),
                        new RbSpace(0.5f),
                        new RbText(string.Format( DssRef.todoLang.Hud_GetExtraX, DssRef.lang.UnitType_HonorGuard)) },
                        Ref.netsett.handicap_extraHonorGuardsProperty));

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.WarsIcon_Resources),
                        new RbSpace(0.5f),
                        new RbText(DssRef.todoLang.ResourceBoost) },
                        Ref.netsett.handicap_resourceBoostProperty));

                    var taxOptions = new DropDownBuilder("handicap tax");
                    {
                        for (HandicapLevel lvl = HandicapLevel.High; lvl <= HandicapLevel.Low; lvl++)
                        {
                            string caption;
                            switch (lvl)
                            {
                                case HandicapLevel.High:
                                    caption = DssRef.lang.Hud_High;
                                    break;
                                default:
                                case HandicapLevel.Default:
                                    caption = DssRef.todoLang.Hud_Default;
                                    break;
                                case HandicapLevel.Low:
                                    caption = DssRef.lang.Hud_Low;
                                    break;

                            }
                            taxOptions.AddOption(caption,
                                lvl == Ref.netsett.clientSettings.handicap_taxIncome,
                                lvl == HandicapLevel.Default,
                                new RbAction1Arg<HandicapLevel>((HandicapLevel selected) =>
                                {
                                    menu.CloseDropDown();
                                    Ref.netsett.clientSettings.handicap_taxIncome = selected;
                                    Ref.netsett.settingsHasChanged = true;
                                }, lvl), null);
                        }
                    }
                    taxOptions.Build(content, SpriteName.rtsIncome, DssRef.lang.Economy_TaxIncome, menu);
                }

                content.h2(DssRef.todoLang.PlayerInteractionTitle, HudLib.TitleColor_Label);
                playerInteractSettings(content, false);
                content.newParagraph();
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.DefaultPeaceful) }, new RbAction(Ref.netsett.resetPeaceful, RbSoundType.Paste))
                { overrideBgColor = Color.DarkGray });
                content.newLine();
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.DefaultCoOptional) }, new RbAction(Ref.netsett.resetMixed, RbSoundType.Paste))
                { overrideBgColor = Color.DarkGray });

                if (Ref.netsett.unlockPvp)
                {
                    content.newLine();
                    content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.DefaultHardcore) }, new RbAction(Ref.netsett.resetHardcore, RbSoundType.Paste))
                    { overrideBgColor = Color.DarkGray });
                }

                content.Add(new RbSeperationLine());
                content.newParagraph();

                content.h2(DssRef.todoLang.GeneralTitle, HudLib.TitleColor_Head2);
            }
            var voiceOpt = new DropDownBuilder("voice");
            {
                SpriteName chatKey = Ref.gamesett.keyboardMap.VoiceChat.Icon;
                for (VoiceOption opt = 0; opt < VoiceOption.NUM; opt++)
                {
                    SpriteName icon;
                    string caption;
                    switch (opt)
                    {
                        default:
                        case VoiceOption.Off:
                            icon = SpriteName.RedErrorCross;
                            caption = DssRef.lang.Hud_Off;
                            break;
                        case VoiceOption.ButtonHold:
                            icon = chatKey;
                            caption = DssRef.todoLang.InputButton_Hold;
                            break;
                        case VoiceOption.ButtonToggle:
                            icon = chatKey;
                            caption = DssRef.todoLang.InputButton_Toggle;
                            break;
                        case VoiceOption.AlwaysOn:
                            icon = SpriteName.MenuPixelIconSoundVol;
                            caption = DssRef.todoLang.VoiceOptAlwaysOn;
                            break;
                    }

                    voiceOpt.AddOption(icon, caption, opt == Ref.netsett.voiceOption, opt == VoiceOption.ButtonHold,
                        new RbAction1Arg<VoiceOption>((VoiceOption vopt) =>
                        {
                            Ref.netsett.voiceOption = vopt;
                            Ref.netsett.settingsHasChanged = true;
                            menu.CloseDropDown();
                        }, opt), null);
                }
            }
            voiceOpt.Build(content, SpriteName.VoiceSoundOn, DssRef.todoLang.VoiceTitle, menu);
            if (bMainMenu)
            {
                var recieveGiftOpt = new DropDownBuilder("gift");
                {
                    for (GiftRecieveOption opt = 0; opt < GiftRecieveOption.NUM; opt++)
                    {
                        string caption;
                        switch (opt)
                        {
                            default:
                            case GiftRecieveOption.Allow:
                                caption = DssRef.todoLang.GiftOptAllow;
                                break;
                            case GiftRecieveOption.FriendsOnly:
                                caption = DssRef.todoLang.GiftOptFriendsOnly;
                                break;
                            case GiftRecieveOption.Blocked:
                                caption = DssRef.todoLang.GiftOptBlocked;
                                break;
                        }

                        recieveGiftOpt.AddOption(caption, opt == Ref.netsett.clientSettings.recieveGifts, opt == GiftRecieveOption.Allow,
                            new RbAction1Arg<GiftRecieveOption>((GiftRecieveOption select) =>
                            {
                                Ref.netsett.clientSettings.recieveGifts = select;
                                Ref.netsett.settingsHasChanged = true;
                            }, opt), null);
                    }
                }
                recieveGiftOpt.Build(content, SpriteName.NO_IMAGE, DssRef.todoLang.ReceiveAchievementsTitle, menu);
                content.space();
                RichBoxContent info = new RichBoxContent();
                HudLib.InfoButton(content, new RbTooltip_Text(DssRef.todoLang.GiftWarning));

                content.newParagraph();
                if (Ref.netsett.unlockPublicGames == false)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.birdUnLock),
                    new RbSpace(),
                    new RbText(string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Hud_Unlock, DssRef.todoLang.Unlock_PublicGames))
                },
                        new RbAction2Arg<string, StackOption>(openmenu, UnlockPublic, StackOption.Stack)));
                    content.newLine();
                }
                if (Ref.netsett.unlockPvp == false)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.birdUnLock),
                    new RbSpace(),
                    new RbText(string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Hud_Unlock, DssRef.todoLang.Unlock_PlayerVersusPlayer))
                },
                    new RbAction2Arg<string, StackOption>(openmenu, UnlockPvp, StackOption.Stack)));
                }

                content.newParagraph();
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.FullReset) }, new RbAction(() =>
                {
                    new NetworkSettings();
                    menu.needRefresh = true;
                })));



                
            }

           

            void setLobbyPublicity(LobbyPublicity publicity)
            {
                Ref.netsett.lobbyPublicity = publicity;
                menu.CloseDropDown();
            }
        }

        void playerInteractSettings(RichBoxContent content, bool host)
        {
            PlayerToPlayerDiplomacyData toPlayerDiplomacy = host ? Ref.netsett.hostPtoP : Ref.netsett.clientPtoP;


            var allowAllianceOptions = new DropDownBuilder("allowAlliance" + host.ToString());
            {
                for (PlayerDiplomacyAllowType allowType = 0; allowType < PlayerDiplomacyAllowType.NUM; allowType++)
                {
                    if (host || allowType != PlayerDiplomacyAllowType.PlayersChoose)
                    {
                        allowAllianceOptions.AddOption(allowTypeString(allowType), allowType == toPlayerDiplomacy.allianceAllow, false,
                            new RbAction1Arg<PlayerDiplomacyAllowType>((PlayerDiplomacyAllowType allowType) =>
                            {
                                if (host)
                                {
                                    Ref.netsett.hostPtoP.allianceAllow = allowType;
                                }
                                else
                                {
                                    Ref.netsett.clientPtoP.allianceAllow = allowType;
                                }
                                Ref.netsett.settingsHasChanged = true;
                                menu.CloseDropDown();
                            }, allowType), null);
                    }
                }
            }
            allowAllianceOptions.Build(content, SpriteName.WarsRelationAlly, DssRef.todoLang.AllowAllianceTitle, menu);

            if (toPlayerDiplomacy.allianceAllow == PlayerDiplomacyAllowType.Allow)
            {
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.CanBreakAlliance) },
                    Ref.netsett.canBreakAllianceProperty)
                { propertyTag = host });
            }

            content.newParagraph();
            var allowWarOptions = new DropDownBuilder("allowWar" + host.ToString());
            {
                for (PlayerDiplomacyAllowType allowType = 0; allowType < PlayerDiplomacyAllowType.NUM; allowType++)
                {
                    if (host || allowType != PlayerDiplomacyAllowType.PlayersChoose)
                    {
                        var opt = allowWarOptions.AddOption(allowTypeString(allowType), allowType == toPlayerDiplomacy.warAllow, false,
                            new RbAction1Arg<PlayerDiplomacyAllowType>((PlayerDiplomacyAllowType allowType) =>
                            {
                                if (host)
                                {
                                    Ref.netsett.hostPtoP.warAllow = allowType;
                                }
                                else
                                {
                                    Ref.netsett.clientPtoP.warAllow = allowType;
                                }
                                Ref.netsett.settingsHasChanged = true;
                                menu.CloseDropDown();
                            }, allowType), null);

                        if (!Ref.netsett.unlockPvp && allowType != PlayerDiplomacyAllowType.Blocked)
                        {
                            opt.enabled = false;
                        }
                    }
                }
            }
            allowWarOptions.Build(content, SpriteName.WarsRelationWar, DssRef.todoLang.AllowWarTitle, menu);

            if (toPlayerDiplomacy.warAllow == PlayerDiplomacyAllowType.Allow)
            {
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.FairProtection) },
                    Ref.netsett.fairProtectionProperty, new RbTooltip_Text(DssRef.todoLang.FairProtectionTooltip))
                { propertyTag = host });

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.MustAsk) },
                    Ref.netsett.warMustAskProperty, new RbTooltip_Text(DssRef.todoLang.MustAskTooltip))
                { propertyTag = host });

                if (!toPlayerDiplomacy.mustAsk)
                {

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.AllianceLimit) },
                        Ref.netsett.warAllianceLimitProperty, new RbTooltip_Text(DssRef.todoLang.AllianceLimitTooltip))
                    { propertyTag = host });

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.GameStartProtection) },
                        Ref.netsett.warUseGameStartTimeProperty)
                    { propertyTag = host });

                    if (toPlayerDiplomacy.gameStartPreparationTime.use)
                    {
                        HudLib.Label(content, DssRef.todoLang.Hud_Time_Minutes);
                        content.hspace();
                        RbDragButton.RbDragButtonGroup(content, new List<float> { 10, 30 }, new DragButtonSettings(5, 120, 5),
                            Ref.netsett.warStartTimeProperty, true, host);
                        content.newParagraph();
                    }
                }

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.WarPreparationTime) },
                    Ref.netsett.warUsePreparationTimeProperty, new RbTooltip_Text(DssRef.todoLang.WarPreparationTimeTooltip))
                { propertyTag = host });

                if (toPlayerDiplomacy.warDeclarePreparationTime.use)
                {
                    HudLib.Label(content, DssRef.todoLang.Hud_Time_Minutes);
                    content.hspace();
                    RbDragButton.RbDragButtonGroup(content, new List<float> { 10, 30 }, new DragButtonSettings(5, 120, 5),
                        Ref.netsett.warPreparationTimeProperty, true, host);
                    content.newParagraph();
                }

            }


        }

        public bool canBreakAllianceProperty2(object tag, bool set, bool value)
        {
            return value;
        }

        string allowTypeString(PlayerDiplomacyAllowType allowType)
        {
            switch (allowType)
            {
                case PlayerDiplomacyAllowType.Allow:
                    return DssRef.todoLang.Hud_Allow;

                default:
                case PlayerDiplomacyAllowType.Blocked:
                    return DssRef.todoLang.Hud_Blocked;

                case PlayerDiplomacyAllowType.PlayersChoose:
                    return DssRef.todoLang.DiplomacyPlayersChoice;
            }
        }

        void unlockMultiplayer_Sure(bool bPublicGames)
        {
            RichBoxContent content = new RichBoxContent();
            content.h1(DssRef.todoLang.UnlockSureTitle, HudLib.TitleColor_Head);
            content.text(DssRef.todoLang.UnlockSureDescription);

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbText(DssRef.todoLang.Hud_Accept)
                        }, new RbAction(() => {
                            if (bPublicGames)
                            {
                                Ref.netsett.unlockPublicGames = true;
                            }
                            else
                            {
                                Ref.netsett.unlockPvp = true;
                                Ref.netsett.hostPtoP.warAllow = PlayerDiplomacyAllowType.PlayersChoose;
                                Ref.netsett.clientPtoP.warAllow = PlayerDiplomacyAllowType.Allow;
                            }

                            Ref.netsett.settingsHasChanged = true;
                            openmenu(MultiplayerSettings, StackOption.ClearStack);
                        }))
            { fillWidth = true });

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                            new RbText(DssRef.lang.Hud_Cancel)
                        }, new RbAction(menu.menuBack, RbSoundType.Back))
            { fillWidth = true });
            menu.Refresh(content);
        }
    }
}