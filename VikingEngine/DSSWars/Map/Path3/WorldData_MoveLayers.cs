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
        public Grid2D_L<MoveCost> moveLayer0;
        public MoveCostLayer moveLayer2;
        public MoveCostLayer4 moveLayer4;

        
        public void InitMoveCostLayers()
        {
            moveLayer0 = new Grid2D_L<MoveCost>(subTileGrid.Size);
            moveLayer2 = new MoveCostLayer(2, MoveCostLayer.Layer2TileWidth, subTileGrid.Size / MoveCostLayer.Layer2TileWidth);
            moveLayer4 = new MoveCostLayer4(this);
        }

        public float TerrainSpeedMultiplier(IntVector2 mapTilePos, bool isShip)
        {
            var movecost = moveLayer0.Get(mapTilePos);
            return 1f / (isShip? movecost.water : movecost.land);
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

            return new LayerPathFinding(layer == 2? moveLayer2 : moveLayer4);

        }

        public void checkNeedUpdate(Path3Thread thread, IntVector2 lay4tile_start, IntVector2 lay4tile_end)
        {
            if (moveLayer4.tileStatus.Get(lay4tile_start) <= MoveCostLayer4.TileStatus_NeedUpdate ||
                moveLayer4.tileStatus.Get(lay4tile_end) <= MoveCostLayer4.TileStatus_NeedUpdate)
            {
                Rectangle2 refreshArea = Rectangle2.FromCenterTileAndRadius(lay4tile_start, 2);
                refreshArea.includeTileAndRadius(lay4tile_end, 1);
                refreshArea.SetTileBounds(moveLayer4.tileStatus.Area);

                ForXYLoop loop = new ForXYLoop(refreshArea);
                while (loop.Next())
                {
                    generateLayer4Tile(thread, loop.Position);
                }
            }
        }

        void generateLayer4Tile(Path3Thread thread, IntVector2 lay4tile)
        {
            ref var status = ref moveLayer4.tileStatus.GetRef(lay4tile);
            if (status <= MoveCostLayer4.TileStatus_NeedUpdate)
            {
                status = MoveCostLayer4.TileStatus_Updateing;
            }
            else
            {
                return;
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
                    MoveCost topLeft = DssRef.world.subTileGrid.Get(pos.X, pos.Y).GetMoveCost();
                    moveLayer0.Set(pos.X, pos.Y, topLeft);
                    MoveCost topRight = DssRef.world.subTileGrid.Get(pos.X + 1, pos.Y).GetMoveCost();
                    moveLayer0.Set(pos.X + 1, pos.Y , topRight);
                    MoveCost bottomLeft = DssRef.world.subTileGrid.Get(pos.X, pos.Y + 1).GetMoveCost();
                    moveLayer0.Set(pos.X, pos.Y + 1 , bottomLeft);
                    MoveCost bottomRight = DssRef.world.subTileGrid.Get(pos.X + 1, pos.Y + 1).GetMoveCost();
                    moveLayer0.Set(pos.X + 1, pos.Y + 1, bottomRight);

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
                    int lay2PosIx = moveLayer2.GetPositionStart(pos);

                    int topLeftPos = thread.layer1_temp.GetPositionStart(lay1pos);
                    int topRightPos = thread.layer1_temp.GetPositionStart(lay1pos.ReturnSum(1, 0));
                    int bottomLeftPos = thread.layer1_temp.GetPositionStart(lay1pos.ReturnSum(0, 1));
                    int bottomRightPos = thread.layer1_temp.GetPositionStart(lay1pos.ReturnSum(1, 1));


                    //Horizontal
                    path1 = MoveCost.Sum(thread.layer1_temp.Get(topLeftPos, MoveCostLayer.Dir_WestToEast), thread.layer1_temp.Get(topRightPos, MoveCostLayer.Dir_WestToEast));
                    path2 = MoveCost.Sum(thread.layer1_temp.Get(bottomLeftPos, MoveCostLayer.Dir_WestToEast), thread.layer1_temp.Get(bottomRightPos, MoveCostLayer.Dir_WestToEast));
                    moveLayer2.Set(lay2PosIx, MoveCostLayer.Dir_WestToEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Vertical
                    path1 = MoveCost.Sum(thread.layer1_temp.Get(topLeftPos, MoveCostLayer.Dir_NorthToSouth), thread.layer1_temp.Get(bottomLeftPos, MoveCostLayer.Dir_NorthToSouth));
                    path2 = MoveCost.Sum(thread.layer1_temp.Get(topRightPos, MoveCostLayer.Dir_NorthToSouth), thread.layer1_temp.Get(bottomRightPos, MoveCostLayer.Dir_NorthToSouth));
                    moveLayer2.Set(lay2PosIx, MoveCostLayer.Dir_NorthToSouth, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Diagonal NE
                    path1 = MoveCost.Sum(thread.layer1_temp.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer1_temp.Get(topLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer1_temp.Get(topRightPos, MoveCostLayer.Dir_DiagonalNorthEast));
                    path2 = MoveCost.Sum(thread.layer1_temp.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer1_temp.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer1_temp.Get(topRightPos, MoveCostLayer.Dir_DiagonalNorthEast));
                    moveLayer2.Set(lay2PosIx, MoveCostLayer.Dir_DiagonalNorthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Diagonal SE
                    path1 = MoveCost.Sum(thread.layer1_temp.Get(topLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer1_temp.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer1_temp.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalSouthEast));
                    path2 = MoveCost.Sum(thread.layer1_temp.Get(topLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer1_temp.Get(topRightPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer1_temp.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalSouthEast));
                    moveLayer2.Set(lay2PosIx, MoveCostLayer.Dir_DiagonalSouthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));


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

                    int topLeftPos = moveLayer2.GetPositionStart(lay2Pos);
                    int topRightPos = moveLayer2.GetPositionStart(lay2Pos.ReturnSum(1, 0));
                    int bottomLeftPos = moveLayer2.GetPositionStart(lay2Pos.ReturnSum(0, 1));
                    int bottomRightPos = moveLayer2.GetPositionStart(lay2Pos.ReturnSum(1, 1));

                    //Horizontal
                    path1 = MoveCost.Sum(moveLayer2.Get(topLeftPos, MoveCostLayer.Dir_WestToEast), moveLayer2.Get(topRightPos, MoveCostLayer.Dir_WestToEast));
                    path2 = MoveCost.Sum(moveLayer2.Get(bottomLeftPos, MoveCostLayer.Dir_WestToEast), moveLayer2.Get(bottomRightPos, MoveCostLayer.Dir_WestToEast));
                    thread.layer3_temp.Set(lay3PosIx, MoveCostLayer.Dir_WestToEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Vertical
                    path1 = MoveCost.Sum(moveLayer2.Get(topLeftPos, MoveCostLayer.Dir_NorthToSouth), moveLayer2.Get(bottomLeftPos, MoveCostLayer.Dir_NorthToSouth));
                    path2 = MoveCost.Sum(moveLayer2.Get(topRightPos, MoveCostLayer.Dir_NorthToSouth), moveLayer2.Get(bottomRightPos, MoveCostLayer.Dir_NorthToSouth));
                    thread.layer3_temp.Set(lay3PosIx, MoveCostLayer.Dir_NorthToSouth, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Diagonal NE
                    path1 = MoveCost.Sum(moveLayer2.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), moveLayer2.Get(topLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), moveLayer2.Get(topRightPos, MoveCostLayer.Dir_DiagonalNorthEast));
                    path2 = MoveCost.Sum(moveLayer2.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), moveLayer2.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalNorthEast), moveLayer2.Get(topRightPos, MoveCostLayer.Dir_DiagonalNorthEast));
                    thread.layer3_temp.Set(lay3PosIx, MoveCostLayer.Dir_DiagonalNorthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                    //Diagonal SE
                    path1 = MoveCost.Sum(moveLayer2.Get(topLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), moveLayer2.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), moveLayer2.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalSouthEast));
                    path2 = MoveCost.Sum(moveLayer2.Get(topLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), moveLayer2.Get(topRightPos, MoveCostLayer.Dir_DiagonalSouthEast), moveLayer2.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalSouthEast));
                    thread.layer3_temp.Set(lay3PosIx, MoveCostLayer.Dir_DiagonalSouthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                }
            }

            //Layer 4
            expensivePathAdd = 0.03f;
            cheapPathAdd = 0.97f;

            {
                int lay4PosIx = moveLayer4.GetPositionStart(lay4tile);

                int topLeftPos = thread.layer3_temp.GetPositionStart(new IntVector2(0, 0));
                int topRightPos = thread.layer3_temp.GetPositionStart(new IntVector2(1, 0));
                int bottomLeftPos = thread.layer3_temp.GetPositionStart(new IntVector2(0, 1));
                int bottomRightPos = thread.layer3_temp.GetPositionStart(new IntVector2(1, 1));

                //Horizontal
                path1 = MoveCost.Sum(thread.layer3_temp.Get(topLeftPos, MoveCostLayer.Dir_WestToEast), thread.layer3_temp.Get(topRightPos, MoveCostLayer.Dir_WestToEast));
                path2 = MoveCost.Sum(thread.layer3_temp.Get(bottomLeftPos, MoveCostLayer.Dir_WestToEast), thread.layer3_temp.Get(bottomRightPos, MoveCostLayer.Dir_WestToEast));
                moveLayer4.Set(lay4PosIx, MoveCostLayer.Dir_WestToEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                //Vertical
                path1 = MoveCost.Sum(thread.layer3_temp.Get(topLeftPos, MoveCostLayer.Dir_NorthToSouth), thread.layer3_temp.Get(bottomLeftPos, MoveCostLayer.Dir_NorthToSouth));
                path2 = MoveCost.Sum(thread.layer3_temp.Get(topRightPos, MoveCostLayer.Dir_NorthToSouth), thread.layer3_temp.Get(bottomRightPos, MoveCostLayer.Dir_NorthToSouth));
                moveLayer4.Set(lay4PosIx, MoveCostLayer.Dir_NorthToSouth, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                //Diagonal NE
                path1 = MoveCost.Sum(thread.layer3_temp.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer3_temp.Get(topLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer3_temp.Get(topRightPos, MoveCostLayer.Dir_DiagonalNorthEast));
                path2 = MoveCost.Sum(thread.layer3_temp.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer3_temp.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalNorthEast), thread.layer3_temp.Get(topRightPos, MoveCostLayer.Dir_DiagonalNorthEast));
                moveLayer4.Set(lay4PosIx, MoveCostLayer.Dir_DiagonalNorthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));

                //Diagonal SE
                path1 = MoveCost.Sum(thread.layer3_temp.Get(topLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer3_temp.Get(bottomLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer3_temp.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalSouthEast));
                path2 = MoveCost.Sum(thread.layer3_temp.Get(topLeftPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer3_temp.Get(topRightPos, MoveCostLayer.Dir_DiagonalSouthEast), thread.layer3_temp.Get(bottomRightPos, MoveCostLayer.Dir_DiagonalSouthEast));
                moveLayer4.Set(lay4PosIx, MoveCostLayer.Dir_DiagonalSouthEast, MoveCost.Total(ref path1, ref path2, cheapPathAdd, expensivePathAdd));
            }

            status = MoveCostLayer4.TileStatus_Initialized;
        }
    }
}
