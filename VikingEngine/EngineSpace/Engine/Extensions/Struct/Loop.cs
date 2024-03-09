using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    struct ForXYZLoop
    {
        IntervalIntV3 volume;
        IntVector3 position;
        public IntVector3 Position;

        bool done;
        public bool Done
        { get { return done; } }

        public ForXYZLoop(IntVector3 size)
            : this(new IntervalIntV3(IntVector3.Zero, size - 1))
        { }
        public ForXYZLoop(IntVector3 min, IntVector3 max)
            : this(new IntervalIntV3(min, max))
        { }
        public ForXYZLoop(IntervalIntV3 volume)
        {
            this.volume =volume;
            position = volume.Min;
            Position = position;
            done = false;
        }
        public bool AtEdge
        {
            get
            {

                return Position.X == volume.Min.X || Position.X == volume.Max.X ||
                    Position.Y == volume.Min.Y || Position.Y == volume.Max.Y;
            }
        }

        public IntVector3 Next_Old()
        {
            IntVector3 result = position;
            position.X++;
            if (position.X > volume.Max.X)
            {
                position.X = volume.Min.X;
                position.Y++;
                if (position.Y > volume.Max.Y)
                {
                    position.Y = volume.Min.Y;
                    position.Z++;
                    if (position.Z > volume.Max.Z)
                    {
                        done = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// run as while(Next())
        /// </summary>
        public bool Next()
        {
            bool result = !done;

            Position = position;
            ++position.X;
            if (position.X > volume.Max.X)
            {
                position.X = volume.Min.X;
                ++position.Y;
                if (position.Y > volume.Max.Y)
                {
                    position.Y = volume.Min.Y;
                    ++position.Z;
                    if (position.Z > volume.Max.Z)
                    {
                        done = true;
                    }
                }
            }

            return result;
        }
    }

    struct ForXYEdgeLoop
    {
        Rectangle2 area;
        public IntVector2 Position;
        IntVector2 start;
        bool loopHasStarted;

        public ForXYEdgeLoop(IntVector2 size)
            : this(new Rectangle2(size))
        { }
        public ForXYEdgeLoop(IntVector2 min, IntVector2 max)
            : this(new Rectangle2(min, (max - min) + 1))
        { }

        public ForXYEdgeLoop(Rectangle2 area)
            :this()
        {
            this.area = area;
            Reset();
        }

        int stepDir;
        int stepsLeft;
        public void Reset()
        {
            Position = area.pos;
            Position.X -= 1;
            stepDir = 0;
            stepsLeft = area.Width;
            loopHasStarted = false;
        }
        
        static readonly IntVector2[] stepDirOrder = new IntVector2[]
        {
            new IntVector2( 1, 0 ),
            new IntVector2( 0, 1 ),
            new IntVector2( -1, 0),
            new IntVector2( 0, -1),
            new IntVector2( 0, 0 ),
        };

        /// <summary>
        /// run as while(Next())
        /// </summary>
        public bool Next()
        {
            Position += stepDirOrder[stepDir];//IntVector2.From4Dirs[(int)stepDir];
            if (--stepsLeft <= 0)
            {
                if (++stepDir >= 4)
                {
                    stepDir = 0;
                }
                stepsLeft = ((stepDir == 1 || stepDir ==  3)?  area.Height : area.Width) - 1;
            }

            if (!loopHasStarted)
            {
                start = Position;
                loopHasStarted =true;
                return true;   
            }

            return Position != start;
        }

        public void RandomPosition(bool prepareLoop)
        {
            RandomPosition(Ref.rnd, prepareLoop);   
        }

        public void RandomPosition(PcgRandom rnd, bool prepareLoop)
        {
            //Pick random corner, then random steps on the edge
            stepDir = rnd.Int(4);
            stepsLeft = (stepDir == 1 || stepDir == 3) ? area.Height : area.Width;

            int moveAlongEdge = rnd.Int(stepsLeft - 1);
            stepsLeft -= moveAlongEdge;
            if (prepareLoop)
            {
                moveAlongEdge -= 1;
            }
            else
            { 
                stepsLeft -= 1;
            }
            Position = area.GetTileCorner((Corner)stepDir) + stepDirOrder[stepDir] * moveAlongEdge;

        }

        public void ExpandRadius()
        {
            area.AddRadius(1);
            Reset();
        }

        public bool AtRight
        {
            get { return Position.X == area.RightTile; }
        }
        public bool AtBottom
        {
            get { return Position.Y == area.BottomTile; }
        }

        public bool AtXEdge
        {
            get { return Position.X == area.X || Position.X == area.RightTile; }
        }
        public bool AtYEdge
        {
            get { return Position.Y == area.Y || Position.Y == area.BottomTile; }
        }

        public bool AtCenterX
        {
            get { return Position.X ==  area.CenterTileX; }
        }
        public bool AtCenterY
        {
            get { return Position.Y == area.CenterTileY; }
        }

        public Dir4 AtEdgeDir
        {
            get
            {
                if (AtXEdge)
                {
                    if (AtYEdge)
                    {
                        return Dir4.NUM_NON;
                    }
                    return Position.X == area.X ? Dir4.W : Dir4.E;
                }
                else
                {
                    return Position.Y == area.Y ? Dir4.N : Dir4.S;
                }
            }
        }
    }

    

    struct FrameAroundIntV2
    {
        public IntVector2 center;
        IntVector2 position;
        public IntVector2 Position
        { get { return position; } }
        byte part;
        int radius;

        public FrameAroundIntV2(IntVector2 center, int radius)
        {
            part = 0;
            this.center = center;
            this.radius = radius;
            position = center - radius;
            position.X -= 1;
        }

        public bool Next()
        {
            if (radius == 0)
            {
                position = center;
                return part++ == 0;
            }

            switch (part)
            {
                case 0:
                    position.X++;
                    if (position.X > center.X + radius)
                    {
                        part++;
                        position = new IntVector2(center.X + radius, center.Y - radius + 1);
                    }
                    break;
                case 2:
                    position.X--;
                    if (position.X < center.X - radius)
                    {
                        part++;
                        position = new IntVector2(center.X - radius, center.Y + radius - 1);
                    }
                    break;

                case 1:
                    position.Y++;
                    if (position.Y > center.Y + radius)
                    {
                        part++;
                        position = new IntVector2(center.X + radius - 1, center.Y + radius);
                    }
                    break;
                case 3:
                    position.Y--;
                    if (position.Y <= center.Y - radius)
                    {
                        return false;
                    }
                    break;
            }
            
            return true;
        }
    }

    struct TwoDirLoop
    {
        public int dir;
        
        public void Reset()
        {
            dir = 0;
        }
        
        /// <summary>
        /// run as while(Next())
        /// </summary>
        public bool Next()
        {
            switch (dir)
            {
                case 0:
                    dir = -1;
                    break;
                case -1:
                    dir = 1;
                    break;
                case 1:
                    return false;

                default:
                    throw new Exception();
            }

            return true;
        }
    }
}
