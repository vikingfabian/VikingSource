using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Players;

namespace VikingEngine.DSSWars.GameObject.Delivery
{
    struct DeliveryStatus
    {
        public const int MaxQue = 20;

        public DeliveryActiveStatus active;
        
        public bool useSenderMin;
        public int senderMin;

        public bool useRecieverMax;
        public int recieverMax;
       
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

        public void checkCity(LocalPlayer player)
        {
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
        }

        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            active = (DeliveryActiveStatus)r.ReadByte();

            if (subVersion >= 14)
            { 
                useSenderMin = r.ReadBoolean();
                useRecieverMax = r.ReadBoolean();
            }

            senderMin = r.ReadUInt16();
            recieverMax = r.ReadUInt16();

            profile.readGameState(r);
            if (active != DeliveryActiveStatus.Idle)
            {
                inProgress.readGameState(r);
            }
            switch (active)
            {                
                case DeliveryActiveStatus.Delivering:
                    countdown.readGameState(r);
                    break;
            }
            idAndPosition = r.ReadInt32();
            que = r.ReadByte();
        }

        public bool CanSend(City city)
        {
            int min = useSenderMin ? senderMin : 0;
            if (profile.type == ItemResourceType.Men)
            {
                return city.workForce - min >= DssConst.CityDeliveryCount;
            }
            else
            {
                return city.GetGroupedResource(profile.type).amount - min >= DssConst.CityDeliveryCount;
            }
        }

        public bool CanRecieve()
        {
            if (useRecieverMax)
            {
                City city = DssRef.world.cities[profile.toCity];
                if (profile.type == ItemResourceType.Men)
                {
                    return city.workForce < recieverMax;
                }
                else
                {
                    return city.GetGroupedResource(profile.type).amount < recieverMax;
                }
            }
            return true;
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
                remaining = DeliveryProfile.DeliveryTime(from, DssRef.world.cities[profile.toCity], out _).LongString();
            }
            return string.Format("Delivering {0}", remaining);
        }

        public void defaultSetup(bool recruitment)
        {
            senderMin = 100;
            recieverMax = 200;
            profile.toCity = -1;
            profile.type = recruitment ? ItemResourceType.Men : ItemResourceType.Food_G;
        }
    }

    struct DeliveryProfile
    {
        public int toCity;
        public ItemResourceType type;

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((short)toCity);
            w.Write((byte)type);
        }

        public void readGameState(System.IO.BinaryReader r)
        {
            toCity = r.ReadInt16();
            type = (ItemResourceType)r.ReadByte();
        }

        public static TimeLength DeliveryTime(City from, City othercity, out float distance)//int toCity)
        {
            //City othercity = DssRef.world.cities[toCity];
            distance = VectorExt.Length((othercity.tilePos - from.tilePos).Vec);
            float time = distance / DssVar.Men_StandardWalkingSpeed_PerSec;
            if (from.Culture == CityCulture.Networker)
            {
                time *= 0.5f;
            }
            return new TimeLength(time);
        }

    }

    enum DeliveryActiveStatus
    {
        Idle,
        //CollectingItems,
        Delivering,
    }
}
