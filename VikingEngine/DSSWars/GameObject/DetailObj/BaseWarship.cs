using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Warships
{
    class BaseWarship : AbsSoldierUnit
    {
        //int storedAttacks = 0;
        int soldierCount;
        int multiAttackCount;
        float multiAttackTimeCooldown;

        //float maxHealth;
        //public UnitType carryUnitType;

        public BaseWarship()
            : base()
        {
        }

        override public void refreshShipCarryCount()
        {
            var defaultSoldier = group.soldierConscript.init(group.typeSoldierData);
            //var data = group.typeCurrentData;//.SoldierData();
            soldierCount = MathExt.Div_Ceiling(this.health, defaultSoldier.basehealth);
            if (soldierCount > 0)
            {
                multiAttackCount = Math.Min(soldierCount, group.soldierData.rowWidth);
                multiAttackTimeCooldown = defaultSoldier.attackTimePlusCoolDown / (soldierCount / multiAttackCount);
            }
            

        }

        public override void takeDamage(int damageAmount, AbsDetailUnit meleeAttacker, Rotation1D attackDir, Faction damageFaction, bool fullUpdate)
        {
            base.takeDamage(damageAmount, meleeAttacker, attackDir, damageFaction, fullUpdate);
            refreshShipCarryCount();
            model?.displayHealth(health / (float)soldierData.basehealth);
        }

        public override bool IsShipType()
        {
            return true;
        }

        protected override DetailUnitModel initModel()
        {
            var model = new ShipUnitAdvancedModel(this);
            model.displayHealth(health / (float)soldierData.basehealth);
            return model;
        }
        public override Vector3 projectileStartPos()
        {
            Vector3 pos = position;
            pos.Y += DssConst.Men_StandardModelScale * 0.7f;
            pos.X += Ref.rnd.Plus_MinusF(0.1f);
            pos.Z += Ref.rnd.Plus_MinusF(0.1f);

            return pos;
        }
        protected override void commitAttack(bool fullUpdate)
        {


            //int attacks = Ref.rnd.Int(5, 10) + Bound.Max(storedAttacks, 5) - 1;

            //var soldiersC = attackTarget.group.soldiers.counter();
            //while (soldiersC.Next())
            //{
            //    if (soldiersC.sel != attackTarget && soldiersC.sel.Alive_IncomingDamageIncluded())
            //    {
            //        --attacks;
            //        startAttack(fullUpdate, soldiersC.sel, true, true);
            //    }

            //    if (attacks <= 0)
            //    {
            //        break;
            //    }
            //}
            //storedAttacks = attacks;
            startMultiAttack(fullUpdate, attackTarget, true, multiAttackCount, true);
            attackCooldownTime.MilliSeconds = multiAttackTimeCooldown;
        }

        public override bool IsSingleTarget()
        {
            return true;
        }
        //protected override DetailUnitModel initModel()
        //{
        //    if (this.parentArrayIndex == 11)
        //    {
        //        return new KnightBannerModel(this);
        //    }
        //    else
        //    {
        //        return new KnightModel(this);
        //    }
        //}
    }
}
