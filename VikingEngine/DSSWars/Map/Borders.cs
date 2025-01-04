using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
    class Borders
    {
        Graphics.GeneratedObjColor currentModel = null;
        Graphics.GeneratedObjColor nextModel = null;
        //VoxelObjEffectBasicColor modelEffect = new VoxelObjEffectBasicColor(LoadedTexture.WhiteArea);

        public Borders()
        {
        }

        static readonly Color UncoloredEdge = Color.Black;
        public void quedEvent()
        {
            
            const float TileHalfScale = 0.5f;
            const float UncoloredEdgeWidth = 0.05f;
            const float ColoredEdgeWidth = 0.16f;

            const float HeightAboveGround_Color = 0.05f;
            const float HeightAboveGround_Uncolored = HeightAboveGround_Color + 0.03f;


            Tile t;
            Vector3 cornerNW = Vector3.Zero, cornerNE = Vector3.Zero, cornerSW = Vector3.Zero, cornerSE = Vector3.Zero, innerCornerNW = Vector3.Zero, innerCornerNE = Vector3.Zero, innerCornerSW = Vector3.Zero, innerCornerSE = Vector3.Zero;

            Sprite tex = Sprite.FromeName(SpriteName.WhiteArea_LFtiles);
            List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>();

            for (int y = 0; y < DssRef.world.Size.Y; ++y)
            {
                for (int x = 0; x < DssRef.world.Size.X; ++x)
                {
                    t = DssRef.world.tileGrid.Get(x, y);//.GetTile(x, y);
                    if (t.HasBorderImage())
                    {
                        Vector3 tileCenter = new Vector3(x, t.GroundY() + HeightAboveGround_Color, y);
                        float coloredY = t.GroundY() + HeightAboveGround_Color;
                        float uncoloredY = t.GroundY() + HeightAboveGround_Uncolored;


                        //TileSpecial_Border borders = t.Special as TileSpecial_Border;

                        //foreach (BorderDir dir in borders.BorderDirList)
                        //{
                        if (t.BorderCount > 0)
                        {
                            for (int dir = 0; dir < 4; ++dir)
                            {
                                int borderRegion = t.GetBorder(dir);
                                if (borderRegion >= 0 && GameObject.City.Get(borderRegion).faction != t.City().faction)
                                {
                                    //if (dir.toOtherRegion >= 0 && GameObject.City.Get(dir.toOtherRegion).Faction != t.City.Faction)
                                    //{

                                    switch (dir)
                                    {
                                        default: //N
                                            //Outer edge, uncolored
                                            cornerNW.X = tileCenter.X - TileHalfScale;
                                            cornerNW.Y = uncoloredY;
                                            cornerNW.Z = tileCenter.Z - TileHalfScale;

                                            cornerNE.X = tileCenter.X + TileHalfScale;
                                            cornerNE.Y = uncoloredY;
                                            cornerNE.Z = tileCenter.Z - TileHalfScale;

                                            cornerSE = cornerNE;
                                            cornerSE.Z += UncoloredEdgeWidth;

                                            cornerSW = cornerNW;
                                            cornerSW.Z += UncoloredEdgeWidth;

                                            //Inner edge, colored and below
                                            innerCornerNW = cornerSW;
                                            innerCornerNW.Y = coloredY;

                                            innerCornerNE = cornerSE;
                                            innerCornerNE.Y = coloredY;

                                            innerCornerSW = cornerSW;
                                            innerCornerSW.Z += ColoredEdgeWidth;

                                            innerCornerSE = cornerSE;
                                            innerCornerSE.Z += ColoredEdgeWidth;
                                            break;
                                        case 1: //E
                                            //Outer edge, uncolored
                                            cornerNW.X = tileCenter.X + TileHalfScale;
                                            cornerNW.Y = uncoloredY;
                                            cornerNW.Z = tileCenter.Z - TileHalfScale;

                                            cornerNE.X = tileCenter.X + TileHalfScale;
                                            cornerNE.Y = uncoloredY;
                                            cornerNE.Z = tileCenter.Z + TileHalfScale;

                                            cornerSE = cornerNE;
                                            cornerSE.X -= UncoloredEdgeWidth;

                                            cornerSW = cornerNW;
                                            cornerSW.X -= UncoloredEdgeWidth;

                                            //Inner edge, colored and below
                                            innerCornerNW = cornerSW;
                                            innerCornerNW.Y = coloredY;

                                            innerCornerNE = cornerSE;
                                            innerCornerNE.Y = coloredY;

                                            innerCornerSW = cornerSW;
                                            innerCornerSW.X -= ColoredEdgeWidth;

                                            innerCornerSE = cornerSE;
                                            innerCornerSE.X -= ColoredEdgeWidth;
                                            break;

                                        case 2: //S
                                            //Outer edge, uncolored
                                            cornerNW.X = tileCenter.X + TileHalfScale;
                                            cornerNW.Y = uncoloredY;
                                            cornerNW.Z = tileCenter.Z + TileHalfScale;

                                            cornerNE.X = tileCenter.X - TileHalfScale;
                                            cornerNE.Y = uncoloredY;
                                            cornerNE.Z = tileCenter.Z + TileHalfScale;

                                            cornerSE = cornerNE;
                                            cornerSE.Z -= UncoloredEdgeWidth;

                                            cornerSW = cornerNW;
                                            cornerSW.Z -= UncoloredEdgeWidth;

                                            //Inner edge, colored and below
                                            innerCornerNW = cornerSW;
                                            innerCornerNW.Y = coloredY;

                                            innerCornerNE = cornerSE;
                                            innerCornerNE.Y = coloredY;

                                            innerCornerSW = cornerSW;
                                            innerCornerSW.Z -= ColoredEdgeWidth;

                                            innerCornerSE = cornerSE;
                                            innerCornerSE.Z -= ColoredEdgeWidth;
                                            break;

                                        case 3: //W
                                            //Outer edge, uncolored
                                            cornerNW.X = tileCenter.X - TileHalfScale;
                                            cornerNW.Y = uncoloredY;
                                            cornerNW.Z = tileCenter.Z + TileHalfScale;

                                            cornerNE.X = tileCenter.X - TileHalfScale;
                                            cornerNE.Y = uncoloredY;
                                            cornerNE.Z = tileCenter.Z - TileHalfScale;

                                            cornerSE = cornerNE;
                                            cornerSE.X += UncoloredEdgeWidth;

                                            cornerSW = cornerNW;
                                            cornerSW.X += UncoloredEdgeWidth;

                                            //Inner edge, colored and below
                                            innerCornerNW = cornerSW;
                                            innerCornerNW.Y = coloredY;

                                            innerCornerNE = cornerSE;
                                            innerCornerNE.Y = coloredY;

                                            innerCornerSW = cornerSW;
                                            innerCornerSW.X += ColoredEdgeWidth;

                                            innerCornerSE = cornerSE;
                                            innerCornerSE.X += ColoredEdgeWidth;
                                            break;
                                    }

                                    polygons.Add(new Graphics.PolygonColor(new Vector3[]
                                        {
                                            cornerNW,//nw,
                                            cornerNE,//ne,  
                                            cornerSW,//sw,
                                            cornerSE,//se,
                                        },
                                        tex, UncoloredEdge));
                                    polygons.Add(new Graphics.PolygonColor(new Vector3[]
                                        {
                                            innerCornerNW,//nw,
                                            innerCornerNE,//ne,  
                                            innerCornerSW,//sw,
                                            innerCornerSE,//se,
                                        },
                                        tex, t.FactionColor()));
                                    //break;
                                }

                            }
                        }
                    }
                }
            }//end for Y

            nextModel = new Graphics.GeneratedObjColor(new Graphics.PolygonsAndTrianglesColor(
                polygons, null), LoadedTexture.SpriteSheet, false);

            //nextModel.Effect = modelEffect;
            //nextModel.BuildFromPolygons(new PolygonsAndTrianglesColor(polygons, null), new List<int> { polygons.Count }, LoadedTexture.WhiteArea);
        }

        public void SetNewModel()
        {
            currentModel?.DeleteMe();
            nextModel.AddToRender(DrawGame.TerrainLayer);
            currentModel = nextModel;
        }

        public void Draw(int cameraIndex)
        {
            if (currentModel != null)
            {
                currentModel.Draw(cameraIndex);
            }
        }

    }
}
