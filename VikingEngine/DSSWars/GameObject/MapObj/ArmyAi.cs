﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Valve.Steamworks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ.Joust;
using VikingEngine.ToGG.HeroQuest.Players.Ai;

namespace VikingEngine.DSSWars.GameObject
{
    partial class Army
    {
        const float MaxRegroupTime = 5000;
        const float MaxRegroupTime_Battle = 30000;
        
        //Army army;
        WalkingPath path = null, newpath = null;

        public ArmyObjective objective = ArmyObjective.None;
        public bool waitForRegroup = false;
        float stateTime = 0;
        public IntVector2 walkGoal, adjustedWalkGoal;
        public AbsMapObject attackTarget = null;
        public int attackTargetFaction;

        bool nextPathNode = false;

        //public ArmyAi(Army army)
        //{
        //    this.army = army;
        //}

        public void aiUpdate(bool fullUpdate)
        {
            if (faction.factiontype == FactionType.SouthHara)
            {
                lib.DoNothing();
            }
            if (nextPathNode)
            {
                if (newpath != null)
                {
                    path = newpath;
                    newpath = null;
                }
                else
                {
                    path?.NextNode();
                }
                
                applyNewPathNode();
                nextPathNode = false;
            }
        }

        void applyNewPathNode()
        {
            if (id == 1)
            { 
                lib.DoNothing();
            }
            var path_sp=path;
            if (path_sp != null)
            {
                if (path_sp.HasMoreNodes())
                {
                    IntVector2 last = path_sp.LastNode();
                    if (faction.HasArmyBlockingPosition(last))
                    {
                        path_sp.RemoveLast();
                    }
                    else
                    {
                        var tile = DssRef.world.tileGrid.Get(last);
                        if (tile.tileContent == TileContent.City)
                        {
                            if (path_sp.RemoveLast() > 0)
                            {
                                adjustedWalkGoal = path_sp.LastNode();
                            }
                        }
                    }
                }

                PathNodeResult node;

                if (path_sp.TryGetCurrentNode(out node))
                {

                    bool nextIsShipTransform = path_sp.nextNodeIsShip();//path_sp.nextTwoNodesAreShip();
                    bool nextIsFootTransform = path_sp.nextNodeIsFeet();//path_sp.nextTwoNodesAreByFeet();

                    var prevRotation = rotation;

                    setWalkNode(node.position, nextIsFootTransform, nextIsShipTransform);
                    if (nextIsShipTransform ||
                        nextIsFootTransform ||
                        Rotation1D.AngleDifference_Absolute(prevRotation.radians, rotation.Radians) >= MathExt.TauOver8)
                    {
                        waitForRegroup = true;
                        //if (id == 1)
                        //{
                        //    lib.DoNothing();
                        //}
                        stateTime = 0;
                    }

                    if (isOUtSideBattle(path_sp))
                    { 
                        waitForRegroup = true;
                        //if (army.id == 1)
                        //{
                        //    lib.DoNothing();
                        //}
                        stateTime = 0;
                    }
                }
                else
                {
                    clearObjective();
                }
            }
        }

        bool isOUtSideBattle(WalkingPath path)
        {
            return objective == ArmyObjective.Attack && path.NodeCountLeft() == 3;
        }

        public void asynchAiUpdate(float time)
        {
            if (debugTagged)
            {
                lib.DoNothing();
            }
            if (objective != ArmyObjective.None)
            {
                var attackTarget_sp = attackTarget;

                if (objective == ArmyObjective.Attack && attackTarget_sp != null)
                {
                    if (attackTarget_sp.defeatedBy(faction))
                    {
                        if ((walkGoal - tilePos).SideLength() <= 2)
                        {
                            clearObjective();
                        }
                        else
                        {
                            objective = ArmyObjective.MoveTo;
                        }
                        return;
                    }

                    if (attackTarget_sp.faction.parentArrayIndex != attackTargetFaction)
                    {
                        //Target changed owner
                        objective = ArmyObjective.MoveTo;
                    }
                }


                var path_sp = path;

                if (path_sp == null ||
                    //path.PassedNodeCount() >= 3 ||
                    (objective == ArmyObjective.Attack && 
                    (attackTarget != null && walkGoal != attackTarget.tilePos) && 
                    path_sp.PassedNodeCount() >= 1))
                {
                    if (objective == ArmyObjective.Attack)
                    {
                        walkGoal = attackTarget.tilePos;
                        adjustedWalkGoal = walkGoal;
                    }
                    if (calcPath())
                    {
                        nextPathNode = true;
                    }
                }
                else
                {
                    stateTime += time;

                    if (waitForRegroup)
                    {
                        float maxtime = (isOUtSideBattle(path_sp) && !InBattle()) ? MaxRegroupTime_Battle : MaxRegroupTime;
                        if (stateTime < maxtime)
                        {
                            var groupC = groups.counter();
                            while (groupC.Next())
                            {
                                if (!groupC.sel.groupIsIdle)
                                {
                                    return;
                                }
                            }
                        }
                        waitForRegroup = false;
                    }

                    PathNodeResult node;

                    if (path_sp.TryGetCurrentNode(out node))
                    {
                        //var node = path_sp.CurrentNode();
                        float l = VectorExt.Length(node.position.X - position.X, node.position.Y - position.Z);

                        if (l <= 0.2f)
                        {
                            nextPathNode = true;
                        }
                        else if (l > 2f)
                        {
                            //Army center has jumped, need new path
                            path = null;
                        }
                        else if (stateTime >= 5000)
                        {
                            refreshNextWalkingNode();
                        }
                    }
                }                
            }
        }

