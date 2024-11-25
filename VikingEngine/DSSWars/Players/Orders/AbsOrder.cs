using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Players.Orders
{
    abstract class AbsOrder
    {
        public OrderStatus orderStatus = OrderStatus.Waiting;
        public int priority;
        static int NextId = 0;
        public int id;

        virtual public void onAdd()
        { }

        public void baseInit(int priority)
        {
            this.priority = priority;
            id = NextId++;
        }

        

        virtual public bool BuildQueue(City city)
        {
            return false;
        }

        virtual public bool IsBuildOnSubTile(IntVector2 subTile)
        {
            return false;
        }

        abstract public bool IsConflictingOrder(AbsOrder other);

        virtual public void DeleteMe() { }

        virtual public RichBoxContent ToHud()
        {
            throw new NotImplementedException();
        }

        virtual public bool refreshAvailable(Faction faction) { return true; }

        virtual public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)priority);
        }
        virtual public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            priority = r.ReadByte();
        }

        virtual public DemolishOrder GetDemolish()
        {
            return null;
        }
        virtual public BuildOrder GetBuild()
        {
            return null;
        }
        virtual public OrderType GetWorkType(City city)
        { 
            return OrderType.NONE;
        }

        
    }

    enum OrderType
    { 
        NONE,
        Build,
        Demolish,
    }
}
