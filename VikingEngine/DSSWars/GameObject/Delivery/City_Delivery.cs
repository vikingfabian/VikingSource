using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Component;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.GameObject.Delivery;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public int selectedDelivery = 0;
        public List<DeliveryStatus> deliveryServices = new List<DeliveryStatus>();

        public void async_deliveryUpdate()
        {
            lock (deliveryServices)
            {
                for (int i = 0; i < deliveryServices.Count; i++)
                {
                    DeliveryStatus status = deliveryServices[i];
                    switch (status.active)
                    {
                        case DeliveryActiveStatus.Idle:
                            {
                                if (status.que > 0 && status.profile.toCity > 0)
                                {
                                    City othercity = DssRef.world.cities[status.profile.toCity];

                                    if (othercity.faction == this.faction)
                                    {
                                        //if ()
                                        {
                                            //Collecting Items:
                                            if (status.CanSend(this) && status.CanRecieve() &&
                                                status.CountDownQue())
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
                                                status.countdown = new TimeInGameCountdown(DeliveryProfile.DeliveryTime(this, DssRef.world.cities[status.inProgress.toCity], out _));
                                                if (inRender_detailLayer)
                                                {
                                                    Ref.update.AddSyncAction(new SyncAction(() =>
                                                    {
                                                        new ResourceEffect(status.inProgress.type, DssConst.CityDeliveryCount,
                                                           WP.SubtileToWorldPosXZgroundY_Centered(conv.IntToIntVector2(status.idAndPosition)),
                                                           ResourceEffectType.Deliver);
                                                    }));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                            

                        case DeliveryActiveStatus.Delivering:
                            if (status.countdown.TimeOut())
                            {
                                City othercity = DssRef.world.cities[status.inProgress.toCity];
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
        
    }
}
