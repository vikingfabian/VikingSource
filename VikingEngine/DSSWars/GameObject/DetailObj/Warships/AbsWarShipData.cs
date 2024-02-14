using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.DetailObj.Warships;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsWarShipData : AbsSoldierData
    {      
        public AbsWarShipData(UnitType shipUnitType, AbsSoldierData soldierData) 
        {
            modelScale = StandardModelScale * 6f;
            boundRadius = StandardBoundRadius * 6f;
            modelToShadowScale = new Vector3(0.5f, 1f, 0.8f);

            hasBannerMan = false;
            rowWidth = 1;
            columnsDepth = 1;
            
            this.unitType = shipUnitType;

            convertSoldierShipType = soldierData.unitType;
            soldierData.convertSoldierShipType = shipUnitType;

            walkingSpeed = AbsSoldierData.StandardShipSpeed;
            rotationSpeed = StandardRotatingSpeed * 0.4f;
            attackDamage = soldierData.attackDamageSea;
            attackDamageSea = soldierData.attackDamageSea;
            attackDamageStructure = soldierData.attackDamageStructure;
            attackTimePlusCoolDown = soldierData.attackTimePlusCoolDown;

            mainAttack = soldierData.mainAttack;

            if (mainAttack == AttackType.Melee)
            {
                attackRange = 0.2f;
            }
            else
            { 
                attackRange = soldierData.attackRange;
            }

            basehealth = soldierData.rowWidth * soldierData.columnsDepth * soldierData.basehealth;
        }

        public override AbsDetailUnit CreateUnit()
        {
            return new BaseWarship();
        }
    }
}
