using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class CheckedSquares
    {
        Grid2D<bool> grid = new Grid2D<bool>(toggRef.board.Size);

        public bool IsUnchecked(IntVector2 pos)
        {
            if (!grid.InBounds(pos))
            {
                return false;
            }
            return !grid.Set_GetOld(pos, true);
        }
    }    
}
