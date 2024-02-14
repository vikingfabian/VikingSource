using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2
{
    class ObsticleBounds
    {
        int objHeight;
        public int heightCheck;
        public const int CheckBelow = 0;
        const int CheckAbove = 2;
        public int radiusCheck;
        int width; //twice radius +1
        //public bool[,,] grid;
       // public short[] heightNESW;
        public List<CollisionBlock> blocks = new List<CollisionBlock>();

        public Map.WorldPosition gridStart;
        public IntVector3 LastBlockPos;

        public ObsticleBounds(int radius, int height)
        {
            this.radiusCheck = radius;
            objHeight = height;
            heightCheck = height + CheckAbove;
            width = radius * PublicConstants.Twice + 1;
            //grid = new bool[width, heightCheck, width];
            gridStart = Map.WorldPosition.EmptyPos;
            //heightNESW = new short[(int)Facing8Dir.NUM];
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
                        if (LfRef.chunks.Get(pos) != 0)
                        {
                            //Ignore block if its possible to slide over
                            if (notJumpableBlock(feetPos, pos))
                            {
                                blocks.Add(new CollisionBlock(getBoundingBox(pos.WorldGrindex)));
                            }
                        }
                    }
                }
            }


            //LastBlockPos = feetPos.LocalBlockGrindex;
            //gridStart = feetPos;
            //gridStart.WorldGrindex.X -= radiusCheck;
            //gridStart.WorldGrindex.Z -= radiusCheck;
            //gridStart.WorldGrindex.Y -= CheckBelow; 

            //IntVector3 pos = IntVector3.Zero;
            //Map.WorldPosition neighborX;
            //for (pos.X = 0; pos.X < width; pos.X++)
            //{
            //    neighborX = gridStart;
            //    neighborX.WorldGrindex.X+=pos.X;
            //    for (pos.Z = 0; pos.Z < width; pos.Z++)
            //    {
            //        Map.WorldPosition neighborZ = neighborX;
            //        neighborZ.WorldGrindex.Z+=pos.Z;
            //        for (pos.Y = 0; pos.Y < heightCheck; pos.Y++)
            //        {
            //            Map.WorldPosition neighbor = neighborZ;
            //            neighbor.WorldGrindex.Y+=pos.Y;

            //            bool block = LfRef.chunks.Get(neighbor) != 0;
            //            grid[pos.X, pos.Y, pos.Z] = block;

            //            if (block && pos.Y > CheckBelow && pos.Y <= (heightCheck - CheckAbove + 1))
            //            {
            //                bool jumpable = pos.Y == CheckBelow + 1;
            //                if (jumpable)
            //                    lib.DoNothing();
            //                IntVector3 collPos = pos;
            //                if (pos.X != radiusCheck || pos.Z != radiusCheck)
            //                {
            //                    collPos.Y--;
            //                }
            //                blocks.Add(new CollisionBlock(getBoundingBox(collPos), jumpable));
            //                //if (jumpable)
            //                //    break;
            //            }
            //        }
            //    }
            //}
            
            //pos = new IntVector3(radiusCheck, CheckBelow, radiusCheck);
            
            //for (int y = heightCheck -(CheckAbove + 1); y >= 0; y--)
            //{
            //    if (grid[radiusCheck, y, radiusCheck])
            //    {
            //        return y + 1 + gridStart.WorldGrindex.Y;
            //    }
            //}
            //return 1 + gridStart.WorldGrindex.Y;
            
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


        Physics.StaticBoxBound getBoundingBox(IntVector3 pos)
        {
            pos.Add(gridStart.WorldGrindex);
            return new Physics.StaticBoxBound(new VectorVolume(
                pos.Vec, Data.Block.TerrainBlockHalfScaleV3));
        }
        int dirToIndex(int dir)
        {
            return dir == -1?  0 : 1;
        }
    }
    struct CollisionBlock
    {
        public Physics.StaticBoxBound BoundingBox;
       // public bool Jumpable;

        public CollisionBlock(Physics.StaticBoxBound BoundingBox)//, bool Jumpable)
        {
            this.BoundingBox = BoundingBox;
            //this.Jumpable = Jumpable; 
        }
    }
}
