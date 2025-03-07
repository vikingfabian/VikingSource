using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Soldiers
{
    class CityGuardSoldierProfile : AbsSoldierProfile
    {
        public CityGuardSoldierProfile()
        {
            unitType = UnitType.CityGuard;

            boundRadius = DssVar.StandardBoundRadius;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            hasBannerMan = false;
        }
    }
}
