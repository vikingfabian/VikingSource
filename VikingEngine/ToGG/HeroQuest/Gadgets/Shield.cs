using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    abstract class AbsShieldItem : AbsItem
    {
        public AbsShieldItem(ShieldType shieldType)
            :base(ItemMainType.Shield, (int)shieldType)
        { }
        //override public ItemMainType MainType { get { return ItemMainType.Shield; } }
        
        public override EquipSlots Equip => EquipSlots.SecondHand;
    }

    class ShieldRound : AbsShieldItem
    {
        BattleDice defenceDie = hqLib.ArmorDie;
        bool used = false;

        public ShieldRound()
            : base(ShieldType.Round)
        { }

        public override void OnTurnStart()
        {
            base.OnTurnStart();
            used = false;
        }

        public override void collectDefence(DefenceData defence, bool onCommit)
        {
            base.collectDefence(defence, onCommit);

            if (!used)
            {
                if (onCommit)
                {
                    used = true;
                }

                defence.add(defenceDie);
            }
        }

        public override SpriteName Icon => SpriteName.cmdRoundShield;

        public override string Name => "Round shield";

        public override string Description => "Once per turn, gain one " + defenceDie.name;
        
        //override public int SubType { get { return (int)ShieldType.Round; } }
        
    }

    enum ShieldType
    {
        Round,
    }
}
