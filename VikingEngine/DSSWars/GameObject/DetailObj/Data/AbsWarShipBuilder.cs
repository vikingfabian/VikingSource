using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.DetailObj.Warships;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsWarShipBuilder : AbsSoldierBuilder
    {      
        public AbsWarShipBuilder(UnitType shipUnitType) 
        {
            boundRadius = DssVar.StandardBoundRadius * 6f;
            
            this.unitType = shipUnitType;
        }

        public override AbsSoldierUnit CreateUnit()
        {
            return new BaseWarship();
        }

        public override bool IsShip()
        {
            return true;
        }
    }
}
