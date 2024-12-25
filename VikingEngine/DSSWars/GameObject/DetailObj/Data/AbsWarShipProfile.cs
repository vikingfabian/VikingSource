using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.DetailObj.Warships;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsWarShipProfile : AbsSoldierProfile
    {      
        public AbsWarShipProfile(UnitType shipUnitType) 
        {
            //modelScale = DssConst.Men_StandardModelScale * 6f;
            boundRadius = DssVar.StandardBoundRadius * 6f;
            

            hasBannerMan = false;

           
            
            this.unitType = shipUnitType;

            //convertSoldierShipType = soldierData.unitType;
            //soldierData.convertSoldierShipType = shipUnitType;

            //walkingSpeed = DssConst.Men_StandardShipSpeed;
            rotationSpeed = StandardRotatingSpeed * 0.4f;
            //attackDamage = soldierData.attackDamageSea;
            //attackDamageSea = soldierData.attackDamageSea;
            //attackDamageStructure = soldierData.attackDamageStructure;
            //attackTimePlusCoolDown = soldierData.attackTimePlusCoolDown;

            //mainAttack = soldierData.mainAttack;

            //if (mainAttack == AttackType.Melee)
            //{
            //    attackRange = 0.15f;
            //}
            //else
            //{ 
            //    attackRange = soldierData.attackRange;
            //}

            //basehealth = soldierData.rowWidth * soldierData.columnsDepth * soldierData.basehealth;
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
