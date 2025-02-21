using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;
using VikingEngine.PJ.CarBall;
using VikingEngine.PJ.Joust;
using VikingEngine.ToGG.HeroQuest.Players.Ai;

namespace VikingEngine.DSSWars.GameObject
{
    partial class Army
    {
        /*
         * NEW AI
         * -no path finding, just have a current pos and goal
         * -the groups path finds on their own
         * Some type of "new order" trigger is needed, could try using hash code (include army size)
         * -check when move is complete
         * 
         */

        const float MaxRegroupTime = 5000;
        const float MaxRegroupTime_Battle = 30000;

        //Army army;
        //WalkingPath path = null, newpath = null;
        bool needPath_playerview = false;
        WalkingPath path = null;

        public ArmyObjective objective = ArmyObjective.None;
        public bool waitForRegroup = false;
        float stateTime = 0;
        public IntVector2 walkGoal, adjustedWalkGoal;
        float teleportTime;

        public bool walkGoalAsShip;
        public IntVector2 nextNodePos;
        public AbsMapObject attackTarget = null;
        public int attackTargetFaction;
        public int goalId = 0;
        //bool nextPathNode = false;
            
        //public ArmyAi(Army army)
        //{
        //    this.army = army;
        //}

        public void aiUpdate(bool fullUpdate)
        {
            //if (faction.factiontype == FactionType.SouthHara)
            //{
            //    lib.DoNothing();
            //}
            //if (nextPathNode)
            //{
            //    if (newpath != null)
            //    {
            //        path = newpath;
            //        newpath = null;
            //    }
            //    else
            //    {
            //        path?.NextNode();
            //    }

            //    applyNewPathNode();
            //    nextPathNode = false;
            //}
        }

        //void applyNewPathNode()
        //{
        //    if (id == 1)
        //    { 
        //        lib.DoNothing();
        //    }
        //    var path_sp=path;
        //    if (path_sp != null)
        //    {
        //        if (path_sp.HasMoreNodes())
        //        {
        //            IntVector2 last = path_sp.LastNode();
        //            if (faction.HasArmyBlockingPosition(last))
        //            {
        //                path_sp.RemoveLast();
        //            }
        //            else
        //            {
        //                var tile = DssRef.world.tileGrid.Get(last);
        //                if (tile.tileContent == TileContent.City)
        //                {
        //                    if (path_sp.RemoveLast() > 0)
        //                    {
        //                        adjustedWalkGoal = path_sp.LastNode();
        //                    }
        //                }
        //            }
        //        }

        //        PathNodeResult node;

        //        if (path_sp.TryGetCurrentNode(out node))
        //        {

        //            bool nextIsShipTransform = path_sp.nextNodeIsShip();//path_sp.nextTwoNodesAreShip();
        //            bool nextIsFootTransform = path_sp.nextNodeIsFeet();//path_sp.nextTwoNodesAreByFeet();

        //            var prevRotation = rotation;

        //            bool finalNode = path_sp.NodeCountLeft() <= 2;
        //            setWalkNode(node.position, finalNode, nextIsFootTransform, nextIsShipTransform);

        //            if (nextIsShipTransform ||
        //                nextIsFootTransform ||
        //                Rotation1D.AngleDifference_Absolute(prevRotation.radians, rotation.Radians) >= MathExt.TauOver8)
        //            {
        //                waitForRegroup = true;
        //                //if (id == 1)
        //                //{
        //                //    lib.DoNothing();
        //                //}
        //                stateTime = 0;
        //            }

        //            if (isOUtSideBattle(path_sp))
        //            { 
        //                waitForRegroup = true;
        //                //if (army.id == 1)
        //                //{
        //                //    lib.DoNothing();
        //                //}
        //                stateTime = 0;
        //            }
        //        }
        //        else
        //        {
        //            clearObjective();
        //        }
        //    }
        //}

        bool isOUtSideBattle(WalkingPath path)
        {
            return objective == ArmyObjective.Attack && path.NodeCountLeft() == 3;
        }

        public void asyncPathUpdate(int pathThreadIndex)
        {
            if (IdleObjetive())
            {
                recyclePath(pathThreadIndex);

            }
            else if (objective != ArmyObjective.None)
            {
                if (needPath_playerview)
                {
                    if (path == null)
                    {
                        path_calulate(pathThreadIndex);
                    }
                    needPath_playerview = false;
                    var p = faction.player.GetLocalPlayer();
                    if (p != null)
                    {
                        p.hud.needRefresh = true;
                    }
                }

                var path_sp = path;
                if (path_sp != null)
                {
                    path_sp.refreshCurrentNode(tilePos, out bool offTrack);
                    if (offTrack)
                    {
                        path_calulate(pathThreadIndex);
                    }
                }
            }

            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.asyncPathUpdate(pathThreadIndex);
            }
        }

