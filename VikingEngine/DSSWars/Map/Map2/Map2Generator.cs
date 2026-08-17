using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.EngineSpace.Maths;

namespace VikingEngine.DSSWars.Map.Map2
{
    class Map2Generator
    {
        const float Height_WaterPlane = 0;
        public const float Height_WaterBottom = Height_WaterPlane - 0.3f;
        public const float Height_LowGround = Height_WaterPlane + 0.1f;
        const float Height_DefaultGround = Height_WaterPlane + 0.2f;
        const float Height_MountainStart = Height_DefaultGround + 0.3f;
        const float Height_MountainPeek = Height_DefaultGround + 0.6f;

        const float LayerAddHeight = 0.15f;
        const float Height_PostNoise = LayerAddHeight * 2.4f;

        LoadingState loadingState = LoadingState.None;
        public IconWorldData iconWorld;
        public WorldData2 world;
        Grid2D_L<GenTile> dataGrid;
        List<Task> tasks = new List<Task>(64);
        List<Vector2> connectPoints = null;
        EngineSpace.Maths.SimplexNoise2D noiseMap;
        NoiseOptions landNoise = new NoiseOptions(true, 0.1f, 4, 1f, 5f);
        public void generate(Map2GenerateSettings generateSettings)
        {
            Task.Run(async () =>
            {
                connectPoints = new List<Vector2>(512);

                WorldData.SizeDimentions(MapSize.Medium).Area();

                iconWorld = new IconWorldData(generateSettings.IconSize());
                
                noiseMap = new EngineSpace.Maths.SimplexNoise2D(iconWorld.metaData2.seed);
                noiseMap.setSeed(iconWorld.rnd.Int());
                loadingState = LoadingState.Pass;

                dataGrid = iconWorld.iconGrid;

                dataGrid.LoopBegin();
                while (dataGrid.LoopNext())
                {
                    var tile = dataGrid.Get(iconWorld.iconGrid.LoopPosition);

                    tile.groundY = Height_WaterBottom;
                    dataGrid.Set(dataGrid.LoopPosition, tile);
                }

                for (int i = 0; i < 2; i++)//3
                {
                    generateLargeIsland(70);
                }
                for (int i = 0; i < 4; i++)
                {
                    generateLargeIsland(40);
                }
                for (int i = 0; i < 4; i++)
                {
                    generateLargeIsland(20);
                }
                await Task.WhenAll(tasks);
                tasks.Clear();               
                
                for (int i = 0; i < 20; i++)
                {
                    generateDigChains(true);
                }
                for (int i = 0; i < 40; i++)
                {
                    generateDigChains(false);
                }
                await Task.WhenAll(tasks);
                tasks.Clear();

                int mountainCount = 9;

                for (int i = 0; i < mountainCount; i++)
                {
                    generateMountainChains();
                }
                await Task.WhenAll(tasks);
                tasks.Clear();


                for (int i = 0; i < 8; i++)
                {
                    generateLandChains(20, true, true);
                }
                await Task.WhenAll(tasks);
                tasks.Clear();

                for (int i = 0; i < 16; i++)
                {
                    generateLandChains(10, true, true);
                }
                await Task.WhenAll(tasks);
                tasks.Clear();

                for (int i = 0; i < 20; i++)
                {
                    generateHills(10, iconWorld.rnd.Int(8));
                }
                for (int i = 0; i < 20; i++)
                {
                    generateHills(50, iconWorld.rnd.Int(8));
                }
                await Task.WhenAll(tasks);
                tasks.Clear();
                //END


                addNoiseTexture();
                await Task.WhenAll(tasks);
                tasks.Clear();


                //SmoothMap();

                //world = new WorldData2(iconWorld);
                //todo clone
                //scaleUp16x();

                postProcessPixels();

                await Task.WhenAll(tasks);
                tasks.Clear();
                loadingState = LoadingState.Complete;
            });
        }
        void SmoothMap()
        {
            // Create a temporary grid to store the smoothed results
            // We must read from 'dataGrid' and write to 'tempGrid' to avoid race conditions
            Grid2D_L<GenTile> tempGrid = new Grid2D_L<GenTile>(dataGrid.Size);

            // Thresholds
            float heightDiffThreshold = 0.2f; // How "bumpy" it needs to be to trigger smoothing
            float minHeight = Height_WaterPlane;           // Don't smooth below this (e.g., Water)
            float maxHeight = Height_MountainStart;           // Don't smooth above this (e.g., Peaks)

            Parallel.For(0, dataGrid.Size.X, x =>
            {
                for (int y = 0; y < dataGrid.Size.Y; y++)
                {
                    float currentH = dataGrid.Get(x, y).groundY;

                    // CONSTRAINT 1: Value Range Check
                    // If value is too low (water) or too high (mountain peaks), skip it.
                    if (currentH <= minHeight || currentH > maxHeight)
                    {
                        tempGrid.Set(x, y, dataGrid.Get(x, y));
                        continue;
                    }

                    // Check neighbors to calculate average and see if we need to smooth
                    float totalHeight = 0;
                    int count = 0;
                    bool needsSmoothing = false;

                    // Loop through 3x3 block (Moore neighborhood)
                    for (int nx = x - 1; nx <= x + 1; nx++)
                    {
                        for (int ny = y - 1; ny <= y + 1; ny++)
                        {
                            // Boundary check
                            if (nx >= 0 && nx < dataGrid.Size.X && ny >= 0 && ny < dataGrid.Size.Y)
                            {
                                float neighborH = dataGrid.Get(nx, ny).groundY;

                                // CONSTRAINT 2: Difference Check
                                // If ANY neighbor is significantly different, we flag this tile for smoothing
                                if (Math.Abs(neighborH - currentH) > heightDiffThreshold)
                                {
                                    needsSmoothing = true;
                                }

                                totalHeight += neighborH;
                                count++;
                            }
                        }
                    }

                    // Apply smoothing if conditions met
                    if (needsSmoothing)
                    {
                        // "Even them out" -> Average of all neighbors
                        float average = totalHeight / count;
                        tempGrid.Set(x, y, new GenTile { groundY = average });
                    }
                    else
                    {
                        // If it's already smooth enough, keep original value
                        tempGrid.Set(x, y, dataGrid.Get(x, y));
                    }
                }
            });

            // Apply the smoothed result back to the main data
            dataGrid = tempGrid;
            iconWorld.iconGrid = tempGrid;
        }

