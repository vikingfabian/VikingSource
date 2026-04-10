using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;

namespace VikingEngine.DSSWars.Event
{
    abstract class AbsStoryEvent
    {
        //protected TimeInGameCountdown prepareTime;
        protected TimeInGameCountdown checkTime;
        public TimeInGameCountdown triggerTime;
        protected EventState eventState = EventState.InQueue;
        protected IntervalF triggerTimeSpan_Minutes;
        protected IntervalF nextExpectedPlayerSize;
        virtual public void onStart() { }


        protected void init(IntervalF triggerTimeSpan_Minutes, IntervalF nextExpectedPlayerSize)
        {
            this.triggerTimeSpan_Minutes = triggerTimeSpan_Minutes;
            this.nextExpectedPlayerSize = nextExpectedPlayerSize;
            this.init(true, TimeLength.FromMinutes(triggerTimeSpan_Minutes.Min));
        }
        protected void init(bool hasPowerCheck, TimeLength countDown)
        {
            if (hasPowerCheck)
            {
                eventState = EventState.PowerCheck_countdown;
                checkTime.start(countDown);
            }
            else
            {
                eventState = EventState.TriggerEvent_countdown;
                triggerTime.start(countDown);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns>Complete</returns>
        virtual public bool asyncUpdate(float time)
        {
            if (TimedEvent())
            {
                switch (eventState)
                {
                    case EventState.PowerCheck_countdown:
                        {
                            if (checkTime.TimeOut())
                            {
#if DEBUG
                                Ref.update.AddSyncAction(new SyncAction(() =>
                                {
                                    DssRef.state.localPlayers[0].hud.messages.Add(
                                        "Event Power check", StoryEventType().ToString());
                                }));
#endif

                                PowerCheck();
                                eventState = EventState.TriggerEvent_countdown;
                            }
                        }
                        break;
                    case EventState.TriggerEvent_countdown:
                        if (triggerTime.TimeOut())
                        {
#if DEBUG
                            Ref.update.AddSyncAction(new SyncAction(() =>
                            {
                                DssRef.state.localPlayers[0].hud.messages.Add(
                                    "Event Trigger", StoryEventType().ToString());
                            }));

#endif
                            calcAndRunEvent_async();
                            eventState = EventState.Done;
                        }
                        break;
                }
                
            }
            return eventState == EventState.Done;
        }

        /// <summary>
        /// The timer is out and the event is finally fired off
        /// </summary>
        virtual protected void calcAndRunEvent_async()
        {
        }

        protected void PowerCheck()
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
                    //var citiesC = p.faction.cities.counter();
                    //while (citiesC.Next())
                    //{
                    SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                    while (citiesC.Next(ref p.faction.cities, DssRef.world.cities, out City citySel))
                    {
                        totalWorkForce += citySel.HousingCount_Workers;
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

            triggerTime.start(time);

        }

        public void TriggerNow()
        {
            checkTime.start(1);
            triggerTime.start(2);
            triggerTimeSpan_Minutes = IntervalF.NoInterval(0.1f);
        }

        virtual public bool HasSaveData()
        {
            return eventState > EventState.InQueue;
        }

        virtual public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((int)eventState);

            checkTime.writeGameState(w);
            triggerTime.writeGameState(w);

            triggerTimeSpan_Minutes.Write(w);
            nextExpectedPlayerSize.Write(w);
        }

        virtual public void readGameState(System.IO.BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            eventState = (EventState)r.ReadInt32();

            checkTime.readGameState(r);
            triggerTime.readGameState(r);

            triggerTimeSpan_Minutes.Read(r);
            nextExpectedPlayerSize.Read(r);
        }

        virtual protected void asyncPrepare(ref float time)
        { }

        virtual protected bool TimedEvent() { return true; }

        //abstract protected string DebugName();
        abstract public EventType StoryEventType();


        abstract public int OrderIndex();
        virtual public bool RunAi()
        {
            return true;
        }

        virtual public bool MayAttackPlayer()
        {
            return true;
        }

        virtual public bool RunWarManager()
        {
            return true;
        }
    }

    class StoryEvent_Tutorial : AbsStoryEvent
    {
        public override EventType StoryEventType()
        {
            return Event.EventType.Tutorial;
        }

        public override bool asyncUpdate(float time)
        {
            return DssRef.state.localPlayers[0].tutorial == null;
        
        }
        public override bool RunAi()
        {
            return false;
        }

        public override bool MayAttackPlayer()
        {
            return false;
        }
        public override bool RunWarManager()
        {
            return false;
        }

