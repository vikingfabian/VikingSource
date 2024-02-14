using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;
using VikingEngine.Network;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class Coins : AbsItem
    {
        public Coins(int count)
            :base(ItemMainType.Coins)
        {
            //if (count == 0)
            //{
            //    lib.DoNothing();
            //}
            this.count = count;
        }

        public override string Name => "Coins";
        public override string Description => "Used to purchase items";

        //public override ItemMainType MainType => ItemMainType.Coins;
        public override EquipSlots Equip => EquipSlots.None;
        public override int StackLimit => MaxStackCount;
        public override SpriteName Icon => SpriteName.cmdCoin;

        //public override int SubType => 0;
    }
}
