using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.Map.Map2
{
    class Generator2
    {
        static readonly IntervalF digLinkPosDiffRange = new IntervalF(0.5f, 2);

        LoadingState loadingState = LoadingState.None;
        public WorldData2 world;
        MapGenerateSettings generateSettings = new MapGenerateSettings();
        EngineSpace.Maths.SimplexNoise2D noiseMap;
        List<Task> tasks = new List<Task>(64);
       
        public void generate()
        {
            

            Task.Run(async () =>
            {
                world = new WorldData2(MapSize.Medium);
                noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.seed);
                loadingState = LoadingState.Pass;

                world.tileGrid.LoopBegin();
                while (world.tileGrid.LoopNext())
                {
                    var tile = world.tileGrid.Get(world.tileGrid.LoopPosition);
                    tile.color = Color.Blue;
                    world.tileGrid.Set(world.tileGrid.LoopPosition, tile);
                }


                //for (int buildDig = 0; buildDig < 4; buildDig++)
                //{


                //    for (int i = 0; i < 30; i++)
                //    {
                //        generateDigChains();
                //    }

                //    for (int i = 0; i < 60; i++)
                //    {
                //        generateLandChains(100);
                //    }
                //}

                //for (int i = 0; i < 30; i++)
                //{
                //    generateMountainChains();
                //}


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
        void generateLandChains(Vector2 center, float MaxRadius)
        {
            //const float MaxRadius = 300;
            Range chainLengthRange2 = new Range(3, 18);

            Rotation1D growDir = Rotation1D.Random(world.rnd);
            
            float radius = world.rnd.Float(MaxRadius * 0.05f , MaxRadius);
            float smoothness = world.rnd.Float();
            int connectedChains = world.rnd.Int(1, 2);

            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
            {
                int chainLength = chainLengthRange2.GetRandom(world.rnd);

                for (int link = 0; link < chainLength; ++link)
                {
                    startTask_placeDotWithOptions(center + world.rnd.vector2_cirkle(8), radius, 2, Color.Green, 2, generateNoise(world.rnd, false));

                    if (world.rnd.Chance(0.2))
                    {
                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
                    }
                    radius = Bound.Set(radius + world.rnd.Plus_MinusF(8f), 16, MaxRadius);

                    center += growDir.Direction(radius * world.rnd.Float(0.3f, 0.5f));

                }

                growDir.Add(world.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(world.rnd.Float(100f, 200f) + radius);

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
                    generateIsland(center, radius, 2, Color.Green);
                }
            }
        }

        void generateIsland(Vector2 landCenter, float landRadius, int height, Color color)
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

                float radius = world.rnd.Float(0.1f, 0.4f) * landRadius;
                placeDotWithOptions(world.rnd, center, radius, height, color, 1, generateNoise(world.rnd, true));
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

            float radius = Math.Min( world.rnd.Float(MinRadius, MaxRadius), world.rnd.Float(MinRadius, MaxRadius));

            int fractal = radius > 30 ? 2 : 1;

            int connectedChains = world.rnd.Int(1, 4);
            float smoothness = world.rnd.Float();

            for (int connectedIx = 0; connectedIx < connectedChains; ++connectedIx)
            {
                int chainLength = chainLengthRange.GetRandom(world.rnd);

                for (int link = 0; link < chainLength; ++link)
                {

                    startTask_placeDotWithOptions(center + world.rnd.vector2_cirkle(8), radius, 0, Color.Blue, fractal, generateNoise(world.rnd, false));

                    if (world.rnd.Chance(0.2))
                    {
                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
                    }
                    radius = Bound.Set(radius + world.rnd.Plus_MinusF(8f), 4, MaxRadius);

                    center += growDir.Direction(radius * world.rnd.Float(0.3f, 0.5f));

                }

                growDir.Add(world.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(world.rnd.Float(100f, 200f) + radius);

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

            float radius = world.rnd.Float(4f, 16);

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

                    placeDot(world.rnd, center, radius, 4, Color.Gray);

                    if (world.rnd.Chance(0.2))
                    {
                        growDir.Add(world.rnd.Plus_MinusF(0.2f));
                        refreshDirs();
                    }
                    radius = Bound.Set(radius + world.rnd.Plus_MinusF(2f), 8, 50);

                    center += growDir.Direction(radius * world.rnd.Float(0.3f, 0.5f));

                    //Forward check, no crossing
                    Vector2 forwardCheckPos = center + growDir.Direction(radius);
                    if (world.tileGrid.TryGet(new IntVector2(forwardCheckPos), out var forwardTile))
                    {
                        if (forwardTile.groundY > 3)
                        {
                            return;
                        }
                    }
                }

                growDir.Add(world.rnd.Plus_MinusF(0.2f));
                center += growDir.Direction(world.rnd.Float(100f, 200f) + radius);

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
                    sideCenter += dir.Direction(radius * world.rnd.Float(0.3f, 0.6f));
                    startTask_placeDotWithOptions(sideCenter, radius, 3, Color.Brown, 2, generateNoise(world.rnd, false));
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

        //static float Smoothstep(float a, float b, float x)
        //{
        //    float t = Math.Clamp((x - a) / (b - a), 0f, 1f);
        //    return t * t * (3f - 2f * t);
        //}
        //void placeDot_noise(Vector2 center, float radius, float height, Color color, float smoothness)
        //{
        //    // How wide the “noisy rim” is (as a fraction of radius)
        //    float rimStart = 0.6f; // interior up to 60% stays fully solid
        //                           // Max jitter of the edge in world units; scale by smoothness
        //    float maxJitter = MathHelper.Lerp(0f, radius * 0.25f, smoothness); // tweak 0.25f as you like

        //    // Noise frequency: scale with radius so big dots don’t get micro-speckle
        //    float baseFreq = 1.0f / Math.Max(4f, radius); // larger radius => lower frequency

        //    Rectangle2 area = new Rectangle2(new IntVector2(center), (int)radius + 2);
        //    ForXYLoop loopArea = new ForXYLoop(area);

        //    while (loopArea.Next())
        //    {
        //        if (!world.tileGrid.InBounds(loopArea.Position)) continue;

        //        Vector2 pos = loopArea.Position.Vec;
        //        float dist = (pos - center).Length();
        //        if (dist > radius + maxJitter) continue; // quick reject

        //        float rPerc = dist / radius;

        //        // Fade noise from 0 at rimStart to 1 at the edge
        //        float rimFalloff = Smoothstep(rimStart, 1.0f, rPerc);

        //        // fBm / octave noise in [-1, 1]
        //        float n = noiseMap.OctaveNoise2D(
        //            4,        // octaves
        //            0.5f,     // persistence
        //            2.0f,     // lacunarity
        //            pos.X * baseFreq,
        //            pos.Y * baseFreq
        //        );

        //        // Jitter only near the rim
        //        float jitter = n * maxJitter * rimFalloff;

        //        // Perturbed radius
        //        float perturbedRadius = radius + jitter;

        //        if (dist <= perturbedRadius)
        //        {
        //            ref var tile = ref world.tileGrid.GetRef(loopArea.Position);
        //            if (height <= 0 || height > tile.groundY)
        //            {
        //                tile.groundY = height;
        //                tile.color = color;
        //            }
        //        }
        //    }
        //}

        

        void placeDot_noise(PcgRandom rnd, Vector2 center, float radius, float height, Color color, NoiseOptions noiseOpt)
        {
            float noiseCap = new IntervalF(0.9f, 0.3f).GetFromPercent(noiseOpt.smoothness);
            float radiusPercCap = new IntervalF(0.4f, 0.7f).GetFromPercent(noiseOpt.smoothness);
            float percFallOffRadius = 1f - radiusPercCap;

            Rectangle2 area = new Rectangle2(new IntVector2(center), (int)radius + 1);
            ForXYLoop loopArea = new ForXYLoop(area);
            while (loopArea.Next())
            {
                if (world.tileGrid.InBounds(loopArea.Position))
                {
                    Vector2 posDiff = loopArea.Position.Vec - center;
                    float distFromCenter = (posDiff).Length();
                    if (distFromCenter <= radius)
                    {
                        float radiusPerc = distFromCenter / radius;
                        float noise = noiseMap.OctaveNoise2D(noiseOpt,/*4, 0.8f, 4f,*/ -loopArea.Position.X, loopArea.Position.Y);

                        if (radiusPerc < radiusPercCap || Math.Abs(noise) > (( radiusPerc - radiusPercCap) / percFallOffRadius) * noiseCap)
                        {
                            float percentDist = distFromCenter / radius;
                            ref var tile = ref world.tileGrid.GetRef(loopArea.Position);
                            if (height <= 0 || height > tile.groundY)
                            {
                                tile.groundY = height;
                                tile.color = color;
                            }
                        }
                    }
                }
            }
        }

        NoiseOptions generateNoise(PcgRandom rnd, bool use)
        { 
            NoiseOptions noiseOptions = new NoiseOptions(use, rnd.Float(), rnd.Float(3, 5), rnd.Float(0.7f, 0.9f), rnd.Float(2, 6));
            return noiseOptions;
        }

        void startTask_placeDotWithOptions(Vector2 center, float radius, float height, Color color, int fractalDots, NoiseOptions noiseOptions)
        {
            tasks.Add( Task.Run(() =>
            {
                PcgRandom rnd = new PcgRandom(world.rnd.Ushort());
                placeDotWithOptions(rnd, center, radius, height, color, fractalDots, noiseOptions);
            }));
        }

        void placeDotWithOptions(PcgRandom rnd, Vector2 center, float radius, float height, Color color, int fractalDots, NoiseOptions noiseOptions)
        {
            if (noiseOptions.useNoise)
            {
                placeDot_noise(rnd, center, radius, height, color, noiseOptions);
            }
            else
            {
                placeDot(rnd, center, radius, height, color);
            }

            if (fractalDots > 0 && radius > 6)
            {
                int fractalCount = world.rnd.Int(4, 12);
                IntervalF radiusRange = new IntervalF(0.3f, 0.75f) * radius;
                IntervalF offsetRange = new IntervalF(0.7f, 1.1f) * radius;

                for (int i = 0; i < fractalCount; ++i)
                {
                    Vector2 offset = world.rnd.vector2_cirkle(offsetRange.GetRandom(rnd));
                    noiseOptions.useNoise = true;
                    placeDotWithOptions(rnd, center + offset, radiusRange.GetRandom(rnd), height, color, fractalDots - 1, noiseOptions);
                }

            }
        }


        void placeDot(PcgRandom rnd, Vector2 center, float radius, float height, Color color)
        {
            Rectangle2 area = new Rectangle2(new IntVector2(center), (int)radius + 1);
            ForXYLoop loopArea = new ForXYLoop(area);
            while (loopArea.Next())
            {
                if (world.tileGrid.InBounds(loopArea.Position))
                {
                    Vector2 posDiff = loopArea.Position.Vec - center;
                    float distFromCenter = (posDiff).Length();
                    if (distFromCenter <= radius)
                    {                        
                        float percentDist = distFromCenter / radius;
                        ref var tile = ref world.tileGrid.GetRef(loopArea.Position);
                        if (height <= 0 || height > tile.groundY)
                        {
                            tile.groundY = height;
                            tile.color = color;
                        }
                       
                    }
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
