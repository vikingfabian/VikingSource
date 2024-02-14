using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.Input;

namespace VikingEngine.DSSWars.Display
{
    class GameHudDisplays:HUD.RichBox.RichboxGui
    {
        public HeadDisplay headDisplay;
        public ObjectDisplay objectDisplay;
        public DiplomacyDisplay diplomacyDisplay;
        public Army otherArmy;
        public GameHudDisplays(LocalPlayer player)
            :base(HudLib.richboxGui, player.input)
        {
            headDisplay = new HeadDisplay(this);
            objectDisplay = new ObjectDisplay(this);
            diplomacyDisplay = new DiplomacyDisplay(this, player);

            parts = new List<HUD.RichBox.RichboxGuiPart>()
            {
                headDisplay, objectDisplay, diplomacyDisplay,
            };
        }

        public Vector2 BottomLeft()
        {
            Vector2 pos = headDisplay.area.LeftBottom;

            for (int i = 1; i < parts.Count; i++)
            {
                if (parts[i].GetVisible()&& parts[i].area.Bottom>pos.Y)
                { 
                    pos.Y = parts[i].area.Bottom;
                }
            }

            return pos;
        }
    }
}
