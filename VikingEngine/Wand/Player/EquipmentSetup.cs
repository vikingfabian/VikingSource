using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Equipment;
using VikingEngine.DSSWars.Players.Profile;

namespace VikingEngine.Core.Wand.Player
{
    class EquipmentSetup
    {
        public const int MaxWandCount = 6;
        public Weapon handWeapon = new Weapon();
        public List2<MagicWand> wands = new List2<MagicWand>(MaxWandCount);
        public int? selectedWand = null;
        public EquipmentSetup()
        {
            for (int i = 0; i < 4; ++i)
            {
                wands.Add(new MagicWand(3));
            }
        }
    }
}
