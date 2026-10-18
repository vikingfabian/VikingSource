using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.EngineSpace.Maths;

namespace VikingEngine.DSSWars.Map.Generate
{
    struct BiomNode
    {
        public BiomType biom;
        public Vector2 pos;
    }

    class BiomsLayout
    {
        int Width;
        int Height;

        RandomObjects<BiomType>[,] biomGrid;

        public BiomsLayout(PcgRandom rnd, MapSizeCategory sizeCategory)
        {
            int add = 0;
            if (sizeCategory > MapSizeCategory.Medium)
            {
                add = (sizeCategory - MapSizeCategory.Medium);
            }
            Width = 6 + add * 2;
            Height = 4 + add * 2;

            biomGrid = new RandomObjects<BiomType>[Width, Height];


            for (int y = 0; y < Height; ++y)
            {
                float percY = y / (float)Height;

                //todo loop y
                if (percY < 0.25f)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        BiomType defaultBiom;
                        BiomType secondaryBiom;

                        if (x < MathExt.MultiplyInt(0.75, Width))
                        {
                            defaultBiom = BiomType.Frozen;
                            secondaryBiom = BiomType.Tundra;
                        }
                        else
                        {
                            defaultBiom = BiomType.Tundra;
                            secondaryBiom = BiomType.Frozen;
                        }

                        var options = new RandomObjects<BiomType>(new ObjectCommonessPair<BiomType>(10, defaultBiom));

                        if (rnd.Chance(0.6))
                        {
                            options.AddItem(secondaryBiom, 5);
                        }

                        if (rnd.Chance(0.2))
                        {
                            options.AddItem(BiomType.WetGreen, 20);
                        }
                        if (rnd.Chance(0.1))
                        {
                            options.AddItem(BiomType.Swamp, 10);
                        }
                        if (rnd.Chance(0.1))
                        {
                            options.AddItem(BiomType.Green, 20);
                        }


                        biomGrid[x, y] = options;
                    }
                }
                else if (percY < 0.5f)
                {
                    BiomType defaultBiom = BiomType.WetGreen;

                    for (int x = 0; x < Width; x++)
                    {
                        var options = new RandomObjects<BiomType>(new ObjectCommonessPair<BiomType>(10, defaultBiom));
                        options.AddItem(BiomType.GreenDry, 4);

                        if (rnd.Chance(0.25))
                        {
                            options.AddItem(BiomType.Green, 20);
                        }
                        if (rnd.Chance(0.1))
                        {
                            options.AddItem(BiomType.Hills, 20);
                        }
                        if (rnd.Chance(0.2))
                        {
                            options.AddItem(BiomType.WetGreen, 20);
                        }
                        if (rnd.Chance(0.1))
                        {
                            options.AddItem(BiomType.Swamp, 10);
                        }
                        if (rnd.Chance(0.1))
                        {
                            if (x < MathExt.MultiplyInt(0.75, Width))
                            {
                                options.AddItem(BiomType.Frozen, 20);
                            }
                            else
                            {
                                options.AddItem(BiomType.Tundra, 20);
                            }
                        }

                        biomGrid[x, y] = options;
                    }
                }

