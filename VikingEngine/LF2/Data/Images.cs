using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.Data
{
    
    class Images : VikingEngine.LF2.Process.ILoadImage
    {
        public const string VoxelObjByteArrayEnding = ".vox";
        bool isLoaded = false;
        
        public  Dictionary<VoxelModelName, Graphics.VoxelObj> StandardVoxelObjects;
        public  Dictionary<VoxelModelName, Graphics.VoxelObjAnimated> StandardAnimatedVoxelObjects;
        public  Graphics.ImageFile2[] TileIxToImgeFile;
        
        public const int TileSize = 32;
        public  int NumUsedTiles = 6 * TileSize; //får inte överstiga tile sheet storleken
        public const int HalfTile = TileSize / PublicConstants.Twice;
        public  readonly Vector2 TileV2Size = new Vector2(TileSize);
        const int SheetNumTileWidth = 32; //modifiera efter hand som jag lägger till fler tiles
        public const int TileSheetPixWidth = TileSize * SheetNumTileWidth;
        const float TileTextureSize = 1f / SheetNumTileWidth;
         Graphics.RenderTargetImage tileSheetTarget;
         readonly Rectangle ImageFileSize = new Rectangle(0, 0, TileSize, TileSize);

        public  void HideTargetImage()
        {
            tileSheetTarget.Visible = !tileSheetTarget.Visible;
        }
        public  void AddSpriteName(SpriteName name, int index)
        {
            DataLib.Images.AddImageFile(name,
                TileIxToImgeFile[index]);
        }

        const float PullEdge = 0.8f / TileSheetPixWidth;//2.1f / TileSheetPixWidth;
        const float PullEdgeTwice = PullEdge * PublicConstants.Twice;

        public  void Init()
        {
            if (!isLoaded)
            {
                Ref.draw.DontAddNextRenderObject();
                tileSheetTarget = new RenderTargetImage(Vector2.Zero, new Vector2(SheetNumTileWidth * TileSize), ImageLayers.Background9);
                Engine.LoadContent.SetTextureFromTarget(tileSheetTarget.renderTarget, LoadedTexture.LF_TargetSheet);
                new Data.BlockTextureTile();
                isLoaded = true;
                ImageFile2 presetData = new ImageFile2();
                presetData.SpriteSheet = Engine.LoadContent.Texture(LoadedTexture.LF_TargetSheet);
                presetData.Source = new Rectangle(0, 0, TileSize, TileSize);
                presetData.SourceF.Size = new Vector2(TileTextureSize);
                TileIxToImgeFile = new ImageFile2[SheetNumTileWidth * SheetNumTileWidth];
                int currentTilesIx = 0;
                for (int y = 0; y < SheetNumTileWidth; y++)
                {
                    presetData.Source.Y = y * TileSize;
                    presetData.SourceF.Y = y * TileTextureSize;

                    for (int x = 0; x < SheetNumTileWidth; x++)
                    {
                        presetData.Source.X = x * TileSize;
                        presetData.SourceF.X = x * TileTextureSize;

                        presetData.SourcePolygonTopLeft = presetData.SourceF.Position + new Vector2(PullEdge);
                        presetData.SourcePolygonTopRight = presetData.SourceF.Position + new Vector2(presetData.SourceF.Size.X - PullEdgeTwice, PullEdge);
                        presetData.SourcePolygonLowLeft = presetData.SourceF.Position + new Vector2(PullEdge, presetData.SourceF.Size.Y - PullEdgeTwice);
                        presetData.SourcePolygonLowRight = presetData.SourceF.Position + new Vector2(presetData.SourceF.Size.X - PullEdgeTwice, presetData.SourceF.Size.Y - PullEdgeTwice);

                        TileIxToImgeFile[currentTilesIx] = presetData;
                        currentTilesIx++;
                    }
                }

            }
        }
        public void LoadStandardVobjects()
        {
            StandardVoxelObjects = new Dictionary<VoxelModelName, Graphics.VoxelObj>();
            List<VoxelModelName> loadStandardsVObjects = new List<VoxelModelName>
                {

                    VoxelModelName.i,
                    VoxelModelName.temp_block,
                    VoxelModelName.using_menu, 
                    VoxelModelName.using_guide, 
                    VoxelModelName.using_build, 
                    VoxelModelName.Sword1,
                    
                };

            foreach (VoxelModelName obj in loadStandardsVObjects)
            {
                new LF2.Process.LoadImage(this, obj);
            }

            StandardVoxelObjects.Add(VoxelModelName.lightning, 
                Editor.VoxelObjDataLoader.GetVoxelObj(VoxelModelName.lightning, false, Editor.CenterAdjType.CenterAll, Vector3.Zero));

            //ANIMATED
            StandardAnimatedVoxelObjects = new Dictionary<VoxelModelName, Graphics.VoxelObjAnimated>();
            loadStandardsVObjects = new List<VoxelModelName>
            {
               VoxelModelName.Character,

                VoxelModelName.temp_block_animated,
            };
            foreach (VoxelModelName obj in loadStandardsVObjects)
            {
                new LF2.Process.LoadImage(this, obj);
            }
        }

        public bool LoadingComplete()
        {
            return StandardAnimatedVoxelObjects.ContainsKey(VoxelModelName.temp_block_animated);
        }

        public void SetCustomImage(Graphics.VoxelModel original, int link)
        {
            VoxelModelName name = (VoxelModelName)link;
            if (original.Animated)
            {
                StandardAnimatedVoxelObjects.Add(name, (Graphics.VoxelObjAnimated)original);
            }
            else
            {
                StandardVoxelObjects.Add(name, (Graphics.VoxelObj)original);
            }
        }

        public  Graphics.VoxelModelInstance StandardObjInstance(VoxelModelName name)
        {
            //Graphics.TempVoxelObj tempimg = new Graphics.TempVoxelObj(Color.Red, Vector3.One);
            return new Graphics.VoxelModelInstance(StandardVoxelObjects[name]);//tempimg);//
        }
        public  Graphics.VoxelModelInstance StandardAnimObjInstance(VoxelModelName objName, 
            Graphics.AnimationsSettings animationsSettings)
        {
            return new Graphics.VoxelModelInstance(
                    StandardAnimatedVoxelObjects[objName], 
                animationsSettings);
        }
        public  void ReaddToRender()
        {
            Ref.draw.AddToRenderList(tileSheetTarget, true);
            tileSheetTarget.Visible = false;
        }

         Graphics.ImageAdvanced waterTile;
         Graphics.ImageAdvanced lavaTile;
         Graphics.Image waterEdge;
         Graphics.Image lavaEdge;
         Graphics.Image allTiles;

        public  void InitAnimation()
        {
            allTiles = new Image(SpriteName.LFBlockTexturesBase, Vector2.Zero, new Vector2(LoadTiles.BlockTextureWidth), ImageLayers.Background2);
            allTiles.DeleteMe();
            int waterTilePos = MaterialBuilder.Materials[(int)MaterialType.water].TopTiles.GetStandardTile();
            waterTile = new ImageAdvanced(SpriteName.LFWater, GetTilePixPos(waterTilePos),
                TileV2Size, ImageLayers.Foreground9, false);
            waterTile.DeleteMe();
            int lavaTilePos = MaterialBuilder.Materials[(int)MaterialType.lava].TopTiles.GetStandardTile();
            lavaTile = new ImageAdvanced(SpriteName.LFLava, GetTilePixPos(lavaTilePos),
                TileV2Size, ImageLayers.Foreground9, false);
            lavaTile.DeleteMe();

            waterEdge = new Image(SpriteName.LFEdge, waterTile.Position, TileV2Size, ImageLayers.AbsoluteTopLayer);
            waterEdge.Color = new Color(10, 68,217);
            //waterEdge.Transparentsy = 0.6f;
            waterEdge.DeleteMe();
            lavaEdge = new Image(SpriteName.LFEdge, lavaTile.Position, TileV2Size, ImageLayers.AbsoluteTopLayer);
            lavaEdge.Color = Color.DarkRed;
            //lavaEdge.Transparentsy = 0.6f;
            lavaEdge.DeleteMe();

        }
         CirkleCounterDown animLavaTileSourceY = new CirkleCounterDown(TileSize - 1);
         CirkleCounterDown animWaterTileSourceY = new CirkleCounterDown(TileSize - 3);
        public  void UpdateAnimation()
        {
            //move one pixel
            animLavaTileSourceY.Next();
            animWaterTileSourceY.Next();
            waterTile.SourceY = animWaterTileSourceY.Value;
            lavaTile.SourceY = animLavaTileSourceY.Value;
            tileSheetTarget.DrawImagesToTarget(new List<AbsDraw> { waterTile, lavaTile, waterEdge, lavaEdge, allTiles }, true);
        }

        public  void AddImagesToTarget(List<AbsDraw> images)
        {
            tileSheetTarget.DrawImagesToTarget(images, true);
            foreach (AbsDraw img in images)
            {
                img.DeleteMe();
            }
            Graphics.LFHeightMap.InitEffect();
        }

        public  IntVector2 GetTileRowColPos(int tileIx)
        {
            IntVector2 result = IntVector2.Zero;
            result.Y = tileIx / SheetNumTileWidth;
            result.X = tileIx - (result.Y * SheetNumTileWidth);
            return result;
        }
        public  Vector2 GetTilePixPos(int tileIx)
        {
            return (GetTileRowColPos(tileIx).Vec) * TileSize;
        }

    }

    

    
}
