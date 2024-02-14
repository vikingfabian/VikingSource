using System;
using Microsoft.Xna.Framework;

namespace VikingEngine.Physics
{
    class CircleBound : AbsBound2D
    {
        public Vector2 center;
        public float radius;

        public CircleBound()
        { }

        public CircleBound(float radius)
        {
            this.radius = radius;
        }

        public CircleBound(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        override public void baseUpdate(Vector2 center, float rotation)
        {
            this.center = center;
        }

        override public Vector2 Center { get { return center; } set { center = value; } }
        override public float ExtremeRadius { get { return radius; } }
        override public float InnerCirkleRadius { get { return radius; } }

        override public bool VerticeIntersect(Vector2[] vertices)
        {
            Vector2 diff = Vector2.Zero;
            foreach (Vector2 v in vertices)
            {
                diff.X = v.X - center.X;
                diff.Y = v.Y - center.Y;
                if (diff.Length() < radius) return true;
            }
            return false;
        }

        override public bool Intersect(AbsBound2D otherBound)
        {
            if (PhysicsLib2D.ExtremeRadiusColl(this, otherBound))
            {
                if (PhysicsLib2D.MinRadiusColl(this, otherBound)) return true;

                switch (otherBound.Type)
                {
                    case Bound2DType.Circle:
                        return false; //should be picked up by MinRadius coll
                    default:
                        return PhysicsLib2D.CollCirkleAndRectangle(this, otherBound);
                }
            }
            return false;
        }

        override public bool Intersect(Vector2 point)
        {
            return PhysicsLib2D.PointInsideCirkle(point, center, radius);
        }


        override public Collision2D Intersect2(AbsBound2D otherBound)
        {
            if (otherBound.Type == Bound2DType.Circle)
            {
                Vector2 collitionDiff = center - otherBound.Center;
                float totalRadius = radius + otherBound.HalfSize.X;

                float l;
                Vector2 norm = VectorExt.Normalize(collitionDiff, out l);

                if (l < totalRadius)
                {
                    Collision2D result = new Collision2D();
                    result.IsCollision = true;
                    result.depth = Math.Abs(l - totalRadius);

                    if (collitionDiff == Vector2.Zero)
                    {
                        collitionDiff = Rotation1D.Random().Direction(1f);
                    }
                    else
                    {
                        collitionDiff = norm;
                    }
                    result.surfaceNormal = collitionDiff;
                    result.direction = collitionDiff * result.depth;


                    return result;
                }
                else
                {
                    return Collision2D.NoCollision;
                }
            }

            if (PhysicsLib2D.ExtremeRadiusColl(this, otherBound))
            {
                switch (otherBound.Type)
                {
                    case Bound2DType.Rectangle:
                        return PhysicsLib2D.CollCirkleAndRectangle_v2(this, otherBound, true);

                    case Bound2DType.RectangleRotated:
                        return PhysicsLib2D.CollCirkleAndRectangleRotated_v2(this, otherBound, true);

                    default:
                        throw new System.NotImplementedException();
                }
            }
            return Collision2D.NoCollision;
        }
        override public Vector2[] Vertices()
        { throw new NotImplementedException(); }

        public override AbsBound2D Clone()
        {
            return clone();
        }
        public CircleBound clone()
        {
            CircleBound clone = new CircleBound();
            clone.offset = this.offset;
            clone.rotationAdd = this.rotationAdd;
            clone.center = this.center;
            clone.radius = this.radius;

            return clone;
        }

        override public float Rotation { get { return 0; } set { } }
        override public Vector2 HalfSize { get { return VectorExt.V2(radius); } set { radius = value.X; } }
        override public Bound2DType Type { get { return Bound2DType.Circle; } }

        public bool outerBoundRectCheck(VectorRect outerRect, out Vector2 collisionNormal)
        {
            bool collision = false;
            collisionNormal = Vector2.Zero;

            if (center.X - radius < outerRect.X)
            {
                center.X = outerRect.X + radius;
                collision = true;
                collisionNormal.X = 1;
            }
            else if (center.X + radius > outerRect.Right)
            {
                center.X = outerRect.Right - radius;
                collision = true;
                collisionNormal.X = -1;
            }

            if (center.Y - radius < outerRect.Y)
            {
                center.Y = outerRect.Y + radius;
                collision = true;
                collisionNormal.Y = 1;
            }
            else if (center.Y + radius > outerRect.Bottom)
            {
                center.Y = outerRect.Bottom - radius;
                collision = true;
                collisionNormal.Y = -1;
            }

            return collision;
        }

        override public VectorRect Area { get { return VectorRect.FromCenterSize(center, new Vector2(radius * 2f)); } }

        public override RectangleCentered AreaCentered => new RectangleCentered(center, new Vector2(radius));

        public override string ToString()
        {
            return "Cirkle x:" + center.X.ToString() + " y:" + center.Y.ToString() + " r:" + radius.ToString();
        }
    }
}
