using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest.HeroStrategy
{
    class Swing3 : AbsHeroStrategy
    {
        public Swing3()
        {
            cardSprite = SpriteName.hqStrategySwing3;
            name = "Swing 3";
            description = "Move and make a 3 square wide attack";

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
            //heroUnit.attackTargets.targets.list.Clear();

            foreach (var dir in IntVector2.Dir8Array)
            {
                IntVector2 pos = heroUnit.squarePos + dir;

                List<AttackTarget> validTargets;
                collectAttackGroup(heroUnit, pos, out validTargets);

                if (arraylib.HasMembers(validTargets))
                {
                    boardUiTargets.Add(pos);
                    //tryAddTargetToGroup(pos, heroUnit, pos, heroUnit.attackTargets.targets.list, validTargets, false);
                }
            }
        }

        public override List<AttackTarget> collectAttackGroup(Unit heroUnit, IntVector2 startPos, out List<AttackTarget> validTargets)
        {
            List<AttackTarget> result = new List<AttackTarget>();
            validTargets = new List<AttackTarget>();


            IntVector2 dir = startPos - heroUnit.squarePos;
            if (dir.HasValue() && heroUnit.InMeleeRange(startPos))
            {
                Dir8 angle = conv.ToDir8(dir);
                IntVector2 pos = startPos;

                angle = lib.Rotate(angle, -1);

                for (int i = 0; i < 3; ++i)
                {
                    IntVector2 targetPos = heroUnit.squarePos + IntVector2.FromDir8(angle);
                    tryAddTargetToGroup(targetPos, heroUnit, startPos, result, validTargets, false);

                    angle = lib.Rotate(angle, 1);
                }
            }
            return result;
        }

        //public override List<ActionType> actionList()
        //{
        //    return new List<ActionType>() { ActionType.Move, ActionType.Attack };
        //}

        override public HeroStrategyType Type { get { return HeroStrategyType.Swing3; } }
    }
}
