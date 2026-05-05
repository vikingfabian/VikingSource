using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.PJ.Strategy;

namespace VikingEngine.DSSWars.GameObject
{
    abstract partial class AbsDetailUnit : AbsWorldObject
    {
        public int health;
        public float radius;

        protected bool recievedProjectileAttackWhileIdle = false;
        protected int lockedIncomingDamage = 0;

        public AbsDetailUnit attackTarget = null, nextAttackTarget = null;

        public IntVector2 tilePos = IntVector2.NegativeOne;

        public SoldierGroup group;
        public Rotation1D rotation;
        public SoldierState state = new SoldierState();
        public int updatesCount = 0;

        public SoldierData soldierData;
        public DetailUnitModel model;
        virtual public void netShareUnit() { }

        public float angleDiff(AbsDetailUnit target)
        {
            Rotation1D targetAngle = angleToUnit(target);

            float diff = rotation.AngleDifference(targetAngle);

            return diff;
        }

        public Rotation1D angleToUnit(AbsDetailUnit target)
        {


            Vector3 targetPosDiff = target.position - position;

            if (targetPosDiff.X == 0 && targetPosDiff.Z == 0)
            {
                return 0;
            }

            return Rotation1D.FromDirection(VectorExt.V3XZtoV2(targetPosDiff));
        }

        

        virtual public void takeDamage(int damageAmount, float blockReduce, AbsDetailUnit meleeAttacker, Rotation1D attackDir, Faction enemyFaction, bool fullUpdate, out bool blocked)
        {
            if (health > 0)
            {
                if (Ref.peRnd.Chance_CheckForZero(group.damageBlockChance_fromTerrain * blockReduce))
                {
                    blocked = true;
                    return;
                }                

                if (damageAmount > 0)
                {
                    lockedIncomingDamage -= damageAmount;

                    recievedProjectileAttackWhileIdle = state.idle;
                   
                    health -= damageAmount;

                    if (health <= 0 && localMember)
                    {
                        onDeath(fullUpdate, enemyFaction);
                    }                    

                    if (fullUpdate)
                    {
                        GoreManager.ViewDamage(this, damageAmount, attackDir);
                    }
                }
            }

            blocked = false;

            
        }


        public void lockInAttackDamage(int damageAmount)
        {
            if (damageAmount > 0)
            {
                lockedIncomingDamage += damageAmount;
            }
        }

        abstract public void update(float time, bool fullUpdate);


        abstract protected DetailUnitModel initModel(bool bannerman);

        protected void refreshAttackTarget()
        {
            if (debugTagged)
            {
                lib.DoNothing();
            }
            var attackTarget_sp = attackTarget;

            if (attackTarget_sp != null && attackTarget_sp.defeatedBy(factionIndex))
            {
                attackTarget = null;
            }

            var nextAttackTarget_sp= nextAttackTarget;
            nextAttackTarget = null;
            if (nextAttackTarget_sp != null && !nextAttackTarget_sp.defeatedBy(factionIndex))
            {
                attackTarget = nextAttackTarget_sp;
            }
        }


        public void closestTargetCheck(AbsDetailUnit unit,
            ref AbsDetailUnit closestOpponent,
            ref float closestOpponentDistance)
        {
            float distance = spaceBetweenUnits(unit);

            if (distance < DssConst.MeleeAwareRange)
            {
                if (distance < closestOpponentDistance &&
                   canTargetUnit(unit))
                {                    
                    closestOpponent = unit;
                    closestOpponentDistance = distance;   
                }
            }
            else
            {
                float anglediff = Math.Abs(angleDiff(unit));
                distance += anglediff * 0.1f;

                if (distance < closestOpponentDistance &&
                    canTargetUnit(unit))
                {
                    var data = Profile();

                    if (!data.restrictTargetAngle || anglediff <= data.targetAngle)
                    {
                        closestOpponent = unit;
                        closestOpponentDistance = distance;
                    }                    
                }
            }
        }

