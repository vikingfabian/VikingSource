using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Display
{
    class OptionsGroup
    {
        public int selected = -1;
        public int mouseOver = -1;
        public MenuOptionButton[] buttons;
        
        public bool update()
        {
            if (buttons != null)
            {
                mouseOver = -1;

                for (int i = 0; i < buttons.Length; ++i)
                {
                    if (mouseOver >= 0)
                    {
                        buttons[i].emptyUpdate();
                    }
                    else
                    {
                        if (buttons[i].update())
                        {
                            select(i);
                            return true;
                        }

                        if (buttons[i].mouseOver)
                        {
                            mouseOver = i;
                        }
                    }
                }
            }
            return false;
        }

        public void select(int ix)
        {
            if (selected != ix)
            {
                selected = ix;
                for (int i = 0; i < buttons.Length; ++i)
                {
                    buttons[i].selectedOption(i == selected);
                }
            }
        }

        public void Clear()
        {
            arraylib.DeleteAndNull(ref buttons);

        }
    }

    class MenuOptionButton : AbsPjButton
    {
        public MenuOptionButton(VectorRect area, SpriteName icon)
            : base(area, HudLib.LayButtons, HudLib.ButtonSettings)
        {
            initIconImg(icon);
        }

        virtual public void selectedOption(bool selected)
        {
            baseImage.SetSpriteName(selected ? SpriteName.pjOptionButtonTexOn : SpriteName.pjOptionButtonTexOff);
        }
    }

    class SessionOptionButton : MenuOptionButton
    {
        public SessionOptionButton(VectorRect area)
            : base(area, SpriteName.WhiteArea)
        {

        }

        override public void selectedOption(bool selected)
        {
            iconImg.Color = selected ? Color.White : AvailableSessionsDisplay.BgCol;
            baseImage.Color = iconImg.Color;
            //baseImage.SetSpriteName(selected ? SpriteName.pjOptionButtonTexOn : SpriteName.pjOptionButtonTexOff);
        }
    }
}
