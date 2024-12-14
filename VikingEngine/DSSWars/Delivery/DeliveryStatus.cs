using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Delivery
{
    struct DeliveryStatus
    {
        public const ItemResourceType DeliveryType_Resource = ItemResourceType.Food_G;
        public const ItemResourceType DeliveryType_Men = ItemResourceType.Men;
        public const ItemResourceType DeliveryType_Gold = ItemResourceType.Gold;


        public const int MaxQue = 20;

        public DeliveryActiveStatus active;

        public bool useSenderMin;
        public int senderMin;

        public bool useRecieverMax;
        public int recieverMax;

        public int level;
        public int idAndPosition;
        public int que;
        //public int itemsCollected;
        public DeliveryProfile profile;
        public DeliveryProfile inProgress;

        //public Resource.ItemResource nextDelivery;
        public TimeInGameCountdown countdown;

        public void halt()
        {
            que = 0;
        }

        public void useSetup(DeliveryStatus setup, LocalPlayer player)
        {
            useSenderMin = setup.useSenderMin;
            senderMin = setup.senderMin;
            useRecieverMax = setup.useRecieverMax;
            recieverMax = setup.recieverMax;
            profile = setup.profile;

            checkCity(player);
        }

        public bool IsGold()
        { 
            return profile.type == DeliveryType_Gold;
        }

        public void checkCity(LocalPlayer player)
        {
            if (profile.toCity == DeliveryProfile.ToCityAuto)
            {
                return;
            }

            var citiesC = player.faction.cities.counter();

            while (citiesC.Next())
            {
                if (citiesC.sel.parentArrayIndex == profile.toCity)
                {
                    return;
                }
            }

            profile.toCity = -1;
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)active);

            w.Write(useSenderMin);
            w.Write(useRecieverMax);

            w.Write((ushort)senderMin);
            w.Write((ushort)recieverMax);

            profile.writeGameState(w);
            if (active != DeliveryActiveStatus.Idle)
            {
                inProgress.writeGameState(w);
            }
            switch (active)
            {
                case DeliveryActiveStatus.Delivering:
                    countdown.writeGameState(w);
                    break;
            }
            w.Write(idAndPosition);
            w.Write((byte)que);

            w.Write((byte)level);
            
            
        }

        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            active = (DeliveryActiveStatus)r.ReadByte();
                       
            useSenderMin = r.ReadBoolean();
            useRecieverMax = r.ReadBoolean();            

            senderMin = r.ReadUInt16();
            recieverMax = r.ReadUInt16();

            profile.readGameState(r, subVersion);
            if (active != DeliveryActiveStatus.Idle)
            {
                inProgress.readGameState(r, subVersion);
            }
            switch (active)
            {
                case DeliveryActiveStatus.Delivering:
                    countdown.readGameState(r);
                    break;
            }
            idAndPosition = r.ReadInt32();
            que = r.ReadByte();

            if (subVersion >= 42)
            {
                level = r.ReadByte();
            }
            else
            {
                level = 1;
            }
        }

        public bool CanSend(City city)
        {
            int min = useSenderMin ? senderMin : 0;
            if (profile.type == ItemResourceType.Men)
            {
                return city.workForce.amount - min >= DssConst.CityDeliveryChunkSize_Level1;
            }
            else
            {
                return city.GetGroupedResource(profile.type).amount - min >= DssConst.CityDeliveryChunkSize_Level1;
            }
        }

        public bool CanRecieve()
        {
            return CanRecieve(profile.toCity, out _);
        }

        public bool CanRecieve(int cityIx, out int recieverHasAmountPlusDeliveries)
        {
            if (arraylib.InBound(DssRef.world.cities, cityIx))
            {
                City city = DssRef.world.cities[cityIx];
                if (profile.type == DeliveryType_Men)
                {
                    recieverHasAmountPlusDeliveries = city.homesTotal() + city.workForce.deliverCount;

                    if (recieverHasAmountPlusDeliveries + DssConst.CityDeliveryChunkSize_Level1 > city.homesTotal())
                    {
                        return false;
                    }
                }
                else
                {

                    recieverHasAmountPlusDeliveries = city.GetGroupedResource(profile.type).amountPlusDelivery();
                }

                if (useRecieverMax)
                {
                    return recieverHasAmountPlusDeliveries < recieverMax;
                }

                return true;

            }
            else
            {
                recieverHasAmountPlusDeliveries = 0;
                return false;
            }
        }

        public bool CountDownQue()
        {
            if (que > 0)
            {
                if (que <= MaxQue)
                {
                    --que;
                }

                return true;
            }

            return false;
        }

        public bool Recruitment()
        {
            return profile.type == ItemResourceType.Men;
        }

        public string shortActiveString()
        {
            string result = null;
            if (active == DeliveryActiveStatus.Delivering)
            {
                result = string.Format("In progress: {0}", countdown.RemainingLength().ShortString());
            }
            else
            {
                result = active.ToString() + ", " + string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_Queue, que <= MaxQue ? que.ToString() : DssRef.lang.Hud_NoLimit);
            }

            return result;
        }
        public string longTimeProgress(City from)
        {
            string remaining;
            if (active == DeliveryActiveStatus.Delivering)
            {
                remaining = countdown.RemainingLength().LongString();
            }
            else
            {
                remaining = DeliveryProfile.DeliveryTime(from, DssRef.world.cities[profile.toCity], level, out _).LongString();
            }
            return string.Format("Delivering {0}", remaining);
        }

        public void defaultSetup(ItemResourceType deliveryType)
        {
            
            profile.toCity = -1;
            profile.type = deliveryType;// ? ItemResourceType.Men : ItemResourceType.Food_G;

            if (deliveryType == DeliveryType_Gold)
            {
                senderMin = 1000;
                recieverMax = 1000;
                profile.SendAmount = DssConst.GoldDeliveryChunkSize_Level1;
            }
            else
            {
                senderMin = 100;
                recieverMax = 100;
                profile.SendAmount = DssConst.CityDeliveryChunkSize_Level1;
            }

            if (level < 1)
            {
                level = 1;
            }
        }

        public void tooltip(LocalPlayer player, City city, RichBoxContent content)
        {
            if (profile.type == ItemResourceType.Men)
            {
                HudLib.ResourceCost(content, ItemResourceType.Men, DssConst.SoldierGroup_DefaultCount, city.workForce.amount);
            }
            else
            {
                HudLib.ResourceCost(content, profile.type, DssConst.SoldierGroup_DefaultCount, city.GetGroupedResource(profile.type).amount);
            }

            content.newLine();
            content.Add(new RichBoxImage(player.input.Stop.Icon));
            content.space(0.5f);
            content.Add(new RichBoxText(shortActiveString()));

            content.newLine();
            content.Add(new RichBoxImage(player.input.Copy.Icon));
            content.space(0.5f);
            content.Add(new RichBoxText(DssRef.lang.Hud_CopySetup));
            content.space(2);
            content.Add(new RichBoxImage(player.input.Paste.Icon));
            content.space(0.5f);
            content.Add(new RichBoxText(DssRef.lang.Hud_Paste));
        }
    }

    struct DeliveryProfile
    {
        public const int ToCityAuto = short.MaxValue;
        public int toCity;
        public int SendAmount;
        public int autoCity;
        public ItemResourceType type;

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((short)toCity);
            w.Write((byte)type);
            w.Write((short)SendAmount);
            if (toCity == ToCityAuto)
            {
                w.Write((short)autoCity);
            }
        }

        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            toCity = r.ReadInt16();
            type = (ItemResourceType)r.ReadByte();

            if (subVersion >= 42)
            {
                SendAmount = r.ReadByte();
            }
            else if (subVersion >= 43)
            {
                SendAmount = r.ReadInt16();
            }
            else
            {
                SendAmount = DssConst.CityDeliveryChunkSize_Level1;
            }

            if (toCity == ToCityAuto)
            {
                autoCity = r.ReadInt16();
            }
            
        }

        public int ToCity()
        {
            if (toCity == ToCityAuto)
            {
                return autoCity;
            }
            else
            {
                return toCity;
            }
        }


        public static TimeLength DeliveryTime(City from, City othercity, int level, out float distance)
        {
            distance = VectorExt.Length((othercity.tilePos - from.tilePos).Vec);
            float time = distance / DssVar.Men_StandardWalkingSpeed_PerSec;
            if (from.Culture == CityCulture.Networker)
            {
                time *= 0.5f;
            }

            if (level == 2)
            {
                time = MathExt.SubtractIntegerPercentage(time, DssConst.DeliveryLevel2TimeReducePerc);
            }
            else if (level == 3)
            {
                time = MathExt.SubtractIntegerPercentage(time, DssConst.DeliveryLevel3TimeReducePerc);
            }

            return new TimeLength(time);
        }

    }

    enum DeliveryActiveStatus
    {
        Idle,
        Delivering,
    }
}
