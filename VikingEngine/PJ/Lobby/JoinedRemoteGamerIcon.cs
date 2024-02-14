using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ
{
    class JoinedRemoteGamerIcon
    {
        const int RowCount = 4;

        public static VectorRect GamerIconPlacement(int index)
        {
            Vector2 spacing = new Vector2(Engine.Screen.IconSize * 0.3f);
            Vector2 iconSize = new Vector2(Engine.Screen.IconSize * 6.04f, Engine.Screen.IconSize * 1.4f);

            Vector2 startPos = new Vector2(Engine.Screen.CenterScreen.X, Engine.Screen.SafeArea.PercentToPosition(new Vector2(0.63f)).Y);
            startPos.X = Table.CenterTableWidth(startPos.X, iconSize.X, spacing.X, RowCount);

            return Table.CellPlacement(startPos, false, index, RowCount, iconSize, spacing);
        }
    }
}
