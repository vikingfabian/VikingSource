using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Physics;
using VikingEngine.ToGG.MoonFall;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.Battle
{
    class SoldierBattleData
    {
        static List<AbsSoldierUnit> SoldierBuffer = new List<AbsSoldierUnit>(16);
        static List<AbsGroup> GroupBuffer = new List<AbsGroup>(16);
                
        List<AbsSoldierUnit> nearBodyCollisionUnits = new List<AbsSoldierUnit>(8);
        public float queueTime = 0;
        static CircleBound QueBound = new CircleBound();
        public void update(AbsSoldierUnit parent)
        {
            //1. Is the soldier queuing behind friendlies
            //2. Is he bumping into other people/items
            Vector2 collisionForce = Vector2.Zero;

            lock (nearBodyCollisionUnits)
            {
                foreach (var unit in nearBodyCollisionUnits)
                {
                    Physics.Collision2D intersection = parent.bound.Intersect2(unit.bound);
                    //Make sure friendly units dont push eachother forward
                    if (intersection.IsCollision)
                    {
                        if (parent.GetFaction() == unit.GetFaction())
                        {
                            if (Rotation1D.AngleDifference_Absolute(parent.rotation.radians, lib.V2ToAngle(-intersection.direction)) < MathExt.TauOver8)
                            {
                                //Is pushing friend, halt and queue
                                queueTime = 400;
                            }
                        }
                        collisionForce += intersection.direction;                        
                    }
                }
            }

            if (VectorExt.HasValue(collisionForce))
            {
                float collPush = 0.18f;
                if (queueTime > 0)
                {
                    collPush = 0.25f;
                }
                parent.position += VectorExt.V2toV3XZ(collPush * collisionForce);

                //collisionForce = Vector2.Zero;
            }
        }

        public void onTakeMeleeDamage(AbsSoldierUnit parent, AbsDetailUnit meleeAttacker)
        {
            if (parent.group.debugTagged && parent.parentArrayIndex == 3)
            {
                lib.DoNothing();
            }

            if (queueTime > 0)
            {
                queueTime = 0;
                //parent.state2 = SoldierState2.
                parent.attackTarget = meleeAttacker;
                parent.nextAttackTarget = meleeAttacker;
            }
        }

        public bool InQueue(AbsSoldierUnit parent)
        {
            const float Regular_QueTime = 400;
            const float Turn_QueTime = 1200;

            //turn = 0;
            var target_sp = parent.attackTarget;
            if (target_sp != null && parent.distanceToUnit(target_sp) < DssConst.MeleeAwareRange)
            {
                return false;
            }

            if (collision(parent.rotation, Regular_QueTime))
            {
                switch (Ref.rnd.Int(4))
                { 
                    case 0:
                        {
                            Rotation1D dir = parent.rotation;
                            dir.Add(-MathExt.TauOver6);
                            if (!collision(dir, Turn_QueTime))
                            {
                                parent.state2 = SoldierState2.Turn;
                                parent.goalRotation = dir.radians;
                                //parent.stateTime = 400;
                                //return false;
                            }
                        }
                        break;
                    case 1:
                        {
                            Rotation1D dir = parent.rotation;
                            dir.Add(MathExt.TauOver6);
                            if (!collision(dir, Turn_QueTime))
                            {
                                parent.state2 = SoldierState2.Turn;
                                parent.goalRotation = dir.radians;
                                //return false;
                            }
                        }
                        break;
                    case 2:
                        {
                            Rotation1D dir = parent.rotation;
                            dir.Add(-MathExt.TauOver4);
                            if (!collision(dir, Turn_QueTime))
                            {
                                parent.state2 = SoldierState2.Turn;
                                parent.goalRotation = dir.radians;
                                //parent.stateTime = 400;
                                //return false;
                            }
                        }
                        break;
                    case 3:
                        {
                            Rotation1D dir = parent.rotation;
                            dir.Add(MathExt.TauOver4);
                            if (!collision(dir, Turn_QueTime))
                            {
                                parent.state2 = SoldierState2.Turn;
                                parent.goalRotation = dir.radians;
                                //return false;
                            }
                        }
                        break;
                }
                return true;
            }
            return false;

            bool collision(Rotation1D searchDir, float qTime)
            {
                QueBound.radius = parent.bound.ExtremeRadius;
                QueBound.center = parent.bound.Center + searchDir.Direction(QueBound.radius * 0.25f);

                lock (nearBodyCollisionUnits)
                {
                    foreach (var unit in nearBodyCollisionUnits)
                    {
                        if (parent.GetFaction() == unit.GetFaction())
                        {
                            Physics.Collision2D intersection = QueBound.Intersect2(unit.bound);
                            if (intersection.IsCollision)
                            {
                                queueTime = qTime;
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        public void asycUpdate(AbsSoldierUnit parent) 
        {
            if (parent.group.debugTagged)
            {
                lib.DoNothing();
            }
            AbsDetailUnit closestOpponent = null;
            float closestOpponentDistance = float.MaxValue;

            const float SoldierToGroupMaxDistance = 1.0f;

            SoldierBuffer.Clear();
            //Collect nearby collision bounds
            DssRef.world.unitCollAreaGrid.collectGroups(parent.tilePos, ref GroupBuffer, true);

            foreach (var group in GroupBuffer)
            {
                bool opponent = DssRef.diplomacy.InWar(parent.GetFaction(), group.GetFaction());

                if (VectorExt.Length(group.position.X - parent.position.X, group.position.Z - parent.position.Z) < 3)
                {
                    switch( group.gameobjectType())
                    {
                        case GameObjectType.SoldierGroup:
                            var soldiers = group.GetGroup().soldiers;
                            if (soldiers != null)
                            {
                                var soldiersC = soldiers.counter();
                                while (soldiersC.Next())
                                {
                                    if (soldiersC.sel.Alive_IncomingDamageIncluded())
                                    {
                                        if (parent.bound.AsynchCollect(soldiersC.sel.bound) &&
                                            soldiersC.sel != parent)
                                        {
                                            SoldierBuffer.Add(soldiersC.sel);
                                        }

                                        if (opponent)
                                        {
                                            parent.closestTargetCheck(soldiersC.sel,
                                                ref closestOpponent, ref closestOpponentDistance);
                                        }
                                    }
                                }
                            }
                            break;
                        case GameObjectType.City:
                            if (opponent)
                            {
                                parent.closestTargetCheck(group.GetCity().detailObj,
                                ref closestOpponent, ref closestOpponentDistance);
                            }
                            break;
                    }
                }
            }

            lock (nearBodyCollisionUnits)
            {
                nearBodyCollisionUnits.Clear();
                nearBodyCollisionUnits.AddRange(SoldierBuffer);
            }

            parent.nextAttackTarget = closestOpponent;
        }

    }
}
