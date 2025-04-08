using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;

namespace VikingEngine.DSSWars.GameObject
{
    class ConscriptedSoldierProfile : AbsSoldierProfile
    {
        public ConscriptedSoldierProfile()
        {
            unitType = UnitType.Conscript;
           
            boundRadius = DssVar.StandardBoundRadius;
            //rotationSpeed = SoldierGroupStandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
        }
    }
}
