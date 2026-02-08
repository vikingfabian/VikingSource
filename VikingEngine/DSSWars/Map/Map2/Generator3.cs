using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ.Joust;
using VikingEngine.PJ.Tanks;

namespace VikingEngine.DSSWars.Map.Map2
{
    class Generator3
    {
        const float Height_WaterPlane = 0;
        public const float Height_WaterBottom = Height_WaterPlane - 0.3f;
        public const float Height_LowGround = Height_WaterPlane + 0.1f;
        const float Height_DefaultGround = Height_WaterPlane + 0.2f;
        const float Height_MountainStart = Height_DefaultGround + 0.3f;
        const float Height_MountainPeek = Height_DefaultGround + 0.6f;

        const float LayerAddHeight = 0.15f;
        const float Height_PostNoise = LayerAddHeight * 2.4f;
        const float Height_PostNoiseEdges = Height_PostNoise * 3;
        const float Height_NoiseEdgesAdd = LayerAddHeight;

        LoadingState loadingState = LoadingState.None;
        public WorldData2 world;
        Grid2D<Tile2> dataGrid;
        List<Task> tasks = new List<Task>(64);
        List<Vector2> connectPoints = null;
        EngineSpace.Maths.SimplexNoise2D noiseMap;
        NoiseOptions landNoise = new NoiseOptions(true, 0.1f, 4, 1f, 5f);
        public void generate()
        {
            Task.Run(async () =>
            {
                connectPoints = new List<Vector2>(512);
                world = new WorldData2(MapSize.Medium);
                noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.seed);
                loadingState = LoadingState.Pass;

                dataGrid = world.iconGrid;

                dataGrid.LoopBegin();
                while (dataGrid.LoopNext())
                {
                    var tile = dataGrid.Get(world.iconGrid.LoopPosition);

                    tile.groundY = Height_WaterBottom;
                    dataGrid.Set(dataGrid.LoopPosition, tile);
                }

                int mountainCount = 7;//world.rnd.Int(3, 6);

                for (int i = 0; i < mountainCount; i++)
                {
                    generateMountainChains();
                }
                await Task.WhenAll(tasks);
                tasks.Clear();


                for (int i = 0; i < 4; i++)
                {
                    generateLandChains(60, false, true);
                }
                await Task.WhenAll(tasks);
                tasks.Clear();
                noiseMap.setSeed(world.rnd.Int());

                for (int i = 0; i < 4; i++)
                {
                    generateLandChains(60, false, true);
                }
                await Task.WhenAll(tasks);
                tasks.Clear();
                noiseMap.setSeed(world.rnd.Int());


                for (int i = 0; i < 8; i++)
                {
                    generateLandChains(20, false, true);
                }
                await Task.WhenAll(tasks);
                tasks.Clear();

                for (int i = 0; i < 16; i++)
                {
                    generateLandChains(10, false, true);
                }
                await Task.WhenAll(tasks);
                tasks.Clear();

                for (int i = 0; i < 16; i++)
                {
                    generateLandChains(5, false, true);
                }
                await Task.WhenAll(tasks);
                tasks.Clear();

                addNoiseTexture();
                await Task.WhenAll(tasks);
                tasks.Clear();

                scaleUp4();
                scaleUp8();
                scaleUp4();
                scaleUp4();

                postProcessPixels();

                await Task.WhenAll(tasks);
                tasks.Clear();
                loadingState = LoadingState.Complete;
            });
        }

