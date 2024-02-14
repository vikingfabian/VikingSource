using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using VikingEngine.Voxels;

namespace VikingEngine.LootFest.Map
{
    struct PreparedFace
    {
        const float ShadowSideDarkness = 0.5f;
        const float LightSideDarkness = 0.15f;
        const float TopSideDarkness = 0;

        static readonly float[] FaceStartDarkness = new float[] 
        { 
            TopSideDarkness, 0,  //Top, Bottom,
            LightSideDarkness, 
            ShadowSideDarkness,//Front, Back,
            ShadowSideDarkness, 
            LightSideDarkness//Left, Right,
        };

        const int NumDarknessLevels = 50;
        const float SubColorPerLevel = 1f / NumDarknessLevels;
        static Color[] DarknessLevels = new Color[NumDarknessLevels];


        public const CubeFace ShadowSide1 = CubeFace.Znegative;
        public const CubeFace ShadowSide2 = CubeFace.Xpositive;
        public CubeFace faceType;
        public int tileIx;

        public float darkness;
        //CORNER DARKNESS for SSAO
        public IPreparedFaceCorners CornerDarkness;

        public PreparedFace(CubeFace type, byte material, IntVector3 pos)
        {
            faceType = type;
            CornerDarkness = PreparedFaceEmptyCorners.GetEmpty;
            darkness = FaceStartDarkness[(int)faceType];
            if (type != CubeFace.Ypositive)
                tileIx = Data.BlockTextures.Materials[material].SideTiles.GetRandomTile(pos);
            else
                tileIx = Data.BlockTextures.Materials[material].TopTiles.GetRandomTile(pos);
        }

        public PreparedFace(CubeFace type, byte material, WorldPosition wp)
            : this(type, material, wp.LocalBlockGrindex)
        {
            //Create corner darkness, check sourronding blocks that would create an inward corner
            //right now, only care about the visual faces
            bool runCornerCheck = true;
            switch (type)
            {
                case CubeFace.Ypositive:
                    ++wp.WorldGrindex.Y;
                    break;
                case CubeFace.Zpositive:
                    ++wp.WorldGrindex.Z;
                    break;
                case CubeFace.Xpositive:
                    ++wp.WorldGrindex.X;
                    break;
                default:
                    runCornerCheck = false;
                    break;
            }
            if (runCornerCheck)
            {
                WorldPosition c0 = wp;
                WorldPosition c1 = wp;
                WorldPosition c2 = wp;
                WorldPosition c3 = wp;

                switch (type)
                {
                    case CubeFace.Ypositive:
                        ++wp.WorldGrindex.Y;
                        ++c0.WorldGrindex.Z;
                        ++c1.WorldGrindex.X;
                        --c2.WorldGrindex.Z;
                        --c3.WorldGrindex.X;
                        break;
                    case CubeFace.Zpositive:
                        --wp.WorldGrindex.Z;
                        ++c0.WorldGrindex.Y;
                        --c1.WorldGrindex.X;
                        --c2.WorldGrindex.Y;
                        ++c3.WorldGrindex.X;
                        break;
                    case CubeFace.Xpositive:
                        ++wp.WorldGrindex.X;
                        ++c0.WorldGrindex.Y;
                        ++c1.WorldGrindex.Z;
                        --c2.WorldGrindex.Y;
                        --c3.WorldGrindex.Z;
                        break;
                }

                byte numCorners = 0;
                bool side0 = cornerCheck(ref c0, ref numCorners);
                bool side1 = cornerCheck(ref c1, ref numCorners);
                bool side2 = cornerCheck(ref c2, ref numCorners);
                bool side3 = cornerCheck(ref c3, ref numCorners);
                if (numCorners > 0)
                {
                    PreparedFaceCorners corners = new PreparedFaceCorners();
                    if (side0)
                    {
                        corners.Corner2++;
                        corners.Corner3++;
                    }
                    if (side1)
                    {
                        corners.Corner3++;
                        corners.Corner1++;
                    }
                    if (side2)
                    {
                        corners.Corner0++;
                        corners.Corner1++;
                    }
                    if (side3)
                    {
                        corners.Corner2++;
                        corners.Corner0++;
                    }
                    CornerDarkness = corners;
                }
            }

        }
        bool cornerCheck(ref WorldPosition wp, ref byte numCorners)
        {
            if (wp.BlockHasMaterial())
            {
                return false;
            }
            numCorners++;
            return true;
        }
        public PreparedFace ResetDarkness()
        {
            darkness = FaceStartDarkness[(int)faceType];
            return this;
        }
        public void AddDarkness(float value)
        {
            darkness += value;
        }
        public FaceCornerColorYS GetDarknessColor()
        {
            if (CornerDarkness.Empty)
            {
                return new FaceCornerColorYS(1 - darkness, faceType == CubeFace.Xpositive);
            }
            else
            {
                return new FaceCornerColorYS(1 - darkness, faceType == CubeFace.Xpositive,
                    CornerDarkness.C0, CornerDarkness.C1, CornerDarkness.C2, CornerDarkness.C3);
            }
        }
        public static void Init()
        {
            for (int i = 0; i < NumDarknessLevels; i++)
            {
                float col = 1 - i * SubColorPerLevel;
                DarknessLevels[i] = new Color(col, col, col);
            }
        }
    }
}
