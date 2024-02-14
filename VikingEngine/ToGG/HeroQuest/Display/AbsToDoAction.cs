using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.Data.Property;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    abstract class AbsToDoAction
    {
        public Unit unit;

        public AbsToDoAction(Unit unit)
        {
            this.unit = unit;
        }

        abstract public SpriteName Icon { get; }

        abstract public ValueBar Count { get; }
    }

    class ToDoMove : AbsToDoAction
    {
        public ToDoMove(Unit unit)
            : base(unit)
        { }

        public override SpriteName Icon => SpriteName.cmdUnitMoveGui_Small;

        public override ValueBar Count
        {
            get
            {
                int hasMoved, movementLeft, max, staminaMoves, backStabs;
                unit.moveInfo(out hasMoved, out movementLeft, out staminaMoves, out max, out backStabs);

                var moves = new ValueBar(movementLeft, max);
                return moves;
            }
        }
    }

    class ToDoAttack : AbsToDoAction
    {
        public ToDoAttack(Unit unit)
            : base(unit)
        { }

        public override SpriteName Icon => SpriteName.cmdUnitMeleeGui;

        public override ValueBar Count => unit.data.hero.availableStrategies.active.attacks;
    }

    class ToDoUseSkill : AbsToDoAction
    {
        AbsUnitProperty skill;
        public ToDoUseSkill(Unit unit, AbsUnitProperty skill)
            : base(unit)
        {
            this.skill = skill;
        }

        public override SpriteName Icon => skill.Icon;

        public override ValueBar Count => new ValueBar(skill.useCount.count, skill.useCount.baseCount);
    }
}
