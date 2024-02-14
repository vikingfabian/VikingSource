//using VikingEngine.EngineSpace.Maths;
//using VikingEngine.LootFest.Map.Terrain.Generation;
//using Microsoft.Xna.Framework;
//using System.Collections.Generic;
//using System.Linq;
//using System;

//namespace VikingEngine.LootFest.GO.Physics
//{
//    class RectDimMinCoordComparer : IComparer<Rectangle2>
//    {
//        /* Fields */
//        public Dimensions comparisonDimension;

//        /* Interface Methods */
//        public int Compare(Rectangle2 x, Rectangle2 y)
//        {
//            return x.GetMinCoordAlongDimension(comparisonDimension) - y.GetMinCoordAlongDimension(comparisonDimension);
//        }
//    }

//    class CompositeRectBounds
//    {
//        /* Fields */
//        KdTree2<LevelRoom> kdtree;
//        List<LevelRoom> roomList;

//        /* Constructors */
//        public CompositeRectBounds()
//        {
//            roomList = new List<LevelRoom>();
//        }

//        /* Novelty Methods */
//        public void AddRoomToKeepInside(LevelRoom room)
//        {
//            roomList.Add(room);
//        }

//        public void ConstructWallRectangles(Rectangle2 rect, List<Rectangle2> otherRects)
//        {
//            const int DIRS = 4;

//            // Create buckets for cardinal directions
//            List<Rectangle2>[] rectBuckets = new List<Rectangle2>[DIRS];
//            for (int i = 0; i != DIRS; ++i)
//            {
//                rectBuckets[i] = new List<Rectangle2>();
//            }

//            // Put rectangles in buckets
//            for (int i = 0; i < otherRects.Count; ++i)
//            {
//                Rectangle2 r = otherRects[i];
//                IntVector2 diff = r.Center - rect.Center;
//                int bucketIndex = (int)diff.MajorDirection;
//                rectBuckets[bucketIndex].Add(r);
//            }

//            // Construct result buffer
//            List<Rectangle2> walls = new List<Rectangle2>();

//            // Sort buckets and construct walls in all directions
//            RectDimMinCoordComparer comparer = new RectDimMinCoordComparer();
//            for (int i = 0; i < DIRS; ++i)
//            {
//                Dir4 direction = (Dir4)i;
//                Dimensions directionDimension = lib.Dir4ToDimensionsXY(direction);
//                Dimensions comparisonDimension = lib.GetPerpendicularDimensionXY(directionDimension);
//                comparer.comparisonDimension = comparisonDimension;

//                List<Rectangle2> bucket = rectBuckets[i];

//                if (bucket.Count > 0)
//                {
//                    bucket.Sort(comparer);

//                    int coordinate = rect.GetMinCoordAlongDimension(comparisonDimension);
//                    int maxCoordinate = rect.GetMaxCoordAlongDimension(comparisonDimension);
//                    for (int j = 0; j < bucket.Count; ++j)
//                    {
//                        Rectangle2 otherRect = bucket[j];
//                        int otherMin = otherRect.GetMinCoordAlongDimension(comparisonDimension);
//                        int otherMax = otherRect.GetMaxCoordAlongDimension(comparisonDimension);
//                        if (coordinate > otherMin ||
//                            maxCoordinate < otherMax)
//                        {
//                            // Ignore wall building if other rect goes outside of us. It is assumed that our intervals overlap somehow.
//                        }
//                        else
//                        {
//                            Rectangle2 newWall = new Rectangle2();
//                            newWall.SetDim(comparisonDimension, coordinate, otherMin - coordinate);

//                            const int WIDTH = 3;
//                            int pos = rect.GetEdgeCoordinate(direction) - (direction.RepresentsNegativeDirection() ? WIDTH : 0);
//                            newWall.SetDim(directionDimension, pos, WIDTH);

//                            walls.Add(newWall);
//                            coordinate = otherMax;
//                        }
//                    }


