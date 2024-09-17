using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Soldiers
{
     class Pikeman : AbsSoldierData
    {
        public Pikeman()
        {
            unitType = UnitType.Pikeman;

            modelScale = DssConst.Men_StandardModelScale * 1.6f;
            boundRadius = DssVar.StandardBoundRadius;

            walkingSpeed = DssConst.Men_StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 0.055f;
            basehealth = DssConst.Soldier_DefaultHealth;
            mainAttack = AttackType.Melee;
            attackDamage = 50;
            attackDamageStructure = attackDamage;
            attackDamageSea = attackDamage;
            attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;

            modelName = LootFest.VoxelModelName.wars_piker;

            description = DssRef.lang.UnitType_Description_Soldier;
            icon = SpriteName.WarsUnitIcon_Pikeman;
        }
    }
}
