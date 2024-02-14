using System;
using Microsoft.Xna.Framework;

namespace VikingEngine.Physics
{
    abstract class AbsBound2D
    {
        public float rotationAdd = 0;
        public Vector2 offset = Vector2.Zero;
        protected Vector2[] vertices = null;

        virtual public void update(Vector2 center, float rotation = 0)
        {
            if (offset.X != 0 || offset.Y != 0)
            {
                center += VectorExt.RotateVector(offset, rotation);
            }
            baseUpdate(center, rotation + rotationAdd);
        }
        abstract public void baseUpdate(Vector2 center, float rotation);
        
        /// <summary>
        /// The radius of a square that surronds the whole bound shape, this is to make a quick and CPU cheap coll check
        /// </summary>
        abstract public float ExtremeRadius { get; }
        /// <summary>
        /// The radius of a cirkle that fits inside the shape, this is to make a quick and CPU cheap coll check
        /// </summary>
        abstract public float InnerCirkleRadius { get; }
        abstract public float Rotation { get; set; }
        /// <summary>
        /// Check if this bound shape collides with another
        /// </summary>
        /// <returns>
        /// A boolean flag indicating if the bounds intersect.
        /// </returns>
        abstract public bool Intersect(AbsBound2D otherBound);
        abstract public bool Intersect(Vector2 point);

        abstract public Collision2D Intersect2(AbsBound2D otherBound);

        abstract public bool VerticeIntersect(Vector2[] vertices);
                
        abstract public Vector2[] Vertices();

        abstract public Bound2DType Type { get; }

        abstract public Vector2 Center { get; set; }
        abstract public Vector2 HalfSize { get; set; }
        abstract public VectorRect Area { get; }
        abstract public RectangleCentered AreaCentered { get; }
        
        abstract public AbsBound2D Clone();

        public bool AsynchCollect(AbsBound2D otherBound)
        {
            float r = (ExtremeRadius + otherBound.ExtremeRadius) * 1.2f;
            float l = (otherBound.Center - Center).Length();

            return l < r;
        }

        public float ExtremeRadiusDistance(AbsBound2D otherBound)
        {
            float r = ExtremeRadius + otherBound.ExtremeRadius;
            float l = (otherBound.Center - Center).Length();

            if (l > r)
            {
                return l - r;
            }
            else
            {
                return 0;
            }
        }
    }
    
    enum Bound2DType
    {
        Rectangle,
        RectangleRotated,
        Circle,
        Custom,
    }
}
