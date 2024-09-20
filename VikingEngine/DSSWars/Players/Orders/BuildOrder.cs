using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.DSSWars.Map;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Players.Orders
{
    abstract class AbsOrder
    {
        public OrderStatus orderStatus = OrderStatus.Waiting;
        public int priority;
        static int NextId = 0;
        public int id;

        public AbsOrder(int priority)
        {
            this.priority = priority;
            id = NextId++;
        }

        virtual public BuildOrder GetWorkOrder(City city)
        { 
           return null;
        }

        virtual public bool IsBuildOnSubTile(IntVector2 subTile)
        { 
            return false;
        }

        abstract public bool IsConflictingOrder(AbsOrder other);

        virtual public void DeleteMe() { }
    }

    class BuildOrder : AbsOrder
    {
        City city;
        IntVector2 subTile;
        BuildAndExpandType buildingType;
        Graphics.AbsVoxelObj model;
        public BuildOrder(int priority, bool bLocalPlayer, City city, IntVector2 subTile, BuildAndExpandType buildingType)
            :base(priority)
        {
            this.city = city;
            this.subTile = subTile;
            this.buildingType = buildingType;

            if (bLocalPlayer)
            {
                model = DssRef.models.ModelInstance(LootFest.VoxelModelName.buildarea, WorldData.SubTileWidth * 1.4f, false);
                model.AddToRender(DrawGame.UnitDetailLayer);
                model.position = WP.SubtileToWorldPosXZgroundY_Centered(subTile);
            }
        }

        override public void DeleteMe()
        { 
            Debug.CrashIfThreaded();

            model.DeleteMe();
        }
        public override BuildOrder GetWorkOrder(City city)
        {
            if (this.city == city && orderStatus == OrderStatus.Waiting)
            { 
                return this;
            }
            return null;
        }

        public WorkQueMember createWorkQue(out CraftBlueprint blueprint)
        {
            int type = (int)buildingType;
            blueprint = BuildLib.BuildOptions[type].blueprint;//ResourceLib.Blueprint(buildingType);
            var result = new WorkQueMember(WorkType.Build, type, subTile, priority, 0);
            result.orderId = id;
            return result;
        }

        public override bool IsBuildOnSubTile(IntVector2 subTile)
        {
            return this.subTile == subTile;
        }

        public override bool IsConflictingOrder(AbsOrder other)
        {
            return other.IsBuildOnSubTile(subTile);
        }
    }

    enum OrderStatus
    { 
        Waiting,
        Started,
        Complete,
    }
}
