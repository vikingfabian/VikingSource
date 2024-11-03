using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.Display.Component;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.Players;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public int selectedDelivery = 0;
        public List<DeliveryStatus> deliveryServices = new List<DeliveryStatus>();

        public void async_deliveryUpdate()
        {
            if (debugTagged)
            {
                lib.DoNothing();
            }

            lock (deliveryServices)
            {
                for (int i = 0; i < deliveryServices.Count; i++)
                {
                    DeliveryStatus status = deliveryServices[i];
                    switch (status.active)
                    {
                        case DeliveryActiveStatus.Idle:
                            {
                                if (status.que > 0 && 
                                    status.profile.toCity > 0 &&
                                    status.CanSend(this))
                                {
                                    City othercity = findOtherCity(ref status);//DssRef.world.cities[status.profile.toCity];

                                    if (othercity != null && 
                                        othercity.faction == this.faction )
                                    {
                                        //if ()
                                        //{
                                        //Collecting Items:
                                        //if (status.CanSend(this) &&
                                        //    status.CountDownQue())
                                        //{
                                        if (status.CountDownQue())
                                        {
                                            status.inProgress = status.profile;

                                            if (status.inProgress.type == ItemResourceType.Men)
                                            {
                                                workForce -= DssConst.CityDeliveryCount;
                                            }
                                            else
                                            {
                                                var resource = GetGroupedResource(status.inProgress.type);
                                                resource.amount -= DssConst.CityDeliveryCount;
                                                SetGroupedResource(status.inProgress.type, resource);
                                            }

                                            status.active++;
                                            status.countdown = new TimeInGameCountdown(DeliveryProfile.DeliveryTime(this, othercity, out _));
                                            if (inRender_detailLayer)
                                            {
                                                Ref.update.AddSyncAction(new SyncAction(() =>
                                                {
                                                    new ResourceEffect(status.inProgress.type, DssConst.CityDeliveryCount,
                                                       VectorExt.AddY(WP.SubtileToWorldPosXZgroundY_Centered(conv.IntToIntVector2(status.idAndPosition)), DssConst.Men_StandardModelScale * 2f),
                                                       ResourceEffectType.Deliver);
                                                }));
                                            }
                                        }
                                       // }
                                    }
                                }
                            }
                            break;

                            

                        case DeliveryActiveStatus.Delivering:
                            if (status.countdown.TimeOut())
                            {
                                City othercity = DssRef.world.cities[status.inProgress.ToCity()];
                                if (status.inProgress.type == ItemResourceType.Men)
                                {
                                    if (othercity.workForce + DssConst.CityDeliveryCount > othercity.workForceMax)
                                    {
                                        //Add rest to immigration
                                        int rest = othercity.workForce + DssConst.CityDeliveryCount - othercity.workForceMax;
                                        othercity.workForce = othercity.workForceMax;
                                        othercity.immigrants.value += rest;
                                    }
                                    else
                                    {
                                        othercity.workForce += DssConst.CityDeliveryCount;
                                    }
                                }
                                else
                                {
                                    var resource = othercity.GetGroupedResource(status.inProgress.type);

                                    if (status.inProgress.type == ItemResourceType.Food_G &&
                                        resource.amount <= 2 &&
                                        faction.player.IsPlayer())
                                    {
                                        DssRef.achieve.UnlockAchievement_async(AchievementIndex.deliver_food);
                                    }
                                    
                                    resource.amount += DssConst.CityDeliveryCount;
                                    othercity.SetGroupedResource(status.inProgress.type, resource);


                                }
                                status.active = DeliveryActiveStatus.Idle;
                            }
                            break;
                    }

                    deliveryServices[i] = status;
                }
            }

            City findOtherCity(ref DeliveryStatus status)
            {
                if (status.profile.toCity == DeliveryProfile.ToCityAuto)
                {
                    int minAmount = int.MaxValue;
                    City city = null;

                    var citiesC = faction.cities.counter();
                    while (citiesC.Next())
                    {
                        if (citiesC.sel != this && tilePos.SideLength(citiesC.sel.tilePos) <= DssConst.DeliveryMaxDistance)
                        {
                            if (status.CanRecieve(citiesC.sel.parentArrayIndex, out int hasAmount))
                            {
                                if (hasAmount < minAmount)
                                {
                                    minAmount = hasAmount;
                                    city = citiesC.sel;
                                }
                            }
                        }
                    }

                    if (city != null)
                    {
                        status.profile.autoCity = city.parentArrayIndex;
                    }
                    return city;
                }
                else
                {
                    if (status.CanRecieve())
                    {
                        return DssRef.world.cities[status.profile.toCity];
                    }
                    else
                    {
                        return null;
                    }
                }

            }
        }

        public void toggleDeliveryStop()
        {
            toggleDeliveryStop(selectedDelivery);
        }

        public bool toggleDeliveryStop(int index)
        {
            if (arraylib.InBound(deliveryServices, index))
            {
                DeliveryStatus currentStatus = deliveryServices[index];
                currentStatus.que = currentStatus.que > 0 ? 0 : 100;
                deliveryServices[index] = currentStatus;
                return currentStatus.que > 0;
            }
            return false;
        }

        public void copyDelivery(LocalPlayer player)
        {
            copyDelivery(player, selectedDelivery);
        }

        public void copyDelivery(LocalPlayer player, int index)
        {
            if (arraylib.InBound(deliveryServices, index))
            {
                DeliveryStatus currentStatus = deliveryServices[index];
                if (currentStatus.Recruitment())
                {
                    player.menDeliveryCopy = currentStatus;
                }
                else
                {
                    player.itemDeliveryCopy = currentStatus;
                }
            }
        }

        public void pasteDelivery(LocalPlayer player)
        {
            pasteDelivery(player, selectedDelivery);
        }

        public void pasteDelivery(LocalPlayer player, int index)
        {
            if (arraylib.InBound(deliveryServices, index))
            {
                DeliveryStatus currentStatus = deliveryServices[index];
                if (currentStatus.Recruitment())
                {
                    currentStatus.useSetup(player.menDeliveryCopy, player);
                }
                else
                {
                    currentStatus.useSetup(player.itemDeliveryCopy, player);
                }
                deliveryServices[index] = currentStatus;
            }
        }

        public void addDelivery(IntVector2 subPos, bool recruitment)
        {
            DeliveryStatus deliveryStatus = new DeliveryStatus()
            {
                idAndPosition = conv.IntVector2ToInt(subPos),
            };

            deliveryStatus.defaultSetup(recruitment);

            lock (deliveryServices)
            {
                deliveryServices.Add(deliveryStatus);
            }
        }


        public int deliveryIxFromSubTile(IntVector2 subTilePos)
        {
            int id = conv.IntVector2ToInt(subTilePos);
            for (int i = 0; i < deliveryServices.Count; ++i)
            {
                if (deliveryServices[i].idAndPosition == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool GetDelivery(IntVector2 subTilePos, out DeliveryStatus status)
        {
            var index = deliveryIxFromSubTile(subTilePos);
            if (arraylib.InBound(deliveryServices, index))
            {
                status = deliveryServices[index];
                return true;
            }

            status = new DeliveryStatus();
            return false;
        }
    }
}
