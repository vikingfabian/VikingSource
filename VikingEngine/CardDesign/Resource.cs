using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign
{
    struct Resources
    {
        public static readonly Range CostBounds = new Range(0, MaxValue);
        public const int MaxValue = 999999;

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
            int clampedValue = Math.Clamp(value, -MaxValue, MaxValue);

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
