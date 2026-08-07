//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VikingEngine.DSSWars.Map.Generate;
//using VikingEngine.DSSWars.Map.Settings;
//using VikingEngine.EngineSpace.Maths;
//using VikingEngine.LootFest.Map.Terrain;
//using VikingEngine.ToGG.Commander.UnitsData;

//namespace VikingEngine.DSSWars.Map.Map2
//{
    

//    class Generator2
//    {

//        //const float LayerHeight = 0.06f;
//        //static readonly float[] TypeToHeight = new float[]
//        //{
//        //    WaterSurfaceY - 0.3f,//Deep water
//        //    WaterSurfaceY - 0.18f,//Deep water
//        //    WaterSurfaceY - 0.07f,//Water_0,
//        //    0f,//OpenField_1,
//        //    LayerHeight,//Plains_2,
//        //    LayerHeight * 2f,//Vegetation_3,
//        //    LayerHeight * 3f,//Hills_4,
//        //    LayerHeight * 4.2f,//Mountain_5,
//        //    LayerHeight * 5.4f,
//        //    LayerHeight * 6.8f,//MountainRidge_6,
//        //};

//        const float Height_WaterPlane = 0;
//        public const float Height_WaterBottom = Height_WaterPlane - 0.3f;
//        public const float Height_LowGround = Height_WaterPlane + 0.1f;
//        const float Height_DefaultGround = Height_WaterPlane + 0.2f;
//        const float Height_MountainStart = Height_DefaultGround + 0.3f;
//        const float Height_MountainPeek = Height_DefaultGround + 0.6f;

//        const float LayerAddHeight = 0.15f;
//        const float Height_PostNoise = LayerAddHeight * 2.4f;
//        const float Height_PostNoiseEdges = Height_PostNoise * 3;
//        const float Height_NoiseEdgesAdd = LayerAddHeight;


//        static readonly IntervalF digLinkPosDiffRange = new IntervalF(0.5f, 2);

//        LoadingState loadingState = LoadingState.None;
//        public WorldData2 world;
//        MapGenerateSettings generateSettings = new MapGenerateSettings();
//        EngineSpace.Maths.SimplexNoise2D noiseMap;
//        List<Task> tasks = new List<Task>(64);
//        Biom biom;
//        List<Vector2> connectPoints = null;
//        public void generate()
//        {
//            biom = DssRef.map.bioms.bioms[(int)BiomType.Green];
//            Task.Run(async () =>
//            {
//                connectPoints = new List<Vector2>(512);
//                world = new WorldData2(MapSize.Medium);
//                noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.seed);
//                loadingState = LoadingState.Pass;

//                world.tileGrid.LoopBegin();
//                while (world.tileGrid.LoopNext())
//                {
//                    var tile = world.tileGrid.Get(world.tileGrid.LoopPosition);
                    
//                    tile.groundY = Height_WaterBottom;
//                    world.tileGrid.Set(world.tileGrid.LoopPosition, tile);
//                }

//                //testDot();
//                ////Test in one thread
//                // generateLandChains(200, false);
//                //generateDigChains(false);


//                //for (int i = 0; i < 20; i++)
//                //{
//                //    generateLandChains(500, false);
//                //}
//                //await Task.WhenAll(tasks);
//                //tasks.Clear();

//                //for (int i = 0; i < 20; i++)
//                //{
//                //    generateDigChains(false);
//                //}
//                //await Task.WhenAll(tasks);
//                //tasks.Clear();

//                int mountainCount = 1;//world.rnd.Int(3, 6);

//                for (int i = 0; i < mountainCount; i++)
//                {
//                    generateMountainChains();
//                }

//                await Task.WhenAll(tasks);
//                tasks.Clear();

//                //for (int repeatBuildDig = 0; repeatBuildDig < 6; ++repeatBuildDig)
//                //{
//                //    for (int i = 0; i < 5; i++)
//                //    {
//                //        generateDigChains(true);
//                //    }

//                for (int i = 0; i < 3; i++)
//                {
//                    generateLandChains(1000, false);
//                }

//                //    await Task.WhenAll(tasks);
//                //    tasks.Clear();

