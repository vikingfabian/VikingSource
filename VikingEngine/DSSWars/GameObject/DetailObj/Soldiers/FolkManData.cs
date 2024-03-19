﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Soldiers
{
    class FolkManData : AbsSoldierData
    {
        public FolkManData() 
        {
            unitType = UnitType.Folkman;
            goldCost = MathExt.MultiplyInt(0.2, DssLib.GroupDefaultCost);
            upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 0.8f;
            recruitTrainingTimeSec = MathExt.MultiplyInt(0.5, DssLib.DefalutRecruitTrainingTimeSec);

            modelScale = StandardModelScale;
            boundRadius = StandardBoundRadius;

            walkingSpeed = StandardWalkingSpeed * 1.2f;
            ArmySpeedBonusLand = 0.2;
            rotationSpeed = StandardRotatingSpeed * 1.2f;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 0.04f;
            basehealth = 100;
            mainAttack = AttackType.Melee;
            attackDamage = 30;
            attackDamageStructure = attackDamage;
            attackDamageSea = attackDamage;
            attackTimePlusCoolDown = StandardAttackAndCoolDownTime;

            setupJavelinCommand();

            modelName = LootFest.VoxelModelName.war_folkman;
            description = "Cheap untrained soldiers";
            icon = SpriteName.WarsUnitIcon_Folkman;
        }
    }
}
