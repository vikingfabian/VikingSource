using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Map
{
    class UnitCollAreaGrid
    {
        public const int UnitGridSquareWidth = 8;

        public Grid2D<UnitCollArea> grid;

        public List<GameObject.SoldierGroup> groups_nearUpdate = new List<GameObject.SoldierGroup>(32);
        public List<GameObject.AbsGroup> groupsAndCities_nearUpdate = new List<GameObject.AbsGroup>(32);
        public List<GameObject.Army> armies_nearUpdate = new List<GameObject.Army>(8);
        public List<GameObject.AbsGroup> friendlyGroupsAndCities_nearUpdate = new List<GameObject.AbsGroup>(8);

        public List<GameObject.City> cities_aiUpdate = new List<GameObject.City>(8);
        public List<GameObject.Army> armies_aiUpdate = new List<GameObject.Army>(8);

        public List<GameObject.AbsMapObject> mapObjects_aiUpdate = new List<GameObject.AbsMapObject>(8);

        List<AbsMapObject> playerNearMapObjects = new List<AbsMapObject>();
        List<AbsSoldierUnit> playerNearDetailUnits = new List<AbsSoldierUnit>();

        


        //Dictionary<int, float> cityDominationStrength = new Dictionary<int, float>();

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

            //cityGrid = new Grid2D<GameObject.City>(grid.Size);
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

            //MOVE POINTERS
            for (int y = 0; y < grid.Size.Y; ++y)
            {
                for (int x = 0; x < grid.Size.X; ++x)
                {
                    grid.array[x, y].endProcess();
                }
            }
        }

        IntVector2 previousBattleGroupCheckTilePos = IntVector2.NegativeOne;
        List<AbsMapObject> battleGroupNearMapObjects = new List<AbsMapObject>();

        public List<AbsMapObject> BattleGroupNearMapObjects(IntVector2 tilePos, List<Faction> factions)
        {
            battleGroupNearMapObjects.Clear();
            
            if (tilePos != previousBattleGroupCheckTilePos)
            {
                previousBattleGroupCheckTilePos = tilePos;

                IntVector2 areaPos = tilePos / UnitGridSquareWidth;

                UnitCollArea area;

                for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
                {
                    for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
                    {
                        if (grid.TryGet(x, y, out area))
                        {
                            if (area.cities != null)
                            {
                                foreach (var m in area.cities)
                                {
                                    if (m.battleGroup == null &&
                                        m.tilePos.SideLength(tilePos) <= DssLib.BattleChainConflictRadius &&
                                        factions.Contains(m.faction))
                                    {
                                        battleGroupNearMapObjects.Add(m);
                                    }
                                }                                
                            }

                            lock (area.armies)
                            {
                                //var armies_sp = area.armies;
                                if (area.armies != null)
                                {
                                    foreach (var m in area.armies)
                                    {
                                        if (m.battleGroup == null &&
                                            m.tilePos.SideLength(tilePos) <= DssLib.BattleChainConflictRadius &&
                                            m.IdleObjetive() &&
                                           factions.Contains(m.faction))
                                        {
                                            battleGroupNearMapObjects.Add(m);
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }

            return battleGroupNearMapObjects;
        }

        public List<AbsMapObject> MapControlsWorkerCities(IntVector2 tilePos)
        {
            const int MinCityCount = 6;
            UnitCollArea area;
            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            playerNearMapObjects.Clear();
            checkArea(areaPos); //adding center tile

            int radius = 1;

            while (playerNearMapObjects.Count < MinCityCount)
            {
                ForXYEdgeLoop loop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(areaPos, radius));
                while (loop.Next())
                {
                    checkArea(loop.Position);
                }

                ++radius;
            }

            //if (false)
            //{
            //    int find = 138;
            //    //find specific city
            //    for (int y = 0; y < grid.Size.Y; ++y)
            //    {
            //        for (int x = 0; x < grid.Size.X; ++x)
            //        {
            //            foreach (var c in grid.array[x, y].cities)
            //            {
            //                if (c.parentArrayIndex == find)
            //                { 
            //                    lib.DoNothing();
            //                    break;
            //                }
            //            }
                       
            //        }
            //    }
            //}

            return playerNearMapObjects;

            void checkArea(IntVector2 pos)
            {
                if (grid.TryGet(pos, out area))
                {
                    playerNearMapObjects.AddRange(area.cities);
                }
            }
        }

        public List<AbsMapObject> MapControlsNearMapObjects(IntVector2 tilePos, bool controller)
        {
            playerNearMapObjects.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            if (grid.TryGet(areaPos.X, areaPos.Y, out area))
            {
                if (area.cities != null)
                {
                    playerNearMapObjects.AddRange(area.cities);
                }
                lock (area.armies)
                {
                    //var armies_sp = area.armies;
                    if (area.armies != null)
                    {
                        playerNearMapObjects.AddRange(area.armies);
                    }
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
                            if (area.cities != null)
                            {
                                playerNearMapObjects.AddRange(area.cities);
                            }

                            lock (area.armies)
                            {
                                //var armies_sp = area.armies;
                                if (area.armies != null)
                                {
                                    playerNearMapObjects.AddRange(area.armies);
                                }
                            }
                        }
                    }
                }
            }

            return playerNearMapObjects;
        }

        public List<AbsMapObject> MapControlsNearMapObjects_Workers(IntVector2 tilePos, bool controller)
        {
            playerNearMapObjects.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            //if (grid.TryGet(areaPos.X, areaPos.Y, out area))
            //{
            //    if (area.cities != null)
            //    {
            //        playerNearMapObjects.AddRange(area.cities);
            //    }
            //    var armies_sp = area.armies;
            //    if (armies_sp != null)
            //    {
            //        playerNearMapObjects.AddRange(armies_sp);
            //    }
            //}

            //if (!controller && playerNearMapObjects.Count > 0)
            //{
            //    return playerNearMapObjects;
            //}

            const int Radius = 3;

            for (int y = areaPos.Y - Radius; y <= areaPos.Y + Radius; ++y)
            {
                for (int x = areaPos.X - Radius; x <= areaPos.X + Radius; ++x)
                {
                    //if (x != areaPos.X || y != areaPos.Y)
                    //{
                        if (grid.TryGet(x, y, out area))
                        {
                            if (area.cities != null)
                            {
                                playerNearMapObjects.AddRange(area.cities);
                            }
                            var armies_sp = area.armies;
                            if (armies_sp != null)
                            {
                                playerNearMapObjects.AddRange(armies_sp);
                            }
                        }
                    //}
                }
            }

            return playerNearMapObjects;
        }

        public List<AbsSoldierUnit> MapControlsNearDetailUnits(IntVector2 tilePos)
        {
            playerNearDetailUnits.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            //if (grid.TryGet(areaPos.X, areaPos.Y, out area))
            //{
            //    var groups_sp = area.groups;
            //    if (groups_sp != null)
            //    {
            //        for (int i = 0; i < groups_sp.Count; ++i)
            //        {
            //            groups_sp[i].soldiers.toList(ref playerNearDetailUnits);
            //        }
            //    }
            //}

            //if (playerNearDetailUnits.Count > 0)
            //{
            //    return playerNearDetailUnits;
            //}

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
                                if (area.groups != null)
                                {
                                    for (int i = 0; i < area.groups.Count; ++i)
                                    {
                                        area.groups[i].soldiers.toList(ref playerNearDetailUnits);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return playerNearDetailUnits;
        }

        public List<GameObject.SoldierGroup> collectOpponentGroups(Faction faction, IntVector2 tilePos)
        {
            groups_nearUpdate.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
            {
                for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
                {
                    if (grid.TryGet(x, y, out area))
                    {
                        //var groups_sp = area.groups;
                        lock (area.groups)
                        {
                            if (area.groups != null)
                            {
                                foreach (var m in area.groups)
                                {
                                    if (DssRef.diplomacy.InWar(faction, m.army.faction))
                                    {
                                        groups_nearUpdate.Add(m);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return groups_nearUpdate;
        }

        public List<GameObject.AbsGroup> collectOpponents(Faction faction, IntVector2 tilePos)
        {
            groupsAndCities_nearUpdate.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
            {
                for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
                {
                    if (grid.TryGet(x, y, out area))
                    {
                        // var groups_sp = area.groups;
                        lock (area.groups)
                        {
                            if (area.groups != null)
                            {
                                foreach (var m in area.groups)
                                {
                                    if (m.army.faction != faction)
                                    {
                                        groupsAndCities_nearUpdate.Add(m);
                                    }
                                }
                            }
                        }

                        foreach (var city in area.cities)
                        {
                            if (city.faction != faction)
                            {
                                groupsAndCities_nearUpdate.Add(city);
                            }
                        }
                    }
                }
            }

            return groupsAndCities_nearUpdate;
        }

        public void collectMapObjectBattles(Faction faction, IntVector2 tilePos, ref List<AbsMapObject> units, bool collectCities)
        {
            units.Clear();

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
                            if (area.armies != null)
                            {
                                for (int aix = 0; aix < area.armies.Count; ++aix)
                                {
                                    var army = area.armies[aix];
                                    if (army.faction != faction &&
                                        DssRef.diplomacy.InWar(faction, army.faction))
                                    {
                                        units.Add(army);
                                    }
                                }
                            }
                        }

                        if (collectCities)
                        {
                            foreach (var city in area.cities)
                            {
                                if (city.faction != faction &&
                                    city.guardCount > 0 &&
                                    DssRef.diplomacy.InWar(faction, city.faction))
                                {
                                    units.Add(city);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void collectOpponentsAndFriendlies(Faction faction, IntVector2 tilePos,
            out List<GameObject.AbsGroup> opponents, out List<GameObject.AbsGroup> friendly)
        {
            groupsAndCities_nearUpdate.Clear();
            friendlyGroupsAndCities_nearUpdate.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
            {
                for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
                {
                    if (grid.TryGet(x, y, out area))
                    {
                        // var groups_sp = area.groups;
                        lock (area.groups)
                        {
                            if (area.groups != null)
                            {
                                foreach (var m in area.groups)
                                {
                                    if (m.army.faction == faction)
                                    {
                                        friendlyGroupsAndCities_nearUpdate.Add(m);
                                    }
                                    else
                                    {
                                        groupsAndCities_nearUpdate.Add(m);
                                    }
                                }
                            }
                        }

                        //var city = this.cityGrid.Get(x, y);
                        //if (collArea.city != null)// && city.faction != faction)
                        foreach (var city in area.cities)
                        {
                            if (city.faction == faction)
                            {
                                friendlyGroupsAndCities_nearUpdate.Add(city);
                            }
                            else
                            {
                                groupsAndCities_nearUpdate.Add(city);
                            }
                        }
                    }
                }
            }


            opponents = groupsAndCities_nearUpdate;
            friendly = friendlyGroupsAndCities_nearUpdate;
            //return groupsAndCities_nearUpdate;
        }

        

        //public Faction CityDomination(City city)
        //{
        //    cityDominationStrength.Clear();

        //    cityDominationStrength.Add(city.faction.index, 0);

        //    IntVector2 areaPos = city.tilePos / UnitGridSquareWidth;
        //    UnitCollArea area;

        //    for (int y = areaPos.Y - 1; y <= areaPos.Y + 1; ++y)
        //    {
        //        for (int x = areaPos.X - 1; x <= areaPos.X + 1; ++x)
        //        {
        //            if (grid.TryGet(x, y, out area))
        //            {
        //                var armies_sp = area.armies;
        //                if (armies_sp != null)
        //                {
        //                    foreach (var army in armies_sp)
        //                    {
        //                        bool inRange = (army.tilePos - city.tilePos).Length() <= DssLib.CityDominationRadius;

        //                        if (inRange)
        //                        {
        //                            if (cityDominationStrength.ContainsKey(army.faction.index))
        //                            {
        //                                cityDominationStrength[army.faction.index] += army.strengthValue;
        //                            }
        //                            else if (DssRef.diplomacy.InWar(army.faction, city.faction))
        //                            {
        //                                cityDominationStrength.Add(army.faction.index, army.strengthValue);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    int strongestFaction = -1;
        //    float strongest = float.MinValue;

        //    foreach (var kv in cityDominationStrength)
        //    {
        //        if (kv.Value > strongest)
        //        {
        //            strongestFaction = kv.Key;
        //            strongest = kv.Value;
        //        }
        //    }

        //    return DssRef.world.factions.Array[strongestFaction];
        //}

        public void collectArmies(Faction factionFilter, IntVector2 tilePos, int areaRadius,
            List<GameObject.Army> armies)
        {
            armies.Clear();
            //GameObject.Army prevArmy = null;

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
                            //var armies_sp = area.armies;
                            //var groups_sp = area.groups;
                            if (area.armies != null)
                            {
                                foreach (var m in area.armies)//crash (ändras i realtid)
                                {
                                    if (m.faction == factionFilter)
                                    {
                                        //prevArmy = m.army;
                                        if (!armies.Contains(m))
                                        {
                                            armies.Add(m);
                                        }
                                        //armies.Add(m);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        public void collectOpponentArmies(Faction faction, IntVector2 tilePos, int areaRadius,
            List<GameObject.Army> armies)
        {
            armies.Clear();
            GameObject.Army prevArmy = null;

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
                            //var groups_sp = area.groups;
                            if (area.groups != null)
                            {
                                foreach (var m in area.groups)//crash (ändras i realtid)
                                {
                                    if (m.army.faction != faction &&
                                        m.army != prevArmy)
                                    {
                                        prevArmy = m.army;
                                        if (!armies.Contains(m.army))
                                        {
                                            armies.Add(m.army);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public GameObject.Army AdjacenToArmy(Faction factionFilter, Army ignore, IntVector2 tilePos, float maxTileDistance)
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
                            if (area.armies != null)
                            {
                                for (int i = 0; i < area.armies.Count; ++i)
                                {
                                    var army = area.armies[i];
                                    if (army != ignore &&
                                        army.faction == factionFilter &&
                                        (army.tilePos - tilePos).Length() <= maxTileDistance)
                                    {
                                        return army;
                                    }
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

            grid.Get(areaPos).cities.Add(city);
        }

        //public void remove(GameObject.City city)
        //{
        //    IntVector2 areaPos = city.tilePos / UnitGridSquareWidth;

        //    grid.Get(areaPos).cities.Remove(city);
        //}

        public GameObject.City closestCity(IntVector2 tilePos)
        {
            //cities_aiUpdate.Clear();
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
                    foreach (var city in area.cities)
                    {
                        closest.Next(city.tilePos.Length(tilePos), city);
                    }
                }
            }
        }

        public void collectCities_fromArea(IntVector2 areaPos, int minCount,
            List<GameObject.City> nearCities, 
            Faction myFaction = null, Faction factionFilter = null)
        {
            if (factionFilter != null)
            {
                if (factionFilter.cities.Count < minCount)
                {
                    minCount = factionFilter.cities.Count;
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
                    foreach (var city in area.cities)
                    {
                        if (factionFilter != null)
                        {
                            if (city.faction == factionFilter)
                            {
                                nearCities.Add(city);
                            }
                        }
                        else if (myFaction != city.faction)
                        {
                            nearCities.Add(city);
                        }
                    }
                }
            }
        }

        public void collectCitiesAndArmies(IntVector2 areaPos, int goalCount, float maxStrengthValue,
            List<GameObject.AbsMapObject> nearMapObjects,
            Faction myFaction = null, Faction factionFilter = null)
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
                    foreach (var city in area.cities)
                    {
                        if (city.strengthValue + city.ai_armyDefenceValue <= maxStrengthValue)
                        {
                            if (factionFilter != null)
                            {
                                if (city.faction == factionFilter)
                                {
                                    nearMapObjects.Add(city);
                                }
                            }
                            else if (myFaction != city.faction)
                            {
                                nearMapObjects.Add(city);
                            }
                        }
                    }
                    lock (area.armies)
                    {
                        //var armies_sp = area.armies;
                        if (area.armies != null)
                        {
                            foreach (var army in area.armies)
                            {
                                if (army.strengthValue <= maxStrengthValue)
                                {
                                    if (factionFilter != null)
                                    {
                                        if (army.faction == factionFilter)
                                        {
                                            nearMapObjects.Add(army);
                                        }
                                    }
                                    else if (myFaction != army.faction)
                                    {
                                        nearMapObjects.Add(army);
                                    }
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

        public List<GameObject.City> cities = new List<City>(2);

        public void processAdd(GameObject.SoldierGroup group)
        {
            //if (processingGroups == null)
            //{
            //    processingGroups = new List<GameObject.SoldierGroup>(16);
            //}

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
                //if (processingGroups == null || processingGroups.Count == 0)
                //{
                //    processingGroups = null;
                //    groups = null;
                //}
                //else
                //{
                    var pointer = groups;
                    groups = processingGroups;
                    processingGroups = pointer;
                //}
            }

            lock (armies)
            {
                //if (processingArmies == null || processingArmies.Count == 0)
                //{
                //    processingArmies = null;
                //    armies = null;
                //}
                //else
                //{
                    var pointer = armies;
                    armies = processingArmies;
                    processingArmies = pointer;
                //}
            }
        }
    }

    //class UnitAreaCityCollector
    //{
    //    UnitCollAreaGrid grid;
        
        
    //    public UnitAreaCityCollector(UnitCollAreaGrid grid)
    //    {
    //        this.grid = grid;
            
    //    }

       

        

    //    //public void end()
    //    //{
    //    //    var loop = grid.grid.LoopInstance();

    //    //    while (loop.Next())
    //    //    {
    //    //        var clist = cities[loop.Position.X, loop.Position.Y];
    //    //        if (clist != null)
    //    //        {
    //    //            if (clist.Count > 1)
    //    //            {
    //    //                throw new Exception();
    //    //            }
    //    //            grid.grid.Get(loop.Position).city = clist.ToArray()[0];
    //    //        }
    //    //    }
    //    //}
    //}
}