//                //    //for (int i = 0; i < 20; i++)
//                //    //{
//                //    //    generateLandChains(200, true);
//                //    //}

//                await Task.WhenAll(tasks);
//                tasks.Clear();
//                //}

//                var postNoise = generateNoise(world.rnd, true);

//                const int PostProcessDivs = 8;

//                Rectangle2 area = new Rectangle2(world.tileGrid.Size);
//                area.size.X /= 8;

//                for (int divIx = 0; divIx < PostProcessDivs; divIx++)
//                {
//                    Rectangle2 divArea = area;
//                    tasks.Add(Task.Run(() =>
//                    { 
//                        ForXYLoop loop = new ForXYLoop(divArea);
//                        while (loop.Next())
//                        {
//                            var tile = world.tileGrid.Get(loop.Position);
//                            tile.groundY -= noiseMap.OctaveNoise2D_Normal(postNoise, loop.Position.X, loop.Position.Y) * 
//                                (tile.groundY < Height_LowGround? Height_PostNoiseEdges :  Height_PostNoise);
                            
//                            if (tile.groundY < Height_WaterBottom)
//                            { tile.groundY = Height_WaterBottom; }

//                            tileColor(ref tile);
//                            world.tileGrid.Set(loop.Position, tile);
//                        }
//                    }));
//                    area.X += area.size.X;
//                }
//                //world.tileGrid.LoopBegin();
//                //while (world.tileGrid.LoopNext())
//                //{
//                //    var tile = world.tileGrid.Get(world.tileGrid.LoopPosition);
//                //    tile.groundY -= noiseMap.OctaveNoise2D_Normal(postNoise, world.tileGrid.LoopPosition.X, world.tileGrid.LoopPosition.Y) * Height_PostNoise;
//                //    if (tile.groundY < Height_WaterBottom)
//                //    { tile.groundY = Height_WaterBottom; }

//                //    tileColor(ref tile);
//                //    world.tileGrid.Set(world.tileGrid.LoopPosition, tile);
//                //}
//                await Task.WhenAll(tasks);
//                tasks.Clear();

//                loadingState = LoadingState.Complete;
//            });


//        }
//        void generateLandChains(float MaxRadius, bool addativeOnly)
//        {
//            Vector2 center = world.rnd.vector2(world.tileGrid.Size.X, world.tileGrid.Size.Y);

//            while (addativeOnly)
//            {
//                int maxLoops = 50;

//                do
//                {
//                    if (--maxLoops < 0)
//                    {
//                        return;
//                    }
//                    center = world.rnd.vector2(world.tileGrid.Size.X, world.tileGrid.Size.Y);
//                } while (world.tileGrid.Get(new IntVector2(center)).groundY < Height_LowGround);
//            }


//            generateLandChains(center, MaxRadius);
//        }

//        Vector2 pickCenter(bool useConnected)
//        {
//            Vector2 center;

//            if (useConnected && world.rnd.Chance(0.5))
//            {
//               center = arraylib.RandomListMemberPop(connectPoints) + world.rnd.vector2_cirkle(world.rnd.Float(4, 400));

//                if (world.tileGrid.Area.IntersectPoint(new IntVector2(center)))
//                {
//                    return center;
//                }
//            }
            
//            center = world.rnd.vector2(world.tileGrid.Size.X, world.tileGrid.Size.Y);

//            return center;
            
//        }

//        void testDot()
//        {
//            Vector2 center = world.tileGrid.Size.Vec * 0.5f;

//            DrawMapOptions draw = new DrawMapOptions()
//            {
//                add = false,
//                radius = 50,
//                flatness = 0.25f,
//                addHeight = Height_MountainPeek,
//            };

//            placeDotWithOptions(world.rnd,center, draw, 0, generateNoise(world.rnd, false));
//        }

//        void generateLandChains(Vector2 center, float MaxRadius)
//        {
//            Range chainLengthRange2 = new Range(3, 18);

