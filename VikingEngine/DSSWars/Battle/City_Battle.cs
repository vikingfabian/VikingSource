using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public void asynchFindBattleTarget()
        {
            //float defence = 0;

            //DssRef.world.unitCollAreaGrid.collectArmies(faction, tilePos, 1,
            //    DssRef.world.unitCollAreaGrid.armies_nearUpdate);

            //foreach (var m in DssRef.world.unitCollAreaGrid.armies_nearUpdate)
            //{
            //    if (m.tilePos.SideLength(tilePos) <= 4)
            //    {
            //        defence += m.strengthValue;
            //    }
            //}

            //ai_armyDefenceValue = defence;

            detailObj.asynchFindBattleTarget();
        }
    }
}
