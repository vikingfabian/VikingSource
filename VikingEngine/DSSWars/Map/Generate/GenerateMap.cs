using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ.Joust;
using VikingEngine.PJ.SmashBirds;
using VikingEngine.PJ.Tanks;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Map.Generate
{
    class GenerateMap
    {
        const float LandChainMinRadius = 2;
        const float LandChainMaxRadius = 30;
        public static int LoadStatus = 0;
        static readonly IntervalF linkPosDiffRange = new IntervalF(0.5f, 3);
        static readonly Range landSpotSzRange = new Range(2, 24);
        public WorldData world;

        public bool postComplete = false;
        bool[] partComplete;
        GenerateRegion region = new GenerateRegion();
        CityCultureCollection cityCultureCollection = new CityCultureCollection();
        public bool abort = false;
        VikingEngine.EngineSpace.Maths.SimplexNoise2D noiseMap;
        BiomsLayout biomsLayout;
        public bool Generate(MapSize size, int number, bool save) 
        {
            ushort seed = Ref.rnd.Ushort();
            world = new WorldData(seed, size);
            world.saveIndex = number;
            biomsLayout = new BiomsLayout(world.rnd);

            try
            {
                ForXYLoop loop = new ForXYLoop(new IntVector2(world.Size.X, world.Size.Y));
                while (loop.Next())
                {
                    world.tileGrid.Set(loop.Position, new Tile());
                }

                LoadStatus = 20;

                generateLandChains();
                LoadStatus = 25;
                generateDigChains();
                LoadStatus = 30;
                generateLandChains();
                LoadStatus = 35;
                generateDigChains();
                LoadStatus = 40;
                generateLandChains();
                LoadStatus = 45;
                generateDigChains();
                LoadStatus = 50;

                setWaterHeightAndWaterHeatmap();

                LoadStatus = 55;
                generateCities();
                LoadStatus = 60;
                bindTilesToCities();
                LoadStatus = 65;

                bool areasuccess = calculateCityAreaSize_success();
                if (!areasuccess)
                {
                    return false;
                }
                LoadStatus = 70;
                factionStartAreas(size);
                                
                if (save)
                {
                    WorldDataStorage storage = new WorldDataStorage();
                    storage.saveMap(world);
                }
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return false;
            }

        }

        public void postLoadGenerate_Part1(WorldData world)
        { 
            this.world = world;
            world.rnd = new PcgRandom(world.seed);
            noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.seed);

            partComplete = new bool[ProcessSubTileParts];

            for (int i = 0; i < ProcessSubTileParts; i++)
            {
                int part = i;
                Task.Factory.StartNew(() =>
                {
                    processSubTiles(part);
                    partComplete[part] = true;
                    //postComplete = true;

                    bool allComplete = true;
                    for (int pc = 0; pc < ProcessSubTileParts; pc++)
                    {
                        if (!partComplete[pc])
                        {
                            allComplete = false;
                            break;
                        }
                    }

                    postComplete = allComplete;
                });
            }
        }

        public void postLoadGenerate_Part2(WorldData world)
        {
            this.world = world;
            world.rnd = new PcgRandom(world.seed);

            Task.Factory.StartNew(() =>
            {
                //generateSubTileFoliage();
                postComplete = true;
            });
        }

        void generateLandChains()
        {
            int[] mountain = new int[]
            {
                7,
                6,
                5,
                4,
                3,

            };
            int[] hills = new int[]
            {
                6,
                5,
                3,
                2,
                2,
            };
            int[] plain = new int[]
            {
                4,
                4,
                3,
                3,
                2,
            };
            int[] veryplain = new int[]
            {
                3,
                3,
                3,
                2,
                2,
            };


            RandomObjects<int[]> terrainTypes = new RandomObjects<int[]>(
                new ObjectCommonessPair<int[]>(4, mountain),
                new ObjectCommonessPair<int[]>(3, hills),
                new ObjectCommonessPair<int[]>(3, plain),
                new ObjectCommonessPair<int[]>(4, veryplain)
            );

            int numLandChains = world.areaTileCount / 2000;

            const float MaxRadiusChange = 2;
            const float MaxDirChange = 0.6f;

            IntervalF restartDistRange = new IntervalF(2, 30);
            int[] heightCurve;

            BiomType biom;
            Vector2 center = Vector2.Zero;
            float radius;
            Rotation1D growDir;
            int chainLength;
            Rotation1D heightCenter;
            float heightCenterLength;
            //Vector2 percentHCenter;
            Vector2 posDiff;
            float percentDist;
            Tile tile;
            Vector2 chainCenter;
            IntVector2 chainCenterSquare;
            int loopRadius;
            IntVector2 start;
            IntVector2 end;
            IntVector2 pos = IntVector2.Zero;
            Vector2 startPos;

            for (int i = 0; i < numLandChains; ++i)
            {
                heightCurve = terrainTypes.GetRandom(world.rnd);
                //biom = world.rnd.Chance(0.2) ? BiomType.YellowDry : BiomType.Green;//lib.Rnd.NextDouble() < 0.2;//world.rnd.RandomChance(0.2f);

                center = world.rnd.vector2(world.Size.X, world.Size.Y);
                biom = biomsLayout.get(world, center);

                startPos = center;
                newChain(out radius, out growDir, out chainLength, out heightCenter, out heightCenterLength);

                //go through each link in the chain
                for (int link = 0; link < chainLength; ++link)
                {
                    chainCenterSquare = new IntVector2(center);
                    loopRadius = (int)radius + 1;
                    start = chainCenterSquare - loopRadius;
                    end = chainCenterSquare + loopRadius;


                    for (pos.Y = start.Y; pos.Y <= end.Y; ++pos.Y)
                    {
                        for (pos.X = start.X; pos.X <= end.X; ++pos.X)
                        {
                            //percentHCenter = Vector2.One + heightCenter.Direction(heightCenterLength);
                            posDiff = pos.Vec - center;

                            float distFromCenter = posDiff.Length();
                            if (distFromCenter <= radius)
                            {
                                //posDiff *= percentHCenter;
                                percentDist = distFromCenter / radius;
                                //tile = GetTileSafe(pos);
                                if (world.GetTileSafe(pos, out tile))
                                {
                                    int setTerrain;
                                    if (percentDist < 0.2f)
                                    {
                                        setTerrain = heightCurve[0];
                                    }
                                    else if (percentDist < 0.4f)
                                    {
                                        setTerrain = heightCurve[1];
                                    }
                                    else if (percentDist < 0.6f)
                                    {
                                        setTerrain = heightCurve[2];
                                    }
                                    else if (percentDist < 0.8f)
                                    {
                                        setTerrain = heightCurve[3];
                                    }
                                    else
                                    {
                                        setTerrain = heightCurve[4];
                                    }

                                    if (setTerrain > tile.heightLevel)
                                    {
                                        tile.heightLevel = setTerrain;
                                        tile.biom = biom;
                                    }
                                    else 
                                    {
                                        tile.biom = biom;
                                    }
                                    //else if (world.rnd.Chance(0.4f))
                                    //{
                                    //    tile.biom = biom;
                                    //}
                                }

                            }
                        }
                    }

                    //move to the next link location
                    growDir.Add(world.rnd.Plus_MinusF(MaxDirChange));
                    radius = Bound.Set(radius + world.rnd.Plus_MinusF(MaxRadiusChange), LandChainMinRadius, LandChainMaxRadius);

                    center += growDir.Direction(linkPosDiffRange.GetRandom());

                    heightCenter.Add(world.rnd.Plus_MinusF(0.2f));
                    heightCenterLength = Bound.Set(heightCenterLength + world.rnd.Plus_MinusF(0.2f), 0, 0.9f);

                    if (link == chainLength - 1)
                    {
                        //some chance to restart closeby
                        if (world.rnd.Chance(0.5f))
                        {
                            link = 0;
                            chainCenter = (center + startPos) * PublicConstants.Half;
                            center = chainCenter + Rotation1D.Random().Direction(restartDistRange.GetRandom());

                            newChain(out radius, out growDir, out chainLength, out heightCenter, out heightCenterLength);
                        }
                    }
                }
            }
        }


        static readonly IntervalF startRadiusRange = new IntervalF(LandChainMinRadius, LandChainMaxRadius * 0.5f);
        static readonly Range chainLengthRange = new Range(2, 20);

        void newChain(out float radius, out Rotation1D growDir, out int chainLength,
            out Rotation1D heightCenter, out float heightCenterLength)
        {


            radius = lib.SmallestValue(startRadiusRange.GetRandom(), startRadiusRange.GetRandom());
            growDir = Rotation1D.Random();
            chainLength = chainLengthRange.GetRandom();

            heightCenter = Rotation1D.Random();
            heightCenterLength = world.rnd.Float(0.7f);
        }
        static readonly IntervalF digLinkPosDiffRange = new IntervalF(0.5f, 2);
        void generateDigChains()
        {
            int numLandChains = world.areaTileCount / 1800;
            for (int i = 0; i < numLandChains; ++i)
            {
                int[,] sunken = new int[world.Size.X, world.Size.Y];
                int depth = world.rnd.Chance(0.6f) ? 2 : 1;
                Vector2 center = world.rnd.vector2(world.Size.X, world.Size.Y);
                float radius = world.rnd.Float(0.6f, 4);
                Rotation1D growDir = Rotation1D.Random();
                int chainLength = world.rnd.Int(5, 200);
                //go through each link in the chain
                for (int link = 0; link < chainLength; ++link)
                {
                    Rectangle2 area = new Rectangle2(new IntVector2(center), (int)radius + 1);
                    ForXYLoop loopArea = new ForXYLoop(area);
                    while (loopArea.Next())
                    {
                        Vector2 posDiff = loopArea.Position.Vec - center;
                        float distFromCenter = (posDiff).Length();
                        if (distFromCenter <= radius)
                        {
                            float percentDist = distFromCenter / radius;
                            Tile t;
                            if (world.GetTileSafe(loopArea.Position, out t))
                            {
                                int sub = percentDist < 0.5f ? depth : 1;
                                if (sub > sunken[loopArea.Position.X, loopArea.Position.Y])
                                {
                                    sunken[loopArea.Position.X, loopArea.Position.Y] = sub;
                                    t.heightLevel -= sub;
                                    Bound.Min(ref t.heightLevel,  Height.LowWaterHeight);
                                }


                            }
                        }
                    }
                    //move to the next link location
                    growDir.Add(world.rnd.Plus_MinusF(0.6f));
                    radius = Bound.Set(radius + world.rnd.Plus_MinusF(0.6f), 1, 6);

                    center += growDir.Direction(digLinkPosDiffRange.GetRandom());
                }
            }
        }

        void setWaterHeightAndWaterHeatmap()
        {
            const int OrthogonalHeat = 10;
            const int DiagonalHeat = 12;


            //Först markera ut alla borders
            //Loopa tills alla avstånd är uträknade


            ForXYLoop loop = new ForXYLoop(world.Size);
            while (loop.Next())
            {
                var tile = world.tileGrid.array[loop.Position.X, loop.Position.Y];
                if (tile.IsWater())
                {
                    tile.heightLevel = Height.DeepWaterHeight;
                    Tile nTile;

                    //Check if it has a neighbor tile that is land
                    foreach (IntVector2 dir in IntVector2.Dir4Array)
                    {
                        var npos = loop.Position + dir;
                        if (world.GetTileSafe(npos, out nTile) && nTile.IsLand())
                        {
                            //Is water to land border
                            nTile.seaDistanceHeatMap = OrthogonalHeat;

                            tile.heightLevel = Height.LowWaterHeight;
                            tile.seaDistanceHeatMap = -OrthogonalHeat;
                        }
                    }

                    if (tile.seaDistanceHeatMap == int.MinValue)
                    {
                        foreach (IntVector2 dir in IntVector2.AllDiagonalsArray)
                        {
                            var npos = loop.Position + dir;
                            if (world.GetTileSafe(npos, out nTile) && nTile.IsLand())
                            {
                                //Is water to land border
                                if (nTile.seaDistanceHeatMap == int.MinValue)
                                {
                                    nTile.seaDistanceHeatMap = DiagonalHeat;
                                }
                                tile.heightLevel = Height.LowWaterHeight;
                                if (tile.seaDistanceHeatMap == int.MinValue)
                                {
                                    tile.seaDistanceHeatMap = -DiagonalHeat;
                                }
                            }
                        }
                    }
                }
            }


            //Loop until every tile has a distance value
            int updatedTiles = int.MaxValue;

            while (updatedTiles > 0)
            {
                updatedTiles = 0;

                loop.Reset();
                while (loop.Next())
                {
                    var tile = world.tileGrid.array[loop.Position.X, loop.Position.Y];
                    if (tile.seaDistanceHeatMap == int.MinValue)
                    {
                        Tile nTile;

                        foreach (IntVector2 dir in IntVector2.Dir4Array)
                        {
                            var npos = loop.Position + dir;
                            if (world.GetTileSafe(npos, out nTile) && nTile.seaDistanceHeatMap != int.MinValue)
                            {
                                ++updatedTiles;

                                if (tile.IsLand())
                                {
                                    tile.setWaterHeat_Land(nTile.seaDistanceHeatMap + OrthogonalHeat);
                                }
                                else
                                {
                                    tile.setWaterHeat_Water(nTile.seaDistanceHeatMap - OrthogonalHeat);
                                }
                            }
                        }

                        foreach (IntVector2 dir in IntVector2.AllDiagonalsArray)
                        {
                            var npos = loop.Position + dir;
                            if (world.GetTileSafe(npos, out nTile) && 
                                nTile.seaDistanceHeatMap != int.MinValue)
                            {
                                bool land = tile.IsLand();

                                if (land == nTile.IsLand())
                                {
                                    ++updatedTiles;

                                    if (land)
                                    {
                                        tile.setWaterHeat_Land(nTile.seaDistanceHeatMap + DiagonalHeat);
                                    }
                                    else
                                    {
                                        tile.setWaterHeat_Water(nTile.seaDistanceHeatMap - DiagonalHeat);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void generateCities()
        {
            int numHeadCities = world.areaTileCount / 2000;
            world.cities = new List<City>(numHeadCities);

            generateCityType(CityType.Head, numHeadCities, 14);
            generateCityType(CityType.Large, numHeadCities * 2, 9);
            generateCityType(CityType.Small, numHeadCities * 4, 8);
        }
        void generateCityType(CityType type, int amount, float neededSpace)
        {
            int totalAmount = world.cities.Count + amount;
            IntVector2[] fourDirs = IntVector2.Dir4Array;

            Rectangle2 cityArea = world.tileBounds;
            cityArea.AddRadius(-10);

            while (world.cities.Count < totalAmount)
            {
                IntVector2 pos = new IntVector2(cityArea.RandomPos());
                Tile cityTile = world.tileGrid.Get(pos);
                {
                    if (cityTile.IsLand() && cityTile.heightLevel < Height.MountainHeightStart)
                    {
                        //TODO check larger area
                        int numWaterTiles = 0;
                        for (int i = 0; i < fourDirs.Length; ++i)
                        {
                            Tile neighbor = world.tileGrid.Get(pos + fourDirs[i]);
                            if (neighbor.IsWater())
                            { ++numWaterTiles; }
                        }

                        //Make sure most cities are close to water
                        //pulls its food from the sea or wet land
                        if (numWaterTiles > 0 || 
                            (world.rnd.Chance(0.2f) && cityTile.biom != BiomType.YellowDry))
                        {
                            if (cityHasEnoughGround(pos))//numWaterTiles <= 2
                            {                                
                                float closestDist;
                                world.closestCity(pos, out closestDist);
                                if (closestDist > neededSpace)
                                {
                                    City c = new City(world.cities.Count, pos, type, world);
                                    c.generateCultureAndEconomy(world, cityCultureCollection);
                                    world.cities.Add(c);
                                    //addUnitToGrid(c);
                                    cityTile.tileContent = TileContent.City;
                                    //t.CityIndex = c.index;

                                    world.unitCollAreaGrid.add(c);
                                }
                                
                            }
                        }
                    }
                }

            }

        }

        bool cityHasEnoughGround(IntVector2 pos)
        {
            ForXYEdgeLoop edgeLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(pos, 1));
            int edgeCount = 0;
            while (edgeLoop.Next())
            {
                var t = world.tileGrid.Get(edgeLoop.Position);
                if (t.IsLand() && t.heightLevel < Height.MountainHeightStart)
                {
                    ++edgeCount;
                }
            }

            if (edgeCount < 2)
            { 
                return false;
            }

            edgeLoop.ExpandRadius();
            edgeCount = 0;
            while (edgeLoop.Next())
            {
                var t = world.tileGrid.Get(edgeLoop.Position);
                if (t.IsLand() && t.heightLevel < Height.MountainHeightStart)
                {
                    ++edgeCount;
                }
            }

            if (edgeCount < 4)
            {
                return false;
            }

            Rectangle2 area = Rectangle2.FromCenterTileAndRadius(pos, 4);
            area.SetBounds(world.tileBounds);

            int usableTileCount = 0;

            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                var t =  world.tileGrid.Get(loop.Position);
                if (t.IsLand() && t.heightLevel < Height.MountainHeightStart)
                {
                    ++usableTileCount;
                }
            }

            //Area is 9*9 = 81, expecting a quarter to be usable
            return usableTileCount >= 20;
        }

        void bindTilesToCities()
        {
            // figure out which tile is closest to which city, version 2
            new CityMapInfluence().generate(world);

            {
                //calc what tiles are in border to eachother
                IntVector2[] checkDirs = IntVector2.Dir4Array;
                Rectangle2 checkTiles = world.tileBounds;
                checkTiles.AddRadius(-1);

                ForXYLoop loop = new ForXYLoop(checkTiles);

                while (loop.Next())
                {
                    Tile t = world.tileGrid.Get(loop.Position);
                    if (t.IsLand())
                    {
                        City owner = world.cities[t.CityIndex];
                        int borderCity = -1;

                        for (int dirIx = 0; dirIx < checkDirs.Length; ++dirIx)//each (IntVector2 dir in checkDirs)
                        {
                            IntVector2 dir = checkDirs[dirIx];
                            Tile neighbor = world.tileGrid.array[dir.X + loop.Position.X, dir.Y + loop.Position.Y];
                            bool land = neighbor.IsLand();
                            if (neighbor.CityIndex != owner.index)
                            {
                                t.AddBorder(dirIx, land? neighbor.CityIndex: Tile.SeaBorder);
                                borderCity = neighbor.CityIndex;
                            }
                            //else if (!neighbor.IsLand())
                            //{
                            //    t.AddBorder(dirIx, Tile.SeaBorder);
                            //}
                        }

                        if (t.BorderCount > 0)
                        {
                            if (!arraylib.InBound(world.cities, borderCity))
                            {
                                lib.DoNothing();
                            }
                            owner.AddNeighborCity(borderCity);
                        }
                    }
                }
            }
        }



        bool calculateCityAreaSize_success()
        {
            //bool success = true;
            world.tileGrid.LoopBegin();
            while (world.tileGrid.LoopNext())
            {
                var tile = world.tileGrid.LoopValueGet();
                if (tile.IsLand() && tile.tileContent != TileContent.City)
                {
                    ++world.cities[tile.CityIndex].areaSize;
                }
            }

            //check city sizes
            for(int i = world.cities.Count - 1; i >= 0; --i)
            {
                if (!world.cities[i].hasNeededAreaSize())
                {
                    return false;
                    ////Remove the city, it has not enough area!
                    //world.tileGrid.Get(world.cities[i].tilePos).tileContent = TileContent.NONE;
                    //world.unitCollAreaGrid.remove(world.cities[i]);
                    //world.cities.RemoveAt(i);
                }
            }

            return true;
        }

        void factionStartAreas(MapSize mapSize)
        {
            int goalWorkForce = DssLib.HeadCityMaxWorkForce + DssLib.LargeCityMaxWorkForce + DssLib.SmallCityMaxWorkForce;

            if (mapSize >= MapSize.Epic)
            {
                goalWorkForce += DssLib.HeadCityMaxWorkForce;
            }
            else if (mapSize >= MapSize.Huge)
            {
                goalWorkForce += DssLib.LargeCityMaxWorkForce;
            }

            
            namedFactionsOnMap(goalWorkForce);

            //if (mapSize >= MapSize.Huge)
            //{
            //    goalIncome += DssLib.HeadCityBasicIncome;
            //}

            foreach (City c in world.cities)
            {
                //c.SetStartFaction(goalWorkForce, world.factions, world);

                if (c.faction == null)
                {
                    region.Reset(goalWorkForce);
                    region.GetStartFactionRegion(c, world);

                    var faction = new Faction(world, FactionType.DefaultAi);
                    
                    region.ApplyFaction(faction);

                    if (region.currentWorkforce >= region.goalWorkForce)
                    {
                        faction.availableForPlayer = true;

                    }
                }
            }

            if (world.factions.Count > DssLib.RtsMaxFactions)
            {
                throw new Exception();
            }
        }

        void namedFactionsOnMap(int standardWorkForce)
        {   
            {
                var DarkFollower = new Faction(world, FactionType.DarkFollower);

                region.Reset(MathExt.MultiplyInt(3, standardWorkForce));

                region.GetStartFactionRegion(randomCity(), world);
                region.ApplyFaction(DarkFollower);
            }

            { 
                var UnitedKingdom = new Faction(world, FactionType.UnitedKingdom);
            
                region.Reset(MathExt.MultiplyInt(5, standardWorkForce));

                region.GetStartFactionRegion(randomCity(), world);
                region.ApplyFaction(UnitedKingdom);
            }

            {
                var GreenWood = new Faction(world, FactionType.GreenWood);

                region.Reset(MathExt.MultiplyInt(1.5, standardWorkForce));

                region.GetStartFactionRegion(collection_pullNextCity(cityCultureCollection.LargeGreen), world);
                region.ApplyFaction(GreenWood);
            }

            {
                var EasternEmpire = new Faction(world, FactionType.EasternEmpire);

                region.Reset(MathExt.MultiplyInt(3, standardWorkForce));

                region.GetStartFactionRegion(collection_pullNextCity(cityCultureCollection.DryEast), world);
                region.ApplyFaction(EasternEmpire);
            }

            {
                var NordicRealms = new Faction(world, FactionType.NordicRealm);

                region.Reset(MathExt.MultiplyInt(2, standardWorkForce));

                region.GetStartFactionRegion(collection_pullNextCity(cityCultureCollection.NorthSea), world);
                region.ApplyFaction(NordicRealms);
            }

            {
                var BearClaw = new Faction(world, FactionType.BearClaw);

                region.Reset(MathExt.MultiplyInt(1.5, standardWorkForce));

                region.GetStartFactionRegion(collection_pullNextCity(cityCultureCollection.NorthSea), world);
                region.ApplyFaction(BearClaw);
            }

            {
                var NordicSpur = new Faction(world, FactionType.NordicSpur);

                region.Reset(MathExt.MultiplyInt(1.5, standardWorkForce));

                region.GetStartFactionRegion(collection_pullNextCity(cityCultureCollection.NorthSea), world);
                region.ApplyFaction(NordicSpur);
            }

            {
                var IceRaven = new Faction(world, FactionType.IceRaven);

                region.Reset(MathExt.MultiplyInt(1.5, standardWorkForce));

                region.GetStartFactionRegion(collection_pullNextCity(cityCultureCollection.NorthSea), world);
                region.ApplyFaction(IceRaven);
            }

            {
                var DragonSlayer = new Faction(world, FactionType.DragonSlayer);

                region.Reset(MathExt.MultiplyInt(1.5, standardWorkForce));

                region.GetStartFactionRegion(randomCity(), world);
                region.ApplyFaction(DragonSlayer);
            }
        }

        City collection_pullNextCity(List<City> collection)
        {
            while (collection.Count > 0)
            {
                var city = arraylib.RandomListMemberPop(collection, world.rnd);
                if (city.faction == null)
                {
                    return city;
                }
            }

            return randomCity();
            
        }

        City randomCity()
        {
            City city=null;

            do
            {
                city = arraylib.RandomListMember(world.cities, world.rnd);
            } while (city.faction != null);

            return city;
        }

        const int ProcessSubTileParts = 8;

        void processSubTiles(int part)
        {
            const int WidthMin1 = WorldData.SubTileWidth - 1;

            //ForXYLoop loop = new ForXYLoop(world.Size);

            int partWidth = world.Size.X / ProcessSubTileParts;
            int startX = partWidth * part;
            int endX = startX + partWidth;

            

            //while (loop.Next())
            for (int loopy = 0; loopy < world.Size.Y; ++loopy)
            {
                int supTileStartY = loopy * WorldData.SubTileWidth;
                for (int loopx = startX; loopx < endX; ++loopx)
                {
                    int supTileStartX = loopx * WorldData.SubTileWidth;

                    Tile tile = world.tileGrid.array[loopx, loopy];// .Get(loop.Position);
                    Height h = DssRef.map.heigts[tile.heightLevel];
                    BiomColor biom = DssRef.map.bioms.colors[(int)tile.biom];
                    //var col = biom.colors_height_variant[tile.heightLevel, subTile.colorVariant];

                    SubTileMainType tileType = tile.IsLand() ? SubTileMainType.DefaultLand : SubTileMainType.DefaultSea;
                    //var terrain = Tile.TerrainTypes[tile.biom, tile.heightLevel];

                    //IntVector2 start = loop.Position * UnitDetailMap3.Width;

                    float groundY = tile.GroundY();

                    float groundY_w = edgeHeight(-1, 0);
                    float groundY_e = edgeHeight(1, 0);

                    float groundY_n = edgeHeight(0, -1);
                    float groundY_s = edgeHeight(0, 1);

                    for (int y = 1; y < WidthMin1; ++y)
                    {
                        for (int x = 1; x < WidthMin1; ++x)
                        {
                            subTile(x, y, groundY, tileType);
                        }
                    }

                    for (int sidePos = 1; sidePos < WidthMin1; ++sidePos)
                    {
                        subTile(0, sidePos, groundY_w, tileType);

                        subTile(WidthMin1, sidePos, groundY_e, tileType);

                        subTile(sidePos, 0, groundY_n, tileType);

                        subTile(sidePos, WidthMin1, groundY_s, tileType);
                    }

                    subTile(0, 0, lib.SmallestValue(groundY_w, groundY_n), tileType);
                    subTile(WidthMin1, 0, lib.SmallestValue(groundY_e, groundY_n), tileType);
                    subTile(0, WidthMin1, lib.SmallestValue(groundY_w, groundY_s), tileType);
                    subTile(WidthMin1, WidthMin1, lib.SmallestValue(groundY_s, groundY_e), tileType);

                    float edgeHeight(int x, int y)
                    {
                        float result = groundY;
                        Tile nTile;
                        if (world.tileGrid.TryGet(loopx + x, loopy + y, out nTile))
                        {
                            result = nTile.GroundY();
                            result = 0.8f * groundY + 0.2f * result;
                        }

                        return result;
                    }

                    void subTile(int x, int y, float topY, SubTileMainType tiletype)
                    {
                        const int RndRange = 3;

                        int subX = supTileStartX + x;
                        int subY = supTileStartY + y;

                        Microsoft.Xna.Framework.Color rndColor;

                        //float noise =  noiseMap.OctaveNoise2D(4, 0.6f, 30, subX, subY);
                        //int colorVariant = 0;
                        //if (noise > 0.25f)
                        //{
                        //    colorVariant = 1;
                        //}
                        //else if (noise < -0.2f)
                        //{ 
                        //    colorVariant = 2;
                        //}

                        var col = biom.Color(tile); //biom.colors_height[tile.heightLevel];// colorVariant = 0];

                        if (world.rnd.Chance(0.6))
                        {
                            rndColor = new Microsoft.Xna.Framework.Color(
                                Bound.Byte(col.Color.R + world.rnd.Plus_Minus(RndRange)),
                                Bound.Byte(col.Color.G + world.rnd.Plus_Minus(RndRange)),
                                Bound.Byte(col.Color.B + world.rnd.Plus_Minus(RndRange)));
                        }
                        else
                        {
                            rndColor = col.Color;
                        }

                        if (topY < groundY)
                        {
                            rndColor = ColorExt.ChangeBrighness(rndColor, 10);
                        }

                        if (world.rnd.Chance(h.groundYoffsetChance))
                        {
                            topY += world.rnd.Plus_MinusF(h.groundYoffset);
                        }

                        if (h.mountainPeak != null)
                        {
                            topY += h.mountainPeak[x, y];
                        }

                        var subTile = new SubTile(tiletype, rndColor, topY);
                        //subTile.colorVariant = colorVariant;
                        TerrainContent.createSubTileContent(subX, subY, tile, ref subTile, world, noiseMap);

                        world.subTileGrid.Set(subX, subY, subTile);

                    }
                }

            }
        }

       

        //void generateSubTileFoliage()
        //{
        //    int numTreeSpots = world.areaTileCount / 50;
        //    int numStoneSpots = world.areaTileCount / 100;

        //    const int SmallSpotMaxOffset = 6;
        //    Rectangle2 area = new Rectangle2(new IntVector2(6), world.subTileGrid.Size - 6);

        //    IntVector2 radius = IntVector2.Zero;
        //    IntVector2 center = IntVector2.Zero;

        //    foliageAreas(SubTileFoilType.TreeHard, numTreeSpots, new Range(2, 14), new Range(1, 8));
        //    singleFoilDots(SubTileFoilType.Stones, numStoneSpots);

        //    void foliageAreas(SubTileFoilType type, int count,
        //        Range large, Range small)
        //    {
        //        int int_type = (int)type;

        //        for (int i = 0; i < count; ++i)
        //        {
        //            center = area.RandomTile();
        //            Tile tile = world.tileFromSubTilePos(center);

        //            if (tile.terrain().foilEnabled[int_type])
        //            {
        //                radius.X = large.GetRandom();
        //                radius.Y = large.GetRandom();

        //                generateSpot();

        //                center.X += world.rnd.Plus_Minus(SmallSpotMaxOffset);
        //                center.Y += world.rnd.Plus_Minus(SmallSpotMaxOffset);

        //                radius.X = small.GetRandom();
        //                radius.Y = small.GetRandom();

        //                generateSpot();

        //                if (world.rnd.Chance(0.4))
        //                {
        //                    center.X += world.rnd.Plus_Minus(SmallSpotMaxOffset);
        //                    center.Y += world.rnd.Plus_Minus(SmallSpotMaxOffset);

        //                    radius.X = small.GetRandom();
        //                    radius.Y = small.GetRandom();

        //                    generateSpot();
        //                }

        //                void generateSpot()
        //                {
        //                    Vector2 centerVec = center.Vec;
        //                    float maxRadius = radius.SideLength() + 1;
        //                    ForXYLoop loop = new ForXYLoop(Rectangle2.FromCenterTileAndRadius(center, radius));
        //                    while (loop.Next())
        //                    {
        //                        float offsetPerc = 1f -
        //                            (loop.Position.Vec - centerVec).Length() / maxRadius;

        //                        float chance = offsetPerc * 0.6f + 0.1f;


        //                        if (world.rnd.Chance(chance))
        //                        {
        //                            if (world.subTileGrid.InBounds(loop.Position) &&
        //                                world.tileFromSubTilePos(loop.Position).IsLand())
        //                            {
        //                                var st = world.subTileGrid.Get(loop.Position);
        //                                st.maintype = SubTileMainType.Foil;
        //                                st.undertype = int_type;
        //                                world.subTileGrid.Set(loop.Position, st);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    void singleFoilDots(SubTileFoilType type, int count)
        //    {
        //        int int_type = (int)type;

        //        for (int i = 0; i < count; ++i)
        //        {
        //            center = area.RandomTile();
        //            Tile tile = world.tileFromSubTilePos(center);

        //            if (tile.terrain().foilEnabled[(int)type])
        //            {
        //                //subTileGrid.Get(center).foil = type;
        //                var st = world.subTileGrid.Get(center);
        //                st.maintype = SubTileMainType.Foil;
        //                st.undertype = int_type;
        //                world.subTileGrid.Set(center, st);
        //            }
        //        }
        //    }
        //}
    }


    class CityCultureCollection
    { 
        public List<City> LargeGreen = new List<City>();
        public List<City> DryEast = new List<City>();
        public List<City> NorthSea = new List<City>();

    }

}
