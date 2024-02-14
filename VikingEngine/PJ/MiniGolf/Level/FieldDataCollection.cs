using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    class FieldDataCollection
    {
        public bool movingCannon;
        public Vector2 cannonPos1, cannonPos2;
        public Rotation1D cannonDir;

        public Dictionary<int, FieldArea> areas = new Dictionary<int, FieldArea>();
        public Dictionary<int, Choke> chokes = new Dictionary<int, Choke>();

        public FieldDataCollection()
        {
            List<IntVector2> cannonPos = new List<IntVector2>(4);

            GolfRef.field.squares.LoopBegin();
            while (GolfRef.field.squares.LoopNext())
            {
                var sq = GolfRef.field.squares.LoopValueGet();

                switch (sq.type)
                {
                    case FieldSquareType.Area:
                        FieldArea area;
                        if (areas.TryGetValue(sq.typeIndex, out area) == false)
                        {
                            area = new FieldArea();
                            areas.Add(sq.typeIndex, area);
                        }                
                        area.squares.Add(GolfRef.field.squares.LoopPosition);
                        break;
                    case FieldSquareType.ChokeStart:
                        addChoke(true, GolfRef.field.squares.LoopPosition, sq.typeIndex);
                        break;
                    case FieldSquareType.ChokeEnd:
                        addChoke(false, GolfRef.field.squares.LoopPosition, sq.typeIndex);
                        break;

                    case FieldSquareType.LaunchCannon:
                        cannonPos.Add(GolfRef.field.squares.LoopPosition);
                        break;
                }
            }

            cannon(cannonPos);
        }

        void addChoke(bool start, IntVector2 pos, int ix)
        {
            Choke chk;

            if (ix == 16)
            {
                lib.DoNothing();
            }

            if (chokes.TryGetValue(ix, out chk) == false)
            {
                chk = new Choke();
                chokes.Add(ix, chk);
            }

            if (start)
            {
                chk.start = pos;
            }
            else
            {
                chk.end = pos;
            }
        }

        void cannon(List<IntVector2> pos)
        {
            if (pos.Count == 0)
            {
                movingCannon = false;

                cannonPos1 = GolfRef.field.toSquareScreenArea(IntVector2.Zero).Center;
            }
            else if (pos.Count == 2 && pos[0].SideLength(pos[1]) > 1)
            {
                movingCannon = true;

                cannonPos1 = GolfRef.field.toSquareScreenArea(pos[0]).Center;
                cannonPos2 = GolfRef.field.toSquareScreenArea(pos[1]).Center;
            }
            else
            {
                movingCannon = false;

                Vector2 sum = Vector2.Zero;
                foreach (var m in pos)
                {
                    sum += GolfRef.field.toSquareScreenArea(m).Center;
                }
                cannonPos1 = sum / pos.Count;
            }

            cannonDir = new Rotation1D(GolfRef.field.launchAngle);
        }
    }

    class FieldArea
    {
        public List<IntVector2> squares = new List<IntVector2>();

        public Vector2 RandomSquareWp()
        {
            return GolfRef.field.toSquareScreenArea(RandomSquare()).Center;
        }

        public IntVector2 RandomSquare()
        {
            return arraylib.RandomListMemberPop(squares);
        }

        public bool HasSquares => squares.Count > 0;

        public bool randomArea(IntVector2 sz, bool pull, out Rectangle2 area)
        {
            const int Trials = 10;
            area = new Rectangle2(IntVector2.Zero, sz);

            if (squares.Count > 1)
            {
                for (int i = 0; i < Trials; i++)
                {
                    area.pos = arraylib.RandomListMember(squares);
                    ForXYLoop loop = new ForXYLoop(area);
                    bool available = true;

                    while (loop.Next())
                    {
                        if (!squares.Contains(loop.Position))
                        {
                            available = false;
                            break;
                        }
                    }

                    if (available)
                    {
                        if (pull)
                        {
                            loop.Reset();
                            while (loop.Next())
                            {
                                squares.Remove(loop.Position);
                            }
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        public int randomItemCount(int min, int max)
        {
            int result = Ref.rnd.Int(min, max + 1);
            if (result > squares.Count)
            {
                result = squares.Count;
            }

            return result;
        }
    }

    class Choke
    {
        public IntVector2 start= IntVector2.NegativeOne, end = IntVector2.NegativeOne;

        public List<IntVector2> squares()
        {
            List<IntVector2> result = new List<IntVector2>(8);

            if (start.X < 0)
            {
                //Failed choke
                result.Add(end);
            }
            else if (end.X < 0)
            {
                //Failed choke
                result.Add(start);
            }
            else
            {
                IntVector2 pos = start;
                result.Add(pos);
                int loops = 0;

                do
                {
                    IntVector2 diff = end - pos;
                    if (Math.Abs(diff.X) == Math.Abs(diff.Y))
                    {
                        pos.X += lib.ToLeftRight(diff.X);
                        pos.Y += lib.ToLeftRight(diff.Y);
                    }
                    else if (Math.Abs(diff.X) > Math.Abs(diff.Y))
                    {
                        pos.X += lib.ToLeftRight(diff.X);
                    }
                    else
                    {
                        pos.Y += lib.ToLeftRight(diff.Y);
                    }

                    result.Add(pos);

                    if (++loops > 100)
                    {
                        throw new EndlessLoopException("Choke squares()", 0);
                    }

                } while (pos != end);
            }

            return result;
        }
    }
}