        void scaleUp4()
        {
            // Initialize the new grid with double the dimensions
            Grid2D<Tile2> largeGrid = new Grid2D<Tile2>(dataGrid.Size * 2);

            // Loop through the original grid coordinates
            for (int x = 0; x < dataGrid.Size.X; x++)
            {
                for (int y = 0; y < dataGrid.Size.Y; y++)
                {
                    // 1. Identify the 2x2 block position in the new grid
                    int lgX = x * 2;
                    int lgY = y * 2;

                    // 2. Get neighbor indices (Clamp to edges to avoid IndexOutOfRange)
                    int nextX = (x + 1 < dataGrid.Size.X) ? x + 1 : x;
                    int nextY = (y + 1 < dataGrid.Size.Y) ? y + 1 : y;

                    // 3. Get the height values of the 4 surrounding source tiles
                    float hTL = dataGrid.array[x, y].groundY;          // Top-Left (Current)
                    float hTR = dataGrid.array[nextX, y].groundY;      // Top-Right
                    float hBL = dataGrid.array[x, nextY].groundY;      // Bottom-Left
                    float hBR = dataGrid.array[nextX, nextY].groundY;  // Bottom-Right

                    // 4. Calculate interpolated heights
                    float avgTop = (hTL + hTR) * 0.5f;
                    float avgLeft = (hTL + hBL) * 0.5f;
                    float avgCenter = (hTL + hTR + hBL + hBR) * 0.25f; // Average of 4 surrounding

                    // 5. Assign to the 2x2 block in largeGrid
                    // Note: We create new structs since Tile2 is a value type
                    largeGrid.array[lgX, lgY] = new Tile2 { groundY = hTL };           // Original
                    largeGrid.array[lgX + 1, lgY] = new Tile2 { groundY = avgTop };    // Right gap
                    largeGrid.array[lgX, lgY + 1] = new Tile2 { groundY = avgLeft };   // Bottom gap
                    largeGrid.array[lgX + 1, lgY + 1] = new Tile2 { groundY = avgCenter }; // Center
                }
            }
            

            dataGrid = largeGrid;
            world.iconGrid = largeGrid;
        }
        void scaleUp8()
        {
            Grid2D<Tile2> largeGrid = new Grid2D<Tile2>(dataGrid.Size * 2);

            for (int x = 0; x < dataGrid.Size.X; x++)
            {
                for (int y = 0; y < dataGrid.Size.Y; y++)
                {
                    int lgX = x * 2;
                    int lgY = y * 2;

                    // 1. Get smoothed values for the 4 corners of the current "quad"
                    // We calculate the 8-neighbor average for each corner involved
                    float sTL = GetSmoothedHeight(x, y);
                    float sTR = GetSmoothedHeight(x + 1, y);
                    float sBL = GetSmoothedHeight(x, y + 1);
                    float sBR = GetSmoothedHeight(x + 1, y + 1);

                    // 2. Interpolate the gaps using these smoothed values
                    float avgTop = (sTL + sTR) * 0.5f;
                    float avgLeft = (sTL + sBL) * 0.5f;
                    float avgCenter = (sTL + sTR + sBL + sBR) * 0.25f;

                    // 3. Assign to Large Grid
                    largeGrid.array[lgX, lgY] = new Tile2 { groundY = sTL };
                    largeGrid.array[lgX + 1, lgY] = new Tile2 { groundY = avgTop };
                    largeGrid.array[lgX, lgY + 1] = new Tile2 { groundY = avgLeft };
                    largeGrid.array[lgX + 1, lgY + 1] = new Tile2 { groundY = avgCenter };
                }
            }

            dataGrid = largeGrid;
        }

        // Helper to get average of a tile and its 8 neighbors
        float GetSmoothedHeight(int cx, int cy)
        {
            // Clamp center to grid bounds to prevent errors at the far edges
            cx = (cx >= dataGrid.Size.X) ? dataGrid.Size.X - 1 : cx;
            cy = (cy >= dataGrid.Size.Y) ? dataGrid.Size.Y - 1 : cy;

            float totalHeight = 0;
            int count = 0;

            // Loop through 3x3 block centered on (cx, cy)
            for (int nx = cx - 1; nx <= cx + 1; nx++)
            {
                for (int ny = cy - 1; ny <= cy + 1; ny++)
                {
                    // Check bounds for neighbors
                    if (nx >= 0 && nx < dataGrid.Size.X && ny >= 0 && ny < dataGrid.Size.Y)
                    {
                        totalHeight += dataGrid.array[nx, ny].groundY;
                        count++;
                    }
                }
            }

            return totalHeight / count;
        }

        void addNoiseTexture()
        {
            const int PostProcessDivs = 8;

            EngineSpace.Maths.SimplexNoise2D noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.seed + 11);
            NoiseOptions postNoise = new NoiseOptions(true, 0.1f, 4, 1f, 30f);
            //NoiseOptions islandNoise = new NoiseOptions(true, 0.1f, 4, 1f, 5f);

            Rectangle2 area = new Rectangle2(dataGrid.Size);
            area.size.X /= 8;

