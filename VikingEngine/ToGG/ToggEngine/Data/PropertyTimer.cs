using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.Data
{
    abstract class AbsPropertyCounter
    {
        public int baseCount;
        public int count;

        /// <returns>Remove</returns>
        virtual public PropertyEventAction OnEvent(EventType eventType)
        {
            if (eventType == this.EventType)
            {
                if (--count <= 0)
                {
                    return CountOutAction;
                }
            }

            return PropertyEventAction.None;
        }

        virtual public void Use()
        {
            --count;
        }

        public void reset()
        {
            count = baseCount;
        }

        public bool UseEnabled => count > 0;

        virtual public EventType EventType => EventType.None;

        virtual protected PropertyEventAction CountOutAction => PropertyEventAction.Remove;
    }

    class TurnStartCounter : AbsPropertyCounter
    {
        public TurnStartCounter(int turns)
        {
            this.count = turns;
        }

        public override EventType EventType => EventType.TurnStart;
    }

    class SkillUseCounter : AbsPropertyCounter
    {
        public SkillUseCounter(int useCount)
        {
            this.count = useCount;
            baseCount = useCount;
        }
               
        public override EventType EventType => EventType.TurnStart;

        protected override PropertyEventAction CountOutAction => PropertyEventAction.Reset;
    }

    class UntilNextTurn : TurnStartCounter
    {
        public UntilNextTurn()
            : base(1)
        { }
    }

    enum EventType
    {
        None,
        GameStart,
        TurnStart,
        TurnEnd,
        UnitPositionChange,
        UnitDeath,
        ParentUnitMoved,
        OtherUnitMoved,
        ReadingQuestMission,
        StrategySelected,
        OpenBackpack,
        OpenAttackDisplay,
        Attack,
        AttackComplete,
        DamageTarget,
        DestroyedTarget,
        Heal,
        LevelProgress,
        DoorOpened,
    }

    enum PropertyEventAction
    {
        None,
        Remove,
        Reset,
    }
}