//            Rotation1D growDir = Rotation1D.Random(world.rnd);

            
//            DrawMapOptions draw = new DrawMapOptions()
//            {
//                add = true,
//                radius = world.rnd.Float(MaxRadius * 0.05f, MaxRadius),
//                flatness = world.rnd.Float(0.05f, 0.2f),
//                addHeight = LayerAddHeight * world.rnd.Float(0.8f, 2f),
//            };
//            draw = drawAddCalc(center, draw);

//            int fractals = 2 + (int)(draw.radius / 200);

//            float smoothness = world.rnd.Float();
//            int connectedChains = world.rnd.Int(1, 2);

//            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
//            {
//                int chainLength = chainLengthRange2.GetRandom(world.rnd);
//                connectPoints.Add(center);
//                for (int link = 0; link < chainLength; ++link)
//                {
//                    startTask_placeDotWithOptions(center + world.rnd.vector2_cirkle(8), draw, fractals, generateNoise(world.rnd, false));

//                    if (world.rnd.Chance(0.2))
//                    {
//                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
//                    }
//                    if (world.rnd.Chance(0.2))
//                    {
//                        draw.adjustHeight(world.rnd.Plus_MinusF(0.1f));
//                    }
//                    draw.radius = Bound.Set(draw.radius + world.rnd.Plus_MinusF(8f), 16, MaxRadius);

//                    draw.refreshRadius();
//                    center += growDir.Direction(draw.flatRadius * world.rnd.Float(0.2f, 0.4f));

//                }
//                connectPoints.Add(center);

//                growDir.Add(world.rnd.Plus_MinusF(0.2f));
//                center += growDir.Direction(world.rnd.Float(100f, 200f) + draw.radius);

//                if (!world.tileGrid.InBounds(new IntVector2(center)))
//                {
//                    break;
//                }
//            }

//            if (false )//world.rnd.Chance(0.2))
//            {
//                int islandCount = world.rnd.Int(1, 6);
//                for (int i = 0; i < islandCount; ++i)
//                {
//                    generateIsland(center, draw.radius);
//                }
//            }
//        }

//        void generateIsland(Vector2 landCenter, float landRadius)
//        {
//            if (landRadius > 8)
//            {
//                int maxLoops = 6;
//                Vector2 center = Vector2.Zero;

//                do
//                {
//                    if (--maxLoops < 0)
//                    {
//                        return;
//                    }
//                    float distance = world.rnd.Float(1.2f, 4f) * landRadius;
//                    center = landCenter + world.rnd.vector2_cirkle(distance);
//                } while (!world.tileGrid.TryGet(new IntVector2(center), out var tile) || tile.groundY > 0);

//                DrawMapOptions draw = new DrawMapOptions()
//                {
//                    add = true,
//                    radius = world.rnd.Float(0.1f, 0.4f) * landRadius,
//                    flatness = 0.4f,
//                    addHeight = LayerAddHeight * 0.5f,
//                };
//                draw = drawAddCalc(center, draw);

//                startTask_placeDotWithOptions(center, draw, 1, generateNoise(world.rnd, true));
//            }
//        }

//        void generateDigChains(bool large)
//        {
//            int maxLoops = 5;
//            Vector2 center = Vector2.Zero;

//            do
//            {
//                if (--maxLoops < 0)
//                {
//                    return;
//                }
//               center = world.rnd.vector2(world.tileGrid.Size.X, world.tileGrid.Size.Y);
//            } while (world.tileGrid.Get(new IntVector2(center)).groundY <= 0);

//            const float MinRadius = 8;

//            float MaxRadius;
//            Range chainLengthRange;
//            if (large)
//            {
//                MaxRadius = 50;
//                chainLengthRange = new Range(4, 32);
//            }
//            else
//            {
//                MaxRadius = 32;
//                chainLengthRange = new Range(2, 16);
//            }
            
            

//            Rotation1D growDir = Rotation1D.Random(world.rnd);

//            DrawMapOptions draw = new DrawMapOptions()
//            {
//                add = true,
//                radius = Math.Min(world.rnd.Float(MinRadius, MaxRadius), world.rnd.Float(MinRadius, MaxRadius)),
//                flatness = 0.0f,
//                addHeight = -Height.DefaultGroundYoffset * world.rnd.Float(0.6f, 1.6f),
//            };
//            draw = drawAddCalc(center, draw);

