using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    abstract class AbsArmor : AbsItem
    {
        protected BattleDice defenceDie;
        protected int dieCount = 1;

        public AbsArmor(ArmorType armorType)
            :base(ItemMainType.Armor, (int)armorType)
        { }

        public override string Description => "Gain " + dieCount.ToString() + " " + defenceDie.name;
        
        public override EquipSlots Equip => EquipSlots.Body;

        public override void collectDefence(DefenceData defence, bool onCommit)
        {
            base.collectDefence(defence, onCommit);

            defence.add(defenceDie, dieCount);
        }
    }

    class ArmorLeather : AbsArmor
    {
        public ArmorLeather()
            :base( ArmorType.Leather)
        {
            defenceDie = hqLib.ArmorDie;
        }

        public override SpriteName Icon => SpriteName.cmdLeatherArmor;

        public override string Name => "Leather armor";
    }

    class ArmorElf : AbsArmor
    {
        public ArmorElf( )
            :base(ArmorType.Elf)
        {
            defenceDie = hqLib.DodgeDie;
        }

        public override SpriteName Icon => SpriteName.cmdElfArmor;

        public override string Name => "Elf armor";
    }

    class ArmorChainmail : AbsArmor
    {
        public ArmorChainmail()
            :base(ArmorType.Chainmail)
        {
            defenceDie = hqLib.HeavyArmorDie;
        }

        public override SpriteName Icon => SpriteName.cmdMailArmor;

        public override string Name => "Chainmail armor";
    }

    enum ArmorType
    {
        Leather,
        //Scout,
        Elf,
        Chainmail,
    }
}
