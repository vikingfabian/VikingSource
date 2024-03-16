using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
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

        public GameEvents()
        {
            if (DssRef.storage.bossTimeSettings != BossTimeSettings.Never)
            {
                prepareNext();
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
        }
        public void readGameState(System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            nextEvent = (EventType)r.ReadInt32();
            eventState = (EventState)r.ReadInt32();
            eventPrepareTimeSec = r.ReadSingle();
            eventCheckGameTimeSec = r.ReadSingle();
            eventTriggerGameTimeSec = r.ReadSingle();

            nextTotalGameTimeMin.Read(r);
            nextExpectedPlayerSize.Read(r);

            playerMostSouthCity = IOLib.ReadObjectList<City>(r).ToArray();
            spawnPos_Player = IOLib.ReadBinaryList<IntVector2>(r).ToArray();
            darkLordAvailableFactions = IOLib.ReadObjectList<Faction>(r);
            darkLordAllies = IOLib.ReadObjectList<Faction>(r);
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
                            new IntervalF(5,8),//Immediate,                           
                            new IntervalF(20,30),
                            new IntervalF(30,40),//Normal,
                            new IntervalF(40,60),
                            new IntervalF(60,80),//VeryLate,
                        };

                        nextTotalGameTimeMin = timeMinutes[(int)DssRef.storage.bossTimeSettings];
                        nextExpectedPlayerSize = new IntervalF(DssLib.HeadCityMaxWorkForce * 2f, DssLib.HeadCityMaxWorkForce * 4f);
                    }
                    break;
                case EventType.DarkLordWarning:
                    nextTotalGameTimeMin = IntervalF.NoInterval(8);
                    break;
                case EventType.DarkLord:
                    {
                        IntervalF[] timeMinutes =
                           {
                            new IntervalF(5,10),//Immediate,                            
                            new IntervalF(70,80),
                            new IntervalF(80,100),//Normal,
                            new IntervalF(100,240),
                            new IntervalF(140,320),//VeryLate,
                        };

                        nextTotalGameTimeMin = timeMinutes[(int)DssRef.storage.bossTimeSettings];
                        nextExpectedPlayerSize = new IntervalF(DssLib.HeadCityMaxWorkForce * 4f, DssLib.HeadCityMaxWorkForce * 8f);
                    }
                    break;
            }

            eventPrepareTimeSec = Ref.TotalGameTimeSec;
            eventCheckGameTimeSec = nextTotalGameTimeMin.Min * 60 + Ref.TotalGameTimeSec - 1f;
        }

        public void TestNextEvent()
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
                    victory();
                }
            }

            DssRef.state.localPlayers[0].hud.messages.Add(
                    "Test event", nextEvent.ToString());
        }

        void RunNextEvent()
        {
            switch (nextEvent)
            {
                case EventType.SouthShips:
                    {
                        var enemyFac = DssRef.world.factions.Array[DssRef.Faction_SouthHara];

                        for (int playerIx = 0; playerIx < DssRef.state.localPlayers.Count; ++playerIx)
                        {
                            if (playerMostSouthCity[playerIx] != null)
                            {
                                IntVector2 spawn = spawnPos_Player[playerIx];
                                Rotation1D enemyRot = Rotation1D.D0;

                                Range soldierCount = Range.Zero;

                                switch (DssRef.storage.bossSize)
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
                                    new SoldierGroup(army, UnitType.Pikeman, false);
                                }
                                count = soldierCount.GetRandom() / DssRef.state.localPlayers.Count;
                                for (int i = 0; i < count; ++i)
                                {
                                    new SoldierGroup(army, UnitType.Sailor, false);
                                }
                                count = soldierCount.GetRandom() / DssRef.state.localPlayers.Count;
                                for (int i = 0; i < count; ++i)
                                {
                                    new SoldierGroup(army, UnitType.CrossBow, false);
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
                            p.hud.messages.Add("A dark prophesy", "The Eye of Doom will appear soon, and your enemies will join him!");
                        }
                    }
                    break;
                case EventType.DarkLord:
                    {
                        if (arraylib.HasMembers(darkLordAvailableFactions))
                        { 
                            DssRef.state.darkLordPlayer.EnterMap(arraylib.RandomListMember(darkLordAvailableFactions), darkLordAllies);

                            darkLordAllies = null;
                            darkLordAvailableFactions = null;

                            var greenwood = DssRef.world.factions[DssRef.Faction_GreenWood];

                            foreach (var p in DssRef.state.localPlayers)
                            {
                                p.hud.messages.Add("Dark times", "The Eye of Doom has entered the map!");

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

        public void asyncUpdate()
        {
            if (DssRef.storage.bossTimeSettings != BossTimeSettings.Never &&
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
                    totalWorkForce += citiesC.sel.workForce.max;
                }
            }

            float time;
            System.Diagnostics.Debug.WriteLine("Player work force: " + totalWorkForce);
            System.Diagnostics.Debug.WriteLine("Expected work force: " + nextExpectedPlayerSize.ToString());
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
                DssRef.state.darkLordPlayer.factoriesLeft = 0;
                nextEvent = EventType.DarkLordInPerson;
                //Ref.update.AddSyncAction(new SyncAction(RunNextEvent));
            }
        }

        public void onDarkLordSpawn()
        {
            nextEvent = EventType.KillTheDarkLord;

            foreach (var p in DssRef.state.localPlayers)
            {
                p.hud.messages.Add("A desperate attack", "The dark lord has joined the battlefield. Now is your chance to destroy him!");
            }

        }
        public void onDarkLorDeath()
        {
            if (nextEvent != EventType.End)
            {
                victory();
            }
        }

        public void onAllDarkCitiesDestroyed()
        {
            if (nextEvent != EventType.End)
            {
                if (DssRef.state.darkLordPlayer.darkLordUnit == null)
                {
                    DssRef.achieve.UnlockAchievement(AchievementIndex.no_darklord);
                }
                victory();
            }
        }

        void victory()
        {
            nextEvent = EventType.End;
            DssRef.achieve.onVictory();
            DssRef.state.localPlayers[0].menuSystem.victoryScreen();
        }

       
    }

    enum BossTimeSettings
    { 
        Immediate,
        Early,
        Normal,
        Late,
        VeryLate,
        Never,
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
