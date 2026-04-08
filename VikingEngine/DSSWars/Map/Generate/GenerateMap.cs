using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Network;
using VikingEngine.ToGG.HeroQuest.Data.UnitAction;

namespace VikingEngine.DSSWars.Map.Generate
{
    enum GenerateMapPass
    {         
        Clear,
        Build,
        Dig,
        AllTerrain,
        CleanUp,
        ClearPopulation,
        SubTiles,
        Cities,
        Countries,
        AllPopulation,
        All,
    }

    class GenerateMap
    {
        public static int LoadStatus = 0;
        
        public WorldData world;

        public bool postComplete = false;
        GenerateRegion region = new GenerateRegion();
        CityCultureCollection cityCultureCollection = new CityCultureCollection();
        public bool abort = false;
        VikingEngine.EngineSpace.Maths.SimplexNoise2D noiseMap;
        BiomsLayout biomsLayout;

        const int ProcessTilesDivisionParts = 8;
        public ForXYLoop ProcessPartLoop(int part)
        {
            int partWidth = world.Size.X / ProcessTilesDivisionParts;
            int startX = partWidth * part;
            var area = new Rectangle2(startX, 0, partWidth, world.Size.Y);
            
            return new ForXYLoop(area);
        }

        public bool GeneratePass(Data.WorldMetaData worldMeta, MapGenerateSettings generateSettings, GenerateMapPass pass, List<Task> extraTasks)
        {
            try
            {
                switch (pass)
                {
                    case GenerateMapPass.Clear:
                        clearCityData();
                        generate_clearpass(worldMeta, generateSettings);
                        break;

                    case GenerateMapPass.Build:
                        generateLandChains(generateSettings);
                        break;

                    case GenerateMapPass.Dig:
                        generateDigChains(generateSettings);
                        break;

                    case GenerateMapPass.AllTerrain:
                        generate_clearpass(worldMeta, generateSettings);
                        generate_allTerrain(generateSettings);
                        if (generateSettings.cleanUpSingleTiles)
                        {
                            generate_cleanup();
                        }
                        break;

                    case GenerateMapPass.CleanUp:
                        generate_cleanup();
                        break;

                    case GenerateMapPass.ClearPopulation:
                        clearCityData();
                        break;

                    case GenerateMapPass.Cities:
                        {
                            clearCityData();

                            generateCities(generateSettings);
                            bindTilesToCities();
                            bool areasuccess = calculateCityAreaSize_success();
                            if (!areasuccess)
                            {
                                return false;
                            }
                        }
                        break;

                    case GenerateMapPass.Countries:
                        generateSubTiles(world);
                        findCityTerrain(generateSettings);

                        factionStartAreas(worldMeta.mapSize, 
                            DssRef.storage.gameRuleset.factionStartSize != FactionStartSize.Full, 
                            generateSettings);
                        break;

                    case GenerateMapPass.AllPopulation:
                        {
                            extraTasks.Add(mountainPeaks());
                            extraTasks.Add(setLowWaterHeightAndWaterHeatmap());
                            world.rnd = new PcgRandom(Ref.rnd.Ushort());
                            clearCityData();

                            generateCities(generateSettings);
                            bindTilesToCities();
                            bool areasuccess = calculateCityAreaSize_success();
                            if (!areasuccess)
                            {
                                return false;
                            }

                            generateSubTiles(world);
                            findCityTerrain(generateSettings);

                            factionStartAreas(worldMeta.mapSize, 
                                DssRef.storage.gameRuleset.factionStartSize != FactionStartSize.Full, 
                                generateSettings);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return false;
            }

            world.generatePassCompleted = pass;
            return true;
        }

        public async Task<bool> Generate(bool save, Data.WorldMetaData worldMeta, MapGenerateSettings generateSettings, List<Task> extraTasks)
        {
            //Debug.Log("Generate map, " + worldMeta.seed);
            try
            {
                generate_clearpass(worldMeta, generateSettings);
                generate_allTerrain(generateSettings);
                //setWaterHeightAndWaterHeatmap();
                extraTasks.Add(mountainPeaks());
                extraTasks.Add(setLowWaterHeightAndWaterHeatmap());

                LoadStatus = 55;
                generateCities(generateSettings);
                LoadStatus = 56;
                bindTilesToCities();
                LoadStatus = 57;

                bool areasuccess = calculateCityAreaSize_success();
                if (!areasuccess)
                {
                    return false;
                }
                LoadStatus = 60;

                generateSubTiles(world);
                
                LoadStatus = 65;

                findCityTerrain(generateSettings);

                if (generateSettings.factionsOnMap)
                {
                    factionStartAreas(worldMeta.mapSize, DssRef.storage.gameRuleset.factionStartSize != FactionStartSize.Full, generateSettings);
                }

                if (save)
                {
                    WorldDataStorage storage = new WorldDataStorage();
                    storage.saveMap(world);
                }

                world.generatePassCompleted = GenerateMapPass.All;
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return false;
            }

        }

        void generate_cleanup()
        {
            var water = new Tile();
            Rectangle2 area = new Rectangle2(IntVector2.Zero, world.Size);
            area.AddRadius(-1);
            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                var tile = world.tileGrid.array[loop.Position.X, loop.Position.Y];
                if (tile.IsLand())
                {
                    for (int dirIx = 0; dirIx < IntVector2.Dir4Array.Length; ++dirIx)
                    {
                        IntVector2 dir = IntVector2.Dir4Array[dirIx];
                        Tile neighbor = world.tileGrid.array[dir.X + loop.Position.X, dir.Y + loop.Position.Y];
                        if (neighbor.IsLand())
                        {
                            goto approved_tile;
                        }

                    }

                    world.tileGrid.array[loop.Position.X, loop.Position.Y] = water;
                }
                else
                {
                    for (int dirIx = 0; dirIx < IntVector2.Dir4Array.Length; ++dirIx)
                    {
                        IntVector2 dir = IntVector2.Dir4Array[dirIx];
                        Tile neighbor = world.tileGrid.array[dir.X + loop.Position.X, dir.Y + loop.Position.Y];
                        if (neighbor.IsWater())
                        {
                            goto approved_tile;
                        }

                    }

                    world.tileGrid.array[loop.Position.X, loop.Position.Y] = world.tileGrid.array[loop.Position.X, loop.Position.Y -1];
                }

            approved_tile:;
            }
        }

        private void generate_allTerrain(MapGenerateSettings generateSettings)
        {
            const int ChainStartStatus = 20;
            const int ChainEndStatus = 50;
            LoadStatus = ChainStartStatus;


            float loadStatusAdd = (float)(ChainEndStatus - ChainStartStatus) / (generateSettings.repeatBuildDigCount * 2);
            float totalAdd = 0;
            for (int i = 0; i < generateSettings.repeatBuildDigCount; i++)
            {
                generateLandChains(generateSettings); //BUILD
                totalAdd += loadStatusAdd;
                LoadStatus = ChainStartStatus + (int)totalAdd;

                generateDigChains(generateSettings); //DIG
                totalAdd += loadStatusAdd;
                LoadStatus = ChainStartStatus + (int)totalAdd;
            }

            LoadStatus = ChainEndStatus;
        }

        void generate_clearpass(Data.WorldMetaData worldMeta, MapGenerateSettings generateSettings)
        {
            world = new WorldData(worldMeta, generateSettings);

            world.availableGenericAiTypes = WorldData.AvailableGenericAiTypes();
            biomsLayout = new BiomsLayout(world.rnd);

            var water = new Tile();
            var land = new Tile();
            land.heightLevel = Height.MinLandHeight;
            land.biom = BiomType.Green;

            switch (generateSettings.StartAs)
            {
                case MapStartAs.Water:
                    {
                        ForXYLoop loop = new ForXYLoop(world.Size);
                        while (loop.Next())
                        {
                            world.tileGrid.Set(loop.Position, water);
                        }
                    }
                    break;

                case MapStartAs.Land:
                    {
                        ForXYLoop loop = new ForXYLoop(world.Size);
                        while (loop.Next())
                        {
                            world.tileGrid.Set(loop.Position, land);
                        }
                    }
                    break;

                case MapStartAs.Circle:
                    {
                        int centerX = world.Size.X / 2;
                        int centerY = world.Size.Y / 2;
                        //int radius = Math.Min(world.Size.X, world.Size.Y) / 3; // Adjust the landmass size

                        ForXYLoop loop = new ForXYLoop(world.Size);
                        while (loop.Next())
                        {
                            Vector2 percPos = loop.Position.Vec / world.Size.Vec - VectorExt.V2Half;
                            //int dx = loop.Position.X - centerX;
                            //int dy = loop.Position.Y - centerY;
                            if (percPos.Length() <= 0.4f)
                            {
                                world.tileGrid.Set(loop.Position, land);
                            }
                            else
                            {
                                world.tileGrid.Set(loop.Position, water);
                            }
                        }
                    }
                    break;
            }
            LoadStatus = 10;
        }

        public void generateSubTiles(WorldData world)
        { 
            this.world = world;
            world.rnd = new PcgRandom(world.metaData.seed);
            noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.metaData.seed);

            //Debug.Log("postLoadGenerate_Part1, " + world.metaData.seed);
            //partComplete = new bool[ProcessSubTileParts];
            //var task = Task.Factory.StartNew(async () =>
            //{
            //    try
            //    {
                    List<Task> tasks = new List<Task>();

                    for (int i = 0; i < ProcessTilesDivisionParts; i++)
                    {
                        int part = i;
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                biomGradient(part);
                            }
                            catch (Exception ex)
                            {
                                BlueScreen.ThreadException = ex;
                            }
                        }));
                    }

            Task.WaitAll(tasks.ToArray());
            tasks.Clear();


                    for (int i = 0; i < ProcessTilesDivisionParts; i++)
                    {
                        int part = i;
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                processSubTiles(part);
                            }
                            catch (Exception ex)
                            {
                                BlueScreen.ThreadException = ex;
                            }
                            
                        }));
                    }


            Task.WaitAll(tasks.ToArray());
            postComplete = true;

                    //new Exception("test");
            //    }
            //    catch (Exception ex)
            //    {
            //        BlueScreen.ThreadException = ex;
            //    }
            //});

        }
 
        public void postLoadGenerate_Part2(WorldData world, SaveStateMeta loadMeta)
        {
            this.world = world;
            world.rnd = new PcgRandom(world.metaData.seed);

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    GenerateRoads roads = new GenerateRoads();

                    if (loadMeta == null)
                    {
                        CityTemplateCollection templateCollection = new CityTemplateCollection();

                        // Create a list to hold the tasks
                        List<Task> tasks = new List<Task>();

                        foreach (var c in world.cities)
                        {
                            City city = c;

                           
                            // Start the task and add it to the list
                            tasks.Add(Task.Factory.StartNew(() =>
                            {
                                try
                                {
                                    city.createBuildingSubtiles(world, templateCollection);
                                }
                                catch (Exception ex)
                                {
                                    BlueScreen.ThreadException = ex;
                                }
                                
                            }));
                        }

                        // Wait for all tasks to complete
                        await Task.WhenAll(tasks);

                        tasks.Clear();

                        foreach (var c in world.cities)
                        {
                            // Start the task and add it to the list
                            tasks.Add(Task.Factory.StartNew(() =>
                            {
                                try
                                {
                                    if (!abort)
                                    {
                                        roads.fromCity(world, c);
                                    }
                                }
                                catch (Exception ex)
                                {
#if DEBUG
                                    BlueScreen.ThreadException = ex;
#endif
                                }
                               
                            }));
                        }

                        // Wait for all tasks to complete
                        await Task.WhenAll(tasks);
                    }
                    postComplete = true;
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
            });
        }

        void generateLandChains(MapGenerateSettings generateSettings)
        {
            int[] mountain = new int[]
            {
                8,
                7,
                6,
                5,
                4,
            };
            int[] hills = new int[]
            {
                7,
                6,
                4,
                3,
                3,
            };
            int[] plain = new int[]
            {
                5,
                5,
                4,
                4,
                3,
            };
            int[] veryplain = new int[]
            {
                4,
                4,
                4,
                3,
                3,
            };


            RandomObjects<int[]> terrainTypes = new RandomObjects<int[]>(
                new ObjectCommonessPair<int[]>(4, mountain),
                new ObjectCommonessPair<int[]>(3, hills),
                new ObjectCommonessPair<int[]>(3, plain),
                new ObjectCommonessPair<int[]>(4, veryplain)
            );

            int numLandChains = Convert.ToInt32(world.areaTileCount / 100f * generateSettings.BuildChainsCount_per100Tiles); //2000;

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

                center = world.rnd.vector2(world.Size.X, world.Size.Y);
                biom = biomsLayout.get(world, center);

                startPos = center;
                newChain(out radius, out growDir, out chainLength, out heightCenter, out heightCenterLength, generateSettings);

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
                    radius = Bound.Set(radius + world.rnd.Plus_MinusF(MaxRadiusChange), generateSettings.LandChainMinRadius, generateSettings.LandChainMaxRadius);

                    center += growDir.Direction(generateSettings.linkPosDiffRange.GetRandom(world.rnd));

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

                            newChain(out radius, out growDir, out chainLength, out heightCenter, out heightCenterLength, generateSettings);
                        }
                    }
                }
            }
        }
        


        

        void newChain(out float radius, out Rotation1D growDir, out int chainLength,
            out Rotation1D heightCenter, out float heightCenterLength, MapGenerateSettings generateSettings)
        {


            radius = lib.SmallestValue(generateSettings.startRadiusRange.GetRandom(world.rnd), generateSettings.startRadiusRange.GetRandom(world.rnd));
            growDir = Rotation1D.Random(world.rnd);
            chainLength = generateSettings.chainLengthRange.GetRandom(world.rnd);

            heightCenter = Rotation1D.Random(world.rnd);
            heightCenterLength = world.rnd.Float(0.7f);
        }
        static readonly IntervalF digLinkPosDiffRange = new IntervalF(0.5f, 2);
        void generateDigChains(MapGenerateSettings generateSettings)
        {
            //int numLandChains = world.areaTileCount / 1800;
            int numLandChains = Convert.ToInt32(world.areaTileCount / 100f * generateSettings.DigChainsCount_per100Tiles);

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
                                    Bound.Min(ref t.heightLevel,  Height.LowerWaterHeight);
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

        Task mountainPeaks()
        {
            var result = Task.Factory.StartNew(async () =>
            {
                List<Task> tasks = new List<Task>();
                for (int part = 0; part < ProcessTilesDivisionParts; ++part)
                {
                    ForXYLoop loop = ProcessPartLoop(part);
                    // Start the task and add it to the list
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            Tile nTile;
                            while (loop.Next())
                            {
                                ref var tile = ref world.tileGrid.array[loop.Position.X, loop.Position.Y];
                                if (tile.heightLevel == Height.MountainLowPeak)
                                {
                                    bool centermountain = true;
                                    foreach (IntVector2 dir in IntVector2.Dir4Array)
                                    {
                                        var npos = loop.Position + dir;
                                        if (world.GetTileSafe(npos, out nTile) && nTile.heightLevel < Height.MountainLowPeak)
                                        {
                                            centermountain = false;
                                            break;
                                        }
                                    }

                                    if (centermountain)
                                    {
                                        tile.heightLevel = Height.MaxHeight;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            BlueScreen.ThreadException = ex;
                        }
                        
                    }));
                }

                // Wait for all tasks to complete
                await Task.WhenAll(tasks);
            });

            return result;
        }

        Task setLowWaterHeightAndWaterHeatmap()
        {
            const int OrthogonalHeat = 10;
            const int DiagonalHeat = 12;

            var result = Task.Factory.StartNew(async () =>
            {
                try
                {
                    {
                        //Clean up all water heights
                        List<Task> tasks = new List<Task>();

                        for (int part = 0; part < ProcessTilesDivisionParts; ++part)
                        {
                            ForXYLoop loop = ProcessPartLoop(part);

                            tasks.Add(Task.Factory.StartNew(() =>
                            {
                                while (loop.Next())
                                {
                                    ref var tile = ref world.tileGrid.array[loop.Position.X, loop.Position.Y];
                                    if (tile.IsWater())
                                    {
                                        tile.heightLevel = Height.DeepWaterHeight;
                                    }
                                }
                            }));
                        }


                        // Wait for all tasks to complete
                        await Task.WhenAll(tasks);
                    }
                    {
                        List<Task> tasks = new List<Task>();

                        for (int part = 0; part < ProcessTilesDivisionParts; ++part)
                        {
                            ForXYLoop loop = ProcessPartLoop(part);
                            // Start the task and add it to the list
                            tasks.Add(Task.Factory.StartNew(() =>
                            {
                                Tile nTile;
                                while (loop.Next())
                                {
                                    ref var tile = ref world.tileGrid.array[loop.Position.X, loop.Position.Y];
                                    if (tile.IsWater())
                                    {
                                        //tile.heightLevel = Height.DeepWaterHeight;

                                        bool landAdjacent = false;
                                        //Check if it has a neighbor tile that is land
                                        foreach (IntVector2 dir in IntVector2.Dir4Array)
                                        {
                                            var npos = loop.Position + dir;
                                            if (world.GetTileSafe(npos, out nTile) && nTile.IsLand())
                                            {
                                                landAdjacent = true;
                                                //Is water to land border
                                                nTile.seaDistanceHeatMap = OrthogonalHeat;


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
                                                    landAdjacent = true;
                                                    //Is water to land border
                                                    if (nTile.seaDistanceHeatMap == int.MinValue)
                                                    {
                                                        nTile.seaDistanceHeatMap = DiagonalHeat;
                                                    }
                                                    //tile.heightLevel = Height.LowWaterHeight;
                                                    if (tile.seaDistanceHeatMap == int.MinValue)
                                                    {
                                                        tile.seaDistanceHeatMap = -DiagonalHeat;
                                                    }
                                                }
                                            }
                                        }

                                        if (landAdjacent)
                                        {
                                            tile.heightLevel = Height.LowWaterHeight;
                                            foreach (IntVector2 dir in IntVector2.Dir4Array)
                                            {
                                                var npos = loop.Position + dir;

                                                if (world.tileGrid.InBounds(npos))
                                                {
                                                    ref var neigborTile = ref world.tileGrid.array[npos.X, npos.Y];
                                                    if (neigborTile.heightLevel == Height.DeepWaterHeight)
                                                    {
                                                        neigborTile.heightLevel = Height.LowerWaterHeight;
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
                                            //Tile nTile;

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
                            }));
                        }


                        // Wait for all tasks to complete
                        await Task.WhenAll(tasks);
                    }
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
                
            });

            return result;
        }

        public const int HeadCityNeededFreeRadius = 14;


        void clearCityData()
        {
            world.factions.Clear();

            if (world.cities != null)
            {
                world.cities = null;

                ForXYLoop loop = new ForXYLoop(world.Size);
                while (loop.Next())
                {
                    Tile tile = world.tileGrid.Get(loop.Position);
                    tile.clearCityData();
                    world.tileGrid.Set(loop.Position, tile);
                }
            }
        }

        void generateCities(MapGenerateSettings generateSettings)
        {

            int numHeadCities = world.areaTileCount / 2000;
            world.cities = new List<City>(numHeadCities);

            switch (DssRef.storage.gameRuleset.factionStartSize)
            { 
                case FactionStartSize.Full:
                    generateSettings.percentageUnclaimed = 0.25f;
                    generateCityType(CityType.Capital, numHeadCities, HeadCityNeededFreeRadius, generateSettings);
                    generateCityType(CityType.Town, numHeadCities * 2, 9, generateSettings);
                    generateCityType(CityType.Village, numHeadCities * 4, 8, generateSettings);
                    break;
                case FactionStartSize.OneCity:
                    generateSettings.percentageUnclaimed = 0.85f;
                    generateCityType(CityType.Village, numHeadCities * 8, 8, generateSettings);
                    break;
                case FactionStartSize.Settler:
                    generateSettings.percentageUnclaimed = 0.85f;
                    generateCityType(CityType.Campsite, numHeadCities * 8, 8, generateSettings);
                    break;
            }            

            world.Init_CityComponents(world.cities.Count);
            foreach (City city in world.cities)
            {
                city.generateCultureAndEconomy(world, cityCultureCollection);
            }
        }
        void generateCityType(CityType type, int amount, float neededSpace, MapGenerateSettings generateSettings)
        {
            

            ConcurrentStack<IntVector2> preppedTiles = new ConcurrentStack<IntVector2>();

            int totalAmount = world.cities.Count + amount;

            Rectangle2 cityArea = world.tileBounds;
            cityArea.AddRadius(-10);
            int loopCount = 0;

            while (world.cities.Count < totalAmount)
            {
                Task prepTask = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        prepAvailableCityTiles();
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }
                    
                });

                bool success = Task.Run(() =>
                {
                    try
                    {
                        while (!prepTask.IsCompleted || preppedTiles.Count > 0)
                        {
                            if (preppedTiles.TryPop(out IntVector2 pos))
                            {
                                if (cityHasNeededSpace(pos))
                                {
                                    var setType = type;
                                    //if (world.rnd.Chance(generateSettings.percentageUnclaimed))
                                    //{
                                    //    setType = CityType.UnClaimed;
                                    //}

                                    City c = new City(world.cities.Count, pos, setType, world);
                                    //c.generateCultureAndEconomy(world, cityCultureCollection);
                                    world.cities.Add(c);

                                    Tile cityTile = world.tileGrid.Get(pos);
                                    cityTile.tileContent = TileContent.City;
                                    world.tileGrid.Set(pos, cityTile);

                                    world.unitCollAreaGrid.add(c);
                                }
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }
                    return true;
                }).Result;

                if (success)
                {
                    loopCount++;
#if DEBUG
                    if (!prepTask.IsCompleted && preppedTiles.Count > 0)
                    {
                        throw new Exception();
                    }
#endif
                }

            }

            void prepAvailableCityTiles()
            {
                const int GoalPrepCount = 8;

                int maxLoops = 1000;
                int prepCount = 0;

                while (--maxLoops > 0 && prepCount < GoalPrepCount)
                {
                    IntVector2 pos = new IntVector2(cityArea.RandomPos(world.rnd));
                    Tile cityTile = world.tileGrid.Get(pos);
                    {
                        if (cityTile.IsLand() && cityTile.heightLevel < Height.MountainHeightStart)
                        {
                            int numWaterTiles = 0;
                            for (int i = 0; i < IntVector2.Dir4Array.Length; ++i)
                            {
                                Tile neighbor = world.tileGrid.Get(pos + IntVector2.Dir4Array[i]);
                                if (neighbor.IsWater())
                                { ++numWaterTiles; }
                            }

                            //Make sure most cities are close to water
                            //pulls its food from the sea or wet land
                            if (numWaterTiles > 0 ||
                                (world.rnd.Chance(0.2f) && cityTile.biom != BiomType.YellowDry))
                            {
                                if (cityHasNeededSpace(pos))
                                {
                                    preppedTiles.Push(pos);
                                }
                            }
                        }
                    }
                }
            }

            bool cityHasNeededSpace(IntVector2 pos)
            {
                if (cityHasEnoughGround(pos))
                {
                    float closestDist;
                    world.closestCity(pos, out closestDist);
                    if (closestDist > neededSpace)
                    {
                        return true;
                    }

                }
                return false;
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

            
            //calc what tiles are in border to eachother
            Rectangle2 area = world.tileBounds;
            area.AddRadius(-1);

            ForXYLoop loop = new ForXYLoop(area);

            while (loop.Next())
            {
                Tile t = world.tileGrid.Get(loop.Position);
                if (t.IsLand())
                {
                    //if (!arraylib.InBound(world.cities, t.CityIndex)) 
                    //{
                    //    lib.DoNothing();
                    //}

                    City owner = world.cities[t.CityIndex];
                    int borderCity = -1;

                    for (int dirIx = 0; dirIx < IntVector2.Dir4Array.Length; ++dirIx)
                    {
                        IntVector2 dir = IntVector2.Dir4Array[dirIx];
                        Tile neighbor = world.tileGrid.array[dir.X + loop.Position.X, dir.Y + loop.Position.Y];
                        bool land = neighbor.IsLand();
                        if (neighbor.CityIndex != owner.myIndex)
                        {
                            t.AddBorder(dirIx, land ? neighbor.CityIndex : Tile.SeaBorder);
                            borderCity = neighbor.CityIndex;
                        }
                    }

                    if (t.BorderCount > 0)
                    {
                        if (!arraylib.InBound(world.cities, borderCity))
                        {
                            lib.DoNothing();
                        }
                        owner.AddNeighborCity(world, borderCity);
                    }

                    world.tileGrid.Set(loop.Position, t);
                }
            }

            
        }

        void findCityTerrain(MapGenerateSettings generateSettings)
        {
            //DssRef.world = world;
            //Calculating start terrain
            List<Task> tasks = new List<Task>();
            foreach (var city in world.cities)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    try
                    {
                        CityStructure cityStructure = new CityStructure();
                        cityStructure.update(world, city, 0, 0);
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            tasks.Clear();

            const int LoopSplit = 8;
            int currentCityIndex = 0;
            int unclaimed = 0;
            
            for (int loop = 0; loop < LoopSplit; loop++)
            {
                int start = currentCityIndex;
                int ex_end = currentCityIndex + world.cities.Count / LoopSplit;
                if (loop == LoopSplit - 1)
                {
                    ex_end = world.cities.Count;
                }

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    try
                    {
                        for (int cityIx = start; cityIx < ex_end; ++cityIx)
                        {
                            if (world.cities[cityIx].terrainStructure.HasIndependantResources() == false)
                            {
                                if (DssRef.storage.gameRuleset.factionStartSize != FactionStartSize.Full || Ref.rnd.Chance(0.75))
                                {
                                    world.cities[cityIx].cityType = CityType.UnClaimed;
                                    unclaimed++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());


            int expectedUnclaimCount = MathExt.MultiplyInt(generateSettings.percentageUnclaimed, world.cities.Count);
            while (expectedUnclaimCount > unclaimed)
            { 
                randomCity().cityType = CityType.UnClaimed;
                unclaimed++;
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

        void factionStartAreas(MapSize mapSize, bool oneCity, MapGenerateSettings generateSettings)
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

            if (DssRef.difficulty.setting_gameMode == GameModeMainType.QuickMatch)
            {
                namedFactionsOnMap_QuickMatch(DssRef.difficulty.QuickMatchPlayerStartSize(), oneCity);
            }
            else
            {
                namedFactionsOnMap(goalWorkForce, oneCity);
            }
            //var last = world.cities.Last();

            foreach (City c in world.cities)
            {
                if (c.factionIndex < 0 && c.cityType > CityType.UnClaimed)
                {
                    int size = goalWorkForce;
                    bool rndEmpire = useRandomEmpires && world.rnd.Chance(0.25);
                    if (rndEmpire)
                    { 
                        size = MathExt.MultiplyInt(randomEmpiresSizeMulti.GetRandom(world.rnd), size);
                    }

                    size = MathExt.MultiplyInt(size, 1.0 - generateSettings.percentageUnclaimed);

                    //region.Reset((int)size);
                    var faction = new Faction(world, FactionType.DefaultAi);
                    int regionCurrentWorkforce = region.GetStartFactionRegion(size, oneCity, c, world, faction);


                    if ((regionCurrentWorkforce >= size && !rndEmpire) || oneCity)
                    {
                        faction.availableForPlayer = true;
                    }
                }
            }

            if (world.factions.Count > DssLib.RtsMaxFactions)
            {
                throw new Exception("RtsMaxFactions");
            }
        }

        void namedFactionsOnMap_QuickMatch(int nationWorkForce, bool oneCity)
        {
            List<FactionType> opponents = new List<FactionType> {
                FactionType.DarkFollower,
                FactionType.UnitedKingdom,
                FactionType.GreenWood,
                FactionType.DyingMonger,
                FactionType.EasternEmpire,
                FactionType.NordicRealm,
                FactionType.DragonSlayer,
                FactionType.BearClaw,
                FactionType.DyingDestru,
            };

            int count = DssRef.difficulty.setting_QuickMatch_PlayerCount - DssRef.storage.playerCount;
            world.quickMatchFactions = new List<int>(count);
            //DssRef.settings.Faction_QuickMatch_Start = -1;
            //DssRef.settings.Faction_QuickMatch_End = -1;

            for (int i = 0; i < count; ++i)
            {               
                var faction = new Faction(world, opponents[i]);
                faction.quickMatchFaction = true;
                faction.displayInFullOverview = true;
                region.GetStartFactionRegion(nationWorkForce, oneCity, randomCity_inMapCenter(), world, faction);

                world.quickMatchFactions.Add(faction.myIndex);
                //if (i == 0)
                //{
                //    DssRef.settings.Faction_QuickMatch_Start = faction.myIndex;
                //    DssRef.settings.Faction_QuickMatch_End = faction.myIndex + count -1;
                //}
            }
        }

        void namedFactionsOnMap(int standardWorkForce, bool oneCity)
        {
            bool bStory = DssRef.difficulty.setting_gameMode == GameModeMainType.FullStory;
            if (bStory)
            {
                var faction = new Faction(world, FactionType.DarkFollower);

                int size = MathExt.MultiplyInt(3, standardWorkForce);
                region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.DarkLands), world, faction);
                
            }

            if (bStory)
            { 
                var faction = new Faction(world, FactionType.UnitedKingdom);

                int size = MathExt.MultiplyInt(5, standardWorkForce);

                region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.WestKingdom), world, faction);
                
            }

            {
                var faction = new Faction(world, FactionType.GreenWood);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.LargeGreen), world, faction);
                
            }

            if (world.metaData.mapSize >= MapSize.Medium)
            {
                {
                    var faction = new Faction(world, FactionType.DyingMonger);

                    int size = MathExt.MultiplyInt(2, standardWorkForce);

                    region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);
                    
                }
                {
                    var faction = new Faction(world, FactionType.DyingHate);

                    int size = MathExt.MultiplyInt(2, standardWorkForce);

                    region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);
                   
                }
                {
                    var faction = new Faction(world, FactionType.DyingDestru);

                    int size = MathExt.MultiplyInt(2, standardWorkForce);

                    region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);
                    
                }

            }

            {
                var faction = new Faction(world, FactionType.EasternEmpire);

                int size = MathExt.MultiplyInt(3, standardWorkForce);

                region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);
                
            }

            {
                var faction = new Faction(world, FactionType.NordicRealm);

                int size = MathExt.MultiplyInt(2, standardWorkForce);

                region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);
                
            }


            if (DateTime.Now.Month == 12 || PlatformSettings.DebugLevel == BuildDebugLevel.Dev)
            {
                var faction = new Faction(world, FactionType.Tomten);

                int size = MathExt.MultiplyInt(0.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);

            }

            {
                var faction = new Faction(world, FactionType.BearClaw);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);
                //region.ApplyFaction(BearClaw);
            }

            {
                var faction = new Faction(world, FactionType.NordicSpur);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);
                //region.ApplyFaction(NordicSpur);
            }

            {
                var faction = new Faction(world, FactionType.IceRaven);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);
                //region.ApplyFaction(IceRaven);
            }

            {
                var faction = new Faction(world, FactionType.DragonSlayer);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, randomCity(), world, faction);
                //region.ApplyFaction(DragonSlayer);
            }



            {
                var faction = new Faction(world, FactionType.BramblebrookHill);
                int size = MathExt.MultiplyInt(0.3, standardWorkForce);
                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.LargeGreen), world, faction);
            }
            {
                var faction = new Faction(world, FactionType.Tumblehill);
                int size = MathExt.MultiplyInt(0.3, standardWorkForce);
                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.LargeGreen), world, faction);
            }
        }

        City collection_pullNextCity(List<City> collection)
        {
            while (collection.Count > 0)
            {
                var city = arraylib.RandomListMemberPop(collection, world.rnd);
                if (city.factionIndex < 0 && city.cityType > CityType.UnClaimed)
                {
                    return city;
                }
            }

            return randomCity();
            
        }



        City randomCity_inMapCenter()
        {
            //if (world.metaData.mapSize > MapSize.Tiny)
            //{
                //Rectangle2 centerArea = new Rectangle2(IntVector2.Zero, world.Size);
                ///// centerArea.
                //centerArea.AddWidthRadius(-world.Size.X / 4);
                //centerArea.AddHeightRadius(-world.Size.Y / 4);
                Rectangle2 centerArea = world.CenterArea();

                int loops = 100;
                while (loops-- > 0)
                {
                    var city = randomCity();
                    if (centerArea.IntersectTilePoint(city.tilePos))
                    {
                        return city;
                    }
                }
            //}
            return randomCity();
        }
        //public Faction getPlayerAvailableFaction(bool firstPlayer, List<Players.LocalPlayer> players)
        //{
        //    const int MultiPlayerDistance = GenerateMap.HeadCityNeededFreeRadius * 8;

        //    Rectangle2 centerArea = new Rectangle2(IntVector2.Zero, Size);
        //    /// centerArea.
        //    centerArea.AddWidthRadius(-Size.X / 4);
        //    centerArea.AddHeightRadius(-Size.Y / 4);

        //    int loops = 0;
        //    while (true)
        //    {
        //        Faction result = factions.GetRandom(Ref.rnd);

        //        if (result.availableForPlayer &&
        //            (centerArea.IntersectPoint(result.mainCity.tilePos) || loops >= 100))
        //        {
        //            if (firstPlayer || loops >= 100)
        //            {
        //                return result;
        //            }
        //            else if (!result.HasPlayerNeighbor() &&
        //                players[0].faction.mainCity.distanceTo(result.mainCity) <= MultiPlayerDistance)
        //            {
        //                return result;
        //            }
        //            ++loops;
        //        }

        //        if (++loops > 1000)
        //        {
        //            throw new EndlessLoopException("getPlayerAvailableFaction");
        //        }
        //    }

        //    //return null;
        //}


        City randomCity()
        {
            int ix = world.rnd.Int(world.cities.Count);

            while (world.cities[ix].factionIndex >= 0 || world.cities[ix].cityType == CityType.UnClaimed)
            {
                ix++;
                if (ix >= world.cities.Count)
                {
                    ix = 0;
                }
            }

            return world.cities[ix];
        }

       
        void biomGradient(int part)
        {
            int partWidth = world.Size.X / ProcessTilesDivisionParts;
            int startX = partWidth * part;
            int endX = startX + partWidth;

            for (int loopy = 0; loopy < world.Size.Y; ++loopy)
            {
                int supTileStartY = loopy * WorldData.TileSubDivitions;
                for (int loopx = startX; loopx < endX; ++loopx)
                {
                    int supTileStartX = loopx * WorldData.TileSubDivitions;

                    ref Tile tile = ref world.tileGrid.array[loopx, loopy]; //lefttop side
                    checkAdj(loopx + 1, loopy, ref tile);
                    checkAdj(loopx, loopy + 1, ref tile);

                    void checkAdj(int x, int y, ref Tile tile)
                    {
                        if (world.tileGrid.InBounds(x, y))
                        {
                            ref Tile ntile = ref world.tileGrid.array[x, y];
                            if (tile.biom != ntile.biom)
                            {
                                tile.secondaryBiom = ntile.biom;
                                tile.secondaryBiomStrength = 2;
                                lowFade(loopx -1, loopy, tile.biom, ntile.biom);
                                lowFade(loopx - 1, loopy -1, tile.biom, ntile.biom);
                                lowFade(loopx, loopy -1, tile.biom, ntile.biom);

                                ntile.secondaryBiom = tile.biom;
                                ntile.secondaryBiomStrength = 2;
                                lowFade(x + 1, y, ntile.biom, tile.biom);
                                lowFade(x + 1, y + 1, ntile.biom, tile.biom);
                                lowFade(x, y + 1, ntile.biom, tile.biom);


                                void lowFade(int x, int y, BiomType fromBiom, BiomType toBiom)
                                {
                                    if (world.tileGrid.InBounds(x, y))
                                    {
                                        ref Tile fadeTile = ref world.tileGrid.array[x, y];
                                        if (fadeTile.biom == fromBiom)
                                        {
                                            fadeTile.secondaryBiom = toBiom;
                                            fadeTile.secondaryBiomStrength = 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void processSubTiles(int part)
        {
            List<IntVector2> mineLocations = new List<IntVector2>(1024);
            List<IntVector2> animalSpawns = new List<IntVector2>(1024);
            const int WidthMin1 = WorldData.TileSubDivitions - 1;

            int partWidth = world.Size.X / ProcessTilesDivisionParts;
            int startX = partWidth * part;
            int endX = startX + partWidth;

            //Debug.Log($"processSubTiles part{part} start{startX}, end{endX}");

            for (int loopy = 0; loopy < world.Size.Y; ++loopy)
            {
                int supTileStartY = loopy * WorldData.TileSubDivitions;
                for (int loopx = startX; loopx < endX; ++loopx)
                {
                    int supTileStartX = loopx * WorldData.TileSubDivitions;

                    Tile tile = world.tileGrid.array[loopx, loopy];
                    var city = world.cities[tile.CityIndex];
                    var cityPos = city.tilePos;
                    float distanceToCity = VectorExt.SideLength(cityPos.X - loopx, cityPos.Y - loopy);
                    IntervalF mudRadius = city.cityType == CityType.UnClaimed ? new IntervalF(0, 1) : new IntervalF(1, 2);

                    Height heightSett = DssRef.map.heigts[tile.heightLevel];
                    Biom biom = DssRef.map.bioms.bioms[(int)tile.biom];
                    Biom secondarybiom = DssRef.map.bioms.bioms[(int)tile.secondaryBiom];

                    int defaultSubType = 0;
                    TerrainMainType tileType;
                    if (tile.IsLand())
                    {
                        tileType = TerrainMainType.DefaultLand;
                        defaultSubType = (int)(tile.heightLevel < Height.MountainHeightStart ? TerrainDefaultLandType.Flat : TerrainDefaultLandType.Mountain);
                    }
                    else
                    {
                        tileType = TerrainMainType.DefaultSea;
                        defaultSubType = (int)(tile.heightLevel == Height.LowWaterHeight? TerrainSeaType.Low : TerrainSeaType.Deep);
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
                            subTile(x, y, groundY, tileType, defaultSubType);
                        }
                    }

                    for (int sidePos = 1; sidePos < WidthMin1; ++sidePos)
                    {
                        subTile(0, sidePos, groundY_w, tileType, defaultSubType);

                        subTile(WidthMin1, sidePos, groundY_e, tileType, defaultSubType);

                        subTile(sidePos, 0, groundY_n, tileType, defaultSubType);

                        subTile(sidePos, WidthMin1, groundY_s, tileType, defaultSubType);
                    }

                    subTile(0, 0, lib.SmallestValue(groundY_w, groundY_n), tileType, defaultSubType);
                    subTile(WidthMin1, 0, lib.SmallestValue(groundY_e, groundY_n), tileType, defaultSubType);
                    subTile(0, WidthMin1, lib.SmallestValue(groundY_w, groundY_s), tileType, defaultSubType);
                    subTile(WidthMin1, WidthMin1, lib.SmallestValue(groundY_s, groundY_e), tileType, defaultSubType);

                    float edgeHeight(int x, int y)
                    {
                        float edgeY = groundY;
                        Tile nTile;
                        if (world.tileGrid.TryGet(loopx + x, loopy + y, out nTile))
                        {
                            edgeY = nTile.GroundY();
                            edgeY = 0.7f * groundY + 0.3f * edgeY;
                        }

                        return edgeY;
                    }

                    void subTile(int x, int y, float topY, TerrainMainType tiletype, int subType)
                    {
                        const int RndRange = 3;

                        int subX = supTileStartX + x;
                        int subY = supTileStartY + y;

                        Color rndColor;

                        var col = biom.TileColor(tile);
                        if (tile.secondaryBiomStrength > 0)
                        {
                            TileColor col2 = secondarybiom.TileColor(tile);
                            col.Color = ColorExt.Mix(col2.Color, col.Color, tile.secondaryBiomStrength * 0.25f); 
                        }

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

                        if (tile.IsWater())
                        {
                            Bound.Max(ref topY, Tile.WaterSurfaceY - Height.DefaultGroundYoffset * 0.5f);
                        }

                        var subTile = new SubTile(tiletype, subType, rndColor, topY);
                        TerrainContent.createSubTileContent(subX, subY, distanceToCity, tile, heightSett, biom, ref mudRadius, ref subTile, world, noiseMap, mineLocations, animalSpawns);

                        world.subTileGrid.Set(subX, subY, subTile);

                    }
                }

            }

            //void addWildAnimals()
            {
                foreach (var pos in animalSpawns)
                {
                    Tile tile =  world.tileGrid.Get(WP.SubtileToTilePos(pos));
                    var biome = world.cities[tile.CityIndex].cityBiome;

                    double rnd = world.rnd.Double();

                    TerrainBuildingType animal;

                    switch (biome)
                    {
                        case CityBiome.Frozen:
                            if (rnd < 0.3)
                            {
                                animal = TerrainBuildingType.OxHabitat;
                            }
                            else
                            {
                                animal = TerrainBuildingType.BoarHabitat;
                            }
                            break;
                        case CityBiome.Forest:
                            if (rnd < 0.5)
                            {
                                animal = TerrainBuildingType.CatHabitat;
                            }
                            else
                            {
                                animal = TerrainBuildingType.BoarHabitat;
                            }
                            break;
                        case CityBiome.Desert:
                            if (rnd < 0.5)
                            {
                                animal = TerrainBuildingType.ElephantHabitat;
                            }
                            else
                            {
                                animal = TerrainBuildingType.PonyHabitat;
                            }
                            break;
                        case CityBiome.Desolate:
                            if (rnd < 0.5)
                            {
                                animal = TerrainBuildingType.WolfHabitat;
                            }
                            else
                            {
                                animal = TerrainBuildingType.DogHabitat;
                            }
                            break;
                        default:
                            if (rnd < 0.1)
                            {
                                animal = TerrainBuildingType.DogHabitat;
                            }
                            else if (rnd < 0.4)
                            {
                                animal = TerrainBuildingType.FowlHabitat;
                            }
                            else if (rnd < 0.6)
                            {
                                animal = TerrainBuildingType.BoarHabitat;
                            }
                            else if (rnd < 0.8)
                            {
                                animal = TerrainBuildingType.OxHabitat;
                            }
                            else
                            {
                                animal = TerrainBuildingType.PonyHabitat;
                            }
                            break;
                    }

                    var subTile = world.subTileGrid.Get(pos);
                    subTile.SetType(TerrainMainType.Building, (int)animal, 1);
                    world.subTileGrid.Set(pos, subTile);
                }
            }


            int mithrilCount = 0;
            switch (world.metaData.mapSize)
            {
               //Tiny, Small, Medium, Large, Huge, Epic
               default:
                    mithrilCount = 1;
                    break;

                case MapSize.Medium:
                    mithrilCount = 2;
                    break;

                case MapSize.Large:
                    mithrilCount = 3;
                    break;

                case MapSize.Huge:
                case MapSize.Epic:
                    mithrilCount = 4;
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

            int tin = MathExt.MultiplyInt(world.rnd.Double(0.08, 0.1), mineLocations.Count);
            int cupper = MathExt.MultiplyInt(world.rnd.Double(0.1, 0.12), mineLocations.Count);
            int lead = MathExt.MultiplyInt(world.rnd.Double(0.07, 0.09), mineLocations.Count);
            int silver = MathExt.MultiplyInt(world.rnd.Double(0.04, 0.05), mineLocations.Count);
            int gold = MathExt.MultiplyInt(world.rnd.Double(0.02, 0.03), mineLocations.Count);
            int sulfur = MathExt.MultiplyInt(world.rnd.Double(0.07, 0.09), mineLocations.Count);
            int salt = MathExt.MultiplyInt(world.rnd.Double(0.12, 0.14), mineLocations.Count);
            int stone = MathExt.MultiplyInt(world.rnd.Double(0.12, 0.14), mineLocations.Count);
            int coal = MathExt.MultiplyInt(world.rnd.Double(0.1, 0.12), mineLocations.Count);

            addMines(tin, (int)TerrainMineType.TinOre);
            addMines(cupper, (int)TerrainMineType.CopperOre);
            addMines(lead, (int)TerrainMineType.LeadOre);
            addMines(silver, (int)TerrainMineType.SilverOre);
            addMines(gold, (int)TerrainMineType.GoldOre);
            addMines(sulfur, (int)TerrainMineType.Sulfur);
            addMines(salt, (int)TerrainMineType.Salt);
            addMines(stone, (int)TerrainMineType.StoneBlock);
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
                if (mineLocations.Count < count)
                {
                    return;
                }

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
        public RandomObjects<CityResurceSeed> CitySeedCommoness;
        //enum CityResurceSeed
        //{
        //    Hen,
        //    Pig,
        //    Mount,
        //    Dog,
        //    Oxen,
        //    Linnen,
        //    Storage,
        //    Bronze,
        //    Iron,
        //    ConservedFood,
        //    Brick,
        //    NUM
        //}

        public List<City> LargeGreen = new List<City>();
        public List<City> DryEast = new List<City>();
        public List<City> NorthSea = new List<City>();

        public List<City> DarkLands = new List<City>();
        public List<City> WestKingdom = new List<City>();


        public CityCultureCollection()
        {
            CitySeedCommoness = new RandomObjects<CityResurceSeed>();
            CitySeedCommoness.AddItem(CityResurceSeed.HenOrPig, 50);
            CitySeedCommoness.AddItem(CityResurceSeed.Mount, 100);
            CitySeedCommoness.AddItem(CityResurceSeed.DogOrOxen, 50);
            CitySeedCommoness.AddItem(CityResurceSeed.Linnen, 25);
            CitySeedCommoness.AddItem(CityResurceSeed.Storage, 25);
            CitySeedCommoness.AddItem(CityResurceSeed.Bronze, 25);
            CitySeedCommoness.AddItem(CityResurceSeed.Iron, 25);
            CitySeedCommoness.AddItem(CityResurceSeed.ConservedFood, 10);
            CitySeedCommoness.AddItem(CityResurceSeed.Brick, 10);

        }

        public static readonly CityCulture[] GeneralCultures =
            {
                CityCulture.LargeFamilies,
                CityCulture.Archers,
                CityCulture.Warriors,
                //CityCulture.AnimalBreeder,
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
                CityCulture.Nomads,

                CityCulture.Butchers, //Larger meat production
                CityCulture.AnimalBreeder2, //Higher chance of successful breeding
                CityCulture.Potters, //Higher pottery production
                CityCulture.Wainwright, //High wagon production
                //CityCulture.Wheelwright, //Speed bonus to conscripted carts
                CityCulture.ShieldMaker, //High shield production
                CityCulture.Nomads, //Low settler cost
                CityCulture.Coopers, //High storage box production
                CityCulture.Salters, //High conserved food production

            };
    }

}
