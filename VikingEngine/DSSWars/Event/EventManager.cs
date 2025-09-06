using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface.CutScene;
using VikingEngine.DSSWars.Players;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Event
{
    class EventManager
    {

        public List<City> factories = new List<City>(3);

        Time dyingFactionsTimer = Time.Zero;

        Time toPeacefulCheckTimer = new Time(Ref.rnd.Float(20, 40), TimeUnit.Minutes);

        ConcurrentQueue<AbsStoryEvent> mainStory = new ConcurrentQueue<AbsStoryEvent>();

        public int maxWars = 0;

        public EventManager()
        {
        }

        virtual public void onGameStarted()
        { }

        public void onTutorialEnd()
        {
            if (PlatformSettings.STEAM_DEMO &&
                !DssRef.state.LocalHost().profile.casualControls)
            {
                onDemoTimeUp();
            }
        }

        protected void onDemoTimeUp()
        {
            DssRef.state.LocalHost().hud.messages.Add(DssRef.lang.Demo_TimesUp_Title, DssRef.lang.Demo_EndInOneMinuteDescription);
            new Timer.TimedAction2ArgTrigger_InGame<GameEndReason, VictoryType>(triggerGameEnd, GameEndReason.TimesUp, VictoryType.None, TimeExt.MinuteInSeconds * 1f);
        }
        //protected void viewEndScreen(GameEndReason endReason)
        //{
        //    new EndScene(endReason, VictoryType.None);
        //}

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
                    mainStory.TryDequeue(out _);// .RemoveAt(0);
                    if (mainStory.TryPeek(out var next))
                    {
                        next.onStart();
                    }
                }
            }

            asyncUpdateDyingFactions(time);

            asyncUpdateTooPeaceful(time);

            //asyncCheckPlayerDominance();

            if (Ref.peRnd.ChanceF(0.1f))
            {
                asyncCheckVictory();
            }
        }

        void asyncCheckVictory()
        {
            int dominationCount = DssRef.storage.gameRuleset.mapSize > MapSize.Small ? 5 : 3;
            
            foreach (var p in DssRef.state.localPlayers)
            {
                int hillFriends = 0;
                int allyCount = 0;
                bool worldPeace = true;

                var relations = p.faction.diplomaticRelations;

                for (int relIx = 0; relIx < relations.Length; ++relIx)
                {
                    if (relIx != p.faction.myIndex)
                    {
                        Faction otherFaction = DssRef.world.factions.GetIndex_Safe(relIx);

                        if (otherFaction != null && otherFaction.isAlive)
                        {
                            RelationType relation = RelationType.RelationType0_Neutral;
                            SpeakTerms speak = SpeakTerms.SpeakTerms0_Normal;

                            if (relations[relIx] != null)
                            {
                                relation = relations[relIx].Relation;
                                speak = relations[relIx].SpeakTerms;
                            }

                            if (relation >= RelationType.RelationType3_Ally)
                            {
                                allyCount++;
                                if (otherFaction.factiontype == FactionType.BramblebrookHill ||
                                    otherFaction.factiontype == FactionType.Tumblehill)
                                {
                                    hillFriends++;
                                }
                            }

                            if (relation < RelationType.RelationType1_Peace && speak != SpeakTerms.SpeakTermsN2_None)
                            {
                                worldPeace = false;
                                //break;
                            }
                        }
                    }
                }

                if (allyCount > p.previousAllyCount)
                { 
                    p.previousAllyCount = allyCount;
                    DssRef.achieve.onAllyCount(allyCount);
                }
                if (hillFriends >= 2)
                {
                    DssRef.achieve.UnlockAchievement_async(AchievementIndex.worthy_friends);
                }

                if (worldPeace)
                {
                    Ref.update.AddSyncAction(new SyncAction1Arg<VictoryType>(victory, VictoryType.WorldPeace));
                    return;
                }


                bool domination = true;

                int missingCities = 0;
                foreach (var city in DssRef.world.cities)
                {
                    if (city.factionIndex != p.faction.myIndex)
                    {
                        missingCities++;
                        if (missingCities > dominationCount)
                        { 
                            domination = false;
                            break;
                        }
                    }
                }
                if (domination)
                {
                    Ref.update.AddSyncAction(new SyncAction1Arg<VictoryType>(victory, VictoryType.Domination));
                    return;
                }

            }            
            
        }

        //void asyncCheckPlayerDominance()
        //{
        //    foreach (var p in DssRef.state.localPlayers)
        //    {
        //        if (p.faction.cities.Count >= p.nextDominationSize)
        //        {
        //            //p.nextDominationSize = p.faction.cities.Count + DssConst.DominationSizeIncrease.GetRandom();
        //            p.cohalitionEvent = true;

        //            collectAllianceAgainstPlayerDomination(p);
        //        }
        //    }
        //}

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

                    if (DssRef.difficulty.runStory &&
                        PlatformSettings.STEAM_DEMO == false)
                    {
                        addStoryEvent(new List<AbsStoryEvent>
                        {
                            new StoryEvent_AiDelay(),
                            new StoryEvent_AiWarDelay(),
                            new StoryEvent_FirstAttack(),
                            new StoryEvent_WarmanagerDelay(),
                            new StoryEvent_Barbarians(),
                            new StoryEvent_Mercenaries(),
                            new StoryEvent_Cohalition(),
                            new StoryEvent_DarkLordWarning(),
                            new StoryEvent_DarkLord(),
                            new StoryEvent_DefeatTheBoss(),
                        }, true);
                    }
                    else
                    {
                        addStoryEvent(new List<AbsStoryEvent>
                        {
                            new StoryEvent_AiDelay(),
                            new StoryEvent_AiWarDelay(),
                            new StoryEvent_WarmanagerDelay(),
                        }, true);
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
                mainStory.Clear();
            }

            foreach (var m in events)
            {
                mainStory.Enqueue(m);
            }
            
            if (replace)
            {

                if (mainStory.TryPeek(out var first))
                {
                    first.onStart();
                }
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

            var storyArray = mainStory.ToArray();
            w.Write((byte)mainStory.Count);

            for (int i = 0; i < storyArray.Length; ++i)
            {
                var ev = storyArray[i];
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
            w.Write((ushort)maxWars);
            
            
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
                for (int i = 0; i < mainStoryCount; ++i)
                {
                    var type = (EventType)r.ReadByte();
                    var ev = CreateEvent(type);
                    mainStory.Enqueue(ev);
                    if (r.ReadBoolean())
                    {
                        ev.readGameState(r, subVersion, pointers);
                    }
                }
            }

            dyingFactionsTimer.read(r);
            dyingFactionsTimer.MilliSeconds = Bound.Min(dyingFactionsTimer.MilliSeconds, 1);

            if (subVersion >= 72)
            { 
                maxWars = r.ReadUInt16();
            }
        }

        public void onBattleEnd_async(AbsArmy army, InBattleWith inBattleWith)
        {
            if (army.GetPlayer().IsLocalPlayer() &&
                inBattleWith.ContainsFaction(DssRef.settings.Faction_Barbarian) && 
                Bound.IsWithin(StoryIndex(), EventsOrder.Barbarians, EventsOrder.Barbarians +1))
            {
                army.GetPlayer().GetLocalPlayer().barbarianKiller = true;
            }
        }

        public void onFactionDestroyed(Faction faction)
        {

            //Happens in one second update
            if (faction.myIndex == DssRef.settings.Faction_Barbarian)
            {
                foreach (var p in DssRef.state.localPlayers)
                {
                    if (p.barbarianKiller)
                    {
                        p.barbarianKiller = false;

                        IntVector2 onTile = p.faction.mainCity.ArmySpawnTilePos();
                        var mainArmy = p.faction.NewArmy(onTile);
                        {
                            SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                            {
                                conscript = new ConscriptProfile()
                                {
                                    weapon = Resource.ItemResourceType.KnightsLance,
                                    armorLevel = Resource.ItemResourceType.FullPlateArmor,
                                    training = TrainingLevel.Champion,
                                    specialization = SpecializationType.Traditional,
                                }
                            };

                            for (int i = 0; i < 4; ++i)
                            {
                                new SoldierGroup(mainArmy, SoldierProfile, mainArmy.position);
                            }
                        }
                        mainArmy.tagBack = CityTagBack.Blue;
                        mainArmy.tagArt = ArmyTagArt.LevelMaster;
                        mainArmy.setAsStartArmy();

                        p.hud.messages.Add(DssRef.todoLang.EventMessage_DarkHordeKiller_Title, DssRef.todoLang.EventMessage_DarkHordeKiller_Message);

                        DssRef.achieve.UnlockAchievement_onAny_100(AchievementIndex.barbarian_bane_any, AchievementIndex.barbarian_bane_100);
                    }


                }
            }
            else if (faction == DssRef.settings.darkLordPlayer.faction)
            {
                victory(VictoryType.DefeatBoss);
            }
            else if (faction.myIndex == DssRef.settings.Faction_UnitedKingdom)
            {
                if (IsStoryBeforeBoss() && DssRef.diplomacy.InWarWithPlayer(faction))
                {
                    DssRef.achieve.UnlockAchievement_onAny_100(AchievementIndex.early_uk_any, AchievementIndex.early_uk_100);
                }
            }
            else if (faction.myIndex == DssRef.settings.Faction_DarkFollower)
            {
                if (IsStoryBeforeBoss() && DssRef.diplomacy.InWarWithPlayer(faction))
                {
                    DssRef.achieve.UnlockAchievement_onAny_100(AchievementIndex.early_dread_any, AchievementIndex.early_dread_100);
                }
            }

            foreach (var p in DssRef.state.localPlayers)
            {
                if (DssRef.diplomacy.InWar(faction, p.faction))
                {
                    var citiesC = p.faction.cities.counter();
                    while (citiesC.Next())
                    {
                        if (citiesC.sel.previousOwner == faction.myIndex && 
                            (citiesC.sel.myIndex == faction.lostCity_Time0 || citiesC.sel.myIndex == faction.lostCity_Time1))
                        { //Credited with killing off the faction
                            p.factionsTerminated++;

                            if (DssRef.difficulty.setting_gameMode == GameModeMainType.FullStory)
                            {
                                switch (p.factionsTerminated)
                                {
                                    case 0:
                                        DssRef.achieve.UnlockAchievement_on75(AchievementIndex.purge_nation_tier1);
                                        break;
                                    case 4:
                                        DssRef.achieve.UnlockAchievement_on75(AchievementIndex.purge_nation_tier2);
                                        break;
                                    case 12:
                                        DssRef.achieve.UnlockAchievement_on75(AchievementIndex.purge_nation_tier3);
                                        break;

                                }
                            }

                            if (p.firstAttacker == faction.myIndex)
                            { 
                                DssRef.achieve.UnlockAchievement_onAny_100(AchievementIndex.destroy_first_attacker_any, AchievementIndex.destroy_first_attacker_100);
                            }
                        }
                    }
                }
            }

                
            
        }

        bool IsStoryBeforeBoss()
        {
            if (DssRef.difficulty.setting_gameMode == GameModeMainType.FullStory)
            {
                var current = CurrentEvent();
                if (current != null && current.OrderIndex() < EventsOrder.DarkLord)
                {
                    return true;
                }
            }
            return false;
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
                    toPeacefulCheckTimer = new Time(Ref.rnd.Float(1.5f, 3f), TimeUnit.Hours);

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

        

        

       

        //public void onFactoryBuilt(City city)
        //{
        //    factories.Add(city);
        //}

        //public void onFactoryDestroyed(City city)
        //{
        //    factories.Remove(city);
        //    if (factories.Count == 0)
        //    {
        //        DssRef.settings.darkLordPlayer.factoriesLeft = 0;

        //        addStoryEvent(new List<AbsStoryEvent>{
        //            //new StoryEvent_FactoriesDestroyed(),
        //            new StoryEvent_DarkLordInPerson(),
        //            new StoryEvent_KillTheDarkLord() }, 
        //            true );

        //        //nextEvent = EventType.DarkLordInPerson;
        //        //Ref.update.AddSyncAction(new SyncAction1Arg<EventType>(RunNextEvent_synced, nextEvent));
        //    }
        //}

        //public void onDarkLordSpawn()
        //{
        //    var ev = mainStory.FirstOrDefault();
        //    if (ev != null)
        //    {
        //        if (ev.StoryEventType() != EventType.KillTheDarkLord)
        //        {
        //            addStoryEvent(new List<AbsStoryEvent>{
        //                new StoryEvent_KillTheDarkLord() 
        //            },
        //            true);
        //        }
        //    }

        //    //    if (nextEvent < EventType.KillTheDarkLord)
        //    //{
        //    //    nextEvent = EventType.KillTheDarkLord;

        //    //    foreach (var p in DssRef.state.localPlayers)
        //    //    {
        //    //        p.hud.messages.Add(DssRef.lang.EventMessage_FinalBattleTitle, DssRef.lang.EventMessage_FinalBattleText);
        //    //    }
        //    //}
        //}
        //public void onDarkLorDeath()
        //{

        //    if (mainStory.Count > 0)
        //    {
        //        victory(true);
        //    }
        //}
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
                        var otherfaction = DssRef.world.cities[cindex].GetFaction();
                        if (factionMayStartWar(otherfaction, defender))
                        {
                            return otherfaction;
                        }
                    }
                }
            }
            return null;
        }

        public Faction findAttackingNeighborFaction_keepExpanding(Faction defender)
        {
            //var cities = defender.cities.toList();
            bool[] factionsChecked = new bool[DssRef.world.factions.Array.Length];
            List<Faction> factionsToCheck = new List<Faction>(8);
            factionsToCheck.Add(defender);
            factionsChecked[defender.myIndex] = true;

            while (factionsToCheck.Count > 0)
            {
                int checkIx = Math.Min(Ref.rnd.Int(factionsToCheck.Count), Ref.rnd.Int(factionsToCheck.Count));
                Faction check = arraylib.Pull(factionsToCheck, checkIx);

                var cities = check.cities.toList();

                while (cities.Count > 0)
                {
                    var city = arraylib.RandomListMemberPop(cities);

                    if (city != null)
                    {
                        foreach (var cindex in city.neighborCities)
                        {
                            var otherfaction = DssRef.world.cities[cindex].GetFaction();
                            if (otherfaction.myIndex != city.factionIndex &&
                                !factionsChecked[otherfaction.myIndex])
                            {
                                if (factionMayStartWar(otherfaction, defender))
                                {
                                    return otherfaction;
                                }
                                else
                                {
                                    factionsToCheck.Add(otherfaction);
                                    factionsChecked[otherfaction.myIndex] = true;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public bool factionMayStartWar(Faction attacker, Faction defender)
        {
            if ((attacker.factiontype == FactionType.DefaultAi || attacker.diplomaticSide == DiplomaticSide.Dark) &&
                (attacker.diplomaticSide != DiplomaticSide.Light || defender.diplomaticSide == DiplomaticSide.Dark) &&
                attacker.armies.Count > 0)
            {
                if (defender.player.IsLocalPlayer())
                {
                    if (attacker.myIndex == DssRef.settings.Faction_DarkFollower)
                    { return false; }

                    if (attacker.militaryStrength < Math.Min(defender.militaryStrength * 0.25f, 6) ||
                        attacker.militaryStrength > defender.militaryStrength * 3f)
                    {
                        return false;
                    }
                }

                var rel = DssRef.diplomacy.GetRelationType(defender, attacker);
                if (rel >= RelationType.RelationTypeN1_Enemies && rel <= RelationType.RelationType1_Peace)
                {
                    return true;
                }
            }

            return false;
        }

        public void onAllDarkCitiesDestroyed()
        {
            //if (mainStory.Count > 0)
            //{
                //if (DssRef.settings.darkLordPlayer.darkLordUnit == null)
                //{
                //    DssRef.achieve.UnlockAchievement(AchievementIndex.no_darklord);
                //}
                victory(VictoryType.DefeatBoss);
            //}
        }

        //public void onWorldDomination()
        //{
        //    victory( VictoryType.Domination);
        //}

        void victory(VictoryType vType)
        {
            if (mainStory.Count > 0)
            {
                mainStory.Clear();
                

                triggerGameEnd(GameEndReason.Victory, vType);
            }
        }

        public void onPlayerEnterWar(Players.LocalPlayer player, Faction other, bool isAggressor)
        {
            if (isAggressor)
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

                ++player.statistics.WarsStartedByYou;
            }
            else
            {
                ++player.statistics.WarsStartedByEnemy;

                if (player.firstAttacker != ushort.MaxValue)
                { 
                    player.firstAttacker = other.myIndex;
                }
            }

            Task.Run(() =>
            {
                int wars = player.faction.CountWars();
                maxWars = Math.Max(maxWars, wars);
            });
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

                //new EndScene(GameEndReason.Defeat,  VictoryType.None);
                triggerGameEnd(GameEndReason.Defeat,  VictoryType.None);
            }
        }

        protected void triggerGameEnd(GameEndReason endReason, VictoryType vType)
        {
            new EndScene(endReason, vType);

            if (!PlatformSettings.STEAM_DEMO &&
                (endReason == GameEndReason.Victory || DssRef.time.TotalIngameTime().TotalHours > 10))
            {
                if (!DssRef.storage.metaProgression.unlockedDangerousSettings)
                {
                    DssRef.storage.metaProgression.unlockedDangerousSettings = true;
                   
                }
            }

            if (endReason == GameEndReason.Victory)
            {
                DssRef.achieve.onVictory(vType);

                int difficulty = Convert.ToInt32(DssRef.difficulty.TotalDifficulty() * 100);
                switch (vType)
                {
                    case VictoryType.DefeatBoss:
                        DssRef.storage.metaProgression.Act1_Victory_Boss.addVictory(difficulty);
                        break;
                    case VictoryType.Domination:
                        DssRef.storage.metaProgression.Act1_Victory_Domination.addVictory(difficulty);
                        break;
                    case VictoryType.WorldPeace:
                        DssRef.storage.metaProgression.Act1_Victory_WorldPeace.addVictory(difficulty);
                        break;

                }
            }

            DssRef.storage.Save(null);
        }

       

        AbsStoryEvent CreateEvent(EventType type)
        {
            switch (type)
            {
                case EventType.AiDelay:
                    return new StoryEvent_AiDelay();
                case EventType.AiWarDelay:
                    return new StoryEvent_AiWarDelay();
                case EventType.FirstAttack:
                    return new StoryEvent_FirstAttack();
                case EventType.WarmanagerDelay:
                    return new StoryEvent_WarmanagerDelay();
                case EventType.Barbarians:
                    return new StoryEvent_Barbarians();
                case EventType.Mercenaries:
                    return new StoryEvent_Mercenaries();
                case EventType.Cohalition:
                    return new StoryEvent_Cohalition();
                case EventType.BossWarning:
                    return new StoryEvent_DarkLordWarning();
                case EventType.Boss:
                    return new StoryEvent_DarkLord();
                case EventType.DefeatTheBoss:
                    return new StoryEvent_DefeatTheBoss();
                

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Unhandled event type.");
            }
        }


    }


    enum VictoryType
    { 
        None = 0,
        DefeatBoss,
        WorldPeace,
        Domination,
        DarkSide,
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
