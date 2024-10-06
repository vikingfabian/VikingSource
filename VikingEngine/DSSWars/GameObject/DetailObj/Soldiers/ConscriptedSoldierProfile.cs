using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;

namespace VikingEngine.DSSWars.GameObject
{
    class ConscriptedSoldierProfile : AbsSoldierProfile
    {
        public ConscriptedSoldierProfile()//SoldierProfile profile)
        {
            unitType = UnitType.Conscript;

           
            boundRadius = DssVar.StandardBoundRadius;

           // walkingSpeed = DssConst.Men_StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            //init(profile);
        }

        //public ConscriptedSoldierData(System.IO.BinaryReader r)
        //{
        //    SoldierProfile profile = new SoldierProfile();
        //    profile.readGameState(r);

        //    init(profile);
        //}

        

        //override public void writeGameState(System.IO.BinaryWriter w)
        //{
        //    profile.writeGameState(w);
        //}
        //override public void readGameState(System.IO.BinaryReader r)
        //{
        //    profile.readGameState(r);
        //}
    }
}
