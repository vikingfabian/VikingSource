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
            if (myIndex == 40)
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
            int count = 0;
            float totalStrength = 0;
            Intvector2MinMax minMax = new Intvector2MinMax(tilePos);
            bool allGropsAreIdle = true;

            if (groups.Count > 0)
            {
                var groupsC = groups.counter();

                while (groupsC.Next())
                {
                    count += groupsC.sel.soldierCount;
                    allGropsAreIdle &= groupsC.sel.HasIdleState();
                    totalStrength += AllUnits.GroupStrengh(groupsC.sel.soldierCount, ref groupsC.sel.soldierData, !groupsC.sel.isShip);

                    minMax.Next(ref groupsC.sel.tilePos);
                }
            }

            army_isIdle = allGropsAreIdle;
            soldiersCount = count;
            this.strengthValue = totalStrength;
            guardCullingMinMax = minMax;
        }
    }
}
