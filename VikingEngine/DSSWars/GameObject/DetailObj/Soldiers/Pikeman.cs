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

            modelScale = StandardModelScale * 1.6f;
            boundRadius = StandardBoundRadius;

            walkingSpeed = StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 0.055f;
            basehealth = DefaultHealth;
            mainAttack = AttackType.Melee;
            attackDamage = 50;
            attackDamageStructure = attackDamage;
            attackDamageSea = attackDamage;
            attackTimePlusCoolDown = StandardAttackAndCoolDownTime;

            modelName = LootFest.VoxelModelName.wars_piker;

            description = DssRef.lang.UnitType_Description_Soldier;
            icon = SpriteName.WarsUnitIcon_Pikeman;
        }
    }
}
