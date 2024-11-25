using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.GameObject.DetailObj.Soldiers;
using VikingEngine.DSSWars.GameObject.DetailObj.Warships;

namespace VikingEngine.DSSWars.GameObject
{
    class DarkLordProfile : ConscriptedSoldierProfile
    {
        public DarkLordProfile()
        {
            unitType = UnitType.DarkLord;

            //modelScale = DssConst.Men_StandardModelScale;
            boundRadius = DssVar.StandardBoundRadius;


            //walkingSpeed = DssConst.Men_StandardWalkingSpeed;
            rotationSpeed = StandardRotatingSpeed;
            targetSpotRange = StandardTargetSpotRange;
            data.attackRange = 0.02f;
            data.basehealth = DssConst.Soldier_DefaultHealth * 4;
            data.mainAttack = AttackType.Melee;
            data.attackDamage = 500;
            data.attackDamageStructure = data.attackDamage;
            data.attackDamageSea = data.attackDamage;
            data.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 0.5f;
            hasBannerMan = false;

            
            goldCost = 0;

            
            

            data.modelName = LootFest.VoxelModelName.wars_darklord;

            description = DssRef.lang.UnitType_Description_DarkLord;
        }
        override public UnitType ShipType()
        {
            return UnitType.DarkLordWarship;
        }
        public override AbsSoldierUnit CreateUnit()
        {
            var unit = new DarkLord();
            DssRef.settings.darkLordPlayer.darkLordUnit = unit;
            return unit;
        }
    }

    //class DarkLordWarshipData : KnightWarshipData
    //{
    //    public DarkLordWarshipData(UnitType shipUnitType, AbsSoldierProfile soldierData)
    //        : base(shipUnitType, soldierData)
    //    {

    //        //mainAttack = AttackType.Javelin;
    //        //attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 2.5f;
    //        attackRange = 2f;
    //        //attackDamage = 500;
    //        //attackDamageStructure = attackDamage;
    //        //attackDamageSea = attackDamage;

    //        walkingSpeed *= 1.5f;
    //    }

    //    public override AbsDetailUnit CreateUnit()
    //    {
    //        var unit = new DarkLordWarship();
    //        DssRef.settings.darkLordPlayer.darkLordUnit = unit;
    //        return unit;
    //    }
    //}

    class DarkLordWarshipData : ConscriptedWarshipData
    {
        public DarkLordWarshipData() 
            :base()
        {
            unitType = UnitType.DarkLordWarship;
        }

        public override AbsSoldierUnit CreateUnit()
        {
            var unit = new DarkLordWarship();
            DssRef.settings.darkLordPlayer.darkLordUnit = unit;
            return unit;
        }
    }

    class DarkLord : BaseSoldier
    {
        public DarkLord()
            : base()
        {
            DssRef.state.events.onDarkLordSpawn();
        }

        public override void onDeath(bool fullUpdate, Faction enemyFaction)
        {
            base.onDeath(fullUpdate, enemyFaction);

            Ref.update.AddSyncAction(new SyncAction(DssRef.state.events.onDarkLorDeath));
        }
    }

    class DarkLordWarship : BaseWarship
    {
        public DarkLordWarship()
            : base()
        {
        }

        public override void onDeath(bool fullUpdate, Faction enemyFaction)
        {
            base.onDeath(fullUpdate, enemyFaction);

            Ref.update.AddSyncAction(new SyncAction(DssRef.state.events.onDarkLorDeath));
        }
    }

}
