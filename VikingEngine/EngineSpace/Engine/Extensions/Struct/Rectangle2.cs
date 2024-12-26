using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Physics;
using VikingEngine.EngineSpace.Maths;
using System.Runtime.CompilerServices;


namespace VikingEngine
{
    public struct Rectangle2
    {
        /*Properties */
        public IntVector2 BottomRight { get { return pos + size; } }
        public IntVector2 BottomRightTile { get { return pos + size - 1; } }
        public IntVector2 BottomLeft { get { return pos + new IntVector2(0, size.Y); } }
        public IntVector2 BottomLeftTile { get { return pos + new IntVector2(0, size.Y - 1); } }
        public IntVector2 TopRight { get { return pos + new IntVector2(size.X, 0); } }
        public IntVector2 TopRightTile { get { return pos + new IntVector2(size.X - 1, 0); } }
        public IntVector2 TopCenter { get { return pos + new IntVector2(size.X / 2, 0); } }
        public IntVector2 CenterLeft { get { return pos + new IntVector2(0, size.Y / 2); } }
        public IntVector2 BottomCenter { get { return pos + new IntVector2(size.X / 2, size.Y); } }
        public IntVector2 CenterRight { get { return pos + new IntVector2(size.X, size.Y / 2); } }
        public IntVector2 BottomCenterTile { get { return pos + new IntVector2(size.X / 2, size.Y - 1); } }
        public IntVector2 CenterRightTile { get { return pos + new IntVector2(size.X - 1, size.Y / 2); } }
        public IntervalF BoundsX { get { return new IntervalF(pos.X, Right); } }
        public IntervalF BoundsY { get { return new IntervalF(pos.Y, Bottom); } }
        public IntervalF TileBoundsX { get { return new IntervalF(pos.X, RightTile); } }
        public IntervalF TileBoundsY { get { return new IntervalF(pos.Y, BottomTile); } }
        public int X
        {
            get { return pos.X; }
            set { pos.X = value; }
        }
        public int Y
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }
        public int Width
        {
            get { return size.X; }
            set { size.X = value; }
        }
        public int Height
        {
            get { return size.Y; }
            set { size.Y = value; }
        }
        public int Right
        {
            get { return pos.X + size.X; }
            set { pos.X = value - size.X; }
        }
        public int Bottom
        {
            get { return pos.Y + size.Y; }
            set { pos.Y = value - size.Y; }
        }
        public int CenterX
        {
            get { return pos.X + size.X / 2; }
            set { pos.X = value - size.X / 2; }
        }
        public int CenterY
        {
            get { return pos.Y + size.Y / 2; }
            set { pos.Y = value - size.Y / 2; }
        }
        public int RightTile
        {
            get { return pos.X + size.X - 1; }
        }
        public int BottomTile
        {
            get { return pos.Y + size.Y - 1; }
        }

        public int Area => size.Area();

        /*Fields */
        public IntVector2 pos;
        public IntVector2 size;


        /// <summary>
        /// Returns the right or bottom coordinate, depending on the dimension.
        /// </summary>
        public int GetMaxCoordAlongDimension(Dimensions dim)
        {
            if (dim == Dimensions.X)
                return Right;
            else if (dim == Dimensions.Y)
                return Bottom;
            return 0;
        }

