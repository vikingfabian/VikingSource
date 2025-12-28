using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.Conscript;

namespace VikingEngine.DSSWars.Event
{
    

    //WAR MANAGER
    partial class EventManager
    {
        
        public void testToPeacefulCheck()
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                p.testToPeacefulCheck();
            }
        }

        void asyncUpdateTooPeaceful(float time)
        {
            if (DssRef.difficulty.toPeacefulPercentage > 0)
            {
                var storyevent = mainStory.FirstOrDefault();
                if (storyevent == null || storyevent.RunWarManager())
                {
                    foreach (var p in DssRef.state.localPlayers)
                    {
                        p.asyncUpdateTooPeaceful(time);
                    }
                }
            }
        }
    }
}

namespace VikingEngine.DSSWars.Players
{
    struct WarManagerGear
    {
        public const int StartGear = 1;
        public const int MaxGear = 3;

        public int gear;
        public int maxCityCount;
        public IntervalF checkTimeHours;
        public float tooPeacefulPercentageMulti;
        public float allyChance;
        public Range maxPeacefulChecks;

        public WarManagerGear(int gear)
        {
#if DEBUG
            if (!Bound.IsWithin(gear, 1, MaxGear))
            {
                throw new ArgumentException();
            }
#endif 

            gear = Bound.Set(gear, 1, MaxGear);
            this.gear = gear;
                        
            
            PcgRandom random = new PcgRandom(DssRef.world.metaData.seed + gear * 11);

            switch (gear)
            {
                case StartGear:
                    maxCityCount = random.Int(7, 9);
                    checkTimeHours = new IntervalF(0.8f, 4f);
                    tooPeacefulPercentageMulti = 1f;
                    allyChance = 0.05f;

                    maxPeacefulChecks = new Range(1, 4);
                    break;

                case 2:
                    maxCityCount = random.Int(16, 21);
                    checkTimeHours = new IntervalF(0.6f, 3.5f);
                    tooPeacefulPercentageMulti = 1.4f;
                    allyChance = 0.25f;

                    maxPeacefulChecks = new Range(2, 6);
                    break;

                case MaxGear:
                    maxCityCount = int.MaxValue;
                    checkTimeHours = new IntervalF(0.5f, 3f);
                    tooPeacefulPercentageMulti = 2f;
                    allyChance = 0.75f;

                    maxPeacefulChecks = new Range(8, 16);
                    break;

            }

            if (DssRef.difficulty.aiAggressivity <= AiAggressivity.Low)
            {
                maxCityCount += 5;
            }
            else if (DssRef.difficulty.aiAggressivity >= AiAggressivity.High)
            {
                maxCityCount -= 2;
                maxPeacefulChecks += 4;

                if (DssRef.difficulty.extremeAggression)
                {
                    allyChance += 0.1f;
                    maxPeacefulChecks += 2;
                    checkTimeHours.Max *= 0.75f;
                }
            }
            //}
            //else
            //{
            //    throw new ArgumentOutOfRangeException("WarManagerGear " + gear);
            //}

        }
    }

    partial class LocalPlayer
    {
        WarManagerGear warManagerGear;
        Time tooPeacefulCheckTimer =
//#if DEBUG
            //new Time(4, TimeUnit.Seconds);
//#else
            new Time(Ref.rnd.Float(20, 40), TimeUnit.Minutes);
//#endif
        public void testToPeacefulCheck()
        {
            tooPeacefulCheckTimer.setZero();
        }

        public void asyncUpdateTooPeaceful(float time)
        {
            if (tooPeacefulCheckTimer.CountDown(time))
            {
                if (faction.cities.Count > warManagerGear.maxCityCount)
                {
                    warManagerGear = new WarManagerGear(warManagerGear.gear + 1);
                }

                tooPeacefulCheckTimer = new Time(warManagerGear.checkTimeHours.GetRandom(), TimeUnit.Hours);

                toPeacefulCheck_asynch();
                
            }
        }

