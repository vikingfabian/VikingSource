using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest
{
    class DebugChunkLoading : AbsUpdateable
    {
        VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero;
        Grid2D<Graphics.Image> imageGrid;
        Grid2D<Graphics.TextG> letterGrid;
        IntVector2 centerTile;
        Graphics.Image currentLoading;

        public DebugChunkLoading()
            :base(true)
        {
            hero = LfRef.AllHeroes[0];

            imageGrid = new Grid2D<Graphics.Image>(21);
            letterGrid = new Grid2D<Graphics.TextG>(imageGrid.Size);
            centerTile = imageGrid.Size / 2;

            imageGrid.LoopBegin();
            while (imageGrid.LoopNext())
            {
                Vector2 pos = imageGrid.LoopPosition.Vec * (Engine.Screen.IconSize + 2);

                var img = new Graphics.Image(SpriteName.WhiteArea, pos, Engine.Screen.IconSizeV2, ImageLayers.Background5);
                img.Color = Color.Black;
                imageGrid.LoopValueSet(img);

                var letter = new Graphics.TextG(LoadedFont.Console, img.Center, Vector2.One, Graphics.Align.CenterAll,
                    "-", Color.White, ImageLayers.Background3);

                letterGrid.Set(imageGrid.LoopPosition, letter);
            }

            var centerArea = imageGrid.Get(centerTile).Area;
            centerArea.AddRadius(4);
            Graphics.Image centerMark = new Graphics.Image(SpriteName.WhiteArea, centerArea.Position, centerArea.Size, ImageLayers.Background6);

            currentLoading = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Engine.Screen.IconSizeV2 * 0.2f, ImageLayers.Background2);
        }

        public override void Time_Update(float time_ms)
        {
            currentLoading.Visible = false;
            IntVector2 startPos = hero.WorldPos.ChunkGrindex - centerTile;

            if (hero.WorldPos.InsideSafeArea)
            {

                imageGrid.LoopBegin();
                while (imageGrid.LoopNext())
                {
                    IntVector2 pos = imageGrid.LoopPosition + startPos;
                    var c = LfRef.chunks.GetScreenUnsafe(pos);

                    VikingEngine.LootFest.Map.ScreenOpenStatus openstatus;
                    bool compressed = false;

                    if (c == null)
                    {
                        openstatus = Map.ScreenOpenStatus.Closed_0;
                    }
                    else
                    {
                        openstatus = c.openstatus;
                        compressed = c.DataGrid == null;
                    }

                    string openStatusText = openstatus == Map.ScreenOpenStatus.Mesh_3 ? "S" : ((int)openstatus).ToString();
                    string compressionStatusText = compressed ? "C" : "";
                    letterGrid.Get(imageGrid.LoopPosition).TextString = compressionStatusText + openStatusText;

                    Color col;
                    if (openstatus == Map.ScreenOpenStatus.Closed_0)
                    {
                        col = Color.Black;
                    }
                    else if (openstatus == Map.ScreenOpenStatus.Mesh_3)
                    {
                        col = Color.DarkGreen;
                    }
                    else
                    {
                        col = ColorExt.VeryDarkGray;
                    }

                    imageGrid.Get(imageGrid.LoopPosition).Color = col;

                    if (imageGrid.LoopPosition == LfRef.world.currentlyOpeningScreen)
                    {
                        currentLoading.Position = imageGrid.Get(imageGrid.LoopPosition).Position;
                        currentLoading.Visible = true;
                    }
                }
            }
        }
    }
}
