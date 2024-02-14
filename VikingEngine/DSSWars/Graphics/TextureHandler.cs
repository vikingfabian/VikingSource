//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using VikingEngine.Graphics;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DSSWars
//{
//    class TextureHandler : Engine.AbsSpriteSheetLayout
//    {
//        const int TileSize = 48;
//        const int TileWidthCount = 20;
//        const int TextureSize = TileSize * TileWidthCount;

//        const int ColorAreaSize = TileSize / 3;
//        public const int FlagAreaSize = ColorAreaSize * 2;

//        List<Graphics.AbsDraw> renderList;
//        Graphics.RenderTargetImage renderTarget;

//        public TextureHandler()
//        {
//            //Ref.draw.DontAddNextRenderObject();
//            renderTarget = new Graphics.RenderTargetImage(Vector2.Zero, new Vector2(TextureSize), ImageLayers.NUM, false);
//            Engine.LoadContent.SetTargetTexture(renderTarget.renderTarget);

//            //Generate the textures for all faction flags and icons
//            this.Settings(TextureSize, TileWidthCount);
//            this.TileSheetIx = LoadedTexture.TargetColor0;
//            for (int i = 0; i < DssRef.world.factions.Count; ++i)
//            {
//                Faction faction = DssRef.world.factions[i];
//                faction.FlagTextureTargetSheetPos = setNextPositon((int)SpriteName.rtsFactionFlagTexureStart + i);
//            }
            

//            beginGenerateTextures();
//        }

//        void beginGenerateTextures()
//        {
//            new Timer.AsynchActionTrigger(asynchGeneratingPart1);
//        }

//        void asynchGeneratingPart1()
//        {
//            //renderList = new List<Graphics.AbsDraw>(1 * DssRef.world.factions.Count);
//            //Vector2 iconSz = new Vector2(TileSize);
//            //Vector2 flagSz = new Vector2(FlagAreaSize + 1f);
//            //Vector2 colSz = new Vector2(ColorAreaSize);

//            //for (int i = 0; i < DssRef.world.factions.Count; ++i)
//            //{
//            //    Faction faction = DssRef.world.factions[i];


//            //    Graphics.Image baseTexture = new Graphics.Image(SpriteName.rtsFlagTextureBase, faction.FlagTextureTargetSheetPos, iconSz, ImageLayers.Background1, false, false);
//            //    renderList.Add(baseTexture);

//            //    if (faction.Owner is AbsHumanPlayer)
//            //    {
//            //        AbsHumanPlayer player = faction.Owner as AbsHumanPlayer;
//            //        PixelImage flagIcon = new PixelImage(faction.FlagTextureTargetSheetPos, flagSz, ImageLayers.Lay1, false, new IntVector2(DssLib.UserHeraldicWidth), false);
//            //        flagIcon.pixelTexture.SetData(player.profile.flagDesign.toColorArray(player.profile));

//            //        renderList.Add(flagIcon);
//            //    }
//            //    else
//            //    {
//            //        Image aiBG = new Image(SpriteName.WhiteArea, faction.FlagTextureTargetSheetPos, flagSz, ImageLayers.Lay2, false, false);
//            //        aiBG.Color = faction.Owner.Color1;
//            //        Image aiIcon = new Image(SpriteName.rtsAIFlaySymbol, faction.FlagTextureTargetSheetPos, flagSz, ImageLayers.Lay1, false, false);
//            //        aiIcon.Color = faction.Owner.Color2;

//            //        renderList.Add(aiBG);
//            //        renderList.Add(aiIcon);
//            //    }

//            //    Graphics.Image col1Tex = new Image(SpriteName.WhiteArea, faction.FlagTextureTargetSheetPos, colSz, ImageLayers.Lay1, false, false);
//            //    col1Tex.Xpos += FlagAreaSize;
//            //    col1Tex.Color = faction.Owner.Color1;

//            //    Graphics.Image col2Tex = new Image(SpriteName.WhiteArea, col1Tex.Position, colSz, ImageLayers.Lay1, false, false);
//            //    col2Tex.Ypos += ColorAreaSize;
//            //    col2Tex.Color = faction.Owner.Color2;

//            //    renderList.Add(col1Tex);
//            //    renderList.Add(col2Tex);
                


//            //}

//            //new Timer.Action0ArgTrigger(completeGeneratingPart2);
//        }

//        void completeGeneratingPart2()
//        {
//            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            
//            renderTarget.DrawImagesToTarget(renderList, true);
//            renderList = null;

//            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

//            for (int i = 0; i < DssRef.world.factions.Count; ++i)
//            {
//                DssRef.world.factions[i].OnFlagtextureLoaded();
//            }
//        }


//        Vector2 setNextPositon(int SpriteName)
//        { 
//            int tileX = currentIndex;
//            int tileY = tileIxYpos(ref tileX);

//            add((SpriteName)SpriteName);
//            return new Vector2(tileX * TileSize, tileY * TileSize);
//        }
//    }
//}
