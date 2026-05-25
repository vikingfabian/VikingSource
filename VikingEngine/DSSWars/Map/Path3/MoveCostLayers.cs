using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Path3;

namespace VikingEngine.DSSWars
{
    partial class WorldData
    {
        public Grid2D<MoveCost> layer0;
        public MoveCostLayer layer2;
        public MoveCostLayer4 layer4;

        ConcurrentStack<LayerPathFinding> poolPathLayer2 = new ConcurrentStack<LayerPathFinding>();
        ConcurrentStack<LayerPathFinding> poolPathLayer4 = new ConcurrentStack<LayerPathFinding>();

        ConcurrentQueue<LayerWalkingPath> poolLayerResult = new ConcurrentQueue<LayerWalkingPath>();

        void InitMoveCostLayers()
        {
            layer0 = new Grid2D<MoveCost>(subTileGrid.Size);
            layer2 = new MoveCostLayer(2, MoveCostLayer.Layer2TileWidth, subTileGrid.Size / MoveCostLayer.Layer2TileWidth);
            layer4 = new MoveCostLayer4(this);
        }

        public LayerPathFinding GetLayerPath(int layer)
        {
            if (layer == 2)
            {
                if (poolPathLayer2.TryPop(out LayerPathFinding path))
                {
                    return path;
                }
            }
            else
            {
                if (poolPathLayer4.TryPop(out LayerPathFinding path))
                {
                    return path;
                }
            }

            return new LayerPathFinding(layer == 2? layer2 : layer4);

        }

        public LayerWalkingPath GetLayerResult()
        {
            if (poolLayerResult.TryDequeue(out LayerWalkingPath path))
            {
                //if (path.timeStamp + 2 >= Ref.TotalFrameCount)
                //{
                //    poolRes.Enqueue(new LayerWalkingPath());
                //    poolRes.Enqueue(new LayerWalkingPath());
                //    System.Threading.Thread.Sleep(32);
                //}
                path.recycle();
                return path;
            }

            return new LayerWalkingPath();

        }

        public void Return(LayerPathFinding path)
        {
            // Reset the node to a default state
            if (path != null)
            {
                path.recycle();
                if (path.layer == 2)
                {
                    poolPathLayer2.Push(path);
                }
                else
                {
                    poolPathLayer4.Push(path);
                }
            }
        }

        public void Return(LayerWalkingPath pathresult)
        {
            // Reset the node to a default state
            if (pathresult != null)
            {
                //path.recycle();
                pathresult.timeStamp = Ref.TotalFrameCount;
                poolLayerResult.Enqueue(pathresult);
            }
        }

       
        //public void Test()
        //{
        //    Path3Thread thread = new Path3Thread();
        //    generateLayer4Tile(thread, new IntVector2(1, 0));
        //}

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





                    lay1pos.X += 2;
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
}
