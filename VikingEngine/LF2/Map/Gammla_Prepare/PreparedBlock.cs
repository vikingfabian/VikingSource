using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.Map
{
    interface IPreparedBlock
    {
        bool HaveSurface { get; }
        bool ShadowSide { get; }
        void AddShadowValue(float value);
        
    }
    struct EmptyBlock : IPreparedBlock
    {
        public bool HaveSurface { get { return false; } }
        public bool ShadowSide { get { return false; } }
        public void AddShadowValue(float value)
        { }
    }
    //struct UsingFaces
    //{
    //    public bool top;
    //    public bool bottom;
    //    public bool front;
    //    public bool back;
    //    public bool left;
    //    public bool right;

    //    public UsingFaces(bool top, bool bottom, bool front, bool back, bool left, bool right)
    //    {
    //        this.top = top;
    //        this.bottom = bottom;
    //        this.front = front;
    //        this.back = back;
    //        this.left = left;
    //        this.right = right;
    //    }

    //    public bool ShadowSide
    //    {
    //        get { return bottom || back || left; }
    //    }
    //}
    //class PreparedBlock : IPreparedBlock
    //{
    //    public bool HaveSurface { get { return true; } }
    //    bool shadowSide;
    //    public bool ShadowSide { get { return shadowSide; } }
    //    List<PreparedFace> preparedFaces;
    //    public IntVector3 Position;
    //    public void GenerateMesh(Graphics.PolygonsAndTrianglesColor polys)
    //    {
    //        foreach (PreparedFace face in preparedFaces)
    //        {
    //            polys.Polygons.Add(new Graphics.PolygonColor(face, Position));
    //        }
    //    }
    //    public void ResetDarkness()
    //    {
    //        for (int i = 0; i < preparedFaces.Count; i++)
    //        {
    //            preparedFaces[i] = preparedFaces[i].ResetDarkness();
    //        }
    //    }
    //    public void AddShadowValue(float value)
    //    {
           
    //        for (int i = 0; i < preparedFaces.Count; i++ )
    //        {
    //            if (preparedFaces[i].faceType != Data.CubeFace.Front && preparedFaces[i].faceType != Data.CubeFace.Left)
    //            {
    //                PreparedFace face = preparedFaces[i];
    //                face.darkness += value * (1 - face.darkness);
    //                preparedFaces[i] = face;
    //            }
    //        }
    //    }
    //    public WorldPosition WorldPos { get { return new WorldPosition(Position); } }
    //    public PreparedBlock(List<PreparedFace> faces, bool shadowSide, IntVector3 position)
    //    {
    //        preparedFaces = faces;
    //        this.shadowSide = shadowSide;
    //        this.Position= position;

    //    }
    //}
    
}
