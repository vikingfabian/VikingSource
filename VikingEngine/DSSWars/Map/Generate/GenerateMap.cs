using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ.Joust;
using VikingEngine.PJ.SmashBirds;
using VikingEngine.PJ.Tanks;
using VikingEngine.ToGG.Commander.UnitsData;
using VikingEngine.ToGG.MoonFall;

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
        //bool[] partComplete;
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
            //int endX = startX + partWidth;
            var area = new Rectangle2(startX, 0, partWidth, world.Size.Y);
            //area.size -= 1;
            return new ForXYLoop(area);
        }

        //IntervalF[] citySizeToMudRadius = new IntervalF[]
        //{
        //    new IntervalF(1, 1),
        //    new IntervalF(1, 1),
        //    new IntervalF(5, 7),
        //};

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

                            generateCities();
                            bindTilesToCities();
                            bool areasuccess = calculateCityAreaSize_success();
                            if (!areasuccess)
                            {
                                return false;
                            }
                        }
                        break;

                    case GenerateMapPass.Countries:
                        factionStartAreas(worldMeta.mapSize);
                        break;

                    case GenerateMapPass.AllPopulation:
                        {
                            extraTasks.Add(mountainPeaks());
                            extraTasks.Add(setLowWaterHeightAndWaterHeatmap());
                            world.rnd = new PcgRandom(Ref.rnd.Ushort());
                            clearCityData();

                            generateCities();
                            bindTilesToCities();
                            bool areasuccess = calculateCityAreaSize_success();
                            if (!areasuccess)
                            {
                                return false;
                            }

                            factionStartAreas(worldMeta.mapSize);
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

        public bool Generate(bool save, Data.WorldMetaData worldMeta, MapGenerateSettings generateSettings, List<Task> extraTasks)
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

                if (generateSettings.factionsOnMap)
                {
                    factionStartAreas(worldMeta.mapSize);
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

        public void postLoadGenerate_Part1(WorldData world)
        { 
            this.world = world;
            world.rnd = new PcgRandom(world.metaData.seed);
            noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.metaData.seed);

            //Debug.Log("postLoadGenerate_Part1, " + world.metaData.seed);
            //partComplete = new bool[ProcessSubTileParts];
            var task = Task.Factory.StartNew(async () =>
            {
                try
                {
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

                    await Task.WhenAll(tasks);
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


                    await Task.WhenAll(tasks);
                    postComplete = true;

                    //new Exception("test");
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
            });

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
                                    BlueScreen.ThreadException = ex;
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

        void generateCities()
        {
            int numHeadCities = world.areaTileCount / 2000;
            world.cities = new List<City>(numHeadCities);

            generateCityType(CityType.Capital, numHeadCities, HeadCityNeededFreeRadius);
            generateCityType(CityType.Town, numHeadCities * 2, 9);
            generateCityType(CityType.Village, numHeadCities * 4, 8);
        }
        void generateCityType(CityType type, int amount, float neededSpace)
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
                                    City c = new City(world.cities.Count, pos, type, world);
                                    c.generateCultureAndEconomy(world, cityCultureCollection);
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
                        owner.AddNeighborCity(borderCity);
                    }

                    world.tileGrid.Set(loop.Position, t);
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

            //var last = world.cities.Last();

            foreach (City c in world.cities)
            {
                //if (c == last)
                //{
                //    lib.DoNothing();
                //}
                //c.SetStartFaction(goalWorkForce, world.factions, world);

                if (c.factionIndex < 0)
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
                if (c.factionIndex < 0)
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
                region.GetStartFactionRegion(size, collection_pullNextCity(cityCultureCollection.DarkLands), world, faction);
                //region.ApplyFaction(DarkFollower);
            }

            { 
                var faction = new Faction(world, FactionType.UnitedKingdom);

                int size = MathExt.MultiplyInt(5, standardWorkForce);

                region.GetStartFactionRegion(size, collection_pullNextCity(cityCultureCollection.WestKingdom), world, faction);
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
                if (city.factionIndex < 0)
                {
                    return city;
                }
            }

            return randomCity();
            
        }

        City randomCity()
        {
            int ix = world.rnd.Int(world.cities.Count);

            while (world.cities[ix].factionIndex >= 0)
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
                    IntervalF mudRadius = new IntervalF(1, 2);

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
            addMines(cupper, (int)TerrainMineType.CopperOre);
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
        public List<City> LargeGreen = new List<City>();
        public List<City> DryEast = new List<City>();
        public List<City> NorthSea = new List<City>();

        public List<City> DarkLands = new List<City>();
        public List<City> WestKingdom = new List<City>();


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
