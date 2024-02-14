using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf.GO
{
    class ObjectCollection
    {
        public LaunchCannon cannon;

        public List<FieldEdge> edges = new List<FieldEdge>();
        public List<FieldEdgeCorner> edgeCorners = new List<FieldEdgeCorner>();
        
        public List<AbsItem> items = new List<AbsItem>(32);
        public List<AbsItem> flagPoints = new List<AbsItem>();
        public List<Ball> balls = new List<Ball>();

        public SpottedArray<AbsItem> updateList;
        public SpottedArrayCounter<AbsItem> updateCounter;
        public SpottedArrayCounter<AbsItem> updateAsynchCounter;

        Time hideCannonTime = 0;

        public ObjectCollection()
        {
            GolfRef.objects = this;

            updateList = new SpottedArray<AbsItem>(2);
            updateCounter = new SpottedArrayCounter<AbsItem>(updateList);
            updateAsynchCounter = new SpottedArrayCounter<AbsItem>(updateList);
        }

        public void Add(Ball ball)
        {
            balls.Add(ball);
        }

        public void Remove(Ball ball)
        {
            balls.Remove(ball);
        }

        public void Add(AbsItem item)
        {
            items.Add(item);
            if (item is FlagPoint)
            {
                flagPoints.Add(item);
            }
        }

        public void AddToUpdate(AbsItem item)
        {
            if (PlatformSettings.DevBuild)
            {
                if (updateList.Contains(item))
                {
                    throw new Exception();
                }
            }
            updateList.Add(item);
            item.inUpdateList = true;
        }

        public void RemoveFromUpdate(AbsItem item)
        {
            updateList.Remove(item);
            item.inUpdateList = false;
        }

        public void Remove(AbsItem item)
        {
            items.Remove(item);

            if (item is FlagPoint)
            {
                flagPoints.Remove(item);

                if (flagPoints.Count == 1)
                {
                    //Convert the last flag to the final hole
                    new Hole(flagPoints[0].image.Position);
                    flagPoints[0].DeleteMe();
                }
            }

            if (item.inUpdateList &&
                item.Type != ObjectType.ObsticleBug)
            {
                updateList.Remove(item);
            } 
        }

        public void createEdgeCorners()
        {
            var lineEndings = new Grid2D<int>(GolfRef.field.cornersCount);

            foreach (var m in edges)
            {
                {
                    int count = lineEndings.Get(m.from) + 1;
                    lineEndings.Set(m.from, count);
                }
                {
                    int count = lineEndings.Get(m.to) + 1;
                    lineEndings.Set(m.to, count);
                }
            }

            lineEndings.LoopBegin();
            while (lineEndings.LoopNext())
            {
                if (lineEndings.LoopValueGet() >= 2)
                {
                    new FieldEdgeCorner(lineEndings.LoopPosition);
                }
            }

        }

        public void debugHole()
        {
            while (flagPoints.Count > 0)
            {
                flagPoints[0].DeleteMe();
            }
        }

        public void update(bool inCannon)
        {
            if (cannon != null)
            {
                if (inCannon)
                {
                    hideCannonTime.CountDown();
                    cannon.update();
                }
                else
                {
                    cannon.DeleteMe();
                    cannon = null;
                }
            }

            updateCounter.Reset();
            while (updateCounter.Next())
            {
                updateCounter.sel.update();
            }
        }

        public void update_asynch()
        {
            updateAsynchCounter.Reset();
            while (updateAsynchCounter.Next())
            {
                updateAsynchCounter.sel.update_asynch();
            }

            var _cannon = cannon;
            if (_cannon != null && hideCannonTime.TimeOut)
            {
                Physics.CircleBound cBound = new Physics.CircleBound(
                    _cannon.image.Position, GolfRef.gamestate.BallScale * 0.7f);

                float opacity = 1f;

                for (int i = 0; i < balls.Count; ++i)
                {
                    if (balls[i].isIdle && balls[i].bound.Intersect2(cBound).IsCollision)
                    {
                        hideCannonTime.MilliSeconds = 600;
                        var result = balls[i].bound.Intersect2(cBound);
                        opacity = 0.3f;
                        break;
                    }
                }

                _cannon.image.Opacity = opacity;
            }
        }

        public void ClearItems()
        {
            arraylib.DeleteAndClearArray(items);
        }

        public void clearMap()
        {
            arraylib.DeleteAndClearArray(edges);
            arraylib.DeleteAndClearArray(edgeCorners);

            ClearItems();
            cannon?.image.DeleteMe();
        }
    }
}
