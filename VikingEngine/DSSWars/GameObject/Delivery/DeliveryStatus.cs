using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
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
        public int itemsCollected;
        public DeliveryProfile profile;
        public DeliveryProfile inProgress;

        public Resource.ItemResource nextDelivery;
        public TimeInGameCountdown countdown;

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
                remaining = DeliveryProfile.DeliveryTime(from, profile.toCity).LongString();
            }
            return string.Format("Delivering {0}", remaining);
        }
    }

    struct DeliveryProfile
    {
        public int toCity;
        public ItemResourceType type;

        public static TimeLength DeliveryTime(City from, int toCity)
        {
            City othercity = DssRef.world.cities[toCity];
            float distance = VectorExt.Length((othercity.tilePos - from.tilePos).Vec);
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