        void scaleUp16x()
        { 
            bool[] scalePassesIs4 = {  true, false, true, true };
            foreach (bool pass4 in scalePassesIs4)
            {
                if (pass4)
                {
                    scaleUp4();
                }
                else
                {
                    scaleUp8();
                }                    
            }
        }
       
        void scaleUp4()
        {
            Grid2D_L<GenTile> largeGrid = new Grid2D_L<GenTile>(dataGrid.Size * 2);

            // Parallel.For handles the splitting automatically.
            // We iterate over the X-axis in parallel, and let each thread handle a full column (Y-loop).
            Parallel.For(0, dataGrid.Size.X, x =>
            {
                for (int y = 0; y < dataGrid.Size.Y; y++)
                {
                    // --- Standard Interpolation Logic ---

                    int lgX = x * 2;
                    int lgY = y * 2;

                    int nextX = (x + 1 < dataGrid.Size.X) ? x + 1 : x;
                    int nextY = (y + 1 < dataGrid.Size.Y) ? y + 1 : y;

                    float hTL = dataGrid.Get(x, y).groundY;
                    float hTR = dataGrid.Get(nextX, y).groundY;
                    float hBL = dataGrid.Get(x, nextY).groundY;
                    float hBR = dataGrid.Get(nextX, nextY).groundY;

                    float avgTop = (hTL + hTR) * 0.5f;
                    float avgLeft = (hTL + hBL) * 0.5f;
                    float avgCenter = (hTL + hTR + hBL + hBR) * 0.25f;

                    largeGrid.Set(lgX, lgY, new GenTile { groundY = hTL });
                    largeGrid.Set(lgX + 1, lgY, new GenTile { groundY = avgTop });
                    largeGrid.Set(lgX, lgY + 1, new GenTile { groundY = avgLeft });
                    largeGrid.Set(lgX + 1, lgY + 1, new GenTile { groundY = avgCenter });
                }
            });

            dataGrid = largeGrid;
            iconWorld.iconGrid = largeGrid;
        }
        void scaleUp8()
        {
            Grid2D_L<GenTile> largeGrid = new Grid2D_L<GenTile>(dataGrid.Size * 2);

            Parallel.For(0, dataGrid.Size.X, x =>
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
                    largeGrid.Set(lgX, lgY, new GenTile { groundY = sTL });
                    largeGrid.Set(lgX + 1, lgY, new GenTile { groundY = avgTop });
                    largeGrid.Set(lgX, lgY + 1, new GenTile { groundY = avgLeft });
                    largeGrid.Set(lgX + 1, lgY + 1, new GenTile { groundY = avgCenter });
                }
            });

