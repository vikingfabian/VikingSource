using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.HUD
{
    class DynamicButton : AbsButtonGui
    {
        public DynamicButton(ButtonGuiSettings sett)
            : base(sett)
        { }

        public void InitDynamicButton(Graphics.Image buttonImage)
        {
            this.baseImage = buttonImage;
            createHighlight();
            refresh();
        }
    }


}