        void refreshNextWalkingNode()
        {

            if (battleGroup != null)
            {
                return;
            }
            //if (army.id == 1)
            //{
            //    lib.DoNothing();
            //}

            IntVector2 walkPos = tilePos;
            var path_sp = path;
            if (path_sp != null)
            {
                PathNodeResult node;

                if (path_sp.TryGetCurrentNode(out node))
                {
                    walkPos = node.position;
                }
            }

            var groupC = groups.counter();
            while (groupC.Next())
            {
                groupC.sel.bumpWalkToNode(walkPos);
            }
            
            stateTime = 0;
        }

        bool calcPath()
        {
            if ((objective == ArmyObjective.Attack  || objective == ArmyObjective.MoveTo)
                &&
                tilePos != adjustedWalkGoal)
            {
                PathFinding pf = DssRef.state.pathFindingPool.Get();
                {
                    newpath = pf.FindPath(tilePos, rotation, walkGoal,
                        isShip);
                } 
                DssRef.state.pathFindingPool.Return(pf);

                return true;//todo success check
            }

            return false;
        }

        public void onArmyMerge()
        {
            waitForRegroup = true;
            stateTime = 0;
            refreshNextWalkingNode();
        }


        public void Order_MoveTo(IntVector2 goalTilePos)
        {
            clearObjective();

            if (goalTilePos != tilePos)
            {
                walkGoal = goalTilePos;
                adjustedWalkGoal = walkGoal;
                objective = ArmyObjective.MoveTo;
            }
        }

        public void Order_Attack(AbsMapObject attackTarget)
        {
            DssRef.diplomacy.declareWar(faction, attackTarget.faction);
            clearObjective();
            this.attackTarget = attackTarget;
            this.attackTargetFaction = attackTarget.faction.parentArrayIndex;
            objective = ArmyObjective.Attack;
        }

        public void haltMovement()
        {
            clearObjective();
            objective = ArmyObjective.Halt;

            tilePos = WP.ToTilePos(position);
            setWalkNode(tilePos, false, false);

        }

        public void clearObjective()
        {
            objective = ArmyObjective.None;
            attackTarget = null;
            path = null;
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
            //army.haltMovement();
        }

        public void hoverAndSelectInfo(Graphics.ImageGroup images, int playerIndex)
        {
            if (objective != ArmyObjective.None && objective != ArmyObjective.Halt)
            {
                PathVisuals pv = new PathVisuals(playerIndex);
                pv.refresh(path, attackTarget != null, true);
                pv.addTo(images);
            }
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
            if (objective == ArmyObjective.Attack)
            {
                new ArmyAttackObjectPointer(w, attackTarget);
            }
            else if (objective == ArmyObjective.MoveTo)
            {
                WP.writeTilePos(w, walkGoal);
            }
        }
        public void readAiState(System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            objective = (ArmyObjective)r.ReadByte();
            if (objective == ArmyObjective.Attack)
            {
                pointers.pointers.Add(new ArmyAttackObjectPointer(r, this));
            }
            else if (objective == ArmyObjective.MoveTo)
            {
                walkGoal = WP.readTilePos(r);
                adjustedWalkGoal = walkGoal;
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

        public void Ai_EnterPeaceEvent()
        {
            waitForRegroup = true;
            stateTime = 0;

            if (IdleObjetive())
            {
                Order_MoveTo(positionBeforeBattle);
            }
        }
    }

    enum ArmyObjective
    {
        None,
        Halt,
        MoveTo,
        Attack,
    }
}
