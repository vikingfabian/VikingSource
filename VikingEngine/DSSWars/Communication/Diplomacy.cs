using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars
{
    

    class Diplomacy
    {

        public const float MiltitaryStrengthXServant = 2f;
       
        List<int> aiPlayerAsynchUpdate_wars = new List<int>(8);
        List<int> aiPlayerAsynchUpdate_threats = new List<int>(8);
        List<int> aiPlayerAsynchUpdate_wars_withplayer = new List<int>(2);
        public List<int> aiPlayerAsynchUpdate_collectAlliances = new List<int>(8);

        public int ServantMaxCities = 2;
        public int DefaultMaxDiplomacy = 4;
        public double DefaultDiplomacyPerSecond = 1.0 / 240.0;
        public double EmbassyAddDiplomacy = 1.0 / 480.0;
        public double EmbassyAddMaxDiplomacy = 0.25;


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
                    EmbassyAddDiplomacy = 1.0 / 240.0;
                    EmbassyAddMaxDiplomacy = 0.5;
                    ServantMaxCities = 4;

                    SpeakTermsOnWar_BadChance = 0.3;
                    SpeakTermsOnWar_NoneChance = 0.04;
                    SpeakTermsOnNeigbor_BadChance = 0.2;
                    SpeakTermsOnNeigbor_NoneChance = 0;
                    break;

                case 1:
                    DefaultMaxDiplomacy = 3;
                    DefaultDiplomacyPerSecond = 1.0 / 90.0;
                    EmbassyAddDiplomacy = 1.0 / 300.0;
                    EmbassyAddMaxDiplomacy = 0.25;

                    SpeakTermsOnWar_BadChance = 0.5;
                    SpeakTermsOnWar_NoneChance = 0.08;
                    SpeakTermsOnNeigbor_BadChance = 0.4;
                    SpeakTermsOnNeigbor_NoneChance = 0.05;
                    break;

                case 2:
                    DefaultMaxDiplomacy = 2;
                    DefaultDiplomacyPerSecond = 1.0 / 120.0;
                    EmbassyAddDiplomacy = 1.0 / 600.0;
                    EmbassyAddMaxDiplomacy = 0.1;

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

        public bool InplayerAlliance(Faction aifaction)
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                if (GetRelationType(p.faction, aifaction) >= RelationType.RelationType3_Ally)
                { 
                    return true;
                }
            }

            return false;
        }

        public bool OppositeDiplomaticSides(Faction faction1, Faction faction2)
        {
            if (faction1.diplomaticSide == DiplomaticSide.Light)
            {
                return faction2.diplomaticSide == DiplomaticSide.Dark;
            }
            else if (faction1.diplomaticSide == DiplomaticSide.Dark)
            {
                return faction2.diplomaticSide == DiplomaticSide.Light;
            }
            return false;
        }

        public List<int> aiPlayerAsynchUpdate_collectWars(Faction aifaction)
        {
            aiPlayerAsynchUpdate_wars.Clear();
            aiPlayerAsynchUpdate_wars_withplayer.Clear();
            for (int relIx = 0; relIx < aifaction.diplomaticRelations.Length; ++relIx)
            {
                var otherFaction = DssRef.world.faction(relIx);
                if (InWar(aifaction, otherFaction)) 
                {
                    if (otherFaction.player.IsLocalPlayer())
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

        public List<int> aiPlayerAsynchUpdate_collectThreats(Faction aifaction, float threatFactor = 1.6f)
        { 
            aiPlayerAsynchUpdate_threats.Clear();

            //var cities_c = aifaction.cities.counter();
            //while (cities_c.Next())
            //{
            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref aifaction.cities, DssRef.world.cities, out City city))
            {
                //foreach (var nCityIx in city.neighborCities)
                EcsStaticArrayCounter neighbors = city.CityNeighbors();
                while (neighbors.Next(DssRef.world.cities, out City nCity))//
                {
                    if (nCity.factionIndex != aifaction.myIndex &&
                        !aiPlayerAsynchUpdate_threats.Contains(nCity.myIndex))
                    {
                        aiPlayerAsynchUpdate_threats.Add(nCity.myIndex);
                    }
                }
            }

            for (int i = aiPlayerAsynchUpdate_threats.Count - 1; i >= 0; i--)
            {
                var otherFaction = DssRef.world.faction(aiPlayerAsynchUpdate_threats[i]);
                if (otherFaction == null ||
                    DssRef.diplomacy.GetRelationType(aifaction, otherFaction) >= RelationType.RelationType2_Good ||
                    aifaction.MyPlusAllianceStrengthValue() * threatFactor >= otherFaction.MyPlusAllianceStrengthValue())
                {
                    aiPlayerAsynchUpdate_threats.RemoveAt(i);
                }
            }

            return aiPlayerAsynchUpdate_threats;
        }

        public List<int> aiPlayerAsynchUpdate_GetAllied(Faction aifaction)
        {
            aiPlayerAsynchUpdate_collectAlliances.Clear();

            for (int relIx = 0; relIx < aifaction.diplomaticRelations.Length; ++relIx)
            {
                if (aifaction.diplomaticRelations[relIx] != null &&
                    relIx != aifaction.myIndex &&
                   aifaction.diplomaticRelations[relIx].Relation >= RelationType.RelationType3_Ally)
                {
                    aiPlayerAsynchUpdate_collectAlliances.Add(relIx);                    
                }
            }
            //aiPlayerAsynchUpdate_collectAlliances.Add(aifaction.myIndex);

            return aiPlayerAsynchUpdate_collectAlliances;
        }

        public bool aiPlayerAsynchUpdate_mayAlly_checkConflict(Faction faction1, Faction faction2, Faction enemyFaction, bool tryEndOtherWars)
        {
            List<int> allies = aiPlayerAsynchUpdate_GetAllied(faction1);

            foreach (int fIx in allies)
            {
                var ally = DssRef.world.faction(fIx);
                if (ally != null)
                {
                    if (DssRef.diplomacy.GetRelationType(ally, faction2) <= RelationType.RelationTypeN3_War)
                    {
                        return false;
                    }
                }
            }

            var wars1 = collectWars(faction1);
            var wars2 = collectWars(faction2);

            foreach (int war in wars1)
            {
                //Dont get dragged into more wars
                if (war != enemyFaction.myIndex && !wars2.Contains(war))
                {
                    if (tryEndOtherWars)
                    {
                        faction1.player.GetAiPlayer()?.tryEndBotWars(wars1);
                        faction2.player.GetAiPlayer()?.tryEndBotWars(wars2);
                    }

                    return false;
                }
            }

            return true;
        }

        public List<int> collectWars(Faction aifaction)
        {
            List<int> wars = new List<int>();

            for (int relIx = 0; relIx < aifaction.diplomaticRelations.Length; ++relIx)
            {
                if (InWar(aifaction, DssRef.world.faction(relIx)))
                {
                    wars.Add(relIx);
                }
            }
            return wars;
        }

        public RelationType GetRelationType(Faction faction1, Faction faction2)
        {
            if (faction1 != null && faction2 != null)
            {
                if (faction2.myIndex < faction1.diplomaticRelations.Length)
                {
                    DiplomaticRelation rel = faction1.diplomaticRelations[faction2.myIndex];
                    if (rel == null)
                    {
                        return RelationType.RelationType0_Neutral;
                    }
                    else
                    {
                        return rel.Relation;
                    }
                }
            }
            return RelationType.RelationType0_Neutral;
        }
        public bool InWar(int faction1, int faction2)
        {
            if (faction1 < 0 || faction2 < 0)
            {
                return false;
            }

            if (faction1 != faction2)
            {
                return InWar(DssRef.world.faction(faction1), DssRef.world.faction(faction2));
            }
            return false;
        }

        public bool InWarWithPlayer(Faction faction)
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                if (InWar(p.faction, faction))
                { 
                    return true;
                }
            }
            return false;
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

            DiplomaticRelation rel = faction1.diplomaticRelations[faction2.myIndex];
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

            DiplomaticRelation rel = faction1.diplomaticRelations[faction2.myIndex];           
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
            var faction1_pointer = DssRef.world.faction(faction1);
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
            DiplomaticRelation rel = faction1.diplomaticRelations[faction2.myIndex];
            if (rel == null)
            {
                rel = NewRelation(faction1, faction2, RelationType.RelationType0_Neutral);
            }
            return rel;
        }

        public DiplomaticRelation SetRelationType(Faction faction1, Faction faction2, RelationType newRelation, bool createOnNeutral = false)
        {
            if (faction1 != null && faction1 != faction2)
            {
                DiplomaticRelation rel = faction1.diplomaticRelations[faction2.myIndex];
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
                rel = new DiplomaticRelation(faction1.myIndex, faction2.myIndex, newRelation, speakterms);

                faction1.player.onNewRelation(faction2, rel, RelationType.RelationType0_Neutral);
                faction2.player.onNewRelation(faction1, rel, RelationType.RelationType0_Neutral);
                return rel;
            }
            return null;    
        }

        public bool botMayStartWar(Faction attacker, Faction defender)
        {
            if (attacker != null && 
                defender != null &&
                attacker.armies.Count > 0 &&
                attacker != defender &&
                attacker.player.IsBot())
            {
                var rel = DssRef.diplomacy.GetRelationType(defender, attacker);
                if (rel <= RelationType.RelationTypeN3_War)
                {
                    return true;
                }

                bool mayAttackPlayer = !DssRef.difficulty.peaceful && DssRef.state.events.MayAttackPlayer() && attacker.player.mayAttackPlayer;


                if (!mayAttackPlayer &&
                    (defender.player.IsLocalPlayer() || DssRef.diplomacy.InplayerAlliance(defender)))
                {
                    return false;
                }

                if (defender.player.IsLocalPlayer())
                {
                    if (attacker.militaryStrength < Math.Min(defender.militaryStrength * 0.25f, 6) ||
                        attacker.militaryStrength > defender.militaryStrength * 3f)
                    {
                        return false;
                    }
                }
                else
                {
                    if (defender.player.protectedFromBotAttacks)
                    {
                        if (defender.Size() >= FactionSize.Big && Ref.peRnd.Chance(0.25))
                        {
                            
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                    
                if (rel >= RelationType.RelationTypeN1_Enemies && rel < RelationType.RelationType1_Peace)
                {
                    return true;
                }
                else if (rel == RelationType.RelationType1_Peace ||
                    rel == RelationType.RelationType2_Good)
                {
                    var relation = DssRef.diplomacy.GetOrCreateRelation(defender, attacker);
                    if (relation.RelationEnd_GameTimeSec.HasTime())
                    {
                        return false;
                    }
                    return Ref.peRnd.Chance(0.05);
                }


            }
            return false;
        }

        public void endRelations(Faction actingFaction, Faction otherFaction)
        {
            if (actingFaction != null && otherFaction != null)
            {
                RelationType prevRelation = GetRelationType(actingFaction, otherFaction);
                if (prevRelation > RelationType.RelationType0_Neutral)
                {
                    SetRelationType(actingFaction, otherFaction, RelationType.RelationType0_Neutral);
                    if (actingFaction.player.IsLocalPlayer())
                    {
                        int cost = EndRelationCost(prevRelation);
                        var player = actingFaction.player.GetLocalPlayer();

                        player.diplomaticPoints.pay(cost, true);
                    }
                }
            }
        }

        public void declareWar(Faction attacker, Faction defender)
        {
            if (attacker != null && defender != null &&
                !InWar(attacker, defender))
            {
                RelationType prevRelation = GetRelationType(attacker, defender);
                var relation = SetRelationType(attacker, defender, RelationType.RelationTypeN3_War);

                if (relation != null)
                {

                    if (attacker.player.IsLocalPlayer())
                    {
                        int cost = DeclareWarCost(prevRelation);
                        var player = attacker.player.GetLocalPlayer();

                        player.diplomaticPoints.pay(cost, true);
                        DssRef.state.events?.onPlayerEnterWar(player, defender, true);

                        if (prevRelation >= RelationType.RelationType1_Peace)
                        {
                            relation.SetWorseSpeakTerms(SpeakTermsOnWar_BadChance + 0.4, SpeakTermsOnWar_NoneChance + 0.4);
                        }
                        else
                        {
                            relation.SetWorseSpeakTerms(SpeakTermsOnWar_BadChance, SpeakTermsOnWar_NoneChance);
                        }

                        if (prevRelation >= RelationType.RelationType3_Ally)
                        {
                            DssRef.achieve.UnlockAchievement(AchievementIndex.traitor);
                        }

                        if (defender.player.IsLocalPlayer())
                        {
                            var otherPlayer = defender.player.GetLocalPlayer();
                            var PtoP = player.toPlayerDiplomacies[otherPlayer.playerData.localPlayerIndex];
                            PtoP.suggestingNewRelation = false;
                        }
                    }
                    if (defender.player.IsLocalPlayer())
                    {
                        var player = defender.player.GetLocalPlayer();
                        DssRef.state.events?.onPlayerEnterWar(player, attacker, false);
                    }
                }
            }
        }

        public bool PositiveRelationWithPlayer(Faction faction, RelationType minRelation = RelationType.RelationType1_Peace)
        {
            if (faction.player.IsLocalPlayer())
            { 
                return true;
            }

            foreach (var p in DssRef.state.localPlayers)
            {
                if (GetRelationType(faction, p.faction) >= minRelation)
                { 
                    return true;
                }
            }

            return false;
        }

        public bool NegativeRelationWithPlayer(Faction faction)
        {
            if (faction.player.IsLocalPlayer())
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
            Task.Run(() =>
            {
                try
                {
                    for (int relIx = 0; relIx < faction.diplomaticRelations.Length; ++relIx)
                    {
                        if (faction.diplomaticRelations[relIx] != null)
                        {
                            var otherFaction = DssRef.world.faction(relIx);
                            if (otherFaction != null)
                            {
                                var f = DssRef.world.faction(relIx);
                                if (f != null)
                                { f.diplomaticRelations[faction.myIndex] = null; }
                            }
                        }
                    }

                    var factionsC = DssRef.world.factions.counter();
                    while (factionsC.Next())
                    {
                        for (int relIx = 0; relIx < factionsC.sel.diplomaticRelations.Length; ++relIx)
                        {
                            var rel = factionsC.sel.diplomaticRelations[relIx];
                            if (rel != null && rel.Relation >= RelationType.RelationType3_Ally && rel.allyAgainst == faction.myIndex)
                            {
                                rel.Relation = RelationType.RelationType0_Neutral;
                            }
                        }
                    }

                }
                catch (Exception ex) 
                {
                    BlueScreen.ThreadException = ex;
                }

            });
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
                case RelationType.RelationTypeN1_Enemies: return DssRef.lang.Diplomacy_RelationType_Enemies;
                case RelationType.RelationTypeN2_Truce: return DssRef.lang.Diplomacy_RelationType_Truce;
                case RelationType.RelationTypeN3_War: return DssRef.lang.Diplomacy_RelationType_War;
                case RelationType.RelationTypeN4_TotalWar: return DssRef.lang.Diplomacy_RelationType_TotalWar;
                case RelationType.ExtendTruce: return DssRef.lang.Diplomacy_ExtendTruceAction;

                default:
                    return TextLib.Error;
                    //throw new NotImplementedException("RelationString " + relation.ToString());
            }
        }
        public static SpriteName RelationSprite(RelationType relation)
        {
            switch (relation)
            {
                case RelationType.RelationType4_Servant: return SpriteName.WarsRelationServant;
                case RelationType.RelationType3_Ally: return SpriteName.WarsRelationAlly;
                case RelationType.RelationType2_Good: return SpriteName.WarsRelationGood;
                case RelationType.RelationType1_Peace: return SpriteName.WarsRelationPeace;
                case RelationType.RelationType0_Neutral: return SpriteName.WarsRelationNeutral;
                case RelationType.RelationTypeN1_Enemies: return SpriteName.WarsRelationEnemy;
                case RelationType.RelationTypeN2_Truce:
                case RelationType.ExtendTruce: return SpriteName.WarsRelationTruce;
                case RelationType.RelationTypeN3_War: return SpriteName.WarsRelationWar;
                case RelationType.RelationTypeN4_TotalWar: return SpriteName.WarsRelationTotalWar;

                default:
                    return SpriteName.MissingImage;
                    //throw new NotImplementedException("RelationString " + relation.ToString());
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

        public static int EndWarCost(Faction toFaction, RelationType relation, SpeakTerms speakterms, bool againstDark, bool peace_notTruce)
        {
            int cost = 0;
            cost -= (int)relation; //2 or 3
            cost -= (int)speakterms;//0

            int max = 6;
            if (peace_notTruce)
            {
                cost *= 2;
                max = 8;
            }

            cost += toFaction.WorkForceInCityCount() / 5;

            if (againstDark)
            {
                cost -= 1;
            }

            cost = Bound.Set(cost, 1, max);

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

        public static int AllianceCost(LocalPlayer player, Faction toFaction, RelationType relation, SpeakTerms speakterms, bool againstDark, bool ally_notFriend, out int allyCountCost)
        {
            RelationType toRelation = ally_notFriend ? RelationType.RelationType3_Ally : RelationType.RelationType2_Good;
            int diff = toRelation - relation; //1 or 2

            int cost = diff * 2 /*+ 1*/;
            allyCountCost = 0;

            if (ally_notFriend)
            {
                cost += 1;

                if (DssRef.difficulty.diplomacyDifficulty > 0)
                {
                    allyCountCost = (int)(player.allyCount * DssConst.DiplomacyExtraCostPerAlly);
                }
            }
            cost += allyCountCost;

            cost += toFaction.WorkForceInCityCount() / 3;
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

        public static int EndRelationCost(RelationType relation)
        {
            return DeclareWarCost(relation) -1;
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

        ExtendTruce = 100,
    }

    enum SpeakTerms
    {
        SpeakTerms1_Good = 1,
        SpeakTerms0_Normal = 0,
        SpeakTermsN1_Bad = -1,
        SpeakTermsN2_None = -2,
    }
}
