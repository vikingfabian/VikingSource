using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars.Map.Generate
{
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
                BiomType defaultBiom = BiomType.Frozen;                

                for (int x = 0; x < Width; x++)
                {
                    var options = new RandomObjects<BiomType>(new ObjectCommonessPair<BiomType>(10, defaultBiom));
                    if (rnd.Chance(0.25))
                    {
                        options.AddItem(BiomType.WetGreen, 20);
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
                    if (rnd.Chance(0.25))
                    {
                        options.AddItem(BiomType.Green, 20);
                    }
                    if (rnd.Chance(0.1))
                    {
                        options.AddItem(BiomType.Frozen, 20);
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
                    if (rnd.Chance(0.25))
                    {
                        options.AddItem(BiomType.WetGreen, 20);
                    }
                    if (rnd.Chance(0.1))
                    {
                        options.AddItem(BiomType.YellowDry, 20);
                    }

                    biomGrid[x, y] = options;
                }
            }

            {
                int y = 3;
                BiomType defaultBiom = BiomType.YellowDry;

                for (int x = 0; x < Width; x++)
                {
                    var options = new RandomObjects<BiomType>(new ObjectCommonessPair<BiomType>(10, defaultBiom));
                    if (rnd.Chance(0.25))
                    {
                        options.AddItem(BiomType.Green, 20);
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

        public BiomType get(WorldData world, Vector2 pos)
        {
            int x = (int)(pos.X / world.Size.X * Width);
            int y = (int)(pos.Y / world.Size.Y * Height);

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
