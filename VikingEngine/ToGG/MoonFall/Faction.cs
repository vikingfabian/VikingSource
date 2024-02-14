using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.MoonFall
{
    class Faction
    {
        public Color color;
        FactionType type;

        public Faction(FactionType type)
        {
            this.type = type;

            switch (type)
            {
                case FactionType.Orc:
                    color = Color.ForestGreen;
                    break;
                case FactionType.Human:
                    color = Color.Blue;
                    break;
                case FactionType.Elf:
                    color = Color.Orange;
                    break;
                case FactionType.Undead:
                    color = Color.DarkGray;
                    break;
                case FactionType.Chaos:
                    color = Color.Purple;
                    break;

            }
        }
    }

    enum FactionType
    {
        Orc,
        Human,
        Elf,
        Undead,
        Chaos,
        NUM_NON
    }
}
