using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign
{
    static class IconName
    {
        public static void Resource(ResourceType type, out SpriteName icon, out string name)
        {
            switch (type)
            {
                case ResourceType.WildMana:
                    icon = SpriteName.MissingImage;
                    name = "Wild Mana";
                    break;

                case ResourceType.ActionPoint:
                    icon = SpriteName.LfMenuMoreMenusArrow;
                    name = "Action Point";
                    break;

                case ResourceType.Mana:
                    icon = SpriteName.CardIconMana;
                    name = "Mana";
                    break;

                case ResourceType.RedMana:
                    icon = SpriteName.CardIconManaRed;
                    name = "Red Mana";
                    break;

                case ResourceType.GreenMana:
                    icon = SpriteName.CardIconManaGreen;
                    name = "Green Mana";
                    break;

                case ResourceType.BlueMana:
                    icon = SpriteName.CardIconManaBlue;
                    name = "Blue Mana";
                    break;

                case ResourceType.YellowMana:
                    icon = SpriteName.CardIconManaYellow;
                    name = "Yellow Mana";
                    break;

                case ResourceType.WhiteMana:
                    icon = SpriteName.CardIconManaWhite;
                    name = "White Mana";
                    break;

                case ResourceType.BlackMana:
                    icon = SpriteName.CardIconManaBlack;
                    name = "Black Mana";
                    break;

                case ResourceType.Coin:
                    icon = SpriteName.CardIconCoin;
                    name = "Coin";
                    break;

                case ResourceType.VictoryPoint:
                    icon = SpriteName.CardIconVictoryPoint;
                    name = "Victory Point";
                    break;

                case ResourceType.NUM_NONE:
                default:
                    icon = SpriteName.MissingImage;
                    name = TextLib.Error;
                    break;
            }
        }
    }
}
