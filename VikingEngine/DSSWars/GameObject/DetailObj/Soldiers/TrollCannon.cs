using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Soldiers
{
    class TrollCannon : AbsSoldierData
    {
        public TrollCannon()
        {
            unitType = UnitType.Trollcannon;

            modelScale = StandardModelScale * 1.6f;
            boundRadius = StandardBoundRadius;

            walkingSpeed = StandardWalkingSpeed * 0.6f;
            ArmySpeedBonusLand = -0.5;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 3f;
            basehealth = DefaultHealth * 4;
            mainAttack = AttackType.Cannonball;
            attackDamage = 500;
            attackDamageStructure = 2500;
            attackDamageSea = 600;
            splashDamageCount = 3;
            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 10f;

            maxAttackAngle = 0.07f;

            rowWidth = 3;
            columnsDepth = 2;
            groupSpacing = DefaultGroupSpacing * 2.2f;

            workForcePerUnit = 0;
            goldCost = MathExt.MultiplyInt(2, DssLib.GroupDefaultCost);
            upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;

            modelName = LootFest.VoxelModelName.wars_trollcannon;
            hasBannerMan = false;
            ArmyFrontToBackPlacement = ArmyPlacement.Back;

            description = "Heavy long range units";
            icon = SpriteName.WarsUnitIcon_Trollcannon;
        }
    }
}
