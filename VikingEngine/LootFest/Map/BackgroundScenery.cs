using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Map
{
    /// <summary>
    /// Mountains shown around the player
    /// </summary>
    class BackgroundScenery
    {
        const float SkyRadius =  3000;
        const float Height = 2000;
        const int CirkularDivitions = 32;
        const int HeightDivitions = CirkularDivitions;

        static readonly Color TopCol = new Color(13, 112, 153);//45, 88, 123);
        static readonly Color MidCol = new Color(53, 160, 205);//new Color(137, 202, 229);
        static readonly Color BottomCol = new Color(218, 240, 249);

        Color cloudBottomCol = Color.White;
        Color frontCol = Color.LightGray;
        Color cloudBackCol = Color.LightGray;
        Color cloudSide1Col = Color.LightYellow;
        Color cloudSide2Col = Color.LightYellow;

        Graphics.AbsVoxelObj bgVoxModel;
        VikingEngine.LootFest.BlockMap.AbsLevelTerrain currentTerrain = null;
        Graphics.AbsVoxelObj angel = null;
        Vector3 angelPos;


        Time nextBlinkTimer = new Time(2, TimeUnit.Seconds);
        Timer.Basic openEyesTimer = new Timer.Basic(120, true);

        public BackgroundScenery()
        {
            bgVoxModel = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.scenery_test3, 
                2900, 0, false, false); 
            bgVoxModel.UseCameraCulling = false; 

            if (Ref.rnd.Chance(0.01f))
            {
                angel = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.angel_watch,
                    6f, 0, false, true);
            }

            Ref.draw.ClrColor = new Color(69, 92, 107);//Color.DarkGray;

            createSkyBox();
            
        }

        public void Draw(int cameraIndex)
        {
            if (Ref.draw.DrawGround)
            {
                //bgVoxModel.Draw(cameraIndex);
                skybox.Draw(cameraIndex);
            }
        }


        public void update(Players.Player player)
        {
            if (player.hero.Level != null && player.hero.Level.terrainFromWp(player.hero.WorldPos) != currentTerrain)
            {
                currentTerrain = player.hero.Level.terrainFromWp(player.hero.WorldPos);

                if (currentTerrain.backgroundScenery.underground)
                {
                    bgVoxModel.Visible = false;
                    skybox.Visible = false;
                    Ref.draw.ClrColor = Color.Black;
                }
                else
                {
                    bgVoxModel.Visible = true;
                    skybox.Visible = true;

                    bgVoxModel.position = VectorExt.V2toV3XZ(player.hero.Level.levelCenterInWorldXZ().Vec);
                    bgVoxModel.position.Y = -10;

                    skybox.position = bgVoxModel.position;

                    if (angel != null)
                    { angel.position = bgVoxModel.position + angelPos; }
                }
            }

            if (angel != null)
            {
                if (nextBlinkTimer.CountDown())
                {
                    angel.Frame = 1;
                    if (openEyesTimer.Update())
                    {
                        angel.Frame = 0;
                        nextBlinkTimer.Seconds = Ref.rnd.Float(0.6f, 10f);
                    }
                }
            }
        }

        Graphics.GeneratedObjColor skybox;
        void createSkyBox()
        {
            List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>(HeightDivitions * CirkularDivitions);
            bool col1 = false;
             float topRadius = SkyRadius, bottomRadius = SkyRadius;


            for (int hSlice = 0; hSlice < HeightDivitions; ++hSlice)
            {
                col1 = !col1;
                float top = (float)(Math.Sin(MathHelper.PiOver2 * (hSlice + 1) / HeightDivitions) * Height);
                float bottom = (float)(Math.Sin(MathHelper.PiOver2 * hSlice / HeightDivitions) * Height);
                Color topCol = heightColor(hSlice + 1);
                Color bottomCol = heightColor(hSlice);
                if (hSlice == HeightDivitions - 1)
                {
                    topCol = bottomCol;
                }

                topRadius = (float)(Math.Cos(MathHelper.PiOver2 * top / Height) * SkyRadius);
                bottomRadius = (float)(Math.Cos(MathHelper.PiOver2 * bottom / Height) * SkyRadius);

                for (int r = 0; r < CirkularDivitions; ++r)
                {
                    float angle1 = (float)r / CirkularDivitions * MathHelper.TwoPi;
                    float angle2 = (float)(r + 1) / CirkularDivitions * MathHelper.TwoPi;

                    Graphics.PolygonColor poly = new Graphics.PolygonColor();

                    //Top angle1
                    poly.V0sw.Position = VectorExt.V2toV3XZ(lib.AngleToV2(angle1, topRadius), top);
                    //Top angle2
                    poly.V1nw.Position = VectorExt.V2toV3XZ(lib.AngleToV2(angle2, topRadius), top);
                    //Bottom angle1
                    poly.V2se.Position = VectorExt.V2toV3XZ(lib.AngleToV2(angle1, bottomRadius), bottom);
                    //Bottom angle2
                    poly.V3ne.Position = VectorExt.V2toV3XZ(lib.AngleToV2(angle2, bottomRadius), bottom);

                    poly.V0sw.Color = topCol;
                    poly.V1nw.Color = topCol;
                    poly.V2se.Color = bottomCol;
                    poly.V3ne.Color = bottomCol;
                    
                    polygons.Add(poly);
                }
            }


            //STARS
            const float StarY = Height * 0.5f;
            const float StarRadius = SkyRadius * 0.5f;
            const float GalaxyRadius = StarRadius * 0.2f;
            //const float StarW = 2f;
            IntervalF StarScaleRange = new IntervalF(0.8f, 1.8f);
            const int StarCount = 200;

            Color starCol1 = ColorExt.Mix(MidCol, BottomCol, 0.5f);//MidCol;//Color.White;
            Color starCol2 = ColorExt.Mix(MidCol, BottomCol, 0.3f);//MidCol;//Color.White;
            Color starCol;

            for (int i = 0; i < StarCount; ++i)
            {
                float StarW = StarScaleRange.GetRandom();
               
                Vector3 pos = new Vector3(Ref.rnd.Plus_MinusF(StarRadius), StarY, Ref.rnd.Plus_MinusF(StarRadius));
                if (Ref.rnd.Chance(0.3f))
                {
                    pos.Z = Ref.rnd.Plus_MinusF(GalaxyRadius);
                }


                Graphics.PolygonColor poly = new Graphics.PolygonColor();
                //Top left
                poly.V0sw.Position = pos;
                //Top right
                poly.V1nw.Position = pos;
                poly.V1nw.Position.X -= StarW;
                //Bottom left
                poly.V2se.Position = pos;
                poly.V2se.Position.Z += StarW;
                //Bottom right
                poly.V3ne.Position = pos;
                poly.V3ne.Position.X -= StarW;
                poly.V3ne.Position.Z += StarW;

                if (i == 0)
                {
                    starCol = Color.Salmon;
                }
                else if (i == 1)
                {
                    starCol = Color.YellowGreen;
                }
                else if (i == 2)
                {
                    starCol = Color.DarkViolet;
                }
                else
                {
                    starCol = Ref.rnd.Chance(0.8f) ? starCol1 : starCol2;
                }
                poly.V0sw.Color = starCol;
                poly.V1nw.Color = starCol;
                poly.V2se.Color = starCol;
                poly.V3ne.Color = starCol;
                polygons.Add(poly);
            }

            //CLOUDS
            const float CloudY = Height * 0.2f;
            const float CloudRadius = SkyRadius * 0.7f;
            const int CloudCount = 100;

            const float CloudMaxSz = 240;
            IntervalF CloudWRange = new IntervalF(0.4f * CloudMaxSz, 0.8f * CloudMaxSz);
            IntervalF CloudLRange = new IntervalF(0.6f * CloudMaxSz, 1f * CloudMaxSz);
            IntervalF CloudHeightScale = new IntervalF(0.12f * CloudMaxSz, 0.2f * CloudMaxSz);

            IntervalF ClusterPartScale = new IntervalF(0.1f * CloudMaxSz, 0.4f * CloudMaxSz);
            IntervalF ClusterPartHeight = new IntervalF(0.1f * CloudMaxSz, 0.1f * CloudMaxSz);

            Range ClusterCountRange = new Range(2, 5);

            cloudBottomCol = new Color(150, 183, 197);//97, 158, 184);
            frontCol = new Color(242, 242, 232);
            cloudBackCol = frontCol;
            cloudSide1Col = new Color(238, 230, 220);
            cloudSide2Col = cloudSide1Col;

            for (int i = 0; i < CloudCount; ++i)
            {
                Vector3 cloudClusterPos = new Vector3(Ref.rnd.Plus_MinusF(CloudRadius), CloudY + Ref.rnd.Float(30), Ref.rnd.Plus_MinusF(CloudRadius));
                float cloudH = CloudHeightScale.GetRandom();

                float width = CloudWRange.GetRandom(), length = CloudLRange.GetRandom();

                createCloudBlock(width, length, cloudH, cloudClusterPos, polygons);

                int clusterCount = ClusterCountRange.GetRandom();
                for (int c = 0; c < clusterCount; c++)
                {
                    Vector3 pos = cloudClusterPos +
                        new Vector3(
                            Ref.rnd.Plus_MinusF(width * 0.9f),
                            Ref.rnd.Float(-cloudH * 0.8f, cloudH * 0.9f),
                            Ref.rnd.Plus_MinusF(length * 1.1f));

                    createCloudBlock(ClusterPartScale.GetRandom(), ClusterPartScale.GetRandom(), ClusterPartHeight.GetRandom(), pos, polygons);
                }
            }

            if (angel != null)
            {
                angelPos.Y = CloudY + 10;
                createCloudBlock(16, 16, 8, angelPos, polygons);
                angelPos.Z += 9.2f;
                angelPos.Y += 7f;
            }

            skybox = new Graphics.GeneratedObjColor(new Graphics.PolygonsAndTrianglesColor(
                polygons, null), LoadedTexture.WhiteArea, false);
        }

        

        void createCloudBlock(float width, float length, float cloudH, Vector3 pos, List<Graphics.PolygonColor> polygons)
        {
            Graphics.PolygonColor bottomPoly = new Graphics.PolygonColor();
            //Top left
            pos.X += width * 0.5f;
            pos.Z -= length * 0.5f;



            bottomPoly.V0sw.Position = pos;

            //Top right
            bottomPoly.V1nw.Position = pos;
            bottomPoly.V1nw.Position.X -= width;
            //Bottom left
            bottomPoly.V2se.Position = pos;
            bottomPoly.V2se.Position.Z += length;
            //Bottom right
            bottomPoly.V3ne.Position = pos;
            bottomPoly.V3ne.Position.X -= width;
            bottomPoly.V3ne.Position.Z += length;

            

            bottomPoly.V0sw.Color = cloudBottomCol;
            bottomPoly.V1nw.Color = cloudBottomCol;
            bottomPoly.V2se.Color = cloudBottomCol;
            bottomPoly.V3ne.Color = cloudBottomCol;

            polygons.Add(bottomPoly);


            Graphics.PolygonColor cloudFrontPoly = new Graphics.PolygonColor();
            cloudFrontPoly.V0sw.Position = bottomPoly.V1nw.Position;
            cloudFrontPoly.V1nw.Position = bottomPoly.V0sw.Position;

            cloudFrontPoly.V2se.Position = bottomPoly.V1nw.Position;
            cloudFrontPoly.V2se.Position.Y += cloudH;
            cloudFrontPoly.V3ne.Position = bottomPoly.V0sw.Position;
            cloudFrontPoly.V3ne.Position.Y += cloudH;

           
            cloudFrontPoly.V0sw.Color = frontCol;
            cloudFrontPoly.V1nw.Color = frontCol;
            cloudFrontPoly.V2se.Color = frontCol;
            cloudFrontPoly.V3ne.Color = frontCol;

            polygons.Add(cloudFrontPoly);

            Graphics.PolygonColor backPoly = new Graphics.PolygonColor();
            backPoly.V0sw.Position = bottomPoly.V2se.Position;
            backPoly.V1nw.Position = bottomPoly.V3ne.Position;

            backPoly.V2se.Position = bottomPoly.V2se.Position;
            backPoly.V2se.Position.Y += cloudH;
            backPoly.V3ne.Position = bottomPoly.V3ne.Position;
            backPoly.V3ne.Position.Y += cloudH;

            
            backPoly.V0sw.Color = cloudBackCol;
            backPoly.V1nw.Color = cloudBackCol;
            backPoly.V2se.Color = cloudBackCol;
            backPoly.V3ne.Color = cloudBackCol;

            polygons.Add(backPoly);


            Graphics.PolygonColor side1Poly = new Graphics.PolygonColor();
            side1Poly.V0sw.Position = bottomPoly.V3ne.Position;
            side1Poly.V1nw.Position = bottomPoly.V1nw.Position;

            side1Poly.V2se.Position = bottomPoly.V3ne.Position;
            side1Poly.V2se.Position.Y += cloudH;
            side1Poly.V3ne.Position = bottomPoly.V1nw.Position;
            side1Poly.V3ne.Position.Y += cloudH;

            
            side1Poly.V0sw.Color = cloudSide1Col;
            side1Poly.V1nw.Color = cloudSide1Col;
            side1Poly.V2se.Color = cloudSide1Col;
            side1Poly.V3ne.Color = cloudSide1Col;

            polygons.Add(side1Poly);

            Graphics.PolygonColor side2Poly = new Graphics.PolygonColor();
            side2Poly.V0sw.Position = bottomPoly.V0sw.Position;
            side2Poly.V1nw.Position = bottomPoly.V2se.Position;

            side2Poly.V2se.Position = bottomPoly.V0sw.Position;
            side2Poly.V2se.Position.Y += cloudH;
            side2Poly.V3ne.Position = bottomPoly.V2se.Position;
            side2Poly.V3ne.Position.Y += cloudH;

            
            side2Poly.V0sw.Color = cloudSide2Col;
            side2Poly.V1nw.Color = cloudSide2Col;
            side2Poly.V2se.Color = cloudSide2Col;
            side2Poly.V3ne.Color = cloudSide2Col;

            polygons.Add(side2Poly);
        }

        const float MidColPercY = 0.4f;
        

        Color heightColor(int hSlice)
        {
            float percY = (float)hSlice / HeightDivitions;
            if (percY < MidColPercY)
            {
                return ColorExt.Mix(MidCol, BottomCol,  percY / MidColPercY);
            }
            else
            {
                return ColorExt.Mix(TopCol, MidCol,  (percY - MidColPercY) / (1f - MidColPercY));
            }
        }
    }

    struct BackgroundSceneryData
    {
        public bool underground;

        public BackgroundSceneryData(bool underground)
        {
            this.underground = underground;
        }
    }
}
