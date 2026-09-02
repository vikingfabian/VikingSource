using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
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
        public List<AbsArmy> armies_nearUpdate = new List<AbsArmy>(8);
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

        public List<AbsSoldierUnit> MapControlsNearDetailUnits(IntVector2 tilePos)
        {
            playerNearDetailUnits.Clear();

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
                                
                                for (int i = 0; i < area.groups.Count; ++i)
                                {
                                    area.groups[i].GetSoldierGroup(out _)?.soldiers?.toList(ref playerNearDetailUnits);
                                }
                                
                        }
                    }                    
                }
            }

            return playerNearDetailUnits;
        }

        public void netSubTilesRecieved(IntVector2 tilePos)
        {            
            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;
            
            if (grid.TryGet(areaPos, out area))
            {
                lock (area.groups)
                {
                    for (int i = 0; i < area.groups.Count; ++i)
                    {
                        var soldiers_sp = area.groups[i].GetSoldierGroup(out _)?.soldiers;
                        if (soldiers_sp != null)
                        {
                            var soldiersC = soldiers_sp.counter();
                            while (soldiersC.Next() && soldiersC.sel.tilePos == tilePos)
                            {
                                new Timer.TimedAction1ArgTrigger<bool>(soldiersC.sel.updateGroudY, true, 3000);                                 
                            }
                        }
                    }
                }
            }
        }

        public void netTilesRecieved(IntVector2 tilePos)
        {
            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            if (grid.TryGet(areaPos, out area))
            {
                lock (area.armies)
                {
                    foreach (PArmy obj in area.armies)
                    {
                        if (obj.TryGetArmy(out var army))
                        {
                            army.updateModelsPosition();
                        }
                    }
                }
            }
        }

        public List<AbsMapObject> MapControlsMultiselectMapObjects(IntVector2 tilePosStart, IntVector2 tilePosEnd, PFaction faction)
        {
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
                            foreach (PArmy obj in area.armies)
                            {
                                if (obj.pfaction == faction)
                                {
                                    if (obj.TryGetArmy(out var army))
                                    {
                                        playerNearMapObjects.Add(army);
                                    }
                                }
                            }                            
                        }
                    }
                }
            }

            return playerNearMapObjects;
        }


        public bool PlayerInBattle(IntVector2 tilePos, PFaction playerFaction)
        {
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
                            foreach (var pgroup in area.groups)
                            {
                                var group = pgroup.GetSoldierGroup(out _);
                                if (group != null && group.attackTarget_soldierGroupOrCity.HasValue() && group.pfaction == playerFaction)
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
            playerNearMapObjects.Clear();

            IntVector2 areaPos = tilePos / UnitGridSquareWidth;
            UnitCollArea area;

            if (grid.TryGet(areaPos.X, areaPos.Y, out area))
            {
                for (int i = 0; i < area.cities.Count; ++i)
                {
                    playerNearMapObjects.Add(DssRef.world.cities[area.cities[i]]);
                }
                lock (area.armies)
                {
                    foreach (var pa in area.armies)
                    {
                        if (pa.TryGetArmy(out var army))
                        {
                            playerNearMapObjects.Add(army);
                        }
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
                            for (int i = 0; i < area.cities.Count; ++i)
                            {
                                playerNearMapObjects.Add(DssRef.world.cities[area.cities[i]]);
                            }

                            lock(area.armies)
                            {
                                foreach (var pa in area.armies)
                                {
                                    if (pa.TryGetArmy(out var army))
                                    {
                                        playerNearMapObjects.Add(army);
                                    }
                                }

                            }
                        }
                    }
                }
            }

            return playerNearMapObjects;
        }

        public List<AbsMapObject> MapControlsNearMapObjects_PlusWorkers(IntVector2 tilePos, bool controller)
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
                    if (grid.TryGet(x, y, out area))
                    {
                        for (int i = 0; i < area.cities.Count; ++i)//foreach (var cityIx in area.cities)
                        {
                            playerNearMapObjects.Add(DssRef.world.cities[area.cities[i]]);
                        }
                        lock (area.armies)
                        {
                            //var armies_sp = area.armies;
                            foreach (var pa in area.armies)
                            {
                                if (pa.TryGetArmy(out var army))
                                {
                                    playerNearMapObjects.Add(army);
                                }
                            }

                        }
                    }
                    //}
                }
            }

            return playerNearMapObjects;
        }

        public List<SoldierGroup> MapControlsNearGroups_Rectangle(IntVector2 tilePosStart, IntVector2 tilePosEnd, PFaction faction,
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
                                if (area.groups[i].pabsarmy.pfaction == faction &&
                                    area.groups[i].TryGetSoldierGroup(out var group) &&
                                    group.rectangleCollision(rectangle))
                                {
                                    playerNearGroups.Add(group);
                                }
                            }
                            
                        }
                    }
                }
            }

            return playerNearGroups;
        }


        public PFaction cityCaptureCheck(City city, int radius)
        {
            IntVector2 areaStart = (city.tilePos - radius) / UnitGridSquareWidth;
            IntVector2 areaEnd = (city.tilePos + radius) / UnitGridSquareWidth;
            
            Dictionary<PFaction, float> faction_power = new Dictionary<PFaction, float>();
            //faction_power.Add(city.faction.parentArrayIndex, 0);

            for (int arY = areaStart.Y; arY <= areaEnd.Y; ++arY)
            {
                for (int arX = areaStart.X; arX <= areaEnd.X; ++arX)
                {
                    if (grid.TryGet(arX, arY, out UnitCollArea area))
                    {
                        lock (area.groups)
                        {
                            
                                foreach (var pSoldierGroup in area.groups)
                                {
                                    var m = pSoldierGroup.GetSoldierGroup(out _);
                                    if (m != null && m.tilePos.SideLength(city.tilePos) <= radius)
                                    {
                                        if (city.pfaction == m.pfaction ||
                                            DssRef.world.diplomacy.GetRelation(city.pfaction, m.pfaction).InWar())
                                        {
                                            if (faction_power.TryGetValue(m.pfaction, out float strength))
                                            {
                                                faction_power[m.pfaction] = strength + m.strengthValue();
                                            }
                                            else
                                            {
                                                faction_power.Add(m.pfaction, m.strengthValue());
                                            }
                                        }
                                    }
                                }
                            
                        }
                    }
                }
            }

            PFaction strongest = city.pfaction;
            float strongestValue = 0;

            foreach (var kv in faction_power)
            {
                if (kv.Value > strongestValue)
                {
                    strongest = kv.Key;
                    strongestValue = kv.Value;
                }
            }

            return strongest;
        }

        public void collectOpponentGroups(PFaction faction, IntVector2 tilePos, out List<GameObject.SoldierGroup> groups, out List<City> cities)
        {
            //lock (groups_nearUpdate)
            //{
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
                                    if ( DssRef.world.diplomacy.GetRelation(faction, m.pabsarmy.pfaction).InWar() &&
                                        m.TryGetSoldierGroup(out var group))
                                    {                                       
                                        groups_nearUpdate.Add(group);
                                    }
                                }
                            }

                            for (int i = 0; i < area.cities.Count; ++i)
                            {
                                var city = DssRef.world.cities[area.cities[i]];
                                if (DssRef.world.diplomacy.GetRelation(faction, city.pfaction).InWar())
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
                
            //}
        }

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
                            foreach (var pgroup in area.groups) 
                            {
                                if (pgroup.TryGetSoldierGroup(out var group))
                                {
                                    groups.Add(group);
                                }
                            }
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


        //public void collectArmies(IntVector2 tilePos, List<PArmy> armies)
        //{
        //    armies.Clear();

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
        //                    foreach (var m in area.armies)
        //                    {
        //                        if (!armies.Contains(m))
        //                        {
        //                            armies.Add(m);
        //                        }                                    
        //                    }                            
        //                }
        //            }
        //        }
        //    }
        //}
        public void collectArmies(IntVector2 tilePos, List<AbsArmy> armies)
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
                            foreach (var areaPArmy in area.armies)
                            {
                                if (areaPArmy.TryGetArmy(out Army areaArmy))
                                {
                                    if (!armies.Contains(areaArmy))
                                    {
                                        armies.Add(areaArmy);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void net_collectArmies(Rectangle2 mapTileArea, List<AbsArmy> armies)
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
                                    if (m.TryGetArmy(out Army areaArmy)
                                        && !armies.Contains(areaArmy))
                                    {
                                        armies.Add(areaArmy);
                                    }
                                }
                            
                        }
                    }
                }
            }
        }

        public void collectArmies(PFaction factionFilter, IntVector2 tilePos, int areaRadius,
            List<AbsArmy> armies)
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
                                    if (m.pfaction == factionFilter && 
                                        m.TryGetArmy(out var areaArmy))
                                    {
                                        if (!armies.Contains(areaArmy))
                                        {
                                            armies.Add(areaArmy);
                                        }
                                    }
                                }
                            
                        }
                    }
                }
            }
        }


        public void collectOpponentArmies(PFaction faction, IntVector2 tilePos, int areaRadius,
            List<AbsArmy> armies)
        {
            armies.Clear();
            PMapObject prevPArmy = PMapObject.Empty;

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
                            
                                foreach (var pgroup in area.groups)
                                {
                                    if (pgroup.pabsarmy.pfaction != faction && prevPArmy != pgroup.pabsarmy)
                                        /*prevArmy != m.army*/
                                    {
                                        prevPArmy = pgroup.pabsarmy;
                                        if (prevPArmy.TryGetAbsArmy(out var prevArmy) && 
                                        
                                            !armies.Contains(prevArmy))
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

        public GameObject.Army AdjacenToArmy(PFaction factionFilter, PArmy ignore, IntVector2 tilePos, float maxTileDistance)
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
                                    var parmy = area.armies[i];
                                    if (parmy != ignore &&
                                        parmy.pfaction == factionFilter &&
                                        parmy.TryGetArmy(out var army) &&
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
            PFaction myFaction, PFaction factionFilter)
        {
            if ( factionFilter.TryGetFaction(out var pFilter))
            {
                //var pFilter = factionFilter.;
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
                        if (factionFilter.HasValue())
                        {
                            if (city.pfaction == factionFilter)
                            {
                                nearCities.Add(city);
                            }
                        }
                        else if (myFaction != city.pfaction)
                        {
                            nearCities.Add(city);
                        }
                    }
                }
            }
        }

        public void collectCitiesAndArmies(IntVector2 areaPos, int goalCount, float maxStrengthValue,
            List<GameObject.AbsMapObject> nearMapObjects,
            PFaction myFaction, PFaction factionFilter)
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
                            if (factionFilter.HasValue())
                            {
                                if (city.pfaction == factionFilter)
                                {
                                    nearMapObjects.Add(city);
                                }
                            }
                            else if (myFaction != city.pfaction)
                            {
                                nearMapObjects.Add(city);
                            }
                        }
                    }
                    lock (area.armies)
                    {
                        
                            foreach (var parmy in area.armies)
                            {
                                if (parmy.TryGetArmy(out var army) && army.strengthValue <= maxStrengthValue)
                                {
                                    if (factionFilter.HasValue())
                                    {
                                        if (parmy.pfaction == factionFilter)
                                        {
                                            nearMapObjects.Add(army);
                                        }
                                    }
                                    else if (myFaction != parmy.pfaction)
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
        public List<PSoldierGroup> processingGroups = new List<PSoldierGroup>(16);
        public List<PSoldierGroup> groups = new List<PSoldierGroup>(16);

        public List<PArmy> processingArmies = new List<PArmy>(2);//null;
        public List<PArmy> armies = new List<PArmy>(2);//null;

        public List<int> cities = new List<int>(2);

        public void processAdd(GameObject.SoldierGroup group)
        {
            processingGroups.Add(group.pointer());
        }

        public void processAdd(GameObject.Army army)
        {
            if (processingArmies == null)
            {
                processingArmies = new List<PArmy>(2);
            }

            processingArmies.Add(army.pointer());
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


}
