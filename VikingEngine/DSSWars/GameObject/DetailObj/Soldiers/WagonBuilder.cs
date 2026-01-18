using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Soldiers
{
    class WagonBuilder : ConscriptedSoldierBuilder
    {
        public WagonBuilder() 
            :base()
        { 
            unitBuildType = UnitBuildType.ConscriptWagon;
            
        }
    }
}