//            int fractal = draw.radius > 30 ? 2 : 1;

//            int connectedChains = world.rnd.Int(1, 4);
//            float smoothness = world.rnd.Float();

//            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
//            {
//                int chainLength = chainLengthRange.GetRandom(world.rnd);

//                for (int link = 0; link < chainLength; ++link)
//                {

//                    startTask_placeDotWithOptions(center + world.rnd.vector2_cirkle(8), draw, fractal, generateNoise(world.rnd, true));

//                    if (world.rnd.Chance(0.2))
//                    {
//                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
//                    }
//                    draw.radius = Bound.Set(draw.radius + world.rnd.Plus_MinusF(8f), 4, MaxRadius);

//                    center += growDir.Direction(draw.radius * world.rnd.Float(0.15f, 0.25f));

//                }

//                growDir.Add(world.rnd.Plus_MinusF(0.2f));
//                center += growDir.Direction(world.rnd.Float(100f, 200f) + draw.radius);

//                if (!world.tileGrid.InBounds(new IntVector2(center)))
//                {
//                    break;
//                }
//            }
//        }

//        void generateMountainChains()
//        {
//            Range chainLengthRange2 = new Range(5, 30);
//            Vector2 center = world.rnd.vector2(world.tileGrid.Size.X, world.tileGrid.Size.Y);
            
//            Rotation1D growDir = Rotation1D.Random(world.rnd);
//            Rotation1D leftDir = Rotation1D.D0;
//            Rotation1D rightDir = Rotation1D.D0;
//            refreshDirs();

//            const float MaxRadius = 80;
//            const float MinRadius = 40;


//            DrawMapOptions drawMountain = new DrawMapOptions()
//            {
//                add = false,
//                radius = world.rnd.Float(MinRadius, MaxRadius),
//                flatness = 0.02f,
//                addHeight = Height_MountainPeek,
//            };
//            drawMountain.refreshRadius();

//            DrawMapOptions drawCenterGround = new DrawMapOptions()
//            {
//                add = true,
//                radius = world.rnd.Float(4, 7) * drawMountain.radius,
//                flatness = 0.4f,
//                addHeight = Height_DefaultGround,
//            };
//            drawCenterGround.refreshRadius();
//            drawCenterGround = drawAddCalc(center, drawCenterGround);

//            int leftSide = world.rnd.Int(0, 8);
//            int rightSide = world.rnd.Int(1, 32);

//            int connectedChains = world.rnd.Int(1, 4);

//            int nextCenterGrounds = world.rnd.Int(0, 7);

//            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
//            {
//                int chainLength = chainLengthRange2.GetRandom(world.rnd);

//                connectPoints.Add(center);
//                for (int link = 0; link < chainLength; ++link)
//                {
//                    if (nextCenterGrounds <= 0)
//                    {
//                        nextCenterGrounds = world.rnd.Int(2, 8);
//                        startTask_placeDotWithOptions(center, drawCenterGround, 2, generateNoise(world.rnd, false));
//                    }
//                    else
//                    {
//                        nextCenterGrounds--;
//                    }

//                    bool growSides = link < chainLength / 2;
//                    sideLinks(ref leftSide, leftDir, growSides);
//                    sideLinks(ref rightSide, rightDir, growSides);
//                    drawMountain.centerHeight = drawMountain.addHeight;
//                    placeMountainSquare(world.rnd, new IntVector2(center + world.rnd.vector2(new Vector2(drawMountain.radius * 0.4f))), drawMountain, generateNoise(world.rnd, true));

//                    int sideMountains = world.rnd.Int(0, 5);
//                    for (int i = 0; i < sideMountains; i++)
//                    {
//                        DrawMapOptions draw = drawMountain;
//                        float scale = world.rnd.Float(0.5f, 0.9f);
//                        draw.centerHeight *= scale;
//                        draw.radius *= scale;
//                        placeMountainSquare(world.rnd, new IntVector2(center + world.rnd.vector2(new Vector2(drawMountain.radius * 1.7f))), draw, generateNoise(world.rnd, true));
//                    }

