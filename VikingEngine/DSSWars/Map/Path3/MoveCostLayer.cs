using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Path3
{
    class MoveCostLayer
    {
        public const int Dir_NorthToSouth = 0;
        public const int Dir_DiagonalNorthEast = 1;
        public const int Dir_WestToEast = 2;
        public const int Dir_DiagonalSouthEast = 3;

        public const int Layer2TileWidth = 4;
        
        public const int StoredDirections = 4;
        protected IntVector2 size;
        public MoveCost[] cost_n_ne_e_se;

        public MoveCostLayer(IntVector2 size)
        { 
            this.size = size;
            cost_n_ne_e_se = new MoveCost[size.Area() * StoredDirections];
        }

        public void Set(IntVector2 layerPos, int direction, MoveCost cost)
        {
            int ix = (layerPos.X + layerPos.Y * size.X) * StoredDirections + direction;
            cost_n_ne_e_se[ix] = cost;
        }

        public void Set(int layerIxStart, int direction, MoveCost cost)
        {
            int ix = layerIxStart + direction;
            cost_n_ne_e_se[ix] = cost;
        }
    }

    class MoveCostLayer4 : MoveCostLayer
    {
        const byte TileStatus_None = 0;
        const byte TileStatus_NeedUpdate = 1;
        const byte TileStatus_Initialized = 2;

        public const int Layer4TileWidth = 64;
        public Grid2D<byte> tileStatus;
        public MoveCostLayer4(WorldData world)
            :base(world.subTileGrid.Size / Layer4TileWidth)
        {
            tileStatus = new Grid2D<byte>(size);
        }
    }

    class MoveCostLayers
    {
        Grid2D<MoveCost> layer0;
        MoveCostLayer layer1_temp;
        MoveCostLayer layer2;
        MoveCostLayer layer3_temp;
        MoveCostLayer4 layer4;

        public MoveCostLayers(WorldData world)
        {
            layer0 = new Grid2D<MoveCost>(world.subTileGrid.Size);
            layer1_temp = new MoveCostLayer(new IntVector2(MoveCostLayer4.Layer4TileWidth / 2));
            layer2 = new MoveCostLayer(world.subTileGrid.Size / MoveCostLayer.Layer2TileWidth);
            layer3_temp = new MoveCostLayer(new IntVector2(2));
            layer4 = new MoveCostLayer4(world);
        }

        void generateLayer4Tile(IntVector2 tile)
        { 
            var status = layer4.tileStatus.GetRef(tile);
            if (status == 1)
            {
                status = 2;
            }

            //Layer 0
            IntVector2 subTilePos = tile * MoveCostLayer4.Layer4TileWidth;
            IntVector2 subTileEnd = subTilePos + MoveCostLayer4.Layer4TileWidth;

            IntVector2 pos = IntVector2.Zero;
            //for (pos.Y = subTilePos.Y; pos.Y < subTileEnd.Y; ++pos.Y)
            //{
            //    for (pos.X = subTilePos.X; pos.X < subTileEnd.X; ++pos.X)
            //    {
            //        layer0.array[pos.X, pos.Y] = DssRef.world.subTileGrid.array[pos.X, pos.Y].GetMoveCost();
            //    }
            //}

            //Layer 0 and 1
            float expensivePathAdd = 0.3f;
            float cheapPathAdd = 0.7f;

            MoveCost path1, path2;
            //IntVector2 lay1pos = IntVector2.Zero;
            int lay1IndexStart = 0;
            for (pos.Y = subTilePos.Y; pos.Y < subTileEnd.Y; pos.Y += 2)
            {
                //lay1pos.Y++;
                for (pos.X = subTilePos.X; pos.X < subTileEnd.X; pos.X += 2)
                {
                    MoveCost topLeft = DssRef.world.subTileGrid.array[pos.X, pos.Y].GetMoveCost();
                    layer0.array[pos.X, pos.Y] = topLeft;
                    MoveCost topRight = DssRef.world.subTileGrid.array[pos.X + 1, pos.Y].GetMoveCost();
                    layer0.array[pos.X + 1, pos.Y] = topRight;
                    MoveCost bottomLeft = DssRef.world.subTileGrid.array[pos.X, pos.Y + 1].GetMoveCost();
                    layer0.array[pos.X, pos.Y + 1] = bottomLeft;
                    MoveCost bottomRight = DssRef.world.subTileGrid.array[pos.X + 1, pos.Y + 1].GetMoveCost();
                    layer0.array[pos.X + 1, pos.Y + 1] = bottomRight;

                    //Horizontal
                    path1 = MoveCost.Sum(topLeft, topRight);
                    path2 = MoveCost.Sum(bottomLeft, bottomRight);
                    layer1_temp.Set(lay1IndexStart, MoveCostLayer.Dir_WestToEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Vertical
                    path1 = MoveCost.Sum(topLeft, bottomLeft);
                    path2 = MoveCost.Sum(topRight, bottomRight);
                    layer1_temp.Set(lay1IndexStart, MoveCostLayer.Dir_NorthToSouth, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Diagonal NE
                    path1 = MoveCost.Sum(bottomLeft, topLeft, topRight);
                    path1 = MoveCost.Sum(bottomLeft, bottomRight, topRight);
                    layer1_temp.Set(lay1IndexStart, MoveCostLayer.Dir_DiagonalNorthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Diagonal SE
                    path1 = MoveCost.Sum(topLeft, bottomLeft, bottomRight);
                    path1 = MoveCost.Sum(topLeft, topRight, bottomRight);
                    layer1_temp.Set(lay1IndexStart, MoveCostLayer.Dir_DiagonalNorthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));


                    //lay1pos.X++;
                    lay1IndexStart += MoveCostLayer.StoredDirections;
                }
                //lay1pos.X = 0;
            }

            //Layer 2


            status = 2;
        }
    }

    struct MoveCost
    {
        public static readonly MoveCost Empty = new MoveCost();

        public float land;
        public float water;

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
