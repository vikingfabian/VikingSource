using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Physics;
using VikingEngine.EngineSpace.Maths;


namespace VikingEngine //AreaVolyme
{
    public enum Alignment
    {
        Left,
        Center,
        Right
    }

    struct BorderedSquare
    {
        public int sideWidth;
        public int borderWidth;

        public BorderedSquare(int sideWidth, int borderWidth)
        {
            this.sideWidth = sideWidth;
            this.borderWidth = borderWidth;
        }

        public IntVector2 CalculateSize()
        {
            return new IntVector2(sideWidth + 2 * borderWidth);
        }

        public bool CheckWithinBorders(IntVector2 position)
        {
            return (position.X > borderWidth && position.X <= borderWidth + sideWidth) &&
                   (position.Y > borderWidth && position.Y <= borderWidth + sideWidth);
        }
    }

    public struct Circle
    {
        public Vector2 Center; public float Radius;

        public Circle(Vector2 position, float radius)
        {
            this.Center = position; this.Radius = radius;
        }

        public bool IntersectPoint(Vector2 point)
        {
            Vector2 diff = Center - point;
            return  diff.Length() <= Radius;
        }

        public bool IntersectPoint_OutDist(Vector2 point, out float distanceFromCenter)
        {
            Vector2 diff = Center - point;
            distanceFromCenter = diff.Length();
            return distanceFromCenter <= Radius;
        }

        public IntersectDetails2D IntersectPointDepth(Vector2 point)
        {
            Vector2 diff = Center - point;
            return new IntersectDetails2D(Radius - diff.Length(), point);
        }
        public bool Intersect(Circle otherCirkle)
        {
            Vector2 diff = Center - otherCirkle.Center;
            float totRadius = Radius + otherCirkle.Radius;
            float l = diff.Length();
            return l < totRadius;
        }

        public override string ToString()
        {
            return "Cirkle { X:" + Center.X.ToString() + ", Y:" + Center.Y.ToString() + ", R:" + Radius.ToString() + " }";
        }
    }

    public struct RectangleCentered
    {
        public static readonly RectangleCentered Zero = new RectangleCentered();

        public Vector2 Center;
        public Vector2 HalfSize;

        public float Left
        {
            get { return Center.X - HalfSize.X; }
        }
        public float Right
        {
            get { return Center.X + HalfSize.X; }
        }
        public float Top
        {
            get { return Center.Y - HalfSize.Y; }
        }
        public float Bottom
        {
            get { return Center.Y + HalfSize.Y; }
        }

        public float X
        {
            get { return Center.X; }
            set { Center.X = value; }
        }
        public float Y
        {
            get { return Center.Y; }
            set { Center.Y = value; }
        }
        public float HalfSizeX
        {
            get { return HalfSize.X; }
            set { HalfSize.X = value; }
        }
        public float HalfSizeY
        {
            get { return HalfSize.Y; }
            set { HalfSize.Y = value; }
        }

        public Vector2 TopLeft
        {
            get
            {
                return Center - HalfSize;
            }
        }

        public Vector2 LeftCenter
        {
            get
            {
                Vector2 result = Center;
                result.X -= HalfSize.X;
                return result;
            }
        }

        public Vector2 RightCenter
        {
            get
            {
                Vector2 result = Center;
                result.X += HalfSize.X;
                return result;
            }
        }

        public RectangleCentered(Vector2 center, Vector2 halfSize)
        {
            Center = center;
            HalfSize = halfSize;
        }
        public RectangleCentered(float xpos, float ypos, float halfSizeX, float halfSizeY)
        {
            Center = Vector2.Zero; HalfSize = Vector2.One;
            Center.X = xpos; Center.Y = ypos; HalfSize.X = halfSizeX; HalfSize.Y = halfSizeY;
        }

        public RectangleCentered(VectorRect area)
        {
            Center = area.Center;
            HalfSize = area.Size * PublicConstants.Half;
        }

        public Dir4 ClosestEdge(Vector2 point, out float intersectionSz)
        {
            Dir4 xEdge, yEdge;
            float xDist, yDist;

            if (point.X > Center.X)
            {
                xEdge = Dir4.E;
                xDist = Right - point.X;
            }
            else
            {
                xEdge = Dir4.W;
                xDist = point.X - Left;
            }

            if (point.Y > Center.Y)
            {
                yEdge = Dir4.S;
                yDist = Bottom - point.X;
            }
            else
            {
                yEdge = Dir4.N;
                yDist = point.X - Top;
            }

            if (Math.Abs(xDist) < Math.Abs(yDist))
            {
                intersectionSz = xDist;
                return xEdge;
            }
            else
            {
                intersectionSz = yDist;
                return yEdge;
            }
        }

