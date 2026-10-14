using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        const int Width = 6;
        const int Height = 4;

        RandomObjects<BiomType>[,] biomGrid;

        public BiomsLayout(PcgRandom rnd)
        {
            biomGrid = new RandomObjects<BiomType>[Width, Height];

            {
                int y = 0;
                              

                for (int x = 0; x < Width; x++)
                {
                    BiomType defaultBiom;
                    BiomType secondaryBiom;

                    if (x < 4)
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

            {
                int y = 1;
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
                        if (x < 4)
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

            {
                int y = 2;
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
                        if (x < 2)
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

            {
                int y = 3;
                
                for (int x = 0; x < Width; x++)
                {
                    BiomType defaultBiom;
                    //BiomType secondaryBiom;

                    RandomObjects<BiomType> options;

                    if (x < 2)
                    {
                        options = new RandomObjects<BiomType>(new ObjectCommonessPair<BiomType>(10, BiomType.Green));
                        options.AddItem(BiomType.GreenDry, 6);
                    }
                    else
                    {
                        if (x < 4)
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

            //Add red desert
            int redDesertX = rnd.Int(Width);
            int redDesertY = rnd.Int(2, Height);

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
            const int NodeGridW = Width * ScaleUp;
            const int NodeGridH = Height * ScaleUp;


            List<BiomNode> nodes = new List<BiomNode>(NodeGridW * NodeGridH * 8);

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

            EngineSpace.Maths.SimplexNoise2D noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.metaData2.seed + 3);
            NoiseOptions noiseOpt = new NoiseOptions(true, 0.1f, 4, 1f, 10f);

            Parallel.For(0, world.iconGrid.Size.X, x =>
            {
                for (int y = 0; y < world.iconGrid.Size.Y; y++)
                {
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
                    if (noiseValue * 0.8f > gradientPos)
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

    //class BiomLayoutMember
    //{
    //    RandomObjects<BiomType> bioms;

    //    public BiomLayoutMember(RandomObjects<BiomType> bioms)
    //    {
    //        this.bioms = new RandomObjects<BiomType>( new ObjectCommonessPair<BiomType>(,);
    //    }
    //}
}