        /// <summary>
        /// Returns the left or top coordinate, depending on the dimension.
        /// </summary>
        public int GetMinCoordAlongDimension(Dimensions dim)
        {
            if (dim == Dimensions.X)
                return X;
            else if (dim == Dimensions.Y)
                return Y;
            return 0;
        }
        public int GetDistanceToEdge(Dir4 dir, IntVector2 pos)
        {
            switch (dir)
            {
                case Dir4.E:
                    return Math.Abs(pos.X - Right);
                case Dir4.N:
                    return Math.Abs(pos.Y - Y);
                case Dir4.S:
                    return Math.Abs(pos.Y - Bottom);
                case Dir4.W:
                    return Math.Abs(pos.X - X);
            }
            return 0;
        }
        public int GetDistanceToTileEdge(Dir4 dir, IntVector2 pos)
        {
            switch (dir)
            {
                case Dir4.E:
                    return Math.Abs(pos.X - RightTile);
                case Dir4.N:
                    return Math.Abs(pos.Y - Y);
                case Dir4.S:
                    return Math.Abs(pos.Y - BottomTile);
                case Dir4.W:
                    return Math.Abs(pos.X - X);
            }
            return 0;
        }
        public int GetEdgeCoordinate(Dir4 dir)
        {
            switch (dir)
            {
                case Dir4.E:
                    return Right;
                case Dir4.N:
                    return Y;
                case Dir4.S:
                    return Bottom;
                case Dir4.W:
                    return X;
            }
            return 0;
        }
        public int GetTileEdgeCoordinate(Dir4 dir)
        {
            switch (dir)
            {
                case Dir4.E:
                    return RightTile;
                case Dir4.N:
                    return Y;
                case Dir4.S:
                    return BottomTile;
                case Dir4.W:
                    return X;
            }
            return 0;
        }
        public bool IsCorner(IntVector2 pos)
        {
            for (Corner c = 0; c < Corner.NUM; c++)
            {
                if (pos == GetCorner(c))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsTileCorner(IntVector2 pos)
        {
            for (Corner c = 0; c < Corner.NUM; c++)
            {
                if (pos == GetTileCorner(c))
                {
                    return true;
                }
            }
            return false;
        }

        public IntVector2 GetCorner(Corner c)
        {
            switch (c)
            {
                case Corner.NW:
                    return pos;
                case Corner.NE:
                    return new IntVector2(Right, pos.Y);
                case Corner.SW:
                    return new IntVector2(pos.X, Bottom);
                case Corner.SE:
                    return pos + size;
            }
            throw new ArgumentOutOfRangeException();
        }
        public Corner GetCorner(IntVector2 pos)
        {
            for (Corner c = 0; c < Corner.NUM; c++)
            {
                if (pos == GetCorner(c))
                {
                    return c;
                }
            }
            return Corner.NO_CORNER;
        }
        public IntVector2 GetTileCorner(Corner c)
        {
            switch (c)
            {
                case Corner.NW:
                    return pos;
                case Corner.NE:
                    return new IntVector2(RightTile, pos.Y);
                case Corner.SW:
                    return new IntVector2(pos.X, BottomTile);
                case Corner.SE:
                    return pos + size - 1;
            }
            throw new ArgumentOutOfRangeException();
        }

        public IntVector2 GetTileEdgeCenter(Dir4 edge)
        {
            switch (edge)
            {
                case Dir4.N:
                    return new IntVector2(CenterTileX, pos.Y);
                case Dir4.S:
                    return new IntVector2(CenterTileX, BottomTile);
                case Dir4.W:
                    return new IntVector2(pos.X, CenterTileY);
                case Dir4.E:
                    return new IntVector2(RightTile, CenterTileY);

            }
            throw new ArgumentOutOfRangeException();
        }
        public IntVector2 GetClosestTilePoint(IntVector2 pos)
        {
            int x = this.pos.X;
            int y = this.pos.Y;
            int rt = RightTile;
            int bt = BottomTile;
            if (pos.X > rt)
                pos.X = rt;
            else if (pos.X < x)
                pos.X = x;
            if (pos.Y > bt)
                pos.Y = bt;
            else if (pos.Y < y)
                pos.Y = y;
            return pos;
        }

        public static Rectangle2 Zero = new Rectangle2(IntVector2.Zero, IntVector2.Zero);
        public static Rectangle2 ZeroOne = new Rectangle2(IntVector2.Zero, IntVector2.One);


        public Rectangle2(Rectangle rectangle)
        {
            pos = new IntVector2(rectangle.X, rectangle.Y); size = new IntVector2(rectangle.Width, rectangle.Height);

        }
        public Rectangle2(IntVector2 position, IntVector2 size)
        {
            pos = position;
            this.size = size;
        }
        public Rectangle2(IntVector2 center, int radius)
        {
            pos = center - radius;
            size = new IntVector2(radius * PublicConstants.Twice + 1);
        }
        public Rectangle2(IntVector2 size)
        {
            pos = IntVector2.Zero;
            this.size = size;
        }

        public static Rectangle2 FromCenterSize(IntVector2 center, IntVector2 size)
        {
            return new Rectangle2(-size / 2 + center, size);
        }

        public static Rectangle2 FromCenterTileAndRadius(IntVector2 center, int radius)
        {
            int width = radius * 2 + 1;
            return new Rectangle2(center.X - radius, center.Y - radius, width, width);
        }

        public static Rectangle2 FromCenterTileAndRadius(IntVector2 center, IntVector2 radius)
        {
            return new Rectangle2(
                center.X - radius.X, center.Y - radius.Y, 
                radius.X * 2 + 1, radius.Y * 2 + 1);
        }

        public Rectangle2(int x, int y, int width, int height)
        {
            pos = new IntVector2(x, y); size = new IntVector2(width, height);
        }

        public static Rectangle2 FromTwoTilePoints(IntVector2 point1, IntVector2 point2)
        {
            Rectangle2 result = FromTwoPoints(point1, point2);
            result.size += IntVector2.One;

            return result;
        }

        public static Rectangle2 FromTwoPoints(IntVector2 point1, IntVector2 point2)
        {
            IntVector2 min = IntVector2.Zero, max = IntVector2.Zero;

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

            return new Rectangle2(min, max - min);
        }

        public void AddToLeftSide(int add)
        {
            pos.X -= add; size.X += add;
        }
        public void AddToTopSide(int add)
        {
            pos.Y -= add; size.Y += add;
        }

        public void AddToSide(Dir4 side, int add)
        {
            switch (side)
            {
                case Dir4.N: AddToTopSide(add); return;
                case Dir4.S: size.Y += add; return;
                case Dir4.W: AddToLeftSide(add); return;
                case Dir4.E: size.X += add; return;
            }
        }

        public Vector2 CenterF
        {
            get
            {
                Vector2 center = pos.Vec;
                center.X += size.X * PublicConstants.Half;
                center.Y += size.Y * PublicConstants.Half;

                return center;
            }
        }

        public IntVector2 Center
        {
            get { return new IntVector2(pos.X + size.X / 2, pos.Y + size.Y / 2); }
            set { pos = value - size / 2; }
        }

        public IntVector2 CenterTile
        {
            get { return new IntVector2(pos.X + (size.X - 1) / 2, pos.Y + (size.Y - 1) / 2); }
            set { pos = value - (size - 1) / 2; }
        }

        public int CenterTileX { get { return pos.X + size.X / 2; } }
        public int CenterTileY { get { return pos.Y + size.Y / 2; } }

        public Vector2 RandomPos()
        {
            Vector2 pos = this.pos.Vec;
            pos.X += (float)(size.X * Ref.rnd.Double());
            pos.Y += (float)(size.Y * Ref.rnd.Double());

            return pos;
        }
        public IntVector2 RandomTile()
        {
            IntVector2 result = pos;
            result.X += Ref.rnd.Int(size.X);
            result.Y += Ref.rnd.Int(size.Y);
            return result;
        }

        public Vector2 RandomPos(PcgRandom prng)
        {
            Vector2 pos = this.pos.Vec;
            pos.X += (float)(size.X * prng.Float());
            pos.Y += (float)(size.Y * prng.Float());

            return pos;
        }
        public IntVector2 RandomTile(PcgRandom prng)
        {
            IntVector2 result = pos;
            result.X += prng.Int(size.X);
            result.Y += prng.Int(size.Y);
            return result;
        }

        public Rectangle Rect
        {
            get
            {
                Rectangle r = Rectangle.Empty;
                r.X = (int)pos.X;
                r.Y = (int)pos.Y;
                r.Width = (int)size.X;
                r.Height = (int)size.Y;
                return r;
            }
            set
            {
                pos.X = value.X;
                pos.Y = value.Y;
                size.X = value.Width;
                size.Y = value.Height;
            }
        }
        public bool IntersectPoint(IntVector2 point)
        {
            if (point.X >= pos.X && point.Y >= pos.Y)
            {
                if (point.X <= pos.X + size.X && point.Y <= pos.Y + size.Y)
                {//HIT!
                    return true;
                }
            }
            return false;
        }
        public bool IntersectPoint(Vector2 point)
        {
            if (point.X >= pos.X && point.Y >= pos.Y)
            {
                if (point.X <= pos.X + size.X && point.Y <= pos.Y + size.Y)
                {//HIT!
                    return true;
                }
            }
            return false;
        }
        public bool IntersectTilePoint(IntVector2 point)
        {
            if (point.X >= pos.X && point.Y >= pos.Y)
            {
                if (point.X < pos.X + size.X && point.Y < pos.Y + size.Y)
                {//HIT!
                    return true;
                }
            }
            return false;
        }

        public IntVector2 KeepTilePointInArea(IntVector2 point)
        {
            if (point.X < pos.X)
            {
                point.X = pos.X;
            }
            else if (point.X >= pos.X + size.X)
            {
                point.X = pos.X + size.X - 1;
            }

            if (point.Y < pos.Y)
            {
                point.Y = pos.Y;
            }
            else if (point.Y >= pos.Y + size.Y)
            {
                point.Y = pos.Y + size.Y - 1;
            }

            return point;
        }

        public bool IntersectRect(Rectangle2 otherRect)
        {
            if (this.pos.X + size.X > otherRect.X &&
                this.pos.X < otherRect.X + otherRect.Width)
            {
                if (this.pos.Y + size.Y > otherRect.Y &&
                    this.pos.Y < otherRect.Y + otherRect.Height)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IntersectRect(IntVector2 otherRectMin, IntVector2 otherRectMax)
        {
            if (this.pos.X + size.X > otherRectMin.X &&
                this.pos.X < otherRectMax.X)
            {
                if (this.pos.Y + size.Y > otherRectMin.Y &&
                    this.pos.Y < otherRectMax.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IntersectRect(Vector2 otherRectMin, Vector2 otherRectMax)
        {
            if (this.pos.X + size.X > otherRectMin.X &&
                this.pos.X < otherRectMax.X)
            {
                if (this.pos.Y + size.Y > otherRectMin.Y &&
                    this.pos.Y < otherRectMax.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddRadius(int radius)
        {
            pos.X -= radius;
            pos.Y -= radius;
            size.X += radius * PublicConstants.Twice;
            size.Y += radius * PublicConstants.Twice;
        }

        public void AddWidthRadius(int radius)
        {
            pos.X -= radius;
            size.X += radius * PublicConstants.Twice;
        }

        public void AddHeightRadius(int radius)
        {
            pos.Y -= radius;
            size.Y += radius * PublicConstants.Twice;
        }

        public void SetMaxRadius(int w, int h)
        {
            if (size.X > w)
            {
                AddWidthRadius((w - size.X) / 2);
            }
            if (size.Y > h)
            {
                AddHeightRadius((h - size.Y) / 2);
            }
        }

        

        public Rectangle2 FindIntersection(Rectangle2 other)
        {
            Rectangle2 result;

            if (IntersectRect(other))
            {
                int x = Math.Max(X, other.X);
                int y = Math.Max(Y, other.Y);
                result = new Rectangle2(x,
                                        y,
                                        Math.Min(Right, other.Right) - x,
                                        Math.Min(Bottom, other.Bottom) - y);
            }
            else
            {
                result = new Rectangle2(0, 0, 0, 0);
            }

            return result;
        }

        public bool Contains(Rectangle2 other)
        {
            return FindIntersection(other) == other;
        }
        public bool LiesOnTileEdge(IntVector2 pos)
        {
            if (MathExt.InIE(this.pos.X, pos.X, Right) && MathExt.InIE(this.pos.Y, pos.Y, Bottom))
            {
                return pos.Y == this.pos.Y || pos.Y == BottomTile ||
                       pos.X == this.pos.X || pos.X == RightTile;
            }
            return false;
        }
        public int LengthToClosestTileEdgeX(int x)
        {
            int leftDist = x - pos.X;
            int rightDist = x - RightTile;
            return Math.Abs(leftDist) < Math.Abs(rightDist) ? leftDist : rightDist;
        }
        public int LengthToClosestTileEdgeY(int y)
        {
            int topDist = y - pos.Y;
            int bottomDist = y - BottomTile;
            return Math.Abs(topDist) < Math.Abs(bottomDist) ? topDist : bottomDist;
        }
        public int LengthToClosestTileEdge(IntVector2 pos)
        {
            return Math.Abs(lib.SmallestAbsoluteValue(LengthToClosestTileEdgeX(pos.X), LengthToClosestTileEdgeY(pos.Y)));
        }
        public int LengthToClosestEdgeX(int x)
        {
            int leftDist = x - pos.X;
            int rightDist = x - Right;
            return Math.Abs(leftDist) < Math.Abs(rightDist) ? leftDist : rightDist;
        }
        public int LengthToClosestEdgeY(int y)
        {
            int topDist = y - pos.Y;
            int bottomDist = y - Bottom;
            return Math.Abs(topDist) < Math.Abs(bottomDist) ? topDist : bottomDist;
        }
        public int LengthToClosestEdge(IntVector2 pos)
        {
            return Math.Abs(lib.SmallestAbsoluteValue(LengthToClosestEdgeX(pos.X), LengthToClosestEdgeY(pos.Y)));
        }
        public Dir4 ClosestTileEdge(IntVector2 pos)
        {
            int leftDist = Math.Abs(pos.X - pos.X);
            int minDist = leftDist;
            Dir4 result = Dir4.W;

            int rightDist = Math.Abs(pos.X - RightTile);
            if (rightDist < minDist)
            {
                minDist = rightDist;
                result = Dir4.E;
            }

            int topDist = Math.Abs(pos.Y - pos.Y);
            if (topDist < minDist)
            {
                minDist = topDist;
                result = Dir4.N;
            }

            int bottomDist = Math.Abs(pos.Y - BottomTile);
            if (bottomDist < minDist)
            {
                return Dir4.S;
            }
            return result;
        }

        public void SetBounds(Rectangle2 outerBound)
        {
            if (pos.X < outerBound.X)
                AddToLeftSide(pos.X - outerBound.X);
            if (pos.Y < outerBound.Y)
                AddToTopSide(pos.Y - outerBound.Y);

            if (outerBound.Right < Right)
                size.X += outerBound.Right - Right;
            if (outerBound.Bottom < Bottom)
                size.Y += outerBound.Bottom - Bottom;

        }

        public void SetTileBounds(Rectangle2 minMaxTiles)
        {
            if (pos.X < minMaxTiles.X)
                AddToLeftSide(pos.X - minMaxTiles.X);
            if (pos.Y < minMaxTiles.Y)
                AddToTopSide(pos.Y - minMaxTiles.Y);

            if (minMaxTiles.Right < RightTile)
                size.X += minMaxTiles.Right - RightTile;
            if (minMaxTiles.Bottom < BottomTile)
                size.Y += minMaxTiles.Bottom - BottomTile;
        }

        /// <summary>
        /// Expand the rectangle so it will cover the tile position
        /// </summary>
        public void includeTile(IntVector2 tile)
        {
            if (pos.X > tile.X)
                AddToLeftSide(pos.X - tile.X);
            if (pos.Y > tile.Y)
                AddToTopSide(pos.Y - tile.Y);

            if (RightTile < tile.X)
                size.X += tile.X - RightTile;
            if (BottomTile < tile.Y)
                size.Y += tile.Y - BottomTile;
        }

        public void SetDim(Dimensions dim, int pos, int width)
        {
            switch (dim)
            {
                case Dimensions.X:
                    X = pos;
                    Width = width;
                    return;
                case Dimensions.Y:
                    Y = pos;
                    Height = width;
                    return;
            }
            throw new NotImplementedException();
        }

        public static bool operator ==(Rectangle2 value1, Rectangle2 value2)
        {
            return value1.pos == value2.pos && value1.size == value2.size;
        }
        public override bool Equals(object obj)
        {
            if (obj is Rectangle2)
            {
                Rectangle2 rect = (Rectangle2)obj;
                return pos == rect.pos && size == rect.size;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return ((3 * pos.X ^ 5 * pos.Y) ^ 7 * size.X) ^ 11 * size.Y;
        }
        public static bool operator !=(Rectangle2 value1, Rectangle2 value2)
        {
            return value1.pos != value2.pos || value1.size != value2.size;
        }
        public static Rectangle2 operator *(Rectangle2 lhs, int rhs)
        {
            return new Rectangle2(lhs.pos * rhs, lhs.size * rhs);
        }
        public static Rectangle2 operator /(Rectangle2 lhs, int rhs)
        {
            return new Rectangle2(lhs.pos / rhs, lhs.size / rhs);
        }
        public override string ToString()
        {
            return "{X:" + pos.X.ToString() + " Y:" + pos.Y.ToString() + " WIDTH:" + size.X.ToString() + " HEIGHT:" + size.Y.ToString() + "}";
            //return "(" + size.X.ToString() + "," + size.Y.ToString() + "): (" + pos.X.ToString() + "-" + Right.ToString() + ", " + pos.Y.ToString() + "-" + Bottom.ToString() + ")";
        }

        public void AxisCenteredDirectionallyDistancedSetPosition(Dir4 direction, Rectangle2 relative, int distance)
        {
            switch (direction)
            {
                case Dir4.N:
                    Bottom = relative.Y - distance;
                    CenterX = relative.CenterX;
                    break;
                case Dir4.S:
                    Y = relative.Bottom + distance;
                    CenterX = relative.CenterX;
                    break;
                case Dir4.E:
                    X = relative.Right + distance;
                    CenterY = relative.CenterY;
                    break;
                case Dir4.W:
                    Right = relative.X - distance;
                    CenterY = relative.CenterY;
                    break;
            }
        }

        public int GetSide(Dir4 forward, Alignment aligment)
        {
            switch (forward)
            {
                case Dir4.E:
                    switch (aligment)
                    {
                        case Alignment.Left:
                            return Bottom;
                        case Alignment.Center:
                            return Center.Y;
                        case Alignment.Right:
                            return Y;
                    }
                    break;
                case Dir4.N:
                    switch (aligment)
                    {
                        case Alignment.Left:
                            return X;
                        case Alignment.Center:
                            return Center.X;
                        case Alignment.Right:
                            return Right;
                    }
                    break;
                case Dir4.S:
                    switch (aligment)
                    {
                        case Alignment.Left:
                            return Right;
                        case Alignment.Center:
                            return Center.X;
                        case Alignment.Right:
                            return X;
                    }
                    break;
                case Dir4.W:
                    switch (aligment)
                    {
                        case Alignment.Left:
                            return Y;
                        case Alignment.Center:
                            return Center.Y;
                        case Alignment.Right:
                            return Bottom;
                    }
                    break;
            }
            return 0;
        }

        public void SetSide(Dir4 forward, Alignment aligment, int val)
        {
            switch (forward)
            {
                case Dir4.E:
                    switch (aligment)
                    {
                        case Alignment.Left:
                            Bottom = val;
                            break;
                        case Alignment.Center:
                            CenterY = val;
                            break;
                        case Alignment.Right:
                            Y = val;
                            break;
                    }
                    break;
                case Dir4.N:
                    switch (aligment)
                    {
                        case Alignment.Left:
                            X = val;
                            break;
                        case Alignment.Center:
                            CenterX = val;
                            break;
                        case Alignment.Right:
                            Right = val;
                            break;
                    }
                    break;
                case Dir4.S:
                    switch (aligment)
                    {
                        case Alignment.Left:
                            Right = val;
                            break;
                        case Alignment.Center:
                            CenterX = val;
                            break;
                        case Alignment.Right:
                            X = val;
                            break;
                    }
                    break;
                case Dir4.W:
                    switch (aligment)
                    {
                        case Alignment.Left:
                            Y = val;
                            break;
                        case Alignment.Center:
                            CenterY = val;
                            break;
                        case Alignment.Right:
                            Bottom = val;
                            break;
                    }
                    break;
            }
        }

        public void Align(Dir4 forward, ref Rectangle2 alignThis, Alignment alignment)
        {
            alignThis.SetSide(forward, alignment, GetSide(forward, alignment));
            switch (forward)
            {
                case Dir4.E:
                    alignThis.X = Right;
                    break;
                case Dir4.N:
                    alignThis.Bottom = Y;
                    break;
                case Dir4.S:
                    alignThis.Y = Bottom;
                    break;
                case Dir4.W:
                    alignThis.Right = X;
                    break;
            }
        }
    }


    
}
