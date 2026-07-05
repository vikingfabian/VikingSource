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
        const string BlockList = "blocklist";

        static readonly RelationType[] DefaultRelationsOptions = { RelationType.RelationType0_Neutral, RelationType.RelationType3_Ally, RelationType.RelationTypeN4_War };

        RichMenu menu;
        public NetworkSettingsMenu(RichMenu menu) 
        { 
            this.menu = menu;
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
                        
                        content.h1(string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Hud_Unlock, "public games"), HudLib.TitleColor_Head);
                        content.h2(SpriteName.cmdWarningTriangle, "Warning!", HudLib.NotAvailableColor);

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText("Do not play with strangers"));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText("The game has zero protection against cheating or trolling"));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText("You will have a bad experience"));

                        content.newParagraph();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbText("Accept")
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
                        
                        content.h1(string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Hud_Unlock, "player versus player"), HudLib.TitleColor_Head);
                        content.h2(SpriteName.cmdWarningTriangle, "Warning!", HudLib.NotAvailableColor);

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText("DSS is not designed for competetive games"));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText("There is no balance, matches will be unfair"));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText("You will have a bad experience"));

                        content.newParagraph();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbText("Accept")
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
                        int count = 0;
                        RichBoxContent content = new RichBoxContent();
                        HudLib.returnButton(content, menu, true, null);

                        content.h1("Blocked players", HudLib.TitleColor_Head);

                        for (int i  = 0; i  < Ref.netsett.storedGamers.Count; i ++)
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
                                    }, i), new RbTooltip_Text("Click: remove ban")));
                            }
                        }
                        if (count == 0)
                        {
                            content.text(DssRef.lang.Hud_EmptyList, HudLib.InfoYellow_Light);
                        }
                        menu.Refresh(content);
                    }
                    break;
            }
        }
        public void openmenu(string menuName, StackOption stack)
        {
            
            menu.OpenMenu(menuName, stack);
        }
        void multiplayerSettingsMenu()
        {
            RichBoxContent content = new RichBoxContent();
            if (menu.menuStack.Count > 1)
            {
                HudLib.returnButton(content, menu, true, null);
                content.newLine();
            }

            content.h1(DssRef.todoLang.Lobby_Category_MultiplayerSettings, HudLib.TitleColor_Head);

            content.h2("Host settings", HudLib.TitleColor_Head2);

            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.todoLang.Network_PlayOffline) },
                Ref.netsett.OfflineProperty));

            if (Ref.netsett.hostNetwork)
            {
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
                HudLib.Label(content, SpriteName.birdPlayerCount, "Max player count");
                content.hspace();
                RbDragButton.RbDragButtonGroup(content, new List<float> { 10 },
                    new DragButtonSettings(2, 64, 1), Ref.netsett.MaxPlayerCountProperty, false);

                content.newLine();
                HudLib.Label(content, SpriteName.WarsHudIconDistanceOnMap, "Distance between players");
                content.hspace();
                content.Add(new RbDragButton(new DragButtonSettings(0, 8, 1), Ref.netsett.PlayerSpacingProperty));


                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconHandicap),
                    new RbSpace(0.5f),
                    new RbText("Allow handicap") },
                    Ref.netsett.allowHandicapProperty));

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { 
                    new RbImage(SpriteName.WarsHudCasualMode),  
                    new RbSpace(0.5f),
                    new RbText("Allow casual controls") },
                    Ref.netsett.allowCasualControlsProperty, new RbTooltip_Text(DssRef.lang.Settings_CasualControls_Description)));

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.VoxelEditorBucket),
                    new RbSpace(0.5f),
                    new RbText("Auto recolor player flags") },
                    Ref.netsett.autoRecolorFlagsProperty, null));

                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbText("Blocked players") },
                    new RbAction2Arg<string, StackOption>(openmenu, BlockList, StackOption.Stack)));
                
                content.newParagraph();

                content.h2("Player interaction", HudLib.TitleColor_Label);

                var defDiplomacyOptions = new DropDownBuilder("def diplomacy");
                {
                    foreach (var relation in DefaultRelationsOptions)
                    {
                        IconName.Relation(relation, out var dipIcon, out var dipName);
                        defDiplomacyOptions.AddOption(dipIcon, dipName, relation == Ref.netsett.hostSettings.startDiplomacy,
                            relation == RelationType.RelationType0_Neutral,
                            new RbAction1Arg<RelationType>((RelationType rel) => {
                                Ref.netsett.hostSettings.startDiplomacy = relation;
                                Ref.netsett.settingsHasChanged = true;
                            }, relation), null);
                    }
                }
                defDiplomacyOptions.Build(content, SpriteName.WarsDiplomaticPoint, "Default diplomacy", menu);

                playerInteractSettings(content, true);

                

                //"Join Permissions"}
                content.Add(new RbSeperationLine());

                content.newParagraph();
                content.h2("Client settings", HudLib.TitleColor_Head2);

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconHandicap),
                    new RbSpace(0.5f),
                    new RbText("Use handicap") },
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
                    aggroOptions.Build(content, SpriteName.NO_IMAGE, "Bot aggression", menu);

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.WarsUnitIcon_Honorguard),
                        new RbSpace(0.5f),
                        new RbText("Extra honor guard") },
                        Ref.netsett.handicap_extraHonorGuardsProperty));

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { 
                        new RbImage(SpriteName.WarsIcon_Resources),
                        new RbSpace(0.5f),
                        new RbText("Resource boost") },
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
                    taxOptions.Build(content, SpriteName.rtsIncome, "Tax income", menu);
                }

                content.h2("Player interaction", HudLib.TitleColor_Label);
                playerInteractSettings(content, false);
                content.newParagraph();
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Default: Peaceful") }, new RbAction(Ref.netsett.resetPeaceful, RbSoundType.Paste))
                { overrideBgColor = Color.DarkGray });
                content.newLine();
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Default: Co-optional") }, new RbAction(Ref.netsett.resetMixed, RbSoundType.Paste))
                { overrideBgColor = Color.DarkGray });

                if (Ref.netsett.unlockPvp)
                {   
                    content.newLine();
                    content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Default: Hardcore") }, new RbAction(Ref.netsett.resetHardcore, RbSoundType.Paste))
                    { overrideBgColor = Color.DarkGray });
                }

                content.Add(new RbSeperationLine());
                content.newParagraph();

                content.h2("General", HudLib.TitleColor_Head2);

                var voiceOpt  = new DropDownBuilder("voice");
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
                                caption = "Off";
                                break;
                            case VoiceOption.ButtonHold:
                                icon = chatKey;
                                caption = "Button hold";
                                break;
                            case VoiceOption.ButtonToggle:
                                icon = chatKey;
                                caption = "Button toggle";
                                break;
                            case VoiceOption.AlwaysOn:
                                icon = SpriteName.MenuPixelIconSoundVol;
                                caption = "Always on";
                                break;
                        }

                        voiceOpt.AddOption(icon, caption, opt == Ref.netsett.voiceOption, opt == VoiceOption.ButtonHold,
                            new RbAction1Arg<VoiceOption>((VoiceOption vopt) =>
                            {
                                Ref.netsett.voiceOption = vopt;
                                Ref.netsett.settingsHasChanged = true;
                            }, opt), null);
                    }
                }
                voiceOpt.Build(content, SpriteName.VoiceSoundOn, "Voice", menu);

                var recieveGiftOpt = new DropDownBuilder("gift");
                {
                    for (GiftRecieveOption opt = 0; opt < GiftRecieveOption.NUM; opt++)
                    {
                       
                        string caption;
                        switch (opt)
                        {
                            default:
                            case GiftRecieveOption.Allow:
                                caption = "Allow";
                                break;
                            case GiftRecieveOption.FriendsOnly:
                                caption = "Friends only";
                                break;
                            case GiftRecieveOption.Blocked:
                                caption = "Blocked";
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
                recieveGiftOpt.Build(content, SpriteName.NO_IMAGE, "Recieve achievements", menu);
                content.space();
                RichBoxContent info = new RichBoxContent();
                HudLib.InfoButton(content, new RbTooltip_Text("Warning! Gifted achievements can feel demeaning"));

                //x Unlock public games
                //- Do not play with strangers
                //- The game has zero protection against cheating or trolling
                //- You will have a bad experience

                //x Unlock PvP
                //- DSS is not designed for competetive games
                //- There is no balance, matches will be unfair
                //- You will have a bad experience

                //-Are you really, really sure?
                //Will you be a big boy and not cry on the forum later?
                content.newParagraph();
                if (Ref.netsett.unlockPublicGames == false)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.birdUnLock),
                    new RbSpace(),
                    new RbText(string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Hud_Unlock, "public games"))
                },
                        new RbAction2Arg<string, StackOption>(openmenu, UnlockPublic, StackOption.Stack)));
                    content.newLine();
                }
                if (Ref.netsett.unlockPvp == false)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.birdUnLock),
                    new RbSpace(),
                    new RbText(string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Hud_Unlock, "player versus player"))
                },
                    new RbAction2Arg<string, StackOption>(openmenu, UnlockPvp, StackOption.Stack)));
                }

                content.newParagraph();
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Full reset") }, new RbAction(() =>
                {
                    new NetworkSettings();
                    menu.needRefresh = true;
                })));

               

                void setLobbyPublicity(LobbyPublicity publicity)
                {
                    Ref.netsett.lobbyPublicity = publicity;
                }
            }

            menu.Refresh(content);
        }

        void playerInteractSettings(RichBoxContent content, bool host)
        {
            PlayerToPlayerDiplomacyData toPlayerDiplomacy = host? Ref.netsett.hostPtoP : Ref.netsett.clientPtoP;

            
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
                                menu.CloseDropDown();
                            }, allowType), null);
                    }
                }
            }
            allowAllianceOptions.Build(content, SpriteName.WarsRelationAlly, "Allow alliance", menu);

            if (toPlayerDiplomacy.allianceAllow == PlayerDiplomacyAllowType.Allow)
            {
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Can break alliance") },
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
                                menu.CloseDropDown();
                            }, allowType), null);

                        if (!Ref.netsett.unlockPvp && allowType != PlayerDiplomacyAllowType.Blocked)
                        {
                            opt.enabled = false;
                        }
                    }
                }
            }
            allowWarOptions.Build(content, SpriteName.WarsRelationWar, "Allow war", menu);

            if (toPlayerDiplomacy.warAllow == PlayerDiplomacyAllowType.Allow)
            {
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Fair protection") },
                    Ref.netsett.fairProtectionProperty, new RbTooltip_Text("Protected players must use their rules on you"))
                { propertyTag = host });

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Must ask") },
                    Ref.netsett.warMustAskProperty, new RbTooltip_Text("Both players must agree to fight"))
                { propertyTag = host });
                //if (!host)
                //{
                if (!toPlayerDiplomacy.mustAsk)
                {
                    
                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Alliance limt") },
                        Ref.netsett.warAllianceLimitProperty, new RbTooltip_Text("Can't be attacked by a larger player alliance"))
                    { propertyTag = host });

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Game start protection") },
                        Ref.netsett.warUseGameStartTimeProperty)
                    { propertyTag = host });

                    if (toPlayerDiplomacy.gameStartPreparationTime.use)
                    {
                        HudLib.Label(content, "Minutes");
                        content.hspace();
                        RbDragButton.RbDragButtonGroup(content, new List<float> { 10, 30 }, new DragButtonSettings(5, 120, 5),
                            Ref.netsett.warStartTimeProperty, true, host);
                        content.newParagraph();
                    }
                }

                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(".War preparation time") },
                    Ref.netsett.warUsePreparationTimeProperty, new RbTooltip_Text("A delay from war declaration until attacks are available"))
                { propertyTag = host });

                if (toPlayerDiplomacy.warDeclarePreparationTime.use)
                {
                    HudLib.Label(content, "Minutes");
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
                    return "Allow";

                default:
                case PlayerDiplomacyAllowType.Blocked:
                    return "Blocked";

                case PlayerDiplomacyAllowType.PlayersChoose:
                    return "Players choice";
            }
        }

        void unlockMultiplayer_Sure(bool bPublicGames)
        {
            RichBoxContent content = new RichBoxContent();
            content.h1("Are you really, really sure?", HudLib.TitleColor_Head);
            content.text("Will you be a big boy and not cry on the forum later?");

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbText("Accept")
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
