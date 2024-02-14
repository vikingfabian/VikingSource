using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map
{
    //class PreparedScreen
    //{

    //    public List<PreparedBlock> FaceBlocks = new List<PreparedBlock>();
    //    public ShortVector2 Position;
    //    bool firstShadows = true;
    //    PreparedBlock[, ,] blockGrid = null;
    //    IntVector2 startXZ;

    //    public PreparedScreen(WorldPosition pos)
    //    {
    //        Position = pos.ScreenIndex;
    //        LfRef.chunks.CollectPreparedBlocks(pos, FaceBlocks);
    //        startXZ = new IntVector2(WorldPosition.SquaresPerScreenXZ * Position.X, WorldPosition.SquaresPerScreenXZ * Position.Y);
    //        AddToShadowGrid2();
    //    }

    //    public PreparedBlock GetBlock(WorldPosition wp)
    //    {
    //        if (wp.GridPos.X >= WorldPosition.SquaresPerScreenXZ || wp.GridPos.Z >= WorldPosition.SquaresPerScreenXZ)
    //        {
    //            return null;
    //        }
    //        return blockGrid[wp.GridPos.X - startXZ.X, wp.GridPos.Y, wp.GridPos.Z - startXZ.Y];
    //    }
    //    public void AddToShadowGrid2()
    //    {
    //        if (blockGrid == null)
    //        {
    //            blockGrid = new PreparedBlock[WorldPosition.SquaresPerScreenXZ,
    //                LfRef.chunks.GetScreen(Position).ScreenHeight + 1, 
    //                WorldPosition.SquaresPerScreenXZ];
                
    //            foreach (PreparedBlock block in FaceBlocks)
    //            {
    //                blockGrid[block.Position.X - startXZ.X, block.Position.Y, block.Position.Z - startXZ.Y] = block;
    //            }
    //        }
    //    }
    //    //public void AddToShadowGrid(PreparedBlock[, ,] blockGrid, RangeIntV3 gridRange)
    //    //{
    //    //    foreach (PreparedBlock block in FaceBlocks)
    //    //    {
    //    //        if (gridRange.WithinRange(block.Position))
    //    //        {
    //    //            blockGrid[block.Position.X - gridRange.Min.X, block.Position.Y - gridRange.Min.Y, block.Position.Z - gridRange.Min.Z] = block;
    //    //        }
    //    //    }
    //    //    firstShadows = true;
    //    //}
    //    public void ResetShadows()
    //    {
    //        if (!firstShadows)
    //        {
    //            foreach (PreparedBlock block in FaceBlocks)
    //            {
                    
    //                block.ResetDarkness();
    //            }
    //        }
    //        firstShadows = false;
    //    }
    //}

}