        public bool IntersectPoint(Vector2 point)
        {
            Vector2 diff = point - Center;
            //lib.RotatePointAroundOrigo(diff, -ro
            return Math.Abs(diff.X) < HalfSize.X && Math.Abs(diff.Y) < HalfSize.Y;
        }

        public bool IntersectPoint(Vector2 point, float volumeRotation)
        {
            Vector2 diff = point - Center;
            diff = lib.RotatePointAroundOrigo(diff, -volumeRotation);
            return Math.Abs(diff.X) < HalfSize.X && Math.Abs(diff.Y) < HalfSize.Y;
        }

        public bool IntersectCirkle(Circle cirkle)
        {
            Vector2 diff = cirkle.Center - Center;
            float l = Bound.Min(diff.Length() - cirkle.Radius, 0);
            diff.Normalize();
            diff *= l;
            return Math.Abs(diff.X) < HalfSize.X && Math.Abs(diff.Y) < HalfSize.Y;
        }
        public IntersectDetails2D IntersectCirkleDepth(Circle adjustedCirkle, float rotation, Circle originalCirkle)
        {
            IntersectDetails2D result = new IntersectDetails2D();
            Vector2 diff = adjustedCirkle.Center - Center;
            float l = Bound.Min(diff.Length() - adjustedCirkle.Radius, 0);
            diff.Normalize();
            diff *= l;
            result.Depth = lib.SmallestValue(
                 Math.Abs(Math.Abs(diff.X) - HalfSize.X),
                 Math.Abs(Math.Abs(diff.Y) - HalfSize.Y));
            if (rotation == 0)
            {
                result.IntersectionCenter = adjustedCirkle.Center + diff;
            }
            else
            {

                result.IntersectionCenter = originalCirkle.Center + VectorExt.RotateVector(diff, rotation);
            }

            return result;
        }

        public void addRadius(float value)
        {
            HalfSize.X += value;
            HalfSize.Y += value;
        }

        public Vector2[] Vertices()
        {
            return new Vector2[]
                {
                    new Vector2(Center.X - HalfSize.X, Center.Y - HalfSize.Y),
                    new Vector2(Center.X + HalfSize.X, Center.Y - HalfSize.Y),
                    new Vector2(Center.X - HalfSize.X, Center.Y + HalfSize.Y),
                    new Vector2(Center.X + HalfSize.X, Center.Y + HalfSize.Y),
                };
        }

        public void UpdateVertices(ref Vector2[] v)
        {
            if (v == null)
            {
                v = new Vector2[4];
            }

            v[0] = new Vector2(Center.X - HalfSize.X, Center.Y - HalfSize.Y);
            v[1] = new Vector2(Center.X + HalfSize.X, Center.Y - HalfSize.Y);
            v[2] = new Vector2(Center.X - HalfSize.X, Center.Y + HalfSize.Y);
            v[3] = new Vector2(Center.X + HalfSize.X, Center.Y + HalfSize.Y);
        }

        public Vector2 positionFromPercent(Vector2 percent)
        {
            percent.X = Center.X + HalfSize.X * (percent.X - 0.5f);
            percent.Y = Center.Y + HalfSize.Y * (percent.Y - 0.5f);
            return percent;
        }

        public Vector2 positionFromPercent(Vector2 percent, float rotation)
        {
            percent.X = HalfSize.X * (percent.X - 0.5f);
            percent.Y = HalfSize.Y * (percent.Y - 0.5f);

            return Center + VectorExt.RotateVector(percent, rotation);
        }

        public VectorRect VectorRect()
        {
            return new VikingEngine.VectorRect(Center.X - HalfSize.X, Center.Y - HalfSize.Y, HalfSize.X * 2f, HalfSize.Y * 2f);
        }

        public override string ToString()
        {
            return "{ Center X:" + Center.X.ToString() + " Y:" + Center.Y.ToString() +
                " HALF WIDTH:" + HalfSize.X.ToString() + " HEIGHT:" + HalfSize.Y.ToString() + " }";
        }
    }

    public struct Rectangle16
    {

