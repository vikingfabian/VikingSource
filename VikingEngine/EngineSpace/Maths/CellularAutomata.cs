using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Maths
{
    class CellularAutomata
    {
        int[,] map;

        int birthLimit;
        int deathLimit;

        public CellularAutomata(int width, int height, int birthLimit, int deathLimit)
        {
            map = new int[width,height];
            this.birthLimit = birthLimit;
            this.deathLimit = deathLimit;
        }

        public void Iterate(int iterations, bool skipCorners)
        {
            int w = map.GetLength(0);
            int h = map.GetLength(1);

            for (int i = 0; i < iterations; ++i)
            {
                // Allocate temp map
                int[,] tempMap = new int[w, h];

                // Copy old map
                for (int x = 0; x < w; ++x)
                {
                    for (int y = 0; y < h; ++y)
                    {
                        tempMap[x, y] = map[x, y];
                    }
                }

                // Loop over each map position
                for (int x = 0; x < w; ++x)
                {
                    for (int y = 0; y < h; ++y)
                    {
                        int nbs = CountAliveNeighbours(x, y, skipCorners);

                        // If cell is active, check for starvation.
                        if (map[x, y] > 0)
                            if (nbs < deathLimit)
                                tempMap[x, y] = 0;
                            else
                                tempMap[x, y] = 1;
                        else // If cell is dead, check for birth
                            if (nbs > birthLimit)
                                tempMap[x, y] = 1;
                            else
                                tempMap[x, y] = 0;
                    }
                }

                // Overwrite old map
                map = tempMap;

                // Garbage collector, anyone? ...
            }
        }

        private void InitMap(int seed, float spawnChance)
        {
            PcgRandom prng = new PcgRandom(seed);

            ForXYLoop loop = new ForXYLoop();
            while (loop.Next())
            {
                IntVector2 p = loop.Position;
                map[p.X, p.Y] = prng.Chance(spawnChance) ? 1 : 0;
            }
        }
        
        private int CountAliveNeighbours(int x, int y, bool skipCorners)
        {
            int count = 0;

            ForXYLoop loop = new ForXYLoop(new IntVector2(-1), new IntVector2(2));
            while(loop.Next())
            {
                IntVector2 p = loop.Position;
                int neighbourX = x + p.X;
                int neighbourY = y + p.Y;
                if(skipCorners && !(p.Y == 0 || p.X == 0))
                {
                    continue;
                }

                if(p.X == 0 && p.Y == 0)
                {
                    //lalala
                }
                else if (neighbourX < 0 || neighbourY < 0 || neighbourX >= map.GetLength(0) || neighbourY >= map.GetLength(1))
                {
                    ++count; // count edge as living
                }
                else if(map[neighbourX, neighbourY] > 0)
                {
                    ++count; // Living cell found
                }
            }

            return count;
        }
    }
}
