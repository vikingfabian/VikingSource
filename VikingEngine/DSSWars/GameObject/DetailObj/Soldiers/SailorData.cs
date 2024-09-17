using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    class SailorData : AbsSoldierData
    {
        public SailorData() 
        {
            unitType = UnitType.Sailor;

            modelScale = DssConst.Men_StandardModelScale;
            boundRadius = DssVar.StandardBoundRadius;

            walkingSpeed = DssConst.Men_StandardWalkingSpeed;
            ShipBuildTimeMultiplier *= 0.75f;
            ArmySpeedBonusSea = 0.5;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 0.04f;
            basehealth = 150;
            mainAttack = AttackType.Melee;
            attackDamage = 30;
            attackDamageStructure = 20;
            attackDamageSea = 100;
            attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;

            setupJavelinCommand();
            modelName = LootFest.VoxelModelName.war_sailor;
            modelVariationCount = 2;

            description = DssRef.lang.UnitType_Description_Sailor;
            icon = SpriteName.WarsUnitIcon_Viking;
        }
    }
}
