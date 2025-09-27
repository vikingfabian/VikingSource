using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Communication
{
    struct DiplomacyOption
    {
        public RelationType toRelation;
        public int cost;
        public bool available;
    }
    class DiplomacyActionManager
    {
        LocalPlayer player;
        Faction botFaction;
        DiplomaticRelation selectedRelation;
        bool againstDark;


        public List<DiplomacyOption> diplomacyOptionsToBot(LocalPlayer player, Faction botFaction)
        {
            List<DiplomacyOption> result = new List<DiplomacyOption>(); 

            this.player = player;
            this.botFaction = botFaction;
            
            selectedRelation = player.faction.diplomaticRelations[botFaction.myIndex];
            againstDark = botFaction.WantToAllyAgainstDark() && player.faction.diplomaticSide == DiplomaticSide.Light;

            if (selectedRelation.SpeakTerms > SpeakTerms.SpeakTermsN2_None &&
                botFaction.player.IsBot())
            {
                if (selectedRelation.Relation <= RelationType.RelationTypeN3_War)
                {
                    bool available = canForgePeace(false, out int cost);
                    DiplomacyOption truce = new DiplomacyOption()
                    {
                        toRelation = RelationType.RelationTypeN2_Truce,
                        available = available,
                        cost = cost,
                    };
                    result.Add(truce);
                }
                else if (selectedRelation.Relation == RelationType.RelationTypeN2_Truce)
                {
                    bool available = canExtendTruce(out int cost);
                    DiplomacyOption extendTruce = new DiplomacyOption()
                    {
                        toRelation = RelationType.ExtendTruce,
                        available = available,
                        cost = cost,
                    };
                    result.Add(extendTruce);
                }

                if (selectedRelation.Relation <= RelationType.RelationTypeN2_Truce)
                {
                    bool available = canForgePeace(true, out int cost);
                    DiplomacyOption peace = new DiplomacyOption()
                    {
                        toRelation = RelationType.RelationType1_Peace,
                        available = available,
                        cost = cost,
                    };
                    result.Add(peace);
                }

                if (selectedRelation.Relation == RelationType.RelationType0_Neutral ||
                        selectedRelation.Relation == RelationType.RelationType1_Peace)
                {
                    bool available = canForgeAlliance(false, out int cost);
                    DiplomacyOption friendly = new DiplomacyOption()
                    {
                        toRelation = RelationType.RelationType2_Good,
                        available = available,
                        cost = cost,
                    };
                    result.Add(friendly);
                }

                if (selectedRelation.Relation == RelationType.RelationType2_Good)
                {
                    bool available = canForgeAlliance(true, out int cost);
                    DiplomacyOption ally = new DiplomacyOption()
                    {
                        toRelation = RelationType.RelationType3_Ally,
                        available = available,
                        cost = cost,
                    };
                    result.Add(ally);
                }

                if (selectedRelation.Relation == RelationType.RelationType3_Ally)
                {
                    bool available = canMakeServant(out int cost);
                    DiplomacyOption servant = new DiplomacyOption()
                    {
                        toRelation = RelationType.RelationType4_Servant,
                        available = available,
                        cost = cost,
                    };
                    result.Add(servant);
                }
            }

            return result;
        }

        bool canForgePeace(bool peace_notTruce, out int cost)
        {
            cost = Diplomacy.EndWarCost(botFaction, selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, peace_notTruce);
            return player.diplomaticPoints.Int() >= cost;
        }
        bool canExtendTruce(out int cost)
        {
            cost = Diplomacy.ExtendTruceCost();
            return player.diplomaticPoints.Int() >= cost;
        }
        bool canForgeAlliance(bool ally_notFriend, out int cost)
        {
            cost = Diplomacy.AllianceCost(player, botFaction, selectedRelation.Relation, selectedRelation.SpeakTerms, againstDark, ally_notFriend, out _);
            return player.diplomaticPoints.Int() >= cost;
        }
        bool canMakeServant(out int cost)
        {
            cost = Diplomacy.MakeServantCost(player, againstDark);

            return selectedRelation.Relation == RelationType.RelationType3_Ally &&
                player.faction.militaryStrength >= Diplomacy.MiltitaryStrengthXServant * botFaction.militaryStrength &&
                player.diplomaticPoints.Int() >= cost &&
                botFaction.cities.Count <= DssRef.diplomacy.ServantMaxCities &&
                hasStrongerFoe();
        }
        bool hasStrongerFoe()
        {
            var wars = DssRef.diplomacy.collectWars(botFaction);

            foreach (var w in wars)
            {
                if (DssRef.world.factions[w].militaryStrength > botFaction.militaryStrength * 1.2f)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
