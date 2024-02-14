using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Valve.Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.GO.PickUp;
using VikingEngine.ToGG.MoonFall;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Display
{
    class DiplomacyDisplay : RichboxGuiPart
    {
        Players.LocalPlayer player;
        DiplomaticRelation selectedRelation;

        public DiplomacyDisplay(RichboxGui gui, Players.LocalPlayer player)
           : base(gui)
        { 
            this.player = player;
        }

        public void refresh(Vector2 pos)
        {
            interaction?.DeleteMe();
            interaction = null;

            bool selection;
            var faction = player.diplomacyMap.mainSelection(out selection);

            setVisible(faction != null);

            if (faction != null)
            {
                beginRefresh();
                toHud(faction, selection);
                endRefresh(pos, true);
            }
        }

        void toHud(Faction faction, bool selection)
        {
            selectedRelation = player.faction.diplomaticRelations[faction.index];
            if (selectedRelation == null)
            {
               selectedRelation = DssRef.diplomacy.SetRelationType(player.faction, faction, RelationType.RelationType0_Neutral, true);
            }

            if ( selectedRelation!= null)
            {
                
                content.Add(new RichBoxBeginTitle(2));
                content.Add(faction.FlagTextureToHud());
                content.Add(new RichBoxText(faction.PlayerName));
                content.Add(new RichBoxSeperationLine());

                string relation = "Relation: ";
                //content.text(string.Format(relation, Diplomacy.RelationString(selectedRelation.Relation)));

                content.Add(new RichBoxText(relation));
                content.Add(new RichBoxImage(Diplomacy.RelationSprite(selectedRelation.Relation)));
                content.Add(new RichBoxText(Diplomacy.RelationString(selectedRelation.Relation)));

                content.newLine();

                if (selectedRelation.Relation == RelationType.RelationTypeN2_Truce)
                {
                    string truceLength = "Ends in {0} seconds";
                    int sec = Convert.ToInt32(selectedRelation.RelationEnd_GameTimeSec - Ref.TotalGameTimeSec);
                    content.text(string.Format(truceLength, sec));
                }

                string speakTerms = "Speaking terms: {0}";
                content.text(string.Format(speakTerms, Diplomacy.SpeakTermsString(selectedRelation.SpeakTerms)));

                if (selectedRelation.SpeakTerms > SpeakTerms.SpeakTermsN2_None &&
                    faction.player.IsAi())
                {
                    content.newParagraph();
                    if (selectedRelation.Relation <= RelationType.RelationTypeN3_War)
                    {
                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {
                            new RichBoxImage(SpriteName.WarsRelationTruce),
                            new RichBoxText("Forge truce"),
                        },
                            new RbAction1Arg<bool>(peaceAction, false, SoundLib.menuBuy),
                            new RbAction1Arg<bool>(peaceTooltip, false),
                            canForgePeace(false)));

                    }
                    else if (selectedRelation.Relation == RelationType.RelationTypeN2_Truce)
                    {
                        content.newLine();

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {

                            new RichBoxImage(SpriteName.WarsRelationTruce),
                            new RichBoxText("Extend truce"),
                        },
                            new RbAction(extendTruceAction, SoundLib.menuBuy),
                            new RbAction(extendTruceTooltip),
                            canExtendTruce()));
                    }

                    if (selectedRelation.Relation <= RelationType.RelationTypeN2_Truce)
                    {
                        content.newLine();

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {
                            new RichBoxImage(SpriteName.WarsRelationPeace),
                            new RichBoxText("Forge peace"),
                        },
                            new RbAction1Arg<bool>(peaceAction, true, SoundLib.menuBuy),
                            new RbAction1Arg<bool>(peaceTooltip, true),
                            canForgePeace(true)));
                    }

                    if (selectedRelation.Relation == RelationType.RelationType0_Neutral ||
                        selectedRelation.Relation == RelationType.RelationType1_Peace)
                    {
                        content.newLine();

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                        {
                            new RichBoxImage(SpriteName.WarsRelationGood),
                            new RichBoxText("Forge good relations"),
                        },
                            new RbAction1Arg<bool>(allianceAction, false, SoundLib.menuBuy),
                            new RbAction1Arg<bool>(allianceTooltip, false),
                            canForgeAlliance(false)));
                    }

                    if (selectedRelation.Relation == RelationType.RelationType2_Good)
                    {
                        content.newLine();

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                            {
                                new RichBoxImage(SpriteName.WarsRelationAlly),
                                new RichBoxText("Forge alliance"),
                            },
                            new RbAction1Arg<bool>(allianceAction, true, SoundLib.menuBuy),
                            new RbAction1Arg<bool>(allianceTooltip, true),
                            canForgeAlliance(true)));
                    }

                    if (selectedRelation.Relation == RelationType.RelationType3_Ally)
                    {
                        content.newLine();

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>()
                            {
                                new RichBoxText("Absorb as servant"),
                            },
                            new RbAction(servantAction, SoundLib.menuBuy),
                            new RbAction(servantTooltip),
                            canMakeServant()));
                    }
                }

            }
        }

        void extendTruceAction()
        {
            int cost = Diplomacy.ExtendTruceCost();
            if (player.diplomaticPoints.pay(cost, false))
            {
                selectedRelation.RelationEnd_GameTimeSec += DssLib.TruceTimeSec;
                player.hud.needRefresh = true;
            }
        }
        void extendTruceTooltip() 
        {
            int cost = Diplomacy.ExtendTruceCost();

            RichBoxContent content = new RichBoxContent();

            diplomacyCostToHud(cost, content);
            string truceDesc = "Extends truce by {0} seconds";
            content.text(string.Format(truceDesc, DssLib.TruceTimeSec));

            player.hud.tooltip.create(player, content, true);
        }

        bool canExtendTruce()
        {
            return player.diplomaticPoints.Int() >= Diplomacy.ExtendTruceCost();
        }

        void peaceAction(bool peace_notTruce)
        {
            //stoppa armeer med attack command
            Faction faction = selectedRelation.opponent(player.faction);
            int cost = Diplomacy.EndWarCost(selectedRelation.Relation, selectedRelation.SpeakTerms, peace_notTruce);

            if (player.diplomaticPoints.pay(cost, false))
            {               
                if (peace_notTruce)
                {
                    DssRef.diplomacy.SetRelationType(player.faction, faction, RelationType.RelationType1_Peace);
                }
                else
                {
                    DssRef.diplomacy.SetRelationType(player.faction, faction, RelationType.RelationTypeN2_Truce);
                    
                    selectedRelation.RelationEnd_GameTimeSec = Ref.TotalGameTimeSec + DssLib.TruceTimeSec;
                }
            }
        }

        bool canForgePeace(bool peace_notTruce)
        { 
            
            return player.diplomaticPoints.Int() >= Diplomacy.EndWarCost(selectedRelation.Relation, selectedRelation.SpeakTerms, peace_notTruce);
        }

        void peaceTooltip(bool peace_notTruce)
        {
            int cost = Diplomacy.EndWarCost(selectedRelation.Relation, selectedRelation.SpeakTerms, peace_notTruce);
            RelationType toRelation = peace_notTruce ? RelationType.RelationType0_Neutral : RelationType.RelationTypeN2_Truce;
            RichBoxContent content = new RichBoxContent();

            diplomacyCostToHud(cost, content);

            content.h2("Gain");
            content.newLine();
            string newRelationString = "New relation: ";

            content.Add(new RichBoxText(newRelationString));
            content.Add(new RichBoxImage(Diplomacy.RelationSprite(toRelation)));
            content.Add(new RichBoxText(Diplomacy.RelationString(toRelation)));

            string truceTimeString = "For {0} seconds";
            content.text(string.Format(truceTimeString, DssLib.TruceTimeSec));


            player.hud.tooltip.create(player, content, true);
        }

        void allianceAction(bool ally_notFriend)
        {
            Faction faction = selectedRelation.opponent(player.faction);
            int cost = Diplomacy.AllianceCost(selectedRelation.Relation, selectedRelation.SpeakTerms, ally_notFriend);

            if (player.diplomaticPoints.pay(cost, false))
            {
                if (ally_notFriend)
                {
                    DssRef.diplomacy.SetRelationType(player.faction, faction, RelationType.RelationType3_Ally);
                }
                else
                {
                    DssRef.diplomacy.SetRelationType(player.faction, faction, RelationType.RelationType2_Good);
                }

                player.hud.needRefresh = true;
            }
        }

        bool canForgeAlliance(bool ally_notFriend)
        {
            return player.diplomaticPoints.Int() >= Diplomacy.AllianceCost(selectedRelation.Relation, selectedRelation.SpeakTerms, ally_notFriend);
        }

        void allianceTooltip(bool ally_notFriend)
        {
            int cost = Diplomacy.AllianceCost(selectedRelation.Relation, selectedRelation.SpeakTerms, ally_notFriend);
            RelationType toRelation = ally_notFriend ? RelationType.RelationType3_Ally : RelationType.RelationType2_Good;

            RichBoxContent content = new RichBoxContent();

            diplomacyCostToHud(cost, content);

            content.h2("Gain");
            content.newLine();
            string newRelationString = "New relation: ";

            content.Add(new RichBoxText(newRelationString));
            content.Add(new RichBoxImage(Diplomacy.RelationSprite(toRelation)));
            content.Add(new RichBoxText(Diplomacy.RelationString(toRelation)));
            //content.text(string.Format(newRelationString, Diplomacy.RelationString(toRelation)));

            if (ally_notFriend)
            {
                content.text("Allies share war declarations.");
            }
            else
            {
                content.text("Limits the ability to declare war.");
            }
            content.text(string.Format("Breaking the relation will cost {0} diplomacy", Diplomacy.DeclareWarCost(toRelation)));

            player.hud.tooltip.create(player, content, true);
        }

        void servantAction()
        {
            Faction faction = selectedRelation.opponent(player.faction);
            int cost = Diplomacy.MakeServantCost(player);

            if (canMakeServant() &&
                player.diplomaticPoints.pay(cost, false))
            {
                player.servantFactions++;
                faction.mergeTo(player.faction);

                player.hud.needRefresh = true;
            }
        }

        bool canMakeServant()
        {
            Faction faction = selectedRelation.opponent(player.faction);

            return selectedRelation.Relation == RelationType.RelationType3_Ally &&
                player.faction.militaryStrength >= Diplomacy.MiltitaryStrengthXServant * faction.militaryStrength && 
                player.diplomaticPoints.Int() >= Diplomacy.MakeServantCost(player) &&
                faction.cities.Count <= DssRef.diplomacy.ServantMaxCities &&
                hasStrongerFoe();
        }

        bool hasStrongerFoe()
        {
            Faction faction = selectedRelation.opponent(player.faction);
            var wars = DssRef.diplomacy.collectWars(faction);

            foreach (var w in wars)
            {
                if (DssRef.world.factions[w].militaryStrength > faction.militaryStrength * 1.2f)
                { 
                    return true;
                }
            }
            return false;
        }

        void servantTooltip()
        {
            Faction faction = selectedRelation.opponent(player.faction);
            int cost = Diplomacy.MakeServantCost(player);
            
            RichBoxContent content = new RichBoxContent();

            content.h2("Requirement");
            content.newLine();
            
            content.ListDot();
            {
                string militaryStrength = "{0}x stronger military power";
                content.Add(new RichBoxText(string.Format(militaryStrength, Diplomacy.MiltitaryStrengthXServant)));
                content.newLine();
                string militaryStrengthCompare = "Strength: Your {0} - Their {1}";
                //content.title("Military strength");
                //string 
                //content.newLine();
                content.Add(new RichBoxText(string.Format(militaryStrengthCompare, Convert.ToInt32(player.faction.militaryStrength), Convert.ToInt32(faction.militaryStrength)), 
                    HudLib.ResourceCostColor(player.faction.militaryStrength >= faction.militaryStrength * Diplomacy.MiltitaryStrengthXServant)));
                //content.text(faction.player.Name + ": " + Convert.ToInt32(faction.militaryStrength));
                content.newLine();
            }
            content.ListDot();
            {
                string militaryStrength = "Servant must be in war against a stronger foe";
                content.Add(new RichBoxText(militaryStrength, HudLib.ResourceCostColor(hasStrongerFoe())));
                content.newLine();
            }
            content.ListDot();
            {
                string militaryStrength = "Servant can have max {0} citites";
                content.Add(new RichBoxText(string.Format(militaryStrength, DssRef.diplomacy.ServantMaxCities), HudLib.ResourceCostColor(faction.cities.Count <= DssRef.diplomacy.ServantMaxCities)));
                content.newLine();
            }

            //content.Add(HudLib.ResourceCost("Military strength", Convert.ToInt32(faction.militaryStrength * Diplomacy.MiltitaryStrengthXServant), Convert.ToInt32(player.faction.militaryStrength)));
            content.newLine();

            diplomacyCostToHud(cost, content);
            content.text("Price will raise for each servant");

            content.h2("Gain");
            
            content.text("Absorb the other faction");

            player.hud.tooltip.create(player, content, true);
        }

        void diplomacyCostToHud(int cost, RichBoxContent content)
        {
            content.h2("Cost");
            content.newLine();
            HudLib.ResourceCost(content, SpriteName.WarsDiplomaticSub,"Diplomacy points", cost, player.diplomaticPoints.Int());
            content.newLine();
        }
    }
}
