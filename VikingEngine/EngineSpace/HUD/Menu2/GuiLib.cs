using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.HUD
{
    enum GuiMemberSizeType
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

    enum GuiMemberSelectionType
    {
        None, //Selection will jump over this member
        Selectable, //Button
        Scrollable, //Longer text that can be scrolled through
    }

    enum GuiLayoutMode
    {
        SingleColumn,
        MultipleColumns,
    }
}
