using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace VikingEngine.DSSWars.GameObject
{
    abstract partial class AbsDetailUnit : AbsWorldObject
    {
        //const float DropTargetAdd = 1;
        //public int id;

        public int health;
        public float radius;

        //protected Damage previousDamage = Damage.NoDamage;
        protected bool recievedProjectileAttackWhileIdle = false;
        protected int lockedIncomingDamage = 0;

        //public AbsDetailUnit closestAttackTarget = null;
        public AbsDetailUnit attackTarget = null, nextAttackTarget = null;


        //public AttackAnimation attack;
        public IntVector2 tilePos = IntVector2.NegativeOne;

        public SoldierGroup group;
        public Rotation1D rotation;
        public SoldierState state = new SoldierState();

        //public float walkingSpeedMultiplier = 1f;
        //public float terrainSpeedMultiplier = 1f;

        public int updatesCount = 0;

        //public bool debug_addedByRemote = false;
        //public float attackSoundPitch = 0f;
        //public bool usedInOrderCheck;

        //public Vector3 position;

        const float AsynchCollisionGroupRadius = AbsSoldierData.StandardBoundRadius * 2f;
        protected Vector2 collisionForce = Vector2.Zero;
        const int CollGroupSize = 8;

        protected SafeCollectAsynchStaticList<AbsDetailUnit> collisionGroup = new SafeCollectAsynchStaticList<AbsDetailUnit>(CollGroupSize);
        protected int collisionFrames = 0;
        public DetailUnitModel model;

        //bool isDeleted = false;

        //virtual public void InitRemote(Players.AbsPlayer player, System.IO.BinaryReader r)
        //{
        //    debug_addedByRemote = true;
        //}

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

        virtual public void takeDamage(int damageAmount, Rotation1D attackDir, Faction enemyFaction, bool fullUpdate)
        {
            if (health > 0)
            {
                
                lockedIncomingDamage -= damageAmount;
               
                if (damageAmount>0)
                {
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
        }
        
        public void lockInAttackDamage(int damageAmount)
        {
            if (damageAmount > 0)
            {
                lockedIncomingDamage += damageAmount;
            }
        }

        abstract public void update(float time, bool fullUpdate);

        abstract public void asynchUpdate();
        
        public void setDetailLevel(bool unitDetailView)
        {
            Debug.CrashIfThreaded();
            if (unitDetailView)
            {
                if (model == null)
                {
                    if (isDeleted)
                    { 
                        lib.DoNothing();
                    }
                    model = initModel();
                }
            }
            else
            {
                model?.DeleteMe();
                model = null;
            }
        }

        abstract protected DetailUnitModel initModel();

        //    protected void updateTargetAim()
        //    {
        //        //if (attackTarget == null)
        //        //{
        //        //    var closest = closestAttackTarget;

        //        //    if (closest != null && distanceToUnit(closest) <= Data().targetSpotRange)
        //        //    {
        //        //        attackTarget = closest;
        //        //    }
        //        //}
        //        //else

        //        var attack_sp = attackTarget;

        //        if (attack_sp != null)
        //        {
        //            if (distanceToUnit(attackTarget) > Data().targetSpotRange + DropTargetAdd)
        //            {
        //                //Target got to far away
        //                attackTarget = null;
        //            }
        //            else if (attackTarget.IsStructure())
        //            {
        //                if (attackTarget.Faction() == this.Faction())
        //                {
        //                    attackTarget = null;
        //                }
        //            } 
        //            else
        //            {
        //                if (attackTarget.Dead())
        //                {
        //                    attackTarget = null;
        //                }
        //            }
        //    }
        //}
        protected void refreshAttackTarget()
        {
            var attackTarget_sp = attackTarget;

            if (attackTarget_sp != null && attackTarget_sp.defeatedBy(GetFaction()))
            {
                attackTarget = null;
            }

            var nextAttackTarget_sp= nextAttackTarget;
            if (nextAttackTarget_sp != null && !nextAttackTarget_sp.defeatedBy(GetFaction()))
            {
                attackTarget = nextAttackTarget_sp;
            }
        }

        //abstract public bool defeated(Faction attacker);

        protected void closestTargetCheck(AbsDetailUnit unit,
            ref AbsDetailUnit closestOpponent,
            ref float closestOpponentDistance)
        {
            float distance = spaceBetweenUnits(unit);

            float anglediff = Math.Abs(angleDiff(unit));
            distance += anglediff * 0.1f;

            if (distance < closestOpponentDistance)
            {
                if (canTargetUnit(unit))
                {
                    var data = Data();
                    
                    if (!data.restrictAngle || anglediff <= data.angle)
                    {
                        closestOpponent = unit;
                        closestOpponentDistance = distance;
                    }
                }
            }
            
        }

        protected void collisionGroupCheck(AbsDetailUnit unit, float distance)
        {
            if (distance < AsynchCollisionGroupRadius)
            {
                if (collisionGroup.processList.Count < CollGroupSize)
                {
                    collisionGroup.processList.Add(unit);
                }
            }
        }

        

        virtual protected AbsMapObject ParentMapObject()
        {
            return group.army;
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
            get { return Data().basehealth - health; }
        }

        virtual public void onDeath(bool fullUpdate, Faction enemyFaction)
        {
            //onEvent(UnitEventType.Death);
            if (enemyFaction.player.IsPlayer())
            {
                ++enemyFaction.player.GetLocalPlayer().statistics.EnemySoldiersKilled;
            }
            if (group.army.faction.player.IsPlayer())
            {
                ++group.army.faction.player.GetLocalPlayer().statistics.FriendlySoldiersLost;
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

        //void onMyGroupDestroyed(AttackAnimation fromAttack)
        //{
        //    fromAttack.soldier.onKilledGroup();
        //}

        //public void onKilledGroup()
        //{
        //    //groupKills++;
        //    //UnitStats stats = GetStats();
        //    //if (stats != null && !warsRef.IsCustomMapGame)
        //    //{
        //    //    stats.groupKills++;

        //    //    if (groupKills == warsLib.AchiveOneGroupKills5)
        //    //    {
        //    //        warsLib.SetAchievement(AchievementIndex.OneGroupKills5);
        //    //    }
        //    //}
        //}

        bool isLocalUnitInOnlineMatch()
        {
            return false;//player.IsLocal && warsRef.gamestate.settings.onlineMatch;
        }

        public void onFlagCapture()
        {
            //flagCaptures++;

            //UnitStats stats = GetStats();
            //if (stats != null && !warsRef.IsCustomMapGame)
            //{
            //    stats.flagCaptures++;

            //    if (flagCaptures == warsLib.AchiveOneGroupCaptures5Flags)
            //    {
            //        warsLib.SetAchievement(AchievementIndex.OneGroupCaptures5);
            //    }
            //}
        }


        //protected UnitStats GetStats()
        //{
        //    if (player is Players.LocalPlayer)
        //    {
        //        return warsRef.storage.unitStats[(int)Type];
        //    }

        //    return null;
        //}

        virtual public void onEvent(UnitEventType type)
        {
            //if (type == UnitEventType.Death && isKing)
            //{ warsRef.gamestate.gameover = GameOverReason.KingDeath; }

            if (localMember)
            {
                //var w = Ref.netSession.BeginWritingPacket(Network.PacketType.stupUnitEvent, Network.PacketReliability.Reliable);
                //warsRef.gamestate.writeUnit(w, this);
                ////writeId(w);
                //w.Write((byte)type);
            }
        }

        //virtual public void onDealtProjectileHit(Projectile projectile)
        //{ }

        //protected System.IO.BinaryWriter beginWriteAddUnit(AbsPlayer2 player)
        //{
        //    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.stupAddUnit, Network.PacketReliability.Reliable);
        //    warsRef.gamestate.writePlayer(player, w);
        //    w.Write((byte)Type);
        //    w.Write((ushort)id);

        //    return w;
        //}

        //protected void readId(System.IO.BinaryReader r)
        //{
        //    id = r.ReadUInt16();
        //}

        //public void writeAttack(int index, AbsUnit target)
        //{
        //    if (localMember)
        //    {
        //        var w = Ref.netSession.BeginWritingPacket(Network.PacketType.stupBeginAttack, Network.PacketReliability.Reliable);
        //        warsRef.gamestate.writeUnit(w, this);

        //        w.Write((byte)index);

        //        if (target == null)
        //        {
        //            w.Write(false);
        //        }
        //        else
        //        {
        //            w.Write(true);
        //            warsRef.gamestate.writeUnit(w, target);
        //        }
        //    }
        //}

        //public void readAttack(System.IO.BinaryReader r)
        //{
        //    int index = r.ReadByte();

        //    AbsUnit target = null;
        //    if (r.ReadBoolean())
        //    {
        //        target = warsRef.gamestate.readUnit(r);
        //    }

        //    if (index == 0)
        //    {
        //        if (target != null)
        //        {
        //            attack.startAttack(target, false);
        //        }
        //    }
        //    else
        //    {
        //        createAlternativeAttack(index, target);
        //    }
        //}

        virtual protected void createAlternativeAttack(int index, AbsDetailUnit target) 
        {
            if (localMember)
            {
                //writeAttack(index, target);
            }
        }

        //virtual public void DeleteMe(bool quickDelete)
        //{
        //    isDeleted = true;
        //    deleteModels();
        //}

        public void deleteModels()
        {
            if (model != null)
            {
                //if (fullUpdate)
                //{
                    Debug.CrashIfThreaded();
                    model.DeleteMe();
                //}
                //else
                //{
                //    Ref.update.AddSyncAction(new SyncAction(model.DeleteMe));
                //}
            }
        }

        virtual public void writeNetworkUpdate() { }
        virtual public void readNetworkUpdate(System.IO.BinaryReader r) { }

        virtual public void applyCollisions()
        {
        }

        public float DPS()
        {
            return Data().attackDamage / TimeExt.MillsSecToSec(Data().attackTimePlusCoolDown);
        }

        public bool Alive()
        {
            return health > 0;
        }
        public bool Dead()
        {
            return health <= 0;
        }

        public override bool defeatedBy(Faction attacker)
        {
            return health <= 0;
        }

        override public bool aliveAndBelongTo(Faction faction) 
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
            get { return player().IsLocal; }
        }

        override public Faction GetFaction()
        {
            return group.army.faction;
        }

        public Players.AbsPlayer player()
        {
            return group.army.faction.player;
        }

        virtual public Vector3 projectileStartPos()
        {
            Vector3 pos = position;
            pos.Y += AbsDetailUnitData.StandardModelScale * 0.6f;
            return pos;
        }

        abstract public UnitType DetailUnitType();


        abstract public bool IsShipType();

        abstract public bool IsStructure();

        abstract public bool IsSoldierUnit();

        abstract public bool IsSingleTarget();

        virtual public AbsSoldierUnit GetSoldierUnit() { return null; }

        virtual protected bool IsStunned
        {
            get { return false; }
        }

        //public bool HasShield { get { return Data().shieldDamageReduction > 0; } }

        virtual public int MaxHealth()
        {
            return Data().basehealth;
        }
        
        abstract public AbsDetailUnitData Data();

        public override string TypeName()
        {
            return DssRef.unitsdata.Name(DetailUnitType()) + "(" + parentArrayIndex.ToString() + ")";
        }

        public override string ToString()
        {
            string groupName = group == null? "" : " group(" + group.groupId.ToString() + ")";
            return DetailUnitType().ToString() + "(" + parentArrayIndex.ToString() + ")" + groupName + " p" + " area(" + tilePos.X.ToString() + "," + tilePos.Y.ToString() + ")";
        }
    }

    enum UnitEventType
    {
        MoveOrder,
        StartAttack,
        Death,
    }

    
}
