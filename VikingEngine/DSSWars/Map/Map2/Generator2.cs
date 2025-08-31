using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.LootFest.Map.Terrain;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.Map.Map2
{
    struct DrawMapOptions
    {
        public bool add;
        public float centerHeight;
        public float edgeHeight;

        public float radius;
        public float flatRadius;
        public float hillRadius;
        public float flatness;

        public void refreshRadius()
        {
            flatRadius = radius * flatness;
            hillRadius = radius - flatRadius;

            if (centerHeight > 0)
            {
                edgeHeight = Bound.Min(centerHeight - (hillRadius * 0.1f * centerHeight), Height.AboveWater_Tile2Y);
            }
            else
            {
                edgeHeight = Bound.Min(centerHeight - (hillRadius * 0.1f * centerHeight), 0);
            }
        }
    }

    class Generator2
    {
        static readonly IntervalF digLinkPosDiffRange = new IntervalF(0.5f, 2);

        LoadingState loadingState = LoadingState.None;
        public WorldData2 world;
        MapGenerateSettings generateSettings = new MapGenerateSettings();
        EngineSpace.Maths.SimplexNoise2D noiseMap;
        List<Task> tasks = new List<Task>(64);
        Biom biom;
        public void generate()
        {
            biom = DssRef.map.bioms.bioms[(int)BiomType.Green];
            Task.Run(async () =>
            {
                world = new WorldData2(MapSize.Medium);
                noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.seed);
                loadingState = LoadingState.Pass;

                world.tileGrid.LoopBegin();
                while (world.tileGrid.LoopNext())
                {
                    var tile = world.tileGrid.Get(world.tileGrid.LoopPosition);
                    tile.color = Color.Black;
                    world.tileGrid.Set(world.tileGrid.LoopPosition, tile);
                }

                //testDot();
                ////Test in one thread
                //generateLandChains(200);

                for (int i = 0; i < 20; i++)
                {
                    generateLandChains(200);
                }
                int mountainCount = world.rnd.Int(3, 6);

                for (int i = 0; i < mountainCount; i++)
                {
                    generateMountainChains();
                }

                await Task.WhenAll(tasks);
                tasks.Clear();

                for (int repeatBuildDig = 0; repeatBuildDig < 6; ++repeatBuildDig)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        generateDigChains(true);
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        generateDigChains(false);
                    }

                    for (int i = 0; i < 15; i++)
                    {
                        generateLandChains(60);
                    }

                    await Task.WhenAll(tasks);
                    tasks.Clear();
                }

                loadingState = LoadingState.Complete;
            });


        }
        void generateLandChains(float MaxRadius)
        {
            Vector2 center = world.rnd.vector2(world.tileGrid.Size.X, world.tileGrid.Size.Y);

            generateLandChains(center, MaxRadius);
        }

        void testDot()
        {
            Vector2 center = world.tileGrid.Size.Vec * 0.5f;

            float MaxRadius = 50;
            DrawMapOptions draw = new DrawMapOptions()
            {
                add = true,
                radius = world.rnd.Float(MaxRadius * 0.05f, MaxRadius),
                flatness = 0.75f,
                centerHeight = Height.MaxLand_Tile2Y,
            };

            placeDotWithOptions(world.rnd,center, draw, 0, generateNoise(world.rnd, false));
        }

        void generateLandChains(Vector2 center, float MaxRadius)
        {
            //const float MaxRadius = 300;
            Range chainLengthRange2 = new Range(3, 18);

            Rotation1D growDir = Rotation1D.Random(world.rnd);


            DrawMapOptions draw = new DrawMapOptions()
            {
                add = true,
                radius = world.rnd.Float(MaxRadius * 0.05f, MaxRadius),
                flatness = 0.75f,
                centerHeight = Height.MaxLand_Tile2Y,
            };
            draw = drawAddCalc(center, draw);

            //float radius = world.rnd.Float(MaxRadius * 0.05f , MaxRadius);
            float smoothness = world.rnd.Float();
            int connectedChains = world.rnd.Int(1, 2);

            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
            {
                int chainLength = chainLengthRange2.GetRandom(world.rnd);

                for (int link = 0; link < chainLength; ++link)
                {
                    startTask_placeDotWithOptions(center + world.rnd.vector2_cirkle(8), draw, 2, generateNoise(world.rnd, false));

                    if (world.rnd.Chance(0.2))
                    {
                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
                    }
                    draw.radius = Bound.Set(draw.radius + world.rnd.Plus_MinusF(8f), 16, MaxRadius);

                    center += growDir.Direction(draw.radius * world.rnd.Float(0.3f, 0.5f));

                }

                growDir.Add(world.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(world.rnd.Float(100f, 200f) + draw.radius);

                if (!world.tileGrid.InBounds(new IntVector2(center)))
                {
                    break;
                }
            }

            if (world.rnd.Chance(0.2))
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
            if (landRadius > 8)
            {
                int maxLoops = 6;
                Vector2 center = Vector2.Zero;

                do
                {
                    if (--maxLoops < 0)
                    {
                        return;
                    }
                    float distance = world.rnd.Float(1.2f, 8f) * landRadius;
                    center = landCenter + world.rnd.vector2_cirkle(distance);
                } while (!world.tileGrid.TryGet(new IntVector2(center), out var tile) || tile.groundY > 0);

                DrawMapOptions draw = new DrawMapOptions()
                {
                    add = true,
                    radius = world.rnd.Float(0.1f, 0.4f) * landRadius,
                    flatness = 0.75f,
                    centerHeight = Height.MaxLand_Tile2Y,
                };
                draw = drawAddCalc(center, draw);

                //float height = Height.MaxLand_Tile2Y;
                //float radius = world.rnd.Float(0.1f, 0.4f) * landRadius;
                startTask_placeDotWithOptions(center, draw, 1, generateNoise(world.rnd, true));
            }
        }

        void generateDigChains(bool large)
        {
            int maxLoops = 100;
            Vector2 center = Vector2.Zero;

            do
            {
                if (--maxLoops < 0)
                {
                    return;
                }
               center = world.rnd.vector2(world.tileGrid.Size.X, world.tileGrid.Size.Y);
            } while (world.tileGrid.Get(new IntVector2(center)).groundY <= 0);

            const float MinRadius = 4;

            float MaxRadius;
            Range chainLengthRange;
            if (large)
            {
                MaxRadius = 50;
                chainLengthRange = new Range(4, 32);
            }
            else
            {
                MaxRadius = 16;
                chainLengthRange = new Range(2, 16);
            }
            
            

            Rotation1D growDir = Rotation1D.Random(world.rnd);

            DrawMapOptions draw = new DrawMapOptions()
            {
                add = true,
                radius = Math.Min(world.rnd.Float(MinRadius, MaxRadius), world.rnd.Float(MinRadius, MaxRadius)),
                flatness = 0.0f,
                centerHeight = -Height.DefaultGroundYoffset * 3,
            };
            draw = drawAddCalc(center, draw);

            int fractal = draw.radius > 30 ? 2 : 1;

            int connectedChains = world.rnd.Int(1, 4);
            float smoothness = world.rnd.Float();

            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
            {
                int chainLength = chainLengthRange.GetRandom(world.rnd);

                for (int link = 0; link < chainLength; ++link)
                {

                    startTask_placeDotWithOptions(center + world.rnd.vector2_cirkle(8), draw, fractal, generateNoise(world.rnd, false));

                    if (world.rnd.Chance(0.2))
                    {
                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
                    }
                    draw.radius = Bound.Set(draw.radius + world.rnd.Plus_MinusF(8f), 4, MaxRadius);

                    center += growDir.Direction(draw.radius * world.rnd.Float(0.3f, 0.5f));

                }

                growDir.Add(world.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(world.rnd.Float(100f, 200f) + draw.radius);

                if (!world.tileGrid.InBounds(new IntVector2(center)))
                {
                    break;
                }
            }
        }

        void generateMountainChains()
        {
            Range chainLengthRange2 = new Range(20, 150);
            Vector2 center = world.rnd.vector2(world.tileGrid.Size.X, world.tileGrid.Size.Y);
            
            Rotation1D growDir = Rotation1D.Random(world.rnd);
            Rotation1D leftDir = Rotation1D.D0;
            Rotation1D rightDir = Rotation1D.D0;
            refreshDirs();

            DrawMapOptions drawMountain = new DrawMapOptions()
            {
                add = false,
                radius = world.rnd.Float(4f, 16),
                flatness = 0.0f,
                centerHeight = Height.MountainLowPeak_Tile2Y,
            };

            drawMountain.refreshRadius();

            int leftSide = world.rnd.Int(2, 4);
            int rightSide = world.rnd.Int(2, 4);

            int connectedChains = world.rnd.Int(1, 4);

            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
            {
                int chainLength = chainLengthRange2.GetRandom(world.rnd);

                for (int link = 0; link < chainLength; ++link)
                {
                    bool growSides = link < chainLength / 2;
                    sideLinks(ref leftSide, leftDir, growSides);
                    sideLinks(ref rightSide, rightDir, growSides);

                    placeDot(world.rnd, center, drawMountain);

                    if (world.rnd.Chance(0.2))
                    {
                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
                        refreshDirs();
                    }
                    drawMountain.radius = Bound.Set(drawMountain.radius + world.rnd.Plus_MinusF(2f), 8, 50);

                    center += growDir.Direction(drawMountain.radius * world.rnd.Float(0.3f, 0.5f));

                    //Forward check, no crossing
                    Vector2 forwardCheckPos = center + growDir.Direction(drawMountain.radius);
                    if (world.tileGrid.TryGet(new IntVector2(forwardCheckPos), out var forwardTile))
                    {
                        if (forwardTile.groundY > 3)
                        {
                            return;
                        }
                    }
                }

                growDir.Add(world.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(world.rnd.Float(100f, 200f) + drawMountain.radius);

                if (!world.tileGrid.InBounds(new IntVector2(center)))
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
                Vector2 sideCenter = center;

                for (int link = 0; link < links; ++link)
                {
                    DrawMapOptions drawMoutainSide = drawMountain;
                    drawMoutainSide.add = true;
                    drawMoutainSide.flatness = 0.2f;
                    drawMoutainSide.centerHeight = Height.MountainStart_Tile2Y;
                    drawMoutainSide = drawAddCalc(center, drawMoutainSide);

                    sideCenter += dir.Direction(drawMountain.radius * world.rnd.Float(0.3f, 0.6f));
                    startTask_placeDotWithOptions(sideCenter, drawMoutainSide, 2, generateNoise(world.rnd, false));
                }

                if (world.rnd.Chance(0.05))
                {
                    generateLandChains(sideCenter, 150);
                }

                if (world.rnd.Chance(0.1))
                {
                    if (grow)
                    {
                        links += world.rnd.Int(-2, 6);  
                    }
                    else
                    {
                        links += world.rnd.Int(-6, 2);
                    }

                    links = Bound.Set(links, 2, 30);
                }
            }
        }


        

        NoiseOptions generateNoise(PcgRandom rnd, bool use)
        { 
            NoiseOptions noiseOptions = new NoiseOptions(use, rnd.Float(), rnd.Float(3, 5), rnd.Float(0.7f, 0.9f), rnd.Float(2, 6));
            return noiseOptions;
        }

        void startTask_placeDotWithOptions(Vector2 center, DrawMapOptions draw, int fractalDots, NoiseOptions noiseOptions)
        {
            tasks.Add(Task.Run(() =>
            {
                PcgRandom rnd = new PcgRandom(world.rnd.Ushort());
                placeDotWithOptions(rnd, center, draw, fractalDots, noiseOptions);
            }));
        }

        void placeDotWithOptions(PcgRandom rnd, Vector2 center, DrawMapOptions draw, int fractalDots, NoiseOptions noiseOptions)
        {
            if (noiseOptions.useNoise)
            {
                placeDot_noise(rnd, center, draw, noiseOptions);
            }
            else
            {
                placeDot(rnd, center, draw);
            }

            if (fractalDots > 0 && draw.radius > 6)
            {
                int fractalCount = world.rnd.Int(4, 12);
                IntervalF radiusRange = new IntervalF(0.3f, 0.75f) * draw.radius;
                IntervalF offsetRange = new IntervalF(0.7f, 1.1f) * draw.radius;

                for (int i = 0; i < fractalCount; ++i)
                {
                    Vector2 offset = world.rnd.vector2_cirkle(offsetRange.GetRandom(rnd));
                    noiseOptions.useNoise = true;
                    draw.radius = radiusRange.GetRandom(rnd);
                    placeDotWithOptions(rnd, center + offset, draw, fractalDots - 1, noiseOptions);
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
                if (world.tileGrid.InBounds(loopArea.Position))
                {
                    Vector2 posDiff = loopArea.Position.Vec - center;
                    float distFromCenter = (posDiff).Length();
                    if (distFromCenter <= draw.radius)
                    {                        
                        //float percentDist = distFromCenter / draw.radius;
                        placeTile(loopArea.Position, drawHeight(distFromCenter, draw));
                    }
                }
            }
        }
        void placeDot_noise(PcgRandom rnd, Vector2 center, DrawMapOptions draw, NoiseOptions noiseOpt)
        {
            
            draw.refreshRadius();
            float noiseCap = new IntervalF(0.9f, 0.3f).GetFromPercent(noiseOpt.smoothness);
            float radiusPercCap = new IntervalF(0.4f, 0.7f).GetFromPercent(noiseOpt.smoothness);
            float percFallOffRadius = 1f - radiusPercCap;

            Rectangle2 area = new Rectangle2(new IntVector2(center), (int)draw.radius + 1);
            ForXYLoop loopArea = new ForXYLoop(area);
            while (loopArea.Next())
            {
                if (world.tileGrid.InBounds(loopArea.Position))
                {
                    Vector2 posDiff = loopArea.Position.Vec - center;
                    float distFromCenter = (posDiff).Length();
                    if (distFromCenter <= draw.radius)
                    {
                        float radiusPerc = distFromCenter / draw.radius;
                        float noise = noiseMap.OctaveNoise2D(noiseOpt,/*4, 0.8f, 4f,*/ -loopArea.Position.X, loopArea.Position.Y);

                        if (radiusPerc < radiusPercCap || Math.Abs(noise) > ((radiusPerc - radiusPercCap) / percFallOffRadius) * noiseCap)
                        {
                            //float percentDist = distFromCenter / draw.radius;
                            placeTile(loopArea.Position, drawHeight(distFromCenter, draw));
                        }
                    }
                }
            }
        }

        DrawMapOptions drawAddCalc(Vector2 center, DrawMapOptions draw)
        {
            if (draw.add && world.tileGrid.TryGet(new IntVector2(center), out var tile))
            {
                draw.centerHeight += tile.groundY * 0.25f;
            }
                return draw;
            
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
                return draw.centerHeight * (1f -percTowardsEdge) + draw.edgeHeight * percTowardsEdge;                
            }
        }

        void placeTile(IntVector2 pos, float height)
        { 
            ref var tile = ref world.tileGrid.GetRef(pos);
            if (height <= Height.LowWater_Tile2Y || height > tile.groundY)
            {
                tile.groundY = height;
                tile.color = biom.Tile2Color(height);
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
