using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.EngineSpace.Graphics.In3D;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Map
{
    class UnitCollAreaGrid
    {
        public const int UnitGridSquareWidth = 8;

        public Grid2D<UnitCollArea> grid;

        public List<GameObject.SoldierGroup> groups_nearUpdate = new List<GameObject.SoldierGroup>(32);
        public List<City> cities_nearUpdate = new List<City>(4);
        public List<GameObject.AbsGroup> groupsAndCities_nearUpdate = new List<GameObject.AbsGroup>(32);
        public List<GameObject.AbsArmy> armies_nearUpdate = new List<GameObject.AbsArmy>(8);
        public List<GameObject.AbsGroup> friendlyGroupsAndCities_nearUpdate = new List<GameObject.AbsGroup>(8);

        public List<GameObject.City> cities_aiUpdate = new List<GameObject.City>(8);
        public List<GameObject.AbsArmy> armies_aiUpdate = new List<GameObject.AbsArmy>(8);

        public List<GameObject.AbsMapObject> mapObjects_aiUpdate = new List<GameObject.AbsMapObject>(8);

        List<AbsMapObject> playerNearMapObjects = new List<AbsMapObject>();
        List<AbsSoldierUnit> playerNearDetailUnits = new List<AbsSoldierUnit>();
        List<SoldierGroup> playerNearGroups = new List<SoldierGroup>();


        public UnitCollAreaGrid(IntVector2 worldSz)
        {
            if ((worldSz.X % UnitGridSquareWidth) != 0 ||
                (worldSz.Y % UnitGridSquareWidth) != 0)
            {
                throw new Exception();
            }

            grid = new Grid2D<UnitCollArea>(worldSz / UnitGridSquareWidth);

            grid.LoopBegin();

            while (grid.LoopNext())
            {
                grid.LoopValueSet(new UnitCollArea());
            }
        }

        public void asynchUpdate()
        {
            //CLEAR UP
            grid.LoopBegin();

            for (int y = 0; y < grid.Size.Y; ++y)
            {
                for (int x = 0; x < grid.Size.X; ++x)
                {
                    grid.array[x, y].beginProcess();
                }
            }

            //COLLECT
            var factions = DssRef.world.factions.counter();

            while (factions.Next())
            {
                var armies = factions.sel.armies.counter();
                while (armies.Next())
                {
                    IntVector2 armyArea = armies.sel.tilePos / UnitGridSquareWidth;
                    grid.array[armyArea.X, armyArea.Y].processAdd(armies.sel);

                    var groups = armies.sel.groups.counter();
                    while (groups.Next())
                    {
                        IntVector2 area = groups.sel.tilePos / UnitGridSquareWidth;
                        UnitCollArea collArea;
                        if (grid.TryGet(area, out collArea))
                        {
                            collArea.processAdd(groups.sel);
                        }
                    }
                }
            }

            foreach (var city in DssRef.world.cities)
            {
                var groups = city.groups.counter();
                while (groups.Next())
                {
                    IntVector2 area = groups.sel.tilePos / UnitGridSquareWidth;
                    UnitCollArea collArea;
                    if (grid.TryGet(area, out collArea))
                    {
                        collArea.processAdd(groups.sel);
                    }
                }
            }

            //MOVE POINTERS
            for (int y = 0; y < grid.Size.Y; ++y)
            {
                for (int x = 0; x < grid.Size.X; ++x)
                {
                    grid.array[x, y].endProcess();
                }
            }
        }

        //IntVector2 previousBattleGroupCheckTilePos = IntVector2.NegativeOne;
        //List<AbsMapObject> battleGroupNearMapObjects = new List<AbsMapObject>();

        //public List<AbsMapObject> BattleGroupNearMapObjects(IntVector2 tilePos, List<Faction> factions)
        //{
        //    battleGroupNearMapObjects.Clear();

        //    if (tilePos != previousBattleGroupCheckTilePos)
        //    {
        //        previousBattleGroupCheckTilePos = tilePos;

        //        IntVector2 areaPos = tilePos / UnitGridSquareWidth;

        //        UnitCollArea area;

        //        for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
        //        {
        //            for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
        //            {
        //                if (grid.TryGet(x, y, out area))
        //                {
        //                    if (area.cities != null)
        //                    {
        //                        foreach (var m in area.cities)
        //                        {
        //                            if (m.battleGroup == null &&
        //                                m.tilePos.SideLength(tilePos) <= DssLib.BattleChainConflictRadius &&
        //                                factions.Contains(m.faction))
        //                            {
        //                                battleGroupNearMapObjects.Add(m);
        //                            }
        //                        }                                
        //                    }

        //                    lock (area.armies)
        //                    {
        //                        //var armies_sp = area.armies;
        //                        if (area.armies != null)
        //                        {
        //                            foreach (var m in area.armies)
        //                            {
        //                                if (m.battleGroup == null &&
        //                                    m.tilePos.SideLength(tilePos) <= DssLib.BattleChainConflictRadius &&
        //                                    m.IdleObjetive() &&
        //                                   factions.Contains(m.faction))
        //                                {
        //                                    battleGroupNearMapObjects.Add(m);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //    }

        //    return battleGroupNearMapObjects;
        //}
        public List<AbsSoldierUnit> MapControlsNearDetailUnits(IntVector2 tilePos)
        {
            playerNearDetailUnits.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
            {
                for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
                {
                    //if (x != areaPos.X || y != areaPos.Y)
                    {
                        if (grid.TryGet(x, y, out area))
                        {
                            //var groups_sp = area.groups;
                            lock (area.groups)
                            {
                                
                                    for (int i = 0; i < area.groups.Count; ++i)
                                    {
                                        area.groups[i].soldiers?.toList(ref playerNearDetailUnits);
                                    }
                                
                            }
                        }
                    }
                }
            }

            return playerNearDetailUnits;
        }

        public List<AbsMapObject> MapControlsMultiselectMapObjects(IntVector2 tilePosStart, IntVector2 tilePosEnd, int faction)
        {
            //Debug.CrashIfThreaded();
            playerNearMapObjects.Clear();

            IntVector2 areaPosStart = tilePosStart / UnitGridSquareWidth;
            IntVector2 areaPosEnd = tilePosEnd / UnitGridSquareWidth;

            areaPosStart -= IntVector2.One;
            areaPosEnd += IntVector2.One;


            UnitCollArea area;

            for (int y = areaPosStart.Y; y <= areaPosEnd.Y; ++y)
            {
                for (int x = areaPosStart.X; x <= areaPosEnd.X; ++x)
                {  
                    if (grid.TryGet(x, y, out area))
                    {
                        lock (area.armies)
                        {
                            
                                foreach (AbsMapObject obj in area.armies)
                                {
                                if (obj.factionIndex == faction)
                                {
                                    playerNearMapObjects.Add(obj);
                                }
                                }
                            
                        }
                    }
                }
            }

            return playerNearMapObjects;
        }


        public bool PlayerInBattle(IntVector2 tilePos, int playerFaction)
        {
            //Debug.CrashIfThreaded();
            //playerNearMapObjects.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
            {
                for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
                {
                    if (grid.TryGet(x, y, out area))
                    {
                        //foreach (var cityIx in area.cities)
                        //{
                        //    var city = DssRef.world.cities[area.cities[i]];
                        //    if (city.detailObj.inBattle != null && city.faction == player)
                        //    {
                        //        return true;
                        //    }
                        //}

                        lock (area.groups)
                        {
                            foreach (var group in area.groups)
                            { 
                                if (group.attackTarget_soldierGroupOrCity != null && group.factionIndex == playerFaction)
                                {
                                    return true;
                                }
                            }

                        }
                    }

                }
            }

            return false;
        }

        public List<AbsMapObject> MapControlsNearMapObjects(IntVector2 tilePos, bool controller)
        {
            //Debug.CrashIfThreaded();
            playerNearMapObjects.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            if (grid.TryGet(areaPos.X, areaPos.Y, out area))
            {
                for (int i = 0; i < area.cities.Count; ++i)//each (var cityIx in area.cities)
                {
                    playerNearMapObjects.Add(DssRef.world.cities[area.cities[i]]);
                }
                lock (area.armies)
                {
                    //var armies_sp = area.armies;
                    
                        playerNearMapObjects.AddRange(area.armies);
                    
                }
            }

            if (!controller && playerNearMapObjects.Count > 0)
            { 
                return playerNearMapObjects;
            }

            for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
            {
                for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
                {
                    if (x != areaPos.X || y != areaPos.Y)
                    {
                        if (grid.TryGet(x, y, out area))
                        {
                            for (int i = 0; i < area.cities.Count; ++i)//foreach (var cityIx in area.cities)
                            {
                                playerNearMapObjects.Add(DssRef.world.cities[area.cities[i]]);
                            }

                            lock (area.armies)
                            {
                                //var armies_sp = area.armies;
                                
                                    playerNearMapObjects.AddRange(area.armies);
                                
                            }
                        }
                    }
                }
            }

            return playerNearMapObjects;
        }

        public List<AbsMapObject> MapControlsNearMapObjects_Workers(IntVector2 tilePos, bool controller)
        {
            //Debug.CrashIfThreaded();
            playerNearMapObjects.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            const int Radius = 3;

            for (int y = areaPos.Y - Radius; y <= areaPos.Y + Radius; ++y)
            {
                for (int x = areaPos.X - Radius; x <= areaPos.X + Radius; ++x)
                {
                    //if (x != areaPos.X || y != areaPos.Y)
                    //{
                    if (grid.TryGet(x, y, out area))
                    {

                        for (int i = 0; i < area.cities.Count; ++i)//foreach (var cityIx in area.cities)
                        {
                            playerNearMapObjects.Add(DssRef.world.cities[area.cities[i]]);
                        }
                        lock (area.armies)
                        {
                            var armies_sp = area.armies;
                            if (armies_sp != null)
                            {
                                playerNearMapObjects.AddRange(armies_sp);
                            }
                        }
                    }
                    //}
                }
            }

            return playerNearMapObjects;
        }

        public List<SoldierGroup> MapControlsNearGroups_Rectangle(IntVector2 tilePosStart, IntVector2 tilePosEnd, Faction faction,
            ScreenToSpaceRectangleBound rectangle)
        {
            playerNearGroups.Clear();

            IntVector2 areaPosStart = tilePosStart / UnitGridSquareWidth;
            IntVector2 areaPosEnd = tilePosEnd / UnitGridSquareWidth;

            UnitCollArea area;
            areaPosStart.Add(-1);
            areaPosEnd.Add(1);

            for (int y = areaPosStart.Y; y <= areaPosEnd.Y; ++y)
            {
                for (int x = areaPosStart.X; x <= areaPosEnd.X; ++x)
                {
                    if (grid.TryGet(x, y, out area))
                    {
                        //var groups_sp = area.groups;
                        lock (area.groups)
                        {
                            
                                for (int i = 0; i < area.groups.Count; ++i)
                                {
                                    if (area.groups[i].GetFaction() == faction &&
                                        area.groups[i].rectangleCollision(rectangle))
                                    {
                                        playerNearGroups.Add(area.groups[i]);
                                    }
                                }
                            
                        }
                    }
                }
            }

            return playerNearGroups;
        }


        public Faction cityCaptureCheck(City city, int radius)
        {
            IntVector2 areaStart = (city.tilePos - radius) / UnitGridSquareWidth;
            IntVector2 areaEnd = (city.tilePos + radius) / UnitGridSquareWidth;
            
            Dictionary<int, float> faction_power = new Dictionary<int, float>();
            //faction_power.Add(city.faction.parentArrayIndex, 0);

            for (int arY = areaStart.Y; arY <= areaEnd.Y; ++arY)
            {
                for (int arX = areaStart.X; arX <= areaEnd.X; ++arX)
                {
                    if (grid.TryGet(arX, arY, out UnitCollArea area))
                    {
                        lock (area.groups)
                        {
                            
                                foreach (var m in area.groups)
                                {
                                    if (m.tilePos.SideLength(city.tilePos) <= radius)
                                    {
                                        if (city.factionIndex == m.factionIndex ||
                                            DssRef.diplomacy.InWar(city.factionIndex, m.factionIndex))
                                        {
                                            if (faction_power.TryGetValue(m.factionIndex, out float strength))
                                            {
                                                faction_power[m.factionIndex] = strength + m.strengthValue();
                                            }
                                            else
                                            {
                                                faction_power.Add(m.factionIndex, m.strengthValue());
                                            }
                                        }
                                    }
                                }
                            
                        }
                    }
                }
            }

            int strongest = city.factionIndex;
            float strongestValue = 0;

            foreach (var kv in faction_power)
            {
                if (kv.Value > strongestValue)
                {
                    strongest = kv.Key;
                    strongestValue = kv.Value;
                }
            }

            return DssRef.world.factions.Array[strongest];
        }

        public void collectOpponentGroups(int faction, IntVector2 tilePos, out List<GameObject.SoldierGroup> groups, out List<City> cities)
        {
            groups_nearUpdate.Clear();
            cities_nearUpdate.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
            {
                for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
                {
                    if (grid.TryGet(x, y, out area))
                    {
                        lock (area.groups)
                        {                              
                            foreach (var m in area.groups)
                            {
                                if (DssRef.diplomacy.InWar(faction, m.factionIndex))
                                {
                                    groups_nearUpdate.Add(m);
                                }
                            }                            
                        }

                        for (int i = 0; i < area.cities.Count; ++i)//foreach (var cityIx in area.cities)
                        {
                            var city = DssRef.world.cities[area.cities[i]];
                            if (DssRef.diplomacy.InWar(faction, city.factionIndex))
                            {
                                var groupsC = city.groups.counter();
                                while (groupsC.Next())
                                {
                                    groups_nearUpdate.Add(groupsC.sel);
                                }
                            }
                        }
                    }
                }
            }

            groups = groups_nearUpdate;
            cities = cities_nearUpdate;
        }

        //public List<GameObject.AbsGroup> collectOpponents(int faction, IntVector2 tilePos)
        //{
        //    groupsAndCities_nearUpdate.Clear();

        //    IntVector2 areaPos = tilePos / UnitGridSquareWidth;
        //    UnitCollArea area;

        //    for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
        //    {
        //        for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
        //        {
        //            if (grid.TryGet(x, y, out area))
        //            {
        //                // var groups_sp = area.groups;
        //                lock (area.groups)
        //                {
        //                    if (area.groups != null)
        //                    {
        //                        foreach (var m in area.groups)
        //                        {
        //                            if (m.army.faction != faction)
        //                            {
        //                                groupsAndCities_nearUpdate.Add(m);
        //                            }
        //                        }
        //                    }
        //                }

        //                for (int i = 0; i < area.cities.Count; ++i)//foreach (var cityIx in area.cities)
        //                {
        //                    var city = DssRef.world.cities[area.cities[i]];
        //                    if (city.faction != faction)
        //                    {
        //                        groupsAndCities_nearUpdate.Add(city);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return groupsAndCities_nearUpdate;
        //}

        //public void collectMapObjectBattles(Faction faction, IntVector2 tilePos, ref List<AbsMapObject> units, bool collectCities)
        //{
        //    units.Clear();

        //    IntVector2 areaPos = tilePos / UnitGridSquareWidth;
        //    UnitCollArea area;

        //    for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
        //    {
        //        for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
        //        {
        //            if (grid.TryGet(x, y, out area))
        //            {
        //                lock (area.armies)
        //                {
        //                    //var armies_sp = area.armies;
        //                    if (area.armies != null)
        //                    {
        //                        for (int aix = 0; aix < area.armies.Count; ++aix)
        //                        {
        //                            var army = area.armies[aix];
        //                            if (army.faction != faction &&
        //                                DssRef.diplomacy.InWar(faction, army.faction))
        //                            {
        //                                units.Add(army);
        //                            }
        //                        }
        //                    }
        //                }

        //                if (collectCities)
        //                {
        //                    for (int i = 0; i < area.cities.Count; ++i)//foreach (var cityIx in area.cities)
        //                    {
        //                        var city = DssRef.world.cities[area.cities[i]];
        //                        if (city.faction != faction &&
        //                            //city.guardCount > 0 &&
        //                            DssRef.diplomacy.InWar(faction, city.faction))
        //                        {
        //                            units.Add(city);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //public void collectOpponentsAndFriendlies(Faction faction, IntVector2 tilePos,
        //    out List<GameObject.AbsGroup> opponents, out List<GameObject.AbsGroup> friendly)
        //{
        //    groupsAndCities_nearUpdate.Clear();
        //    friendlyGroupsAndCities_nearUpdate.Clear();

        //    IntVector2 areaPos = tilePos / UnitGridSquareWidth;
        //    UnitCollArea area;

        //    for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
        //    {
        //        for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
        //        {
        //            if (grid.TryGet(x, y, out area))
        //            {
        //                // var groups_sp = area.groups;
        //                lock (area.groups)
        //                {
        //                    if (area.groups != null)
        //                    {
        //                        foreach (var m in area.groups)
        //                        {
        //                            if (m.army.faction == faction)
        //                            {
        //                                friendlyGroupsAndCities_nearUpdate.Add(m);
        //                            }
        //                            else
        //                            {
        //                                groupsAndCities_nearUpdate.Add(m);
        //                            }
        //                        }
        //                    }
        //                }

        //                for (int i = 0; i < area.cities.Count; ++i)//foreach (var cityIx in area.cities)
        //                {
        //                    var city = DssRef.world.cities[area.cities[i]];
        //                    if (city.faction == faction)
        //                    {
        //                        friendlyGroupsAndCities_nearUpdate.Add(city);
        //                    }
        //                    else
        //                    {
        //                        groupsAndCities_nearUpdate.Add(city);
        //                    }
        //                }
        //            }
        //        }
        //    }


        //    opponents = groupsAndCities_nearUpdate;
        //    friendly = friendlyGroupsAndCities_nearUpdate;
        //}

        public void collectGroups(IntVector2 tilePos,
           ref List<GameObject.AbsGroup> groups, bool cities)
        {
            groups.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
            {
                for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
                {
                    if (grid.TryGet(x, y, out area))
                    {
                        lock (area.groups)
                        {
                            
                                groups.AddRange(area.groups);
                            
                        }

                        for (int i = 0; i < area.cities.Count; ++i)//foreach (var cityIx in area.cities)
                        {
                            var city = DssRef.world.cities[area.cities[i]];
                            var groupsC = city.groups.counter();
                            while (groupsC.Next())
                            {
                                groups.Add(groupsC.sel);
                            }
                        }
                    }
                }
            }
        }


        public void collectArmies(IntVector2 tilePos, List<GameObject.AbsArmy> armies)
        {
            armies.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
            {
                for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
                {
                    if (grid.TryGet(x, y, out area))
                    {
                        lock (area.armies)
                        {
                           
                                foreach (var m in area.armies)
                                {
                                    if (!armies.Contains(m))
                                    {
                                        armies.Add(m);
                                    }                                    
                                }
                            
                        }
                    }
                }
            }
        }

        public void net_collectArmies(Rectangle2 mapTileArea, List<GameObject.Army> armies)
        {
            armies.Clear();
            IntVector2 areaStart = mapTileArea.pos / UnitGridSquareWidth;
            IntVector2 areaEnd = mapTileArea.BottomRightTile / UnitGridSquareWidth;

            for (int arY = areaStart.Y; arY <= areaEnd.Y; ++arY)
            {
                for (int arX = areaStart.X; arX <= areaEnd.X; ++arX)
                {
                    if (grid.TryGet(arX, arY, out UnitCollArea area))
                    {
                        lock (area.armies)
                        {
                            //Todo dont add remote player armies
                            
                                foreach (var m in area.armies)
                                {
                                    if (!armies.Contains(m))
                                    {
                                        armies.Add(m);
                                    }
                                }
                            
                        }
                    }
                }
            }
        }

        public void collectArmies(int factionFilter, IntVector2 tilePos, int areaRadius,
            List<GameObject.AbsArmy> armies)
        {
            armies.Clear();
            
            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            for (int y = areaPos.Y - areaRadius; y <= areaPos.Y + areaRadius; ++y)
            {
                for (int x = areaPos.X - areaRadius; x <= areaPos.X + areaRadius; ++x)
                {
                    if (grid.TryGet(x, y, out area))
                    {
                        lock (area.armies)
                        {
                            
                                foreach (var m in area.armies)
                                {
                                    if (m.factionIndex == factionFilter)
                                    {
                                        if (!armies.Contains(m))
                                        {
                                            armies.Add(m);
                                        }
                                    }
                                }
                            
                        }
                    }
                }
            }
        }


        public void collectOpponentArmies(int faction, IntVector2 tilePos, int areaRadius,
            List<GameObject.AbsArmy> armies)
        {
            armies.Clear();
            GameObject.AbsArmy prevArmy = null;

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            for (int y = areaPos.Y - areaRadius; y <= areaPos.Y + areaRadius; ++y)
            {
                for (int x = areaPos.X - areaRadius; x <= areaPos.X + areaRadius; ++x)
                {
                    if (grid.TryGet(x, y, out area))
                    {
                        lock (area.groups)
                        {
                            
                                foreach (var m in area.groups)
                                {
                                    if (m.factionIndex != faction && !RefExt.EqTarget(prevArmy, m.army))
                                        /*prevArmy != m.army*/
                                    {
                                        prevArmy = m.GetAbsArmy();
                                        if (prevArmy != null && !armies.Contains(prevArmy))
                                        {
                                            armies.Add(prevArmy);
                                        }
                                    }
                                }
                            
                        }
                    }
                }
            }
        }

        public GameObject.Army AdjacenToArmy(int factionFilter, Army ignore, IntVector2 tilePos, float maxTileDistance)
        {
            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
            {
                for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
                {
                    if (grid.TryGet(x, y, out area))
                    {
                        lock (area.armies)
                        {
                            //var armies_sp = area.armies;
                           
                                for (int i = 0; i < area.armies.Count; ++i)
                                {
                                    var army = area.armies[i];
                                    if (army != ignore &&
                                        army.factionIndex == factionFilter &&
                                        (army.tilePos - tilePos).Length() <= maxTileDistance)
                                    {
                                        return army;
                                    }
                                }
                            
                        }
                    }
                }
            }

            return null;
        }

        public void add(GameObject.City city)
        {
            IntVector2 areaPos = city.tilePos / UnitGridSquareWidth;

            grid.Get(areaPos).cities.Add(city.myIndex);
        }


        public GameObject.City closestCity(IntVector2 tilePos)
        {
            IntVector2 areaPos = tilePos / UnitGridSquareWidth;

            FindMinValuePointer<GameObject.City> closest = new FindMinValuePointer<GameObject.City>();
            
            checkArea(areaPos); //adding center tile

            int radius = 1;

            do
            {
                ForXYEdgeLoop loop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(areaPos, radius));
                while (loop.Next())
                {
                    checkArea(loop.Position);
                }

                ++radius;
            } while (closest.minMember == null);

            return closest.minMember;

            void checkArea(IntVector2 pos)
            {
                UnitCollArea area;
                if (grid.TryGet(pos, out area))
                {
                    for (int i = 0; i < area.cities.Count; ++i)//foreach (var cityIx in area.cities)
                    {
                        var city = DssRef.world.cities[area.cities[i]];
                        closest.Next(city.tilePos.Length(tilePos), city);
                    }
                }
            }
        }

        public void collectCities_fromArea(IntVector2 areaPos, int minCount,
            List<GameObject.City> nearCities, 
            int myFaction = -1, int factionFilter = -1)
        {
            if ( factionFilter >=0)
            {
                var pFilter = DssRef.world.faction(factionFilter);
                if (pFilter != null && pFilter.cities.Count < minCount)
                {
                    minCount = pFilter.cities.Count;
                }
            }

            UnitCollArea area;
            nearCities.Clear();

            checkArea(areaPos); //adding center tile

            int radius = 1;

            while (nearCities.Count < minCount)
            {
                ForXYEdgeLoop loop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(areaPos, radius));
                while (loop.Next())
                {
                    checkArea(loop.Position);
                }

                ++radius;
            }

            void checkArea(IntVector2 pos)
            {
                if (grid.TryGet(pos, out area))
                {
                    for (int i = 0; i < area.cities.Count; ++i)//foreach (var cityIx in area.cities)
                    {
                        var city = DssRef.world.cities[area.cities[i]];
                        if (factionFilter >= 0)
                        {
                            if (city.factionIndex == factionFilter)
                            {
                                nearCities.Add(city);
                            }
                        }
                        else if (myFaction != city.factionIndex)
                        {
                            nearCities.Add(city);
                        }
                    }
                }
            }
        }

        public void collectCitiesAndArmies(IntVector2 areaPos, int goalCount, float maxStrengthValue,
            List<GameObject.AbsMapObject> nearMapObjects,
            int myFaction = -1, int factionFilter = -1)
        {

            UnitCollArea area;
            nearMapObjects.Clear();

            checkArea(areaPos); //adding center tile

            int radius = 1;

            while (nearMapObjects.Count < goalCount && radius <= 5)
            {
                ForXYEdgeLoop loop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(areaPos, radius));
                while (loop.Next())
                {
                    checkArea(loop.Position);
                }

                ++radius;
            }

            void checkArea(IntVector2 pos)
            {
                if (grid.TryGet(pos, out area))
                {
                    for (int i = 0; i < area.cities.Count; ++i)//foreach (var cityIx in area.cities)
                    {
                        var city = DssRef.world.cities[area.cities[i]];
                        if (city.strengthValue + city.ai_armyDefenceValue <= maxStrengthValue)
                        {
                            if (factionFilter >= 0)
                            {
                                if (city.factionIndex == factionFilter)
                                {
                                    nearMapObjects.Add(city);
                                }
                            }
                            else if (myFaction != city.factionIndex)
                            {
                                nearMapObjects.Add(city);
                            }
                        }
                    }
                    lock (area.armies)
                    {
                        
                            foreach (var army in area.armies)
                            {
                                if (army.strengthValue <= maxStrengthValue)
                                {
                                    if (factionFilter >= 0)
                                    {
                                        if (army.factionIndex == factionFilter)
                                        {
                                            nearMapObjects.Add(army);
                                        }
                                    }
                                    else if (myFaction != army.factionIndex)
                                    {
                                        nearMapObjects.Add(army);
                                    }
                                }
                            }
                        
                    }
                }
            }
        }

        public static IntVector2 ToAreaPos(IntVector2 tilePos)
        {
            return tilePos / UnitGridSquareWidth;
        }
    } 

    class UnitCollArea
    {
        public List<GameObject.SoldierGroup> processingGroups = new List<GameObject.SoldierGroup>(16);
        public List<GameObject.SoldierGroup> groups = new List<GameObject.SoldierGroup>(16);

        public List<GameObject.Army> processingArmies = new List<GameObject.Army>(2);//null;
        public List<GameObject.Army> armies = new List<GameObject.Army>(2);//null;

        public List<int> cities = new List<int>(2);

        public void processAdd(GameObject.SoldierGroup group)
        {
            processingGroups.Add(group);
        }

        public void processAdd(GameObject.Army army)
        {
            if (processingArmies == null)
            {
                processingArmies = new List<GameObject.Army>(2);
            }

            processingArmies.Add(army);
        }

        public void beginProcess()
        {
            processingGroups?.Clear();
            processingArmies?.Clear();
        }

        public void endProcess()
        {
            processingGroups.TrimExcess();

            lock (groups)
            {
                var pointer = groups;
                groups = processingGroups;
                processingGroups = pointer;
            }

            lock (armies)
            {
                var pointer = armies;
                armies = processingArmies;
                processingArmies = pointer;
            }
        }
    }


    struct SoldierGroupId
    {
        public int faction;
        public int army;
        public int group;
    }
    struct ArmyId
    {
        public int faction;
        public int army;
        public ArmyId(Army army)
        {
            faction = army.factionIndex;
            this.army = army.myIndex;
        }

    }
}
