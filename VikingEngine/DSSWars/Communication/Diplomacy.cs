using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Players;
using VikingEngine.ToGG.MoonFall;
using static Sentry.MeasurementUnit;

namespace VikingEngine.DSSWars
{
    

    class Diplomacy
    {
        public const float MiltitaryStrengthXServant = 2f;

        int factionCapacity;
        public DiplomaticRelation[] diplomaticRelations;
        DiplomaticRelation empty = DiplomaticRelation.Empty;
        int[] indexRegister;

        List<PFaction> aiPlayerAsynchUpdate_wars1 = new List<PFaction>(8);
        List<PFaction> aiPlayerAsynchUpdate_wars2 = new List<PFaction>(8);
        List<PFaction> aiPlayerAsynchUpdate_threats = new List<PFaction>(8);
        List<PFaction> aiPlayerAsynchUpdate_wars_withplayer = new List<PFaction>(2);
        public List<PFaction> aiPlayerAsynchUpdate_collectAlliances = new List<PFaction>(8);

        public int ServantMaxCities = 2;
        public int DefaultMaxDiplomacy = 4;
        public double DefaultDiplomacyPerSecond/* = 1.0 / 240.0*/;
        public double EmbassyAddDiplomacy = 1.0 / 480.0;
        public double EmbassyAddMaxDiplomacy = 0.25;


        public double AddDiplomacy_AfterSoftlock_PerSecond = 1 / 120.0;
        public double Diplomacy_HardMax_Add = 5;

        public double SpeakTermsOnWar_BadChance;
        public double SpeakTermsOnWar_NoneChance;

        public double SpeakTermsOnNeigbor_BadChance; //todo not in war with neighbor
        public double SpeakTermsOnNeigbor_NoneChance;