        public void toPeacefulCheck_asynch()
        {
            if (faction.totalWorkForce > 0)
            {
                int warCount = 0;
                float opposingSize = 0;

                RelationsLoop loop = new RelationsLoop(faction.myIndex);
                while (loop.Next())
                {
                //    for (int relIx = 0; relIx < faction.diplomaticRelations.Length; ++relIx)
                //{
                //    if (faction.diplomaticRelations[relIx] != null &&
                //        faction.diplomaticRelations[relIx].Relation <= RelationType.RelationTypeN2_Truce)
                //    {
                    if (loop.Relation().Relation <= RelationType.RelationTypeN2_Truce &&
                        loop.OtherFaction(out var opponent) &&
                        opponent.player.IsBot())
                    {
                        //var opponent = faction.diplomaticRelations[relIx].opponent(faction);
                        //if (opponent.player.IsBot())
                        //{
                            ++warCount;
                            opposingSize += opponent.PotensialMilitaryStrength();
                        //}
                    }
                }

                bool toPeaceful = true;
                int maxChecks = warManagerGear.maxPeacefulChecks.GetRandom();//Ref.rnd.Int(1, 5);

                //Span<bool> containsBarrack = stackalloc bool[ConscriptDataLib.BarrackTypes.Length];
                int attackersCount = 0;
                Span<int> attackers = stackalloc int[maxChecks];

                while (toPeaceful && maxChecks > 0)
                {
                    maxChecks--;

                    if (opposingSize > 0)
                    {
                        opposingSizePerc = opposingSize / faction.PotensialMilitaryStrength();

                        toPeaceful = opposingSizePerc <= DssRef.difficulty.toPeacefulPercentage * warManagerGear.tooPeacefulPercentageMulti;
                    }
                    else
                    {
                        opposingSizePerc = 0;
                    }

                    if (toPeaceful)
                    {
                        //start a war
                        var attacker = DssRef.state.events.findAttackingNeighborFaction(faction);

                        if (attacker == null && Ref.rnd.Chance(0.6))
                        {
                            attacker = DssRef.state.events.findAttackingNeighborFaction_keepExpanding(faction);

                            //See if can gank any of the players friendlies, since they are not neihbor to the player
                            var friend = DssRef.state.events.findFriendsToDefender(attacker, this.faction);
                            if (friend != null)
                            {
                                DssRef.diplomacy.declareWar(attacker, friend);
                            }
                        }

                        if (attacker != null)
                        {
                            opposingSize += attacker.PotensialMilitaryStrength();

                            attacker.player.setMinimumAggression(AbsPlayer.AggressionLevel2_RandomAttacks);
                            DssRef.diplomacy.declareWar(attacker, faction);

                            attackers[attackersCount] = attacker.myIndex;
                            attackersCount++;
                        }

                    }
                    else
                    {
                        break;
                    }
                }

                if (attackersCount >= 2 && Ref.rnd.Chance(warManagerGear.allyChance))
                {
                    Faction firstAttacker = DssRef.world.faction(attackers[0]);

                    if (firstAttacker != null )
                    {
                        //Try ally the attackers
                        for (int otherIx = 1; otherIx < attackersCount; otherIx++)
                        {
                            var otherFaction = DssRef.world.faction(attackers[otherIx]);
                            var relation = DssRef.diplomacy.GetRelation(firstAttacker, otherFaction).Relation;

                            if (relation <= RelationType.RelationTypeN3_War)
                            {
                                //Try declare peace
                                if (relation > RelationType.RelationTypeN4_TotalWar)
                                {
                                    firstAttacker.player.GetAiPlayer().botToBotPeaceDeclaration(null, otherFaction);
                                }
                            }
                            else if (relation < RelationType.RelationType3_Ally)
                            { 
                                //Try ally
                                firstAttacker.player.GetAiPlayer().botToBotAllyDeclaration(this.faction, otherFaction, true);
                            }
                        }
                    }
                }
            }
        }
    }
}
