using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign
{
    struct Resources
    {
        

        public int mana;
        public int redMana;
        public int greenMana;
        public int blueMana;
        public int yellowMana;
        public int whiteMana;
        public int blackMana;
        public int coin;
        public int victoryPoint;
        public int wildMana;
        public int actionPoint;

        /// <summary>
        /// Returns the current value of the specified resource type.
        /// </summary>
        public int Get(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Mana: return mana;
                case ResourceType.RedMana: return redMana;
                case ResourceType.GreenMana: return greenMana;
                case ResourceType.BlueMana: return blueMana;
                case ResourceType.YellowMana: return yellowMana;
                case ResourceType.WhiteMana: return whiteMana;
                case ResourceType.BlackMana: return blackMana;

                // New Cases
                case ResourceType.WildMana: return wildMana;
                case ResourceType.ActionPoint: return actionPoint;

                case ResourceType.Coin: return coin;
                case ResourceType.VictoryPoint: return victoryPoint;

                case ResourceType.NUM_NONE:
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Sets the resource to a specific value, clamped between -MaxValue and +MaxValue.
        /// </summary>
        public void Set(ResourceType type, int value)
        {
            // Clamp the value to ensure it stays within bounds
            int clampedValue = Math.Clamp(value, -Const.MaxValue, Const.MaxValue);

            switch (type)
            {
                case ResourceType.Mana: mana = clampedValue; break;
                case ResourceType.RedMana: redMana = clampedValue; break;
                case ResourceType.GreenMana: greenMana = clampedValue; break;
                case ResourceType.BlueMana: blueMana = clampedValue; break;
                case ResourceType.YellowMana: yellowMana = clampedValue; break;
                case ResourceType.WhiteMana: whiteMana = clampedValue; break;
                case ResourceType.BlackMana: blackMana = clampedValue; break;

                // New Cases
                case ResourceType.WildMana: wildMana = clampedValue; break;
                case ResourceType.ActionPoint: actionPoint = clampedValue; break;

                case ResourceType.Coin: coin = clampedValue; break;
                case ResourceType.VictoryPoint: victoryPoint = clampedValue; break;

                default: break;
            }
        }

        /// <summary>
        /// Adds (or subtracts if negative) the amount to the specified resource.
        /// </summary>
        public void Add(ResourceType type, int add)
        {
            if (type == ResourceType.NUM_NONE) return;

            int current = Get(type);
            Set(type, current + add);
        }

        /// <summary>
        /// Returns true if any resource field has a non-zero value.
        /// </summary>
        public bool HasValue
        {
            get
            {
                return mana != 0 ||
                       redMana != 0 ||
                       greenMana != 0 ||
                       blueMana != 0 ||
                       yellowMana != 0 ||
                       whiteMana != 0 ||
                       blackMana != 0 ||
                       wildMana != 0 ||
                       actionPoint != 0 ||
                       coin != 0 ||
                       victoryPoint != 0;
            }
        }

        public void ToMenu(RichBoxContent content)
        {
            if (HasValue)
            {
                for (ResourceType type = 0; type < ResourceType.NUM_NONE; ++type)
                {
                    int value = Get(type);
                    if (value != 0)
                    {
                        IconName.Resource(type, out var icon, out _);
                        content.Add(new RbText(value.ToString()));
                        content.hspace();
                        content.Add(new RbImage(icon));
                        content.space(2);
                    }
                }
            }
            else
            {
                content.Add(new RbText("None"));
            }
        }
    }

    enum ResourceType
    {
        ActionPoint,
        Mana,
        RedMana,
        GreenMana,
        BlueMana,
        YellowMana,
        WhiteMana,
        BlackMana,
        WildMana,
        Coin,
        VictoryPoint,
        NUM_NONE
    }
}
