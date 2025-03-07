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
    }
}