        protected override bool TimedEvent()
        {
            return false;
        }
        public override int OrderIndex()
        {
            return EventsOrder.Tutorial;
        }
    }

    class StoryEvent_AiDelay : AbsStoryEvent
    { 
        public StoryEvent_AiDelay()
        {
            
        }
        public override void onStart()
        {
            var time = new TimeLength(DssRef.difficulty.aiDelayTimeSec);
            if (DssRef.difficulty.setting_gameMode == GameModeMainType.QuickMatch)
            {
                time.seconds *= 0.5f;
            }
            init(false, time);
        }
        public override EventType StoryEventType()
        {
            return Event.EventType.AiDelay;
        }
        public override bool RunAi()
        {
            return false;
        }

        public override bool MayAttackPlayer()
        {
            return false;
        }

        public override bool RunWarManager()
        {
            return false;
        }

        public override int OrderIndex()
        {
            return EventsOrder.AiDelay;
        }
        //public bool AiDelay()
        //{
        //    return nextEvent <= EventType.AiDelay;
        //}

        //public bool MayAttackPlayer()
        //{
        //    return nextEvent > EventType.AiWarDelay;
        //}

    }

    class StoryEvent_AiWarDelay : AbsStoryEvent
    {
        public override EventType StoryEventType()
        {
            return Event.EventType.AiWarDelay;
        }
        public override void onStart()
        {
            bool settler = DssRef.storage.gameRuleset.factionStartSize == FactionStartSize.Settler;
            TimeLength time;
            if (DssRef.difficulty.extremeAggression)
            {
                if (settler) 
                {
                    time = new TimeLength(10);
                }
                else
                {
                    time = TimeLength.FromMinutes(5);
                }
            }
            else
            {
                if (settler)
                {
                    time = TimeLength.FromMinutes(50);
                }
                else
                {
                    time = TimeLength.FromMinutes(30); //DEFAULT
                }
            }
            init(false, time);
        }
        public override bool RunAi()
        {
            return true;
        }
        public override bool MayAttackPlayer()
        {
            return false;
        }
        public override bool RunWarManager()
        {
            return false;
        }
        public override int OrderIndex()
        {
            return EventsOrder.AiWarDelay;
        }
    }

    class StoryEvent_FirstAttack : AbsStoryEvent
    {
        public override EventType StoryEventType()
        {
            return Event.EventType.FirstAttack;
        }
        public override void onStart()
        {
            var triggerTimeSpan_Minutes = new IntervalF(15f, 35f) + Ref.rnd.Float(20);

            if (DssRef.difficulty.extremeAggression)
            {
                triggerTimeSpan_Minutes = IntervalF.NoInterval(Ref.rnd.Float(9f, 12f));
            }

            init(
                triggerTimeSpan_Minutes,
                nextExpectedPlayerSize: new IntervalF(DssConst.HeadCityStartMaxWorkForce * 1f, DssConst.HeadCityStartMaxWorkForce * 2f));
        }

        protected override void calcAndRunEvent_async()
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                var attacker = DssRef.state.events.findAttackingNeighborFaction_keepExpanding(p.faction);
                if (attacker != null)
                {
                    attacker.player.setMinimumAggression(AbsPlayer.AggressionLevel2_RandomAttacks);
                    DssRef.world.diplomacy.declareWar(attacker, p.faction);
                }
            }
        }

