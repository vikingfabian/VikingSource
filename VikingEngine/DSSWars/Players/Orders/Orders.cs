using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Players.Orders
{
    class Orders
    {
        public List<AbsOrder> orders = new List<AbsOrder>();

        public int buildQueue(City city)
        {
            int count = 0;

            lock (orders)
            {
                for (int i = 0; i < orders.Count; ++i)
                {
                    if (orders[i].BuildQueue(city))
                    {
                        ++count;
                    }
                }
            }
            return count;
        }

        public void cullingUpdate(bool bStateA, int playerIx)
        {
            lock (orders)
            {
                foreach (AbsOrder order in orders)
                {
                    order.cullingUpdate(bStateA, playerIx);
                }
            }
        }

        public void refreshAvailable(PFaction pfaction)
        {
            lock (orders)
            {
                for (int i = orders.Count - 1; i >= 0; --i)
                {
                    if (!orders[i].refreshAvailable(pfaction))
                    {
                        orders[i].DeleteMe();
                        orders.RemoveAt(i);
                    }
                }
            }
        }

        public AbsOrder orderOnSubTile(IntVector2 subTilePos)
        {
            lock (orders)
            {
                for (int i = 0; i < orders.Count; ++i)
                {
                    if (orders[i].IsBuildOnSubTile(subTilePos))
                    {
                        return orders[i];
                    }
                }
            }
            return null;
        }

        public bool orderConflictingSubTile(IntVector2 subTilePos, bool removeConflict)
        {
            lock (orders)
            {
                for (int i = 0; i < orders.Count; ++i)
                {
                    if (orders[i].IsBuildOnSubTile(subTilePos))
                    {
                        if (removeConflict)
                        {
                            orders[i].DeleteMe();
                            orders.RemoveAt(i);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public void addOrder(int playerIx, AbsOrder order, ActionOnConflict onConflict, bool mainThread = true)
        {
            lock (orders)
            {
                //Check for conflicting
                for (int i = 0; i < orders.Count; ++i)
                {
                    if (order.IsConflictingOrder(orders[i]))
                    {
                        if (onConflict == ActionOnConflict.Cancel)
                        {
                            return;
                        }

                        orders[i].DeleteMe();
                        orders.RemoveAt(i);

                        if (onConflict == ActionOnConflict.Toggle)
                        {
                            return;
                        }
                    }
                }

                if (mainThread)
                {
                    order.onAdd(playerIx);
                }
                else
                {
                    Ref.update.AddSyncAction(new SyncAction1Arg<int>(order.onAdd, playerIx));
                }
                orders.Add(order);
            }
        }

        public void clearAll(City city)
        {
            lock (orders)
            {
                for (int i = orders.Count - 1; i >= 0; --i)
                {
                    if (orders[i].GetWorkType(city) != OrderType.NONE)
                    {
                        orders[i].DeleteMe();
                        orders.RemoveAt(i);
                    }
                }
            }
        }

        public AbsOrder GetFromId(int id)
        {
            lock (orders)
            {
                foreach (AbsOrder order in orders)
                {
                    if (order.id == id)
                        return order;
                }
            }
            return null;
        }

        public AbsOrder StartOrderId(int id)
        {
            lock (orders)
            {
                foreach (AbsOrder order in orders)
                {
                    if (order.id == id)
                    {
                        order.orderStatus = OrderStatus.Started;
                        break;
                    }
                }
            }
            return null;
        }

        public void CompleteOrderId(int id)
        {
            lock (orders)
            {
                for (int i = 0; i < orders.Count; ++i)
                {
                    if (orders[i].id == id)
                    {
                        Ref.update.AddSyncAction(new SyncAction(orders[i].DeleteMe));
                        orders.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void writeGameState(BinaryWriter w)
        {
            //w.Write((ushort)orders.Count);
            StreamLib.WriteGrowing_UShort_Add(w, orders.Count);
            foreach (var order in orders)
            {
                w.Write((byte)order.Type());
                order.writeGameState(w);
            }

            Debug.WriteCheck(w);
        }

        public void readGameState(int playerIx, BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            int ordersCount;
            if (subversion >= 133)
            {
                ordersCount = StreamLib.ReadGrowing_UShort_Add(r);
            }
            else
            {
                ordersCount = r.ReadUInt16();
            }
//#if DEBUG
//            ordersCount += ushort.MaxValue + 1;
//#endif

            for (int i = 0; i < ordersCount; i++)
            {
                OrderType type = OrderType.Build;
                
                if (subversion >= 52)
                {
                    type = (OrderType)r.ReadByte();
                }

                AbsOrder order;

                switch (type)
                { 
                    default:
                        order = new BuildOrder();
                        break;
                    case OrderType.Demolish:
                        order = new DemolishOrder();
                        break;
                }
                
                
                order.readGameState(playerIx, r, subversion, pointers);

                orders.Add(order);
            }
             Debug.ReadCheck(r);
        }
    }

    enum ActionOnConflict
    { 
        Commit,
        Toggle,
        Cancel,
    }
}