        public void asynchAiUpdate(float time)
        {
            //if (debugTagged)
            //{
            //    lib.DoNothing();
            //}
            
                //var attackTarget_sp = attackTarget;

                //if (objective == ArmyObjective.Attack && attackTarget_sp != null)
                //{
                //    if (attackTarget_sp.defeatedBy(faction))
                //    {
                //        if ((walkGoal - tilePos).SideLength() <= 2)
                //        {
                //            clearObjective();
                //        }
                //        else
                //        {
                //            objective = ArmyObjective.MoveTo;
                //        }
                //        return;
                //    }

                //    if (attackTarget_sp.faction.parentArrayIndex != attackTargetFaction)
                //    {
                //        //Target changed owner
                //        objective = ArmyObjective.MoveTo;
                //    }
                //}


                //var path_sp = path;

                //if (path_sp == null ||
                //    //path.PassedNodeCount() >= 3 ||
                //    (objective == ArmyObjective.Attack && 
                //    (attackTarget != null && walkGoal != attackTarget.tilePos) && 
                //    path_sp.PassedNodeCount() >= 1))
                //{
                //    if (objective == ArmyObjective.Attack)
                //    {
                //        walkGoal = attackTarget.tilePos;
                //        adjustedWalkGoal = walkGoal;
                //    }
                //    if (calcPath())
                //    {
                //        nextPathNode = true;
                //    }
                //}
                //else
                //{
                //    stateTime += time;

                //    if (waitForRegroup)
                //    {
                //        float maxtime = (isOUtSideBattle(path_sp) && !InBattle()) ? MaxRegroupTime_Battle : MaxRegroupTime;
                //        if (stateTime < maxtime)
                //        {
                //            var groupC = groups.counter();
                //            while (groupC.Next())
                //            {
                //                if (!groupC.sel.groupIsIdle)
                //                {
                //                    return;
                //                }
                //            }
                //        }
                //        waitForRegroup = false;
                //    }

                //    PathNodeResult node;

                //    if (path_sp.TryGetCurrentNode(out node))
                //    {
                //        //var node = path_sp.CurrentNode();
                //        float l = VectorExt.Length(node.position.X - position.X, node.position.Y - position.Z);

                //        if (l <= 0.2f)
                //        {
                //            nextPathNode = true;
                //        }
                //        else if (l > 2f)
                //        {
                //            //Army center has jumped, need new path
                //            path = null;
                //        }
                //        else if (stateTime >= 5000)
                //        {
                //            refreshNextWalkingNode();
                //        }
                //    }
                //}                
           // }
        }

        //void refreshNextWalkingNode()
        //{
        //    return;
        //    if (battleGroup != null)
        //    {
        //        return;
        //    }
        //    //if (army.id == 1)
        //    //{
        //    //    lib.DoNothing();
        //    //}

        //    IntVector2 walkPos = tilePos;
        //    var path_sp = path;
        //    if (path_sp != null)
        //    {
        //        PathNodeResult node;

        //        if (path_sp.TryGetCurrentNode(out node))
        //        {
        //            walkPos = node.position;
        //        }
        //    }
        //    nextNodePos = walkPos;
        //    refreshGroupPlacements2(walkPos, !path_sp.HasMoreNodes());
        //    //var groupC = groups.counter();
        //    //while (groupC.Next())
        //    //{
        //    //    groupC.sel.bumpWalkToNode(walkPos);
        //    //}

        //    stateTime = 0;
        //}

        //bool calcPath()
        //{
        //    if ((objective == ArmyObjective.Attack  || objective == ArmyObjective.MoveTo)
        //        &&
        //        tilePos != adjustedWalkGoal)
        //    {
        //        PathFinding pf = DssRef.state.pathFindingPool.Get();
        //        {
        //            newpath = pf.FindPath(tilePos, rotation, walkGoal,
        //                isShip);
        //        } 
        //        DssRef.state.pathFindingPool.Return(pf);

        //        return true;//todo success check
        //    }

        //    return false;
        //}

        public void onArmyMerge()
        {
            waitForRegroup = true;
            stateTime = 0;
            //refreshNextWalkingNode();
        }

        public void Ai_Order_MoveTo(IntVector2 goalTilePos)
        {
            if (orderOutsidePlayerAttension(goalTilePos))
            {
                if (Order_MoveTo_Setup(goalTilePos))
                { 
                    objective = ArmyObjective.TeleportMove;
                    calcTeleportTime(goalTilePos);
                }
            }
            else
            { 
                Order_MoveTo(goalTilePos);
            }
        }

