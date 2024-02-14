using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class GenerateBoardModelArgs
    {
        public const float WallFrontLeanPerUnit = 0.32f;
        const float WallFrontLean = WallFrontLeanPerUnit * Map.SquareModelLib.WallHeight;
        const float WallBackLean = 0.24f;
        public const float WallSideLean = 0.1f;

        public PcgRandom rnd = new PcgRandom();
        public BoardSquareContent squareContent;
        public IntVector2 squarePos;
        public Color gradient;
        public Color leftTint, rightTint;

        public Vector3 center = Vector3.Zero;
        public Vector3 nw = Vector3.Zero;
        public Vector3 ne = Vector3.Zero;
        public Vector3 sw = Vector3.Zero;
        public Vector3 se = Vector3.Zero;

        public Vector3 centerTop = Vector3.Zero;
        public Vector3 nwTop = Vector3.Zero;
        public Vector3 neTop = Vector3.Zero;
        public Vector3 swTop = Vector3.Zero;
        public Vector3 seTop = Vector3.Zero;

        public bool openTerrainN, openTerrainS, openTerrainW, openTerrainE;
        public bool openTerrainNW, openTerrainNE, openTerrainSW, openTerrainSE;

        public Graphics.PolygonColor wallWPoly, wallSPoly, wallEPoly;
        public List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>();
        public List<Graphics.PolygonColor> overlaypolygons = new List<Graphics.PolygonColor>();
        public List<ToggEngine.AbsParticleEmitter> torches = new List<ToggEngine.AbsParticleEmitter>();

        public void setTileX(int x)
        {
            squarePos.X = x;
            center.X = x;
            nw.X = x - 0.5f;
            ne.X = x + 0.5f;
            sw.X = x - 0.5f;
            se.X = x + 0.5f;

            rnd.SetSeed(squarePos.X * 3 + squarePos.Y * 7 + toggRef.Seed); 
        }
        public void setTileY(int y)
        {
            squarePos.Y = y;
            int boardH = toggRef.board.Size.Y;
            float gray = 1f - ((float)(boardH - y) / boardH) * 0.1f;
            gradient = ColorExt.GrayScale(gray);

            float darkGray = gray * 0.92f;
            leftTint = new Color(darkGray, darkGray, gray);
            rightTint = new Color(gray, darkGray, darkGray);

            center.Z = y;
            nw.Z = y - 0.5f;
            ne.Z = y - 0.5f;
            sw.Z = y + 0.5f;
            se.Z = y + 0.5f;
        }

        public void setStandardWall(IntVector2 pos)
        {
            openTerrainN = isOpenTerrain(pos, Dir8.N);
            openTerrainS = isOpenTerrain(pos, Dir8.S);
            openTerrainW = isOpenTerrain(pos, Dir8.W);
            openTerrainE = isOpenTerrain(pos, Dir8.E);
            openTerrainNW = isOpenTerrain(pos, Dir8.NW);
            openTerrainNE = isOpenTerrain(pos, Dir8.NE);
            openTerrainSW = isOpenTerrain(pos, Dir8.SW);
            openTerrainSE = isOpenTerrain(pos, Dir8.SE);

            //float height = 0.5f;
            float frontZadd = openTerrainS ? -WallFrontLean : 0f;
            float backZadd = openTerrainN ? WallBackLean : 0f;

            float leftXadd = openTerrainW ? WallSideLean : 0f;
            float rightXadd = openTerrainE ? -WallSideLean : 0f;

            nwTop = nw + new Vector3(leftXadd, Map.SquareModelLib.WallHeight, backZadd);
            neTop = ne + new Vector3(rightXadd, Map.SquareModelLib.WallHeight, backZadd);
            swTop = sw + new Vector3(leftXadd, Map.SquareModelLib.WallHeight, frontZadd);
            seTop = se + new Vector3(rightXadd, Map.SquareModelLib.WallHeight, frontZadd);


            //Cross sections
            {
                if (openTerrainW)
                {
                    if (!openTerrainNW)
                    {
                        nwTop.Z = nw.Z - WallFrontLean;
                    }
                    if (!openTerrainSW)
                    {
                        swTop.Z = sw.Z + WallBackLean;
                    }
                }
                else
                {
                    if (!openTerrainN && openTerrainNW)
                    {
                        nwTop.X = nw.X - WallSideLean;
                        nwTop.Z = nw.Z + WallBackLean;
                    }
                    if (!openTerrainS && openTerrainSW)
                    {
                        swTop.X = sw.X - WallSideLean;
                        swTop.Z = sw.Z - WallFrontLean;
                    }
                }

                if (openTerrainE)
                {
                    if (!openTerrainNE)
                    {
                        neTop.Z = ne.Z - WallFrontLean;
                    }
                    if (!openTerrainSE)
                    {
                        seTop.Z = se.Z + WallBackLean;
                    }
                }
                else
                {
                    if (!openTerrainN && openTerrainNE)
                    {
                        neTop.X = ne.X - WallSideLean;
                        neTop.Z = ne.Z + WallBackLean;
                    }
                    if (!openTerrainS && openTerrainSE)
                    {
                        seTop.X = se.X - WallSideLean;
                        seTop.Z = se.Z - WallFrontLean;
                    }
                }

                if (openTerrainN)
                {
                    if (!openTerrainNW)
                    {
                        nwTop.X = nw.X - WallSideLean;
                    }
                    if (!openTerrainNE)
                    {
                        neTop.X = ne.X + WallSideLean;
                    }
                }
                else
                {
                    if (!openTerrainW && openTerrainNW)
                    {
                        nwTop.X = nw.X + WallSideLean;
                        nwTop.Z = nw.Z + WallBackLean;
                    }
                    if (!openTerrainE && openTerrainNE)
                    {
                        neTop.X = ne.X - WallSideLean;
                        neTop.Z = ne.Z + WallBackLean;
                    }
                }

                if (openTerrainS)
                {
                    if (!openTerrainSW)
                    {
                        swTop.X = sw.X - WallSideLean;
                    }
                    if (!openTerrainSE)
                    {
                        seTop.X = se.X + WallSideLean;
                    }
                }
                else
                {
                    if (!openTerrainW && openTerrainSW)
                    {
                        swTop.X = sw.X + WallSideLean;
                        swTop.Z = sw.Z - WallFrontLean;
                    }
                    if (!openTerrainE && openTerrainSE)
                    {
                        seTop.X = se.X - WallSideLean;
                        seTop.Z = se.Z - WallFrontLean;
                    }
                }
            }


            centerTop = center;
            centerTop.Y += Map.SquareModelLib.WallHeight;
        }

        bool isOpenTerrain(IntVector2 pos, Dir8 dir)
        {
            var sq = toggRef.board.adjacentSquare(pos, dir);
            if (sq != null && sq.Square.ModelType != SquareModelType.Wall)
            {
                return true;
            }
            return false;
        }
    }
}
