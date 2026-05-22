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

        static readonly RelationType[] DefaultRelationsOptions = { RelationType.RelationType0_Neutral, RelationType.RelationType3_Ally, RelationType.RelationTypeN3_War };

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
                HudLib.Label(content, "Max player count");
                content.hspace();
                RbDragButton.RbDragButtonGroup(content, new List<float> { 10 },
                    new DragButtonSettings(2, 64, 1), Ref.netsett.MaxPlayerCountProperty, false);




                //PlayerToPlayerDiplomacy toPlayerDiplomacy = Ref.netsett.hostDiplomacy;
                //content.h2("Player interaction", HudLib.TitleColor_Label);
                //var allowAllianceOptions = new DropDownBuilder("allowAlliance");
                //{
                //    for (PlayerDiplomacyAllowType allowType = 0; allowType < PlayerDiplomacyAllowType.NUM; allowType++)
                //    {
                //        allowAllianceOptions.AddOption(allowTypeString(allowType), allowType == toPlayerDiplomacy, false,
                //            new RbAction1Arg<>)
                //    }
                //}
                //allowAllianceOptions.Build(content, SpriteName.WarsRelationAlly, "Allow: aliance", menu);
                content.newParagraph();

                content.h2("Player interaction", HudLib.TitleColor_Label);

                var defDiplomacyOptions = new DropDownBuilder("def diplomacy");
                {
                    foreach (var relation in DefaultRelationsOptions)
                    {
                        IconName.Relation(relation, out var dipIcon, out var dipName);
                        defDiplomacyOptions.AddOption(dipIcon, dipName, relation == Ref.netsett.startDiplomacy,
                            relation == RelationType.RelationType0_Neutral,
                            new RbAction1Arg<RelationType>((RelationType rel) => {
                                Ref.netsett.startDiplomacy = relation;
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

                content.h2("Player interaction", HudLib.TitleColor_Label);
                playerInteractSettings(content, false);
                content.Add(new RbSeperationLine());
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
                menu.Refresh(content);

                void setLobbyPublicity(LobbyPublicity publicity)
                {
                    Ref.netsett.lobbyPublicity = publicity;
                }
            }
        }

        void playerInteractSettings(RichBoxContent content, bool host)
        {
            PlayerToPlayerDiplomacy toPlayerDiplomacy = host? Ref.netsett.hostDiplomacy : Ref.netsett.clientDiplomacy;

            
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
                                    Ref.netsett.hostDiplomacy.allianceAllow = allowType;
                                }
                                else
                                {
                                    Ref.netsett.clientDiplomacy.allianceAllow = allowType;
                                }
                            }, allowType), null);
                    }
                }
            }
            allowAllianceOptions.Build(content, SpriteName.WarsRelationAlly, "Allow alliance", menu);
            content.newLine();
            //content.Add(new ArtCheckbox(content, Ref.netsett.canBreakAllianceProperty) { propertyTag = host });

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
                                    Ref.netsett.hostDiplomacy.warAllow = allowType;
                                }
                                else
                                {
                                    Ref.netsett.clientDiplomacy.warAllow = allowType;
                                }
                            }, allowType), null);

                        if (!Ref.netsett.unlockPvp && allowType != PlayerDiplomacyAllowType.Blocked)
                        {
                            opt.enabled = false;
                        }
                    }
                }
            }
            allowWarOptions.Build(content, SpriteName.WarsRelationWar, "Allow war", menu);
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
                                Ref.netsett.hostDiplomacy.warAllow = PlayerDiplomacyAllowType.PlayersChoose;
                                Ref.netsett.clientDiplomacy.warAllow = PlayerDiplomacyAllowType.Allow;
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