                else if (percY < 0.75f)
                {
                    BiomType defaultBiom = BiomType.Green;

                    for (int x = 0; x < Width; x++)
                    {
                        var options = new RandomObjects<BiomType>(new ObjectCommonessPair<BiomType>(10, defaultBiom));
                        options.AddItem(BiomType.GreenDry, 4);
                        if (rnd.Chance(0.1))
                        {
                            options.AddItem(BiomType.WetGreen, 20);
                        }
                        if (rnd.Chance(0.1))
                        {
                            options.AddItem(BiomType.Hills, 10);
                        }
                        if (rnd.Chance(0.1))
                        {
                            if (x < MathExt.MultiplyInt(0.35, Width))
                            {
                                options.AddItem(BiomType.DarkLands, 20);
                            }
                            else
                            {
                                options.AddItem(BiomType.YellowDry, 20);
                            }
                        }

                        biomGrid[x, y] = options;
                    }
                }
                else
                {

                    for (int x = 0; x < Width; x++)
                    {
                        BiomType defaultBiom;
                        //BiomType secondaryBiom;

                        RandomObjects<BiomType> options;

                        if (x < MathExt.MultiplyInt(0.35, Width))
                        {
                            options = new RandomObjects<BiomType>(new ObjectCommonessPair<BiomType>(10, BiomType.Green));
                            options.AddItem(BiomType.GreenDry, 6);
                        }
                        else
                        {
                            if (x < MathExt.MultiplyInt(0.75, Width))
                            {
                                options = new RandomObjects<BiomType>(new ObjectCommonessPair<BiomType>(10, BiomType.DarkLands));

                            }
                            else
                            {
                                options = new RandomObjects<BiomType>(new ObjectCommonessPair<BiomType>(10, BiomType.YellowDry));
                            }

                            if (rnd.Chance(0.4))
                            {
                                options.AddItem(BiomType.RedDry, 5);
                            }

                            if (x > 2 && rnd.Chance(0.25))
                            {
                                options.AddItem(BiomType.Green, 10);
                                options.AddItem(BiomType.GreenDry, 20);
                            }
                            else if (rnd.Chance(0.2))
                            {
                                options.AddItem(BiomType.GreenDry, 5);
                            }
                        }

                        biomGrid[x, y] = options;
                    }
                }
            }
                //Add red desert
                int redDesertX = rnd.Int(Width);
                int redDesertY = rnd.Int(MathExt.MultiplyInt(Width, 0.4), Height);

