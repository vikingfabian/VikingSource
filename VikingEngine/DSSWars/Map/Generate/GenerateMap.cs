using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ.Joust;
using VikingEngine.PJ.SmashBirds;
using VikingEngine.PJ.Tanks;
using VikingEngine.ToGG.Commander.UnitsData;
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

        static readonly IntervalF startRadiusRange = new IntervalF(LandChainMinRadius, LandChainMaxRadius * 0.5f);
        static readonly Range chainLengthRange = new Range(2, 20);


        IntervalF[] citySizeToMudRadius = new IntervalF[]
        {
            new IntervalF(2, 4),
            new IntervalF(3, 5),
            new IntervalF(5, 7),
        };
        
        public bool Generate(bool save, Data.WorldMetaData worldMeta)//, int number, ushort seed) 
        {
            //ushort seed = Ref.rnd.Ushort();
            world = new WorldData(worldMeta);//new Data.WorldMetaData(seed, size, number));
            world.availableGenericAiTypes = WorldData.AvailableGenericAiTypes();
            biomsLayout = new BiomsLayout(world.rnd);

            try
            {
                //bool splitLandChainProcess = world.Size.X > (LandChainMaxRadius + chainLengthRange.Max) * 3;
                //int paintStrokeSafeDistance = (int)( (LandChainMaxRadius + chainLengthRange.Max) * 3);
                //int processDivideX = world.Size.X / paintStrokeSafeDistance;
                //int processDivideY = world.Size.Y / paintStrokeSafeDistance;


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
                factionStartAreas(worldMeta.mapSize);
                                
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
            world.rnd = new PcgRandom(world.metaData.seed);
            noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.metaData.seed);

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

        public void postLoadGenerate_Part2(WorldData world, SaveStateMeta loadMeta)
        {
            this.world = world;
            world.rnd = new PcgRandom(world.metaData.seed);

            Task.Factory.StartNew(() =>
            {
                //generateSubTileFoliage();
                if (loadMeta == null)
                {
                    foreach (var c in world.cities)
                    {
                        c.createBuildingSubtiles(world);
                    }
                }

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

                                    world.tileGrid.Set(pos, tile);
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

                    center += growDir.Direction(linkPosDiffRange.GetRandom(world.rnd));

                    heightCenter.Add(world.rnd.Plus_MinusF(0.2f));
                    heightCenterLength = Bound.Set(heightCenterLength + world.rnd.Plus_MinusF(0.2f), 0, 0.9f);

                    if (link == chainLength - 1)
                    {
                        //some chance to restart closeby
                        if (world.rnd.Chance(0.5f))
                        {
                            link = 0;
                            chainCenter = (center + startPos) * PublicConstants.Half;
                            center = chainCenter + Rotation1D.Random(world.rnd).Direction(restartDistRange.GetRandom(world.rnd));

                            newChain(out radius, out growDir, out chainLength, out heightCenter, out heightCenterLength);
                        }
                    }
                }
            }
        }


        

        void newChain(out float radius, out Rotation1D growDir, out int chainLength,
            out Rotation1D heightCenter, out float heightCenterLength)
        {


            radius = lib.SmallestValue(startRadiusRange.GetRandom(world.rnd), startRadiusRange.GetRandom(world.rnd));
            growDir = Rotation1D.Random(world.rnd);
            chainLength = chainLengthRange.GetRandom(world.rnd);

            heightCenter = Rotation1D.Random(world.rnd);
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
                Rotation1D growDir = Rotation1D.Random(world.rnd);
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
                                    world.tileGrid.Set(loopArea.Position, t);
                                }


                            }
                        }
                    }
                    //move to the next link location
                    growDir.Add(world.rnd.Plus_MinusF(0.6f));
                    radius = Bound.Set(radius + world.rnd.Plus_MinusF(0.6f), 1, 6);

                    center += growDir.Direction(digLinkPosDiffRange.GetRandom(world.rnd));
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

                            world.tileGrid.Set(npos, nTile);
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

                    world.tileGrid.array[loop.Position.X, loop.Position.Y] = tile;
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

                        world.tileGrid.array[loop.Position.X, loop.Position.Y] = tile;
                    }
                }
            }
        }

        public const int HeadCityNeededFreeRadius = 14;

        void generateCities()
        {
            int numHeadCities = world.areaTileCount / 2000;
            world.cities = new List<City>(numHeadCities);

            generateCityType(CityType.Head, numHeadCities, HeadCityNeededFreeRadius);
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
                IntVector2 pos = new IntVector2(cityArea.RandomPos(world.rnd));
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
                                    world.tileGrid.Set(pos, cityTile);
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

                        for (int dirIx = 0; dirIx < checkDirs.Length; ++dirIx)
                        {
                            IntVector2 dir = checkDirs[dirIx];
                            Tile neighbor = world.tileGrid.array[dir.X + loop.Position.X, dir.Y + loop.Position.Y];
                            bool land = neighbor.IsLand();
                            if (neighbor.CityIndex != owner.parentArrayIndex)
                            {
                                t.AddBorder(dirIx, land? neighbor.CityIndex: Tile.SeaBorder);
                                borderCity = neighbor.CityIndex;
                            }
                        }

                        if (t.BorderCount > 0)
                        {
                            if (!arraylib.InBound(world.cities, borderCity))
                            {
                                lib.DoNothing();
                            }
                            owner.AddNeighborCity(borderCity);
                        }

                        world.tileGrid.Set(loop.Position, t);
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
                }
            }

            return true;
        }

        void factionStartAreas(MapSize mapSize)
        {
            int goalWorkForce = DssConst.HeadCityStartMaxWorkForce + DssConst.LargeCityStartMaxWorkForce + DssConst.SmallCityStartMaxWorkForce;

            if (mapSize >= MapSize.Epic)
            {
                goalWorkForce += DssConst.HeadCityStartMaxWorkForce;
            }
            else if (mapSize >= MapSize.Huge)
            {
                goalWorkForce += DssConst.LargeCityStartMaxWorkForce;
            }

            bool useRandomEmpires = mapSize >= MapSize.Medium;
            IntervalF randomEmpiresSizeMulti = new IntervalF(1.5f, 2f + (mapSize - MapSize.Medium));


            namedFactionsOnMap(goalWorkForce);


            foreach (City c in world.cities)
            {
                //c.SetStartFaction(goalWorkForce, world.factions, world);

                if (c.faction == null)
                {
                    int size = goalWorkForce;
                    bool rndEmpire = useRandomEmpires && world.rnd.Chance(0.25);
                    if (rndEmpire)
                    { 
                        size = MathExt.MultiplyInt(randomEmpiresSizeMulti.GetRandom(world.rnd), size);
                    }
                    //region.Reset((int)size);
                    var faction = new Faction(world, FactionType.DefaultAi);
                    int regionCurrentWorkforce = region.GetStartFactionRegion(size, c, world, faction);


                    if (regionCurrentWorkforce >= size && !rndEmpire)
                    {
                        faction.availableForPlayer = true;
                    }
                }
#if DEBUG
                if (c.faction == null)
                {
                    throw new Exception();
                }
#endif
            }

            if (world.factions.Count > DssLib.RtsMaxFactions)
            {
                throw new Exception();
            }
        }

        void namedFactionsOnMap(int standardWorkForce)
        {   
            {
                var faction = new Faction(world, FactionType.DarkFollower);

                //region.Reset(MathExt.MultiplyInt(3, standardWorkForce));

                int size = MathExt.MultiplyInt(3, standardWorkForce);
                region.GetStartFactionRegion(size, randomCity(), world, faction);
                //region.ApplyFaction(DarkFollower);
            }

            { 
                var faction = new Faction(world, FactionType.UnitedKingdom);

                int size = MathExt.MultiplyInt(5, standardWorkForce);

                region.GetStartFactionRegion(size, randomCity(), world, faction);
                //region.ApplyFaction(UnitedKingdom);
            }

            {
                var faction = new Faction(world, FactionType.GreenWood);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, collection_pullNextCity(cityCultureCollection.LargeGreen), world, faction);
                //region.ApplyFaction(GreenWood);
            }

            if (world.metaData.mapSize >= MapSize.Medium)
            {
                {
                    var faction = new Faction(world, FactionType.DyingMonger);

                    int size = MathExt.MultiplyInt(2, standardWorkForce);

                    region.GetStartFactionRegion(size, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);
                    //region.ApplyFaction(faction);
                }
                {
                    var faction = new Faction(world, FactionType.DyingHate);

                    int size = MathExt.MultiplyInt(2, standardWorkForce);

                    region.GetStartFactionRegion(size, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);
                   //region.ApplyFaction(faction);
                }
                {
                    var faction = new Faction(world, FactionType.DyingDestru);

                    int size = MathExt.MultiplyInt(2, standardWorkForce);

                    region.GetStartFactionRegion(size, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);
                    //region.ApplyFaction(faction);
                }

            }

            {
                var faction = new Faction(world, FactionType.EasternEmpire);

                int size = MathExt.MultiplyInt(3, standardWorkForce);

                region.GetStartFactionRegion(size, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);
                //region.ApplyFaction(faction);
            }

            {
                var faction = new Faction(world, FactionType.NordicRealm);

                int size = MathExt.MultiplyInt(2, standardWorkForce);

                region.GetStartFactionRegion(size, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);
                //region.ApplyFaction(NordicRealms);
            }

            {
                var faction = new Faction(world, FactionType.BearClaw);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);
                //region.ApplyFaction(BearClaw);
            }

            {
                var faction = new Faction(world, FactionType.NordicSpur);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);
                //region.ApplyFaction(NordicSpur);
            }

            {
                var faction = new Faction(world, FactionType.IceRaven);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);
                //region.ApplyFaction(IceRaven);
            }

            {
                var faction = new Faction(world, FactionType.DragonSlayer);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, randomCity(), world, faction);
                //region.ApplyFaction(DragonSlayer);
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
            List<IntVector2> mineLocations = new List<IntVector2>(1024);

            const int WidthMin1 = WorldData.TileSubDivitions - 1;

            int partWidth = world.Size.X / ProcessSubTileParts;
            int startX = partWidth * part;
            int endX = startX + partWidth;
            
            for (int loopy = 0; loopy < world.Size.Y; ++loopy)
            {
                int supTileStartY = loopy * WorldData.TileSubDivitions;
                for (int loopx = startX; loopx < endX; ++loopx)
                {
                    int supTileStartX = loopx * WorldData.TileSubDivitions;

                    Tile tile = world.tileGrid.array[loopx, loopy];
                    var city = world.cities[tile.CityIndex];
                    var cityPos = city.tilePos;
                    float distanceToCity = VectorExt.Length(cityPos.X - loopx, cityPos.Y - loopy);
                    IntervalF mudRadius = citySizeToMudRadius[(int)city.CityType];

                    Height heightSett = DssRef.map.heigts[tile.heightLevel];
                    Biom biom = DssRef.map.bioms.bioms[(int)tile.biom];

                    int defaultLandType = 0;
                    TerrainMainType tileType;
                    if (tile.IsLand())
                    {
                        tileType = TerrainMainType.DefaultLand;
                        defaultLandType = (int)(tile.heightLevel < Height.MountainHeightStart ? TerrainDefaultLandType.Flat : TerrainDefaultLandType.Mountain);
                    }
                    else
                    {
                        tileType = TerrainMainType.DefaultSea;
                        defaultLandType = (int)(tile.heightLevel == Height.LowWaterHeight? TerrainSeaType.Low : TerrainSeaType.Deep);
                    }
                    
                    float groundY = tile.GroundY();

                    float groundY_w = edgeHeight(-1, 0);
                    float groundY_e = edgeHeight(1, 0);

                    float groundY_n = edgeHeight(0, -1);
                    float groundY_s = edgeHeight(0, 1);

                    for (int y = 1; y < WidthMin1; ++y)
                    {
                        for (int x = 1; x < WidthMin1; ++x)
                        {
                            subTile(x, y, groundY, tileType, defaultLandType);
                        }
                    }

                    for (int sidePos = 1; sidePos < WidthMin1; ++sidePos)
                    {
                        subTile(0, sidePos, groundY_w, tileType, defaultLandType);

                        subTile(WidthMin1, sidePos, groundY_e, tileType, defaultLandType);

                        subTile(sidePos, 0, groundY_n, tileType, defaultLandType);

                        subTile(sidePos, WidthMin1, groundY_s, tileType, defaultLandType);
                    }

                    subTile(0, 0, lib.SmallestValue(groundY_w, groundY_n), tileType, defaultLandType);
                    subTile(WidthMin1, 0, lib.SmallestValue(groundY_e, groundY_n), tileType, defaultLandType);
                    subTile(0, WidthMin1, lib.SmallestValue(groundY_w, groundY_s), tileType, defaultLandType);
                    subTile(WidthMin1, WidthMin1, lib.SmallestValue(groundY_s, groundY_e), tileType, defaultLandType);

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

                    void subTile(int x, int y, float topY, TerrainMainType tiletype, int subType)
                    {
                        const int RndRange = 3;

                        int subX = supTileStartX + x;
                        int subY = supTileStartY + y;

                        Color rndColor;

                        var col = biom.Color(tile);

                        if (world.rnd.Chance(0.6))
                        {
                            rndColor = new Color(
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

                        if (world.rnd.Chance(heightSett.groundYoffsetChance))
                        {
                            topY += world.rnd.Plus_MinusF(heightSett.groundYoffset);
                        }

                        if (heightSett.mountainPeak != null)
                        {
                            topY += heightSett.mountainPeak[x, y];
                        }

                        var subTile = new SubTile(tiletype, subType, rndColor, topY);
                        TerrainContent.createSubTileContent(subX, subY, distanceToCity, tile, heightSett, biom, ref mudRadius, ref subTile, world, noiseMap, mineLocations);

                        world.subTileGrid.Set(subX, subY, subTile);

                    }
                }

            }

            int mithrilCount = 0;
            switch (world.metaData.mapSize)
            {
               //Tiny, Small, Medium, Large, Huge, Epic
               default:
                    mithrilCount = 2;
                    break;

                case MapSize.Medium:
                    mithrilCount = 3;
                    break;

                case MapSize.Large:
                    mithrilCount = 4;
                    break;

                case MapSize.Huge:
                case MapSize.Epic:
                    mithrilCount = 5;
                    break;
            }

            if (world.rnd.Chance(0.4))
            {
                ++mithrilCount;

                if (world.rnd.Chance(0.1))
                {
                    ++mithrilCount;
                }
            }

            addMines(mithrilCount, (int)TerrainMineType.Mithril);

            int tin = MathExt.MultiplyInt(world.rnd.Double(0.12, 0.14), mineLocations.Count);
            int cupper = MathExt.MultiplyInt(world.rnd.Double(0.12, 0.14), mineLocations.Count);
            int lead = MathExt.MultiplyInt(world.rnd.Double(0.12, 0.14), mineLocations.Count);
            int silver = MathExt.MultiplyInt(world.rnd.Double(0.05, 0.06), mineLocations.Count);
            int gold = MathExt.MultiplyInt(world.rnd.Double(0.03, 0.04), mineLocations.Count);
            int sulfur = MathExt.MultiplyInt(world.rnd.Double(0.14, 0.16), mineLocations.Count);
            int coal = MathExt.MultiplyInt(world.rnd.Double(0.14, 0.16), mineLocations.Count);

            addMines(tin, (int)TerrainMineType.TinOre);
            addMines(cupper, (int)TerrainMineType.CupperOre);
            addMines(lead, (int)TerrainMineType.LeadOre);
            addMines(silver, (int)TerrainMineType.SilverOre);
            addMines(gold, (int)TerrainMineType.GoldOre);
            addMines(sulfur, (int)TerrainMineType.Sulfur);
            addMines(coal, (int)TerrainMineType.Coal);

            for (int i = 0; i < mineLocations.Count; ++i)
            {
                IntVector2 pos = mineLocations[i];

                var subTile = world.subTileGrid.Get(pos);
                subTile.subTerrain = (int)TerrainMineType.IronOre;
                world.subTileGrid.Set(pos, subTile);
            }

            void addMines(int count, int type)
            {
                for (int i = 0; i < count; ++i)
                {
                    int index = world.rnd.Int(mineLocations.Count);
                    IntVector2 pos = mineLocations[index];
                    mineLocations.RemoveAt(index);

                    var subTile = world.subTileGrid.Get(pos);
                    subTile.subTerrain = type;

                    world.subTileGrid.Set(pos, subTile);

                }
            }
        }

    }


    class CityCultureCollection
    { 
        public List<City> LargeGreen = new List<City>();
        public List<City> DryEast = new List<City>();
        public List<City> NorthSea = new List<City>();

        public static readonly CityCulture[] GeneralCultures =
            {
                CityCulture.LargeFamilies,
                CityCulture.Archers,
                CityCulture.Warriors,
                CityCulture.AnimalBreeder,
                CityCulture.Builders,
                CityCulture.CrabMentality,
                CityCulture.Networker,
                CityCulture.Brewmaster,

                CityCulture.Weavers,
                CityCulture.SiegeEngineer,
                CityCulture.Armorsmith,
                CityCulture.Noblemen,
                CityCulture.Backtrader,
                CityCulture.Lawbiding,

                CityCulture.Smelters,
                CityCulture.BronzeCasters,
                CityCulture.Apprentices,

            };
    }

}