//                    if (world.rnd.Chance(0.2))
//                    {
//                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
//                        refreshDirs();
//                    }
//                    drawMountain.radius = Bound.Set(drawMountain.radius + world.rnd.Plus_MinusF(2f), MinRadius, MaxRadius);

//                    center += growDir.Direction(drawMountain.radius * world.rnd.Float(0.4f, 0.8f));

//                    //Forward check, no crossing
//                    Vector2 forwardCheckPos = center + growDir.Direction(drawMountain.radius);
//                    if (world.tileGrid.TryGet(new IntVector2(forwardCheckPos), out var forwardTile))
//                    {
//                        if (forwardTile.groundY > 3)
//                        {
//                            return;
//                        }
//                    }
//                }

//                connectPoints.Add(center);

//                growDir.Add(world.rnd.Plus_MinusF(0.2f));
//                center += growDir.Direction(world.rnd.Float(100f, 200f) + drawMountain.radius);

//                if (!world.tileGrid.InBounds(new IntVector2(center)))
//                {
//                    break;
//                }
//            }

//            void refreshDirs()
//            {
//                leftDir = growDir;
//                leftDir.Add(-MathExt.TauOver4);

//                rightDir = growDir;
//                rightDir.Add(MathExt.TauOver4);
//            }

//            void sideLinks(ref int links, Rotation1D dir, bool grow)
//            {
//                if (links > 0)
//                {

//                    Vector2 sideCenter = center;

//                    DrawMapOptions drawMoutainSide = drawMountain;
//                    //drawMoutainSide.add = true;
//                    drawMoutainSide.radius *= world.rnd.Float(3f, 5f);
//                    drawMoutainSide.flatness = world.rnd.Float(0.1f, 0.3f);
//                    drawMoutainSide.addHeight *= world.rnd.Float(0.3f, 0.6f);
//                    drawMoutainSide = drawAddCalc(center, drawMoutainSide);

//                    for (int link = 0; link < links; ++link)
//                    {
//                        sideCenter += dir.Direction(drawMountain.radius * world.rnd.Float(0.3f, 0.6f));
//                        startTask_placeDotWithOptions(sideCenter, drawMoutainSide, 2, generateNoise(world.rnd, false));
//                        drawMoutainSide.adjustHeight(world.rnd.Float(-0.2f, 0.05f));
//                    }

//                    if (world.rnd.Chance(Bound.Max(0.1 + links * 0.1, 0.5)))
//                    {
//                        generateLandChains(sideCenter + world.rnd.vector2_cirkle(20), 150);
//                    }
//                }
//                    if (world.rnd.Chance(0.1))
//                    {
//                        if (grow)
//                        {
//                            links += world.rnd.Int(-1, 3);
//                        }
//                        else
//                        {
//                            links += world.rnd.Int(-3, 1);
//                        }

//                        links = Bound.Set(links, 0, 30);
//                    }
                
//            }
//        }


        

//        NoiseOptions generateNoise(PcgRandom rnd, bool use)
//        { 
//            NoiseOptions noiseOptions = new NoiseOptions(use, rnd.Float(), rnd.Float(3, 5), rnd.Float(0.7f, 0.9f), rnd.Float(2, 6));
//            return noiseOptions;
//        }

//        void startTask_placeDotWithOptions(Vector2 center, DrawMapOptions draw, int fractalDots, NoiseOptions noiseOptions)
//        {
//            tasks.Add(Task.Run(() =>
//            {
//                PcgRandom rnd = new PcgRandom(world.rnd.Ushort());
//                placeDotWithOptions(rnd, center, draw, fractalDots, noiseOptions);
//            }));
//        }

//        void placeDotWithOptions(PcgRandom rnd, Vector2 center, DrawMapOptions draw, int fractalDots, NoiseOptions noiseOptions)
//        {
//            if (noiseOptions.useNoise)
//            {
//                placeDot_noise(rnd, center, draw, noiseOptions);
//            }
//            else
//            {
//                placeDot(rnd, center, draw);
//            }

//            if (fractalDots > 0 && draw.radius > 6)
//            {
//                draw.refreshRadius();

