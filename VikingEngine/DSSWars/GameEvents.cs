using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.CutScene;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars
{
    class GameEvents
    {
        public EventType nextEvent = 0;
        EventState eventState;
        TimeInGameCountdown prepareTime;
        TimeInGameCountdown checkTime;
        TimeInGameCountdown triggerTime;


        IntervalF triggerTimeSpan_Minutes;
        IntervalF nextExpectedPlayerSize;

        City[] playerMostSouthCity;
        IntVector2[] spawnPos_Player;

        List<Faction> darkLordAvailableFactions = null;
        List<Faction> darkLordAllies = null;

        public List<City> factories = new List<City>(3);

        Time dyingFactionsTimer = Time.Zero;

        Time toPeacefulCheckTimer = new Time(3, TimeUnit.Minutes);

        public GameEvents()
        {//eventTriggerGameTimeSec = DssRef.difficulty.aiDelayTimeSec;
        }

        public void asyncUpdate(float time)
        {
            if (DssRef.state.localPlayers[0].tutorial != null ||
                !DssRef.difficulty.runEvents)
            {
                return;
            }

            if (eventState == EventState.Done)
            {
                ++nextEvent;
                eventState = EventState.Prepare;
            }

            if (eventState == EventState.Prepare)
            {
                prepareNext();
                eventState = EventState.Countdown;
            }

            bool timedEvent;

            switch (nextEvent)
            {
                case EventType.AiDelay:
                case EventType.AiWarDelay:
                case EventType.WarmanagerDelay:
                case EventType.SouthShips:
                case EventType.DarkLordWarning:
                case EventType.DarkLord:
                    timedEvent = true;
                    break;

                default:
                    timedEvent = false;
                    break;
            }

            if (timedEvent)
            {
                if (eventState == EventState.Countdown)
                {
                    if (checkTime.TimeOut())
                    {
#if DEBUG
                        Ref.update.AddSyncAction(new SyncAction(() =>
                        {
                            DssRef.state.localPlayers[0].hud.messages.Add(
                                "Event Power check", (nextEvent).ToString());
                        }));
#endif

                        PowerCheck();
                        eventState = EventState.PowerChecked;
                    }
                }
                else if (eventState == EventState.PowerChecked)
                {
                    if (triggerTime.TimeOut())
                    {
#if DEBUG
                        Ref.update.AddSyncAction(new SyncAction(() =>
                        {
                            DssRef.state.localPlayers[0].hud.messages.Add(
                                "Event Trigger", (nextEvent).ToString());
                        }));
                        
#endif
                        calcAndRunEvent();
                        eventState = EventState.Done;
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

        public bool AiDelay()
        { 
            return nextEvent <= EventType.AiDelay;
        }

        public bool MayAttackPlayer()
        { 
            return nextEvent > EventType.AiWarDelay;
        }

        //public bool RunWarmanager()
        //{
        //    return nextEvent > EventType.WarmanagerDelay;
        //}

        public void onGameStart(bool newGame)
        {
            if (newGame)
            {

                eventState = EventState.PowerChecked;
                triggerTime.start(DssRef.difficulty.aiDelayTimeSec);
                //if (DssRef.difficulty.runEvents)
                //{
                //    prepareNext();
                //}

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

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(Ref.TotalGameTimeSec);

            w.Write((int)nextEvent);
            w.Write((int)eventState);
            //w.Write(eventPrepareTimeSec);
            //w.Write(eventCheckGameTimeSec);
            //w.Write(eventTriggerGameTimeSec);
            prepareTime.writeGameState(w);
            checkTime.writeGameState(w);
            triggerTime.writeGameState(w);

            triggerTimeSpan_Minutes.Write(w);
            nextExpectedPlayerSize.Write(w);

            IOLib.WriteObjectList(w, playerMostSouthCity);
            IOLib.WriteBinaryList(w, spawnPos_Player);
            IOLib.WriteObjectList(w, darkLordAvailableFactions);
            IOLib.WriteObjectList(w, darkLordAllies);

            dyingFactionsTimer.write(w);
            
        }
        public void readGameState(System.IO.BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            //float eventPrepareTimeSec;
            //float eventCheckGameTimeSec;
            //float eventTriggerGameTimeSec;

            if (subVersion >= 47)
            {
                Ref.TotalGameTimeSec = r.ReadSingle();
            }

            nextEvent = (EventType)r.ReadInt32();
            eventState = (EventState)r.ReadInt32();
           
            prepareTime.readGameState(r);
            checkTime.readGameState(r);
            triggerTime.readGameState(r);
            

            triggerTimeSpan_Minutes.Read(r);
            nextExpectedPlayerSize.Read(r);

            playerMostSouthCity = arraylib.ToArray_Safe(IOLib.ReadObjectList<City>(r));
            spawnPos_Player = arraylib.ToArray_Safe(IOLib.ReadBinaryList<IntVector2>(r));
            darkLordAvailableFactions = IOLib.ReadObjectList<Faction>(r);
            darkLordAllies = IOLib.ReadObjectList<Faction>(r);

            dyingFactionsTimer.read(r);
            dyingFactionsTimer.MilliSeconds = Bound.Min(dyingFactionsTimer.MilliSeconds, 1);

            if (subVersion < 47)
            {
                Ref.TotalGameTimeSec = r.ReadSingle();
            }
        }

        void prepareNext()
        {
            eventState = 0;
            switch (nextEvent)
            {
                case EventType.AiWarDelay:
                    {
                        triggerTimeSpan_Minutes = IntervalF.NoInterval(15);
                    }
                    break;
                case EventType.WarmanagerDelay:
                    {
                        triggerTimeSpan_Minutes = IntervalF.NoInterval(20);
                    }
                    break;
                case EventType.SouthShips:
                    {
                        triggerTimeSpan_Minutes = new IntervalF(3.6f, 4.5f) * TimeExt.HourInMinutes;
                        nextExpectedPlayerSize = new IntervalF(DssConst.HeadCityStartMaxWorkForce * 2f, DssConst.HeadCityStartMaxWorkForce * 4f);
                    }
                    break;
                case EventType.DarkLordWarning:
                    {
                        triggerTimeSpan_Minutes = new IntervalF(22f, 28f) * TimeExt.HourInMinutes;
                        nextExpectedPlayerSize = new IntervalF(DssConst.HeadCityStartMaxWorkForce * 4f, DssConst.HeadCityStartMaxWorkForce * 8f);
                    }
                    break;
                case EventType.DarkLord:
                    {
                        triggerTimeSpan_Minutes = IntervalF.NoInterval(1f * TimeExt.HourInMinutes);
                    }
                    break;
            }

            prepareTime.zero();
            checkTime.start(TimeLength.FromMinutes(triggerTimeSpan_Minutes.Min)); 
        }

        public void TestNextEvent()
        {
            //if (DssRef.state.events.AiDelay())
            //{
            //    DssRef.state.events.AiDelay() = false;
            //    DssRef.state.localPlayers[0].hud.messages.Add(
            //            "Test event", "Removed AI delay");
            //}
            //else
            //{
            DssRef.state.localPlayers[0].hud.messages.Add(
                    "Test event", (nextEvent).ToString());
            //checkTime.zero();
            //eventState = EventState.Done;

            //asyncUpdate(0);
            checkTime.start(1);
            triggerTime.start(2);
            triggerTimeSpan_Minutes = IntervalF.NoInterval(0.1f);


            //if (nextEvent <= EventType.DarkLord)
            //{
            //    PowerCheck();
            //    calcAndRunEvent();
            //}
            //else
            //{
            //    if (nextEvent == EventType.KillTheDarkLord)
            //    {
            //        victory(true);
            //    }
            //}


            //}
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
                    faction.addMoney_factionWide( -10000);
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
            if (toPeacefulCheckTimer.CountDown(time) &&
                nextEvent > EventType.WarmanagerDelay &&
                DssRef.difficulty.toPeacefulPercentage > 0)
            {
                toPeacefulCheckTimer = new Time(30, TimeUnit.Minutes);

                foreach (var p in DssRef.state.localPlayers)
                {
                    p.toPeacefulCheck_asynch();
                }
            }
        }

        void PowerCheck()
        {
            float time;

            if (triggerTimeSpan_Minutes.Length == 0)
            {
                time = triggerTimeSpan_Minutes.Min;                
            }
            else
            {
                //Set a time depending on the player's strength
                int totalWorkForce = 0;
                foreach (var p in DssRef.state.localPlayers)
                {
                    var citiesC = p.faction.cities.counter();
                    while (citiesC.Next())
                    {
                        totalWorkForce += citiesC.sel.workForceMax;
                    }
                }

               
                if (totalWorkForce < nextExpectedPlayerSize.Min)
                {
                    time = triggerTimeSpan_Minutes.Max;
                }
                else if (totalWorkForce >= nextExpectedPlayerSize.Max)
                {
                    time = triggerTimeSpan_Minutes.Min;
                }
                else
                {
                    time = triggerTimeSpan_Minutes.Center;
                }
            }

            time *= TimeExt.MinuteInSeconds;

            time += time * Ref.rnd.Plus_MinusF(0.2f);

            asyncPrepare(ref time);

            triggerTime.start(time);//eventTriggerGameTimeSec = time + eventPrepareTimeSec;
        }

        void asyncPrepare(ref float time)
        {
            switch (nextEvent)
            {
                case EventType.SouthShips:
                    {
                        int mostSouth = 0;
                        foreach (var p in DssRef.state.localPlayers)
                        {
                            if (p.faction.mainCity != null && p.faction.mainCity.tilePos.Y > mostSouth)
                            {
                                mostSouth = p.faction.mainCity.tilePos.Y;
                            }
                        }

                        int diff = DssRef.world.Size.Y - mostSouth;
                        //Remove two seconds for each tile
                        time -= diff * 2f;
                    }
                    break;

                case EventType.DarkLord:
                    //Find a starting faction, a bit away from the player
                    //darkLordAvailableFactions = new List<Faction>(32);

                    Rectangle2 mapCenter = new Rectangle2(IntVector2.Zero, DssRef.world.Size);
                    mapCenter.AddRadius(-mapCenter.Height / 8);

                    List<Faction> perfectPosition = new List<Faction>();
                    List<Faction> available = new List<Faction>();
                    darkLordAllies = new List<Faction>(16);

                    var factionC = DssRef.world.factions.counter();

                    while (factionC.Next())
                    {
                        if (
                            (
                                factionC.sel.factiontype == FactionType.DefaultAi ||
                                factionC.sel.factiontype == FactionType.DarkFollower ||
                                factionC.sel.factiontype == FactionType.SouthHara
                            ) &&
                            factionC.sel.cities.Count >= 2 &&
                            !DssRef.diplomacy.PositiveRelationWithPlayer(factionC.sel))
                        {
                            available.Add(factionC.sel);

                            if (factionC.sel.cities.Count >= 4 &&
                                factionC.sel.mainCity != null &&
                                mapCenter.IntersectTilePoint(factionC.sel.mainCity.tilePos) &&
                                !factionC.sel.HasPlayerNeighbor())
                            {
                                perfectPosition.Add(factionC.sel);
                            }
                        }

                        if (DssRef.diplomacy.NegativeRelationWithPlayer(factionC.sel) ||
                            factionC.sel.diplomaticSide == DiplomaticSide.Dark)
                        {
                            darkLordAllies.Add(factionC.sel);
                        }
                    }

                    if (perfectPosition.Count > 0)
                    {
                        darkLordAvailableFactions = perfectPosition;
                    }
                    else
                    {
                        darkLordAvailableFactions = available;
                    }

                    break;

            }
        }

        void calcAndRunEvent()
        { 
            switch (nextEvent)
            {
                case EventType.SouthShips:
                    {
                        calcSouthSpawn();
                    }
                    break;
            }

            Ref.update.AddSyncAction(new SyncAction1Arg<EventType>(RunNextEvent_synced, nextEvent));
        }

        void RunNextEvent_synced(EventType nextEvent)
        {
            //Is synced
            switch (nextEvent)
            {
                case EventType.SouthShips:
                    {
                        var enemyFac = DssRef.world.factions.Array[DssRef.settings.Faction_SouthHara];

                        for (int playerIx = 0; playerIx < DssRef.state.localPlayers.Count; ++playerIx)
                        {
                            if (playerMostSouthCity[playerIx] != null)
                            {
                                IntVector2 spawn = spawnPos_Player[playerIx];
                                Rotation1D enemyRot = Rotation1D.D0;

                                Range soldierCount = Range.Zero;

                                switch (DssRef.difficulty.bossSize)
                                {
                                    case BossSize.Small:
                                        soldierCount = new Range(10, 14);
                                        break;
                                    case BossSize.Medium:
                                        soldierCount = new Range(14, 18);
                                        break;
                                    case BossSize.Large:
                                        soldierCount = new Range(18, 23);
                                        break;
                                    case BossSize.Huge:
                                        soldierCount = new Range(24, 30);
                                        break;
                                }

                                var army = enemyFac.NewArmy(VectorExt.AddY(spawn, 0));
                                army.rotation = enemyRot;
                                int count = soldierCount.GetRandom() / DssRef.state.localPlayers.Count;
                                for (int i = 0; i < count; ++i)
                                {
                                    new SoldierGroup(army, DssLib.SoldierProfile_Pikeman, army.position);
                                }
                                count = soldierCount.GetRandom() / DssRef.state.localPlayers.Count;
                                for (int i = 0; i < count; ++i)
                                {
                                    new SoldierGroup(army, DssLib.SoldierProfile_Sailor, army.position);
                                }
                                count = soldierCount.GetRandom() / DssRef.state.localPlayers.Count;
                                for (int i = 0; i < count; ++i)
                                {
                                    new SoldierGroup(army, DssLib.SoldierProfile_CrossbowMan, army.position);
                                }
                                army.startInOnePoint();//refreshPositions(true);


                                //var groupsC = army.groups.counter();
                                //while (groupsC.Next())
                                //{
                                //    groupsC.sel.completeTransform(SoldierTransformType.ToShip);
                                //}

                                DssRef.diplomacy.declareWar(enemyFac, DssRef.state.localPlayers[playerIx].faction);
                                army.Order_MoveTo(VectorExt.AddY(playerMostSouthCity[playerIx].tilePos, 3));
                            }
                        }

                        enemyFac.player.GetAiPlayer().nextDecisionTimer.MilliSeconds = float.MaxValue;
                        new SouthHaraStartAi(enemyFac);

                        playerMostSouthCity = null;
                        spawnPos_Player = null;

                    }
                    break;
                case EventType.DarkLordWarning:
                    {
                        foreach (var p in DssRef.state.localPlayers)
                        {
                            p.hud.messages.Add(DssRef.lang.EventMessage_ProphesyTitle, DssRef.lang.EventMessage_ProphesyText);
                        }
                    }
                    break;
                case EventType.DarkLord:
                    {
                        if (arraylib.HasMembers(darkLordAvailableFactions))
                        {
                            DssRef.settings.darkLordPlayer.EnterMap(arraylib.RandomListMember(darkLordAvailableFactions), darkLordAllies);

                            darkLordAllies = null;
                            darkLordAvailableFactions = null;

                            var greenwood = DssRef.world.factions[DssRef.settings.Faction_GreenWood];

                            foreach (var p in DssRef.state.localPlayers)
                            {
                                p.hud.messages.Add(DssRef.lang.EventMessage_FinalBossEnterTitle, DssRef.lang.EventMessage_FinalBossEnterText);

                                if (!DssRef.diplomacy.InWar(p.faction, greenwood))
                                {
                                    DssRef.diplomacy.GetOrCreateRelation(p.faction, greenwood).SpeakTerms = SpeakTerms.SpeakTerms1_Good;
                                }
                            }
                        }
                    }
                    break;
                    //case EventType.DarkLordInPerson:
                    //    {

                    //    }
                    //    break;
            }

            //++nextEvent;
            //prepareNext();
        }

        private void calcSouthSpawn()
        {
            List<IntVector2> usedTiles = new List<IntVector2>();
            IntVector2[] checkPos = new IntVector2[2];

            playerMostSouthCity = new City[DssRef.state.localPlayers.Count];
            spawnPos_Player = new IntVector2[DssRef.state.localPlayers.Count];

            for (int playerIx = 0; playerIx < DssRef.state.localPlayers.Count; ++playerIx)
            {
                City mostSouth = null;
                var citiesC = DssRef.state.localPlayers[playerIx].faction.cities.counter();
                while (citiesC.Next())
                {
                    if (mostSouth == null || citiesC.sel.tilePos.Y > mostSouth.tilePos.Y)
                    {
                        mostSouth = citiesC.sel;
                    }
                }

                if (mostSouth != null)
                {

                    playerMostSouthCity[playerIx] = mostSouth;

                    //Find spawn
                    int maxLoops = 10000;
                    IntVector2 left = mostSouth.tilePos;
                    left.Y = DssRef.world.Size.Y - 2;
                    IntVector2 right = mostSouth.tilePos;
                    right.Y = DssRef.world.Size.Y - 2;

                    bool foundSpawn = false;

                    while (--maxLoops >= 0 && !foundSpawn)
                    {
                        left.X--;
                        if (left.X <= 2)
                        {
                            left.X = mostSouth.tilePos.X;
                            left.Y--;
                        }
                        right.X++;
                        if (right.X >= DssRef.world.Size.X - 3)
                        {
                            right.X = mostSouth.tilePos.X;
                            right.Y--;
                        }

                        checkPos[0] = left;
                        checkPos[1] = right;

                        foreach (var pos in checkPos)
                        {
                            if (DssRef.world.tileGrid.TryGet(pos, out Map.Tile tile))
                            {
                                if (tile.IsWater())
                                {
                                    bool available = true;

                                    foreach (var used in usedTiles)
                                    {
                                        if (used.SideLength(pos) <= 4)
                                        {
                                            available = false;
                                            break;
                                        }
                                    }

                                    if (available)
                                    {
                                        foundSpawn = true;
                                        spawnPos_Player[playerIx] = pos;
                                        usedTiles.Add(pos);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnPlayerDeclareWar()
        {
            const int DelayReduceToSec = 10;
            if (nextEvent <= EventType.WarmanagerDelay)
            {
                if (triggerTime.length.seconds > DelayReduceToSec)
                {
                    triggerTime.start(DelayReduceToSec);
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
                nextEvent = EventType.DarkLordInPerson;
                Ref.update.AddSyncAction(new SyncAction1Arg<EventType>(RunNextEvent_synced, nextEvent));
            }
        }

        public void onDarkLordSpawn()
        {
            if (nextEvent < EventType.KillTheDarkLord)
            {
                nextEvent = EventType.KillTheDarkLord;

                foreach (var p in DssRef.state.localPlayers)
                {
                    p.hud.messages.Add(DssRef.lang.EventMessage_FinalBattleTitle, DssRef.lang.EventMessage_FinalBattleText);
                }
            }
        }
        public void onDarkLorDeath()
        {
            if (nextEvent != EventType.End)
            {
                victory(true);
            }
        }

        public void onAllDarkCitiesDestroyed()
        {
            if (nextEvent != EventType.End)
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
            if (nextEvent < EventType.End)
            {
                nextEvent = EventType.End;
                DssRef.achieve.onVictory();
                //DssRef.state.localPlayers[0].menuSystem.victoryScreen();

                new EndScene(true, bossVictory);
            }
        }

        public void onPlayerDeath()
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                if (p.faction.isAlive)
                {
                    return;
                }
            }

            new EndScene(false, false);
        }

        public void collectAllianceAgainstPlayerDomination(LocalPlayer player)
        {
            var neighbor = findAttackingNeighborFaction(player.faction);

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
                    weapon = (player.dominationEvents < 3? Resource.ItemResourceType.Crossbow : Resource.ItemResourceType.HandCannon),
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

                player.hud.messages.Add(DssRef.todoLang.EventMessage_EnemyAlliance_Title, DssRef.todoLang.EventMessage_EnemyAlliance);
            }));
            
            new Timer.TimedAction2ArgTrigger_InGame<List<Faction>, LocalPlayer>((List<Faction> attackers, LocalPlayer player) =>
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
                            //var rel = DssRef.diplomacy.GetRelationType(faction, otherfaction);
                            //if (rel >= RelationType.RelationTypeN1_Enemies && rel <= RelationType.RelationType1_Peace)
                            //{
                            //    otherfaction.player.setMinimumAggression(AiPlayer.AggressionLevel2_RandomAttacks);
                                //var aiPlayer = otherfaction.player.GetAiPlayer();
                                //if (aiPlayer.aggressionLevel <= AiPlayer.AggressionLevel1_RevengeOnly)
                                //{
                                //    aiPlayer.aggressionLevel = AiPlayer.AggressionLevel2_RandomAttacks;
                                //    aiPlayer.refreshAggression();
                                //}
                                //DssRef.diplomacy.declareWar(otherfaction, faction);
                                return otherfaction;
                            //}
                        }
                    }
                }
            }
            return null;
        }

        bool factionMayStartWar(Faction attacker, Faction defender)
        {
            if ((attacker.factiontype == FactionType.DefaultAi || attacker.factiontype == FactionType.DarkFollower) &&
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

    }

    enum BossTimeSettings
    { 
        Immediate,
        Early,
        Normal,
        Late,
        VeryLate,
        //Never,
        NUM
    }

    enum EventType
    { 
        AiDelay,
        AiWarDelay,
        WarmanagerDelay,
        SouthShips,
        DarkLordWarning,
        DarkLord,
        Factories,
        FactoriesDestroyed,
        DarkLordInPerson,
        KillTheDarkLord,
        
        End,
    }

    enum EventState
    { 
        Prepare,
        Countdown,
        PowerChecked,
        Done,
    }
}
