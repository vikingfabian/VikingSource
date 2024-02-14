using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest
{
    class ObsticleBounds
    {
        int objHeight;
        public int heightCheck;
        public const int CheckBelow = 0;
        const int CheckAbove = 2;
        public int radiusCheck;
        int width;
        public List<GO.Bounds.StaticBoxBound> blocks = new List<GO.Bounds.StaticBoxBound>();

        public Map.WorldPosition gridStart;
        public IntVector3 LastBlockPos;

        public ObsticleBounds(int radius, int height)
        {
            this.radiusCheck = radius;
            objHeight = height;
            heightCheck = height + CheckAbove;
            width = radius * PublicConstants.Twice + 1;
            gridStart = Map.WorldPosition.EmptyPos;
            LastBlockPos = IntVector3.Zero;
        }
        static readonly List<IntVector2> corners = new List<IntVector2>
        {
            new IntVector2(0, 1),new IntVector2(1, 1),new IntVector2(1, 0),
            new IntVector2(1,-1), new IntVector2(0,-1), new IntVector2(-1,-1), 
            new IntVector2(-1,0),  new IntVector2(-1, 1), 
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="feetPos"></param>
        /// <returns>Highest center block</returns>
        public void Collect(Map.WorldPosition feetPos)
        {
            blocks.Clear();

            Map.WorldPosition start = feetPos;
            start.WorldGrindex.X -= radiusCheck;
            start.WorldGrindex.Z -= radiusCheck;
            start.WorldGrindex.Y -= CheckBelow;

            IntVector3 end = feetPos.WorldGrindex;
            end.X += radiusCheck + 1;
            end.Z += radiusCheck + 1;
            end.Y += heightCheck;

            Map.WorldPosition pos = Map.WorldPosition.EmptyPos;
            for (pos.Y = start.Y; pos.Y < end.Y; ++pos.Y)
            {
                for (pos.Z = start.Z; pos.Z < end.Z; ++pos.Z)
                {
                    for (pos.X = start.X; pos.X < end.X; ++pos.X)
                    {
                        if (pos.BlockHasColllision())//LfRef.chunks.Get(pos) != 0)
                        {
                            //Ignore block if its possible to slide over
                            if (notJumpableBlock(feetPos, pos))
                            {
                                blocks.Add(getBoundingBox(pos.WorldGrindex));
                            }
                        }
                    }
                }
            }

            
        }

        bool notJumpableBlock(Map.WorldPosition feetPos, Map.WorldPosition pos)
        {
            if (pos.WorldGrindex.Y <= feetPos.Y + 1)
            {
                pos.WorldGrindex.Y++;
                if (LfRef.chunks.GetScreen(pos).HasFreeSpaceUp(pos, 3))
                {
                    return false;
                }
            }
            return true;
        }


        GO.Bounds.StaticBoxBound getBoundingBox(IntVector3 pos)
        {
            pos.Add(gridStart.WorldGrindex);
            return new GO.Bounds.StaticBoxBound(pos.Vec, Data.Block.TerrainBlockHalfScaleV3, Vector3.Zero);
        }
        int dirToIndex(int dir)
        {
            return dir == -1?  0 : 1;
        }
    }
}
