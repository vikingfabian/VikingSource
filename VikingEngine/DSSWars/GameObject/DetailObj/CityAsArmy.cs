using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        void updateArmyMembers(float time, bool fullUpdate)
        {
            if (parentArrayIndex == 0)
            {
                lib.DoNothing();
            }

            if (groups.Count > 0)
            {
                if (fullUpdate || !army_isIdle)
                {
                    var groupsC = groups.counter();

                    while (groupsC.Next())
                    {
                        groupsC.sel.update(time, fullUpdate);                        
                    }
                }

                
            }
        }

        public void async_sleepUpate(float time)
        {
            if (!inRender_detailLayer)
            {
                updateArmyMembers(time * Ref.GameTimeSpeed, false);
            }
        }

        public override bool IdleObjetive()
        {
            return true;
        }

        public void asyncPathUpdate(int pathThreadIndex)
        {
            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                if (!groupsC.sel.restingGuardMode)
                {
                    groupsC.sel.asyncPathUpdate(pathThreadIndex);
                }
            }
        }

        protected void async_SoldiersUpdate(bool oneMinute)
        {
            if (groups.Count > 0)
            {
                int count = 0;
                float totalStrength = 0;
                int dps;

                var groupsC = groups.counter();

                while (groupsC.Next())
                {
                    count += groupsC.sel.soldierCount;
                   
                    int health;

                    if (groupsC.sel.isShip)
                    {
                        
                        dps = groupsC.sel.soldierData.DPS_sea();
                        health = groupsC.sel.soldierData.basehealth;
                    }
                    else
                    {
                        dps = groupsC.sel.soldierData.DPS_land();
                        health = groupsC.sel.soldierData.basehealth;
                    }

                    totalStrength += (dps + health * AllUnits.HealthToStrengthConvertion) * groupsC.sel.soldierCount;

                }
                
                this.strengthValue = count;
                soldiersCount = count;
                strengthValue = 2f * totalStrength / AllUnits.AverageGroupStrength;
            }

        }
    }
}
