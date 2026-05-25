using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Path3
{

    struct MoveCost
    {
        public static readonly MoveCost Empty = new MoveCost();

        public float land;
        public float water;

        public override string ToString()
        {
            return $"cost land: {land}, water: {water}";
        }

        public MoveCost(float land)
        {
            this.land = land;
            this.water = land * 100;
        }

        public MoveCost(float land, float water)
        {
            this.land = land;
            this.water = water;
        }

        public static MoveCost Sum(MoveCost cost1, MoveCost cost2)
        {
            cost1.land += cost2.land;
            cost1.water += cost2.water;
            return cost1;
        }

        public static MoveCost Sum(MoveCost cost1, MoveCost cost2, MoveCost cost3)
        {
            const float DiagonalCost = 0.7f;
            cost1.land = (cost1.land + cost2.land + cost3.land) * DiagonalCost;
            cost1.water = (cost1.water + cost2.water + cost3.water) * DiagonalCost;
            return cost1;
        }

        public static MoveCost Total(ref MoveCost path1, ref MoveCost path2,
             float cheapPathAdd, float expensivePathAdd)
        {
            MoveCost result = new MoveCost();

            if (path1.land < path2.land)
            {
                result.land = path1.land * cheapPathAdd + path2.land * expensivePathAdd;
            }
            else
            {
                result.land = path2.land * cheapPathAdd + path1.land * expensivePathAdd;
            }

            if (path1.water < path2.water)
            {
                result.water = path1.water * cheapPathAdd + path2.water * expensivePathAdd;
            }
            else
            {
                result.water = path2.water * cheapPathAdd + path1.water * expensivePathAdd;
            }

            return result;
        }
    }
}
