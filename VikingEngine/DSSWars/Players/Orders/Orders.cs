using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Players.Orders
{
    class Orders
    {
        public List<AbsOrder> orders = new List<AbsOrder>();

        public void refreshAvailable(Faction faction)
        {
            for (int i = orders.Count - 1; i >= 0; --i)
            {
                if (!orders[i].refreshAvailable(faction))
                {
                    orders[i].DeleteMe();
                    orders.RemoveAt(i);
                }
            }
        }

        public AbsOrder orderOnSubTile(IntVector2 subTilePos)
        {
            for (int i = 0; i < orders.Count; ++i)
            {
                if (orders[i].IsBuildOnSubTile(subTilePos))
                {
                    return orders[i];
                }
            }

            return null;
        }

        public bool orderConflictingSubTile(IntVector2 subTilePos)
        {
            for (int i = 0; i < orders.Count; ++i)
            {
                if (orders[i].IsBuildOnSubTile(subTilePos))
                {
                    return true;
                }
            }

            return false;
        }

        public void addOrder(AbsOrder order, bool onConflict_Toggle)
        {
            lock (orders)
            {
                //Check for conflicting
                for (int i = 0; i < orders.Count; ++i)
                {
                    if (order.IsConflictingOrder(orders[i]))
                    {
                        orders[i].DeleteMe();
                        orders.RemoveAt(i);

                        if (onConflict_Toggle)
                        {
                            return;
                        }
                    }
                }

                order.onAdd();
                orders.Add(order);
            }
        }

        public void clearAll(City city)
        {
            for (int i = orders.Count - 1; i >= 0; --i)
            {
                if (orders[i].GetWorkOrder(city) != null)
                {
                    orders[i].DeleteMe();
                    orders.RemoveAt(i);
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
            w.Write((ushort)orders.Count);
            foreach (var order in orders)
            {
                order.writeGameState(w);
            }

            Debug.WriteCheck(w);
        }

        public void readGameState(BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {            
            int ordersCount = r.ReadUInt16();
            for (int i = 0; i < ordersCount; i++)
            {
                BuildOrder order = new BuildOrder();
                order.readGameState(r, subversion, pointers);
                orders.Add(order);
            }
            Debug.ReadCheck(r);
        }
    }
}
