using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Physics
{
    //class OffsetBound2D : AbsBound2D
    //{
    //    public float rotationAdd;
    //    public Vector2 offset;
    //    public AbsBound2D bound;

    //    public OffsetBound2D(AbsBound2D bound, Vector2 offset, float rotationAdd)
    //    {
    //        this.bound = bound;
    //        this.offset = offset;
    //        this.rotationAdd = rotationAdd;
    //    }

    //    public void update(Vector2 center, float rotation)
    //    {
    //        bound.update(center + VectorExt.RotateVector(offset, rotation), rotation + rotationAdd);
    //        //bound.Center = center + VectorExt.RotateVector(offset, rotation);
    //        //bound.Rotation = rotation + rotationAdd;
    //    }

    //    public float ExtremeRadius { get { return bound.ExtremeRadius; } }
    //    public float InnerCirkleRadius
    //    { get { return bound.InnerCirkleRadius; } }
    //    public float Rotation { get { return bound.Rotation; } set { } }
    //    public Bound2DType Type { get { return bound.Type; } }

    //    public bool Intersect(AbsBound2D otherBound)
    //    {
    //        return bound.Intersect(otherBound);
    //    }
    //    public Collision2D Intersect2(AbsBound2D otherBound)
    //    {
    //        return bound.Intersect2(otherBound);
    //    }
    //    public bool Intersect(Vector2 point)
    //    {
    //        return bound.Intersect(point);
    //    }
    //    public bool VerticeIntersect(Vector2[] vertices)
    //    {
    //        return bound.VerticeIntersect(vertices);
    //    }
    //    public Vector2[] Vertices()
    //    {
    //        return bound.Vertices();
    //    }

    //    public Vector2 Center { get { return bound.Center; } set { bound.Center = value; } }
    //    public Vector2 HalfSize { get { return bound.HalfSize; } set { bound.HalfSize = value; } }
    //    public VectorRect Area { get { return bound.Area; } }
    //}
}