        public override bool RunWarManager()
        {
            return false;
        }
        public override int OrderIndex()
        {
            return EventsOrder.FirstAttack;
        }
    }

    class StoryEvent_WarmanagerDelay : AbsStoryEvent
    {
        public override EventType StoryEventType()
        {
            return Event.EventType.WarmanagerDelay;
        }
        public override void onStart()
        {
            init(false, TimeLength.FromMinutes(DssRef.difficulty.extremeAggression ? 1 : 50));
        }
        public override bool RunWarManager()
        {
            return false;
        }
        public override int OrderIndex()
        {
            return EventsOrder.WarmanagerDelay;
        }
    }

    class StoryEvent_Barbarians : AbsStoryEvent
    {
        //List<int> attackCities = null;

        public override EventType StoryEventType()
        {
            return Event.EventType.Barbarians;
        }
        public override void onStart()
        {
            init(
                triggerTimeSpan_Minutes: new IntervalF(1.2f, 2.4f) * TimeExt.HourInMinutes,
                nextExpectedPlayerSize: new IntervalF(DssConst.HeadCityStartMaxWorkForce * 1f, DssConst.HeadCityStartMaxWorkForce * 2f));
        }

        protected override void calcAndRunEvent_async()
        {
            var attackCities = new List<int>(8);

            List<City> completedCities = new List<City>();

            foreach (var p in DssRef.state.localPlayers)
            {
                List<City> searchcities = p.faction.cities.toList(DssRef.world.cities);

                int found = 0;
                while (found < 3)
                { 
                    var check = arraylib.RandomListMemberPop(searchcities);
                    if (check != null)
                    {
                        EcsStaticArrayCounter neighbors = check.CityNeighbors();
                        while (neighbors.Next(out int nCityIx))//foreach (var ncityIx in check.neighborCities)
                        {
                            //int nCityIx = DssRef.world.neighborCities.array[ncaIx];
                            if (!attackCities.Contains(nCityIx))
                            {
                                var ncity_p = DssRef.world.cities[nCityIx];
                                if (!completedCities.Contains(ncity_p) &&
                                    !searchcities.Contains(ncity_p))
                                {
                                    searchcities.Add(ncity_p);
                                }
                            }
                        }

                        var player = check.GetPlayer();
                        if (player != null &&
                            player.IsBot() &&
                            player.faction.diplomaticSide != DiplomaticSide.Dark &&
                            check.cityType < CityType.Capital &&
                            DssRef.world.diplomacy.GetRelation(check.GetFaction(), p.faction).Relation >= RelationType.RelationType0_Neutral)
                        {
                            attackCities.Add(check.myIndex);
                            found++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Ref.update.AddSyncAction(new SyncAction(() =>
            {
                if (attackCities.Count > 0)
                {
                    foreach (var p in DssRef.state.localPlayers)
                    {
                        p.hud.messages.Add(DssRef.lang.EventMessage_Event_Title, DssRef.lang.EventMessage_DarkHorde);
                    }

                    foreach (var cityIx in attackCities)
                    {
                        spawnBarbarians(DssRef.world.cities[cityIx], false);
                    }
                }
                attackCities = null;
            }));
        }

        public static Army spawnBarbarians(City city, bool tutorial)
        {
            for (int trial = 1; trial <= 2; trial++)
            {
                
                ForXYEdgeLoopRandomPicker loop = new ForXYEdgeLoopRandomPicker();
                int cityradius = city.cityTileArea.size.SideLength() / 2;
                for (int radius = Bound.Min(cityradius - 2, 4); radius < cityradius + 2; ++radius)
                {
                    loop.start(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));
                    while (loop.Next())
                    {
                        if (DssRef.world.tileGrid.TryGet(loop.Position, out var tile) &&
                            tile.IsLand() &&
                            tile.tileContent != Map.TileContent.City &&
                            (trial > 1 || tile.CityIndex == city.myIndex)) //require same city area on first trial
                        {
                            //Available for spawn
                            Faction enemyFac = DssRef.world.faction(DssRef.settings.Faction_Barbarian);

                            if (enemyFac == null)
                            {
                                enemyFac = DssRef.world.findOrCreate(FactionType.Barbarians, DssRef.settings.Faction_Barbarian);
                                DssRef.settings.Faction_Barbarian = enemyFac.myIndex;
                            }

                            if (tutorial)
                            {
                                enemyFac.player.GetAiPlayer().armyAi_enabled = false;
                            }

                            var barbarianArmy = enemyFac.NewArmy(loop.Position);
                            {
                                SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                                {
                                    conscript = new ConscriptProfile()
                                    {
                                        weapon = Resource.ItemResourceType.ShortSword,
                                        armorLevel = Resource.ItemResourceType.PaddedArmor,
                                        training = TrainingLevel.Basic,
                                        specialization = SpecializationType.Field,
                                    }
                                };

                                int rndCount = Ref.rnd.Int(3, 5) + (int)DssRef.difficulty.bossSize * 2;
                                if (tutorial)
                                {
                                    rndCount = 2;
                                }

                                for (int i = 0; i < rndCount; ++i)
                                {
                                    new SoldierGroup(barbarianArmy, SoldierProfile, barbarianArmy.position);
                                }
                            }
                            {
                                SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                                {
                                    conscript = new ConscriptProfile()
                                    {
                                        weapon = Resource.ItemResourceType.ThrowingSpear,
                                        armorLevel = Resource.ItemResourceType.NONE,
                                        training = TrainingLevel.Minimal,
                                        specialization = SpecializationType.Field,
                                    }
                                };

                                int rndCount = Ref.rnd.Int(0, 2) + (int)DssRef.difficulty.bossSize * 2;
                                if (tutorial)
                                {
                                    rndCount = 0;
                                }
                                for (int i = 0; i < rndCount; ++i)
                                {
                                    new SoldierGroup(barbarianArmy, SoldierProfile, barbarianArmy.position);
                                }
                            }
                            {
                                SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                                {
                                    conscript = new ConscriptProfile()
                                    {
                                        weapon = Resource.ItemResourceType.Crossbow,
                                        armorLevel = Resource.ItemResourceType.NONE,
                                        training = TrainingLevel.Basic,
                                        specialization = SpecializationType.Field,
                                    }
                                };

                                int rndCount = Ref.rnd.Int(0, 2) + (int)DssRef.difficulty.bossSize * 2;
                                if (tutorial)
                                {
                                    rndCount = 1;
                                }
                                for (int i = 0; i < rndCount; ++i)
                                {
                                    new SoldierGroup(barbarianArmy, SoldierProfile, barbarianArmy.position);
                                }
                            }
                            barbarianArmy.refreshPositions(true);
                            barbarianArmy.setAsStartArmy();
                            barbarianArmy.setMassiveFood();

                            enemyFac.money.SetGold(100000);
                            enemyFac.player.protectedFromDelete = false;

                            foreach (var p in DssRef.state.localPlayers)
                            {
                                DssRef.world.diplomacy.declareWar(enemyFac, p.faction);
                            }

                                return barbarianArmy;
                        }
                    }
                }
            }
#if DEBUG
            throw new Exception("No enemy spawn");
#endif
            return null;
        }

        public override bool RunWarManager()
        {
            return true;
        }
        public override int OrderIndex()
        {
            return EventsOrder.WarmanagerDelay;
        }
    }

    class StoryEvent_Mercenaries : AbsStoryEvent
    {
        City[] playerMostSouthCity;
        IntVector2[] spawnPos_Player;

        public override EventType StoryEventType()
        {
            return Event.EventType.Mercenaries;
        }

        public override void onStart()
        {
           init(
            triggerTimeSpan_Minutes: new IntervalF(3.6f, 4.5f) * TimeExt.HourInMinutes,
            nextExpectedPlayerSize: new IntervalF(DssConst.HeadCityStartMaxWorkForce * 2f, DssConst.HeadCityStartMaxWorkForce * 4f));
        }

        protected override void asyncPrepare(ref float time)
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

        protected override void calcAndRunEvent_async()
        {
            calcSouthSpawn();

            Ref.update.AddSyncAction(new SyncAction(() =>
            {
                var enemyFac = DssRef.world.findOrCreate(FactionType.SouthHara, DssRef.settings.Faction_SouthHara);

                if (enemyFac == null)
                {
                    return;                    
                }
                DssRef.settings.Faction_SouthHara = enemyFac.myIndex;

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
                        army.startInOnePoint();

                        DssRef.world.diplomacy.declareWar(enemyFac, DssRef.state.localPlayers[playerIx].faction);
                        army.Order_MoveTo(VectorExt.AddY(playerMostSouthCity[playerIx].tilePos, 3));
                    }
                }

                enemyFac.player.GetAiPlayer().nextDecisionTimer.MilliSeconds = float.MaxValue;
                new SouthHaraStartAi(enemyFac);

                enemyFac.player.protectedFromDelete = false;
            }));
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
                //var citiesC = DssRef.state.localPlayers[playerIx].faction.cities.counter();
                //while (citiesC.Next())
                //{
                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref DssRef.state.localPlayers[playerIx].faction.cities, DssRef.world.cities, out City citySel))
                {
                    if (mostSouth == null || citySel.tilePos.Y > mostSouth.tilePos.Y)
                    {
                        mostSouth = citySel;
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

        public override void writeGameState(BinaryWriter w)
        {
            base.writeGameState(w);
            IOLib.WriteObjectList(w, playerMostSouthCity);
            IOLib.WriteBinaryList(w, spawnPos_Player);
        }

        public override void readGameState(BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            base.readGameState(r, subVersion, pointers);
            playerMostSouthCity = arraylib.ToArray_Safe(IOLib.ReadObjectList<City>(r));
            spawnPos_Player = arraylib.ToArray_Safe(IOLib.ReadBinaryList<IntVector2>(r));
        }

        public override int OrderIndex()
        {
            return EventsOrder.Mercenaries;
        }
    }

    class StoryEvent_Cohalition : AbsStoryEvent
    {
        public override void onStart()
        {
            base.onStart();

            triggerTime.start(TimeLength.FromHours(Ref.rnd.Float(5f, 7f)));

            foreach (var p in DssRef.state.localPlayers)
            {
                if (p.faction.cities.Count >= p.nextDominationSize)
                {
                    p.nextDominationSize = p.faction.cities.Count + DssConst.DominationSizeIncrease.GetRandom();
                }
            }
        }

        public override bool asyncUpdate(float time)
        {   
            foreach (var p in DssRef.state.localPlayers)
            {
                if (!p.cohalitionEvent)
                {
                    if (p.faction.cities.Count >= p.nextDominationSize)
                    {
                        collectAllianceAgainstPlayerDomination(p);
                    }
                    else if (!p.cohalitionWarning && p.faction.cities.Count >= p.nextDominationSize - 3)
                    {
                        cohalitionWarning(p);
                    }
                }
            }

            if (triggerTime.TimeOut())
            {
#if DEBUG
                Ref.update.AddSyncAction(new SyncAction(() =>
                {
                    DssRef.state.localPlayers[0].hud.messages.Add(
                        "Event Trigger", StoryEventType().ToString());
                }));

#endif
                //calcAndRunEvent_async();
                foreach (var p in DssRef.state.localPlayers)
                {
                    
                        if (!p.cohalitionEvent)
                        {
                            cohalitionWarning(p);
                            collectAllianceAgainstPlayerDomination(p);
                        }
                    
                }
                eventState = EventState.Done;
            }

            return eventState == EventState.Done;
        }

        void cohalitionWarning(LocalPlayer player)
        {
            player.cohalitionWarning = true;

            Ref.update.AddSyncAction(new SyncAction(() =>
            {
                player.hud.messages.Add(DssRef.lang.EventMessage_EnemyAlliance_Title, DssRef.lang.EventMessage_EnemyAlliance);
            }));
        }

        void collectAllianceAgainstPlayerDomination(LocalPlayer player)
        {
            player.cohalitionEvent = true;

            Faction neighbor = DssRef.state.events.findAttackingNeighborFaction_keepExpanding(player.faction);

            if (neighbor == null)
            {
                return;
            }

            List<Faction> attackers = new List<Faction>() { neighbor };
            int totalSize = neighbor.totalWorkForce;
            List<Faction> search = neighbor.adjacentFactions(true);
            List<Faction> has_searched = new List<Faction>();

            int maxLoops = 100;
            while (--maxLoops > 0 && totalSize < player.faction.totalWorkForce * 1.5f)
            {
                if (search.Count > 0)
                {
                    var faction = arraylib.RandomListMemberPop(search);
                    bool bHasSearched = has_searched.Contains(faction);

                    if (!bHasSearched &&
                        DssRef.world.diplomacy.botMayStartWar(faction, player.faction) &&
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
                        search.AddRange(faction.adjacentFactions(true));
                    }
                }
            }

            Faction attackLeader = neighbor;
            //Create an alliance
            foreach (var faction in attackers)
            {
                foreach (var other in attackers)
                {
                    if (other != faction)
                    {
                        DssRef.world.diplomacy.SetRelationType(faction, other, RelationType.RelationType3_Ally);
                    }
                }

                DssRef.world.diplomacy.SetRelationType(faction, player.faction, RelationType.RelationTypeN1_Enemies);

                if (attackLeader == null || faction.militaryStrength > attackLeader.militaryStrength)
                {
                    attackLeader = faction;
                }
            }

            //Prepare leader
            attackers.Remove(attackLeader);
            attackers.Insert(0, attackLeader);
            DssRef.world.diplomacy.SetRelationType(attackLeader, player.faction, null, null, SpeakTerms.SpeakTermsN2_None);
            attackLeader.player.setAggression(Players.AbsPlayer.AggressionLevel1_RevengeOnly);

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
                    weapon = Resource.ItemResourceType.Crossbow,
                    armorLevel = Resource.ItemResourceType.PaddedArmor,
                    training = TrainingLevel.Basic,
                    specialization = SpecializationType.Traditional,
                };

                city.conscriptArmy(meleeProfile, city.defaultConscriptPos(), 3 + (int)DssRef.difficulty.bossSize);
                city.conscriptArmy(rangedProfile, city.defaultConscriptPos(), 3 + (int)DssRef.difficulty.bossSize);

                if (DssRef.difficulty.bossSize >= BossSize.Large)
                {
                    var cannonProfile = new ConscriptProfile()
                    {
                        weapon = Resource.ItemResourceType.ManCannonBronze,
                        armorLevel = Resource.ItemResourceType.PaddedArmor,
                        training = TrainingLevel.Basic,
                        specialization = SpecializationType.Siege,
                    };
                    city.conscriptArmy(cannonProfile, city.defaultConscriptPos(), 1 + (int)DssRef.difficulty.bossSize);
                }
            }));

            new Timer.TimedAction2ArgTrigger_InGame<Faction[], LocalPlayer>((attackers, player) =>
            {
                attackers.First().player.setAggression(Players.AbsPlayer.AggressionLevel3_FocusedAttacks);
                foreach (var faction in attackers)
                {
                    faction.player.setMinimumAggression(Players.AbsPlayer.AggressionLevel2_RandomAttacks);
                    DssRef.world.diplomacy.SetRelationType(faction, player.faction, RelationType.RelationTypeN3_War);
                }

                player.hud.messages.Add(DssRef.lang.EventMessage_Event_Title, DssRef.lang.EventMessage_TheCohalition);
            }, attackers.ToArray(), player, TimeExt.MinuteInSeconds * DssConst.DominationWarTimeDelay_Minutes.GetRandom());


            //List<Faction> adjacentFactions(Faction faction)
            //{
            //    List<Faction> factions = new List<Faction>();
                
            //    SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            //    while (citiesC.Next(ref faction.cities, DssRef.world.cities, out City citySel))
            //    {
            //        EcsStaticArrayCounter neighbors = citySel.CityNeighbors();
            //        while (neighbors.Next(DssRef.world.cities, out City nCity))
            //        {
            //            var nCityFaction = nCity.GetFaction();

            //            if (nCityFaction != faction &&
            //                nCityFaction.player.IsBot() &&
            //                !factions.Contains(nCityFaction))
            //            {
            //                factions.Add(nCityFaction);
            //            }
            //        }
            //    }

            //    return factions;
            //}
        }

        public override void writeGameState(BinaryWriter w)
        {
            base.writeGameState(w);
        }

        public override int OrderIndex()
        {
            return EventsOrder.Cohalition;
        }
        public override EventType StoryEventType()
        {
            return EventType.Cohalition;
        }
    }

    class StoryEvent_DarkLordWarning : AbsStoryEvent
    {
        public override EventType StoryEventType()
        {
            return Event.EventType.BossWarning;
        }
        public override void onStart()
        {
            init(
             triggerTimeSpan_Minutes: new IntervalF(9f, 13f) * TimeExt.HourInMinutes,
             nextExpectedPlayerSize: new IntervalF(DssConst.HeadCityStartMaxWorkForce * 4f, DssConst.HeadCityStartMaxWorkForce * 16f));
        }

        protected override void calcAndRunEvent_async()
        {
            Ref.update.AddSyncAction(new SyncAction(() =>
            {
                if (DssRef.state.events.maxWarsJuggles >= 6)
                {
                    DssRef.achieve.UnlockAchievement(AchievementIndex.warjuggler_tier1);
                    if (DssRef.state.events.maxWarsJuggles >= 9)
                    {
                        DssRef.achieve.UnlockAchievement(AchievementIndex.warjuggler_tier2);
                        if (DssRef.state.events.maxWarsJuggles >= 12)
                        {
                            DssRef.achieve.UnlockAchievement(AchievementIndex.warjuggler_tier3);
                        }
                    }
                }

                foreach (var p in DssRef.state.localPlayers)
                {
                    p.hud.messages.Add(DssRef.lang.EventMessage_ProphesyTitle, DssRef.lang.EventMessage_ProphesyText);
                }
            }));
        }
        public override int OrderIndex()
        {
            return EventsOrder.DarkLordWarning;
        }
    }

    class StoryEvent_DarkLord : AbsStoryEvent
    {
        List<Faction> darkLordAvailableFactions = null;
        List<Faction> darkLordAllies = null;

        public override EventType StoryEventType()
        {
            return Event.EventType.Boss;
        }
        public override void onStart()
        {
            init(false, TimeLength.FromHours(1f));
        }

        void asyncPrepare()
        {
            Rectangle2 mapCenter = new Rectangle2(IntVector2.Zero, DssRef.world.Size);
            mapCenter.AddRadius(-mapCenter.Height / 8);

            List<Faction> perfectPosition = new List<Faction>();
            List<Faction> available = new List<Faction>();
            darkLordAllies = new List<Faction>(16);
            var secondaryChoise = new List<Faction>(16);

            var factionC = DssRef.world.factions.counter();

            while (factionC.Next())
            {
                if (
                    (
                        factionC.sel.factiontype == FactionType.DefaultAi ||
                        factionC.sel.factiontype == FactionType.DarkFollower ||
                        factionC.sel.factiontype == FactionType.Barbarians ||
                        factionC.sel.factiontype == FactionType.SouthHara
                    ) &&
                    factionC.sel.cities.Count >= 2 &&
                    !DssRef.world.diplomacy.PositiveRelationWithPlayer(factionC.sel))
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

                if (DssRef.world.diplomacy.NegativeRelationWithPlayer(factionC.sel) ||
                    factionC.sel.diplomaticSide == DiplomaticSide.Dark)
                {
                    darkLordAllies.Add(factionC.sel);
                }
                else if (!DssRef.world.diplomacy.PositiveRelationWithPlayer(factionC.sel, RelationType.RelationType3_Ally))
                {
                    secondaryChoise.Add(factionC.sel);
                }
            }

            if (perfectPosition.Count > 0)
            {
                darkLordAvailableFactions = perfectPosition;
            }
            else
            {
                darkLordAvailableFactions = available;

                if (available.Count == 0)
                {
                    available.AddRange(secondaryChoise);
                }
            }
        }
        protected override void calcAndRunEvent_async()
        {
            asyncPrepare();

            Ref.update.AddSyncAction(new SyncAction(() =>
            {
                DssRef.achieve.UnlockAchievement_onAny_100(AchievementIndex.reach_boss_any, AchievementIndex.reach_boss_100);

                //if (arraylib.HasMembers(darkLordAvailableFactions))
                //{
                    DssRef.settings.darkLordPlayer.EnterMap(arraylib.RandomListMember(darkLordAvailableFactions), darkLordAllies);

                    var greenwood = DssRef.world.faction(DssRef.settings.Faction_GreenWood);

                    foreach (var p in DssRef.state.localPlayers)
                    {
                        p.hud.messages.Add(DssRef.lang.EventMessage_FinalBossEnterTitle, DssRef.lang.EventMessage_FinalBossEnterText);

                        if (greenwood != null && !DssRef.world.diplomacy.GetRelation(p.faction, greenwood).InWar())
                        {
                            DssRef.world.diplomacy.SetRelationType(p.faction, greenwood, null, null, SpeakTerms.SpeakTerms1_Good);
                        }
                    }
                //}
            }));
        }

        public override void writeGameState(BinaryWriter w)
        {
            base.writeGameState(w);
            IOLib.WriteObjectList(w, darkLordAvailableFactions);
            IOLib.WriteObjectList(w, darkLordAllies);
        }

        public override void readGameState(BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            base.readGameState(r, subVersion, pointers);
            darkLordAvailableFactions = IOLib.ReadObjectList<Faction>(r);
            darkLordAllies = IOLib.ReadObjectList<Faction>(r);
        }
        public override int OrderIndex()
        {
            return EventsOrder.DarkLord;
        }
    }

    //class StoryEvent_Factories : AbsStoryEvent
    //{
    //    public override EventType StoryEventType()
    //    {
    //        return Event.EventType.Factories;
    //    }

    //    protected override bool TimedEvent()
    //    {
    //        return false;
    //    }

    //    public override int OrderIndex()
    //    {
    //        return EventsOrder.Factories;
    //    }
    //}

    //class StoryEvent_FactoriesDestroyed : AbsStoryEvent
    //{
    //    public override EventType StoryEventType()
    //    {
    //        return Event.EventType.FactoriesDestroyed;
    //    }

    //    protected override bool TimedEvent()
    //    {
    //        return false;
    //    }

    //    public override int OrderIndex()
    //    {
    //        return EventsOrder.FactoriesDestroyed;
    //    }
    //}

    //class StoryEvent_DarkLordInPerson : AbsStoryEvent
    //{
    //    public override EventType StoryEventType()
    //    {
    //        return Event.EventType.DarkLordInPerson;
    //    }
    //    protected override bool TimedEvent()
    //    {
    //        return false;
    //    }
    //    public override int OrderIndex()
    //    {
    //        return EventsOrder.DarkLordInPerson;
    //    }
    //}
    //class StoryEvent_KillTheDarkLord : AbsStoryEvent
    //{
    //    public override EventType StoryEventType()
    //    {
    //        return Event.EventType.KillTheDarkLord;
    //    }

    //    public override void onStart()
    //    {
    //        Ref.update.AddSyncAction(new SyncAction(() =>
    //        {
    //            foreach (var p in DssRef.state.localPlayers)
    //            {
    //                p.hud.messages.Add(DssRef.lang.EventMessage_FinalBattleTitle, DssRef.lang.EventMessage_FinalBattleText);
    //            }
    //        }));
    //    }

    //    protected override bool TimedEvent()
    //    {
    //        return false;
    //    }
    //    public override int OrderIndex()
    //    {
    //        return EventsOrder.KillTheDarkLord;
    //    }
    //}

    class StoryEvent_DefeatTheBoss : AbsStoryEvent
    {
        public override EventType StoryEventType()
        {
            return Event.EventType.DefeatTheBoss;
        }

        protected override bool TimedEvent()
        {
            return false;
        }
        public override int OrderIndex()
        {
            return EventsOrder.DefeatTheBoss;
        }
    }

    class StoryEvent_QuickMatch : AbsStoryEvent
    {
        List<Faction> matchFactions;
        public StoryEvent_QuickMatch()
        {
            matchFactions = Factions();
        }
        public override EventType StoryEventType()
        {
            return Event.EventType.QuickMatch;
        }

        public override bool asyncUpdate(float time)
        {
            //int alive = 0;
            if (eventState != EventState.Done)
            {
                for (var i = 0; i < matchFactions.Count; ++i)
                {
                    var faction1 = matchFactions[i];
                    if (faction1.isAlive)
                    {
                        for (var j = i + 1; j < matchFactions.Count; ++j)
                        {
                            var faction2 = matchFactions[j];
                            if (faction2.isAlive && DssRef.world.diplomacy.GetRelation(faction1, faction2).Relation <= RelationType.RelationTypeN3_War)
                            {
                                return false;
                            }

                        }

                        //No alive opponents
                        MatchResult matchResult = new MatchResult();
                        foreach (var participant in matchFactions)
                        {
                            if (participant == faction1 ||
                                DssRef.world.diplomacy.GetRelation(faction1, participant).Relation >= RelationType.RelationType3_Ally)
                            {
                                matchResult.winner.Add(participant);
                            }
                            else
                            {
                                matchResult.loser.Add(participant);
                            }
                        }

                        Ref.update.AddSyncAction(new SyncAction3Arg<Interface.CutScene.GameEndReason, VictoryType, MatchResult>(
                            DssRef.state.events.triggerGameEnd, Interface.CutScene.GameEndReason.Complete, VictoryType.QuickMatchComplete, matchResult));

                        eventState = EventState.Done;
                        return true;
                        //DssRef.state.events.triggerGameEnd(Interface.CutScene.GameEndReason.Complete, VictoryType.QuickMatchComplete, matchResult);
                    }
                }
            }
            
            return eventState == EventState.Done;
        }

        public static List<Faction> Factions()
        {
            List<Faction> matchFactions = new List<Faction>(8);
            
            foreach (var p in DssRef.state.localPlayers)
            {
                matchFactions.Add(p.faction);
            }

            foreach (var ix in DssRef.world.quickMatchFactions)
            {
                var faction = DssRef.world.faction(ix);
                if (faction != null)
                {
                    matchFactions.Add(faction);
                }
            }

            return matchFactions;
        }
        protected override bool TimedEvent()
        {
            return false;
        }
        public override int OrderIndex()
        {
            return EventsOrder.DefeatTheBoss;
        }
    }

    static class EventsOrder
    {
        //Do NOT use for save
        public const int Tutorial = 0;
        public const int AiDelay = 1;
        public const int AiWarDelay = 2;
        public const int FirstAttack = 3;
        public const int WarmanagerDelay = 4;
        public const int Barbarians = 5;
        public const int Mercenaries = 6;
        public const int Cohalition = 7;
        public const int DarkLordWarning = 8;
        public const int DarkLord = 9;
        public const int DefeatTheBoss = 10;
        //public const int Factories = 7;
        //public const int FactoriesDestroyed = 8;
        //public const int DarkLordInPerson = 9;
        //public const int KillTheDarkLord = 10;

        public const int StoryEnd = 100;
    }

    /// <summary>
    /// Do NOT change index
    /// </summary>
    enum EventType
    {        
        Tutorial = 0,
        AiDelay = 1,
        AiWarDelay,
        FirstAttack,
        WarmanagerDelay,
        
        Barbarians,
        Mercenaries,
        Cohalition,
        BossWarning,
        Boss,
        DefeatTheBoss,
        QuickMatch,
        //Horde,
    }

    enum EventState
    {
        InQueue,
        //Prepare,
        //Countdown,
        //PowerChecked,
        PowerCheck_countdown,
        TriggerEvent_countdown,
        Done,
    }
}
