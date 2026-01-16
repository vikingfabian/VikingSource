using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Generate;

namespace VikingEngine.DSSWars.Map.Settings
{
    class CityAreaCulture
    {
        public double land = 0, water = 0, plain = 0, forest = 0, mountain = 0, dryBiom = 0;
        public double percWater;
        public double percForest;
        public double percPlains;
        public double percMountain;
        public double percDry;
        public double worldPercX, worldPercY;
        public CityAreaCulture(City city, WorldData world)
        {
            Rectangle2 cultureArea = Rectangle2.FromCenterTileAndRadius(city.tilePos, 3);
            double total = cultureArea.Area;
            ForXYLoop loop = new ForXYLoop(cultureArea);

            while (loop.Next())
            {
                var tile = world.tileGrid.Get(loop.Position);
                if (tile.IsWater())
                {
                    ++water;
                }
                else
                {
                    ++land;
                    switch (tile.heightSett().culture)
                    {
                        case TerrainCultureType.Plains:
                            ++plain;
                            break;
                        case TerrainCultureType.Forest:
                            ++forest;
                            break;
                        case TerrainCultureType.Mountain:
                            ++mountain;
                            break;
                    }
                    if (tile.biom == BiomType.YellowDry || tile.biom == BiomType.RedDry)
                    {
                        ++dryBiom;
                    }
                }
            }

            percWater = water / total;
            percForest = forest / land;
            percPlains = plain / land;
            percMountain = mountain / land;
            percDry = dryBiom / land;

            //Collect cultures
            worldPercX = city.tilePos.X / (double)world.Size.X;
            worldPercY = city.tilePos.Y / (double)world.Size.Y;
        }
    }
}
