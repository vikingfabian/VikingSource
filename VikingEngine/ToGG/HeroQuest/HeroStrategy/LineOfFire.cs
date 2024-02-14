using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest.HeroStrategy
{
    class LineOfFire : AbsHeroStrategy
    {
        public LineOfFire()
        {
            cardSprite = SpriteName.hqStrategyLineOfFire;
            name = "Line of fire";
            description = "Move and shoot one arrow in a straight line, that hits every enemy it passes.";

            groupAttack = true;
            staminaCost = 1;
            coolDownTurns = new ToggEngine.Data.CooldownCounter(2);
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 1, 1);
        }

        public override void collectBoardUiTargets(Unit heroUnit, List<IntVector2> boardUiTargets)
        {
            // heroUnit.attackTargets.targets.list.Clear();
            if (heroUnit.ableToProjectileAttack())
            {
                foreach (var dir in IntVector2.Dir8Array)
                {
                    IntVector2 start = heroUnit.squarePos + dir * 2;

                    List<AttackTarget> validTargets;
                    List<AttackTarget> group = collectAttackGroup(heroUnit, start, out validTargets);
                    if (validTargets.Count > 0)
                    {
                        foreach (var m in validTargets)
                        {
                            boardUiTargets.Add(m.position);
                        }
                        //heroUnit.attackTargets.targets.list.AddRange(group);
                    }
                }
            }
        }

        public override List<AttackTarget> collectAttackGroup(Unit heroUnit, IntVector2 startPos, 
            out List<AttackTarget> validTargets)
        {
            List<AttackTarget> result = new List<AttackTarget>();
            validTargets = new List<AttackTarget>();

            IntVector2 diff = startPos - heroUnit.squarePos;

            if (!heroUnit.lockedInMelee() &&
                heroUnit.InMinProjectileRange(startPos) &&
                diff.IsOrthogonalOrDiagonal())
            {
                IntVector2 dir = diff.Normal();
                IntVector2 pos = heroUnit.squarePos + dir * 2;

                while (true)
                {
                    if (tryAddTargetToGroup(pos, heroUnit, startPos, result, validTargets, true))
                    {
                        pos += dir;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return result;
        }       

        //public override List<ActionType> actionList()
        //{
        //    return new List<ActionType>() { ActionType.Move, ActionType.Attack };
        //}

        override public HeroStrategyType Type { get { return HeroStrategyType.LineOfFire; } }
    }
}
