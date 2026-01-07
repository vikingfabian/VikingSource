using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;

namespace VikingEngine.DSSWars.GameObject
{
    class ConscriptedSoldierBuilder : AbsSoldierBuilder
    {
        public ConscriptedSoldierBuilder()
        {
            unitType = UnitType.Conscript;
           
            boundRadius = DssVar.StandardBoundRadius;
            //rotationSpeed = SoldierGroupStandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
        }
    }
}
