﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        float eventPrepareTimeSec;
        float eventCheckGameTimeSec;
        float eventTriggerGameTimeSec;

        IntervalF nextTotalGameTimeMin;
        IntervalF nextExpectedPlayerSize;

        City[] playerMostSouthCity;
        IntVector2[] spawnPos_Player;

        List<Faction> darkLordAvailableFactions = null;
        List<Faction> darkLordAllies = null;

        public List<City> factories = new List<City>(3);

        Time dyingFactionsTimer = Time.Zero;

        Time toPeacefulCheckTimer = new Time(3, TimeUnit.Minutes);

        public GameEvents()
        {
        }

        public void onGameStart(bool newGame)
        {
            if (newGame)
            {
                if (DssRef.difficulty.runEvents)
                {
                    prepareNext();
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

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((int)nextEvent);
            w.Write((int)eventState);
            w.Write(eventPrepareTimeSec);
            w.Write(eventCheckGameTimeSec);
            w.Write(eventTriggerGameTimeSec);

            nextTotalGameTimeMin.Write(w);
            nextExpectedPlayerSize.Write(w);

            IOLib.WriteObjectList(w, playerMostSouthCity);
            IOLib.WriteBinaryList(w, spawnPos_Player);
            IOLib.WriteObjectList(w, darkLordAvailableFactions);
            IOLib.WriteObjectList(w, darkLordAllies);

            dyingFactionsTimer.write(w);
            w.Write(Ref.TotalGameTimeSec);
        }
        public void readGameState(System.IO.BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            nextEvent = (EventType)r.ReadInt32();
            eventState = (EventState)r.ReadInt32();
            eventPrepareTimeSec = r.ReadSingle();
            eventCheckGameTimeSec = r.ReadSingle();
            eventTriggerGameTimeSec = r.ReadSingle();

            nextTotalGameTimeMin.Read(r);
            nextExpectedPlayerSize.Read(r);

            playerMostSouthCity = arraylib.ToArray_Safe(IOLib.ReadObjectList<City>(r));
            spawnPos_Player = arraylib.ToArray_Safe(IOLib.ReadBinaryList<IntVector2>(r));
            darkLordAvailableFactions = IOLib.ReadObjectList<Faction>(r);
            darkLordAllies = IOLib.ReadObjectList<Faction>(r);

            dyingFactionsTimer.read(r);
            dyingFactionsTimer.MilliSeconds = Bound.Min(dyingFactionsTimer.MilliSeconds, 1);

            if (subVersion >= 5)
            {
                Ref.TotalGameTimeSec = r.ReadSingle();
            }
            else
            {
                Ref.TotalGameTimeSec = eventCheckGameTimeSec - 5;
            }
        }

        void prepareNext()
        {
            eventState = 0;
            switch (nextEvent)
            {
                case EventType.SouthShips:
                    {
                        IntervalF[] timeMinutes =
                            {
                            new IntervalF(60,80),//Immediate,                           
                            new IntervalF(70,100),
                            new IntervalF(130,140),//Normal,
                            new IntervalF(140,160),
                            new IntervalF(160,180),//VeryLate,
                        };

                        nextTotalGameTimeMin = timeMinutes[(int)DssRef.difficulty.bossTimeSettings];
                        nextExpectedPlayerSize = new IntervalF(DssConst.HeadCityStartMaxWorkForce * 2f, DssConst.HeadCityStartMaxWorkForce * 4f);
                    }
                    break;
                case EventType.DarkLordWarning:
                    {
                        IntervalF[] timeMinutes =
                              {
                            new IntervalF(135,140),//Immediate,                            
                            new IntervalF(200,220),
                            new IntervalF(220,230),//Normal,
                            new IntervalF(230,370),
                            new IntervalF(270,450),//VeryLate,
                        };

                        nextTotalGameTimeMin = timeMinutes[(int)DssRef.difficulty.bossTimeSettings];
                        nextExpectedPlayerSize = new IntervalF(DssConst.HeadCityStartMaxWorkForce * 4f, DssConst.HeadCityStartMaxWorkForce * 8f);
                    }
                    break;
                case EventType.DarkLord:
                    {
                        nextTotalGameTimeMin = IntervalF.NoInterval(30);
                    }
                    break;
            }

            eventPrepareTimeSec = Ref.TotalGameTimeSec;
            eventCheckGameTimeSec = nextTotalGameTimeMin.Min * 60 + Ref.TotalGameTimeSec - 1f;
        }

        public void TestNextEvent()
        {
            if (DssRef.settings.AiDelay)
            {
                DssRef.settings.AiDelay = false;
                DssRef.state.localPlayers[0].hud.messages.Add(
                        "Test event", "Removed AI delay");
            }
            else
            {
                if (nextEvent <= EventType.DarkLord)
                {
                    PowerCheck();
                    calcAndRunEvent();
                }
                else
                {
                    nextEvent++;

                    if (nextEvent == EventType.KillTheDarkLord)
                    {
                        victory(true);
                    }
                }

                DssRef.state.localPlayers[0].hud.messages.Add(
                        "Test event", nextEvent.ToString());
            }
        }

        void RunNextEvent()
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
                                    new SoldierGroup(army, DssLib.SoldierProfile_Pikeman);
                                }
                                count = soldierCount.GetRandom() / DssRef.state.localPlayers.Count;
                                for (int i = 0; i < count; ++i)
                                {
                                    new SoldierGroup(army, DssLib.SoldierProfile_Sailor);
                                }
                                count = soldierCount.GetRandom() / DssRef.state.localPlayers.Count;
                                for (int i = 0; i < count; ++i)
                                {
                                    new SoldierGroup(army, DssLib.SoldierProfile_CrossbowMan);
                                }
                                army.refreshPositions(true);


                                var groupsC = army.groups.counter();
                                while (groupsC.Next())
                                {
                                    groupsC.sel.completeTransform( SoldierTransformType.ToShip);
                                }

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

            ++nextEvent;
            prepareNext();
        }

        public void asyncUpdate(float time)
        {
            if (DssRef.difficulty.runEvents &&
                !DssRef.settings.AiDelay &&
                (
                nextEvent == EventType.SouthShips ||
                nextEvent == EventType.DarkLordWarning ||
                nextEvent == EventType.DarkLord
                ))
            {
                if (eventState == EventState.Countdown)
                {
                    if (Ref.TotalGameTimeSec >= eventCheckGameTimeSec)
                    {
                        PowerCheck();
                    }
                }
                else if (eventState == EventState.PowerChecked)
                {
                    if (Ref.TotalGameTimeSec >= eventTriggerGameTimeSec)
                    {
                        calcAndRunEvent();
                    }
                }
            }

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
                    faction.gold = -10000;
                    faction.hasDeserters = true;
                }
            }

            if (DssRef.difficulty.toPeacefulCheck && toPeacefulCheckTimer.CountDown(time))
            {
                toPeacefulCheckTimer = new Time(15, TimeUnit.Minutes);

                foreach (var p in DssRef.state.localPlayers)
                {
                    p.toPeacefulCheck_asynch();
                }
            }
        }

       

        void PowerCheck()
        {
            eventState = EventState.PowerChecked;
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

            float time;
            //System.Diagnostics.Debug.WriteLine("Player work force: " + totalWorkForce);
            //System.Diagnostics.Debug.WriteLine("Expected work force: " + nextExpectedPlayerSize.ToString());
            if (totalWorkForce < nextExpectedPlayerSize.Min)
            {
                time = nextTotalGameTimeMin.Max;
            }
            else if (totalWorkForce >= nextExpectedPlayerSize.Max)
            {
                time = nextTotalGameTimeMin.Min;
            }
            else
            {
                time = nextTotalGameTimeMin.Center;
            }

            time *= 60;

            time += time * Ref.rnd.Plus_MinusF(0.2f);

            asyncPrepare(ref time);

            eventTriggerGameTimeSec = time + eventPrepareTimeSec;
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
                            factionC.sel.factiontype == FactionType.DarkLord ||
                            factionC.sel.factiontype == FactionType.DarkFollower ||
                            factionC.sel.factiontype == FactionType.SouthHara ||
                            factionC.sel.factiontype == FactionType.UnitedKingdom)
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
            eventState = EventState.Done;

            switch (nextEvent)
            {
                case EventType.SouthShips:
                    {
                        calcSouthSpawn();
                    }
                    break;
                //case EventType.DarkLord:
                //    {

                //    }
                //    break;
            }

            Ref.update.AddSyncAction(new SyncAction(RunNextEvent));
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
                            Map.Tile tile;
                            if (DssRef.world.tileGrid.TryGet(pos, out tile))
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
                Ref.update.AddSyncAction(new SyncAction(RunNextEvent));
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
        Countdown,
        PowerChecked,
        Done,
    }
}
