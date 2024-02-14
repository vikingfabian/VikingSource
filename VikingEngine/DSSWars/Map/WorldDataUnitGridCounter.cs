//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VikingEngine.DSSWars.GameObject;

//namespace VikingEngine.DSSWars.Map
//{
//    class WorldDataUnitGridCounter
//    {
//        SpottedArrayCounter<AbsMapObject> counter;
//        //int listIndex = 0;
//        ForXYLoop loop;
//        bool reachedEnd = false;
//        public AbsMapObject CurrentUnit;

//        public WorldDataUnitGridCounter(IntVector2 position)
//        {
//            Reset(position);
//        }

//        public void Reset(IntVector2 worldTilesPos)
//        {
//            if (DssRef.world.tileBounds.IntersectPoint(worldTilesPos))
//            {
//                IntVector2 gridPos = DssRef.world.squareToUnitGridPos(worldTilesPos);
//                Rectangle2 area = new Rectangle2(gridPos, 1);
               
//                area.SetBounds(DssRef.world.UnitGridAreaLimit);
//                loop = new ForXYLoop(area.pos, area.BottomRight);
//                //loop.Next();
//                counter = new SpottedArrayCounter<AbsMapObject>(DssRef.world.unitGrid[loop.Position.X, loop.Position.Y]);

//                nextUnitList();
//            }
//            else
//            {
//                reachedEnd = true;
//            }
//        }

//        /// <summary>
//        /// Should be in a while() loop
//        /// </summary>
//        public bool NextUnit()
//        {
//            if (reachedEnd)
//            {
//                return false;
//            }
//            else
//            {
//                while (true)
//                {
//                    if (counter.Next())
//                    {
//                        CurrentUnit = counter.sel;
//                        return true;
//                    }
//                    else
//                    {
//                        nextUnitList();
//                        if (reachedEnd)
//                        {
//                            return false;
//                        }
//                    }
//                }
//            }
//            //return !reachedEnd;
//        }

//        void nextUnitList()
//        {
//            while (true)
//            {
//                if (loop.Next())
//                {
//                    if (loop.Position.Y == DssRef.world.unitGrid.GetLength(1) - 1)
//                    {
//                        lib.DoNothing();
//                    }
//                    counter.Reset(DssRef.world.unitGrid[loop.Position.X, loop.Position.Y]);
//                    if (counter.array.Count > 0)
//                    {
//                        return;
//                    }
//                }
//                else
//                {
//                    reachedEnd = true;
//                    return;
//                }
//            }
//        }
//    }
//}
