using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.Map.Path3
{

    class MoveCostLayer
    {
        public const int Dir_NorthToSouth = 0;
        public const int Dir_DiagonalNorthEast = 1;
        public const int Dir_WestToEast = 2;
        public const int Dir_DiagonalSouthEast = 3;

        public const int Layer2TileWidth = 4;
        public const int Layer4TileWidth = 16;
        public const int Lay2WidthOnLay4 = Layer4TileWidth / Layer2TileWidth;

        public const int StoredDirections = 4;
        public IntVector2 size;
        public MoveCost[] cost_n_ne_e_se;
        //public Grid2D<CostLayerTile> tiles;

        public MoveCostLayer(IntVector2 size)
        {
            this.size = size;
            cost_n_ne_e_se = new MoveCost[size.Area() * StoredDirections];
            //tiles = new Grid2D<CostLayerTile>(size);
        }

        public static int ConvertDir8(int dir8)
        {
            if (dir8 > Dir_DiagonalSouthEast)
            {
                return dir8 - 4;
            }
            return dir8;
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

        public MoveCost Get(IntVector2 layerPos, int direction)
        {
            int ix = (layerPos.X + layerPos.Y * size.X) * StoredDirections + direction;
            return cost_n_ne_e_se[ix];
        }

        public MoveCost Get(int layerPos, int direction)
        {
            int ix = layerPos + direction;
            return cost_n_ne_e_se[ix];
        }

        public int GetPositionStart(IntVector2 layerPos)
        {
            return (layerPos.X + layerPos.Y * size.X) * StoredDirections;
        }

        public bool InBounds(IntVector2 position)
        {
            return position.X >= 0 && position.X < size.X &&
                position.Y >= 0 && position.Y < size.Y;
        }
    }

    class MoveCostLayer4 : MoveCostLayer
    {
        public const byte TileStatus_None = 0;
        public const byte TileStatus_NeedUpdate = 1;
        public const byte TileStatus_Updateing = 2;
        public const byte TileStatus_Initialized = 3;

        
        public Grid1D<byte> tileStatus;
        public MoveCostLayer4(WorldData world)
            :base(world.subTileGrid.Size / Layer4TileWidth)
        {
            tileStatus = new Grid1D<byte>(size);
        }
    }

    class Path3Thread
    {
        public MoveCostLayer layer1_temp;
        public MoveCostLayer layer3_temp;

        public Path3Thread()
        {
            layer1_temp = new MoveCostLayer(new IntVector2(MoveCostLayer.Layer4TileWidth / 2));
            layer3_temp = new MoveCostLayer(new IntVector2(2));
        }
    }

    class MoveCostLayers
    {
        Grid2D<MoveCost> layer0;
        MoveCostLayer layer2;
        MoveCostLayer4 layer4;

        public MoveCostLayers(WorldData world)
        {
            layer0 = new Grid2D<MoveCost>(world.subTileGrid.Size);
            layer2 = new MoveCostLayer(world.subTileGrid.Size / MoveCostLayer.Layer2TileWidth);
            layer4 = new MoveCostLayer4(world);
        }

        public void Test()
        {
            Path3Thread thread = new Path3Thread();
            generateLayer4Tile(thread, new IntVector2(1, 0));
        }

        void generateLayer4Tile(Path3Thread thread, IntVector2 lay4tile)
        { 
            var status = layer4.tileStatus.GetRef(lay4tile);
            if (status == MoveCostLayer4.TileStatus_NeedUpdate)
            {
                status = MoveCostLayer4.TileStatus_Updateing;
            }

            //Layer 0
            IntVector2 lay0TileTopLeft = lay4tile * MoveCostLayer4.Layer4TileWidth;
            IntVector2 subTilePos = lay0TileTopLeft;
            IntVector2 subTileEnd = subTilePos + MoveCostLayer4.Layer4TileWidth;

            IntVector2 pos = IntVector2.Zero;

            //Layer 0 and 1
            float expensivePathAdd = 0.3f;
            float cheapPathAdd = 0.7f;

            MoveCost path1, path2;
            int lay1IndexStart = 0;
            for (pos.Y = subTilePos.Y; pos.Y < subTileEnd.Y; pos.Y += 2)
            {
                
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
                    thread.layer1_temp.Set(lay1IndexStart, MoveCostLayer.Dir_WestToEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Vertical
                    path1 = MoveCost.Sum(topLeft, bottomLeft);
                    path2 = MoveCost.Sum(topRight, bottomRight);
                    thread.layer1_temp.Set(lay1IndexStart, MoveCostLayer.Dir_NorthToSouth, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Diagonal NE
                    path1 = MoveCost.Sum(bottomLeft, topLeft, topRight);
                    path2 = MoveCost.Sum(bottomLeft, bottomRight, topRight);
                    thread.layer1_temp.Set(lay1IndexStart, MoveCostLayer.Dir_DiagonalNorthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Diagonal SE
                    path1 = MoveCost.Sum(topLeft, bottomLeft, bottomRight);
                    path2 = MoveCost.Sum(topLeft, topRight, bottomRight);
                    thread.layer1_temp.Set(lay1IndexStart, MoveCostLayer.Dir_DiagonalSouthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));


                    
                    lay1IndexStart += MoveCostLayer.StoredDirections;
                }
            }

            //Layer 2
            expensivePathAdd = 0.15f;
            cheapPathAdd = 0.85f;
            IntVector2 lay2TileTopLeft = lay0TileTopLeft / MoveCostLayer.Layer2TileWidth;
            IntVector2 lay2End = lay2TileTopLeft + MoveCostLayer.Lay2WidthOnLay4;
            IntVector2 lay1pos = IntVector2.Zero;
            for (pos.Y = lay2TileTopLeft.Y; pos.Y < lay2End.Y; pos.Y++)
            {
                
                for (pos.X = lay2TileTopLeft.X; pos.X < lay2End.X; pos.X++)
                {
                    int lay2PosIx = layer2.GetPositionStart(pos);

                    int topLeftPos = thread.layer1_temp.GetPositionStart(lay1pos);
                    int topRightPos = thread.layer1_temp.GetPositionStart(lay1pos.ReturnSum(1, 0));
                    int bottomLeftPos = thread.layer1_temp.GetPositionStart(lay1pos.ReturnSum(0, 1));
                    int bottomRightPos = thread.layer1_temp.GetPositionStart(lay1pos.ReturnSum(1, 1));
                    
                    

                    

                    lay1pos.X+= 2;
                }
                lay1pos.Y += 2;
                lay1pos.X = 0;
            }

            //layer 3
            expensivePathAdd = 0.07f;
            cheapPathAdd = 0.93f;
            IntVector2 lay2Pos;
            for (pos.Y = 0; pos.Y < thread.layer3_temp.size.X; pos.Y++)
            {
                for (pos.X = 0; pos.X < thread.layer3_temp.size.Y; pos.X++)
                {
                    int lay3PosIx = thread.layer3_temp.GetPositionStart(pos);

                    lay2Pos = lay2TileTopLeft + pos * 2;

                    int topLeftPos = layer2.GetPositionStart(lay2Pos);
                    int topRightPos = layer2.GetPositionStart(lay2Pos.ReturnSum(1, 0));
                    int bottomLeftPos = layer2.GetPositionStart(lay2Pos.ReturnSum(0, 1));
                    int bottomRightPos = layer2.GetPositionStart(lay2Pos.ReturnSum(1, 1));

                    //Horizontal
                    path1 = MoveCost.Sum(layer2.Get(topLeftPos, MoveCostLayer.Dir_WestToEast), layer2.Get(topRightPos, MoveCostLayer.Dir_WestToEast));
                    path2 = MoveCost.Sum(layer2.Get(bottomLeftPos, MoveCostLayer.Dir_WestToEast), layer2.Get(bottomRightPos, MoveCostLayer.Dir_WestToEast));
                    thread.layer3_temp.Set(lay3PosIx, MoveCostLayer.Dir_WestToEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Vertical
                    path1 = MoveCost.Sum(layer2.Get(topLeftPos, MoveCostLayer.Dir_NorthToSouth), layer2.Get(bottomLeftPos, MoveCostLayer.Dir_NorthToSouth));
                    path2 = MoveCost.Sum(layer2.Get(topRightPos, MoveCostLayer.Dir_NorthToSouth), layer2.Get(bottomRightPos, MoveCostLayer.Dir_NorthToSouth));
                    thread.layer3_temp.Set(lay3PosIx, MoveCostLayer.Dir_NorthToSouth, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Diagonal NE
                    path1 = MoveCost.Sum(layer2.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), layer2.Get(topLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), layer2.Get(topRightPos, MoveCostLayer.Dir_DiagonalNorthEast));
                    path2 = MoveCost.Sum(layer2.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), layer2.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalNorthEast), layer2.Get(topRightPos, MoveCostLayer.Dir_DiagonalNorthEast));
                    thread.layer3_temp.Set(lay3PosIx, MoveCostLayer.Dir_DiagonalNorthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Diagonal SE
                    path1 = MoveCost.Sum(layer2.Get(topLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), layer2.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), layer2.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalSouthEast));
                    path2 = MoveCost.Sum(layer2.Get(topLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), layer2.Get(topRightPos, MoveCostLayer.Dir_DiagonalSouthEast), layer2.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalSouthEast));
                    thread.layer3_temp.Set(lay3PosIx, MoveCostLayer.Dir_DiagonalSouthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                }
            }

            //Layer 4
            expensivePathAdd = 0.03f;
            cheapPathAdd = 0.97f;
            
            {
                int lay4PosIx = layer4.GetPositionStart(lay4tile);

                int topLeftPos = thread.layer3_temp.GetPositionStart(new IntVector2(0, 0));
                int topRightPos = thread.layer3_temp.GetPositionStart(new IntVector2(1, 0));
                int bottomLeftPos = thread.layer3_temp.GetPositionStart(new IntVector2(0, 1));
                int bottomRightPos = thread.layer3_temp.GetPositionStart(new IntVector2(1, 1));

                //Horizontal
                path1 = MoveCost.Sum(thread.layer3_temp.Get(topLeftPos, MoveCostLayer.Dir_WestToEast), thread.layer3_temp.Get(topRightPos, MoveCostLayer.Dir_WestToEast));
                path2 = MoveCost.Sum(thread.layer3_temp.Get(bottomLeftPos, MoveCostLayer.Dir_WestToEast), thread.layer3_temp.Get(bottomRightPos, MoveCostLayer.Dir_WestToEast));
                layer4.Set(lay4PosIx, MoveCostLayer.Dir_WestToEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                //Vertical
                path1 = MoveCost.Sum(thread.layer3_temp.Get(topLeftPos, MoveCostLayer.Dir_NorthToSouth), thread.layer3_temp.Get(bottomLeftPos, MoveCostLayer.Dir_NorthToSouth));
                path2 = MoveCost.Sum(thread.layer3_temp.Get(topRightPos, MoveCostLayer.Dir_NorthToSouth), thread.layer3_temp.Get(bottomRightPos, MoveCostLayer.Dir_NorthToSouth));
                layer4.Set(lay4PosIx, MoveCostLayer.Dir_NorthToSouth, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                //Diagonal NE
                path1 = MoveCost.Sum(thread.layer3_temp.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer3_temp.Get(topLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer3_temp.Get(topRightPos, MoveCostLayer.Dir_DiagonalNorthEast));
                path2 = MoveCost.Sum(thread.layer3_temp.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer3_temp.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer3_temp.Get(topRightPos, MoveCostLayer.Dir_DiagonalNorthEast));
                layer4.Set(lay4PosIx, MoveCostLayer.Dir_DiagonalNorthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                //Diagonal SE
                path1 = MoveCost.Sum(thread.layer3_temp.Get(topLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer3_temp.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer3_temp.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalSouthEast));
                path2 = MoveCost.Sum(thread.layer3_temp.Get(topLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer3_temp.Get(topRightPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer3_temp.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalSouthEast));
                layer4.Set(lay4PosIx, MoveCostLayer.Dir_DiagonalSouthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));
            }                   

            status = MoveCostLayer4.TileStatus_Initialized;
        }
    }

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
