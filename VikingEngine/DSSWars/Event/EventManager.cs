using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Interface.CutScene;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Event
{
    class EventManager
    {

        public List<City> factories = new List<City>(3);

        Time dyingFactionsTimer = Time.Zero;

        Time toPeacefulCheckTimer = new Time(3, TimeUnit.Minutes);

        List<AbsStoryEvent> mainStory = new List<AbsStoryEvent>();

        public EventManager()
        {//eventTriggerGameTimeSec = DssRef.difficulty.aiDelayTimeSec;
        }

        virtual public void onGameStarted()
        { }

        public void onTutorialEnd()
        {
            if (PlatformSettings.STEAM_DEMO)
            {
                onDemoTimeUp();
            }
        }

        protected void onDemoTimeUp()
        {
            DssRef.state.LocalHost().hud.messages.Add(DssRef.lang.Demo_TimesUp_Title, DssRef.lang.Demo_EndInOneMinuteDescription);
            new Timer.TimedAction1ArgTrigger_InGame<GameEndReason>(viewEndScreen, GameEndReason.TimesUp, TimeExt.MinuteInSeconds * 1f);
        }
        protected void viewEndScreen(GameEndReason endReason)
        {
            new EndScene(endReason, false);
        }

        public AbsStoryEvent CurrentEvent()
        {
            return mainStory.FirstOrDefault();
        }

        public int StoryIndex()
        { 
            var ev = mainStory.FirstOrDefault();
            if (ev == null)
            {
                return EventsOrder.StoryEnd;
            }
            return ev.OrderIndex();
        }

        virtual public void asyncUpdate(float time)
        {
            if (DssRef.state.localPlayers[0].tutorial != null)// ||
                //!DssRef.difficulty.runEvents)
            {
                return;
            }

            var ev = mainStory.FirstOrDefault();
            if (ev != null)
            {
                if (ev.asyncUpdate(time))
                {
                    mainStory.RemoveAt(0);
                    if (mainStory.Count > 0)
                    {
                        mainStory.First().onStart();
                    }
                }
            }

            asyncUpdateDyingFactions(time);

            asyncUpdateTooPeaceful(time);

            asyncCheckPlayerDominance();
        }

        void asyncCheckPlayerDominance()
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                if (p.faction.cities.Count >= p.nextDominationSize)
                {
                    p.nextDominationSize = p.faction.cities.Count + DssConst.DominationSizeIncrease.GetRandom();
                    p.dominationEvents++;

                    collectAllianceAgainstPlayerDomination(p);
                }
            }
        }

        public bool RunAi()
        {
            var storyevent = mainStory.FirstOrDefault();
            if (storyevent != null)
            {
                return storyevent.RunAi();
            }

            return true;
        }

        public bool MayAttackPlayer()
        {
            var storyevent = mainStory.FirstOrDefault();
            if (storyevent != null)
            {
                return storyevent.MayAttackPlayer();
            }

            return true;
        }

        //public bool RunWarmanager()
        //{
        //    return nextEvent > EventType.WarmanagerDelay;
        //}

        public void onGameStart(bool newGame)
        {
            if (newGame)
            {
                if (DssRef.difficulty.setting_gameMode != GameModeMainType.Spectator)
                {
                    addStoryEvent(new List<AbsStoryEvent>
                    {
                        new StoryEvent_AiDelay(),
                        new StoryEvent_AiWarDelay(),
                        new StoryEvent_WarmanagerDelay(),
                    }, true);

                    if (DssRef.difficulty.runStory &&
                        PlatformSettings.STEAM_DEMO == false)
                    {
                        addStoryEvent(new List<AbsStoryEvent>
                        {
                            new StoryEvent_SouthShips(),
                            new StoryEvent_DarkLordWarning(),
                            new StoryEvent_DarkLord(),
                            new StoryEvent_Factories(),
                            //new StoryEvent_FactoriesDestroyed(),
                        }, false);
                    }
                }

                //Prepare secret alliances
                var DarkFollower = DssRef.world.factions.Array[DssRef.settings.Faction_DarkFollower];
                var SouthHara = DssRef.world.factions.Array[DssRef.settings.Faction_SouthHara];
                var UnitedKingdom = DssRef.world.factions.Array[DssRef.settings.Faction_UnitedKingdom];

                DssRef.diplomacy.SetRelationType(DarkFollower, SouthHara, RelationType.RelationType3_Ally).secret = true;
                DssRef.diplomacy.SetRelationType(DarkFollower, UnitedKingdom, RelationType.RelationType3_Ally).secret = true;
                DssRef.diplomacy.SetRelationType(UnitedKingdom, SouthHara, RelationType.RelationType3_Ally).secret = true;

                //Setup dying war
                dyingFactionsTimer = new Time(5, TimeUnit.Minutes);

                var monger = DssRef.world.factions.Array[DssRef.settings.Faction_DyingMonger];
                var hate = DssRef.world.factions.Array[DssRef.settings.Faction_DyingHate];
                var destru = DssRef.world.factions.Array[DssRef.settings.Faction_DyingDestru];

                DssRef.diplomacy.SetRelationType(monger, hate, RelationType.RelationTypeN4_TotalWar);
                DssRef.diplomacy.SetRelationType(monger, destru, RelationType.RelationTypeN4_TotalWar);
                DssRef.diplomacy.SetRelationType(hate, destru, RelationType.RelationTypeN4_TotalWar);
            }
        }

        public void addStoryEvent(List<AbsStoryEvent> events, bool replace)
        {
            if (replace)
            {
                mainStory = events;
                mainStory.First().onStart();
            }
            else
            {
                mainStory.AddRange(events);
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(Ref.TotalGameTimeSec);

            //w.Write((int)nextEvent);
            //w.Write((int)eventState);

            //prepareTime.writeGameState(w);
            //checkTime.writeGameState(w);
            //triggerTime.writeGameState(w);

            //triggerTimeSpan_Minutes.Write(w);
            //nextExpectedPlayerSize.Write(w);

            //IOLib.WriteObjectList(w, playerMostSouthCity);
            //IOLib.WriteBinaryList(w, spawnPos_Player);
            //IOLib.WriteObjectList(w, darkLordAvailableFactions);
            //IOLib.WriteObjectList(w, darkLordAllies);

            w.Write((byte)mainStory.Count);

            for (int i = 0; i < mainStory.Count; ++i)
            {
                var ev = mainStory[i];
                w.Write((byte)ev.StoryEventType());
                if (ev.HasSaveData())
                {
                    w.Write(true);
                    ev.writeGameState(w);
                }
                else
                {
                    w.Write(false);
                }
            }

            dyingFactionsTimer.write(w);

            
            
        }
        public void readGameState(System.IO.BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            //float eventPrepareTimeSec;
            //float eventCheckGameTimeSec;
            //float eventTriggerGameTimeSec;

            //if (subVersion >= 47)
            //{
                Ref.TotalGameTimeSec = r.ReadSingle();
            //}
            if (subVersion < 57)
            {
                //OLD (only preserved for demo)
                var nextEvent = (EventType)r.ReadInt32();
                var eventState = (EventState)r.ReadInt32();

                new TimeInGameCountdown().readGameState(r);
                new TimeInGameCountdown().readGameState(r);
                new TimeInGameCountdown().readGameState(r);


                new IntervalF().Read(r);
                new IntervalF().Read(r);

                var playerMostSouthCity = arraylib.ToArray_Safe(IOLib.ReadObjectList<City>(r));
                var spawnPos_Player = arraylib.ToArray_Safe(IOLib.ReadBinaryList<IntVector2>(r));
                var darkLordAvailableFactions = IOLib.ReadObjectList<Faction>(r);
                var darkLordAllies = IOLib.ReadObjectList<Faction>(r);

                mainStory.Clear();
            }
            else
            {
                //NEW
                mainStory.Clear();
                int mainStoryCount = r.ReadByte();
                for (int i = 0; i < mainStory.Count; ++i)
                {
                    var type = (EventType)r.ReadByte();
                    var ev = CreateEvent(type);
                    mainStory.Add(ev);
                    if (r.ReadBoolean())
                    {
                        ev.readGameState(r, subVersion, pointers);
                    }
                }
            }

            dyingFactionsTimer.read(r);
            dyingFactionsTimer.MilliSeconds = Bound.Min(dyingFactionsTimer.MilliSeconds, 1);

            //if (subVersion < 47)
            //{
            //    Ref.TotalGameTimeSec = r.ReadSingle();
            //}
        }

        //void prepareNext()
        //{
        //    eventState = 0;
        //    switch (nextEvent)
        //    {
        //        case EventType.AiWarDelay:
        //            {
        //                triggerTimeSpan_Minutes = IntervalF.NoInterval(15);
        //            }
        //            break;
        //        case EventType.WarmanagerDelay:
        //            {
        //                triggerTimeSpan_Minutes = IntervalF.NoInterval(20);
        //            }
        //            break;
        //        case EventType.SouthShips:
        //            {
        //                triggerTimeSpan_Minutes = new IntervalF(3.6f, 4.5f) * TimeExt.HourInMinutes;
        //                nextExpectedPlayerSize = new IntervalF(DssConst.HeadCityStartMaxWorkForce * 2f, DssConst.HeadCityStartMaxWorkForce * 4f);
        //            }
        //            break;
        //        case EventType.DarkLordWarning:
        //            {
        //                triggerTimeSpan_Minutes = new IntervalF(22f, 28f) * TimeExt.HourInMinutes;
        //                nextExpectedPlayerSize = new IntervalF(DssConst.HeadCityStartMaxWorkForce * 4f, DssConst.HeadCityStartMaxWorkForce * 8f);
        //            }
        //            break;
        //        case EventType.DarkLord:
        //            {
        //                triggerTimeSpan_Minutes = IntervalF.NoInterval(1f * TimeExt.HourInMinutes);
        //            }
        //            break;
        //    }

        //    prepareTime.zero();
        //    checkTime.start(TimeLength.FromMinutes(triggerTimeSpan_Minutes.Min)); 
        //}

        public void TestNextEvent()
        {
            var ev = mainStory.FirstOrDefault();
            if (ev != null)
            {
                DssRef.state.localPlayers[0].hud.messages.Add(
                        "Test event", ev.StoryEventType().ToString());
                ev.TriggerNow();
                //checkTime.start(1);
                //triggerTime.start(2);
                //triggerTimeSpan_Minutes = IntervalF.NoInterval(0.1f);
            }
        }


        void asyncUpdateDyingFactions(float time)
        { 
            if (dyingFactionsTimer.CountDown_IfActive(time))
            {
                var monger = DssRef.world.factions.Array[DssRef.settings.Faction_DyingMonger];
                var hate = DssRef.world.factions.Array[DssRef.settings.Faction_DyingHate];
                var destru = DssRef.world.factions.Array[DssRef.settings.Faction_DyingDestru];

                var factions =  new List<Faction>() 
                { 
                    monger, hate, destru,
                };

                foreach (var faction in factions)
                {
                    faction.growthMultiplier = 0.5f;
                    faction.addGold_factionWide( -10000);
                    //var citiesC = faction.cities.counter();
                    //while (citiesC.Next())
                    //{
                    //    citiesC.sel.gold = -2000;
                    //}
                    faction.hasDeserters = true;
                }
            }
        }

        void asyncUpdateTooPeaceful(float time)
        {
            var storyevent = mainStory.FirstOrDefault();
            if (storyevent == null || storyevent.RunWarManager())
            {
                if (toPeacefulCheckTimer.CountDown(time) &&
                    DssRef.difficulty.toPeacefulPercentage > 0)
                {
                    toPeacefulCheckTimer = new Time(30, TimeUnit.Minutes);

                    foreach (var p in DssRef.state.localPlayers)
                    {
                        p.toPeacefulCheck_asynch();
                    }
                }
            }
        }

        

        //void asyncPrepare(ref float time)
        //{
        //    switch (nextEvent)
        //    {
        //        case EventType.SouthShips:
        //            {
        //                int mostSouth = 0;
        //                foreach (var p in DssRef.state.localPlayers)
        //                {
        //                    if (p.faction.mainCity != null && p.faction.mainCity.tilePos.Y > mostSouth)
        //                    {
        //                        mostSouth = p.faction.mainCity.tilePos.Y;
        //                    }
        //                }

        //                int diff = DssRef.world.Size.Y - mostSouth;
        //                //Remove two seconds for each tile
        //                time -= diff * 2f;
        //            }
        //            break;

        //        case EventType.DarkLord:
        //            //Find a starting faction, a bit away from the player
        //            //darkLordAvailableFactions = new List<Faction>(32);

        //            Rectangle2 mapCenter = new Rectangle2(IntVector2.Zero, DssRef.world.Size);
        //            mapCenter.AddRadius(-mapCenter.Height / 8);

        //            List<Faction> perfectPosition = new List<Faction>();
        //            List<Faction> available = new List<Faction>();
        //            darkLordAllies = new List<Faction>(16);

        //            var factionC = DssRef.world.factions.counter();

        //            while (factionC.Next())
        //            {
        //                if (
        //                    (
        //                        factionC.sel.factiontype == FactionType.DefaultAi ||
        //                        factionC.sel.factiontype == FactionType.DarkFollower ||
        //                        factionC.sel.factiontype == FactionType.Barbarians ||
        //                        factionC.sel.factiontype == FactionType.SouthHara
        //                    ) &&
        //                    factionC.sel.cities.Count >= 2 &&
        //                    !DssRef.diplomacy.PositiveRelationWithPlayer(factionC.sel))
        //                {
        //                    available.Add(factionC.sel);

        //                    if (factionC.sel.cities.Count >= 4 &&
        //                        factionC.sel.mainCity != null &&
        //                        mapCenter.IntersectTilePoint(factionC.sel.mainCity.tilePos) &&
        //                        !factionC.sel.HasPlayerNeighbor())
        //                    {
        //                        perfectPosition.Add(factionC.sel);
        //                    }
        //                }

        //                if (DssRef.diplomacy.NegativeRelationWithPlayer(factionC.sel) ||
        //                    factionC.sel.diplomaticSide == DiplomaticSide.Dark)
        //                {
        //                    darkLordAllies.Add(factionC.sel);
        //                }
        //            }

        //            if (perfectPosition.Count > 0)
        //            {
        //                darkLordAvailableFactions = perfectPosition;
        //            }
        //            else
        //            {
        //                darkLordAvailableFactions = available;
        //            }

        //            break;

        //    }
        //}

        

        

        public void OnPlayerDeclareWar()
        {
            const int DelayReduceToSec = 10;

            var ev = mainStory.FirstOrDefault();
            if (ev != null)
            {
                if (ev.RunWarManager() == false)
                {
                    if (ev.triggerTime.length.seconds > DelayReduceToSec)
                    {
                        ev.triggerTime.start(DelayReduceToSec);
                    }
                }
            }

            
        }

        public void onFactoryBuilt(City city)
        {
            factories.Add(city);
        }

        public void onFactoryDestroyed(City city)
        {
            factories.Remove(city);
            if (factories.Count == 0)
            {
                DssRef.settings.darkLordPlayer.factoriesLeft = 0;

                addStoryEvent(new List<AbsStoryEvent>{
                    //new StoryEvent_FactoriesDestroyed(),
                    new StoryEvent_DarkLordInPerson(),
                    new StoryEvent_KillTheDarkLord() }, 
                    true );

                //nextEvent = EventType.DarkLordInPerson;
                //Ref.update.AddSyncAction(new SyncAction1Arg<EventType>(RunNextEvent_synced, nextEvent));
            }
        }

        public void onDarkLordSpawn()
        {
            var ev = mainStory.FirstOrDefault();
            if (ev != null)
            {
                if (ev.StoryEventType() != EventType.KillTheDarkLord)
                {
                    addStoryEvent(new List<AbsStoryEvent>{
                        new StoryEvent_KillTheDarkLord() 
                    },
                    true);
                }
            }

            //    if (nextEvent < EventType.KillTheDarkLord)
            //{
            //    nextEvent = EventType.KillTheDarkLord;

            //    foreach (var p in DssRef.state.localPlayers)
            //    {
            //        p.hud.messages.Add(DssRef.lang.EventMessage_FinalBattleTitle, DssRef.lang.EventMessage_FinalBattleText);
            //    }
            //}
        }
        public void onDarkLorDeath()
        {
            
            if (mainStory.Count > 0)
            {
                victory(true);
            }
        }

        public void onAllDarkCitiesDestroyed()
        {
            if (mainStory.Count > 0)
            {
                if (DssRef.settings.darkLordPlayer.darkLordUnit == null)
                {
                    DssRef.achieve.UnlockAchievement(AchievementIndex.no_darklord);
                }
                victory(true);
            }
        }

        public void onWorldDomination()
        {
            victory(false);
        }

        void victory(bool bossVictory)
        {
            if (mainStory.Count > 0)
            {
                mainStory.Clear();
                DssRef.achieve.onVictory();

                new EndScene( GameEndReason.Victory, bossVictory);
            }
        }

        public void onPlayerDeath()
        {
            if (DssRef.difficulty.setting_gameMode != GameModeMainType.Spectator)
            {
                foreach (var p in DssRef.state.localPlayers)
                {
                    if (p.faction.isAlive)
                    {
                        return;
                    }
                }

                new EndScene(GameEndReason.Defeat, false);
            }
        }

        public void collectAllianceAgainstPlayerDomination(LocalPlayer player)
        {
            var neighbor = findAttackingNeighborFaction(player.faction);

            if (neighbor == null)
            {
                return;
            }

            List<Faction> attackers = new List<Faction>() { neighbor };
            int totalSize = neighbor.totalWorkForce;
            List<Faction> search = adjacentFactions(neighbor);
            List<Faction> has_searched = new List<Faction>();

            int maxLoops = 100;
            while (--maxLoops > 0 && totalSize < player.faction.totalWorkForce * 1.5f)
            {
                if (search.Count > 0)
                {   
                    var faction = arraylib.RandomListMemberPop(search);
                    bool bHasSearched = has_searched.Contains(faction);

                    if (!bHasSearched &&
                        factionMayStartWar(faction, player.faction) &&
                        !attackers.Contains(faction))
                    {
                        attackers.Add(faction);
                        totalSize += faction.totalWorkForce;
                    }

                    if (!bHasSearched)
                    {
                        has_searched.Add(faction);
                    }
                }
                else
                {
                    foreach (var faction in has_searched)
                    {
                        search.AddRange(adjacentFactions(faction));
                    }
                }
            }

            Faction attackLeader = null;
            //Create an alliance
            foreach (var faction in attackers)
            {
                foreach (var other in attackers)
                {
                    if (other != faction)
                    { 
                        DssRef.diplomacy.SetRelationType(faction, other, RelationType.RelationType3_Ally);
                    }
                }

                DssRef.diplomacy.SetRelationType(faction, player.faction, RelationType.RelationTypeN1_Enemies);
                

                if (attackLeader == null || faction.militaryStrength > attackLeader.militaryStrength)
                { 
                    attackLeader = faction;
                }
            }

            //Prepare leader
            attackers.Remove(attackLeader); 
            attackers.Insert(0, attackLeader);
            DssRef.diplomacy.GetOrCreateRelation(attackLeader, player.faction).SpeakTerms = SpeakTerms.SpeakTermsN2_None;
            attackLeader.player.setAggression(AbsPlayer.AggressionLevel1_RevengeOnly);

            Ref.update.AddSyncAction(new SyncAction(() =>
            {
                var city = attackLeader.mainCity;

                var meleeProfile = new ConscriptProfile()
                {
                    weapon = Resource.ItemResourceType.Pike,
                    armorLevel = Resource.ItemResourceType.IronArmor,
                    training = TrainingLevel.Basic,
                    specialization = SpecializationType.Traditional,
                };
                var rangedProfile = new ConscriptProfile()
                {
                    weapon = player.dominationEvents < 3? Resource.ItemResourceType.Crossbow : Resource.ItemResourceType.HandCannon,
                    armorLevel = Resource.ItemResourceType.PaddedArmor,
                    training = TrainingLevel.Basic,
                    specialization = SpecializationType.Traditional,
                };

                city.conscriptArmy(meleeProfile, city.defaultConscriptPos(), 3 + player.dominationEvents * 2);
                city.conscriptArmy(rangedProfile, city.defaultConscriptPos(), 3 + player.dominationEvents * 2);

                if (player.dominationEvents >= 3)
                {
                    var cannonProfile = new ConscriptProfile()
                    {
                        weapon = Resource.ItemResourceType.ManCannonBronze,
                        armorLevel = Resource.ItemResourceType.PaddedArmor,
                        training = TrainingLevel.Basic,
                        specialization = SpecializationType.Siege,
                    };
                    city.conscriptArmy(cannonProfile, city.defaultConscriptPos(), 2 + player.dominationEvents);
                }

                player.hud.messages.Add(DssRef.lang.EventMessage_EnemyAlliance_Title, DssRef.lang.EventMessage_EnemyAlliance);
            }));
            
            new Timer.TimedAction2ArgTrigger_InGame<List<Faction>, LocalPlayer>((attackers, player) =>
            {
                attackers.First().player.setAggression(AbsPlayer.AggressionLevel3_FocusedAttacks);
                foreach (var faction in attackers)
                {
                    faction.player.setMinimumAggression(AbsPlayer.AggressionLevel2_RandomAttacks);
                    DssRef.diplomacy.SetRelationType(faction, player.faction, RelationType.RelationTypeN3_War);                    
                }
            }, attackers, player, TimeExt.MinuteInSeconds * DssConst.DominationWarTimeDelay_Minutes.GetRandom());


            List<Faction> adjacentFactions(Faction faction)
            {
                List<Faction> factions = new List<Faction>();
                var citiesC = faction.cities.counter();
                while (citiesC.Next())
                {
                    foreach (var n in citiesC.sel.neighborCities)
                    {
                        var ncity = DssRef.world.cities[n];
                        if (ncity.faction != faction &&
                            ncity.faction.player.IsAi() &&
                            !factions.Contains(ncity.faction))
                        { 
                            factions.Add(ncity.faction);
                        }
                    }
                }
                
                return factions;
            }
        }

        public Faction findAttackingNeighborFaction(Faction defender)
        {
            var cities = defender.cities.toList();

            while (cities.Count > 0)
            {
                var city = arraylib.RandomListMemberPop(cities);

                if (city != null)
                {
                    foreach (var cindex in city.neighborCities)
                    {
                        var otherfaction = DssRef.world.cities[cindex].faction;
                        if (factionMayStartWar(otherfaction, defender))
                        {
                            return otherfaction;
                        }
                    }
                }
            }
            return null;
        }

        bool factionMayStartWar(Faction attacker, Faction defender)
        {
            if ((attacker.factiontype == FactionType.DefaultAi || attacker.diplomaticSide == DiplomaticSide.Dark) &&
                attacker.armies.Count > 0)
            {
                var rel = DssRef.diplomacy.GetRelationType(defender, attacker);
                if (rel >= RelationType.RelationTypeN1_Enemies && rel <= RelationType.RelationType1_Peace)
                {
                    return true;
                } 
            }

            return false;
        }

        AbsStoryEvent CreateEvent(EventType type)
        {
            switch (type)
            {
                case EventType.AiDelay:
                    return new StoryEvent_AiDelay();
                case EventType.AiWarDelay:
                    return new StoryEvent_AiWarDelay();
                case EventType.WarmanagerDelay:
                    return new StoryEvent_WarmanagerDelay();
                case EventType.SouthShips:
                    return new StoryEvent_SouthShips();
                case EventType.DarkLordWarning:
                    return new StoryEvent_DarkLordWarning();
                case EventType.DarkLord:
                    return new StoryEvent_DarkLord();
                case EventType.Factories:
                    return new StoryEvent_Factories();
                case EventType.FactoriesDestroyed:
                    return new StoryEvent_FactoriesDestroyed();
                case EventType.DarkLordInPerson:
                    return new StoryEvent_DarkLordInPerson();
                case EventType.KillTheDarkLord:
                    return new StoryEvent_KillTheDarkLord();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Unhandled event type.");
            }
        }


    }

    //enum BossTimeSettings
    //{ 
    //    Immediate,
    //    Early,
    //    Normal,
    //    Late,
    //    VeryLate,
    //    //Never,
    //    NUM
    //}


}
