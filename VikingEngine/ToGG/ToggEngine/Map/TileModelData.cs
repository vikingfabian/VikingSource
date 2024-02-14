using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    abstract class AbsTileModelData
    {
        public SpriteName image;

        //abstract public void generateModel(GenerateBoardModelArgs args, List<PolygonColor> polygons);

        abstract public SquareModelType Type { get; }
    }

    class GroundTileModelData : AbsTileModelData
    {
        public GroundTileModelData(SpriteName image)
        {
            this.image = image;
        }

        //public override void generateModel(GenerateBoardModelArgs args, List<PolygonColor> polygons)
        //{
        //    polygons.Add(new Graphics.PolygonColor(
        //            args.nw, args.ne, args.sw, args.se,
        //            image, Dir4.N, args.gradient));
        //}

        override public SquareModelType Type { get { return SquareModelType.Ground; } }
    }

    class WallTileModelData : AbsTileModelData
    {
        public SpriteName imageTop;
        Graphics.Sprite topEdgeTex, topCornerTex;

        public WallTileModelData(SpriteName image, SpriteName imageTop)
        {
            this.image = image;
            this.imageTop = imageTop;

            topEdgeTex = DataLib.SpriteCollection.Get(imageTop);
            topCornerTex = topEdgeTex;

            int texH = topEdgeTex.Texture().Height;
            topEdgeTex.Source.Height /= 2;
            topEdgeTex.UpdateSourcePolygon(true);

            topCornerTex.Source.Width /= 2;
            topCornerTex.Source.Height /= 2;
            topCornerTex.Source.Y += topCornerTex.Source.Height;
            topCornerTex.UpdateSourcePolygon(true);

        }

        //public override void generateModel(GenerateBoardModelArgs args, List<PolygonColor> polygons)
        //{
        //    args.setStandardWall(args.squarePos);


        //    if (args.openTerrainW)
        //    {
        //        //W
        //        polygons.Add(new Graphics.PolygonColor(
        //            args.nwTop, args.swTop, args.nw, args.sw,
        //            image, Dir4.N, args.leftTint));
        //    }

        //    if (args.openTerrainE)
        //    {
        //        //E
        //        polygons.Add(new Graphics.PolygonColor(
        //            args.seTop, args.neTop, args.se, args.ne,
        //            image, Dir4.N, args.rightTint));
        //    }

        //    if (args.openTerrainS)
        //    {
        //        //S
        //        polygons.Add(new Graphics.PolygonColor(
        //            args.swTop, args.seTop, args.sw, args.se,
        //            image, Dir4.N, args.gradient));
        //    }
        //    //TOP
        //    {
        //        //EDGES
        //        {
        //            //N
        //            if (args.openTerrainN)
        //            {
        //                Vector3 corner1 = args.nwTop, corner2 = args.neTop;
        //                Vector3 innerLeft = args.centerTop, innerRight = args.centerTop;

        //                if (!args.openTerrainW)
        //                {
        //                    innerLeft.X = corner1.X;
        //                }
        //                if (!args.openTerrainE)
        //                {
        //                    innerRight.X = corner2.X;
        //                }

        //                polygons.Add(new Graphics.PolygonColor(
        //                       corner1, corner2, innerLeft, innerRight, topEdgeTex, args.gradient));
        //            }

        //            //S
        //            if (args.openTerrainS)
        //            {
        //                Vector3 corner1 = args.seTop, corner2 = args.swTop;
        //                Vector3 innerLeft = args.centerTop, innerRight = args.centerTop;

        //                if (!args.openTerrainW)
        //                {
        //                    innerLeft.X = corner1.X;
        //                }
        //                if (!args.openTerrainE)
        //                {
        //                    innerRight.X = corner2.X;
        //                }

        //                polygons.Add(new Graphics.PolygonColor(
        //                       corner1, corner2, innerLeft, innerRight, topEdgeTex, args.gradient));
        //            }

        //            //W
        //            if (args.openTerrainW)
        //            {
        //                Vector3 corner1 = args.swTop, corner2 = args.nwTop;
        //                Vector3 innerTop = args.centerTop, innerBottom = args.centerTop;

        //                if (!args.openTerrainN)
        //                {
        //                    innerBottom.Z = corner2.Z;
        //                }
        //                if (!args.openTerrainS)
        //                {
        //                    innerTop.Z = corner1.Z;
        //                }

        //                polygons.Add(new Graphics.PolygonColor(
        //                       corner1, corner2, innerTop, innerBottom, topEdgeTex, args.gradient));
        //            }

        //            //E
        //            if (args.openTerrainE)
        //            {
        //                Vector3 corner1 = args.neTop, corner2 = args.seTop;
        //                Vector3 innerTop = args.centerTop, innerBottom = args.centerTop;

        //                if (!args.openTerrainN)
        //                {
        //                    innerTop.Z = corner1.Z;
        //                }
        //                if (!args.openTerrainS)
        //                {
        //                    innerBottom.Z = corner2.Z;
        //                }

        //                polygons.Add(new Graphics.PolygonColor(
        //                       corner1, corner2, innerTop, innerBottom, topEdgeTex, args.gradient));
        //            }

        //            //polygons.Add(new Graphics.PolygonColor(
        //            //       args.nwTop, args.neTop, args.swTop, args.seTop,
        //            //       imageTop, Dir4.N, args.gradient));
        //        }

        //        //CORNERS
        //        {
        //            //NE
        //            if (!args.openTerrainN && !args.openTerrainE && args.openTerrainNE)
        //            {
        //                Vector3 midTop = args.neTop; midTop.X = args.centerTop.X;
        //                Vector3 midRight = args.neTop; midRight.Z = args.centerTop.Z;

        //                polygons.Add(new Graphics.PolygonColor(
        //                      midRight, args.centerTop, args.neTop, midTop, topCornerTex, args.gradient));
        //            }

        //            //SE
        //            if (!args.openTerrainS && !args.openTerrainE && args.openTerrainSE)
        //            {  
        //                Vector3 midRight = args.neTop; midRight.Z = args.centerTop.Z;
        //                Vector3 midBottom = args.seTop; midBottom.X = args.centerTop.X;

        //                polygons.Add(new Graphics.PolygonColor(
        //                      midBottom, args.centerTop, args.seTop, midRight, topCornerTex, args.gradient));
        //            }

        //            //SW
        //            if (!args.openTerrainS && !args.openTerrainW && args.openTerrainSW)
        //            {
        //                Vector3 midLeft = args.swTop; midLeft.Z = args.centerTop.Z;
        //                Vector3 midBottom = args.swTop; midBottom.X = args.centerTop.X;

        //                polygons.Add(new Graphics.PolygonColor(
        //                      midLeft, args.centerTop, args.swTop, midBottom, topCornerTex, args.gradient));
        //            }

        //            //NW
        //            if (!args.openTerrainN && !args.openTerrainW && args.openTerrainNW)
        //            {
        //                Vector3 midLeft = args.swTop; midLeft.Z = args.centerTop.Z;
        //                Vector3 midTop = args.neTop; midTop.X = args.centerTop.X;

        //                polygons.Add(new Graphics.PolygonColor(
        //                      midTop, args.centerTop, args.nwTop, midLeft, topCornerTex, args.gradient));
        //            }
        //        }
        //    }
        //}

        override public SquareModelType Type { get { return SquareModelType.Wall; } }
    }

    enum SquareModelType
    {
        Ground,
        Wall,
    }
}
