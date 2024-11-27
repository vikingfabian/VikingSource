using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Players.Orders
{
    class DemolishOrder : AbsBuildOrder
    {
        public DemolishOrder()
        { }
        public DemolishOrder(int priority, bool bLocalPlayer, City city, IntVector2 subTile)
        {
            baseInit(priority);
            this.city = city;
            this.subTile = subTile;
        }

        public override void onAdd()
        {
            createModel(1);
        }

        public WorkQueMember createWorkQue()
        {
            var result = new WorkQueMember(WorkType.Demolish, 0, subTile, priority, 0);
            result.orderId = id;
            return result;
        }

        override public void DeleteMe()
        {
            Debug.CrashIfThreaded();

            model.DeleteMe();
            //icon.DeleteMe();
        }

        public override RichBoxContent ToHud()
        {
            RichBoxContent content = new RichBoxContent();
            content.h2(DssRef.lang.Build_DestroyBuilding);

            return content;
        }

        //public override bool IsBuildOnSubTile(IntVector2 subTile)
        //{
        //    return base.IsBuildOnSubTile(subTile);
        //}

        //public override bool IsConflictingOrder(AbsOrder other)
        //{
        //    var other_d = other.GetDemolish();
        //    if (other_d != null)
        //    {
        //        return this.subTile == other_d.subTile;
        //    }
        //    return false;
        //}

        public override DemolishOrder GetDemolish()
        {
            return this;
        }

        override public OrderType GetWorkType(City city)
        {
            if (this.city == city)
            {
                return OrderType.Demolish;
            }
            return OrderType.NONE;
        }
    }
}