        public ShortVector2 Position;
        public ShortVector2 Size;
        public short X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        public short Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }
        public short Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }
        public short Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }
        public int Right
        {
            get { return Position.X + Size.X; }
        }
        public int Bottom
        {
            get { return Position.Y + Size.Y; }
        }
        public static VectorRect Zero = new VectorRect(Vector2.Zero, Vector2.Zero);
        public static VectorRect ZeroOne = new VectorRect(Vector2.Zero, Vector2.One);
        //{ get { return new VectorRect(); } }


        public Rectangle16(Rectangle rectangle)
        {
            Position = ShortVector2.Zero; Size = ShortVector2.Zero;
            Rect = rectangle;
        }
        public Rectangle16(ShortVector2 position, ShortVector2 size)
        {
            Position = position;
            Size = size;
        }
        public Rectangle16(short xpos, short ypos, short width, short height)
        {
            Position = ShortVector2.Zero; Size = ShortVector2.One;
            Position.X = xpos; Position.Y = ypos; Size.X = width; Size.Y = height;
        }
        public void AddToLeftSide(short add)
        {
            Position.X -= add; Size.X += add;
        }
        public void AddToTopSide(short add)
        {
            Position.Y -= add; Size.Y += add;
        }
        public Vector2 Center
        {
            get
            {
                Vector2 center = Position.Vec;
                center.X += Size.X * PublicConstants.Half;
                center.Y += Size.Y * PublicConstants.Half;

                return center;
            }
        }
        public Vector2 RandomPos()
        {
            Vector2 pos = Position.Vec;
            pos.X += (float)(Size.X * Ref.rnd.Double());
            pos.Y += (float)(Size.Y * Ref.rnd.Double());

            return pos;
        }

        public Rectangle Rect
        {
            get
            {
                Rectangle r = Rectangle.Empty;
                r.X = (int)Position.X;
                r.Y = (int)Position.Y;
                r.Width = (int)Size.X;
                r.Height = (int)Size.Y;
                return r;
            }
            set
            {
                Position.X = (short)value.X;
                Position.Y = (short)value.Y;
                Size.X = (short)value.Width;
                Size.Y = (short)value.Height;
            }
        }
        public bool IntersectPoint(Vector2 point)
        {
            if (point.X >= Position.X && point.Y >= Position.Y)
            {
                if (point.X <= Position.X + Size.X && point.Y <= Position.Y + Size.Y)
                {//HIT!
                    return true;
                }
            }
            return false;
        }
        public bool IntersectRect(VectorRect collideWith)
        {
            if (this.Position.X + Size.X > collideWith.X &&
                this.Position.X < collideWith.X + collideWith.Width)
            {
                if (this.Position.Y + Size.Y > collideWith.Y &&
                this.Position.Y < collideWith.Y + collideWith.Height)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddRadius(short radius)
        {
            Position.X -= radius;
            Position.Y -= radius;
            radius *= 2;
            Size.X += radius;
            Size.Y += radius;
        }
       
        public override string ToString()
        {
            return "{X:" + Position.X.ToString() + " Y:" + Position.Y.ToString() + " WIDTH:" + Size.X.ToString() + " HEIGHT:" + Size.Y.ToString() + "}";
        }

    }

    public struct IntVectorVolume
    {
        public static readonly IntVectorVolume Zero = new IntVectorVolume(IntVector3.Zero, IntVector3.Zero);
        public IntVector3 Position;
        public IntVector3 Size;

        public IntVectorVolume(IntVector3 position, IntVector3 size)
        {
            this.Position = position;
            this.Size = size;
        }
        public void AddToXSide(int add)
        {
            Position.X -= add; Size.X += add;
        }
        public void AddToYSide(int add)
        {
            Position.Y -= add; Size.Y += add;
        }
        public void AddToZSide(int add)
        {
            Position.Z -= add; Size.Z += add;
        }

        public void AddToSide(Dir4 side, int add)
        {
            switch (side)
            {
                case Dir4.N: AddToZSide(add); return;
                case Dir4.S: Size.Y += add; return;
                case Dir4.W: AddToXSide(add); return;
                case Dir4.E: Size.X += add; return;
            }
        }

        public override string ToString()
        {
            return "{X:" + Position.X.ToString() + " Y:" + Position.Y.ToString() + " Z:" + Position.Z.ToString() + 
                " SIZE X:" + Size.X.ToString() + " SIZE Y:" + Size.Y.ToString() + " SIZE Z:" + Size.Y.ToString() + "}";
        }

        
    }

    /// <summary>
    /// A bounding cylinder dimentions with its position in center
    /// </summary>
    public struct CylinderVolume
    {
        public Vector3 Center;
        public float HalfHeight;
        public float Radius;

        public CylinderVolume(Vector3 center, float halfHeight, float radius)
        {
            this.Center = center;
            this.HalfHeight = halfHeight;
            this.Radius = radius;
        }

        public Circle PlaneCirkle()
        {
            return new Circle(new Vector2(Center.X, Center.Z), Radius);
        }

        public bool Intersect(CylinderVolume otherVol)
        {
            return Math.Abs(otherVol.Center.Y - Center.Y) <= HalfHeight + otherVol.HalfHeight &&
                new Vector2(otherVol.Center.X - Center.X, otherVol.Center.Z - Center.Z).Length() <= Radius + otherVol.Radius;
        }
        public IntersectDetails2D IntersectLength2D(CylinderVolume otherVol)
        {
            IntersectDetails2D result = new IntersectDetails2D();
            Vector2 centerDiff = new Vector2(otherVol.Center.X - Center.X, otherVol.Center.Z - Center.Z);
            result.Depth = Math.Abs(centerDiff.Length() - Radius - otherVol.Radius);
            float collRadius = Radius - result.Depth * PublicConstants.Half;
            centerDiff.Normalize();
            result.IntersectionCenter = new Vector2(Center.X + centerDiff.X * collRadius, Center.Z + centerDiff.Y * collRadius);
            return result;
        }
    }

    public struct VectorVolume
    {
        public Vector3 Position;
        public Vector3 Scale;

        public VectorVolume()
        {
            Position = new Vector3();
            Scale = new Vector3();
        }

        public VectorVolume(Vector3 position, Vector3 size)
        {
            Position = position;
            Scale = size;
        }
    }

    /// <summary>
    /// A bounding box dimentions with its position in center
    /// </summary>
    public struct VectorVolumeC
    {
        public static readonly VectorVolumeC ZeroOne = new VectorVolumeC(Vector3.Zero, Vector3.One);
 
        public Vector3 Center;
        public Vector3 HalfSize;
        public VectorVolumeC(Vector3 center, Vector3 halfSize)
        {
            Center = center;
            HalfSize = halfSize;
        }

        public float X
        {
            get { return Center.X; }
            set { Center.X = value; }
        }
        public float Y
        {
            get { return Center.Y; }
            set { Center.Y = value; }
        }
        public float Z
        {
            get { return Center.Z; }
            set { Center.Z = value; }
        }
        public float HalfSizeX
        {
            get { return HalfSize.X; }
            set { HalfSize.X = value; }
        }
        public float HalfSizeY
        {
            get { return HalfSize.Y; }
            set { HalfSize.Y = value; }
        }
        public float HalfSizeZ
        {
            get { return HalfSize.Z; }
            set { HalfSize.Z = value; }
        }
        public bool Intersect(VectorVolumeC otherVol)
        {
            return Math.Abs(this.Center.X - otherVol.Center.X) <= this.HalfSize.X + otherVol.HalfSize.X &&
                Math.Abs(this.Center.Z - otherVol.Center.Z) <= this.HalfSize.Z + otherVol.HalfSize.Z &&
                Math.Abs(this.Center.Y - otherVol.Center.Y) <= this.HalfSize.Y + otherVol.HalfSize.Y;
        }
        public bool Intersect(Vector3 point)
        {
#if PCGAME
            Vector3 posDiff = Center - point;
#endif
            Vector3 halfSz = HalfSize * PublicConstants.Half;

            return Math.Abs(this.Center.X - point.X) <= this.HalfSize.X &&
                Math.Abs(this.Center.Y - point.Y) <= this.HalfSize.Y &&
                Math.Abs(this.Center.Z - point.Z) <= this.HalfSize.Z;

        }
        public IntersectDetails3D IntersectDepth(VectorVolumeC otherVol)
        {
            //Vector3 diff = this.Center - otherVol.Center;
            //diff -= HalfSize + otherVol.HalfSize;
            //return diff.Length();
            IntersectDetails3D result = new IntersectDetails3D();
            Vector3 diff = otherVol.Center - this.Center;
            Vector3 sumHalfSz = this.HalfSize + otherVol.HalfSize;
            //Vector3 thisVolPercentScale = this.HalfSize / sumHalfSz;
            result.Depth = (diff - sumHalfSz).Length();
            Vector3 diffPercentOfScale = diff / sumHalfSz;
            result.IntersectionCenter = Center + this.HalfSize * diffPercentOfScale;
            return result;
        }

        public Vector3 IntersectLength3D(VectorVolumeC otherVol)
        {
            Vector3 posDiff = otherVol.Center - this.Center;
            Vector3 scale = this.HalfSize + otherVol.HalfSize;
            Vector3 result = Vector3.Zero;

            if (Math.Abs(posDiff.X) <= scale.X)
            {
                result.X = (Math.Abs(posDiff.X) - scale.X) * lib.ToLeftRight(posDiff.X);
            }
            if (Math.Abs(posDiff.Y) <= scale.Y)
            {
                result.Y = (Math.Abs(posDiff.Y) - scale.Y) * lib.ToLeftRight(posDiff.Y);
            }
            if (Math.Abs(posDiff.Z) <= scale.Z)
            {
                result.Z = (Math.Abs(posDiff.Z) - scale.Z) * lib.ToLeftRight(posDiff.Z);
            }
            return result;
        }

        public Vector3 randomPosition()
        {
            return new Vector3(
                Center.X + (float)(-HalfSize.X + Ref.rnd.Double() * 2 * HalfSize.X),
                Center.Y + (float)(-HalfSize.Y + Ref.rnd.Double() * 2 * HalfSize.Y),
                Center.Z + (float)(-HalfSize.Z + Ref.rnd.Double() * 2 * HalfSize.Z));
        }

        public override string ToString()
        {
            return "Volyme: Center" + Center.ToString() + " HalfSize" + HalfSize.ToString();
                //"halfSz X:" + HalfSize.X.ToString() + " halfSz Y:" + HalfSize.Y.ToString() + " halfSz Z:" + HalfSize.Z.ToString() + "}";
        }

        public RectangleCentered PlaneRect()
        {
            return new RectangleCentered(Center.X, Center.Z, HalfSize.X, HalfSize.Z);
        }

        public BoundingBox boundingBox()
        {
            return new BoundingBox(Center - HalfSize, Center + HalfSize);
        }
    }

    public struct VectorRect
    {
        public Vector2 Position;
        public Vector2 Size;
        public float X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        public float Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }
        public float Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }
        public float Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }
        public float Right
        {
            get { return Position.X + Size.X; }
            // set { Size.X = value - Position.X; }
        }
        public float Bottom
        {
            get { return Position.Y + Size.Y; }
            set { Position.Y = value - Size.Y; }
        }
        public float RightTile
        {
            get { return Position.X + Size.X - 1; }
            set { Position.X = value - (Size.X - 1); }
        }
        public float BottomTile
        {
            get { return Position.Y + Size.Y - 1; }
            set { Position.Y = value - (Size.Y - 1); }
        }
        public Vector2 LeftBottom
        {
            get
            {
                return new Vector2(Position.X, Bottom);
            }
        }
        public Vector2 RightBottom
        {
            get
            {
                return Position + Size;
            }
            set
            {
                Position = value - Size;
            }
        }
        public Vector2 RightTop
        {
            get
            {
                Vector2 result = Position;
                result.X += Size.X;
                return result;
            }
        }
        public Vector2 CenterTop
        {
            get
            {
                Vector2 result = Position;
                result.X += Size.X * 0.5f;
                return result;
            }
        }
        public Vector2 CenterBottom
        {
            get
            {
                Vector2 result = Position;
                result.X += Size.X * 0.5f;
                result.Y += Size.Y;
                return result;
            }
        }
        public Vector2 RightCenter
        {
            get
            {
                Vector2 result = Position;
                result.X += Size.X;
                result.Y += Size.Y * 0.5f;
                return result;
            }
        }
        public Vector2 LeftCenter
        {
            get
            {
                Vector2 result = Position;
                result.Y += Size.Y * 0.5f;
                return result;
            }
        }
        public static VectorRect Zero = new VectorRect(Vector2.Zero, Vector2.Zero);
        public static VectorRect ZeroOne = new VectorRect(Vector2.Zero, Vector2.One);
        //{ get { return new VectorRect(); } }

        public void Empty()
        { Position = Vector2.Zero; Size = Vector2.One; }

        public VectorRect(Rectangle rectangle)
        {
            Position = Vector2.Zero; Size = Vector2.One;
            Rectangle = rectangle;
        }
        public VectorRect(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }
        public VectorRect(float xpos, float ypos, float width, float height)
        {
            Position = Vector2.Zero; Size = Vector2.One;
            Position.X = xpos; Position.Y = ypos; Size.X = width; Size.Y = height;
        }
        public VectorRect(Rectangle2 rect)
        {
            Position = rect.pos.Vec;
            Size = rect.size.Vec;
        }

        public static VectorRect FromCenterSize(Vector2 center, Vector2 size)
        {
            return new VectorRect(center - size * 0.5f, size);
        }

        public static VectorRect FromEdges(float left, float top, float right, float bottom)
        {
            Vector2 min = new Vector2(left, top);
            Vector2 max = new Vector2(right, bottom);
            return new VectorRect(min, max - min);
        }

        public static VectorRect FromTwoPoints(Vector2 point1, Vector2 point2)
        {
            Vector2 min = Vector2.Zero, max = Vector2.Zero;

            if (point1.X < point2.X)
            {
                min.X = point1.X; max.X = point2.X;
            }
            else
            {
                min.X = point2.X; max.X = point1.X;
            }

            if (point1.Y < point2.Y)
            {
                min.Y = point1.Y; max.Y = point2.Y;
            }
            else
            {
                min.Y = point2.Y; max.Y = point1.Y;
            }

            return new VectorRect(min, max - min);
        }

        public void Round()
        {
            Position.X = Convert.ToInt32(Position.X);
            Position.Y = Convert.ToInt32(Position.Y);

            Size.X = Convert.ToInt32(Size.X);
            Size.Y = Convert.ToInt32(Size.Y);
        }

        //public Vector2 GetPercentPos(float percentX, float percentY)

        public Vector2 PercentToPosition(float percX, float percY)
        {
            return PercentToPosition(new Vector2(percX, percY));
        }
        public Vector2 PercentToPosition(Vector2 percent)
        {
            return Position + Size * percent;
        }

        /// <param name="percent">Expected to range from -0.5 to 0.5</param>
        //public Vector2 GetPercentPosFromCenter(Vector2 percent)
        public Vector2 PercentFromCenterToPosition(Vector2 percent)
        {
            return Center + Size * percent;
        }

        public Vector2 PositionToPercent(Vector2 position)
        {
            return (position - this.Position) / Size;
        }

        public void AddToLeftSide(float add)
        {
            Position.X -= add; Size.X += add;
        }
        public void AddToTopSide(float add)
        {
            Position.Y -= add; Size.Y += add;
        }

        public void AddWidth(float add)
        {
            Position.X -= add * 0.5f;
            Size.X += add;
        }

        public void AddHeight(float add)
        {
            Position.Y -= add * 0.5f;
            Size.Y += add;
        }

        public Vector2 Center
        {
            get
            {
                Vector2 center = Position;
                center.X += Size.X * PublicConstants.Half;
                center.Y += Size.Y * PublicConstants.Half;
                return center;
            }
            set
            {
                Vector2 diff = value - this.Center;
                this.Position += diff;
            }
        }
        public Vector2 RandomPos()
        {
            Vector2 pos = Position;
            pos.X += (float)(Size.X * Ref.rnd.Double());
            pos.Y += (float)(Size.Y * Ref.rnd.Double());

            return pos;
        }

        public Vector2 HalfSize()
        {
            return new Vector2(Size.X * 0.5f, Size.Y * 0.5f);
        }

        public VectorRect CenterSize
        {
            get
            {
                VectorRect result = this;
                result.X += Size.X * 0.5f;
                result.Y += Size.X * 0.5f;
                return result;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                Rectangle r = Rectangle.Empty;
                r.X = (int)Position.X;
                r.Y = (int)Position.Y;
                r.Width = (int)Size.X;
                r.Height = (int)Size.Y;
                return r;
            }
            set
            {
                Position.X = value.X;
                Position.Y = value.Y;
                Size.X = value.Width;
                Size.Y = value.Height;
            }
        }

        public Rectangle2 Rectangle2
        {
            set
            {
                Position.X = value.X;
                Position.Y = value.Y;
                Size.X = value.Width;
                Size.Y = value.Height;
            }
        }

        public RectangleCentered RectangleCentered
        {
            get
            {
                RectangleCentered result = RectangleCentered.Zero;
                result.HalfSize = this.Size * 0.5f;
                result.Center = this.Position + result.HalfSize;

                return result;
            }
        }

        public VectorRect AreaIntersection(VectorRect other)
        {
            VectorRect result;

            if (IntersectRect(other))
            {
                float x = Math.Max(X, other.X);
                float y = Math.Max(Y, other.Y);
                result = new VectorRect(
                    x,
                    y,
                    Math.Min(Right, other.Right) - x,
                    Math.Min(Bottom, other.Bottom) - y);            
            }
            else
            {
                result = VectorRect.Zero;
            }

            return result;
        }

        public Vector2 GetCorner(Corner c)
        {
            switch (c)
            {
                case Corner.NW:
                    return Position;
                case Corner.NE:
                    return new Vector2(Position.X + Size.X, Position.Y);
                case Corner.SW:
                    return new Vector2(Position.X, Position.Y + Size.Y);
                case Corner.SE:
                    return Position + Size;
            }
            throw new ArgumentOutOfRangeException();
        }

        public bool IntersectPoint(Vector2 point)
        {
            if (point.X >= Position.X && point.Y >= Position.Y && point.X <= Position.X + Size.X && point.Y <= Position.Y + Size.Y)
            {
                return true;
            }
            return false;
        }

        public bool IntersectPoint(float x, float y)
        {
            if (x >= Position.X && y >= Position.Y && x <= Position.X + Size.X && y <= Position.Y + Size.Y)
            {
                return true;
            }
            return false;
        }

        public bool IntersectTilePoint(Vector2 point)
        {
            if (point.X >= Position.X && point.Y >= Position.Y && point.X <= Position.X + Size.X - 1 && point.Y <= Position.Y + Size.Y - 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// If the point get outside the bounds, its speed will be manage to move back
        /// </summary>
        /// <returns>the manage speeed</returns>
        public Vector2 KeepPointInsideBound_Speed(Vector2 point, Vector2 speed)
        {
            if (!IntersectPoint(point))
            {
                //Check if the point is passing the x or y border and flip its speed
                if (point.X < Position.X)
                {
                    speed.X = Math.Abs(speed.X);
                }
                else if (point.X > Right)
                {
                    speed.X = -Math.Abs(speed.X);
                }

                if (point.Y < Position.Y)
                {
                    speed.Y = Math.Abs(speed.Y);
                }
                else if (point.Y > Bottom)
                {
                    speed.Y = -Math.Abs(speed.Y);
                }
            }

            return speed;
        }

        public Vector2 KeepPointInsideBound_Position(Vector2 point)
        {
            if (!IntersectPoint(point))
            {
                // Place the point on closest border
                if (point.X < Position.X)
                {
                    point.X = Position.X;
                }
                else if (point.X > Right)
                {
                    point.X = Right;
                }

                if (point.Y < Position.Y)
                {
                    point.Y = Position.Y;
                }
                else if (point.Y > Bottom)
                {
                    point.Y = Bottom;
                }
            }

            return point;
        }

        public void KeepPointInsideBound_Position(ref float x, ref float y)
        {
            //if (!IntersectPoint(point))
            //{
                // Place the point on closest border
                if (x < Position.X)
                {
                    x = Position.X;
                }
                else if (x > Right)
                {
                    x = Right;
                }

                if (y < Position.Y)
                {
                    y = Position.Y;
                }
                else if (y > Bottom)
                {
                    y = Bottom;
                }
            //}

            //return point;
        }

        public VectorRect KeepSmallerRectInsideBound_Position(VectorRect smallRect)
        {
            //if (!IntersectRect(smallRect))
            {
                // Place the point on closest border
                if (smallRect.Position.X < Position.X)
                {
                    smallRect.Position.X = Position.X;
                }
                else if (smallRect.Right > Right)
                {
                    smallRect.SetRight(Right, true);
                }

                if (smallRect.Y < Position.Y)
                {
                    smallRect.Y = Position.Y;
                }
                else if (smallRect.Bottom > Bottom)
                {
                    smallRect.Bottom = Bottom;
                }
            }

            return smallRect;
        }

        public Vector2 KeepPointInsideBound_TilePosition(Vector2 pos)
        {
            // Place the point on closest border
            if (pos.X < Position.X)
                pos.X = Position.X;
            else if (pos.X > RightTile)
                pos.X = RightTile;

            if (pos.Y < Y)
                pos.Y = Y;
            else if (pos.Y > BottomTile)
                pos.Y = BottomTile;

            return pos;
        }
        public void KeepPointInsideBound_TilePositionXZref(ref Vector3 pos)
        {
            // Place the point on closest border
            if (pos.X < Position.X)
                pos.X = Position.X;
            else if (pos.X > RightTile)
                pos.X = RightTile;

            if (pos.Z < Y)
                pos.Z = Y;
            else if (pos.Z > BottomTile)
                pos.Z = BottomTile;
        }
        public Vector2 KeepPointOutsideBound_TilePosition(Vector2 pos)
        {
            if (IntersectTilePoint(pos))
            {
                Dir4 dir = ClosestTileEdge(pos);
                switch(dir)
                {
                    case Dir4.E:
                        pos.X = RightTile;
                        break;
                    case Dir4.N:
                        pos.Y = Y;
                        break;
                    case Dir4.S:
                        pos.Y = BottomTile;
                        break;
                    case Dir4.W:
                        pos.X = X;
                        break;
                }
            }
            return pos;
        }
        public Dir4 ClosestTileEdge(Vector2 pos)
        {
            float leftDist = Math.Abs(pos.X - Position.X);
            float minDist = leftDist;
            Dir4 result = Dir4.W;

            float rightDist = Math.Abs(pos.X - RightTile);
            if (rightDist < minDist)
            {
                minDist = rightDist;
                result = Dir4.E;
            }

            float topDist = Math.Abs(pos.Y - Position.Y);
            if (topDist < minDist)
            {
                minDist = topDist;
                result = Dir4.N;
            }

            float bottomDist = Math.Abs(pos.Y - BottomTile);
            if (bottomDist < minDist)
            {
                return Dir4.S;
            }
            return result;
        }

        //public void setLeftEdge(float left)
        //{
        //    float diff = Position.X - left;
        //    Position.X = left;
        //    Size.X += diff;
        //}
        //public void setRightEdge(float right)
        //{
        //    Size.X -= Right - right;
        //}

        public void SetRight(float right, bool adjustWidth)
        {
            if (adjustWidth)
            {
                Size.X = right - Position.X;
            }
            else
            {
                Position.X = right - Size.X;
            }
        }
        public void SetBottom(float bottom, bool adjustHeight)
        {
            if (adjustHeight)
            {
                Size.Y = bottom - Position.Y;
            }
            else
            {
                Position.Y = bottom - Size.Y;
            }
        }

        public float ClosestEdgeDistance(Vector2 point, INorm norm, out Vector2 closest)
        {
            closest = ClosestEdgePoint(point);
            return norm.Norm(point - closest);
        }
        public float ClosestDistance(Vector2 point, INorm norm, out Vector2 closest)
        {
            closest = ClosestPoint(point);
            return norm.Norm(point - closest);
        }
        public Vector2 ClosestPoint(Vector2 point)
        {
            if (point.X < Position.X)
            {
                point.X = Position.X;
            }
            else if (point.X > Right)
            {
                point.X = Right;
            }

            if (point.Y < Position.Y)
            {
                point.Y = Position.Y;
            }
            else if (point.Y > Bottom)
            {
                point.Y = Bottom;
            }
            return point;
        }
        public Vector2 ClosestEdgePoint(Vector2 point)
        {
            bool insideX = false;

            Vector2 target = Vector2.Zero;

            if (point.X < Position.X)
            {
                point.X = Position.X;
            }
            else if (point.X > Right)
            {
                point.X = Right;
            }
            else
            {
                insideX = true;
                if (Math.Abs(point.X - Position.X) < Math.Abs(point.X - Right))
                {
                    target.X = Position.X;
                }
                else
                {
                    target.X = Right;
                }
            }

            if (point.Y < Position.Y)
            {
                point.Y = Position.Y;
            }
            else if (point.Y > Bottom)
            {
                point.Y = Bottom;
            }
            else
            {
                if (insideX)
                {
                    if (Math.Abs(point.Y - Position.Y) < Math.Abs(point.Y - Bottom))
                    {
                        target.Y = Position.Y;
                    }
                    else
                    {
                        target.Y = Bottom;
                    }

                    if (Math.Abs(target.X - point.X) < Math.Abs(target.Y - point.Y))
                    {
                        target.Y = point.Y;
                    }
                    else
                    {
                        target.X = point.X;
                    }

                    point = target;
                }
            }

            return point;
        }
        public bool IntersectX(float x)
        {
            if (x >= Position.X && x <= Position.X + Size.X)
            {
                return true;
            }
            return false;
        }
        public bool IntersectY(float y)
        {
            if (y >= Position.Y && y <= Position.Y + Size.Y)
            {
                return true;
            }
            return false;
        }

        public bool IntersectRect(VectorRect collideWith)
        {
            if (this.Position.X + Size.X > collideWith.X &&
                this.Position.X < collideWith.X + collideWith.Width)
            {
                if (this.Position.Y + Size.Y > collideWith.Y &&
                this.Position.Y < collideWith.Y + collideWith.Height)
                {
                    return true;
                }
            }
            return false;
        }
        public void AddRadius(float radius)
        {
            Position.X -= radius;
            Position.Y -= radius;
            Size.X += radius * PublicConstants.Twice;
            Size.Y += radius * PublicConstants.Twice;
        }

        public void AddPercentRadius(float percentRadius)
        {
            AddXRadius(percentRadius * Size.X);
            AddYRadius(percentRadius * Size.Y);
        }

        public void AddXRadius(float radius)
        {
            Position.X -= radius;
            Size.X += radius * 2f;
        }
        public void AddYRadius(float radius)
        {
            Position.Y -= radius;
            Size.Y += radius * 2f;
        }

        public void nextAreaX(int dir, float spacing)
        {
            Position.X += dir * (Size.X + spacing);
        }
        public void nextAreaY(int dir, float spacing)
        {
            Position.Y += dir * (Size.Y + spacing);
        }

        public VectorRect nextTilePos(IntVector2 tilepos)
        {
            VectorRect result = this;
            result.Position.X += tilepos.X * result.Size.X;
            result.Position.Y += tilepos.Y * result.Size.Y;
            return result;
        }

        public void FlipX()
        {
            Size.X = -Size.X;
        }

        public float Side(Dir4 side)
        {
            switch (side)
            {
                case Dir4.W: return Position.X;
                case Dir4.N: return Position.Y;
                case Dir4.E: return Position.X + Size.X;
                case Dir4.S: return Position.Y + Size.Y;
            }

            throw new Exception();
        }

        public override string ToString()
        {
            return "{X:" + Position.X.ToString() + " Y:" + Position.Y.ToString() + " WIDTH:" + Size.X.ToString() + " HEIGHT:" + Size.Y.ToString() + "}";
        }


    }
    
}
