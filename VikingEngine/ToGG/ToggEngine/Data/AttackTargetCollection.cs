using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class AttackTargetCollection
    {
        public ListWithSelection<AttackTarget> targets = new ListWithSelection<AttackTarget>();
        
        public AttackType attackType;
        public AbsUnit friendlyUnit;
        bool targetTerrain;
        bool useMeleeLock;

        public AttackTargetCollection(AbsUnit friendlyUnit)
        {
            this.friendlyUnit = friendlyUnit;
        }

        public AttackTargetCollection(AbsUnit friendlyUnit, IntVector2 fromPos, bool useMeleeLock = false, bool targetTerrain = false)
        {
            this.useMeleeLock = useMeleeLock;
            this.friendlyUnit = friendlyUnit;
            this.targetTerrain = targetTerrain;
            refresh(fromPos);
        }

        public void refresh(IntVector2 fromPos)
        {
            targets.list.Clear();

            if (targetTerrain)
            {
                collectTerrainObjects(fromPos);
            }
            else
            {
                collectUnits(fromPos);
            }
        }

        void collectTerrainObjects(IntVector2 fromPos)
        {
            int attackRadius = 1;
            Rectangle2 area = Rectangle2.FromCenterTileAndRadius(fromPos, attackRadius);
            area.SetBounds(toggRef.board.tileGrid.Area);

            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                var sq = toggRef.board.tileGrid.Get(loop.Position);
                var obj = sq.tileObjects.GetAttackTargetObject();
                if (obj != null)
                {
                    AttackTarget target = new AttackTarget(obj as HeroQuest.AbsTrap);
                    //target.attackType = Battle.AttackType.Melee;
                    //target.terrain = obj as HeroQuest.AbsTrap;

                    targets.Add(target, false);
                }
            }
        }

        void collectUnits(IntVector2 fromPos)
        {
            if (friendlyUnit.CatapultAttackType)
            {
                attackType.mainType = AttackMainType.Ranged;

                Rectangle2 area = new Rectangle2(fromPos, friendlyUnit.Data.wep.projectileRange2);
                area.SetBounds(new Rectangle2(IntVector2.Zero, toggRef.board.Size));
                ForXYLoop loop = new ForXYLoop(area);

                while (loop.Next())
                {
                    if ((loop.Position - fromPos).SideLength() >= friendlyUnit.Data.wep.projectileRange)
                    {
                        targets.Add(new AttackTarget(loop.Position), false);
                    }
                }
            }
            else
            {
                SpottedArrayCounter<AbsUnit> enemyUnits;

                if (toggRef.mode == GameMode.Commander)
                {
                    enemyUnits = Commander.cmdRef.players.CollectEnemyUnits(friendlyUnit.globalPlayerIndex);
                }
                else
                {
                    enemyUnits = HeroQuest.hqRef.players.CollectEnemyUnits(friendlyUnit.Player);
                }
                AbsUnitData udata = friendlyUnit.Data;
                List<AttackTarget> rangedTargets = new List<AttackTarget>();
                List<AttackTarget> meleeTargets = new List<AttackTarget>();
                //bool lockedInMelee = false;
                //IntVector2 LOSBlock;

                if (Input.Keyboard.Ctrl)
                {
                    lib.DoNothing();
                }

                enemyUnits.Reset();
                while (enemyUnits.Next())
                {
                    if (enemyUnits.sel.UnitId == 23)
                    {
                        lib.DoNothing();
                    }

                    AttackMainType targetType = friendlyUnit.IsAvailableTarget(fromPos, enemyUnits.sel);

                    switch (friendlyUnit.IsAvailableTarget(fromPos, enemyUnits.sel))
                    {
                        
                        case AttackMainType.Melee:
                            meleeTargets.Add(new AttackTarget(enemyUnits.sel, true));
                            break;
                        case AttackMainType.Ranged:
                            rangedTargets.Add(new AttackTarget(enemyUnits.sel, false));
                            break;
                    }
                }

                if (rangedTargets.Count > 0 && meleeTargets.Count > 0)
                {
                    if (useMeleeLock)
                    {
                        attackType.mainType = AttackMainType.Melee;
                        targets.list = meleeTargets;
                    }
                    else
                    {
                        attackType.mainType = AttackMainType.Mixed;
                        targets.list = arraylib.MergeArrays(rangedTargets, meleeTargets);
                    }
                }
                else if (rangedTargets.Count > 0)
                {
                    attackType.mainType = AttackMainType.Ranged;
                    targets.list = rangedTargets;
                }
                else
                {
                    attackType.mainType = AttackMainType.Melee;
                    targets.list = meleeTargets;
                }                

                targets.selectedIndex = 0;
            }
        }

        public static bool CollectGroupTarget(AbsUnit unit, IntVector2 groupStart, ref AttackTarget target, bool needRangeAndSight)
        {
            target.groupAttackStart = groupStart;
            int dist = (target.position - unit.squarePos).SideLength();
            
            if (dist == 1)
            { //MELEE range
                target.attackType = AttackType.Melee;
            }
            else
            {
                target.attackType = AttackType.Ranged;

                IntVector2 blockingTile;
                if (needRangeAndSight &&
                    (dist > unit.FireRangeWithModifiers(unit.squarePos) ||
                    toggRef.board.InLineOfSight(unit.squarePos, target.position, out blockingTile, unit, false, false) == false))
                {
                    return false;
                }
            }

            ToggEngine.Map.BoardSquareContent square;
            if (toggRef.board.tileGrid.TryGet(target.position, out square))
            {
                if (square.unit != null)
                {
                    if (toggRef.absPlayers.IsOpponent(unit, square.unit))
                    {
                        target.unit = square.unit;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (square.HasProperty(ToggEngine.Map.TerrainPropertyType.Impassable))
                {
                    return false;
                }

                return true;
            }
            else
            {
                //outside map
                return false;
            }
        }

        public List<IntVector2> targetPositions()
        {
            var result = new List<IntVector2>(targets.list.Count);
            foreach (var t in targets.list)
            {
                result.Add(t.position);
            }

            return result;
        }

        public void targetPositions(List<IntVector2> list)
        {
            //var result = new List<IntVector2>(targets.list.Count);
            foreach (var t in targets.list)
            {
                list.Add(t.position);
            }
        }

        public AttackTarget Contains(AbsUnit targetUnit)
        {
            if (targetUnit == null) return null;

            foreach (AttackTarget t in targets.list)
            {
                if (t.unit == targetUnit)
                    return t;
            }
            return null;
        }

        public AttackTarget Contains(IntVector2 pos)
        {
            foreach (AttackTarget t in targets.list)
            {
                if (t.position == pos)
                    return t;
            }
            return null;
        }

        public AttackTarget GetTarget(AbsUnit unit)
        {
            foreach (AttackTarget t in targets.list)
            {
                if (t.unit == unit)
                    return t;
            }
            return null;
        }        
    }

    

    
}