        public Diplomacy(int factionCapacity = 64)
        {
            this.factionCapacity = factionCapacity;
            diplomaticRelations = new DiplomaticRelation[length()];
            initRegister(factionCapacity - 1);

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

                default:
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

        private void initRegister(int length)
        {
            indexRegister = new int[length];

            int nextLength = length;
            int currentIndex = 0;

            for (int i = 0; i < length; i++)
            {
                indexRegister[i] = currentIndex;
                currentIndex += nextLength;
                nextLength--;
            }
        }

        int length()
        {
            return MathExt.GaussSum(factionCapacity - 1);
        }

        //public bool tryGetRelationIndex(int faction1, int faction2)

        public bool RelationIndex(int faction1, int faction2, out int result)
        {
            int lowIndex, highIndex;
            if (faction1 < faction2)
            {
                lowIndex = faction1;
                highIndex = faction2;
            }
            else if (faction2 < faction1)
            {
                highIndex = faction1;
                lowIndex = faction2;
            }
            else
            {
                result = -1;
                return false;
            }

#if DEBUG
            if (!arraylib.InBound(indexRegister, lowIndex))
            {
                //throw new Exception();
                arraylib.InBound(indexRegister, lowIndex);
            }
#endif

            result = indexRegister[lowIndex] + highIndex - lowIndex;

#if DEBUG
            if (result < 0 || result >= diplomaticRelations.Length)
            {
                throw new Exception();
            }
#endif

            return true;
        }

//        public DiplomaticRelation GetRelation(Faction faction1, Faction faction2)
//        {
//            if (faction1 == null || faction2 == null || faction1 == faction2)
//            {
//                return DiplomaticRelation.Empty;
//            }
//#if DEBUG
//            //if (arraylib.InBound(diplomaticRelations, RelationIndex(faction1.myIndex, faction2.myIndex)) == false)
//            //{
//            //    lib.DoNothing();
//            //}
//#endif
//            if (RelationIndex(faction1.myIndex, faction2.myIndex, out int relIndex))
//            {
//                return diplomaticRelations[relIndex];
//            }
//            else
//            {
//                return DiplomaticRelation.Empty;
//            }
//        }

        //public DiplomaticRelation GetRelation_Safe(int faction1, int faction2)
        //{
        //    if (faction1 < 0 || faction1 >= DssRef.world.factions.Array.Length || 
        //        faction2 < 0 || faction2 >= DssRef.world.factions.Array.Length ||
        //        faction1 == faction2)
        //    {
        //        return DiplomaticRelation.Empty;
        //    }
        //    //return diplomaticRelations[RelationIndex(faction1, faction2)];

        //    if (RelationIndex(faction1, faction2, out int relIndex))
        //    {
        //        return diplomaticRelations[relIndex];
        //    }
        //    else
        //    {
        //        return DiplomaticRelation.Empty;
        //    }
        //}

        public DiplomaticRelation GetRelation(PFaction pfaction1, PFaction pfaction2) //int faction1, int faction2)
        {
            if (pfaction1.factionIndex < 0 || pfaction2.factionIndex < 0 || pfaction1 == pfaction2)
            {
                return DiplomaticRelation.Empty;
            }
            //return diplomaticRelations[RelationIndex(faction1, faction2)];
            if (RelationIndex(pfaction1.factionIndex, pfaction2.factionIndex, out int relIndex))
            {
                return diplomaticRelations[relIndex];
            }
            else
            {
                return DiplomaticRelation.Empty;
            }
        }

        public void Set(PFaction pfaction1, PFaction pfaction2, DiplomaticRelation relation)
        {
            if (RelationIndex(pfaction1.factionIndex, pfaction2.factionIndex, out int relIndex))
            {
                diplomaticRelations[relIndex] = relation;
            }
        }

        public ref DiplomaticRelation GetRefRelation(PFaction pfaction1, PFaction pfaction2)
        {
            RelationIndex(pfaction1.factionIndex, pfaction2.factionIndex, out int relIndex);
            return ref diplomaticRelations[relIndex];
        }

        public ref DiplomaticRelation GetRefRelation_Safe(PFaction pfaction1, PFaction pfaction2)
        {
            if (pfaction1.factionIndex < 0 || pfaction2.factionIndex < 0 || pfaction1 == pfaction2 ||
                !RelationIndex(pfaction1.factionIndex, pfaction2.factionIndex, out int relIndex))
            {
                return ref empty;
            }
            return ref diplomaticRelations[relIndex];
        }

        public void writeRelations(System.IO.BinaryWriter w)
        {            
            w.Write((ushort)indexRegister.Length);
            
            for (int currentIndex = 0; currentIndex < diplomaticRelations.Length; ++currentIndex)
            {
                if (diplomaticRelations[currentIndex].HasValue())
                {
                    w.Write(currentIndex);
                    diplomaticRelations[currentIndex].write(w);
                }
            }
            w.Write(int.MaxValue);
            
            Debug.WriteCheck(w);
        }


        public void readRelations(System.IO.BinaryReader r, int subVersion)
        {            
            int indexRegisterLength = r.ReadUInt16();
            initRegister(indexRegisterLength);            

            while (true)
            {
                int currentIndex = r.ReadInt32();

                if (currentIndex < diplomaticRelations.Length)
                {                    
                    diplomaticRelations[currentIndex].read(r, subVersion);
                }
                else
                {
#if DEBUG
                    if (currentIndex != int.MaxValue)
                    {
                        throw new Exception();
                    }
#endif
                    break;
                }
            }
            Debug.ReadCheck(r);
        }

        public void netWriteRelations(System.IO.BinaryWriter w)
        {
            w.Write((ushort)indexRegister.Length);

            for (int currentIndex = 0; currentIndex < diplomaticRelations.Length; ++currentIndex)
            {
                if (diplomaticRelations[currentIndex].Relation != RelationType.RelationType0_Neutral)
                {
                    w.Write(currentIndex);
                    diplomaticRelations[currentIndex].writeRelation(w);
                }
            }
            w.Write(int.MaxValue);

            Debug.WriteCheck(w);
        }


        public void netReadRelations(System.IO.BinaryReader r, int subVersion)
        {
            int indexRegisterLength = r.ReadUInt16();
            initRegister(indexRegisterLength);

            while (true)
            {
                int currentIndex = r.ReadInt32();

                if (currentIndex < diplomaticRelations.Length)
                {
                    diplomaticRelations[currentIndex].readRelation(r);
                }
                else
                {
#if DEBUG
                    if (currentIndex != int.MaxValue)
                    {
                        throw new Exception();
                    }
#endif
                    break;
                }
            }
            Debug.ReadCheck(r);
        }

        public void async_update()
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                RelationsLoop loop = new RelationsLoop(p.pfaction);
                while (loop.Next())
                {
                   
                    diplomaticRelations[loop.RelationIndex()].truce_update();
                }
            }

        }
    

