using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Data.Condition
{
    class Buffs
    {
        public StaticList<UnitBuffsMember> members = new StaticList<UnitBuffsMember>(4, true);
        public Buff total = new Buff();

        void clear()
        {
            total = Buff.Empty;
            members.QuickClear();
        }

        public void collect_asynch(Unit unit)
        {
            clear();

            Buff addBuff;

            var group = unit.PlayerHQ.hqUnits;
            foreach (var m in group.friendly)
            {
                if (m != unit)
                {
                    var properties = m.hq().data.properties;
                    if (properties.members != null)
                    {
                        foreach (var p in properties.members)
                        {
                            if (p.buffUnit(m, unit, true, out addBuff))
                            {
                                total.Add(addBuff);

                                UnitBuffsMember newMember;
                                if (members.NextIsNull())
                                {
                                    newMember = new UnitBuffsMember();
                                    members.Add(newMember);
                                }
                                else
                                {
                                    newMember = members.RecycleNext();
                                }

                                newMember?.Set(addBuff, p, true);
                            }
                        }
                    }
                }
            }
        }

        public List<UnitBuffsMember> listBuffs(bool filterAttacksOnly)
        {
            List<UnitBuffsMember> result = new List<UnitBuffsMember>(members.PreviousCount);
            var counter = members.PrevCounter();
            while (counter.Next())
            {
                bool add = true;

                if (filterAttacksOnly)
                {
                    add = counter.sel.buff.attackDice > 0;
                }

                if (add)
                {
                    result.Add(counter.sel);
                }
            }

            return result;
        }
    }
}
