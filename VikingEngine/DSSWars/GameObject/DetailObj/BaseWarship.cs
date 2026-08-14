using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.ObjectPointer;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Warships
{
    class BaseWarship : AbsSoldierUnit
    {
        const float ShipAttackCooldownMulti = 1;
        int shipCrewCount;
        int shipMultiAttackCount;
        float shipMultiAttackTimeCooldown;

        public BaseWarship()
            : base()
        {
        }
        public override void InitLocal(Vector3 center, IntVector2 gridPlacement, IntVector2 tile, SoldierGroup group)
        {
            
            base.InitLocal(center, gridPlacement, tile, group);

            refreshShipCarryCount();
        }
        override public void refreshShipCarryCount()
        {
            shipCrewCount = MathExt.Div_Ceiling(this.health, group.soldierData_soldier.basehealth);
            if (shipCrewCount > 0)
            {
                shipMultiAttackCount = Math.Min(shipCrewCount, group.soldierData_soldier.rowWidth);
                shipMultiAttackTimeCooldown = group.soldierData_soldier.attackTimePlusCoolDown / (shipCrewCount / shipMultiAttackCount);
                shipMultiAttackTimeCooldown *= ShipAttackCooldownMulti; 
            }
        }

        public override void takeDamage(int damageAmount, float blockReduce, AbsSoldierUnit meleeAttacker, Rotation1D attackDir, PFaction damageFaction, bool fullUpdate, out bool blocked)
        {
            base.takeDamage(damageAmount, blockReduce, meleeAttacker, attackDir, damageFaction, fullUpdate, out blocked);

            if (!blocked)
            {
                refreshShipCarryCount();
                model?.displayHealth(health / (float)soldierData.basehealth);
            }
        }

        public override bool IsShipType()
        {
            return true;
        }

        protected override DetailUnitModel initModel(bool bannerman)
        {
            var model = new ShipUnitAdvancedModel(this);
            model.displayHealth(health / (float)soldierData.basehealth);
            return model;
        }
        public override Vector3 projectileStartPos()
        {
            Vector3 pos = position;
            pos.Y += DssConst.Men_StandardModelScale * 0.7f;
            pos.X += Ref.peRnd.Plus_MinusF(0.1f);
            pos.Z += Ref.peRnd.Plus_MinusF(0.1f);

            return pos;
        }
        protected override void commitAttack(bool fullUpdate)
        {
            startMultiAttack(fullUpdate, attackTarget, true, shipMultiAttackCount, true);
            attackCooldownTime.MilliSeconds = shipMultiAttackTimeCooldown;
        }

        public override bool IsSingleTarget()
        {
            return true;
        }
        public override void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            isDeleted = true;
            health = 0;

            deleteModels();

            if (removeFromParent)
            {
                group?.remove(this);
            }
        }
    }
}
