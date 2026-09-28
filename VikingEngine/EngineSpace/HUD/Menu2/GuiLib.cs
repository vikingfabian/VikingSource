using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.HUD
{
    public enum GuiMemberSizeType
    {
        FullWidth,
        StandardButtonSize,
        LargeButtonSize,
        HalfHeight,
        SquareHalfSize,
        Square,
        SquareDoubleSize,
        Scrollbar,
        
    }

    public enum GuiMemberSelectionType
    {
        None, //Selection will jump over this member
        Selectable, //Button
        Scrollable, //Longer text that can be scrolled through
    }

    public enum GuiLayoutMode
    {
        SingleColumn,
        MultipleColumns,
    }
}
