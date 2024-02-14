using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.HUD
{
    class OptionButtonGroup
    {
        public List<AbsOptionButton> buttons = new List<AbsOptionButton>();
        public AbsOptionButton Selected = null;

        public void update()
        {
            for (int i = 0; i < buttons.Count; ++i)
            {
                if (buttons[i].update())
                {
                    select(i);
                }
            }
        }

        public void select(int index)
        {
            Selected = buttons[index];

            for (int i = 0; i < buttons.Count; ++i)
            {
                buttons[i].Option(i == index);
            }
        }
    }

    abstract class AbsOptionButton : AbsButtonGui
    {
        bool selected = false;
        public object option;

        public AbsOptionButton()
            :base()
        { }

        public AbsOptionButton(ButtonGuiSettings sett, object option)
            :base(sett)
        {
            this.option = option;
        }

        public void Option(bool selected)
        {
            if (this.selected != selected)
            {
                this.selected = selected;
                if (selected)
                {
                    onOptSelect();
                }
                else
                {
                    onOptDeselect();
                }
            }
        }

        virtual protected void onOptSelect()
        {
            if (highlight != null)
            {
                refreshSelectionArea(sett.highlightThickness * 2);
            
                highlight.Visible = true;
            }
        }

        virtual protected void onOptDeselect()
        {
            refreshSelectionArea(sett.highlightThickness);
        }

        protected override void onMouseEnter(bool enter)
        {
            if (!selected)
            {
                base.onMouseEnter(enter);
            }
        }
    }
}