//                    if (maxCoordinate > bucket[bucket.Count - 1].GetMaxCoordAlongDimension(comparisonDimension))
//                    {
//                        Rectangle2 newWall = new Rectangle2();
//                        newWall.SetDim(comparisonDimension, coordinate, maxCoordinate - coordinate);

//                        const int WIDTH = 3;
//                        int pos = rect.GetEdgeCoordinate(direction) - (direction.RepresentsNegativeDirection() ? WIDTH : 0);
//                        newWall.SetDim(directionDimension, pos, WIDTH);

//                        walls.Add(newWall);
//                    }
//                }
//                else
//                {
//                    Rectangle2 newWall = new Rectangle2();
//                    int minDim = rect.GetMinCoordAlongDimension(comparisonDimension);
//                    int maxDim = rect.GetMaxCoordAlongDimension(comparisonDimension);
//                    newWall.SetDim(comparisonDimension, minDim, maxDim - minDim);

//                    const int WIDTH = 3;
//                    int pos = rect.GetEdgeCoordinate(direction) - (direction.RepresentsNegativeDirection() ? WIDTH : 0);
//                    newWall.SetDim(directionDimension, pos, WIDTH);

//                    walls.Add(newWall);
//                }
//            }
//            comparer = null;
//        }

//        public void CommitAndConstruct()
//        {
//            List<KdNode2<LevelRoom>> nodesToAdd = new List<KdNode2<LevelRoom>>();

//            for (int i = 0; i < roomList.Count; ++i)
//            {
//                //LevelRoom room = roomList[i];
//                //var rooms = room.GetConnectingRooms();
//                //List<Rectangle2> rects = new List<Rectangle2>();
//                //foreach (var otherRoom in rooms)
//                //{
//                //    rects.Add(otherRoom.WorldRect);
//                //}
//                //ConstructWallRectangles(room.WorldRect, rects);

//                LevelRoom room = roomList[i];
//                Rectangle2 rect = room.WorldRect;
//                ForXYEdgeLoop loop = new ForXYEdgeLoop(rect);
//                while (loop.Next())
//                {
//                    nodesToAdd.Add(new KdNode2<LevelRoom>(room, loop.Position.Vec));
//                }
//            }

//            kdtree = new KdTree2<LevelRoom>(nodesToAdd.ToArray());
//        }

//        public void Clear()
//        {
//            roomList.Clear();
//            kdtree = null;
//        }

//        public float MinDistance(Vector2 position, INorm norm, out Vector2 closestPosition, out LevelRoom closestRoom)
//        {
//            float distance;
//            if (kdtree == null)
//            {
//                CommitAndConstruct();
//            }
//            KdNode2<LevelRoom> closestNode = kdtree.NearestNeighborSearch(position - new Vector2(0.5f), norm, out distance);
//            closestRoom = closestNode.data;

//            VectorRect rect = new VectorRect(closestRoom.WorldRect);

//            return rect.ClosestDistance(position, norm, out closestPosition);
//        }

//        Vector2 FindMovement(Vector2 check)
//        {
//            Vector2 closestPos;
//            LevelRoom nvm;
//            MinDistance(check, new EuclideanSquaredNorm(), out closestPos, out nvm);
//            return closestPos - check;
//        }

//        public void AdjustGameObject(AbsVoxelObj obj)
//        {
//            // Get the character's collision rectangle.
//            Vector2 charPlaneSize = new Vector2(obj.CollisionAndDefaultBound.MainBound.outerBound.HalfSize.X * 2,
//                                                obj.CollisionAndDefaultBound.MainBound.outerBound.HalfSize.Z * 2);

//            // Add necessary movement for each corner.
//            Vector2 move  = FindMovement(obj.PlanePos);
//                    move += FindMovement(obj.PlanePos + new Vector2(charPlaneSize.X, 0));
//                    move += FindMovement(obj.PlanePos + new Vector2(0, charPlaneSize.Y));
//                    move += FindMovement(obj.PlanePos + charPlaneSize);

//            // Apply movement vector.
//            obj.Position = obj.Position + new Vector3(move.X, 0, move.Y);
//        }
//    }
//}
