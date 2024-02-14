using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class MapFogModel
    {
        const float Height = 0.9f;

        const float BottomHalfW = Board.SquareWidth * 0.58f;
        const float TopHalfW = BottomHalfW;//Board.SquareRadius;

        Color DarkCol = new Color(52, 68, 87);
        Color LightCol = new Color(54, 71, 91);

        Color outlineCol = new Color(48, 64, 82);

        public Graphics.VoxelModel model, newModel;
        Graphics.VoxelModel squareOddOrg, squareEvenOrg;

        public MapFogModel()
        {
            {
                List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>();

                squarePolys(polygons, Vector3.Zero, true);

                squareOddOrg = new Graphics.VoxelModel(false);
                squareOddOrg.Effect = toggLib.ModelEffect;
                squareOddOrg.BuildFromPolygons(
                    new Graphics.PolygonsAndTrianglesColor(polygons, null),
                    new List<int> { polygons.Count }, LoadedTexture.WhiteArea);
            }
            {
                List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>();

                squarePolys(polygons, Vector3.Zero, true);

                squareEvenOrg = new Graphics.VoxelModel(false);
                squareEvenOrg.Effect = toggLib.ModelEffect;
                squareEvenOrg.BuildFromPolygons(
                    new Graphics.PolygonsAndTrianglesColor(polygons, null),
                    new List<int> { polygons.Count }, LoadedTexture.WhiteArea);
            }
        }

        

        public void refresh()
        {
            new QueAndSynchTask(generateAsynch, onGenerateComplete);
        }

        public void generateAsynch()
        {
            
            //const float OutlineHalfW = TopHalfW * 0.7f;

            List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>();
            var loop = toggRef.board.tileGrid.LoopInstance();
       
            while (loop.Next())
            {
                if (toggRef.board.tileGrid.Get(loop.Position).hidden)
                {
                    squarePolys(polygons, 
                        toggRef.board.toWorldPos_Center(loop.Position, 0),
                        oddSquare(loop.Position));

                    //bool odd = lib.IsOdd(loop.Position.X + loop.Position.Y);

                    //Color col;
                    //float width;
                    //float height;
                    ////bool bTopOutline;
                    //if (odd)
                    //{
                    //    col = DarkCol;
                    //    width = 1.05f;
                    //    height = Height * 1.05f;
                    //    //bTopOutline = true;
                    //}
                    //else
                    //{
                    //    col = LightCol;
                    //    width = 1.0f;
                    //    height = Height;
                    //    //bTopOutline = false;
                    //}

                    //var top = new PolygonColor(
                    //    new Vector3(-TopHalfW, 0, -TopHalfW) * width,
                    //    new Vector3(TopHalfW, 0, -TopHalfW) * width,
                    //    new Vector3(-TopHalfW, 0, TopHalfW) * width,
                    //    new Vector3(TopHalfW, 0, TopHalfW) * width,
                    //    SpriteName.WhiteArea, Dir4.N, col);

                    ////var topOutline = new PolygonColor(
                    ////    new Vector3(-OutlineHalfW, 0, -OutlineHalfW),
                    ////    new Vector3(OutlineHalfW, 0, -OutlineHalfW),
                    ////    new Vector3(-OutlineHalfW, 0, OutlineHalfW),
                    ////    new Vector3(OutlineHalfW, 0, OutlineHalfW),
                    ////    SpriteName.WhiteArea, Dir4.N, outlineCol);

                    //var bottom = new PolygonColor(
                    //    new Vector3(-BottomHalfW, 0, -BottomHalfW) * width,
                    //    new Vector3(BottomHalfW, 0, -BottomHalfW) * width,
                    //    new Vector3(-BottomHalfW, 0, BottomHalfW) * width,
                    //    new Vector3(BottomHalfW, 0, BottomHalfW) * width,
                    //    SpriteName.WhiteArea, Dir4.N, Color.Black);

                    //Vector3 center = toggRef.board.toWorldPos_Center(loop.Position, 0);
                    //top.Move(center + height * upVector);

                    ////toggLib.TowardCamVector);
                    ////topOutline.Move(center + (height * 1.01f) * upVector);
                    //bottom.Move(center);

                    //polygons.Add(top);
                    //polygons.Add(//LEFT
                    //    new PolygonColor(
                    //    top.V1nw.Position,
                    //    top.V0sw.Position,
                    //    bottom.V1nw.Position,
                    //    bottom.V0sw.Position,
                    //    SpriteName.WhiteArea, Dir4.N, col));

                    //polygons.Add(//FRONT
                    //    new PolygonColor(
                    //    top.V0sw.Position,
                    //    top.V2se.Position,
                    //    bottom.V0sw.Position,
                    //    bottom.V2se.Position,
                    //    SpriteName.WhiteArea, Dir4.N, col));

                    //polygons.Add(//RIGHT
                    //    new PolygonColor(
                    //    top.V2se.Position,
                    //    top.V3ne.Position,
                    //    bottom.V2se.Position,
                    //    bottom.V3ne.Position,
                    //    SpriteName.WhiteArea, Dir4.N, col));

                }
            }

            newModel = new Graphics.VoxelModel(false);
            newModel.Effect = toggLib.ModelEffect;
            newModel.BuildFromPolygons(
                new Graphics.PolygonsAndTrianglesColor(polygons, null),
                new List<int> { polygons.Count }, LoadedTexture.WhiteArea);
            
        }

        void squarePolys(List<Graphics.PolygonColor> polygons, Vector3 center, bool odd)
        {
            Vector3 upVector = Vector3.Up + toggLib.TowardCamVector;
            upVector.Normalize();

           //b.IsOdd(squarePos.X + squarePos.Y);

            Color col;
            float width;
            float height;
            //bool bTopOutline;
            if (odd)
            {
                col = DarkCol;
                width = 1.05f;
                height = Height * 1.05f;
                //bTopOutline = true;
            }
            else
            {
                col = LightCol;
                width = 1.0f;
                height = Height;
                //bTopOutline = false;
            }

            var top = new PolygonColor(
                new Vector3(-TopHalfW, 0, -TopHalfW) * width,
                new Vector3(TopHalfW, 0, -TopHalfW) * width,
                new Vector3(-TopHalfW, 0, TopHalfW) * width,
                new Vector3(TopHalfW, 0, TopHalfW) * width,
                SpriteName.WhiteArea, Dir4.N, col);

            //var topOutline = new PolygonColor(
            //    new Vector3(-OutlineHalfW, 0, -OutlineHalfW),
            //    new Vector3(OutlineHalfW, 0, -OutlineHalfW),
            //    new Vector3(-OutlineHalfW, 0, OutlineHalfW),
            //    new Vector3(OutlineHalfW, 0, OutlineHalfW),
            //    SpriteName.WhiteArea, Dir4.N, outlineCol);

            var bottom = new PolygonColor(
                new Vector3(-BottomHalfW, 0, -BottomHalfW) * width,
                new Vector3(BottomHalfW, 0, -BottomHalfW) * width,
                new Vector3(-BottomHalfW, 0, BottomHalfW) * width,
                new Vector3(BottomHalfW, 0, BottomHalfW) * width,
                SpriteName.WhiteArea, Dir4.N, Color.Black);

            //Vector3 center;
            
            //else
            //{
            //    center = Vector3.Zero;
            //}
            top.Move(center + height * upVector);
            bottom.Move(center);

            polygons.Add(top);
            polygons.Add(//LEFT
                new PolygonColor(
                top.V1nw.Position,
                top.V0sw.Position,
                bottom.V1nw.Position,
                bottom.V0sw.Position,
                SpriteName.WhiteArea, Dir4.N, col));

            polygons.Add(//FRONT
                new PolygonColor(
                top.V0sw.Position,
                top.V2se.Position,
                bottom.V0sw.Position,
                bottom.V2se.Position,
                SpriteName.WhiteArea, Dir4.N, col));

            polygons.Add(//RIGHT
                new PolygonColor(
                top.V2se.Position,
                top.V3ne.Position,
                bottom.V2se.Position,
                bottom.V3ne.Position,
                SpriteName.WhiteArea, Dir4.N, col));
        }

        bool oddSquare(IntVector2 squarePos)
        {
            return lib.IsOdd(squarePos.X + squarePos.Y);
        }

        public void onGenerateComplete()
        {
            if (model != null)
            {
                model.DeleteMe();
            }
            model = newModel;

            model.AddToRender();
        }

        public Graphics.VoxelModelInstance CreateSquare(IntVector2 pos)
        {
            var model = new VoxelModelInstance(oddSquare(pos) ? squareOddOrg : squareEvenOrg);
            model.position = toggRef.board.toWorldPos_Center(pos, 0);

            return model;
        }
    }

    class AnimateFogSquare : AbsUpdateable
    {
        Time delay;
        Graphics.VoxelModelInstance square;

        public AnimateFogSquare(IntVector2 pos, IntVector2 heroPos)
            :base(true)
        {
            delay.MilliSeconds = ((heroPos - pos).SideLength() - 2) * 120;

            square = toggRef.board.fogModel.CreateSquare(pos);
        }

        public override void Time_Update(float time_ms)
        {
            if (delay.CountDown())
            {
                square.scale.X -= 4 * Ref.DeltaTimeSec;
                square.scale.Z = square.scale.X;

                if (square.scale.X <= 0)
                {
                    this.DeleteMe();
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            square.DeleteMe();
        }
    }
}
