using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Players.PlayerControls.Casual
{
    struct CasualRecruitQueueItem
    {
        public CasualSoldierType soldierType;
        public SoldierPurchaseOption soldier;
        public int count;
        

        public CasualRecruitQueueItem(CasualSoldierType soldierType, SoldierPurchaseOption option, int count)
        { 
            soldier = option;
            this.count = count;
        }

        public bool Equals(CasualRecruitQueueItem other)
        {
            return soldierType == other.soldierType;
        }
    }

    

    class CasualControls
    {
        
    }

    class CityCasualProgress
    {
        List<CasualRecruitQueueItem> recruitQueue = new List<CasualRecruitQueueItem>(16);
        int recruitTimeSeconds = -1;

        public void AddRecruit(City city, CasualRecruitQueueItem queueItem)
        {
            if (recruitQueue.Count > 0 && recruitQueue.Last().Equals(queueItem))
            {
                var last = arraylib.Last(recruitQueue);
                {
                    last.count += queueItem.count;
                }
                arraylib.ReplaceLast(recruitQueue, last);
            }
            else
            { 
                recruitQueue.Add(queueItem);
            }
        }

        public void oneSecondUpdate(City city)
        {
            if (recruitTimeSeconds < 0)
            {
                if (recruitQueue.Count > 0)
                {
                    var first = arraylib.First(recruitQueue);
                    recruitTimeSeconds = city.casualRecruitTime_sec(first.soldierType);
                }
            }
            else if(recruitQueue.Count > 0)
            {
                recruitTimeSeconds--;
                if (recruitTimeSeconds < 0)
                {
                    var first = arraylib.First(recruitQueue);

                    //Spawn

                    if (--first.count <= 0)
                    {
                        recruitQueue.RemoveAt(0);
                    }
                    else
                    {
                        arraylib.ReplaceFirst(recruitQueue, first);
                    }
                }
            }
        }
    }
}
