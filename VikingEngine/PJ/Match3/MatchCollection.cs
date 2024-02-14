using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Match3
{
    class MatchCollection
    {
        static readonly IntVector2[] CheckDirs = new IntVector2[]
            {
            new IntVector2(1, -1),
            new IntVector2(1, 0),
            new IntVector2(1, 1),
            new IntVector2(0, 1)
            };

        public List<Match> matches = new List<Match>();
        

        public MatchCollection(BrickBox box)
        {
            List<Brick> matchgroup = new List<Brick>(16);

            box.grid.LoopBegin();
            while (box.grid.LoopNext())
            {
                Brick startBrick = box.grid.LoopValueGet();

                if (startBrick != null)
                {
                    for (int i = 0; i < CheckDirs.Length; ++i)
                    {
                        matchgroup.Clear();
                        matchgroup.Add(startBrick);

                        IntVector2 dir = CheckDirs[i];
                        IntVector2 pos = box.grid.LoopPosition;
                        bool gotMatch;

                        do
                        {
                            gotMatch = false;
                            pos += dir;
                            Brick nextBrick;
                            if (box.grid.TryGet(pos, out nextBrick))
                            {
                                if (nextBrick != null)
                                {
                                    gotMatch = colorMatch(nextBrick, startBrick);
                                    if (gotMatch)
                                    {
                                        matchgroup.Add(nextBrick);
                                    }
                                }
                            }
                        }
                        while (gotMatch);

                        if (matchgroup.Count >= 3)
                        {
                            var match = new Match(matchgroup, i);

                            if (findDuplicate(match) == false)
                            {
                                matches.Add(match);
                            }
                        }
                    }
                }
            }
        }

        bool colorMatch(Brick b1, Brick b2)
        {
            return b1.color != BrickColor.Stone &&
                b1.color == b2.color;
        }

        public void orderLengthLowToHigh()
        {
            if (matches.Count > 1)
            {
                var array = matches.ToArray();
                arraylib.Quicksort(array, true);
                matches = new List<Match>(array);
            }
        }

        public bool findDuplicate(Match match)
        {
            foreach (var m in matches)
            {
                if (m.isDuplicate(match))
                {
                    m.merge(match);
                    return true;
                }
            }
            return false;
        }

        public bool GotMatch { get { return matches.Count > 0; } }

        //public void removeGroups()
        //{
        //    foreach (var g in matches)
        //    {
        //        foreach (var b in g.bricks)
        //        {
        //            b.DeleteMe();
        //        }
        //    }
        //}
    }

    class Match : IComparable
    {
        public List<Brick> bricks;
        public int dir;

        public Match(List<Brick> matchgroup, int dir)
        {
            bricks = new List<Brick>(matchgroup);
            this.dir = dir;
        }

        public bool isDuplicate(Match match)
        {
            if (this.dir == match.dir)
            {
                foreach (var myBrick in this.bricks)
                {
                    foreach (var otherBrick in match.bricks)
                    {
                        if (myBrick.gridPos == otherBrick.gridPos)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public int SharedBricks(Match other)
        {
            int shared = 0;

            foreach (var myBrick in this.bricks)
            {
                foreach (var otherBrick in other.bricks)
                {
                    if (myBrick == otherBrick)
                    {
                        shared++;
                    }
                }
            }

            return shared;
        }

        public void merge(Match other)
        {
            foreach (var myBrick in this.bricks)
            {
                for (int i = 0; i < other.bricks.Count; ++i)
                {
                    if (myBrick.gridPos == other.bricks[i].gridPos)
                    {
                        other.bricks.RemoveAt(i);
                        break;
                    }
                }
            }

            bricks.AddRange(other.bricks);
        }

        public Vector2 center()
        {
            Vector2 result = Vector2.Zero;
            foreach (var m in bricks)
            {
                result += m.images.ParentPosition;
            }

            return result / bricks.Count;
        }

        public int CompareTo(object obj)
        {
            return bricks.Count - ((Match)obj).bricks.Count;
        }
            
    }
}