        public bool InplayerAlliance(PFaction aifaction)
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                if (GetRelation(p.pfaction, aifaction).Relation >= RelationType.RelationType3_Ally)
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

        public List<PFaction> aiPlayerAsynchUpdate_collectWars(Faction aifaction)
        {
            aiPlayerAsynchUpdate_wars1.Clear();
            aiPlayerAsynchUpdate_wars_withplayer.Clear();
            
            RelationsLoop loop = new RelationsLoop(aifaction.pfaction);
            while (loop.Next())
            {
                if (loop.Relation().InWar())
                {
                    if (loop.OtherFaction(out var other) && other.player.IsLocalPlayer())
                    {
                        aiPlayerAsynchUpdate_wars_withplayer.Add(loop.OtherFaction_P());
                    }
                    aiPlayerAsynchUpdate_wars1.Add(loop.OtherFaction_P());
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
                return aiPlayerAsynchUpdate_wars1;
            }
        }

        public List<PFaction> aiPlayerAsynchUpdate_collectThreats(Faction aifaction, float threatFactor = 1.6f)
        { 
            aiPlayerAsynchUpdate_threats.Clear();

            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref aifaction.cities, DssRef.world.cities, out City city))
            {
                EcsStaticArrayCounter neighbors = city.CityNeighbors();
                while (neighbors.Next(DssRef.world.cities, out City nCity))
                {
                    if (nCity.pfaction.factionIndex != aifaction.myIndex &&
                        !aiPlayerAsynchUpdate_threats.Contains(nCity.pfaction))
                    {
                        aiPlayerAsynchUpdate_threats.Add(nCity.pfaction);
                    }
                }
            }

            for (int i = aiPlayerAsynchUpdate_threats.Count - 1; i >= 0; i--)
            {
                var otherFaction = aiPlayerAsynchUpdate_threats[i];
                if (GetRelation(aifaction.pfaction, otherFaction).Relation >= RelationType.RelationType2_Good ||
                    (otherFaction.TryGetFaction(out var of) && aifaction.MyPlusAllianceStrengthValue() * threatFactor >= of.MyPlusAllianceStrengthValue()))
                {
                    aiPlayerAsynchUpdate_threats.RemoveAt(i);
                }
            }

