using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LootFest.Data
{
    
    class Images
    {

//#if TEX2
        public const int BlockTexturesTileSize = 48;
        const int BlockTexturesTilesWidth = 32;
//#else
//        public const int BlockTexturesTileSize = 32;
//        const int BlockTexturesTilesWidth = 32;
//#endif


        bool isLoaded = false;

        //public Graphics.VoxelModel StandardModel_TempBlock;
        public Graphics.VoxelModel StandardModel_UsingMenu;
        public Graphics.VoxelModel StandardModel_Sword;

        public Graphics.VoxelModel StandardModel_Character;
        public Graphics.VoxelModel StandardModel_TempBlockAnimated;

        public  Graphics.Sprite[] TileIxToImgeFile;
        
        
        public  int NumUsedTiles = 6 * BlockTexturesTileSize; //får inte överstiga tile sheet storleken
        public const int HalfTile = BlockTexturesTileSize / PublicConstants.Twice;
        public  readonly Vector2 TileV2Size = VectorExt.V2(BlockTexturesTileSize);
        
        public const int TileSheetPixWidth = BlockTexturesTileSize * BlockTexturesTilesWidth;
        const float TileTextureSize = 1f / BlockTexturesTilesWidth;
        // Graphics.RenderTargetImage tileSheetTarget;
         //readonly Rectangle ImageFileSize = new Rectangle(0, 0, BlockTexturesTileSize, BlockTexturesTileSize);

        public  void HideTargetImage()
        {
            //tileSheetTarget.Visible = !tileSheetTarget.Visible;
        }
        public  void AddSpriteName(SpriteName name, int index)
        {
            DataLib.SpriteCollection.Set(name,
                TileIxToImgeFile[index]);
        }

        const int PullEdge = 2;//0.8f / TileSheetPixWidth;
        const int PullEdgeTwice = PullEdge * PublicConstants.Twice;

        public  void Init()
        {
            if (!isLoaded)
            {
                //Ref.draw.DontAddNextRenderObject();
                //tileSheetTarget = new RenderTargetImage(Vector2.Zero, VectorExt.V2(BlockTexturesTilesWidth * BlockTexturesTileSize), ImageLayers.Background9);
                //Engine.LoadContent.SetTextureFromTarget(tileSheetTarget.renderTarget, LoadedTexture.LF_TargetSheet);
                new Data.BlockTextureTile();
                isLoaded = true;
                Sprite presetData = new Sprite();
                presetData.textureIndex = (int)LfLib.BlockTexture;
                presetData.Source = new Rectangle(0, 0, BlockTexturesTileSize - PullEdgeTwice, BlockTexturesTileSize - PullEdgeTwice);
                presetData.SourceF.Size = VectorExt.V2(TileTextureSize);
                TileIxToImgeFile = new Sprite[BlockTexturesTilesWidth * BlockTexturesTilesWidth];
                int currentTilesIx = 0;
                for (int y = 0; y < BlockTexturesTilesWidth; y++)
                {
                   // presetData.Source.Y = y * BlockTexturesTileSize ;
                    //presetData.SourceF.Y = y * TileTextureSize;

                    for (int x = 0; x < BlockTexturesTilesWidth; x++)
                    {
                        presetData.Source.X = x * BlockTexturesTileSize + PullEdge;
                        presetData.Source.Y = y * BlockTexturesTileSize + PullEdge;

                        //presetData.UpdateSourceF(false, TileSheetPixWidth);
                        presetData.UpdateSourcePolygon(false);
                        //presetData.SourceF.X = x * TileTextureSize;

                        //presetData.SourcePolygonTopLeft = presetData.SourceF.Position + VectorExt.V2(PullEdge);
                        //presetData.SourcePolygonTopRight = presetData.SourceF.Position + new Vector2(presetData.SourceF.Size.X - PullEdgeTwice, PullEdge);
                        //presetData.SourcePolygonLowLeft = presetData.SourceF.Position + new Vector2(PullEdge, presetData.SourceF.Size.Y - PullEdgeTwice);
                        //presetData.SourcePolygonLowRight = presetData.SourceF.Position + new Vector2(presetData.SourceF.Size.X - PullEdgeTwice, presetData.SourceF.Size.Y - PullEdgeTwice);

                        TileIxToImgeFile[currentTilesIx] = presetData;
                        currentTilesIx++;
                    }
                }

            }
        }
        

        public void LoadStandardVobjects()
        {
            //StandardModel_TempBlock = Editor.VoxelObjDataLoader.GetVoxelObjMaster(VoxelModelName.temp_block, Vector3.Zero);
            StandardModel_UsingMenu = Editor.VoxelObjDataLoader.GetVoxelObjMaster(VoxelModelName.using_menu, Vector3.Zero);
            StandardModel_Sword = Editor.VoxelObjDataLoader.GetVoxelObjMaster(VoxelModelName.Sword1, Vector3.Zero);

            StandardModel_Character = (Graphics.VoxelModel)Editor.VoxelObjDataLoader.GetVoxelObjMaster(VoxelModelName.temp_hero, Vector3.Zero);
            StandardModel_TempBlockAnimated = (Graphics.VoxelModel)Editor.VoxelObjDataLoader.GetVoxelObjMaster(VoxelModelName.temp_block_animated, Vector3.Zero);
        }

        //public Graphics.VoxelModelInstance getInstance_TempblockAnim()
        //{
        //    return new Graphics.VoxelModelInstance(LfRef.Images.StandardModel_TempBlockAnimated, new Graphics.AnimationsSettings(3, 0.1f));
        //}

        public  void ReaddToRender()
        {
            //Ref.draw.AddToRenderList(tileSheetTarget, true);
            //tileSheetTarget.Visible = false;
        }

         //Graphics.ImageAdvanced waterTile;
         //Graphics.ImageAdvanced lavaTile;
         //Graphics.Image waterEdge;
         //Graphics.Image lavaEdge;
         //Graphics.Image allTiles;

        //public  void InitAnimation()
        //{
        //    //allTiles = new Image(SpriteName.LFBlockTexturesBase, Vector2.Zero, VectorExt.V2(LoadTiles.BlockTextureWidth), ImageLayers.Background2);
        //    //allTiles.DeleteMe();
        //    int waterTilePos = BlockTextures.Materials[(int)MaterialType.water].TopTiles.GetStandardTile();
        //    waterTile = new ImageAdvanced(SpriteName.LFWater, GetTilePixPos(waterTilePos),
        //        TileV2Size, ImageLayers.Foreground9, false);
        //    waterTile.DeleteMe();
        //    int lavaTilePos = BlockTextures.Materials[(int)MaterialType.lava].TopTiles.GetStandardTile();
        //    lavaTile = new ImageAdvanced(SpriteName.LFLava, GetTilePixPos(lavaTilePos),
        //        TileV2Size, ImageLayers.Foreground9, false);
        //    lavaTile.DeleteMe();

        //    waterEdge = new Image(SpriteName.LFEdge, waterTile.Position, TileV2Size, ImageLayers.AbsoluteTopLayer);
        //    waterEdge.Color = new Color(10, 68,217);
        //    //waterEdge.Transparentsy = 0.6f;
        //    waterEdge.DeleteMe();
        //    lavaEdge = new Image(SpriteName.LFEdge, lavaTile.Position, TileV2Size, ImageLayers.AbsoluteTopLayer);
        //    lavaEdge.Color = Color.DarkRed;
        //    //lavaEdge.Transparentsy = 0.6f;
        //    lavaEdge.DeleteMe();

        //}
        // CirkleCounterDown animLavaTileSourceY = new CirkleCounterDown(BlockTexturesTileSize - 1);
        // CirkleCounterDown animWaterTileSourceY = new CirkleCounterDown(BlockTexturesTileSize - 3);
        //public  void UpdateAnimation()
        //{
        //    ////move one pixel
        //    //animLavaTileSourceY.Next();
        //    //animWaterTileSourceY.Next();
        //    //waterTile.SourceY = animWaterTileSourceY.Value;
        //    //lavaTile.SourceY = animLavaTileSourceY.Value;
        //    //tileSheetTarget.DrawImagesToTarget(new List<AbsDraw> { waterTile, lavaTile, waterEdge, lavaEdge, allTiles }, true);
        //}

        //public  void AddImagesToTarget(List<AbsDraw> images)
        //{
        //    //tileSheetTarget.DrawImagesToTarget(images, true);
        //    //foreach (AbsDraw img in images)
        //    //{
        //    //    img.DeleteMe();
        //    //}
        //    Graphics.LFHeightMap.InitEffect();
        //}

        public  IntVector2 GetTileRowColPos(int tileIx)
        {
            IntVector2 result = IntVector2.Zero;
            result.Y = tileIx / BlockTexturesTilesWidth;
            result.X = tileIx - (result.Y * BlockTexturesTilesWidth);
            return result;
        }
        public  Vector2 GetTilePixPos(int tileIx)
        {
            return (GetTileRowColPos(tileIx).Vec) * BlockTexturesTileSize;
        }

    }

    

    
}
