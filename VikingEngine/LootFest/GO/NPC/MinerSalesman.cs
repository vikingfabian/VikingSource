using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.NPC
{
    class MinerSalesman : Salesman
    {
        public MinerSalesman(GoArgs args)
            :base(args)
        {  }

        protected override void initSales()
        {
            GoArgs itemArgs = new GoArgs(WorldPos);

            itemArgs.startWp.WorldGrindex.Z += 3;

            itemArgs.startWp.WorldGrindex.X -= 4;
            itemArgs.startWp.SetAtClosestFreeY(1);
            itemArgs.updatePosV3();
            new GO.PickUp.ItemForSale(itemArgs, GO.PickUp.ForSaleType.PickAxe);
            itemArgs.startWp.WorldGrindex.X += 3;
            itemArgs.startWp.SetAtClosestFreeY(1);
            itemArgs.updatePosV3();
            new GO.PickUp.ItemForSale(itemArgs, GO.PickUp.ForSaleType.SpecialAmmoRefill);
            itemArgs.startWp.WorldGrindex.X += 3;
            itemArgs.startWp.SetAtClosestFreeY(1);
            itemArgs.updatePosV3();
            new GO.PickUp.ItemForSale(itemArgs, GO.PickUp.ForSaleType.ItemApple);
            itemArgs.startWp.WorldGrindex.X += 3;
            itemArgs.startWp.SetAtClosestFreeY(1);
            itemArgs.updatePosV3();
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.MinerSalesman; }
        }
    }

   
}
