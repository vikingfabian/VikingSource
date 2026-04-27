using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
//using VikingEngine.DSSWars.Battle;

namespace VikingEngine.DSSWars.GameObject
{

    partial class SoldierGroup
    {
        //const float BattlePrepSpeedBoost = 1.4f;

        public bool battleWalkPath = false;
        public float battleQueTime = 0;

      
        public void setBattleNode(Vector3 wp)
        {
            goalWp = wp;
            var soldiersC = soldiers.counter();
            while (soldiersC.Next())
            {
                soldiersC.sel.setBattleNode();
            }
        }

        protected float WalkingSpeed_Battle()
        {
            return soldierData.walkingSpeed * terrainSpeedMultiplier;
        }
    }
}
