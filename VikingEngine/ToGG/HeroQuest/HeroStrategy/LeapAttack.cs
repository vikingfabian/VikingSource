using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.HeroQuest.Data.UnitAction;

namespace VikingEngine.ToGG.HeroQuest.HeroStrategy
{
    class LeapAttack : AbsHeroStrategy
    {
        LeapAttackAction leapAttackAction = new LeapAttackAction();

        public LeapAttack()
        {
            cardSprite = SpriteName.hqStrategyLeapAttack;
            name = "Leap attack";
            description = LeapAttackAction.LeapAttackDesc();

            coolDownTurns = new ToggEngine.Data.CooldownCounter(2);
            staminaCost = 1;
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            leapAttackAction.useCount.reset();
            setMoveAttackCount(heroUnit, 0, 0);
        }

        public override void collectActions(List<AbsUnitAction> unitActions)
        {
            base.collectActions(unitActions);
            unitActions.Add(leapAttackAction);
        }

        public override HeroStrategyType Type => HeroStrategyType.LeapAttack;
    }

    class LeapAttackAction : AbsUnitAction
    {
        public static string LeapAttackDesc()
        {
            return "Jump a in a straight line. Attack all units passed.";
        }

        public LeapAttackAction()
        {
            useCount = new SkillUseCounter(1);
        }

        public override List<IntVector2> unitActionTargets(Unit unit)
        {
            List<IntVector2> result = new List<IntVector2>((unit.data.move - 2) * 8);

            foreach (var dir in IntVector2.Dir8Array)
            {
                for (int dist = 2; dist <= unit.data.move; ++dist)
                {
                    IntVector2 pos = unit.squarePos + dir * dist;

                    if (toggRef.board.MovementRestriction(pos, unit, true) ==
                        ToggEngine.Map.MovementRestrictionType.Impassable)
                    {
                        break;
                    }
                    else if (toggRef.board.CanEndMovementOn(pos, unit))
                    {
                        result.Add(pos);
                    }

                }
            }

            return result;
        }

        public override bool IsValidActionTarget(Unit unit, IntVector2 pos,
            out List<IntVector2> groupSelection)
        {
            groupSelection = null;
            return unitActionTargets(unit).Contains(pos);
        }

        public override bool Use(Unit unit, IntVector2 pos)
        {
            if (toggRef.board.CanEndMovementOn(pos, unit))
            {
                List<ToggEngine.GO.AbsUnit> targets = new List<ToggEngine.GO.AbsUnit>(8);

                IntVector2 diff = (pos - unit.squarePos).Normal();
                IntVector2 checkPos = unit.squarePos + diff;
                do
                {
                    var target = toggRef.board.getUnit(checkPos);
                    if (target != null && unit.canTargetUnit(target))
                    {
                        targets.Add(target);
                    }
                    checkPos += diff;
                } while (checkPos != pos);

                unit.SetPosition(pos);
                hqRef.netManager.writeMoveUnit(unit, false);
                if (targets.Count > 0)
                {
                    unit.HqLocalPlayer.openAttackDisplay(new AttackTargetGroup(targets, true));
                }

                return true;
            }

            return false;
        }

        public override List<AbsRichBoxMember> actionTargetToolTip()
        {
            return new List<AbsRichBoxMember>{
                new RbImage(SpriteName.cmdFlying),
                new RbText("Jump here")
            };
        }

        

        public override string Name => "Leap attack";

        public override SpriteName Icon => SpriteName.cmdFlying;

        public override UnitPropertyType Type => UnitPropertyType.Num_Non;

        public override string Desc => LeapAttackDesc();

        public override bool InstantAction => false;
    }
}
