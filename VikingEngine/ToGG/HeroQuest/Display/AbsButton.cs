using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VikingEngine.Graphics;
using VikingEngine.HUD;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    abstract class AbsButton : ImageGroupButton
    {
        public AbsButton(VectorRect area, ImageLayers layer)
            : base(area, layer, HudLib.ButtonGuiSettings)
        {
        }

        public void addTooltipText(string title, string text, Dir4 dir, VectorRect? buttonArea = null)
        {
            HudLib.AddTooltipText(tooltip, title, text, dir, this.area, buttonArea);
        }

        public void addTooltipText(List<HUD.RichBox.AbsRichBoxMember> members, 
            Dir4 dir, VectorRect? buttonArea = null)
        {
            HudLib.AddTooltipText(tooltip, members, dir, this.area, buttonArea);
        }


    }
}
