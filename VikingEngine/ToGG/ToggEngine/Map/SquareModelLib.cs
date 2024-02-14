using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    static class SquareModelLib
    {
        public const float TerrainY_MoveArrow = 0.05f;
        public const float TerrainY_MoveArrowLow = TerrainY_MoveArrow - 0.02f;

        public const float TerrainY_SelectionFrame = 0.02f;
        public const float TerrainY_UnitShadow = 0.01f;
        public const float TerrainY_Loot = 0.005f;
        public const float TerrainY_OverlayTile = 0.004f;

        public const float TerrainY_GroundDetails3 = 0.0014f;
        public const float TerrainY_GroundDetails2 = 0.0012f;
        public const float TerrainY_GroundDetails1 = 0.001f;
        public const float TerrainY_Ground = 0;

        public const float WallHeight = 0.5f;
        const float BlackWallYDiff = -0.01f;

        public static SpriteName SquareTextureVariant(SpriteName[] textures, TileTextureVariantType variationType, PcgRandom rnd, IntVector2 pos)
        {
            switch (variationType)
            {
                case TileTextureVariantType.None:
                    return textures[0];
                case TileTextureVariantType.Random:
                    {
                        int ix = rnd.Int_HighToLowProbabilityHalf(textures.Length);
                        return textures[ix];
                    }
                case TileTextureVariantType.TwoByTwo:
                    {
                        pos.X %= 2;
                        pos.Y %= 2;
                        int ix = pos.X + pos.Y * 2;
                        return textures[ix];
                    }
            }
            throw new NotImplementedException();
        }

        public static void FlatGroundModel(GenerateBoardModelArgs args, SpriteName[] textures, TileTextureVariantType variationType, PcgRandom rnd)
        {
            FlatGroundModel(args, SquareTextureVariant(textures, variationType, rnd, args.squarePos));
        }
        public static void FlatGroundModel(GenerateBoardModelArgs args, SpriteName image)
        {
            args.polygons.Add(new Graphics.PolygonColor(
                    args.nw, args.ne, args.sw, args.se,
                    image, Dir4.N, args.gradient));
        }

        public static void WallModel(GenerateBoardModelArgs args,
            SpriteName[] textures, TileTextureVariantType variationType, PcgRandom rnd, Graphics.Sprite topEdgeTex, Graphics.Sprite topCornerTex)
        {
            SpriteName tex = SquareTextureVariant(textures, variationType, rnd, args.squarePos);
            WallModel(args, tex, tex, topEdgeTex, topCornerTex);
        }

        public static void WallModel(GenerateBoardModelArgs args, 
            SpriteName frontWallTex, SpriteName sideWallTex, Graphics.Sprite topEdgeTex, Graphics.Sprite topCornerTex)
        {
            args.setStandardWall(args.squarePos);

            if (args.openTerrainW)
            {
                //W
                args.wallWPoly = new Graphics.PolygonColor(
                    args.nwTop, args.swTop, args.nw, args.sw,
                    sideWallTex, Dir4.N, args.leftTint);
                args.polygons.Add(args.wallWPoly);
            }

            if (args.openTerrainE)
            {
                //E
                args.wallEPoly = new Graphics.PolygonColor(
                    args.seTop, args.neTop, args.se, args.ne,
                    sideWallTex, Dir4.N, args.rightTint);
                args.polygons.Add(args.wallEPoly);
            }

            if (args.openTerrainS)
            {
                //S
                args.wallSPoly = new Graphics.PolygonColor(
                    args.swTop, args.seTop, args.sw, args.se,
                    frontWallTex, Dir4.N, args.gradient);
                args.polygons.Add(args.wallSPoly);
            }

            //TOP
            {
                //EDGES
                {
                    //N
                    if (args.openTerrainN)
                    {
                        Vector3 corner1 = args.nwTop, corner2 = args.neTop;
                        Vector3 innerLeft = args.centerTop, innerRight = args.centerTop;

                        if (!args.openTerrainW)
                        {
                            innerLeft.X = corner1.X;
                        }
                        if (!args.openTerrainE)
                        {
                            innerRight.X = corner2.X;
                        }

                        args.polygons.Add(new Graphics.PolygonColor(
                               corner1, corner2, innerLeft, innerRight, topEdgeTex, args.gradient));
                    }

                    //S
                    if (args.openTerrainS)
                    {
                        Vector3 corner1 = args.seTop, corner2 = args.swTop;
                        Vector3 innerLeft = args.centerTop, innerRight = args.centerTop;

                        if (!args.openTerrainE)
                        {
                            innerLeft.X = corner1.X;
                        }
                        if (!args.openTerrainW)
                        {
                            innerRight.X = corner2.X;
                        }

                        args.polygons.Add(new Graphics.PolygonColor(
                               corner1, corner2, innerLeft, innerRight, topEdgeTex, args.gradient));
                    }

                    //W
                    if (args.openTerrainW)
                    {
                        Vector3 corner1 = args.swTop, corner2 = args.nwTop;
                        Vector3 innerTop = args.centerTop, innerBottom = args.centerTop;

                        if (!args.openTerrainN)
                        {
                            innerBottom.Z = corner2.Z;
                        }
                        if (!args.openTerrainS)
                        {
                            innerTop.Z = corner1.Z;
                        }

                        args.polygons.Add(new Graphics.PolygonColor(
                               corner1, corner2, innerTop, innerBottom, topEdgeTex, args.gradient));
                    }

                    //E
                    if (args.openTerrainE)
                    {
                        Vector3 corner1 = args.neTop, corner2 = args.seTop;
                        Vector3 innerTop = args.centerTop, innerBottom = args.centerTop;

                        if (!args.openTerrainN)
                        {
                            innerTop.Z = corner1.Z;
                        }
                        if (!args.openTerrainS)
                        {
                            innerBottom.Z = corner2.Z;
                        }

                        args.polygons.Add(new Graphics.PolygonColor(
                               corner1, corner2, innerTop, innerBottom, topEdgeTex, args.gradient));
                    }
                }

                //CORNERS
                {
                    //NE
                    if (!args.openTerrainN && !args.openTerrainE && args.openTerrainNE)
                    {
                        Vector3 midTop = args.neTop; midTop.X = args.centerTop.X;
                        Vector3 midRight = args.neTop; midRight.Z = args.centerTop.Z;

                        args.polygons.Add(new Graphics.PolygonColor(
                              midRight, args.centerTop, args.neTop, midTop, topCornerTex, args.gradient));
                    }

                    //SE
                    if (!args.openTerrainS && !args.openTerrainE && args.openTerrainSE)
                    {
                        Vector3 midRight = args.neTop; midRight.Z = args.centerTop.Z;
                        Vector3 midBottom = args.seTop; midBottom.X = args.centerTop.X;

                        args.polygons.Add(new Graphics.PolygonColor(
                              midBottom, args.centerTop, args.seTop, midRight, topCornerTex, args.gradient));
                    }

                    //SW
                    if (!args.openTerrainS && !args.openTerrainW && args.openTerrainSW)
                    {
                        Vector3 midLeft = args.swTop; midLeft.Z = args.centerTop.Z;
                        Vector3 midBottom = args.swTop; midBottom.X = args.centerTop.X;

                        args.polygons.Add(new Graphics.PolygonColor(
                              midLeft, args.centerTop, args.swTop, midBottom, topCornerTex, args.gradient));
                    }

                    //NW
                    if (!args.openTerrainN && !args.openTerrainW && args.openTerrainNW)
                    {
                        Vector3 midTop = args.nwTop; midTop.X = args.centerTop.X;
                        Vector3 midLeft = args.nwTop; midLeft.Z = args.centerTop.Z;

                        args.polygons.Add(new Graphics.PolygonColor(
                           midTop, args.centerTop, args.nwTop, midLeft, topCornerTex, args.gradient));

                        //args.polygons.Add(new Graphics.PolygonColor(
                        //    VectorExt.AddY(args.nwTop, 0.05f) , VectorExt.AddY(args.neTop, 0.05f), VectorExt.AddY(args.swTop, 0.05f), VectorExt.AddY(args.seTop, 0.05f),   SpriteName.birdLobbyMenuButton, Dir4.N, args.gradient));
                    }
                }

                //MID WALL BLACK SURFACE
               
                args.polygons.Add(new Graphics.PolygonColor(
                    VectorExt.AddY(args.nwTop, BlackWallYDiff),
                    VectorExt.AddY(args.neTop, BlackWallYDiff),
                    VectorExt.AddY(args.swTop, BlackWallYDiff),
                    VectorExt.AddY(args.seTop, BlackWallYDiff),
                sideWallTex, Dir4.N, Color.Black));
               
            }
        }

        public static void WallFloorEdge(GenerateBoardModelArgs args, SpriteName[] textures)
        {
            WallFloorEdge(args, SquareTextureVariant(textures, TileTextureVariantType.Random, args.rnd, args.squarePos));
        }

        public static void WallFloorEdge(GenerateBoardModelArgs args, SpriteName image)
        {
            const float AdjustIntoWall = 0.01f;

            if (args.openTerrainS)
            {
                var walledge1 = args.sw;
                walledge1.Y = TerrainY_GroundDetails1;
                walledge1.Z -= AdjustIntoWall;

                var walledge2 = args.se;
                walledge2.Y = TerrainY_GroundDetails1;
                walledge2.Z -= AdjustIntoWall;

                args.overlaypolygons.Add(new Graphics.PolygonColor(
                   walledge1, walledge2, VectorExt.AddZ(walledge1, 0.5f), VectorExt.AddZ(walledge2, 0.5f),
                   image, Dir4.N, args.gradient));
            }

            if (args.openTerrainW)
            {
                var walledge1 = args.nw;
                walledge1.Y = TerrainY_GroundDetails2;
                walledge1.X += AdjustIntoWall;

                var walledge2 = args.sw;
                walledge2.Y = TerrainY_GroundDetails2;
                walledge2.X += AdjustIntoWall;

                args.overlaypolygons.Add(new Graphics.PolygonColor(
                   walledge1, walledge2, VectorExt.AddX(walledge1, -0.5f), VectorExt.AddX(walledge2, -0.5f),
                   image, Dir4.N, args.gradient));
            }

            if (args.openTerrainE)
            {
                var walledge1 = args.se;
                walledge1.Y = TerrainY_GroundDetails3;
                walledge1.X -= AdjustIntoWall;

                var walledge2 = args.ne;
                walledge2.Y = TerrainY_GroundDetails3;
                walledge2.X -= AdjustIntoWall;

                args.overlaypolygons.Add(new Graphics.PolygonColor(
                   walledge1, walledge2, VectorExt.AddX(walledge1, 0.5f), VectorExt.AddX(walledge2, 0.5f),
                   image, Dir4.N, args.gradient));
            }
        }

        public static void FullWallDecal_S(GenerateBoardModelArgs args, SpriteName image)
        {
            if (args.openTerrainS)
            {
                args.wallSPoly.Move(VectorExt.V3FromZ(0.02f));
                args.wallSPoly.setSprite(image, Dir4.N);
                args.polygons.Add(args.wallSPoly);
            }
        }

        public static void GroundBillboardModel(GenerateBoardModelArgs args, SpriteName sprite, IntVector2 spriteSz, float scale, Vector2 centerDiffPos)
        {
            Graphics.Sprite file = DataLib.SpriteCollection.Get(sprite);

            var poly = toggLib.CamFacingPolygon(VectorExt.AddXZ(args.center, centerDiffPos),
                0.5f * scale * spriteSz.Vec, file, args.gradient);
            args.polygons.Add(poly);
        }
    }
}
