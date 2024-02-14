using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Data.Condition
{
    class UnitConditions
    {
        Unit unit;
        public Buffs buffs = new Buffs();
        Buffs buffsProcess = new Buffs();

        public SpottedArray<AbsCondition> conditions =
            new SpottedArray<AbsCondition>(8);

        public UnitConditions(Unit unit)
        {
            this.unit = unit;
        }

        public void update_asynch()
        {
            buffsProcess.collect_asynch(unit);
            var store = buffs;
            buffs = buffsProcess;
            buffsProcess = store;

            var status = conditions.counter();
            while (status.Next())
            {
                if (status.sel.update_asynch(unit))
                {
                    status.sel.onRemoved(unit, true, true);
                    status.RemoveAtCurrent();
                }
            }
        }

        public AbsCondition GetBase(BaseCondition condition)
        {
            var status = conditions.counter();
            while (status.Next())
            {
                if (status.sel.Contains(condition))
                {
                    return status.sel;
                }
            }

            return null;
        }

        public AbsCondition Get(ConditionType type)
        {
            var status = conditions.counter();
            while (status.Next())
            {
                if (status.sel.ConditionType == type)
                {
                    return status.sel;
                }
            }

            return null;
        }

        public void Set(AbsCondition condition, bool increase, bool animate, bool netShare)
        {
            Set(condition.ConditionType, condition.Level, increase, animate, netShare);
        }

        public void Set(ConditionType type, int level, bool increase, bool animate, bool netShare)
        {
            if (type == ConditionType.Stunned && Get(ConditionType.StunImmune) != null)
            {
                return;
            }

            AbsCondition condition = Get(type);

            if (condition == null)
            {
                condition = AbsCondition.Create(type, level);
                conditions.Add(condition);
                condition.onApply(unit);

                if (animate)
                {
                    unit.textAnimation(condition.Icon, condition.Name);
                }
            }
            else
            {
                int levelChange = 0;

                if (increase)
                {
                    condition.Level += level;
                    levelChange = level;
                }
                else if (condition.Level < level)
                {
                    levelChange = level - condition.Level;
                    condition.Level = level;
                }

                if (levelChange > 0 && animate)
                {
                    unit.textAnimation(condition.Icon, TextLib.ValuePlusMinus(levelChange));
                }
            }

            if (netShare)
            {
                condition.netWriteApply(unit, true);
            }
        }

        public void Remove(ConditionType type, bool local)
        {
            var counter = conditions.counter();
            while (counter.Next())
            {
                if (counter.sel.ConditionType == type)
                {
                    counter.sel.onRemoved(unit, local, false);
                    counter.RemoveAtCurrent();
                }
            }
        }

        public void OnEvent(ToGG.Data.EventType eventType, object tag)
        {
            var statusCounter = conditions.counter();
            while (statusCounter.Next())
            {
                if (statusCounter.sel.OnEvent(unit, eventType, tag) == ToGG.Data.PropertyEventAction.Remove)
                {
                    statusCounter.sel.onRemoved(unit, true, false);
                    statusCounter.RemoveAtCurrent();
                }
            }
        }

        //public void attackCountModifiers(BattleSetup coll,
        //   bool isAttacker)
        //{
        //    var statuses = conditions.counter();
        //    while (statuses.Next())
        //    {
        //        statuses.sel.attackCountModifiers(coll, isAttacker);
        //    }
        //}

        public void AddToUnitCard(ToggEngine.Display2D.UnitCardDisplay card, ref Vector2 position)
        {
            var buffsList = buffs.total.properties();

            if (conditions.Count > 0 || buffsList != null)
            {
                position.Y += Engine.Screen.BorderWidth;

                List<AbsUnitStatus> statusList = new List<AbsUnitStatus>();
                if (conditions.Count > 0)
                {
                    statusList.AddRange(conditions.toList());
                }
                if (buffsList != null)
                {
                    statusList.AddRange(buffsList);
                }

                //SORT LIST
                List<ToGG.Data.Property.AbsProperty> positive = null, neutral = null, negative = null;
                foreach (var m in statusList)
                {
                    int isPositive = m.StatusIsPositive;
                    if (isPositive == 0)
                    {
                        arraylib.AddOrCreate(ref neutral, m);
                    }
                    else if (isPositive < 0)
                    {
                        arraylib.AddOrCreate(ref negative, m);
                    }
                    else
                    {
                        arraylib.AddOrCreate(ref positive, m);
                    }
                }

                card.propertyList(ref position, positive,
                    SpriteName.toggConditionPositiveTex);
                card.propertyList(ref position, neutral,
                    SpriteName.toggConditionNeutralTex);
                card.propertyList(ref position, negative,
                    SpriteName.toggConditionNegativeTex);

            }


        }
    }
}