//                int fractalCount = world.rnd.Int(4, 12);
//                IntervalF radiusRange = new IntervalF(0.3f, 0.75f) * draw.radius;
//                IntervalF offsetRange = new IntervalF(0.5f, 0.9f) * Bound.Min(draw.flatRadius, 4);

//                for (int i = 0; i < fractalCount; ++i)
//                {
//                    Vector2 offset = world.rnd.vector2_cirkle(offsetRange.GetRandom(rnd));
//                    noiseOptions.useNoise = true;
//                    DrawMapOptions drawFractal = draw;
//                    drawFractal.radius = radiusRange.GetRandom(rnd);
                   
//                    placeDotWithOptions(rnd, center + offset, drawFractal, fractalDots - 1, noiseOptions);
//                }

//            }
//        }


//        void placeDot(PcgRandom rnd, Vector2 center, DrawMapOptions draw)
//        {
            
//            draw.refreshRadius();
//            Rectangle2 area = new Rectangle2(new IntVector2(center), (int)draw.radius + 1);
//            ForXYLoop loopArea = new ForXYLoop(area);
//            while (loopArea.Next())
//            {
//                if (world.tileGrid.InBounds(loopArea.Position))
//                {
//                    Vector2 posDiff = loopArea.Position.Vec - center;
//                    float distFromCenter = (posDiff).Length();
//                    if (distFromCenter <= draw.radius)
//                    {                        
//                        //float percentDist = distFromCenter / draw.radius;
//                        placeTile(loopArea.Position, drawHeight(distFromCenter, draw), draw.addHeight > 0);
//                    }
//                }
//            }
//        }
//        void placeDot_noise(PcgRandom rnd, Vector2 center, DrawMapOptions draw, NoiseOptions noiseOpt)
//        {
            
//            draw.refreshRadius();
//            float noiseCap = new IntervalF(0.9f, 0.3f).GetFromPercent(noiseOpt.smoothness);
//            float radiusPercCap = new IntervalF(0.2f, 0.5f).GetFromPercent(noiseOpt.smoothness);
//            float percFallOffRadius = 1f - radiusPercCap;

//            Rectangle2 area = new Rectangle2(new IntVector2(center), (int)draw.radius + 1);
//            ForXYLoop loopArea = new ForXYLoop(area);
//            while (loopArea.Next())
//            {
//                if (world.tileGrid.InBounds(loopArea.Position))
//                {
//                    Vector2 posDiff = loopArea.Position.Vec - center;
//                    float distFromCenter = (posDiff).Length();
//                    if (distFromCenter <= draw.radius)
//                    {
//                        float radiusPerc = distFromCenter / draw.radius;

//                        float height = drawHeight(distFromCenter, draw);

//                        if (radiusPerc < radiusPercCap)// ||  ((radiusPerc - radiusPercCap) / percFallOffRadius) * noiseCap)
//                        {
//                            //float percentDist = distFromCenter / draw.radius;
//                            placeTile(loopArea.Position, height, draw.addHeight > 0);
//                        }
//                        else
//                        {
//                            float noise = noiseMap.OctaveNoise2D_Normal(noiseOpt, -loopArea.Position.X, loopArea.Position.Y) * 1.2f;

//                            //float noiseEffect = (radiusPerc - radiusPercCap) / percFallOffRadius;
//                            //noise *= noiseEffect;
//                            height = height * noise + draw.edgeHeight * (1- noise) - noise * LayerAddHeight;//
//                            placeTile(loopArea.Position, height, draw.addHeight > 0);
//                        }
//                    }
//                }
//            }
//        }

//        void placeMountainSquare(PcgRandom rnd, IntVector2 center, DrawMapOptions draw, NoiseOptions noiseOpt)
//        {

//            draw.refreshRadius();
//            float noiseCap = new IntervalF(0.9f, 0.3f).GetFromPercent(noiseOpt.smoothness);
//            float radiusPercCap = new IntervalF(0.2f, 0.5f).GetFromPercent(noiseOpt.smoothness);
//            float percFallOffRadius = 1f - radiusPercCap;

//            float add = draw.centerHeight - Height_WaterBottom;
            