                for (int x = redDesertX - 1; x <= redDesertX + 1; x++)
                {
                    if (Bound.IsWithin(x, 0, Width - 1))
                    {
                        var options = biomGrid[x, redDesertY];
                        options.AddItem(BiomType.RedDry, 50);
                    }
                }
            
        }
        
        public void GenerateNodes(IconWorldData world)
        {
            const int ScaleUp = 2;
            int NodeGridW = Width * ScaleUp;
            int NodeGridH = Height * ScaleUp;


            List<BiomNode> nodes = new List<BiomNode>(NodeGridW * NodeGridH * 8); //TODO lookup grid

            Vector2 cellsize = world.iconGrid.Size.Vec / new Vector2(NodeGridW, NodeGridH);
            for (int y = 0; y < NodeGridH; y++)
            {
                for (int x = 0; x < NodeGridW; x++)
                {
                    var biom = biomGrid[x / ScaleUp, y / ScaleUp].GetRandom();
                    int nodesCount = world.rnd.Int(2, 4);
                    for (int i = 0; i < nodesCount; i++)
                    {

                        BiomNode node = new BiomNode()
                        {
                            biom = biom,     
                            pos = new Vector2(x, y) * cellsize + world.rnd.vector2(cellsize), 
                        };

                        nodes.Add(node);
                    }
                }
            }

            EngineSpace.Maths.SimplexNoise2D noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.metaData.worldId.seed + 3);
            NoiseOptions noiseOpt = new NoiseOptions(true, 0.1f, 4, 1f, 10f);

            Parallel.For(0, world.iconGrid.Size.X, x =>
            //for (int x =0; x < world.iconGrid.Size.X; x++)
            {
                for (int y = 0; y < world.iconGrid.Size.Y; y++)
                {
                    //TODO use three nodes in the noise
                    BiomNode node1 = new BiomNode();
                    float node1Dist = float.MaxValue;
                    BiomNode node2 = new BiomNode();
                    float node2Dist = float.MaxValue;

                    foreach (var node in nodes)
                    {
                        float dist = VectorExt.Length(node.pos.X - x, node.pos.Y - y);
                        if (dist < node2Dist)
                        {
                            if (dist < node1Dist)
                            {
                                node1Dist = dist;
                                node1 = node;
                            }
                            else
                            {
                                node2Dist = dist;
                                node2 = node;
                            }
                        }
                    }

                    float noiseValue = noiseMap.OctaveNoise2D_Normal(noiseOpt, x, y);
                    float gradientPos = node1Dist / (node1Dist + node2Dist);
                    if (noiseValue * 0.85f > gradientPos)
                    {
                        world.iconGrid.GetRef(x, y).biom1 = node1.biom;
                    }
                    else
                    {
                        world.iconGrid.GetRef(x, y).biom1 = node2.biom;
                    }
                }
            });
        }
        
        /*
        public void GenerateNodes(IconWorldData world)
        {
            const int ScaleUp = 2;
            int NodeGridW = Width * ScaleUp;
            int NodeGridH = Height * ScaleUp;

            // Fixed malformed list initialization and allocated a reasonable starting capacity
            List<BiomNode> nodes = new List<BiomNode>(NodeGridW * NodeGridH * 4);

            // TODO: lookup grid

            Vector2 cellsize = world.iconGrid.Size.Vec / new Vector2(NodeGridW, NodeGridH);
            for (int y = 0; y < NodeGridH; y++)
            {
                for (int x = 0; x < NodeGridW; x++)
                {
                    var biom = biomGrid[x / ScaleUp, y / ScaleUp].GetRandom();
                    int nodesCount = world.rnd.Int(2, 4);

                    for (int i = 0; i < nodesCount; i++)
                    {
                        BiomNode node = new BiomNode()
                        {
                            biom = biom,
                            pos = new Vector2(x, y) * cellsize + world.rnd.vector2(cellsize),
                        };

                        nodes.Add(node);
                    }
                }
            }

            EngineSpace.Maths.SimplexNoise2D noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.metaData.worldId.seed + 3);
            NoiseOptions noiseOpt = new NoiseOptions(true, 0.1f, 4, 1f, 10f);

            Parallel.For(0, world.iconGrid.Size.X, x =>
            {
                for (int y = 0; y < world.iconGrid.Size.Y; y++)
                {
                    // TODO: use three nodes in the noise
                    BiomNode node1 = new BiomNode();
                    float node1DistSq = float.MaxValue;

                    BiomNode node2 = new BiomNode();
                    float node2DistSq = float.MaxValue;

                    foreach (var node in nodes)
                    {
                        // Improvement: Use squared distance to avoid Math.Sqrt inside the heavy loop
                        float dx = node.pos.X - x;
                        float dy = node.pos.Y - y;
                        float distSq = (dx * dx) + (dy * dy);

                        // Fixed: Shift the old closest node to second closest when a new closest is found
                        if (distSq < node1DistSq)
                        {
                            node2DistSq = node1DistSq;
                            node2 = node1;

                            node1DistSq = distSq;
                            node1 = node;
                        }
                        else if (distSq < node2DistSq)
                        {
                            node2DistSq = distSq;
                            node2 = node;
                        }
                    }

                    // Perform the expensive Square Root only twice per pixel to get the actual distance for the gradient
                    float node1Dist = (float)Math.Sqrt(node1DistSq);
                    float node2Dist = (float)Math.Sqrt(node2DistSq);

                    float noiseValue = noiseMap.OctaveNoise2D_Normal(noiseOpt, x, y);
                    float gradientPos = node1Dist / (node1Dist + node2Dist);

                    // Fixed syntax error: noiseValue0.85f -> noiseValue * 0.85f
                    if (noiseValue * 0.85f > gradientPos)
                    {
                        world.iconGrid.GetRef(x, y).biom1 = node1.biom;
                    }
                    else
                    {
                        world.iconGrid.GetRef(x, y).biom1 = node2.biom;
                    }
                }
            });
        }
        */
        /*
        public void GenerateNodes(IconWorldData world)
        {
            const int ScaleUp = 2;
            int NodeGridW = Width * ScaleUp;
            int NodeGridH = NodeGridW; // Assuming Width/Height logic, adjusted based on your original snippet

            // 1. Initialize the Lookup Grid
            List<BiomNode>[,] nodeGrid = new List<BiomNode>[NodeGridW, NodeGridH];
            for (int y = 0; y < NodeGridH; y++)
            {
                for (int x = 0; x < NodeGridW; x++)
                {
                    nodeGrid[x, y] = new List<BiomNode>();
                }
            }

            Vector2 cellsize = world.iconGrid.Size.Vec / new Vector2(NodeGridW, NodeGridH);

            // 2. Populate the Lookup Grid
            for (int y = 0; y < NodeGridH; y++)
            {
                for (int x = 0; x < NodeGridW; x++)
                {
                    var biom = biomGrid[x / ScaleUp, y / ScaleUp].GetRandom();
                    int nodesCount = world.rnd.Int(2, 4);
                    for (int i = 0; i < nodesCount; i++)
                    {
                        BiomNode node = new BiomNode()
                        {
                            biom = biom,
                            pos = new Vector2(x, y) * cellsize + world.rnd.vector2(cellsize),
                        };

                        nodeGrid[x, y].Add(node);
                    }
                }
            }

            EngineSpace.Maths.SimplexNoise2D noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.metaData.worldId.seed + 3);
            NoiseOptions noiseOpt = new NoiseOptions(true, 0.1f, 4, 1f, 10f);

            Parallel.For(0, world.iconGrid.Size.X, x =>
            {
                try
                {
                    for (int y = 0; y < world.iconGrid.Size.Y; y++)
                    {
                        BiomNode n1 = new BiomNode(); float d1Sq = float.MaxValue;
                        BiomNode n2 = new BiomNode(); float d2Sq = float.MaxValue;
                        BiomNode n3 = new BiomNode(); float d3Sq = float.MaxValue;

                        // Determine which grid cell this pixel falls into
                        int cx = Math.Max(0, Math.Min((int)(x / cellsize.X), NodeGridW - 1));
                        int cy = Math.Max(0, Math.Min((int)(y / cellsize.Y), NodeGridH - 1));

                        // Define the 3x3 search area around the pixel's cell
                        int startX = Math.Max(0, cx - 1);
                        int endX = Math.Min(NodeGridW - 1, cx + 1);
                        int startY = Math.Max(0, cy - 1);
                        int endY = Math.Min(NodeGridH - 1, cy + 1);

                        // 3. Search only the local 3x3 grid cells
                        for (int gridY = startY; gridY <= endY; gridY++)
                        {
                            for (int gridX = startX; gridX <= endX; gridX++)
                            {
                                foreach (var node in nodeGrid[gridX, gridY])
                                {
                                    // Using squared distance avoids the expensive square root inside the hot loop
                                    float dx = node.pos.X - x;
                                    float dy = node.pos.Y - y;
                                    float distSq = (dx * dx) + (dy * dy);

                                    // Properly shift previous nodes down when a closer one is found
                                    if (distSq < d3Sq)
                                    {
                                        if (distSq < d1Sq)
                                        {
                                            d3Sq = d2Sq; n3 = n2;
                                            d2Sq = d1Sq; n2 = n1;
                                            d1Sq = distSq; n1 = node;
                                        }
                                        else if (distSq < d2Sq)
                                        {
                                            d3Sq = d2Sq; n3 = n2;
                                            d2Sq = distSq; n2 = node;
                                        }
                                        else
                                        {
                                            d3Sq = distSq; n3 = node;
                                        }
                                    }
                                }
                            }
                        }

                        // Calculate actual distances for smooth weighting
                        float d1 = (float)Math.Sqrt(d1Sq);
                        float d2 = (float)Math.Sqrt(d2Sq);
                        float d3 = (float)Math.Sqrt(d3Sq);

                        // 4. Inverse Distance Weighting to blend the 3 nodes
                        // Small offset (0.0001f) prevents division by zero if pixel is exactly on the node
                        float w1 = 1.0f / (d1 + 0.0001f);
                        float w2 = 1.0f / (d2 + 0.0001f);
                        float w3 = 1.0f / (d3 + 0.0001f);

                        float totalWeight = w1 + w2 + w3;
                        float p1 = w1 / totalWeight; // Probability of picking node 1
                        float p2 = w2 / totalWeight; // Probability of picking node 2

                        float noiseValue = noiseMap.OctaveNoise2D_Normal(noiseOpt, x, y);
                        float n = noiseValue * 0.85f;

                        // Assign biome based on noise falling into the probability buckets
                        if (n < p1)
                        {
                            world.iconGrid.GetRef(x, y).biom1 = n1.biom;
                        }
                        else if (n < p1 + p2)
                        {
                            world.iconGrid.GetRef(x, y).biom1 = n2.biom;
                        }
                        else
                        {
                            world.iconGrid.GetRef(x, y).biom1 = n3.biom;
                        }
                    }
                }
                catch (Exception e)
                {
                    BlueScreen.ThreadException = e;
                }
            });
        }
        */
        public BiomType get(WorldData world, Vector2 pos)
        {
            int x = (int)(pos.X / world.Size.X * Width);
            int y = (int)(pos.Y / world.Size.Y * Height);

            return biomGrid[x, y].GetRandom(world.rnd);
        }

        public BiomType get(IconWorldData world, Vector2 pos)
        {
            int x = (int)(pos.X / world.iconGrid.Size.X * Width);
            int y = (int)(pos.Y / world.iconGrid.Size.Y * Height);

            return biomGrid[x, y].GetRandom(world.rnd);
        }
    }

}
