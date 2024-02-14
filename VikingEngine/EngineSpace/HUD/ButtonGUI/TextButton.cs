using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD
{
    class TextButton : ImageGroupButton
    {
        public TextButton(VectorRect area, ImageLayers layer, ButtonGuiSettings sett, string text, Color textCol, float percentHeight)
            : base(area, layer, sett)
        {
            addCenterText(text, textCol, percentHeight);
        }
        
    }
}
