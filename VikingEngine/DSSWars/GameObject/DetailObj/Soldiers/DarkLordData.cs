using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.GameObject.DetailObj.Soldiers;

namespace VikingEngine.DSSWars.GameObject
{
    class DarkLordData : AbsSoldierData
    {
        public DarkLordData()
        {
            unitType = UnitType.DarkLord;

            modelScale = StandardModelScale;
            boundRadius = StandardBoundRadius;

            walkingSpeed = StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            attackRange = 0.02f;
            basehealth = DefaultHealth * 4;
            mainAttack = AttackType.Melee;
            attackDamage = 500;
            attackDamageStructure = attackDamage;
            attackDamageSea = attackDamage;
            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 0.5f;
            hasBannerMan = false;

            workForcePerUnit = 0;
            goldCost = 0;

            rowWidth = 1;
            columnsDepth = 1;
            ArmyFrontToBackPlacement = ArmyPlacement.Back;

            upkeepPerSoldier = 0;
            recruitTrainingTimeSec = 0;

            modelName = LootFest.VoxelModelName.wars_darklord;
            
            description = "The final boss";
        }

        public override AbsDetailUnit CreateUnit()
        {
            var unit = new DarkLord();
            DssRef.state.darkLordPlayer.darkLordUnit = unit;
            return unit;
        }
    }

    class DarkLord : BaseSoldier
    {
        public DarkLord()
            : base()
        {
            DssRef.state.darkLordPlayer.darkLordUnit = this;
            DssRef.state.events.onDarkLordSpawn();
        }

        public override void onDeath(bool fullUpdate)
        {
            base.onDeath(fullUpdate);

            Ref.update.AddSyncAction(new SyncAction(DssRef.state.events.onDarkLorDeath));
        }
    }

}
