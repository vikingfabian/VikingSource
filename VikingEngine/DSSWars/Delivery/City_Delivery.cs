using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.Interface.Component;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.Players;
using VikingEngine.Network;
using VikingEngine.PJ.Bagatelle;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public int selectedDelivery = 0;
        public List<DeliveryStatus> deliveryServices = new List<DeliveryStatus>();

        public void async_deliveryUpdate()
        {
            if (debugTagged || myIndex == 1049)
            {
                lib.DoNothing();
            }

            var f = pfaction.GetFaction();
            if (f == null)
                return;

            lock (deliveryServices)
            {
                for (int i = 0; i < deliveryServices.Count; i++)
                {
                    DeliveryStatus status = deliveryServices[i];

                    if (status.inProgress.ToCity() == 253)
                    {
                        lib.DoNothing();
                    }

                    switch (status.active)
                    {
                        case DeliveryActiveStatus.Idle:
                            {
                                if (f.player.IsBot() ||
                                    (automateCity && status.que == 0))//OR fully auto
                                {
                                    status.profile.toCity = DeliveryProfile.ToCityAuto;
                                    status.que = 100;
                                    status.recieverMax = 100;
                                    status.useRecieverMax = true;
                                    if (!status.IsRecruitment() && !status.IsGold())
                                    {
                                        status.inProgress.type = ItemResourceType.AutomatedItem;

                                    }
                                }

                                if (status.que > 0 &&
                                    status.profile.toCity > 0 &&
                                    status.CanSend(this, out ItemResourceType sendItem))
                                {
                                    City othercity = findOtherCity(sendItem, ref status);

                                    if (othercity != null)
                                    {
                                        bool correctOwner = othercity.pfaction == this.pfaction;
                                        if (!correctOwner && DssRef.world.diplomacy.GetRelation(pfaction, othercity.pfaction).Relation >= RelationType.RelationType2_Good)
                                        {
                                            correctOwner = true;
                                            if (!othercity.IsNetHosted && status.remoteDeliveryUpdateRequest.minPassed(5))
                                            { 
                                                var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssDeliverStatusRequest, Network.PacketReliability.Unrelyable,
                                                    Network.SendPacketTo.OneSpecific, othercity.NetHostingPeer().fullId, out var packet);
                                                Net.ObjectId.WriteCity(w, othercity);
                                                w.Write((byte)status.profile.type);
                                                packet.EndWrite_Asynch();
                                            }
                                        }

                                        if (correctOwner)
                                        {
                                            if (status.CountDownQue())
                                            {
                                                status.inProgress = status.profile;
                                                status.inProgress.type = sendItem;

                                                if (status.inProgress.type == ItemResourceType.Men)
                                                {
                                                    workForce.amount -= status.inProgress.SendAmount;

                                                    othercity.workForce.deliverCount += status.inProgress.SendAmount;
                                                }
                                                else
                                                {
                                                    AddGroupedResource(status.inProgress.type, -status.inProgress.SendAmount);

                                                    var resource_recieve = othercity.GetGroupedResource(status.inProgress.type);
                                                    resource_recieve.deliverCount += status.inProgress.SendAmount;
                                                    othercity.AddGroupedResource(status.inProgress.type, status.inProgress.SendAmount);
                                                }

                                                status.active++;
                                                status.countdown = new TimeInGameCountdown(DeliveryProfile.DeliveryTime(this, othercity, status.level, out _));
                                                if (inRender_detailLayer)

                                                {
                                                    Ref.update.AddSyncAction(new SyncAction(() =>
                                                    {
                                                        /*new ResourceEffect*/
                                                        SpriteText3D.GetOrCreate().init(status.inProgress.type, status.inProgress.SendAmount,
                                                           VectorExt.AddY(WP.SubtileToWorldPosXZgroundY_Centered(conv.IntToIntVector2(status.idAndPosition)), DssConst.Men_StandardModelScale * 2f),
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

                            bool resetDeliverRecieveValue = Ref.peRnd.Chance(0.05); //Just to adjust any drifting values

                            if (status.countdown.TimeOut())
                            {
                                City othercity = DssRef.world.cities[status.inProgress.ToCity()];

                                if (othercity.pfaction != pfaction && !othercity.IsNetHosted)
                                {
                                    //Send over net
                                    NetWriteDelivery(ref status, othercity);
                                    status.remoteDeliveryUpdateRequest.setNow();
                                }

                                if (status.inProgress.type == ItemResourceType.Men)
                                {
                                    othercity.addWorkers(status.inProgress.SendAmount);
                                    
                                    if (resetDeliverRecieveValue)
                                    {
                                        othercity.workForce.deliverCount = 0;
                                    }
                                    othercity.workForce.deliverCount = Bound.Min( othercity.workForce.deliverCount - status.inProgress.SendAmount, 0);
                                }
                                else
                                {
                                    var resource = othercity.GetGroupedResource(status.inProgress.type);

                                    if (status.IsGold() &&
                                        pfaction.GetPlayer().IsLocalPlayer())
                                    {
                                        DssRef.achieve.UnlockAchievement_async(AchievementIndex.gold_deliver);
                                    }

                                    resource.amount += status.inProgress.SendAmount;
                                    resource.deliverCount = Bound.Min(resource.deliverCount - status.inProgress.SendAmount, 0);
                                    othercity.SetGroupedResource(status.inProgress.type, resource);
                                }
                                status.active = DeliveryActiveStatus.Idle;
                            }
                            break;
                    }

                    deliveryServices[i] = status;
                }
            }

            City findOtherCity(ItemResourceType sendItem, ref DeliveryStatus status)
            {
                if (status.profile.toCity == DeliveryProfile.ToCityAuto)
                {
                    int minAmount = int.MaxValue;
                    City foundcity = null;

                    //var citiesC = GetFaction().cities.counter();
                    //while (citiesC.Next())
                    //{
                    SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                    while (citiesC.Next(ref pfaction.GetFaction().cities, DssRef.world.cities, out City citySel))
                    {
                        if (citySel != this && tilePos.SideLength(citySel.tilePos) <= DssConst.DeliveryMaxDistance)
                        {
                            if (status.CanRecieve(sendItem, citySel.myIndex, out int hasAmount))
                            {
                                if (hasAmount < minAmount)
                                {
                                    minAmount = hasAmount;
                                    foundcity = citySel;// = citiesC.sel;
                                }
                            }
                        }
                    }

                    if (foundcity != null)
                    {
                        status.profile.autoCity = foundcity.myIndex;
                    }
                    return foundcity;
                }
                else
                {
                    if (status.CanRecieve(sendItem))
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

        void NetWriteDelivery(ref DeliveryStatus status, City recievingCity)
        {
            var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssDeliver, Network.PacketReliability.Reliable,
                Network.SendPacketTo.OneSpecific, recievingCity.NetHostingPeer().fullId, out var packet);
            {
                Net.ObjectId.WriteCityAndOwner(w, this);
                Net.ObjectId.WriteCity(w, recievingCity);
                w.Write((byte)status.inProgress.type);

                int amount = status.inProgress.SendAmount;
                if (status.inProgress.type == ItemResourceType.Gold)
                {
                    amount /= DssConst.GoldDeliveryChunkSize_Mini;
                }
                w.Write((byte)amount);

                w.Write((byte)status.profile.type);

            } packet.EndWrite_Asynch();
        }

        public static void NetReadDelivery(ReceivedPacket packet)
        {
            var sendingCity = Net.ObjectId.ReadCityAndOwner(packet.r);
            var recievingCity = Net.ObjectId.ReadCity(packet.r);
            if (recievingCity != null)
            { 
                ItemResourceType resourceType = (ItemResourceType)packet.r.ReadByte();
                int amount = packet.r.ReadByte();
                if (resourceType == ItemResourceType.Gold)
                {
                    amount *= DssConst.GoldDeliveryChunkSize_Mini;
                }

                recievingCity.AddGroupedResource(resourceType, amount);

                if (recievingCity.pfaction.TryGetPlayer(out var p) && p.IsLocalPlayer())
                {
                    var lp = p.GetLocalPlayer();
                    if (lp.hud.messages.DeliveryMessageTime.minPassed(10) &&
                        sendingCity != null && sendingCity.pfaction.TryGetPlayer(out var r) && r.IsRemotePlayer())
                    {

                        lp.hud.messages.DeliveryMessageTime.setNow();

                        RichBoxContent content = new RichBoxContent();
                        content.h1(SpriteName.WarsBuild_Postal, DssRef.lang.MenuTab_Delivery, HudLib.TitleColor_Head);

                        content.newLine();
                        r.GetRemotePlayer().addNetGamerToHud(content, true, false);
                        content.hspace();
                        content.Add(new RbImage(SpriteName.cmdConvertArrow));
                        content.newLine();

                        recievingCity.CityPresentationHud(new Interface.ObjectHudArgs() { content = content }, true);
                        content.newLine();

                        IconName.Item(resourceType, out SpriteName itemIcon, out string itemName);
                        content.Add(new RbText(TextLib.PlusMinus(amount)));
                        content.space();
                        content.Add(new RbImage(itemIcon));
                        content.hspace();
                        content.Add(new RbText(itemName));

                        lp.hud.messages.Add(content, SoundLib.netMessage);
                    }
                }
                NetWriteDeliveryStatusReply(packet, recievingCity, resourceType);
            }
            
        }
        public static void NetReadDeliveryStatusRequest(ReceivedPacket packet)
        {
            var city = Net.ObjectId.ReadCity(packet.r);
            ItemResourceType resourceType = (ItemResourceType)packet.r.ReadByte();

            if (city != null)
            {
                //Send reply
                NetWriteDeliveryStatusReply(packet, city, resourceType);
            }
        }

        public static void NetWriteDeliveryStatusReply(ReceivedPacket packet, City city, ItemResourceType resourceType)
        {
            var w = Ref.netSession.BeginWritingPacket(PacketType.DssDeliverStatusReply, PacketReliability.Unrelyable, SendPacketTo.OneSpecific, packet.sender.fullId, null);
            Net.ObjectId.WriteCity(w, city);
            w.Write((byte)resourceType);

            if (resourceType == ItemResourceType.AutomatedItem)
            {
                city.writeResources(w);
            }
            else
            {
                city.GetGroupedResource(resourceType).writeNet(w);
            }
        }

        public static void NetReadDeliveryStatusReply(ReceivedPacket packet)
        {
            var city = Net.ObjectId.ReadCity(packet.r);
            ItemResourceType resourceType = (ItemResourceType)packet.r.ReadByte();

            if (city != null)
            {
                if (resourceType == ItemResourceType.AutomatedItem)
                {
                    city.readResources(packet.r, int.MaxValue);
                }
                else
                {
                    city.GetRefGroupedResource(resourceType).readNet(packet.r);
                }
            }
        }

        public ItemResourceType findAutoItem()
        {
            ItemResourceType result = ItemResourceType.NONE;

            for (int i = 0; i < 10; ++i)
            {
                if (find(ResourceLib.MovableCityResource_WeaponMelee))
                {
                    return result;
                }
                if (find(ResourceLib.MovableCityResource_WeaponRanged))
                {
                    return result;
                }
                if (find(ResourceLib.MovableCityResource_Armor))
                {
                    return result;
                }
                if (find(ResourceLib.MovableCityResource_Metals))
                {
                    return result;
                }
                if (find(ResourceLib.MovableCityResource_Misc))
                {
                    return result;
                }
            }
            return result;

            bool find(ItemResourceType[] movableCityResource)
            {                
                ItemResourceType type = movableCityResource[Ref.rnd.Int(movableCityResource.Length)];

                if (GetGroupedResource(type).canTradeAway())
                {
                    result = type;
                    return true;
                }

                return false;
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
            //if (arraylib.InBound(deliveryServices, index))
            //{
            //    DeliveryStatus currentStatus = deliveryServices[index];

            //    switch (currentStatus.profile.type)
            //    {
            //        default:
            //            player.itemDeliveryCopy = currentStatus;
            //            break;
            //        case DeliveryStatus.DeliveryType_Men:
            //            player.menDeliveryCopy = currentStatus;
            //            break;
            //        case DeliveryStatus.DeliveryType_Gold:
            //            player.goldDeliveryCopy = currentStatus;
            //            break;
            //    }
            //}
            CopyPasteDelivery(player, true, index);
        }
        public void pasteDelivery(LocalPlayer player, int index)
        {
            //if (arraylib.InBound(deliveryServices, index))
            //{
            //    DeliveryStatus currentStatus = deliveryServices[index];

            //    switch (currentStatus.profile.type)
            //    { 
            //        default:
            //            currentStatus.useSetup(player.itemDeliveryCopy, player);
            //            break;
            //        case DeliveryStatus.DeliveryType_Men:
            //            currentStatus.useSetup(player.menDeliveryCopy, player);
            //            break;
            //        case DeliveryStatus.DeliveryType_Gold:
            //            currentStatus.useSetup(player.goldDeliveryCopy, player);
            //            break;
            //    }

            //    deliveryServices[index] = currentStatus;
            //}
            CopyPasteDelivery(player, false, index);
        }

        public DeliveryStatus CopyPasteDelivery(LocalPlayer player, bool copy, int index)
        {
            if (arraylib.InBound(deliveryServices, index))
            {
                DeliveryStatus currentStatus = deliveryServices[index];
                ref var playerCopy = ref getDeliveryCopyRef(player, currentStatus.profile.type);

                if (copy)
                {
                    playerCopy = currentStatus;
                }
                else
                { 
                    currentStatus.useSetup(playerCopy, player);
                    deliveryServices[index] = currentStatus;
                }

                return playerCopy;
            }

            return new DeliveryStatus();
        }

        public ref DeliveryStatus getDeliveryCopyRef(LocalPlayer player, ItemResourceType type)
        {
            switch (type)
            {
                default:
                    return ref player.itemDeliveryCopy;
                case DeliveryStatus.DeliveryType_Men:
                    return ref player.menDeliveryCopy;
                case DeliveryStatus.DeliveryType_Gold:
                    return ref player.goldDeliveryCopy;
            }
        }

        public void pasteDelivery(LocalPlayer player)
        {
            if (selectedDelivery < 0)
            {
                pasteDeliveryToAll(player);
            }
            else
            {
                pasteDelivery(player, selectedDelivery);
            }
        }

       

        public void pasteDeliveryToAll(LocalPlayer player)
        {
            for (int i = 0; i < deliveryServices.Count; ++i)
            {
                if (player.deliverySupTab == deliveryServices[i].GetFilterType() ||
                     player.deliverySupTab == ItemResourceType.NUM)
                {
                    pasteDelivery(player, i);
                }
            }
        }

        public void addDelivery(IntVector2 subPos, int level, ItemResourceType deliveryType)
        {
            DeliveryStatus deliveryStatus = new DeliveryStatus()
            {
                idAndPosition = conv.IntVector2ToInt(subPos),
                level = level,
            };

            deliveryStatus.defaultSetup(deliveryType);

            lock (deliveryServices)
            {
                for (int i = 0; i < deliveryServices.Count; ++i)
                {
                    if (deliveryServices[i].idAndPosition == deliveryStatus.idAndPosition)
                    {
                        //Upgrade
                        var prevDelivery = deliveryServices[i];
                        prevDelivery.level = level;
                        deliveryServices[i] = prevDelivery;
                        return;
                    }
                }
                deliveryServices.Add(deliveryStatus);
            }
        }

        public void destroyDelivery(IntVector2 subPos)
        {
            lock (deliveryServices)
            {
                int index = deliveryIxFromSubTile(subPos);
                //deliveryServices[index].returnItems(this);
                deliveryServices.RemoveAt(index);
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
