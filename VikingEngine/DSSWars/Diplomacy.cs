﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Players;
using VikingEngine.LootFest.Players;
using VikingEngine.PJ.Strategy;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars
{
    class Diplomacy
    {
        public const float MiltitaryStrengthXServant = 2f;
       
        List<int> aiPlayerAsynchUpdate_wars = new List<int>();
        List<int> aiPlayerAsynchUpdate_wars_withplayer = new List<int>();

        public int ServantMaxCities = 2;
        public int DefaultMaxDiplomacy = 4;
        public double DefaultDiplomacyPerSecond = 1.0 / 120.0;
        public double NobelHouseAddDiplomacy = 1.0 / 480.0;
        public double NobelHouseAddMaxDiplomacy = 0.25;


        public double AddDiplomacy_AfterSoftlock_PerSecond = 1 / 120.0;
        public double Diplomacy_HardMax_Add = 5;

        public double SpeakTermsOnWar_BadChance;
        public double SpeakTermsOnWar_NoneChance;

        public double SpeakTermsOnNeigbor_BadChance; //todo not in war with neighbor
        public double SpeakTermsOnNeigbor_NoneChance;

        public Diplomacy()
        {
            DssRef.diplomacy = this;

            switch (DssRef.difficulty.diplomacyDifficulty)
            {
                case 0:
                    DefaultMaxDiplomacy = 4;
                    DefaultDiplomacyPerSecond = 1.0 / 60.0;
                    NobelHouseAddDiplomacy = 1.0 / 240.0;
                    NobelHouseAddMaxDiplomacy = 0.5;
                    ServantMaxCities = 4;

                    SpeakTermsOnWar_BadChance = 0.3;
                    SpeakTermsOnWar_NoneChance = 0.04;
                    SpeakTermsOnNeigbor_BadChance = 0.2;
                    SpeakTermsOnNeigbor_NoneChance = 0;
                    break;

                case 1:
                    DefaultMaxDiplomacy = 3;
                    DefaultDiplomacyPerSecond = 1.0 / 90.0;
                    NobelHouseAddDiplomacy = 1.0 / 300.0;
                    NobelHouseAddMaxDiplomacy = 0.25;

                    SpeakTermsOnWar_BadChance = 0.5;
                    SpeakTermsOnWar_NoneChance = 0.08;
                    SpeakTermsOnNeigbor_BadChance = 0.4;
                    SpeakTermsOnNeigbor_NoneChance = 0.05;
                    break;

                case 2:
                    DefaultMaxDiplomacy = 2;
                    DefaultDiplomacyPerSecond = 1.0 / 120.0;
                    NobelHouseAddDiplomacy = 1.0 / 600.0;
                    NobelHouseAddMaxDiplomacy = 0.1;

                    SpeakTermsOnWar_BadChance = 0.8;
                    SpeakTermsOnWar_NoneChance = 0.2;
                    SpeakTermsOnNeigbor_BadChance = 0.75;
                    SpeakTermsOnNeigbor_NoneChance = 0.2;
                    break;
            }

        }

        public void async_update()
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                for (int relIx = 0; relIx < p.faction.diplomaticRelations.Length; ++relIx)
                {
                    var rel = p.faction.diplomaticRelations[relIx];
                    if (rel != null)
                    { 
                        rel.truce_update();
                    }
                }
            }
        }



        public List<int> aiPlayerAsynchUpdate_collectWars(Faction aifaction)
        {
            aiPlayerAsynchUpdate_wars.Clear();
            aiPlayerAsynchUpdate_wars_withplayer.Clear();
            for (int relIx = 0; relIx < aifaction.diplomaticRelations.Length; ++relIx)
            {
                if (InWar(aifaction, DssRef.world.factions[relIx])) 
                {
                    if (DssRef.world.factions[relIx].player.IsPlayer())
                    {
                        aiPlayerAsynchUpdate_wars_withplayer.Add(relIx);
                    }
                    aiPlayerAsynchUpdate_wars.Add(relIx);
                }
            }

            if (aiPlayerAsynchUpdate_wars_withplayer.Count > 0 &&
                (
                    DssRef.difficulty.aiAggressivity == AiAggressivity.High ||
                    (DssRef.difficulty.aiAggressivity == AiAggressivity.Medium && Ref.rnd.Chance(0.5))
                ))
            {
                return aiPlayerAsynchUpdate_wars_withplayer;
            }
            else
            {
                return aiPlayerAsynchUpdate_wars;
            }
        }

        public List<int> collectWars(Faction aifaction)
        {
            List<int> wars = new List<int>();

            for (int relIx = 0; relIx < aifaction.diplomaticRelations.Length; ++relIx)
            {
                if (InWar(aifaction, DssRef.world.factions[relIx])) // aifaction.diplomaticRelations[relIx] != null && aifaction.diplomaticRelations[relIx].Relation == RelationTypeN3_War)
                {
                    wars.Add(relIx);
                }
            }
            return wars;
        }

        public RelationType GetRelationType(Faction faction1, Faction faction2)
        {
            DiplomaticRelation rel = faction1.diplomaticRelations[faction2.parentArrayIndex];
            if (rel == null)
            {
                return RelationType.RelationType0_Neutral;
            }
            else
            {
                return rel.Relation;
            }
        }

        public bool InWar(Faction faction1, Faction faction2)
        {
            if (faction1 == null || faction2 == null)
            {
                return false;
            }

            if (faction1 == faction2)
            {
                return false;
            }

            DiplomaticRelation rel = faction1.diplomaticRelations[faction2.parentArrayIndex];
            if (rel == null)
            {
                return false;
            }
            else
            {
                return rel.Relation <=  RelationType.RelationTypeN3_War;
            }
        }

        public bool MayTrade(Faction faction1, Faction faction2, out RelationType relation)
        {
            relation = RelationType.RelationType0_Neutral;

            if (faction1 == null || faction2 == null)
            {
                return false;
            }

            if (faction1 == faction2)
            {
                return false;
            }

            DiplomaticRelation rel = faction1.diplomaticRelations[faction2.parentArrayIndex];           
            if (rel == null)
            {
                return true;
            }
            else
            {
                relation = rel.Relation;
                return rel.Relation >= RelationType.RelationType0_Neutral;
            }
        }

        public RelationType GetRelationType(int faction1, int faction2)
        {
            var faction1_pointer = DssRef.world.factions[faction1];
            if (faction1_pointer != null)
            {
                DiplomaticRelation rel = faction1_pointer.diplomaticRelations[faction2];
                if (rel == null)
                {
                    return RelationType.RelationType0_Neutral;
                }
                else
                {
                    return rel.Relation;
                }
            }
            return RelationType.RelationType0_Neutral;
        }

        public DiplomaticRelation GetOrCreateRelation(Faction faction1, Faction faction2)
        {
            DiplomaticRelation rel = faction1.diplomaticRelations[faction2.parentArrayIndex];
            if (rel == null)
            {
                rel = NewRelation(faction1, faction2, RelationType.RelationType0_Neutral);
            }
            return rel;
        }

        public DiplomaticRelation SetRelationType(Faction faction1, Faction faction2, RelationType newRelation, bool createOnNeutral = false)
        {
            if (faction1 != faction2)
            {
                DiplomaticRelation rel = faction1.diplomaticRelations[faction2.parentArrayIndex];
                if (rel != null)
                {
                    if (rel.Relation != newRelation)
                    { 
                        RelationType previous = rel.Relation;
                        rel.Relation = newRelation;
                        faction1.player.onNewRelation(faction2, rel, previous);
                        faction2.player.onNewRelation(faction1, rel, previous);

                    }
                }
                else if (newRelation != RelationType.RelationType0_Neutral || createOnNeutral)
                {
                    rel = NewRelation(faction1, faction2, newRelation);
                }

                return rel;
            }

            return null;
        }

        DiplomaticRelation NewRelation(Faction faction1, Faction faction2, RelationType newRelation)
        {
            if (faction1 != faction2)
            {
                DiplomaticRelation rel;
                SpeakTerms speakterms = (SpeakTerms)Math.Min((int)faction1.DefaultSpeakingTerms(), (int)faction2.DefaultSpeakingTerms());
                rel = new DiplomaticRelation(faction1.parentArrayIndex, faction2.parentArrayIndex, newRelation, speakterms);

                faction1.player.onNewRelation(faction2, rel, RelationType.RelationType0_Neutral);
                faction2.player.onNewRelation(faction1, rel, RelationType.RelationType0_Neutral);
                return rel;
            }
            return null;    
        }

        public void declareWar(Faction attacker, Faction defender)
        {
            if (!InWar(attacker, defender))
            {
                RelationType prevRelation = GetRelationType(attacker, defender);
                var relation = SetRelationType(attacker, defender, RelationType.RelationTypeN3_War);

                if (attacker.player.IsPlayer())
                {
                    int cost = DeclareWarCost(prevRelation);
                    var player = attacker.player.GetLocalPlayer();
                    ++player.statistics.WarsStartedByYou;
                    player.diplomaticPoints.pay(cost, true);
                    DssRef.settings.OnPlayerDeclareWar();

                    if (prevRelation >= RelationType.RelationType1_Peace)
                    {
                        relation.SetWorseSpeakTerms(SpeakTermsOnWar_BadChance + 0.4, SpeakTermsOnWar_NoneChance + 0.4);
                    }
                    else
                    {
                        relation.SetWorseSpeakTerms(SpeakTermsOnWar_BadChance, SpeakTermsOnWar_NoneChance);
                    }

                    if (defender.player.IsPlayer())
                    {
                        var otherPlayer = defender.player.GetLocalPlayer();
                        var PtoP = player.toPlayerDiplomacies[otherPlayer.playerData.localPlayerIndex];
                        PtoP.suggestingNewRelation = false;
                    }
                }
                if (defender.player.IsPlayer())
                { 
                    ++defender.player.GetLocalPlayer().statistics.WarsStartedByEnemy;
                }
            }
        }

        public bool PositiveRelationWithPlayer(Faction faction)
        {
            if (faction.player.IsPlayer())
            { 
                return true;
            }

            foreach (var p in DssRef.state.localPlayers)
            {
                if (GetRelationType(faction, p.faction) >= RelationType.RelationType1_Peace)
                { 
                    return true;
                }
            }

            return false;
        }

        public bool NegativeRelationWithPlayer(Faction faction)
        {
            if (faction.player.IsPlayer())
            {
                return false;
            }

            foreach (var p in DssRef.state.localPlayers)
            {
                if (GetRelationType(faction, p.faction) <= RelationType.RelationTypeN1_Enemies)
                {
                    return true;
                }
            }

            return false;
        }

        public void onFactionDeath(Faction faction)
        {
            for (int relIx = 0; relIx < faction.diplomaticRelations.Length; ++relIx)
            {
                if (faction.diplomaticRelations[relIx] != null)
                {
                    DssRef.world.factions[relIx].diplomaticRelations[faction.parentArrayIndex] = null;
                }
            }
        }

        public static string RelationString(RelationType relation)
        {
            switch (relation)
            {
                case RelationType.RelationType4_Servant: return DssRef.lang.Diplomacy_RelationType_Servant;
                case RelationType.RelationType3_Ally: return DssRef.lang.Diplomacy_RelationType_Ally;
                case RelationType.RelationType2_Good: return DssRef.lang.Diplomacy_RelationType_Good;
                case RelationType.RelationType1_Peace: return DssRef.lang.Diplomacy_RelationType_Peace;
                case RelationType.RelationType0_Neutral: return DssRef.lang.Diplomacy_RelationType_Neutral;
                case RelationType.RelationTypeN2_Truce: return DssRef.lang.Diplomacy_RelationType_Truce;
                case RelationType.RelationTypeN3_War: return DssRef.lang.Diplomacy_RelationType_War;
                case RelationType.RelationTypeN4_TotalWar: return DssRef.lang.Diplomacy_RelationType_TotalWar;

                default:
                    throw new NotImplementedException("RelationString " + relation.ToString());
            }
        }
        public static SpriteName RelationSprite(RelationType relation)
        {
            switch (relation)
            {
                case RelationType.RelationType4_Servant: return SpriteName.NO_IMAGE;
                case RelationType.RelationType3_Ally: return SpriteName.WarsRelationAlly;
                case RelationType.RelationType2_Good: return SpriteName.WarsRelationGood;
                case RelationType.RelationType1_Peace: return SpriteName.WarsRelationPeace;
                case RelationType.RelationType0_Neutral: return SpriteName.WarsRelationNeutral;
                case RelationType.RelationTypeN2_Truce: return SpriteName.WarsRelationTruce;
                case RelationType.RelationTypeN3_War: return SpriteName.WarsRelationWar;
                case RelationType.RelationTypeN4_TotalWar: return SpriteName.WarsRelationTotalWar;

                default:
                    throw new NotImplementedException("RelationString " + relation.ToString());
            }
        }

        public static string SpeakTermsString(SpeakTerms speak)
        {
            switch (speak)
            {
                case SpeakTerms.SpeakTerms1_Good: return DssRef.lang.Diplomacy_SpeakTerms_Good;
                case SpeakTerms.SpeakTerms0_Normal: return DssRef.lang.Diplomacy_SpeakTerms_Normal;
                case SpeakTerms.SpeakTermsN1_Bad: return DssRef.lang.Diplomacy_SpeakTerms_Bad;
                case SpeakTerms.SpeakTermsN2_None: return DssRef.lang.Diplomacy_SpeakTerms_None;

                default:
                    throw new NotImplementedException("Speaking terms " + speak.ToString());
            }
        }

        public static int EndWarCost(RelationType relation, SpeakTerms speakterms, bool againstDark, bool peace_notTruce)
        {
            int cost = 0;
            cost -= (int)relation; //2 or 3
            cost -= (int)speakterms;//0

            if (peace_notTruce)
            {
                cost *= 2;
            }

            if (againstDark)
            {
                cost -= 1;
            }

            cost = Bound.Min(cost, 1);

            return cost;
        }

        public static int ExtendTruceCost()
        {
            return 1;
        }

        public static int MakeServantCost(LocalPlayer player, bool againstDark)
        {
            int baseCost = 5;
            if (againstDark)
            {
                baseCost -= 1;
            }
            return baseCost * (player.statistics.ServantFactions + 1);
        }

        public static int AllianceCost(RelationType relation, SpeakTerms speakterms, bool againstDark, bool ally_notFriend)
        {
            RelationType toRelation = ally_notFriend ? RelationType.RelationType3_Ally : RelationType.RelationType2_Good;
            int diff = toRelation - relation; //1 or 2

            int cost = diff * 2 + 1;
            cost -= (int)speakterms;//0

            int minCost = 2;

            if (againstDark)
            {
                minCost = 1;
                cost -= 1;
            }

            cost = Bound.Min(cost, minCost);
            
            return cost;
        }

        public static int DeclareWarCost(RelationType relation)
        {
            if (relation == RelationType.RelationTypeN2_Truce || 
                relation == RelationType.RelationType1_Peace)
            {
                return 2;
            }
            else if (relation == RelationType.RelationType2_Good)
            {
                return 3;
            }
            else if (relation == RelationType.RelationType3_Ally)
            {
                return 6;
            }
            
            return 1;
        }

        public static bool IsWar(RelationType relation)
        {
            return relation <= RelationType.RelationTypeN3_War;
        }

    }

    class DiplomaticRelation
    {
        int faction1, faction2;
        public RelationType Relation;
        public SpeakTerms SpeakTerms;
        public float RelationEnd_GameTimeSec;
        public bool secret = false;

        public DiplomaticRelation()
        { }

        public DiplomaticRelation(int faction1, int faction2, RelationType Relation, SpeakTerms speakterms)
        {
            this.Relation = Relation;
            this.SpeakTerms = speakterms;

            if (faction1 < faction2)
            {
                this.faction1 = faction1;
                this.faction2 = faction2;
            }
            else
            {
                this.faction1 = faction2;
                this.faction2 = faction1;
            }

            addToFactions();
        }

        public void addToFactions()
        { 
            DssRef.world.factions.Array[faction1].diplomaticRelations[faction2] = this;
            DssRef.world.factions.Array[faction2].diplomaticRelations[faction1] = this;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((short)faction1);
            w.Write((short)faction2);
            w.Write((sbyte)Relation);
            w.Write((sbyte)SpeakTerms);
            w.Write(Convert.ToUInt16(RelationEnd_GameTimeSec));
        }

        public bool read(System.IO.BinaryReader r, int version)
        {
            faction1 = r.ReadInt16();
            if (faction1 >= 0)
            {
                faction2 = r.ReadInt16();
                Relation = (RelationType)r.ReadSByte();
                SpeakTerms = (SpeakTerms)r.ReadSByte();
                RelationEnd_GameTimeSec = r.ReadUInt16();
                return true;
            }

            return false;
        }

        public bool opponentIsPlayer(Faction faction)
        {
            return !opponent(faction).player.IsAi();
        }

        public void SetWorseSpeakTerms(double subOneChance, double subTwoChance)
        {

            if (Ref.rnd.Chance(subTwoChance))
            {
                changeSpeakTerms(-2);
            }
            if (Ref.rnd.Chance(subOneChance))
            {
                changeSpeakTerms(-1);
            }
        }

        void changeSpeakTerms(int change)
        {
            SpeakTerms = (SpeakTerms)Bound.Set((int)SpeakTerms + change, (int)SpeakTerms.SpeakTermsN2_None, (int)SpeakTerms.SpeakTerms1_Good);
        }

        public Faction opponent(Faction faction)
        {
            if (faction.parentArrayIndex == faction1)
            {
                return DssRef.world.factions[faction2];
            }
            else
            {
                return DssRef.world.factions[faction1];
            }
        }

        public void truce_update()
        {
            if (Relation == RelationType.RelationTypeN2_Truce && 
                RelationEnd_GameTimeSec < Ref.TotalGameTimeSec)
            {
                Relation = RelationType.RelationTypeN3_War;
            }
        }

        public bool IsFactionOne(Faction faction)
        {
            return faction.parentArrayIndex == faction1;
        }
    }

    enum RelationType
    {
        RelationType4_Servant = 4,
        RelationType3_Ally = 3,
        RelationType2_Good = 2,
        RelationType1_Peace = 1,
        RelationType0_Neutral = 0,
        RelationTypeN1_Enemies = -1,
        RelationTypeN2_Truce = -2,
        RelationTypeN3_War = -3,
        RelationTypeN4_TotalWar = -4,
    }

    enum SpeakTerms
    {
        SpeakTerms1_Good = 1,
        SpeakTerms0_Normal = 0,
        SpeakTermsN1_Bad = -1,
        SpeakTermsN2_None = -2,
    }
}
