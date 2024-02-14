using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Match3
{
    class PunishMeter
    {
        const int MissedBlocksCount = 2;

        //int missCount = 0;
        int punishCount = 0; 
        //Graphics.Image[] missedBlocks;
        List<Graphics.Image> punishBlocks = new List<Graphics.Image>();
        Vector2 punishStartPos;

        public PunishMeter(BrickBox box)
        {
            //missedBlocks = new Graphics.Image[MissedBlocksCount];

            Vector2 pos = box.area.Position;
            pos.Y -= m3Ref.TileWidth * 3f;

            //for (int i = 0; i < MissedBlocksCount; ++i)
            //{
            //    Graphics.Image miss = new Graphics.Image(SpriteName.m3BrickStone, pos, new Vector2(m3Ref.TileWidth * 0.4f), ImageLayers.Lay4);
            //    missedBlocks[i] = miss;

            //    pos.X += miss.Width * 1.1f;
            //}

            punishStartPos = pos;
            refresh();
        }

        void refresh()
        {
            //for (int i = 0; i < missedBlocks.Length; ++i)
            //{
            //    missedBlocks[i].Color = i < missCount ? Color.White : Color.Black;
            //}

            while (punishBlocks.Count > punishCount)
            {
                arraylib.PullLastMember(punishBlocks).DeleteMe();
            }

            while (punishBlocks.Count < punishCount)
            {
                Graphics.Image punish = new Graphics.Image(SpriteName.m3BrickStone,
                    punishStartPos, new Vector2(m3Ref.TileWidth * 0.8f), ImageLayers.Lay4);
                
                punish.Xpos += punishBlocks.Count * punish.Width * 1.1f;
                punishBlocks.Add(punish);
            }
        }

        public void addMiss()
        {
            //missCount += 2;
            //if (missCount >= MissedBlocksCount)
            //{
            //    missCount = 0;
                punishCount++;
            //}
            refresh();
        }

        public void addPunish(int add)
        {
            punishCount += add;
            refresh();
        }

        public void removePunish(int remove)
        {
            punishCount = Bound.Min(punishCount - remove, 0);
            refresh();
        }

        //public void usedBlock()
        //{
        //    //missCount = Bound.SetMinVal(missCount - 1, 0);
        //    refresh();
        //}

        public void dropPunishBlocks(BrickBox box)
        {
            int row = -1;

            while (punishCount > 0)
            {
                //if (punishCount == 1)
                //{
                //    punishCount = 0;
                //    //Drop about every second column
                //    int count = BrickBox.BrickCountSz.X / 2;

                //    for (int i = 0; i < count; ++i)
                //    {
                //        addStoneBrick(new IntVector2(i * 2 + lib.BoolToInt01(Ref.rnd.Bool()), row), box); 
                //    }
                //}
                //else
                //{
                    punishCount -= 1;
                    IntVector2 pos = new IntVector2(0, row);
                    for (pos.X = 0; pos.X < BrickBox.BrickCountSz.X; ++pos.X)
                    {
                        addStoneBrick(pos, box);
                    }
                //}
                row--;
            }

            refresh();
        }

        void addStoneBrick(IntVector2 pos, BrickBox box)
        {
            //if (box.grid.Get(pos) == null)
            //{
                new Brick(BrickColor.Stone, box, pos);
            //}
            //else if (pos.Y == 0)
            //{
            //    box.gamer.onDeath();
            //}
        }
        
        public bool HasPunishment { get { return punishCount > 0; } }
    }
}