            for (int divIx = 0; divIx < PostProcessDivs; divIx++)
            {
                Rectangle2 divArea = area;
                tasks.Add(Task.Run(() =>
                {
                    ForXYLoop loop = new ForXYLoop(divArea);
                    while (loop.Next())
                    {
                        var tile = dataGrid.Get(loop.Position);
                        tile.groundY -= noiseMap.OctaveNoise2D(postNoise, loop.Position.X, loop.Position.Y) * 0.3f; //*
                                                                                                                    //(tile.groundY < Height_LowGround ? Height_PostNoiseEdges : Height_PostNoise);

                        if (tile.groundY < Height_WaterBottom)
                        { tile.groundY = Height_WaterBottom; }

                        dataGrid.Set(loop.Position, tile);
                    }
                }));
                area.X += area.size.X;
            }
        }


        void postProcessPixels()
        {
            const int PostProcessDivs = 8;

            EngineSpace.Maths.SimplexNoise2D noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.seed + 3);
            NoiseOptions postNoise = new NoiseOptions(true, 0.1f, 4, 1f, 10f);
            //NoiseOptions islandNoise = new NoiseOptions(true, 0.1f, 4, 1f, 5f);

            Rectangle2 area = new Rectangle2(dataGrid.Size);
            area.size.X /= 8;

            for (int divIx = 0; divIx < PostProcessDivs; divIx++)
            {
                Rectangle2 divArea = area;
                tasks.Add(Task.Run(() =>
                {
                    ForXYLoop loop = new ForXYLoop(divArea);
                    while (loop.Next())
                    {
                        var tile = dataGrid.Get(loop.Position);
                        tile.groundY -= noiseMap.OctaveNoise2D(postNoise, loop.Position.X, loop.Position.Y) * 0.1f; //*
                        //(tile.groundY < Height_LowGround ? Height_PostNoiseEdges : Height_PostNoise);

                        if (tile.groundY < Height_WaterBottom)
                        { tile.groundY = Height_WaterBottom; }

                        tileColor(ref tile);
                        dataGrid.Set(loop.Position, tile);
                    }
                }));
                area.X += area.size.X;
            }
        }
        //NoiseOptions generateNoise(PcgRandom rnd, bool use)
        //{
        //    NoiseOptions noiseOptions = new NoiseOptions(use, rnd.Float(), rnd.Float(3, 5), rnd.Float(0.7f, 0.9f), rnd.Float(2, 6));
        //    return noiseOptions;
        //}

        void tileColor(ref Tile2 tile)
        {
            if (tile.groundY > Height_WaterBottom)
            {
                lib.DoNothing();
            }
            if (tile.groundY < 0)
            {
                float depth = 1f - tile.groundY / Height_WaterBottom;
                tile.color = new Color(depth * 0.5f, depth * 0.5f, depth * 0.5f + 0.2f);
            }
            else
            {
                float depth = tile.groundY / Height_MountainPeek;
                depth *= 0.75f;
                tile.color = new Color(depth, depth + 0.2f, depth);
            }
        }

        void generateMountainChains()
        {
            Range chainLengthRange2 = new Range(5, 16);
            Vector2 center = world.rnd.vector2(dataGrid.Size.X, dataGrid.Size.Y);

            Rotation1D growDir = Rotation1D.Random(world.rnd);
            Rotation1D leftDir = Rotation1D.D0;
            Rotation1D rightDir = Rotation1D.D0;
            refreshDirs();

            const float MaxRadius = 8;
            const float MinRadius = 4;

            DrawMapOptions drawMountain = new DrawMapOptions()
            {
                add = false,
                radius = world.rnd.Float(MinRadius, MaxRadius),
                flatness = 0.02f,
                addHeight = Height_MountainPeek,
            };
            drawMountain.refreshRadius();

            DrawMapOptions drawCenterGround = new DrawMapOptions()
            {
                add = true,
                radius = world.rnd.Float(1, 2) * drawMountain.radius,
                flatness = 0.4f,
                addHeight = Height_DefaultGround,
            };
            drawCenterGround.refreshRadius();
            drawCenterGround = drawAddCalc(center, drawCenterGround);

            int leftSide = world.rnd.Int(0, 4);
            int rightSide = world.rnd.Int(1, 20);

            int connectedChains = world.rnd.Int(1, 4);

            int nextCenterGrounds = world.rnd.Int(0, 7);

            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
            {
                int chainLength = chainLengthRange2.GetRandom(world.rnd);

                connectPoints.Add(center);
                for (int link = 0; link < chainLength; ++link)
                {
                    if (nextCenterGrounds <= 0)
                    {
                        nextCenterGrounds = world.rnd.Int(2, 8);
                        startTask_placeDotWithOptions(center, drawCenterGround, true, 2/*, generateNoise(world.rnd, false)*/);
                    }
                    else
                    {
                        nextCenterGrounds--;
                    }

                    bool growSides = link < chainLength / 2;
                    sideLinks(ref leftSide, leftDir, growSides);
                    sideLinks(ref rightSide, rightDir, growSides);
                    drawMountain.centerHeight = drawMountain.addHeight;
                    placeMountainSquare(world.rnd, new IntVector2(center + world.rnd.vector2(new Vector2(drawMountain.radius * 0.4f))), drawMountain/*, generateNoise(world.rnd, true)*/);

                    int sideMountains = world.rnd.Int(0, 5);
                    for (int i = 0; i < sideMountains; i++)
                    {
                        DrawMapOptions draw = drawMountain;
                        float scale = world.rnd.Float(0.5f, 0.9f);
                        draw.centerHeight *= scale;
                        draw.radius *= scale;
                        placeMountainSquare(world.rnd, new IntVector2(center + world.rnd.vector2(new Vector2(drawMountain.radius * 1.7f))), draw/*, generateNoise(world.rnd, true)*/);
                    }

                    if (world.rnd.Chance(0.2))
                    {
                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
                        refreshDirs();
                    }
                    drawMountain.radius = Bound.Set(drawMountain.radius + world.rnd.Plus_MinusF(2f), MinRadius, MaxRadius);

                    center += growDir.Direction(drawMountain.radius * world.rnd.Float(0.5f, 1.4f));

                    //Forward check, no crossing
                    Vector2 forwardCheckPos = center + growDir.Direction(drawMountain.radius);
                    if (dataGrid.TryGet(new IntVector2(forwardCheckPos), out var forwardTile))
                    {
                        if (forwardTile.groundY > 3)
                        {
                            return;
                        }
                    }
                }

                connectPoints.Add(center);

                growDir.Add(world.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(world.rnd.Float(100f, 200f) + drawMountain.radius);

                if (!dataGrid.InBounds(new IntVector2(center)))
                {
                    break;
                }
            }

            void refreshDirs()
            {
                leftDir = growDir;
                leftDir.Add(-MathExt.TauOver4);

                rightDir = growDir;
                rightDir.Add(MathExt.TauOver4);
            }

            void sideLinks(ref int links, Rotation1D dir, bool grow)
            {
                if (links > 0)
                {

                    Vector2 sideCenter = center;

                    DrawMapOptions drawMoutainSide = drawMountain;
                    //drawMoutainSide.add = true;
                    drawMoutainSide.radius *= world.rnd.Float(1f, 3f);
                    drawMoutainSide.flatness = world.rnd.Float(0.1f, 0.3f);
                    drawMoutainSide.addHeight *= world.rnd.Float(0.3f, 0.6f);
                    drawMoutainSide = drawAddCalc(center, drawMoutainSide);

                    for (int link = 0; link < links; ++link)
                    {
                        sideCenter += dir.Direction(drawMountain.radius * world.rnd.Float(0.3f, 0.6f));
                        startTask_placeDotWithOptions(sideCenter, drawMoutainSide, true, 2/*, generateNoise(world.rnd, false)*/);
                        drawMoutainSide.adjustHeight(world.rnd.Float(-0.2f, 0.05f));
                    }

                    if (world.rnd.Chance(Bound.Max(0.1 + links * 0.1, 0.5)))
                    {
                        generateLandChains(sideCenter + world.rnd.vector2_cirkle(8), 20, true);
                    }
                }
                if (world.rnd.Chance(0.1))
                {
                    if (grow)
                    {
                        links += world.rnd.Int(-1, 3);
                    }
                    else
                    {
                        links += world.rnd.Int(-3, 1);
                    }

                    links = Bound.Set(links, 0, 30);
                }

            }
        }

        void placeMountainSquare(PcgRandom rnd, IntVector2 center, DrawMapOptions draw/*, NoiseOptions noiseOpt*/)
        {

            draw.refreshRadius();
            //float noiseCap = new IntervalF(0.9f, 0.3f).GetFromPercent(noiseOpt.smoothness);
            //float radiusPercCap = new IntervalF(0.2f, 0.5f).GetFromPercent(noiseOpt.smoothness);
            //float percFallOffRadius = 1f - radiusPercCap;

            float add = draw.centerHeight - Height_WaterBottom;


            if (rnd.Chance(0.6))
            {
                Rectangle2 area = new Rectangle2(center, (int)draw.radius + 1);
                ForXYLoop loopArea = new ForXYLoop(area);

                while (loopArea.Next())
                {
                    //Create a pyramid shape
                    if (dataGrid.InBounds(loopArea.Position))
                    {
                        int sideLength = loopArea.Position.SideLength(center);

                        float height = (1f - sideLength / draw.radius) * add + Height_WaterBottom;
                        placeTile(loopArea.Position, height, true);
                    }
                }
            }
            else
            {
                draw.radius *= 1.5f;
                Rectangle2 area = new Rectangle2(center, (int)draw.radius + 1);
                ForXYLoop loopArea = new ForXYLoop(area);

                while (loopArea.Next())
                {
                    // Create a pyramid shape, rotated 45 degrees (diamond falloff via Manhattan distance)
                    if (dataGrid.InBounds(loopArea.Position))
                    {
                        int dx = Math.Abs(loopArea.Position.X - center.X);
                        int dy = Math.Abs(loopArea.Position.Y - center.Y);
                        int manhattan = dx + dy;

                        // Normalize so height hits water bottom at L1 distance == radius
                        float t = Math.Min(1f, manhattan / draw.radius);
                        float height = (1f - t) * add + Height_WaterBottom;

                        placeTile(loopArea.Position, height, true);
                    }
                }
            }
        }

        void generateLandChains(float MaxRadius, bool addativeOnly, bool noise)
        {
            Vector2 center = world.rnd.vector2(dataGrid.Size.X, dataGrid.Size.Y);

            while (addativeOnly)
            {
                int maxLoops = 50;

                do
                {
                    if (--maxLoops < 0)
                    {
                        return;
                    }
                    center = world.rnd.vector2(dataGrid.Size.X, dataGrid.Size.Y);
                } while (dataGrid.Get(new IntVector2(center)).groundY < Height_LowGround);
            }


            generateLandChains(center, MaxRadius, noise);
        }

        void generateLandChains(Vector2 center, float MaxRadius, bool noise)
        {

            Range chainLengthRange2 = new Range(2, 20);
            if (MaxRadius < 6)
            {
                chainLengthRange2.Max = 40;
            }

            Rotation1D growDir = Rotation1D.Random(world.rnd);


            DrawMapOptions draw = new DrawMapOptions()
            {
                noiseStrength = world.rnd.Float(0.1f, 1.5f),
                noise = noise,
                add = true,
                radius = world.rnd.Float(MaxRadius * 0.02f, MaxRadius),
                flatness = world.rnd.Float(0.05f, 0.2f),
                addHeight = LayerAddHeight * world.rnd.Float(0.8f, 2f),
            };
            draw = drawAddCalc(center, draw);

            int fractals = 2 + (int)(draw.radius / 10);

            float smoothness = world.rnd.Float();
            int connectedChains = world.rnd.Int(1, 2);

            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
            {
                int chainLength = chainLengthRange2.GetRandom(world.rnd);
                connectPoints.Add(center);
                for (int link = 0; link < chainLength; ++link)
                {
                    startTask_placeDotWithOptions(center + world.rnd.vector2_cirkle(1), draw, false, fractals);

                    if (world.rnd.Chance(0.2))
                    {
                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
                    }
                    if (world.rnd.Chance(0.2))
                    {
                        draw.adjustHeight(world.rnd.Plus_MinusF(0.1f));
                    }
                    draw.radius = Bound.Set(draw.radius + world.rnd.Plus_MinusF(1f), 1, MaxRadius);

                    draw.refreshRadius();
                    center += growDir.Direction(draw.flatRadius * world.rnd.Float(0.6f, 2f));

                }
                connectPoints.Add(center);

                growDir.Add(world.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(world.rnd.Float(10f, 20f) + draw.radius);

                if (!dataGrid.InBounds(new IntVector2(center)))
                {
                    break;
                }
            }

            if (world.rnd.Chance(0.5))
            {
                int islandCount = world.rnd.Int(1, 8);
                for (int i = 0; i < islandCount; ++i)
                {
                    generateIsland(center, draw.radius);
                }
            }
        }

        void generateIsland(Vector2 landCenter, float landRadius)
        {
            if (landRadius > 1)
            {
                int maxLoops = 6;
                Vector2 center = Vector2.Zero;

                do
                {
                    if (--maxLoops < 0)
                    {
                        return;
                    }
                    float distance = world.rnd.Float(1.0f, 5f) * landRadius;
                    center = landCenter + world.rnd.vector2_cirkle(distance);
                } while (!dataGrid.TryGet(new IntVector2(center), out var tile) || tile.groundY > Height_LowGround);

                DrawMapOptions draw = new DrawMapOptions()
                {
                    noiseStrength = world.rnd.Float(0.1f, 1.5f),
                    noise = true,
                    add = true,
                    radius = world.rnd.Float(0.2f, 1.1f) * landRadius,
                    flatness = 0.4f,
                    addHeight = LayerAddHeight * 0.5f,
                };
                draw = drawAddCalc(center, draw);

                startTask_placeDotWithOptions(center, draw, false, 1/*, generateNoise(world.rnd, true)*/);
            }
        }

        void generateDigChains(bool large)
        {
            int maxLoops = 5;
            Vector2 center = Vector2.Zero;

            do
            {
                if (--maxLoops < 0)
                {
                    return;
                }
                center = world.rnd.vector2(dataGrid.Size.X, dataGrid.Size.Y);
            } while (dataGrid.Get(new IntVector2(center)).groundY <= 0);

            const float MinRadius = 2;

            float MaxRadius;
            Range chainLengthRange;
            if (large)
            {
                MaxRadius = 10;
                chainLengthRange = new Range(4, 32);
            }
            else
            {
                MaxRadius = 3;
                chainLengthRange = new Range(2, 16);
            }



            Rotation1D growDir = Rotation1D.Random(world.rnd);

            DrawMapOptions draw = new DrawMapOptions()
            {
                add = true,
                radius = Math.Min(world.rnd.Float(MinRadius, MaxRadius), world.rnd.Float(MinRadius, MaxRadius)),
                flatness = 0.0f,
                addHeight = -Height.DefaultGroundYoffset * world.rnd.Float(0.6f, 1.6f),
            };
            draw = drawAddCalc(center, draw);

            int fractal = draw.radius > 4 ? 2 : 1;

            int connectedChains = world.rnd.Int(1, 4);
            float smoothness = world.rnd.Float();

            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
            {
                int chainLength = chainLengthRange.GetRandom(world.rnd);

                for (int link = 0; link < chainLength; ++link)
                {

                    startTask_placeDotWithOptions(center + world.rnd.vector2_cirkle(8), draw, false, fractal/*, generateNoise(world.rnd, true)*/);

                    if (world.rnd.Chance(0.2))
                    {
                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
                    }
                    draw.radius = Bound.Set(draw.radius + world.rnd.Plus_MinusF(8f), 4, MaxRadius);

                    center += growDir.Direction(draw.radius * world.rnd.Float(0.15f, 0.25f));

                }

                growDir.Add(world.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(world.rnd.Float(10f, 20f) + draw.radius);

                if (!dataGrid.InBounds(new IntVector2(center)))
                {
                    break;
                }
            }
        }

        DrawMapOptions drawAddCalc(Vector2 center, DrawMapOptions draw)
        {
            if (draw.add && dataGrid.TryGet(new IntVector2(center), out var tile))
            {
                if (draw.addHeight > 0)
                {
                    tile.groundY = Bound.Max(tile.groundY, Height_MountainStart);
                }

                draw.centerHeight = draw.addHeight * 0.25f + tile.groundY;

                if (draw.addHeight > 0 && draw.centerHeight < Height_DefaultGround)
                {

                    draw.addHeight += Height_DefaultGround;
                    draw.centerHeight = draw.addHeight;
                    draw.radius += 0.5f;
                    draw.flatness *= 0.5f;

                }

            }
            else
            {
                draw.centerHeight = draw.addHeight;
            }
            return draw;

        }

        void startTask_placeDotWithOptions(Vector2 center, DrawMapOptions draw, bool placeIslands, int fractalDots/*, NoiseOptions noiseOptions*/)
        {
            tasks.Add(Task.Run(() =>
            {
                PcgRandom rnd = new PcgRandom(world.rnd.Ushort());
                placeDotWithOptions(rnd, center, draw, placeIslands, fractalDots/*, noiseOptions*/);
            }));
        }

        void placeDotWithOptions(PcgRandom rnd, Vector2 center, DrawMapOptions draw, bool placeIslands, int fractalDots/*, NoiseOptions noiseOptions*/)
        {
            if (draw.noise)
            {
                placeDot_noise(rnd, center, draw);
            }
            else
            {
                placeDot(rnd, center, draw);
            }

            if (fractalDots > 0 && draw.radius > 6)
            {
                draw.refreshRadius();

                int fractalCount = world.rnd.Int(1, 4);
                IntervalF radiusRange = new IntervalF(0.3f, 0.75f) * draw.radius;
                IntervalF offsetRange = new IntervalF(0.5f, 0.9f) * Bound.Min(draw.flatRadius, 4);

                for (int i = 0; i < fractalCount; ++i)
                {
                    Vector2 offset = world.rnd.vector2_cirkle(offsetRange.GetRandom(rnd));
                    //noiseOptions.useNoise = true;
                    DrawMapOptions drawFractal = draw;
                    drawFractal.radius = radiusRange.GetRandom(rnd);

                    placeDotWithOptions(rnd, center + offset, drawFractal, placeIslands, fractalDots - 1 /*, noiseOptions*/);
                }

            }


            if (placeIslands && world.rnd.Chance(0.1))
            {
                int islandCount = world.rnd.Int(1, 4);
                for (int i = 0; i < islandCount; ++i)
                {
                    generateIsland(center, draw.radius);
                }
            }
        }

        void placeDot(PcgRandom rnd, Vector2 center, DrawMapOptions draw)
        {

            draw.refreshRadius();
            Rectangle2 area = new Rectangle2(new IntVector2(center), (int)draw.radius + 1);
            ForXYLoop loopArea = new ForXYLoop(area);
            while (loopArea.Next())
            {
                if (dataGrid.InBounds(loopArea.Position))
                {
                    Vector2 posDiff = loopArea.Position.Vec - center;
                    float distFromCenter = (posDiff).Length();
                    if (distFromCenter <= draw.radius)
                    {
                        //float percentDist = distFromCenter / draw.radius;
                        placeTile(loopArea.Position, drawHeight(distFromCenter, draw), draw.addHeight > 0);
                    }
                }
            }
        }
        void placeDot_noise(PcgRandom rnd, Vector2 center, DrawMapOptions draw)
        {

            draw.refreshRadius();
            Rectangle2 area = new Rectangle2(new IntVector2(center), (int)draw.radius + 1);
            ForXYLoop loopArea = new ForXYLoop(area);
            while (loopArea.Next())
            {
                if (dataGrid.InBounds(loopArea.Position))
                {
                    Vector2 posDiff = loopArea.Position.Vec - center;
                    float distFromCenter = (posDiff).Length();
                    if (distFromCenter <= draw.radius)
                    {
                        //float percentDist = distFromCenter / draw.radius;
                        float add = Bound.Max( noiseMap.OctaveNoise2D(landNoise, loopArea.Position.X, loopArea.Position.Y) * 2f, 0.1f) * draw.noiseStrength;
                        placeTile(loopArea.Position, drawHeight(distFromCenter, draw) + add, draw.addHeight > 0);
                    }
                }
            }
        }

        float drawHeight(float distFromCenter, DrawMapOptions draw)
        {
            if (distFromCenter < draw.flatRadius)
            {
                return draw.centerHeight;
            }
            else
            {
                float percTowardsEdge = (distFromCenter - draw.flatRadius) / draw.hillRadius;
                return draw.centerHeight * (1f - percTowardsEdge) + draw.edgeHeight * percTowardsEdge;
            }
        }


        void placeTile(IntVector2 pos, float height, bool increase)
        {
            ref var tile = ref dataGrid.GetRef(pos);
            if (increase)
            {
                if (height > tile.groundY)
                {
                    tile.groundY = height;
                    //tile.color = biom.Tile2Color(height);
                }
            }
            else
            {
                if (height < tile.groundY)
                {
                    tile.groundY = height;
                    //tile.color = biom.Tile2Color(height);
                }
            }
        }

        public bool complete()
        {
            return loadingState == LoadingState.Complete;
        }

        enum LoadingState
        {
            None,
            Pass,
            Complete,
        }
    }
}