            return aiPlayerAsynchUpdate_threats;
        }

        public List<PFaction> aiPlayerAsynchUpdate_GetAllied(PFaction aifaction)
        {
            aiPlayerAsynchUpdate_collectAlliances.Clear();

            RelationsLoop loop = new RelationsLoop(aifaction);
            while (loop.Next())
            {
                if (loop.Relation().Relation >= RelationType.RelationType3_Ally)
                {
                    aiPlayerAsynchUpdate_collectAlliances.Add(loop.OtherFaction_P());
                }
            }

            return aiPlayerAsynchUpdate_collectAlliances;
        }

        public bool nextAlly(Faction faction, ref RelationsLoop loop, out Faction ally)
        {
            while (loop.Next())
            {
                if (loop.Relation().Relation >= RelationType.RelationType3_Ally)
                {
                    loop.OtherFaction(out ally);    
                    return true;
                }
            }
            ally = null;
            return false;
        }

        public bool aiPlayerAsynchUpdate_mayAlly_checkConflict(PFaction faction1, PFaction faction2, PFaction enemyFaction, bool tryEndOtherWars)
        {
            List<PFaction> allies = aiPlayerAsynchUpdate_GetAllied(faction1);

            foreach (PFaction ally in allies)
            {
                //var ally = DssRef.world.faction(fIx);
                //if (ally != null)
                //{
                    if (GetRelation(ally, faction2).Relation <= RelationType.RelationTypeN3_Mobilization)
                    {
                        return false;
                    }
                //}
            }

            collectWars(faction1, aiPlayerAsynchUpdate_wars1);
            collectWars(faction2, aiPlayerAsynchUpdate_wars2);

            foreach (PFaction war in aiPlayerAsynchUpdate_wars1)
            {
                //Dont get dragged into more wars
                if (war != enemyFaction && !aiPlayerAsynchUpdate_wars2.Contains(war))
                {
                    if (tryEndOtherWars)
                    {
                        faction1.GetPlayer()?.GetAiPlayer()?.tryEndBotWars(aiPlayerAsynchUpdate_wars1);
                       
                        faction2.GetPlayer()?.GetAiPlayer()?.tryEndBotWars(aiPlayerAsynchUpdate_wars2);
                    }

                    return false;
                }
            }

            return true;
        }

        public void collectWars(PFaction aifaction, List<PFaction> wars)
        {
            wars.Clear();

            RelationsLoop loop = new RelationsLoop(aifaction);
            while (loop.Next())
            {
                if (loop.Relation().InWar())
                {
                    wars.Add(loop.OtherFaction_P());
                }
            }
        }

        public bool InWarWithPlayer(PFaction faction)
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                if (GetRelation(p.pfaction, faction).InWar())
                { 
                    return true;
                }
            }
            return false;
        }


        public void SetRelationType(PFaction faction1, PFaction faction2, PFaction actuator, RelationType? newRelation, float? relationSecondsLength = null, SpeakTerms? speakTerms = null, bool secret = false)
        {
            if (/*faction1 != null && faction2 != null && */faction1 != faction2)
            {
                ref var relation = ref GetRefRelation_Safe(faction1, faction2);
                
                if (speakTerms.HasValue)
                {
                    relation.SpeakTerms = speakTerms.Value;
                }
                if (relationSecondsLength.HasValue)
                {
                    relation.RelationEnd_GameTimeSec.setTimeFromNow(relationSecondsLength.Value);
                }
                relation.secret = secret;

                if (newRelation.HasValue)
                {
                    relation.SetRelation(faction1, faction2, newRelation.Value, actuator, out RelationType previous);
                }
            }
        }

        public void SetDefaultSpeakTerms(PFaction faction, SpeakTerms speakTerms)
        {
            for (int i = 0; i < DssRef.world.factions.Array.Length; i++)
            {
                if (i != faction.factionIndex)
                {
                    ref var relation = ref GetRefRelation(faction, new PFaction(i));
                    relation.SpeakTerms = speakTerms;
                }
            }
        }

        //DiplomaticRelation NewRelation(Faction faction1, Faction faction2, RelationType newRelation)
        //{
        //    if (faction1 != faction2)
        //    {
        //        DiplomaticRelation rel;
        //        SpeakTerms speakterms = (SpeakTerms)Math.Min((int)faction1.DefaultSpeakingTerms(), (int)faction2.DefaultSpeakingTerms());
        //        rel = new DiplomaticRelation(faction1.myIndex, faction2.myIndex, newRelation, speakterms);

        //        faction1.player.onNewRelation(faction2, rel, RelationType.RelationType0_Neutral);
        //        faction2.player.onNewRelation(faction1, rel, RelationType.RelationType0_Neutral);
        //        return rel;
        //    }
        //    return null;    
        //}

        public bool botMayStartWar(Faction attacker, Faction defender)
        {
            if (attacker != null && 
                defender != null &&
                attacker.armies.Count > 0 &&
                attacker != defender &&
                attacker.player.IsBot())
            {
                var rel = GetRelation(defender.pfaction, attacker.pfaction);
                if (rel.InWar())
                {
                    return true;
                }

                bool mayAttackPlayer = !DssRef.difficulty.peaceful && DssRef.state.events.MayAttackPlayer() && attacker.player.mayAttackPlayer;


                if (!mayAttackPlayer &&
                    (defender.player.IsLocalPlayer() || DssRef.world.diplomacy.InplayerAlliance(defender.pfaction)))
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
                    
                if (rel.Relation >= RelationType.RelationTypeN1_Enemies && rel.Relation < RelationType.RelationType1_Peace)
                {
                    return true;
                }
                else if (rel.Relation == RelationType.RelationType1_Peace ||
                    rel.Relation == RelationType.RelationType2_Good)
                {
                    var relation = GetRelation(defender.pfaction, attacker.pfaction);
                    if (relation.RelationEnd_GameTimeSec.HasTime())
                    {
                        return false;
                    }
                    return Ref.peRnd.Chance(0.05);
                }


            }
            return false;
        }

        public void endRelations(PFaction actingFaction, PFaction otherFaction)
        {
            //if (actingFaction != null && otherFaction != null)
            {
                ref DiplomaticRelation relation = ref GetRefRelation(actingFaction, otherFaction);
                
                if (relation.Relation > RelationType.RelationType0_Neutral)
                {
                    relation.SetRelation(actingFaction, otherFaction, RelationType.RelationType0_Neutral, actingFaction, out RelationType prev);
                    
                    if (actingFaction.TryGetPlayer(out var player) && player.IsLocalPlayer())
                    {
                        int cost = EndRelationCost(prev);
                        player.GetLocalPlayer().diplomaticPoints.pay(cost, true);
                    }
                }
            }
        }

        public void declareWar(PFaction attacker, PFaction defender)
        {
            //if (attacker != null && 
            //    defender != null &&
            //    attacker.player != null &&
            //    defender.player != null &&
            if (attacker != defender &&
                !GetRelation(attacker, defender).InWar() &&
                attacker.TryGetPlayer(out var aPlayer) &&
                defender.TryGetPlayer(out var dPlayer))
            {
                ref var relation = ref GetRefRelation(attacker, defender);
                relation.SetRelation(attacker, defender, RelationType.RelationTypeN4_War, attacker, out RelationType prevRelation);
                

                if (aPlayer.IsLocalPlayer())
                {
                    int cost = DeclareWarCost(prevRelation);
                    var player = aPlayer.GetLocalPlayer();

                    player.diplomaticPoints.pay(cost, true);
                    DssRef.state.events?.onPlayerEnterWar(player, defender.GetFaction(), true);

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

                    if (dPlayer.IsLocalPlayer())
                    {
                        var otherPlayer = dPlayer.GetLocalPlayer();
                        var PtoP = player.GetOrCreateToPlayerDiplomacy(otherPlayer);//toPlayerDiplomacies[otherPlayer.playerData.localPlayerIndex];
                        PtoP.suggestingNewRelation = false;
                    }
                }
                if (dPlayer.IsLocalPlayer())
                {
                    var player = dPlayer.GetLocalPlayer();
                    DssRef.state.events?.onPlayerEnterWar(player, attacker.GetFaction(), false);
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
                if (GetRelation(faction.pfaction, p.pfaction).Relation >= minRelation)
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
                if (GetRelation(faction.pfaction, p.pfaction).Relation <= RelationType.RelationTypeN1_Enemies)
                {
                    return true;
                }
            }

            return false;
        }

        public void onFactionDeath(PFaction faction)
        {
            Task.Run(() =>
            {
                try
                {
                    RelationsLoop loop = new RelationsLoop(faction);
                    while (loop.Next())
                    {
                        diplomaticRelations[loop.RelationIndex()].OnDeath();
                    }

                }
                catch (Exception ex) 
                {
                    BlueScreen.ThreadException = ex;
                }

            });
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

            int cost = diff * 2;
            allyCountCost = 0;

            if (ally_notFriend)
            {
                cost += 1;

                if (DssRef.difficulty.diplomacyDifficulty > 0)
                {
                    allyCountCost = (int)(player.alliedFactions.Count * DssConst.DiplomacyExtraCostPerAlly);
                }
            }
            cost += allyCountCost;

            cost += toFaction.WorkForceInCityCount() / 3; //WorkForceInCityCount = totalWorkForce / DssConst.HeadCityStartMaxWorkForce;
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
            return relation <= RelationType.RelationTypeN3_Mobilization;
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
        RelationTypeN3_Mobilization = -3,
        RelationTypeN4_War = -4,
        RelationTypeN5_TotalWar = -5,

        NONE = -100,
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
