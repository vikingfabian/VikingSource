using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.Display
{
    /// <summary>
    /// A window that views the current terrain generating progress
    /// </summary>
    class DebugTerrainGenDisplay :AbsUpdateable
    {
        GO.PlayerCharacter.AbsHero hero;
        Graphics.Image bg, heroIcon, heroFocusIcon;
        Graphics.TextS chunkCount;
        Graphics.TextG[] statusColorText;

        Dictionary<IntVector2, ChunkImage> chunks = new Dictionary<IntVector2, ChunkImage>(32);
        Vector2 chunkImageSz;
        List<IntVector2> clearList = new List<IntVector2>();

        static readonly Color[] OpenStatusColor = new Color[]
        {
            Color.DarkRed,//Closed_0,
            Color.Orange,//HeightMap_1a,
            Color.Purple,//LoadingOrRecievingData_1b,
            Color.Yellow,//Detail_2,
            Color.DarkOliveGreen,//Mesh_3,
            Color.LightGreen,//GameObjects_4,
        };

        public DebugTerrainGenDisplay(Players.Player p)
            :base(true)
        {
            this.hero = p.hero;
            Vector2 size = new Vector2(Engine.Screen.Height * 0.4f);
            chunkImageSz = size * 0.05f;
            bg = new Graphics.Image(SpriteName.WhiteArea, p.localPData.view.safeScreenArea.RightTop,
                size, ImageLayers.Foreground4);
            bg.Xpos -= size.X;
            bg.Color = Color.Black;
            heroIcon = new Graphics.Image(SpriteName.WhiteArea, bg.Center, new Vector2(4), ImageLayers.Foreground1, true);

            heroFocusIcon = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, new Vector2(6), ImageLayers.Foreground1, true);
            heroFocusIcon.Color = Color.Magenta;

            chunkCount = new Graphics.TextS(LoadedFont.Console, bg.Position, Vector2.One, Graphics.Align.Zero, "", Color.White, ImageLayers.Top9);

            statusColorText = new Graphics.TextG[(int)ScreenOpenStatus.NUM];
            for (ScreenOpenStatus status = 0; status < ScreenOpenStatus.NUM; status++)
            {
                var text = new Graphics.TextG(LoadedFont.Console, bg.LeftBottom, Vector2.One, Graphics.Align.Zero,
                    status.ToString(), OpenStatusColor[(int)status], ImageLayers.Top9);
                text.Ypos += (int)status * 20;
                statusColorText[(int)status] = text;
            }
        }

        int timeMark = 0;
        public override void Time_Update(float time)
        {
            timeMark++;
            Vector2 heroChunkPos = VectorExt.V3XZtoV2(hero.Position) / Map.WorldPosition.ChunkWidth;

           heroFocusIcon.Position = (hero.ChunkUpdateCenter.Vec - heroChunkPos) * chunkImageSz + heroIcon.Position;
            

            LfRef.chunks.OpenChunksCounter.Reset();

            while (LfRef.chunks.OpenChunksCounter.Next())
            {
                ChunkImage ch;
                if (!chunks.TryGetValue(LfRef.chunks.OpenChunksCounter.sel.Index, out ch))
                {
                    ch = new ChunkImage();
                    ch.image = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, chunkImageSz * 0.9f, ImageLayers.Foreground3, false);
                    chunks.Add(LfRef.chunks.OpenChunksCounter.sel.Index, ch);
                }
                
                int status = (int)LfRef.chunks.OpenChunksCounter.sel.openstatus;
                if (LfRef.chunks.OpenChunksCounter.sel.generatedGameObjects)
                {
                    status++;
                }
                ch.image.Color = OpenStatusColor[status];
                ch.image.Position = (LfRef.chunks.OpenChunksCounter.sel.Index.Vec - heroChunkPos) * chunkImageSz + heroIcon.Position;
                ch.timeMark = timeMark;
            }


            clearList.Clear();
            foreach (var kv in chunks)
            {
                if (kv.Value.timeMark != timeMark)
                {
                    clearList.Add(kv.Key);
                }
            }
            foreach (var key in clearList)
            {
                chunks[key].image.DeleteMe();
                chunks.Remove(key);
            }

            chunkCount.TextString = "Chunks: " + LfRef.chunks.OpenChunksList.Count.ToString();
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            bg.DeleteMe(); heroIcon.DeleteMe(); heroFocusIcon.DeleteMe();
            chunkCount.DeleteMe();
            foreach (var kv in chunks)
            {
                kv.Value.image.DeleteMe();
            }
            foreach (var txt in statusColorText)
            {
                txt.DeleteMe();
            }
        }

        class ChunkImage
        {
            //public IntVector2 worldPos;
            public Graphics.Image image;
            public int timeMark;
        }
    }

    
}
