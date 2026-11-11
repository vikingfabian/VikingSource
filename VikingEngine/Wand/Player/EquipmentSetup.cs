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
            wands.Add(new MagicWand(1));
            wands.Add(new MagicWand(1));
            wands.Add(new MagicWand(1));

        }
    }
}
