using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.GameObject.Resource;

namespace VikingEngine.DSSWars.GameObject.Delivery
{
    struct DeliveryStatus
    {
        public const int MaxQue = 20;

        public DeliveryActiveStatus active;
        public int senderMin;
        public int recieverMax;
        public int idAndPosition;
        public int que;
        //public int itemsCollected;
        public DeliveryProfile profile;
        public DeliveryProfile inProgress;

        //public Resource.ItemResource nextDelivery;
        public TimeInGameCountdown countdown;

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)active);
            
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
                    //itemsCollected = 
                    countdown.writeGameState(w);
                    break;
            }
            w.Write(idAndPosition);
            w.Write((byte)que);
        }

        public void readGameState(System.IO.BinaryReader r)
        {
            active = (DeliveryActiveStatus)r.ReadByte();

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
            if (inProgress.type == ItemResourceType.Men)
            {
                return city.workForce - senderMin >= DssConst.CityDeliveryCount;
            }
            else
            {
                return city.GetGroupedResource(inProgress.type).amount - senderMin >= DssConst.CityDeliveryCount;
            }
        }

        public bool CanRecieve()
        {
            City city = DssRef.world.cities[inProgress.toCity];
            if (inProgress.type == ItemResourceType.Men)
            {
                return city.workForce < recieverMax;
            }
            else
            {
                return city.GetGroupedResource(inProgress.type).amount < recieverMax;
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
                result = active.ToString() + ", " + string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Hud_Que, que <= MaxQue ? que.ToString() : DssRef.todoLang.Hud_NoLimit);
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
        CollectingItems,
        Delivering,
    }
}
