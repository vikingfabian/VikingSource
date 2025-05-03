using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                            calcAndRunEvent();
                            eventState = EventState.Done;
                        }
                        break;
                }
                
            }
            return eventState == EventState.Done;
        }

        virtual protected void calcAndRunEvent()
        {
            //switch (nextEvent)
            //{
            //    case EventType.SouthShips:
            //        {
            //            calcSouthSpawn();
            //        }
            //        break;
            //}

            //Ref.update.AddSyncAction(new SyncAction1Arg<EventType>(RunNextEvent_synced, nextEvent));
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
                    var citiesC = p.faction.cities.counter();
                    while (citiesC.Next())
                    {
                        totalWorkForce += citiesC.sel.HousingCount_Workers;
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

    class StoryEvent_AiDelay : AbsStoryEvent
    { 
        public StoryEvent_AiDelay()
        {
            
        }
        public override void onStart()
        {
            init(false, new TimeLength(DssRef.difficulty.aiDelayTimeSec));
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
            return 1;
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
            init(false, TimeLength.FromMinutes(15));
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
            return 2;
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
            init(false, TimeLength.FromMinutes(20));
        }
        public override bool RunWarManager()
        {
            return false;
        }
        public override int OrderIndex()
        {
            return 3;
        }
    }
    class StoryEvent_SouthShips : AbsStoryEvent
    {
        City[] playerMostSouthCity;
        IntVector2[] spawnPos_Player;

        public override EventType StoryEventType()
        {
            return Event.EventType.SouthShips;
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

        protected override void calcAndRunEvent()
        {
            calcSouthSpawn();

            Ref.update.AddSyncAction(new SyncAction(() =>
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

                //playerMostSouthCity = null;
                //spawnPos_Player = null;
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
            return 4;
        }
    }

    class StoryEvent_DarkLordWarning : AbsStoryEvent
    {
        public override EventType StoryEventType()
        {
            return Event.EventType.DarkLordWarning;
        }
        public override void onStart()
        {
            init(
             triggerTimeSpan_Minutes: new IntervalF(22f, 28f) * TimeExt.HourInMinutes,
             nextExpectedPlayerSize: new IntervalF(DssConst.HeadCityStartMaxWorkForce * 4f, DssConst.HeadCityStartMaxWorkForce * 8f));
        }

        protected override void calcAndRunEvent()
        {
            Ref.update.AddSyncAction(new SyncAction(() =>
            {
                foreach (var p in DssRef.state.localPlayers)
                {
                    p.hud.messages.Add(DssRef.lang.EventMessage_ProphesyTitle, DssRef.lang.EventMessage_ProphesyText);
                }
            }));
        }
    }

    class StoryEvent_DarkLord : AbsStoryEvent
    {
        List<Faction> darkLordAvailableFactions = null;
        List<Faction> darkLordAllies = null;

        public override EventType StoryEventType()
        {
            return Event.EventType.DarkLord;
        }
        public override void onStart()
        {
            init(false, TimeLength.FromHours(1f));
        }

        protected override void asyncPrepare(ref float time)
        {
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
                        factionC.sel.factiontype == FactionType.Barbarians ||
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
        }
        protected override void calcAndRunEvent()
        {
            Ref.update.AddSyncAction(new SyncAction(() =>
            {
                if (arraylib.HasMembers(darkLordAvailableFactions))
                {
                    DssRef.settings.darkLordPlayer.EnterMap(arraylib.RandomListMember(darkLordAvailableFactions), darkLordAllies);

                    //darkLordAllies = null;
                    //darkLordAvailableFactions = null;

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
    }

    class StoryEvent_Factories : AbsStoryEvent
    {
        public override EventType StoryEventType()
        {
            return Event.EventType.Factories;
        }

        protected override bool TimedEvent()
        {
            return false;
        }
    }

    class StoryEvent_FactoriesDestroyed : AbsStoryEvent
    {
        public override EventType StoryEventType()
        {
            return Event.EventType.FactoriesDestroyed;
        }

        protected override bool TimedEvent()
        {
            return false;
        }
    }

    class StoryEvent_DarkLordInPerson : AbsStoryEvent
    {
        public override EventType StoryEventType()
        {
            return Event.EventType.DarkLordInPerson;
        }
        protected override bool TimedEvent()
        {
            return false;
        }
    }
    class StoryEvent_KillTheDarkLord : AbsStoryEvent
    {
        public override EventType StoryEventType()
        {
            return Event.EventType.KillTheDarkLord;
        }

        public override void onStart()
        {
            Ref.update.AddSyncAction(new SyncAction(() =>
            {
                foreach (var p in DssRef.state.localPlayers)
                {
                    p.hud.messages.Add(DssRef.lang.EventMessage_FinalBattleTitle, DssRef.lang.EventMessage_FinalBattleText);
                }
            }));
        }

        protected override bool TimedEvent()
        {
            return false;
        }
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
