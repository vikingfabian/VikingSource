using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Soldiers
{
    class CityGuardSoldierBuilder : AbsSoldierBuilder
    {
        public CityGuardSoldierBuilder()
        {
            unitBuildType = UnitBuildType.CityGuard;

            boundRadius = DssVar.StandardBoundRadius;
            //rotationSpeed = SoldierGroupStandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            //hasBannerMan = false;
        }
    }
}
