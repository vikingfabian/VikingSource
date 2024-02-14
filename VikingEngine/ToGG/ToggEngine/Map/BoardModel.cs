using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class BoardModelContent
    {
        public Graphics.VoxelModel model;
        public List<ToggEngine.AbsParticleEmitter> emitters = new List<ToggEngine.AbsParticleEmitter>(16);

        public BoardModelContent()
        {
            model = new Graphics.VoxelModel(true);
            model.Visible = false;
        }

        public void DeleteMe()
        {
            model.DeleteMe();
        }
    }

    class BoardModel
    {
        public BoardModelContent content = null, newConent = null;
        public bool isGenerating = false;

        public BoardModel()
        {
            lib.DoNothing();
            //beginGenerateModel();
        }

        public void beginGenerateModel()
        {
            isGenerating = true;
            newConent = new BoardModelContent();

            new QueAndSynchTask(generateAsynch, onGenerateComplete);
        }

        void generateAsynch()
        {
            Grid2D<BoardSquareContent> tileGrid = toggRef.board.tileGrid;

            const LoadedTexture Texture = LoadedTexture.SpriteSheet;

            IntVector2 pos = IntVector2.Zero;            
            GenerateBoardModelArgs modelArgs = new GenerateBoardModelArgs();

            //Tiles
            for (pos.Y = 0; pos.Y < tileGrid.Size.Y; ++pos.Y)
            {
                modelArgs.setTileY(pos.Y);

                for (pos.X = 0; pos.X < tileGrid.Width; ++pos.X)
                {
                    modelArgs.squareContent = tileGrid.Get(pos);
                    modelArgs.setTileX(pos.X);

                    modelArgs.squareContent.Square.generateModel(modelArgs);

                    //var roomFlag = modelArgs.squareContent.tileObjects.GetObject(ToggEngine.TileObjectType.RoomFlag);
                    //if (roomFlag != null)
                    //{
                    //    ((HeroQuest.MapGen.RoomFlag)roomFlag).collectTiles_asynch();
                    //}
                }
            }

            modelArgs.polygons.AddRange(modelArgs.overlaypolygons);

            newConent.emitters = modelArgs.torches;
            newConent.model.Effect = toggLib.ModelEffect;
            newConent.model.BuildFromPolygons(
                new Graphics.PolygonsAndTrianglesColor(modelArgs.polygons, null), 
                new List<int> { modelArgs.polygons.Count }, Texture);
            //newConent.model.Color = ColorExt.GrayScale(10);//Color.Gray;

        }

        void onGenerateComplete()
        {
            toggRef.board.onNewModel(this);

            content = newConent;
            newConent = null;
            content.model.Visible = true;
            isGenerating = false;
        }
        

        public void draw()
        {
           content.model.Draw();
        }

        public bool drawready()
        {
            return content != null && content.model.Visible;
        }

        public void DeleteMe()
        {
            if (content != null)
            {
                content.DeleteMe();
            }
        }
    }

    
}