            dataGrid = largeGrid;
            iconWorld.iconGrid = largeGrid;
            //dataGrid = largeGrid;
            //world.tileGrid = largeGrid;
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
                        totalHeight += dataGrid.Get(nx, ny).groundY;
                        count++;
                    }
                }
            }

            return totalHeight / count;
        }

        void addNoiseTexture()
        {
            const int LoopDivs = 8;

            EngineSpace.Maths.SimplexNoise2D noiseMap = new EngineSpace.Maths.SimplexNoise2D(iconWorld.metaData2.seed + 11);
            NoiseOptions postNoise = new NoiseOptions(true, 0.1f, 4, 1f, 30f);
            float edgeThickness = LayerAddHeight * 1.6f;
            //NoiseOptions islandNoise = new NoiseOptions(true, 0.1f, 4, 1f, 5f);

            Rectangle2 area = new Rectangle2(dataGrid.Size);
            area.size.X /= LoopDivs;

            for (int divIx = 0; divIx < LoopDivs; divIx++)
            {
                Rectangle2 divArea = area;
                tasks.Add(Task.Run(() =>
                {
                    ForXYLoop loop = new ForXYLoop(divArea);
                    while (loop.Next())
                    {
                        var tile = dataGrid.Get(loop.Position);
                        float edge = Bound.Max(Math.Abs(Height_WaterPlane - tile.groundY), edgeThickness) / edgeThickness;
                        tile.groundY -= noiseMap.OctaveNoise2D(postNoise, loop.Position.X, loop.Position.Y) * (0.08f + (1f - edge) * 0.3f); //*
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
            const bool PostNoise = true;
            //const int PostProcessDivs = 8;

            EngineSpace.Maths.SimplexNoise2D noiseMap = new EngineSpace.Maths.SimplexNoise2D(iconWorld.metaData2.seed + 3);
            NoiseOptions postNoise = new NoiseOptions(true, 0.1f, 4, 1f, 10f);
            //NoiseOptions islandNoise = new NoiseOptions(true, 0.1f, 4, 1f, 5f);

            //Rectangle2 area = new Rectangle2(dataGrid.Size);
            //area.size.X /= PostProcessDivs;

            //for (int divIx = 0; divIx < PostProcessDivs; divIx++)
            //{
            //    Rectangle2 divArea = area;
            //    tasks.Add(Task.Run(() =>
            //    {
            //ForXYLoop loop = new ForXYLoop(divArea);
            //while (loop.Next())
            //{
            Parallel.For(0, dataGrid.Size.X, x =>
            {
                for (int y = 0; y < dataGrid.Size.Y; y++)
                {
                    var tile = dataGrid.Get(x, y);
                    if (PostNoise)
                    {
                        tile.groundY -= noiseMap.OctaveNoise2D(postNoise, x, y) * 0.1f;
                    }
                    if (tile.groundY < Height_WaterBottom)
                    { tile.groundY = Height_WaterBottom; }

                    tileColor(ref tile);
                    dataGrid.Set(x, y, tile);
                }
            });
        }
        //            }
        //        }));
        //        area.X += area.size.X;
        //    }
        //}
        //NoiseOptions generateNoise(PcgRandom rnd, bool use)
        //{
        //    NoiseOptions noiseOptions = new NoiseOptions(use, rnd.Float(), rnd.Float(3, 5), rnd.Float(0.7f, 0.9f), rnd.Float(2, 6));
        //    return noiseOptions;
        //}

        void tileColor(ref GenTile tile)
        {
            if (tile.groundY > Height_WaterBottom)
            {
                lib.DoNothing();
            }
            if (tile.groundY < 0)
            {
                float depth = 1f - tile.groundY / Height_WaterBottom;
                tile.color = new Microsoft.Xna.Framework.Color(depth * 0.5f, depth * 0.5f, depth * 0.5f + 0.2f);
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
            Vector2 center = iconWorld.rnd.vector2(dataGrid.Size.X - 1, dataGrid.Size.Y - 1);

            Rotation1D growDir = Rotation1D.Random(iconWorld.rnd);
            Rotation1D leftDir = Rotation1D.D0;
            Rotation1D rightDir = Rotation1D.D0;
            refreshDirs();

            const float MaxRadius = 8;
            const float MinRadius = 4;

            DrawMapOptions drawMountain = new DrawMapOptions()
            {
                add = false,
                radius = iconWorld.rnd.Float(MinRadius, MaxRadius),
                flatness = 0.02f,
                addHeight = Height_MountainPeek,
            };
            drawMountain.refreshRadius();

            DrawMapOptions drawCenterGround = new DrawMapOptions()
            {
                add = true,
                radius = iconWorld.rnd.Float(1, 2) * drawMountain.radius,
                flatness = 0.4f,
                addHeight = Height_DefaultGround,
            };
            drawCenterGround.refreshRadius();
            drawCenterGround = drawAddCalc(center, drawCenterGround);

            int leftSide = iconWorld.rnd.Int(0, 4);
            int rightSide = iconWorld.rnd.Int(1, 20);

            int connectedChains = iconWorld.rnd.Int(1, 4);

            int nextCenterGrounds = iconWorld.rnd.Int(0, 7);

            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
            {
                int chainLength = chainLengthRange2.GetRandom(iconWorld.rnd);

                connectPoints.Add(center);
                for (int link = 0; link < chainLength; ++link)
                {
                    if (nextCenterGrounds <= 0)
                    {
                        nextCenterGrounds = iconWorld.rnd.Int(2, 8);
                        startTask_placeDotWithOptions(center, drawCenterGround, true, 2);
                    }
                    else
                    {
                        nextCenterGrounds--;
                    }

                    bool growSides = link < chainLength / 2;
                    sideLinks(ref leftSide, leftDir, growSides);
                    sideLinks(ref rightSide, rightDir, growSides);
                    drawMountain.centerHeight = drawMountain.addHeight;
                    placeMountainSquare(iconWorld.rnd, new IntVector2(center + iconWorld.rnd.vector2(new Vector2(drawMountain.radius * 0.4f))), drawMountain/*, generateNoise(world.rnd, true)*/);

                    int sideMountains = iconWorld.rnd.Int(0, 5);
                    for (int i = 0; i < sideMountains; i++)
                    {
                        DrawMapOptions draw = drawMountain;
                        float scale = iconWorld.rnd.Float(0.5f, 0.9f);
                        draw.centerHeight *= scale;
                        draw.radius *= scale;
                        placeMountainSquare(iconWorld.rnd, new IntVector2(center + iconWorld.rnd.vector2(new Vector2(drawMountain.radius * 1.7f))), draw/*, generateNoise(world.rnd, true)*/);
                    }

                    if (iconWorld.rnd.Chance(0.2))
                    {
                        growDir.Add(iconWorld.rnd.Plus_MinusF(0.2f));
                        refreshDirs();
                    }
                    drawMountain.radius = Bound.Set(drawMountain.radius + iconWorld.rnd.Plus_MinusF(2f), MinRadius, MaxRadius);

                    center += growDir.Direction(drawMountain.radius * iconWorld.rnd.Float(0.5f, 1.4f));

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

                growDir.Add(iconWorld.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(iconWorld.rnd.Float(100f, 200f) + drawMountain.radius);

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
                    drawMoutainSide.radius *= iconWorld.rnd.Float(1f, 3f);
                    drawMoutainSide.flatness = iconWorld.rnd.Float(0.1f, 0.3f);
                    drawMoutainSide.addHeight *= iconWorld.rnd.Float(0.3f, 0.6f);
                    drawMoutainSide = drawAddCalc(center, drawMoutainSide);

                    for (int link = 0; link < links; ++link)
                    {
                        sideCenter += dir.Direction(drawMountain.radius * iconWorld.rnd.Float(0.3f, 0.6f));
                        startTask_placeDotWithOptions(sideCenter, drawMoutainSide, true, 2);
                        drawMoutainSide.adjustHeight(iconWorld.rnd.Float(-0.2f, 0.05f));
                    }

                    if (iconWorld.rnd.Chance(Bound.Max(0.1 + links * 0.1, 0.5)))
                    {
                        generateLandChains(sideCenter + iconWorld.rnd.vector2_cirkle(8), drawMoutainSide.radius * 2f, true);
                    }
                }
                if (iconWorld.rnd.Chance(0.1))
                {
                    if (grow)
                    {
                        links += iconWorld.rnd.Int(-1, 3);
                    }
                    else
                    {
                        links += iconWorld.rnd.Int(-3, 1);
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
            Vector2 center = iconWorld.rnd.vector2(dataGrid.Size.X - 1, dataGrid.Size.Y - 1);

            int maxLoops = 50;
            if (addativeOnly)
            {
                do
                {
                    if (--maxLoops < 0)
                    {
                        return;
                    }
                    center = iconWorld.rnd.vector2(dataGrid.Size.X - 1, dataGrid.Size.Y - 1);
                } while (dataGrid.Get(new IntVector2(center)).groundY >= Height_WaterPlane);
            }
            else
            {
                do
                {
                    if (--maxLoops < 0)
                    {
                        return;
                    }
                    center = iconWorld.rnd.vector2(dataGrid.Size.X - 1, dataGrid.Size.Y - 1);
                } while (dataGrid.Get(new IntVector2(center)).groundY < Height_LowGround);
            }

            generateLandChains(center, MaxRadius, noise);
        }

        void generateLandChains(Vector2 center, float MaxRadius, bool noise)
        {

            Range chainLengthRange2 = new Range(3, 8);
            if (MaxRadius < 6)
            {
                chainLengthRange2.Max = 15;
            }

            Rotation1D growDir = Rotation1D.Random(iconWorld.rnd);


            DrawMapOptions draw = new DrawMapOptions()
            {
                noiseStrength = iconWorld.rnd.Float(0.1f, 1.5f),
                noise = noise,
                add = true,
                radius = iconWorld.rnd.Float(MaxRadius * 0.02f, MaxRadius),
                flatness = iconWorld.rnd.Float(0.05f, 0.2f),
                addHeight = LayerAddHeight * iconWorld.rnd.Float(0.8f, 2f),
            };

            draw.quadChance = draw.radius < 20 ? 0.3f : 0;

            draw = drawAddCalc(center, draw);

            int fractals = 2 + (int)(draw.radius / 1);

            float smoothness = iconWorld.rnd.Float();
            int connectedChains = iconWorld.rnd.Int(1, 2);

            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
            {
                int chainLength = chainLengthRange2.GetRandom(iconWorld.rnd);
                connectPoints.Add(center);
                for (int link = 0; link < chainLength; ++link)
                {
                    startTask_placeDotWithOptions(center + iconWorld.rnd.vector2_cirkle(1), draw, false, fractals);

                    if (iconWorld.rnd.Chance(0.2))
                    {
                        growDir.Add(iconWorld.rnd.Plus_MinusF(0.2f));
                    }
                    if (iconWorld.rnd.Chance(0.2))
                    {
                        draw.adjustHeight(iconWorld.rnd.Plus_MinusF(0.1f));
                    }
                    draw.radius = Bound.Set(draw.radius + iconWorld.rnd.Plus_MinusF(1f), 1, MaxRadius);

                    draw.refreshRadius();
                    center += growDir.Direction(draw.flatRadius * iconWorld.rnd.Float_LowDisp(3f, 12f));

                }
                connectPoints.Add(center);

                growDir.Add(iconWorld.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(iconWorld.rnd.Float(12f, 25f) * draw.radius); //skip to a new chain

                if (!dataGrid.InBounds(new IntVector2(center)))
                {
                    break;
                }
            }

            if (iconWorld.rnd.Chance(0.3))
            {
                int islandCount = iconWorld.rnd.Int(1, 8);
                for (int i = 0; i < islandCount; ++i)
                {
                    generateIsland(center, draw.radius);
                }
            }
        }

        void generateLargeIsland(float MaxRadius)
        {
            PcgRandom rnd = new PcgRandom(iconWorld.rnd.Ushort());

            tasks.Add(Task.Run(() =>
            {
                Vector2 center = rnd.vector2(dataGrid.Size.X, dataGrid.Size.Y);
                float landRadius = rnd.Int(5, 25);
               
                DrawMapOptions drawCenter = new DrawMapOptions()
                {
                    noiseStrength = rnd.Float(0.1f, 0.3f),
                    noise = false,
                    add = false,
                    radius = rnd.Float(MaxRadius * 0.4f, MaxRadius),
                    flatness = 0.6f,
                    addHeight = LayerAddHeight * rnd.Float(0.9f, 2f),
                };

                int chainLength = 1;
                if (rnd.Chance(0.8)) 
                {
                    chainLength = rnd.Int(2, 4);
                }

                while (chainLength > 0)
                {
                    drawCenter = drawAddCalc(center, drawCenter);
                    drawCenter.refreshRadius();

                    startTask_placeDotWithOptions(center, drawCenter, false, 0);


                    //place dots in a cirkle around
                    int dotsCount = (int)(drawCenter.radius * rnd.Float(0.15f, 0.5f));

                    for (int i = 0; i <= dotsCount; ++i)
                    {
                        DrawMapOptions drawDot = drawCenter;
                        drawDot.radius *= rnd.Float(0.05f, 1.2f);
                        drawDot.refreshRadius();
                        drawDot.quadChance = drawDot.radius < 30 ? 0.6f : 0;
                        drawDot.addHeight *= rnd.Float(0.8f, 1.1f);

                        startTask_placeDotWithOptions(center + rnd.vector2_cirkle(drawCenter.flatRadius * rnd.Float(0.3f, 1.2f)), drawDot, true, 1);
                    }

                    chainLength--;

                    if (chainLength > 0)
                    {
                        center += rnd.vector2_cirkle(drawCenter.flatRadius * rnd.Float(0.3f, 0.9f));
                        drawCenter.radius *= rnd.Float(0.3f, 0.9f);
                    }
                }

            }));

            
        }

        void generateHills(float MaxRadius, int fractals)
        {
            PcgRandom rnd = new PcgRandom(iconWorld.rnd.Ushort());

            tasks.Add(Task.Run(() =>
            {
                int maxLoops = 10;
                Vector2 center = Vector2.Zero;
                float groundY = 0;
                do
                {
                    if (--maxLoops < 0)
                    {
                        return;
                    }
                    center = rnd.vector2(dataGrid.Size.X - 1, dataGrid.Size.Y - 1);
                    groundY = dataGrid.Get(new IntVector2(center)).groundY;
                } while (groundY < Height_LowGround || groundY > Height_MountainStart);
               
                
                DrawMapOptions draw = new DrawMapOptions()
                {
                    noiseStrength = rnd.Float(0.1f, 0.3f),
                    noise = false,
                    add = true,
                    radius = rnd.Float(MaxRadius * 0.4f, MaxRadius),
                    flatness = 0.1f,
                    addHeight = LayerAddHeight * rnd.Float(1f, 5f),
                };
                draw.quadChance = draw.radius < 20 ? 0.3f : 0;

                draw = drawAddCalc(center, draw);
                draw.refreshRadius();

                startTask_placeDotWithOptions(center, draw, false, 1);

                for (int i = 0; i < fractals; i++)
                {
                    center += rnd.vector2_cirkle(draw.radius * rnd.Float(0.6f, 2f));
                    draw.radius *= rnd.Float(0.3f, 0.9f);
                    draw.addHeight *= rnd.Float(0.4f, 0.95f);
                    draw.refreshRadius();
                    startTask_placeDotWithOptions(center, draw, false, 1);
                }
                

            }));


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
                    float distance = iconWorld.rnd.Float(1.0f, 5f) * landRadius;
                    center = landCenter + iconWorld.rnd.vector2_cirkle(distance);
                } while (!dataGrid.TryGet(new IntVector2(center), out var tile) || tile.groundY > Height_LowGround);

                DrawMapOptions draw = new DrawMapOptions()
                {
                    noiseStrength = iconWorld.rnd.Float(0.1f, 1.5f),
                    noise = true,
                    add = true,
                    radius = iconWorld.rnd.Float(0.2f, 0.6f) * landRadius,
                    flatness = 0.4f,
                    addHeight = LayerAddHeight * 0.5f,
                };
                draw.quadChance = draw.radius < 20 ? 0.6f : 0;
                draw = drawAddCalc(center, draw);

                startTask_placeDotWithOptions(center, draw, false, 8);
            }
        }

        void generateQuadIsland()
        {
            tasks.Add(Task.Run(() =>
            {
                PcgRandom rnd = new PcgRandom(iconWorld.rnd.Ushort());
                
           
                Vector2 center = rnd.vector2(dataGrid.Size.X, dataGrid.Size.Y);
                float landRadius = rnd.Int(5, 25);
                if (landRadius > 1)
                {
                    //int maxLoops = 6;
                    //Vector2 center = Vector2.Zero;

                    //do
                    //{
                    //    if (--maxLoops < 0)
                    //    {
                    //        return;
                    //    }
                    //    float distance = iconWorld.rnd.Float(1.0f, 5f) * landRadius;
                    //    center = landCenter + iconWorld.rnd.vector2_cirkle(distance);
                    //} while (!dataGrid.TryGet(new IntVector2(center), out var tile) || tile.groundY > Height_LowGround);

                    DrawMapOptions draw = new DrawMapOptions()
                    {
                        noiseStrength = rnd.Float(0.1f, 1.5f),
                        noise = true,
                        add = true,
                        radius = rnd.Float(0.2f, 0.6f) * landRadius,
                        flatness = 0.4f,
                        addHeight = LayerAddHeight * 0.5f,
                    };
                    draw = drawAddCalc(center, draw);

                    QuadPenShape quadPen = new QuadPenShape(rnd, center, landRadius);

                    placeQuad(quadPen, draw);
                        //startTask_placeDotWithOptions(center, draw, false, 8/*, generateNoise(world.rnd, true)*/);
                    }
            }));
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
                center = iconWorld.rnd.vector2(dataGrid.Size.X -1, dataGrid.Size.Y -1);
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



            Rotation1D growDir = Rotation1D.Random(iconWorld.rnd);

            DrawMapOptions draw = new DrawMapOptions()
            {
                add = true,
                radius = Math.Min(iconWorld.rnd.Float(MinRadius, MaxRadius), iconWorld.rnd.Float(MinRadius, MaxRadius)),
                flatness = 0.0f,
                addHeight = -Height.DefaultGroundYoffset * iconWorld.rnd.Float(0.6f, 4f),
            };
            draw = drawAddCalc(center, draw);

            int fractal = 1;//draw.radius > 4 ? 2 : 1;

            int connectedChains = iconWorld.rnd.Int(1, 4);
            float smoothness = iconWorld.rnd.Float();

            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
            {
                int chainLength = chainLengthRange.GetRandom(iconWorld.rnd);

                for (int link = 0; link < chainLength; ++link)
                {

                    startTask_placeDotWithOptions(center + iconWorld.rnd.vector2_cirkle(8), draw, false, fractal/*, generateNoise(world.rnd, true)*/);

                    if (iconWorld.rnd.Chance(0.2))
                    {
                        growDir.Add(iconWorld.rnd.Plus_MinusF(0.2f));
                    }
                    draw.radius = Bound.Set(draw.radius + iconWorld.rnd.Plus_MinusF(8f), 4, MaxRadius);

                    center += growDir.Direction(draw.radius * iconWorld.rnd.Float(0.15f, 0.25f));

                }

                growDir.Add(iconWorld.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(iconWorld.rnd.Float(10f, 20f) + draw.radius);

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

        void startTask_placeDotWithOptions(Vector2 center, DrawMapOptions draw, bool placeIslands, int fractalDots)
        {
            tasks.Add(Task.Run(() =>
            {
                PcgRandom rnd = new PcgRandom(iconWorld.rnd.Ushort());
                placeDotWithOptions(rnd, center, draw, placeIslands, fractalDots/*, noiseOptions*/);
            }));
        }

        void placeDotWithOptions(PcgRandom rnd, Vector2 center, DrawMapOptions draw, bool placeIslands, int fractalDots)
        {
            if (rnd.Chance(draw.quadChance))
            {
                QuadPenShape penShape = new QuadPenShape(rnd, center, draw.radius);
                placeQuad(penShape, draw);
            }
            else
            {
                if (draw.noise)
                {
                    placeDot_noise(rnd, center, draw);
                }
                else
                {
                    placeDot(rnd, center, draw);
                }
            }

            if (fractalDots > 0 && draw.radius > 6)
            {
                draw.refreshRadius();

                int fractalCount = iconWorld.rnd.Int(1, 4);
                IntervalF radiusRange = new IntervalF(0.3f, 0.75f) * draw.radius;
                IntervalF offsetRange = new IntervalF(0.5f, 0.9f) * Bound.Min(draw.flatRadius, 4);

                for (int i = 0; i < fractalCount; ++i)
                {
                    Vector2 offset = iconWorld.rnd.vector2_cirkle(offsetRange.GetRandom(rnd));
                    //noiseOptions.useNoise = true;
                    DrawMapOptions drawFractal = draw;
                    drawFractal.radius = radiusRange.GetRandom(rnd);

                    placeDotWithOptions(rnd, center + offset, drawFractal, placeIslands, fractalDots - 1);
                }

            }


            if (placeIslands && iconWorld.rnd.Chance(0.1))
            {
                int islandCount = iconWorld.rnd.Int(1, 4);
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


        void placeQuad(QuadPenShape quadPen, DrawMapOptions draw)
        {
            draw.radius = quadPen.radius;
            draw.refreshRadius();
            //Rectangle2 area = new Rectangle2(new IntVector2(center), (int)draw.radius + 1);
            var minmax = quadPen.BeginDraw(iconWorld);
            //ForXYLoop loopArea = new ForXYLoop(area);
            //while (loopArea.Next())
            //{
            for (int y = minmax.min.Y; y <= minmax.max.Y; ++y)
            {
                for (int x = minmax.min.X; x <= minmax.max.X; ++x)
                {
                    //if (dataGrid.InBounds(loopArea.Position))
                    //{
                    //Vector2 posDiff = loopArea.Position.Vec - center;
                    //float distFromCenter = (posDiff).Length();
                    //if (distFromCenter <= draw.radius)
                    //{
                    //float percentDist = distFromCenter / draw.radius;
                    var pos = new IntVector2(x, y);
                    if (quadPen.DrawPixel(pos, out var intensity))
                    {
                        placeTile(pos, draw.centerHeight * intensity, draw.addHeight > 0);
                    }
                        //}
                    //}
                }
            }
            //}
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
