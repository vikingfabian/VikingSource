using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.ToggEngine.Data
{
    class UnitTag
    {
        //Type: must be destroyed
        public void onDeath()
        {
            HeroQuest.hqRef.setup.conditions.refeshConditions();
        }
    }
}