        public void Ai_Order_Attack(AbsMapObject attackTarget)
        {
            if (orderOutsidePlayerAttension(attackTarget))
            {
                Order_Attack_Setup(attackTarget);
                objective = ArmyObjective.TeleportAttack;
                calcTeleportTime(attackTarget.tilePos);
            }
            else
            {
                Order_Attack(attackTarget);
            }
        }

        void calcTeleportTime(IntVector2 to)
        {
            float dist = to.SideLength(tilePos);
            float speedPerSec = (isShip ? transportSpeedSea : transportSpeedLand) * TimeExt.SecondToMs;
            float time = dist / speedPerSec;
            teleportTime = Ref.TotalGameTimeSec + time;
        }

        void Ai_Finalize_Attack()
        {
            if (attackTarget != null)
            {
                //todo räkna in support
                if (this.strengthValue > attackTarget.strengthValue)
                {
                    if (attackTarget.gameobjectType() == GameObjectType.City)
                    {
                        attackTarget.GetCity().setFaction(faction);
                    }
                    else
                    {
                        Ref.update.AddSyncAction(new SyncAction1Arg<AbsMapObject>((AbsMapObject attackTarget) =>
                        {
                            attackTarget?.DeleteMe(DeleteReason.Death, true);
                        }, attackTarget));
                    }
                }
                else
                {
                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        DeleteMe(DeleteReason.Death, true);
                    }));
                }

                Ai_Finalize_Move();
            }
        }

        void Ai_Finalize_Move()
        {
            onNewGoal(true);
        }

        bool orderOutsidePlayerAttension(AbsMapObject target)
        {            
            return target.faction.player.IsAi() &&
                orderOutsidePlayerAttension(target.tilePos);
        }

        bool orderOutsidePlayerAttension(IntVector2 to)
        {

            return Ref.rnd.Chance(0.8) &&
                !inRender_overviewLayer &&
                DssRef.state.culling.outsidePlayerAttension(tilePos) &&
                DssRef.state.culling.outsidePlayerAttension(to);
        }

        public void Order_MoveTo(IntVector2 goalTilePos)
        {
            if (Order_MoveTo_Setup(goalTilePos))
            {        
                onNewGoal(false);
            }
        }
        public bool Order_MoveTo_Setup(IntVector2 goalTilePos)
        {
            clearObjective();

            if (goalTilePos != tilePos)
            {
                walkGoal = goalTilePos;
                adjustedWalkGoal = walkGoal;
                objective = ArmyObjective.MoveTo;
                return true;
                //onNewGoal(false);
            }

            return false;
        }

        public void Order_Attack(AbsMapObject attackTarget)
        {
            if (attackTarget != null)
            {
                Order_Attack_Setup(attackTarget);
                objective = ArmyObjective.Attack;
                onNewGoal(false);
            }
        }

        public void Order_Attack_Setup(AbsMapObject attackTarget)
        {
            DssRef.diplomacy.declareWar(faction, attackTarget.faction);
            clearObjective();
            this.attackTarget = attackTarget;
            this.attackTargetFaction = attackTarget.faction.parentArrayIndex;
            //objective = ArmyObjective.Attack;
        }

        public void haltMovement()
        {
            clearObjective();
            objective = ArmyObjective.Halt;

            setWalkNode(tilePos, true, false, false);
            onNewGoal(false);
        }

        void onNewGoal(bool teleport)
        {
            IntVector2 goal;
            if (IdleObjetive())
            {
                goal = tilePos;
                needPath_playerview = false;
            }
            else if (objective == ArmyObjective.Attack || objective == ArmyObjective.TeleportAttack)
            {
                goal = attackTarget.tilePos;
            }
            else
            {
                goal = walkGoal;
            }


            var tile = DssRef.world.tileGrid.Get(goal);
            if (tile.tileContent == TileContent.City)
            {
                IntVector2 dir = walkGoal - tilePos;
                if (dir.HasValue())
                {   
                    IntVector2 adjusted = goal - dir.Normal();
                    if (DssRef.world.tileGrid.Get(adjusted).IsWater())
                    {
                        int closestDist = 10;
                        
                        for (Dir8 d = 0; d < Dir8.NUM; d++)
                        {
                            IntVector2 pos = IntVector2.FromDir8(d) + goal;
                            if (DssRef.world.tileGrid.Get(pos).IsLand())
                            {
                                var l = goal.SideLength(pos);
                                if (l < closestDist)
                                { 
                                    adjusted = pos;
                                }
                            }
                        }
                    }

                    goal = adjusted;
                }
            }

            walkGoal = goal;

            walkGoalAsShip = DssRef.world.tileGrid.Get(goal).IsWater();
            refreshGroupPlacements2(goal, true, teleport);

            //path = null;
            goalId++;
        }

        public void clearObjective()
        {
            objective = ArmyObjective.None;
            attackTarget = null;
        }

        public void stopAllAttacksAgainst(Faction otherFaction)
        {
            var attackTarget_sp = attackTarget;
            if (attackTarget_sp != null && 
                attackTarget_sp.faction == otherFaction)
            {
                haltMovement();
            }
        }

        public void removeAttackTarget()
        {
            attackTarget = null;
            if (objective == ArmyObjective.Attack)
            {
                objective = ArmyObjective.None;
            }
        }

        public void hoverAndSelectInfo(LocalPlayer player, Graphics.ImageGroup images)
        {
            if (objective != ArmyObjective.None && objective != ArmyObjective.Halt)
            {
                var path_sp = path;

                if (path_sp == null)
                {
                    needPath_playerview = true;
                }
                else
                {
                    PathVisuals pv = new PathVisuals(player.playerData.localPlayerIndex);
                    pv.refresh(path_sp, attackTarget != null, true);
                    pv.addTo(images);
                }
            }
        }

        void recyclePath(int pathThreadIndex)
        {
            //if (path != null)
            //{
            //lock (DssRef.state.pathFindingPool)
            //{
            DssRef.state.pathUpdates[pathThreadIndex].pathFindingPool.Return(path);
            path = null;
            //}
            //}
        }

        void path_calulate(int pathThreadIndex)
        {
            recyclePath(pathThreadIndex);

            PathFinding pf = DssRef.state.pathUpdates[pathThreadIndex].pathFindingPool.GetPf();
            {
                path = pf.FindPath(pathThreadIndex, tilePos, rotation, walkGoal, isShip);
            }
            DssRef.state.pathUpdates[pathThreadIndex].pathFindingPool.Return(pf);
        }

        public override void stateDebugText(RichBoxContent content)
        {
            content.text("Objective: " + objective.ToString());
            if (objective != ArmyObjective.None)
            {
                if (objective == ArmyObjective.Attack)
                {
                    content.text("Attack: " + attackTarget.TypeName());
                }
                content.text("walkGoal: " + walkGoal.ToString());
                content.text("adjusted walkGoal: " + adjustedWalkGoal.ToString());
            }
        }

        public void writeAiState(System.IO.BinaryWriter w)
        {
            w.Write((byte)objective);

            switch (objective)
            { 
                case ArmyObjective.Attack:
                    new ArmyAttackObjectPointer(w, attackTarget);
                    break;
                case ArmyObjective.TeleportAttack:
                    new ArmyAttackObjectPointer(w, attackTarget);
                    w.Write(teleportTime);
                    break;
                case ArmyObjective.MoveTo:
                    WP.writeTilePos(w, walkGoal);
                    break;
                case ArmyObjective.TeleportMove:
                    WP.writeTilePos(w, walkGoal);
                    w.Write(teleportTime);
                    break;
            }

        }
        public void readAiState(System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            objective = (ArmyObjective)r.ReadByte();

            switch (objective)
            {
                case ArmyObjective.Attack:
                    pointers.pointers.Add(new ArmyAttackObjectPointer(r, this, false));
                    break;
                case ArmyObjective.TeleportAttack:
                    pointers.pointers.Add(new ArmyAttackObjectPointer(r, this, true));
                    teleportTime = r.ReadSingle();
                    break;
                case ArmyObjective.MoveTo:
                    walkGoal = WP.readTilePos(r);
                    Order_MoveTo(walkGoal);
                    
                    break;
                case ArmyObjective.TeleportMove:
                    walkGoal = WP.readTilePos(r);
                    Order_MoveTo_Setup(walkGoal);
                    teleportTime = r.ReadSingle();
                    break;
            }

        }

        public bool IdleObjetive()
        {
            if (objective == ArmyObjective.Attack) 
            {
                var attackTarget_sp = attackTarget;
                if (attackTarget_sp != null && !attackTarget_sp.aliveAndBelongTo(attackTargetFaction))
                {
                    if (debugTagged)
                    {
                        lib.DoNothing();
                            
                    }

                    attackTarget = null;
                    objective = ArmyObjective.None;
                    return true;
                }
            }
            return objective == ArmyObjective.None || objective == ArmyObjective.Halt;
        }

        //public void Ai_EnterPeaceEvent()
        //{
        //    waitForRegroup = true;
        //    stateTime = 0;

        //    if (IdleObjetive())
        //    {
        //        Order_MoveTo(positionBeforeBattle);
        //    }
        //}
    }

    enum ArmyObjective
    {
        None,
        Halt,
        MoveTo,
        Attack,
        TeleportMove,
        TeleportAttack,

    }
}