        virtual protected AbsMapObject ParentMapObject()
        {
            
            group.army.TryGetTarget(out var tArmy);
            return tArmy;
        }

        virtual protected bool canTargetUnit(AbsDetailUnit unit)
        {
            return true;
        }

        public float distanceToUnit(AbsDetailUnit other)
        {
            return VectorExt.Length(other.position.X - position.X, other.position.Z - position.Z);
        }

        protected float spaceBetweenUnits(AbsDetailUnit other)
        {
            float result = VectorExt.Length(other.position.X - position.X, other.position.Z - position.Z) - 
                radius - other.radius;
            if (result < 0)
            {
                return 0;
            }
            return result;
        }

        virtual public void onNewModel(LootFest.VoxelModelName name, Graphics.VoxelModel master)
        {
            model?.onNewModel(name, master, this);
        }       

        public int missingHealth
        {
            get { return soldierData.basehealth - health; }
        }

        virtual public void onDeath(bool fullUpdate, Faction enemyFaction)
        {
            //onEvent(UnitEventType.Death);
            if (enemyFaction != null && enemyFaction.player.IsLocalPlayer())
            {
                ++enemyFaction.player.GetLocalPlayer().statistics.EnemySoldiersKilled;
            }
            if (group.GetPlayer().IsLocalPlayer())
            {
                ++group.GetPlayer().GetLocalPlayer().statistics.FriendlySoldiersLost;
            }

            if (fullUpdate)
            {
                DeleteMe(DeleteReason.Death, true);
            }
            else
            { 
                Ref.update.AddSyncAction(new SyncAction2Arg<DeleteReason, bool>(DeleteMe, DeleteReason.Death, true));
            }
        }

        
        public void deleteModels()
        {
            if (model != null)
            {                
                model.DeleteMe();
            }
        }

        virtual public void writeNetworkUpdate() { }
        virtual public void readNetworkUpdate(System.IO.BinaryReader r) { }

        virtual public void applyCollisions()
        {
        }

        public float DPS()
        {
            return soldierData.attackDamage / TimeExt.MillsSecToSec(soldierData.attackTimePlusCoolDown);
        }

        public bool Alive()
        {
            return health > 0;
        }
        public bool Dead()
        {
            return health <= 0;
        }

        public override bool defeatedBy(int attackerFaction)
        {
            return health <= 0;
        }

        override public bool aliveAndBelongTo(int faction) 
        { 
            return health > 0;
        }

        public bool Alive_IncomingDamageIncluded()
        {
            return health - lockedIncomingDamage > 0;
        }

        public bool Dead_IncomingDamageIncluded()
        {
            return health - lockedIncomingDamage <= 0;
        }

        public bool localMember
        {
            get {
                var p = player();
                return p != null && p.IsLocal; 
            }
        }

        public Players.AbsPlayer player()
        {
            return GetFaction()?.player;
        }

        virtual public Vector3 projectileStartPos()
        {
            Vector3 pos = position;
            model?.RotateVector(soldierData.attackStart, ref pos);
            return pos;
        }

        abstract public UnitBuildType DetailUnitType();


        abstract public bool IsShipType();

        abstract public bool IsStructure();

        abstract public bool IsSoldierUnit();

        abstract public bool IsSingleTarget();

        virtual public AbsSoldierUnit GetSoldierUnit() { return null; }

        virtual protected bool IsStunned
        {
            get { return false; }
        }

        virtual public int MaxHealth()
        {
            return soldierData.basehealth;
        }
        
        abstract public AbsDetailUnitBuilder Profile();

        public override string TypeName()
        {
            return TextLib.Error;
        }

        public override string ToString()
        {
            string groupName = group == null? "" : " group(" + group.myIndex.ToString() + ")";
            return DetailUnitType().ToString() + "(" + myIndex.ToString() + ")" + groupName + " p" + " area(" + tilePos.X.ToString() + "," + tilePos.Y.ToString() + ")";
        }
    }

    enum UnitEventType
    {
        MoveOrder,
        StartAttack,
        Death,
    }

    
}
