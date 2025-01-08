using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.ToGG.HeroQuest.Data.UnitAction
{
    class ThrowConditionBomb : AbsUnitAction
    {
        public const int Range = 5;
        public const int PoisionLevel = 2;
        //Condition.ConditionType condition;
        HeroStrategy.AbsConditionBomb bomb;
        public ThrowConditionBomb(HeroStrategy.AbsConditionBomb bomb)//Condition.ConditionType condition)
        {
            this.bomb = bomb;
            useCount = new ToGG.Data.SkillUseCounter(1);
        }

        public override List<IntVector2> unitActionTargets(Unit unit)
        {
            List<IntVector2> result = new List<IntVector2>(16);

            Rectangle2 areaInRange = Rectangle2.FromCenterTileAndRadius(unit.squarePos, Range);
            ForXYLoop arealoop = new ForXYLoop(areaInRange);
            while (arealoop.Next())
            {
                var group = collectAttackGroup(unit, arealoop.Position);
                if (group.Count > 0)
                {
                    result.Add(arealoop.Position);
                }
            }

            return result;
        }

        public List<Unit> collectAttackGroup(Unit heroUnit, IntVector2 startPos)
        {
            List<Unit> result = new List<Unit>(9);

            if (heroUnit.InRangeAndSight(startPos, Range, false, false))
            {
                ForXYLoop loop = new ForXYLoop(Rectangle2.FromCenterTileAndRadius(startPos, 1));
                while (loop.Next())
                {
                   var target = toggRef.board.getUnit(loop.Position);
                    if (target != null && heroUnit.IsOpponent(target))
                    {
                        result.Add(target.hq());
                    }
                }
            }
            return result;
        }

        public override bool IsValidActionTarget(Unit unit, IntVector2 pos,
            out List<IntVector2> groupSelection)
        {
            groupSelection = null;

            var targets = collectAttackGroup(unit, pos);
            if (targets.Count > 0)
            {
                groupSelection = new List<IntVector2>(9);
                ForXYLoop loop = new ForXYLoop(Rectangle2.FromCenterTileAndRadius(pos, 1));
                while (loop.Next())
                {
                    if (toggRef.board.tileGrid.InBounds(loop.Position))
                    {
                        groupSelection.Add(loop.Position);
                    }
                }
                return true;
            }
            return false;
        }

        public override bool Use(Unit unit, IntVector2 pos)
        {
            var targets = collectAttackGroup(unit, pos);
            if (targets.Count > 0)
            {
                foreach (var m in targets)
                {
                    m.condition.Set(bomb.condition, PoisionLevel, true, true, true);                    
                }
                return true;
            }

            return false;
        }

        public override List<AbsRichBoxMember> actionTargetToolTip()
        {
            return new List<AbsRichBoxMember>
            {
                new RbImage(SpriteName.cmdThrowBombAction),
                new RbText("Throw bomb"),
            };
        }

        public override bool InstantAction => false;

        public override SpriteName Icon => SpriteName.cmdThrowBombAction;

        public override string Name => bomb.name;

        public override string Desc => bomb.actionDesc;

        public override UnitPropertyType Type => UnitPropertyType.Num_Non;
    }
}
