using System;
using System.Collections.Generic;
using System.Linq;

using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;


namespace VikingEngine.DSSWars.Interface
{   

    class DiplomacyDisplay
    {
        static readonly RelationType[] RelationOptionsAsGod = { RelationType.RelationTypeN4_War, RelationType.RelationType1_Peace, RelationType.RelationType3_Ally };

        Players.LocalPlayer player;
        DiplomaticRelation selectedRelation;
        Faction otherfaction;
        bool againstDark;

        public DiplomacyDisplay(Players.LocalPlayer player)
        { 
            this.player = player;
        }

        public void quickSelect()
        {
            DiplomacyActionManager diplomacyActionManager = new DiplomacyActionManager();
            var options = diplomacyActionManager.diplomacyOptionsToBot(player, otherfaction);
            if (options.Count > 0)
            {
                commitDiplomacyAction(options.First().toRelation);
            }
        }


        public void toHud(RichBoxContent content, Faction botFaction, bool viewFactionInfo)
        {
            otherfaction = botFaction;
            
            if (player.faction == botFaction || botFaction == null || botFaction.player == player)
            {
                return;
            }

            selectedRelation = DssRef.world.diplomacy.GetRelation(player.faction, botFaction);
            againstDark = botFaction.WantToAllyAgainstDark() && player.faction.diplomaticSide == DiplomaticSide.Light;
           

            FactionRelationDisplay(botFaction, selectedRelation.Relation, content, viewFactionInfo);

            content.newLine();

            if (DssRef.difficulty.setting_gameMode != Data.GameModeMainType.Spectator)
            {
                if (otherfaction.player.IsBot())
                {
                    playerToAi();
                }
                else if (otherfaction.player.IsHumanPlayer())
                {
                    playerToPlayer(content);
                }
                else
                {
                    content.text(TextLib.Error);
                }
            }
            //}
            
            if (player.gameControls.diplomacy != null &&
                player.gameControls.diplomacy.previousFactionsLookedAt.Count > 1)
            {
                content.newParagraph();
                content.h2(DssRef.lang.Diplomacy_RelationWithOthers, HudLib.TitleColor_Label);

                for (int i = 1; i < player.gameControls.diplomacy.previousFactionsLookedAt.Count; i++)
                {
                    content.newLine();
                    var thirdPartFaction = player.gameControls.diplomacy.previousFactionsLookedAt[i];
                    var relation = DssRef.world.diplomacy.GetRelation(otherfaction, thirdPartFaction).Relation;

                    content.Add(thirdPartFaction.FlagTextureToHud());
                    content.hspace();
                    content.Add(new RbText(thirdPartFaction.PlayerName));

                    IconName.Relation(relation, out SpriteName relIcon, out string relName);
                    content.Add(new RbText(": "));
                    content.Add(new RbImage(relIcon));
                    content.hspace();
                    content.Add(new RbText(relName));

                    if (DssRef.difficulty.setting_gameMode == Data.GameModeMainType.Spectator)
                    {
                        content.space();
                        foreach (var forceRelation in RelationOptionsAsGod)
                        {
                            IconName.Relation(relation, out SpriteName frelIcon, out string frelName);
                            content.Add(new ArtButton( RbButtonStyle.GodPower,
                                new List<AbsRichBoxMember> { new RbImage(frelIcon) },
                                new RbAction3Arg<RelationType, Faction, Faction>(setRelation_AsGod, forceRelation, otherfaction, thirdPartFaction),
                                new RbTooltip_Text(frelName), true));
                    } }
                }

            }

            void CostDisplay(RichBoxContent content, int cost)
            {
                
                content.Add(new RbImage(SpriteName.WarsDiplomaticSub));
                content.hspace();
                content.Add(new RbText(cost.ToString()));
                content.space(2);
            }

            void playerToAi()
            {
                DiplomacyActionManager diplomacyActionManager = new DiplomacyActionManager();
                var options = diplomacyActionManager.diplomacyOptionsToBot(player, botFaction);

                if (selectedRelation.Relation == RelationType.RelationTypeN2_Truce ||
                    selectedRelation.Relation == RelationType.RelationTypeN3_Mobilization)
                {
                    int sec = Convert.ToInt32(selectedRelation.RelationEnd_GameTimeSec.Seconds);
                    content.text(string.Format(DssRef.lang.Diplomacy_TruceTimeLength, sec));
                }

                HudLib.LabelAndText(content,SpriteName.NO_IMAGE, DssRef.lang.Diplomacy_SpeakTermIs, Diplomacy.SpeakTermsString(selectedRelation.SpeakTerms));
                //content.text(string.Format(DssRef.lang.Diplomacy_SpeakTermIs, Diplomacy.SpeakTermsString(selectedRelation.SpeakTerms)));
               
                for (int i = 0; i < options.Count; ++i)
                {
                    DiplomacyOption opt = options[i];

                    content.newLine();
                    if (i == 0)
                    {
                        player.gameControls.input.QuickSelect.ToRichContent(content);
                    }

                    content.Add(new RbTab(0.075f));
                    CostDisplay(content, opt.cost);
                    forgeRelationButton(content, opt);
                }

#if DEBUG
                if (StartupSettings.EndlessDiplomacy)
                {
                    content.newParagraph();
                    content.Add(new RbButton(new List<AbsRichBoxMember>()
                        {
                            new RbText("*Merge"),
                        },
                      new RbAction(makeServant)));
                }
#endif
               
                if (againstDark)
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.Diplomacy_LightSide));
                }
            }
        }

        private void forgeRelationButton(RichBoxContent content, DiplomacyOption opt)
        {
            string forgeRelationString;

            IconName.Relation(opt.toRelation, out SpriteName relIcon, out string relName);
            switch (opt.toRelation)
            {
                case RelationType.RelationTypeN3_Mobilization:
                case RelationType.RelationTypeN4_War:
                    forgeRelationString = DssRef.lang.Hud_WardeclarationTitle;
                    break;
                case RelationType.RelationType0_Neutral:
                    forgeRelationString = DssRef.lang.Diplomacy_EndRelations;
                    break;
                default:
                    forgeRelationString = string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, relName);
                    break;
            }

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>()
                        {
                            new RbImage( relIcon),
                            new RbSpace(),
                            new RbText(forgeRelationString),
                        },
               new RbAction1Arg<RelationType>(commitDiplomacyAction, opt.toRelation, RbSoundType.Buy),
               new RbTooltip(forgeRelationToolTip, opt), opt.available));
        }

        void forgeRelationToolTip(RichBoxContent content, object tag)
        {
            DiplomacyOption opt = (DiplomacyOption)tag;

            if (opt.toRelation <= RelationType.RelationTypeN3_Mobilization &&
                !opt.available)
            {
                if (opt.tooLargeAlliance)
                {
                    content.icontext(HudLib.NotAvailableIcon, ".Alliance limt", HudLib.NotAvailableColor);
                    content.text(".Can't be attacked by a larger player alliance", HudLib.InfoYellow_Light);
                }
                if (opt.startProtection)
                {
                    content.icontext(HudLib.NotAvailableIcon, ".Game start protection", HudLib.NotAvailableColor);
                    content.text(HudLib.TimeSpan_LongText(opt.protectionTime), HudLib.InfoYellow_Light);
                }
                content.newParagraph();
            }

            relationTooltip(content, opt.toRelation);
        }
        void setRelation_AsGod(RelationType relation, Faction faction1, Faction faction2)
        {
            DssRef.world.diplomacy.SetRelationType(faction1, faction2, relation);
        }

        public static void FactionRelationDisplay(Faction faction, RelationType relation, RichBoxContent content, bool viewFactionInfo)
        {
            if (viewFactionInfo)
            {
                if (faction.player != null)
                {
                    content.Add(new RbBeginTitle(1));
                    content.Add(faction.FlagTextureToHud());
                    content.space(0.5f);
                    content.Add(new RbImage(SpriteName.WarsGovernmentIcon));
                    content.space(0.5f);
                    content.Add(new RbText(faction.PlayerName, HudLib.TitleColor_Name));

                    content.space(1);
                    content.Add(new RbText(string.Format(DssRef.lang.UnitId, faction.myIndex), HudLib.SecondaryTextColor));

                    content.Add(new RbSeperationLine());

                    content.newLine();
                }

                content.Add(new RbImage(SpriteName.rtsMoney));
                content.space();
                content.Add(new RbText(TextLib.LargeNumber(faction.money.GetGold())));

                content.space(2);


                content.Add(new RbImage(SpriteName.WarsWorker));
                content.space();
                content.Add(new RbText(TextLib.LargeNumber(faction.totalWorkForce)));

                content.space(2);

                content.Add(new RbImage(SpriteName.WarsStrengthIcon));
                content.space();
                content.Add(new RbText(TextLib.LargeNumber(Convert.ToInt32(faction.militaryStrength))));

                content.newParagraph();
            }
            if (DssRef.difficulty.setting_gameMode != Data.GameModeMainType.Spectator)
            {
                IconName.Relation(relation, out SpriteName relIcon, out string relName);
                var relType = new RbText(DssRef.lang.Diplomacy_RelationType + ": ", HudLib.TitleColor_Label);
                content.Add(relType);
                content.Add(new RbImage(relIcon));
                content.hspace();
                content.Add(new RbText(relName));
            }
        }


        void playerToPlayer(RichBoxContent content)
        {
            var otherPlayer = otherfaction.player.GetHumanPlayer();
            var settings = otherPlayer.NetClientSettings();

            var PtoP = player.GetOrCreateToPlayerDiplomacy(otherPlayer);
            if (PtoP == null)
            {
                return;
            }

            PtoP.refresh(selectedRelation.Relation);

            if (PtoP.suggestingNewRelation)
            {
                IconName.Relation(PtoP.suggestedRelation, out SpriteName relIcon, out string relName);
                content.Add(new RbImage(relIcon));
                content.hspace();
                content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_NewRelationOffered, relName)));
                content.newLine();

                if (PtoP.suggestedBy == player.faction.myIndex)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary,new List<AbsRichBoxMember>()
                        {
                            new RbText(Ref.langOpt.Hud_Cancel),
                        },
                        new RbAction1Arg<AbsHumanPlayer>(cancelToPlayerRelation, otherPlayer, RbSoundType.Stop)));
                }
                else
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary,new List<AbsRichBoxMember>()
                        {
                            new RbText(DssRef.lang.Diplomacy_AcceptRelationOffer),
                        },
                       new RbAction1Arg<AbsHumanPlayer>(acceptToPlayerRelation, otherPlayer, RbSoundType.Buy)));
                }
            }
            else
            {
               

                if (selectedRelation.Relation <= RelationType.RelationTypeN2_Truce)
                {
                    content.newLine();
                    offerToPlayerRelationButton(content, RelationType.RelationType1_Peace);
                    

                    //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>()
                    //    {
                    //        new RbImage(SpriteName.WarsRelationPeace),
                    //        new RbText(DssRef.lang.Diplomacy_OfferPeace),
                    //    },
                    //    new RbAction1Arg<RelationType>(offerToPlayerRelation, RelationType.RelationType1_Peace, RbSoundType.Buy)));
                }
                else if (selectedRelation.Relation < RelationType.RelationType3_Ally)
                {
                    if (settings.clientPtoP.allianceAllow == Network.PlayerDiplomacyAllowType.Allow)
                    {
                        content.newLine();
                        offerToPlayerRelationButton(content, RelationType.RelationType3_Ally);
                        

                        //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>()
                        //{
                        //    new RbImage(SpriteName.WarsRelationAlly),
                        //    new RbText(DssRef.lang.Diplomacy_OfferAlliance),
                        //},
                        //new RbAction1Arg<RelationType>(offerToPlayerRelation, RelationType.RelationType3_Ally, RbSoundType.Buy)));
                    }
                }

               
                if (selectedRelation.Relation == RelationType.RelationType3_Ally &&
                   settings.clientPtoP.canBreakAlliance)
                {
                    content.newLine();
                    forgeRelationButton(content, new DiplomacyOption(RelationType.RelationType0_Neutral));
                }

                if (selectedRelation.Relation >= RelationType.RelationTypeN1_Enemies &&
                    (selectedRelation.Relation != RelationType.RelationType3_Ally || settings.clientPtoP.canBreakAlliance))
                {

                    RelationType warLevel = settings.clientPtoP.warDeclarePreparationTime.use ?
                        RelationType.RelationTypeN3_Mobilization : RelationType.RelationTypeN4_War;
                    var warOption = new DiplomacyOption(warLevel);

                    warOption.tooLargeAlliance = false;

                    if (settings.clientPtoP.allianceLimit)
                    {
                        warOption.tooLargeAlliance = player.AllianceCount_Humans() > otherPlayer.AllianceCount_Humans();
                    }



                    warOption.startProtection = false;
                    warOption.protectionTime = TimeSpan.Zero;
                    if (settings.clientPtoP.gameStartPreparationTime.use)
                    {
                        warOption.protectionTime = settings.clientPtoP.gameStartPreparationTime.time.TimeSpan - otherPlayer.timePlayed;
                        warOption.startProtection = warOption.protectionTime.Ticks > 0;
                    }


                    warOption.available = !warOption.tooLargeAlliance && !warOption.startProtection;

                    if (settings.clientPtoP.mustAsk)
                    {
                        content.newLine();
                        offerToPlayerRelationButton(content, warLevel);
                    }
                    else
                    {
                        content.newLine();
                        forgeRelationButton(content, warOption);
                    }
                }
            }
        }

        void offerToPlayerRelationButton(RichBoxContent content, RelationType relation)
        {
            IconName.Relation(relation, out SpriteName icon, out string name);

            content.newLine();

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>()
                {
                    new RbImage(icon),
                    new RbSpace(0.5f),
                    new RbText(string.Format(DssRef.lang.Language_ItemCount_Colon, ".Offer relation", name)),
                },
                new RbAction1Arg<RelationType>(offerToPlayerRelation, relation, RbSoundType.Buy), new RbTooltip(offerRelationTooltip, relation )));
        }

        void offerToPlayerRelation(RelationType relation)
        {
            AbsHumanPlayer otherPlayer = otherfaction.player.GetHumanPlayer();
            PlayerToPlayerDiplomacy PtoP = player.GetOrCreateToPlayerDiplomacy(otherPlayer);

            PtoP.suggestingNewRelation = true;
            PtoP.suggestedRelation = relation;
            PtoP.suggestedBy = player.faction.myIndex;

            if (otherPlayer.IsLocal)
            {
                playerOfferedRelationMessage(player, otherPlayer.GetLocalPlayer(), PtoP);
            }
            else
            {
                netSendP2p(PtoP, otherPlayer);
            }
        }

        void netSendP2p(PlayerToPlayerDiplomacy PtoP, AbsHumanPlayer otherPlayer)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.DssPlayerToPlayerRelation, Network.PacketReliability.Reliable,
                     Network.SendPacketTo.OneSpecific, otherPlayer.networkPeer.peer.FullId, player.playerData.localPlayerIndex);
            
            PtoP.writeNet(w);
        }

        public void netReadP2pRelation(System.IO.BinaryReader r, AbsHumanPlayer fromPlayer)
        {
            otherfaction = fromPlayer.faction;
            PlayerToPlayerDiplomacy PtoP = player.GetOrCreateToPlayerDiplomacy(fromPlayer);
            if (PtoP == null)
            {
                return;
            }
            PtoP.readNet(r, fromPlayer);

            if (PtoP.suggestingNewRelation)
            {
                playerOfferedRelationMessage(fromPlayer, player, PtoP);
            }
            else
            {
                //Cancelled offer
                //playerDeclinedRelationMessage(fromPlayer, player, PtoP);    
            }
        }

        void playerOfferedRelationMessage(AbsHumanPlayer sending, LocalPlayer recieving, PlayerToPlayerDiplomacy PtoP)
        {
            IconName.Relation(PtoP.suggestedRelation, out SpriteName relIcon, out string relName);

            var message = new RichBoxContent();
            message.h1(string.Format(DssRef.lang.Diplomacy_PlayerOfferAlliance, sending.Name));
            message.newLine();
            message.Add(new RbImage(relIcon));
            message.hspace();
            message.Add(new RbText(relName));
            message.newLine();

            var acceptButtonContent = new List<AbsRichBoxMember>(7);
            MessageGroup_Ingame.ControllerInputIcons(player, acceptButtonContent);
            acceptButtonContent.Add(new RbText(DssRef.lang.Diplomacy_AcceptRelationOffer));
            message.Add(new ArtButton(RbButtonStyle.Primary,
                acceptButtonContent,
                new RbAction1Arg<AbsHumanPlayer>(acceptToPlayerRelation, sending)));

            recieving.hud.messages.Add(message, SoundLib.netMessage);            
        }

        void playerDeclinedRelationMessage(AbsHumanPlayer sending, LocalPlayer recieving, PlayerToPlayerDiplomacy PtoP)
        {
            var message = new RichBoxContent();
            sending.addNetGamerToHud(message, true, false);
            message.text(".Declined relation offer");
            recieving.hud.messages.Add(message, SoundLib.stop);
        }

        void acceptToPlayerRelation(AbsHumanPlayer otherPlayer)
        {            
            var PtoP = player.GetOrCreateToPlayerDiplomacy(otherPlayer);

            if (PtoP.suggestingNewRelation)
            {
                var localSettings = player.NetClientSettings();
                if (otherPlayer.NetClientSettings().clientPtoP.fairProtection)
                {
                    localSettings.clientPtoP.ApplyFairProtection(otherPlayer.NetClientSettings().clientPtoP);
                }

                float? secondsLength = null;
                if (PtoP.suggestedRelation == RelationType.RelationTypeN3_Mobilization)
                {
                    secondsLength = localSettings.clientPtoP.gameStartPreparationTime.time.seconds;
                }
                
                DssRef.world.diplomacy.SetRelationType(player.faction, otherPlayer.faction, PtoP.suggestedRelation, secondsLength);

            }

            PtoP.suggestingNewRelation = false;
        }

        void cancelToPlayerRelation(AbsHumanPlayer otherPlayer)
        {
            var PtoP = player.GetOrCreateToPlayerDiplomacy(otherPlayer);

            PtoP.suggestingNewRelation = false;

            if (otherPlayer.IsRemotePlayer())
            { 
                netSendP2p(PtoP, otherPlayer);
            }
        }

        void extendTruceAction()
        {
            int cost = Diplomacy.ExtendTruceCost();
            if (player.diplomaticPoints.pay(cost, false))
            {
                ref var relation = ref DssRef.world.diplomacy.GetRefRelation_Safe(player.faction.myIndex, otherfaction.myIndex);
                relation.RelationEnd_GameTimeSec.addTime(DssConst.TruceTimeSec);
                player.hud.needRefresh = true;
            }
        }
        void extendTruceTooltip(RichBoxContent content, object tag)
        {
            int cost = Diplomacy.ExtendTruceCost();

            diplomacyCostToHud(cost, content);
            content.text(string.Format(DssRef.lang.Diplomacy_TruceExtendTimeLength, DssConst.TruceTimeSec));
        }

        bool canExtendTruce(out int cost)
        {
            cost = Diplomacy.ExtendTruceCost();
            return player.diplomaticPoints.Int() >= cost;
        }

        void commitDiplomacyAction(RelationType toRelation)
        {
            switch (toRelation)
            {
                case RelationType.RelationTypeN2_Truce:
                    peaceAction(false);
                    break;
                case RelationType.ExtendTruce:
                    extendTruceAction();
                    break;
                case RelationType.RelationType1_Peace:
                    peaceAction(true);
                    break;
                case RelationType.RelationType2_Good:
                    allianceAction(false);
                    break;
                case RelationType.RelationType3_Ally:
                    allianceAction(true);
                    break;
                case RelationType.RelationType4_Servant:
                    servantAction();
                    break;
                case RelationType.RelationType0_Neutral:
                    DssRef.world.diplomacy.endRelations(player.faction, otherfaction);
                    break;
                case RelationType.RelationTypeN3_Mobilization:
                    DssRef.world.diplomacy.declareWar(player.faction, otherfaction);
                    break;
                case RelationType.RelationTypeN4_War:
                    DssRef.world.diplomacy.declareWar(player.faction, otherfaction);
                    break;
            }
        }



        void peaceAction(bool peace_notTruce)
        {
            int cost = Diplomacy.EndWarCost(otherfaction,selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, peace_notTruce);

            if (player.diplomaticPoints.pay(cost, false))
            {
                if (peace_notTruce)
                {
                    DssRef.world.diplomacy.SetRelationType(player.faction, otherfaction, RelationType.RelationType1_Peace, DssConst.PeaceSafeTimeSec.GetRandom());

                    //selectedRelation.RelationEnd_GameTimeSec.setTimeFromNow(DssConst.PeaceSafeTimeSec.GetRandom());
                }
                else
                {
                    bool success = true;
                    if (DssRef.difficulty.UseTruceFailure(out float fail))
                    {
                        if (Ref.rnd.Chance(fail))
                        {
                            success = false;
                        }
                    }

                    if (success)
                    {
                        DssRef.world.diplomacy.SetRelationType(player.faction, otherfaction, RelationType.RelationTypeN2_Truce, DssConst.TruceTimeSec);

                    }
                    else
                    {
                        ref var relation = ref DssRef.world.diplomacy.GetRefRelation(player.faction.myIndex, otherfaction.myIndex);
                        relation.SpeakTerms--;
                        if (relation.SpeakTerms < SpeakTerms.SpeakTermsN2_None)
                        {
                            relation.SpeakTerms = SpeakTerms.SpeakTermsN2_None;
                        }

                        SoundLib.wrong.Play();
                    }
                }
            }
        }

        void offerRelationTooltip(RichBoxContent content, object tag)
        {
            content.h1(".If the other player accepts:", HudLib.TitleColor_Label2);
            relationTooltip(content, tag);
        }

        void relationTooltip( RichBoxContent content, object tag)
        {
            RelationType relationType = (RelationType)tag;
            switch (relationType)
            {
                case RelationType.RelationTypeN2_Truce:
                    peaceTooltip(content, false);
                    break;
                case RelationType.ExtendTruce:
                    extendTruceTooltip(content, null);
                    break;
                case RelationType.RelationType1_Peace:
                    peaceTooltip(content, true);
                    break;
                case RelationType.RelationType2_Good:
                    allianceTooltip(content, false);
                    break;
                case RelationType.RelationType3_Ally:
                    allianceTooltip(content, true);
                    break;
                case RelationType.RelationType4_Servant:
                    servantTooltip(content, null);
                    break;

                case RelationType.RelationType0_Neutral:
                    endRelationsTooltip(content, null);
                    break;
                case RelationType.RelationTypeN3_Mobilization:
                case RelationType.RelationTypeN4_War:
                    declareWarTooltip(content, relationType);
                    break;
            }
        }

        void endRelationsTooltip(RichBoxContent content, object tag)
        {
            int cost = 0;

            if (otherfaction.player.IsBot())
            {
              cost = Diplomacy.EndRelationCost(selectedRelation.Relation);
                diplomacyCostToHud(cost, content, true);
            }
                        
            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain, HudLib.TitleColor_Label);
            content.newLine();

            IconName.Relation(RelationType.RelationType0_Neutral, out SpriteName relIcon, out string relName);
            content.Add(new RbImage(relIcon));
            content.hspace();
            content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, relName)));
        }

        void declareWarTooltip(RichBoxContent content, object tag)
        {
            RelationType rel = (RelationType)tag;
            int cost = 0;

            if (otherfaction.player.IsBot())
            {
                cost = Diplomacy.DeclareWarCost(selectedRelation.Relation);

                diplomacyCostToHud(cost, content, true);
            }

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            IconName.Relation(rel, out SpriteName relIcon, out string relName);
            content.Add(new RbImage(relIcon));
            content.hspace();
            content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, relName)));

            if (rel == RelationType.RelationTypeN3_Mobilization)
            {
                var time = otherfaction.GetPlayer().GetHumanPlayer().NetClientSettings().clientPtoP.warDeclarePreparationTime.time.TimeSpan;
                HudLib.LabelAndText(content, SpriteName.cmdIconTimeOut, ".War preparation time", HudLib.TimeSpan_LongText(time));
            }
        }

        void peaceTooltip(RichBoxContent content, object tag)
        {
            bool peace_notTruce = (bool)tag;
            int cost = Diplomacy.EndWarCost(otherfaction, selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, peace_notTruce);
            RelationType toRelation = peace_notTruce ? RelationType.RelationType1_Peace : RelationType.RelationTypeN2_Truce;
            
            diplomacyCostToHud(cost, content);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            IconName.Relation(toRelation, out SpriteName relIcon, out string relName);
            content.Add(new RbImage(relIcon));
            content.hspace();
            content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, relName)));

            if (peace_notTruce == false)
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_TruceTimeLength, DssConst.TruceTimeSec)));

                if (DssRef.difficulty.UseTruceFailure(out float failChance))
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.cmdWarningTriangle));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.Event_ChanceOfFailure, TextLib.PercentText(failChance))));
                }
            }
        }

        void allianceAction(bool ally_notFriend)
        {
            int cost = Diplomacy.AllianceCost(player, otherfaction, selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, ally_notFriend, out _);

            if (player.diplomaticPoints.pay(cost, false))
            {
                if (ally_notFriend)
                {
                    ++player.statistics.AlliedFactions;
                    DssRef.world.diplomacy.SetRelationType(player.faction, otherfaction, RelationType.RelationType3_Ally);
                }
                else
                {
                    DssRef.world.diplomacy.SetRelationType(player.faction, otherfaction, RelationType.RelationType2_Good, DssConst.PeaceSafeTimeSec.GetRandom());

                    //selectedRelation.RelationEnd_GameTimeSec.setTimeFromNow(DssConst.PeaceSafeTimeSec.GetRandom());
                }

                player.hud.needRefresh = true;
            }
        }

        void allianceTooltip(RichBoxContent content, object tag)
        {
            bool ally_notFriend = (bool)tag;
            RelationType toRelation = ally_notFriend ? RelationType.RelationType3_Ally : RelationType.RelationType2_Good;
            IconName.Relation(toRelation, out SpriteName relIcon, out string relName);

            int cost = 0;
            if (otherfaction.player.IsBot())
            {
                cost = Diplomacy.AllianceCost(player, otherfaction, selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, ally_notFriend, out int allyCountCost);
                diplomacyCostToHud(cost, content);
            }

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain, HudLib.TitleColor_Label);
            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbImage(relIcon));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ForgeNewRelationTo, relName)));
            
            if (ally_notFriend)
            {                
                content.text(DssRef.lang.Diplomacy_AllyDescription, HudLib.InfoYellow_Light);
                var opponents = otherfaction.CollectWars();
                for (int i = opponents.Count - 1; i >= 0; --i)
                {
                    var opprelation = DssRef.world.diplomacy.GetRelation(player.faction, opponents[i]).Relation;
                    if (opprelation <= RelationType.RelationTypeN2_Truce)
                    {
                        opponents.RemoveAt(i);
                    }
                }

                if (opponents.Count > 0)
                {
                    content.newLine();
                    content.h2(DssRef.lang.Diplomacy_DeclareWarAgainst, HudLib.TitleColor_Label);
                }
                foreach (var m in opponents)
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                   
                    var opprelation = DssRef.world.diplomacy.GetRelation(player.faction, m).Relation;
                    IconName.Relation(opprelation, out SpriteName opprelIcon, out string opprelName);
                    content.Add(new RbImage(opprelIcon));
                    content.space();
                    content.Add(m.FlagTextureToHud());
                    content.Add(new RbText(m.PlayerName));
                }
            }
            else
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(DssRef.lang.Diplomacy_GoodRelationDescription));
            }

            if (otherfaction.player.IsBot())
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_BreakingRelationCost, Diplomacy.DeclareWarCost(toRelation))));

                if (ally_notFriend && DssRef.difficulty.AllyCountCost())
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_CostPerAlly, DssConst.DiplomacyExtraCostPerAlly)));

                    content.newLine();
                    content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Diplomacy_AllyCount, player.alliedFactions.Count), HudLib.InfoYellow_Light));
                }
            }
        }

        void servantAction()
        {
            if (canMakeServant(out int cost) &&
                player.diplomaticPoints.pay(cost, false))
            {
                makeServant();
            }
        }

        public void makeServant()
        { 
            ++player.statistics.ServantFactions;
            otherfaction.mergeTo(player.faction);
            player.gameControls.diplomacy?.cancel();
        }

        bool canMakeServant(out int cost)
        {
            cost = Diplomacy.MakeServantCost(player, againstDark);

            return selectedRelation.Relation == RelationType.RelationType3_Ally &&
                player.faction.militaryStrength >= Diplomacy.MiltitaryStrengthXServant * otherfaction.militaryStrength && 
                player.diplomaticPoints.Int() >= cost &&
                otherfaction.cities.Count <= DssRef.world.diplomacy.ServantMaxCities &&
                hasStrongerFoe();
        }

        bool hasStrongerFoe()
        {
            List<int> wars = new List<int>(8);
            DssRef.world.diplomacy.collectWars(otherfaction, wars);

            foreach (var w in wars)
            {
                if (DssRef.world.factions[w].militaryStrength > otherfaction.militaryStrength * 1.2f)
                { 
                    return true;
                }
            }
            return false;
        }

        void servantTooltip(RichBoxContent content, object tag)
        {
            int cost = Diplomacy.MakeServantCost(player, againstDark);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            
            HudLib.BulletPoint(content);
            {
                content.Add(new RbText(string.Format(DssRef.lang.Diplomacy_ServantRequirement_XStrongerMilitary, Diplomacy.MiltitaryStrengthXServant)));
                content.newLine();
                content.Add(new RbText(string.Format(DssRef.lang.Hud_CompareMilitaryStrength_YourToOther, Convert.ToInt32(player.faction.militaryStrength), Convert.ToInt32(otherfaction.militaryStrength)), 
                    HudLib.ResourceCostColor(player.faction.militaryStrength >= otherfaction.militaryStrength * Diplomacy.MiltitaryStrengthXServant)));
                
                content.newLine();
            }
            HudLib.BulletPoint(content);
            {
                string militaryStrength = DssRef.lang.Diplomacy_ServantRequirement_HopelessWar;
                content.Add(new RbText(militaryStrength, HudLib.ResourceCostColor(hasStrongerFoe())));
                content.newLine();
            }
            HudLib.BulletPoint(content);
            {
                string militaryStrength = DssRef.lang.Diplomacy_ServantRequirement_MaxCities;
                content.Add(new RbText(string.Format(militaryStrength, DssRef.world.diplomacy.ServantMaxCities), HudLib.ResourceCostColor(otherfaction.cities.Count <= DssRef.world.diplomacy.ServantMaxCities)));
                content.newLine();
            }

            content.newLine();

            diplomacyCostToHud(cost, content);
            content.text(DssRef.lang.Diplomacy_ServantPriceWillRise);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
            
            content.text(DssRef.lang.Diplomacy_ServantGainAbsorbFaction);
        }

        void diplomacyCostToHud(int cost, RichBoxContent content, bool allowNegative = false)
        {
            content.h2(DssRef.lang.Hud_PurchaseTitle_Cost).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            if (allowNegative)
            {
                content.Add(new RbImage(SpriteName.WarsDiplomaticSub));
                content.space(0.5f);
                content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCount, DssRef.lang.ResourceType_DiplomacyPoints, cost)));
         
            }
            else
            {
                HudLib.ResourceCost(content, ResourceType.DiplomaticPoint, cost, player.diplomaticPoints.Int());
            }
            content.newLine();
        }
    }
}