//            if (rnd.Chance(0.6))
//            {
//                Rectangle2 area = new Rectangle2(center, (int)draw.radius + 1);
//                ForXYLoop loopArea = new ForXYLoop(area);

//                while (loopArea.Next())
//                {
//                    //Create a pyramid shape
//                    if (world.tileGrid.InBounds(loopArea.Position))
//                    {
//                        int sideLength = loopArea.Position.SideLength(center);

//                        float height = (1f - sideLength / draw.radius) * add + Height_WaterBottom;
//                        placeTile(loopArea.Position, height, true);
//                    }
//                }
//            }
//            else
//            {
//                draw.radius *= 1.5f;
//                Rectangle2 area = new Rectangle2(center, (int)draw.radius + 1);
//                ForXYLoop loopArea = new ForXYLoop(area);

//                while (loopArea.Next())
//                {
//                    // Create a pyramid shape, rotated 45 degrees (diamond falloff via Manhattan distance)
//                    if (world.tileGrid.InBounds(loopArea.Position))
//                    {
//                        int dx = Math.Abs(loopArea.Position.X - center.X);
//                        int dy = Math.Abs(loopArea.Position.Y - center.Y);
//                        int manhattan = dx + dy;

//                        // Normalize so height hits water bottom at L1 distance == radius
//                        float t = Math.Min(1f, manhattan / draw.radius);
//                        float height = (1f - t) * add + Height_WaterBottom;

//                        placeTile(loopArea.Position, height, true);
//                    }
//                }
//            }
//        }

//        DrawMapOptions drawAddCalc(Vector2 center, DrawMapOptions draw)
//        {
//            if (draw.add && world.tileGrid.TryGet(new IntVector2(center), out var tile))
//            {
//                if (draw.addHeight > 0)
//                {
//                    tile.groundY = Bound.Max(tile.groundY, Height_MountainStart);
//                }

//                draw.centerHeight = draw.addHeight * 0.25f + tile.groundY;

//                if (draw.addHeight > 0 && draw.centerHeight < Height_DefaultGround)
//                {

//                    draw.addHeight += Height_DefaultGround;
//                    draw.centerHeight = draw.addHeight;
//                    draw.radius += 0.5f;
//                    draw.flatness *= 0.5f;

//                }

//            }
//            else
//            {
//                draw.centerHeight = draw.addHeight;
//            }
//            return draw;
            
//        }

//        float drawHeight(float distFromCenter, DrawMapOptions draw)
//        {
//            if (distFromCenter < draw.flatRadius)
//            {
//                return draw.centerHeight;
//            }
//            else
//            { 
//                float percTowardsEdge = (distFromCenter - draw.flatRadius) / draw.hillRadius;
//                return draw.centerHeight * (1f -percTowardsEdge) + draw.edgeHeight * percTowardsEdge;                
//            }
//        }

//        void placeTile(IntVector2 pos, float height, bool increase)
//        { 
//            ref var tile = ref world.tileGrid.GetRef(pos);
//            if (increase)
//            {
//                if (height > tile.groundY)
//                {
//                    tile.groundY = height;
//                    //tile.color = biom.Tile2Color(height);
//                }
//            }
//            else
//            {
//                if (height < tile.groundY)
//                {
//                    tile.groundY = height;
//                    //tile.color = biom.Tile2Color(height);
//                }
//            }
//        }

//        void tileColor(ref GenTile tile)
//        {
//            if (tile.groundY > Height_WaterBottom)
//            {
//                lib.DoNothing();
//            }
//            if (tile.groundY < 0)
//            {
//                float depth = 1f - tile.groundY / Height_WaterBottom;
//                tile.color = new Color(depth * 0.5f, depth * 0.5f, depth * 0.5f + 0.2f);
//            }
//            else
//            {
//                float depth = tile.groundY / Height_MountainPeek;
//                depth *= 0.75f;
//                tile.color = new Color(depth, depth + 0.2f, depth);
//            }
//        }

//        public bool complete()
//        { 
//            return loadingState == LoadingState.Complete;
//        }

//        enum LoadingState
//        {
//            None,
//            Pass,
//            Complete,
//        }
//    }

//}
