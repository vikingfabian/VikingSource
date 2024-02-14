using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest;
using VikingEngineShared.ToGG.ToggEngine.Data;
using VikingEngine.ToGG.Commander;

namespace VikingEngine.ToGG.Data.Property
{
    class UnitPropertyColl
    {
        public List<AbsUnitProperty> members = null;

        public void set(params AbsUnitProperty[] members)
        {
            this.members = new List<AbsUnitProperty>(members);
        }
        public void set(List<AbsUnitProperty> members)
        {
            this.members = members;
        }

        public ForListLoop<AbsUnitProperty> Loop()
        {
            return new ForListLoop<AbsUnitProperty>(members);
        }

        public void Add(AbsUnitProperty m, ToggEngine.GO.AbsUnit unit = null)
        {
            if (members == null)
            {
                members = new List<AbsUnitProperty> { m };
            }
            else
            {
                members.Add(m);
            }

            m.OnApplied(unit);
        }

        public void remove(AbsUnitProperty m, ToggEngine.GO.AbsUnit unit = null)
        {
            if (members != null)
            {
                members.Remove(m);
                m.OnRemoved(unit);

                if (members.Count == 0)
                {
                    members = null;
                }
            }
        }

        public void remove(UnitPropertyType property, ToggEngine.GO.AbsUnit unit = null)
        {
            for (int i = members.Count - 1; i >= 0; --i)
            {
                if (members[i].Type == property)
                {
                    var m = members[i];
                    members.RemoveAt(i);
                    m.OnRemoved(unit);
                }
            }
        }

        public bool HasProperty(UnitPropertyType property)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m.Type == property)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public PropertyMoveModifiers moveModifiers()
        {
            PropertyMoveModifiers result = new PropertyMoveModifiers();
            if (members != null)
            {
                foreach (var m in members)
                {
                    m.moveModifiers(ref result);
                }
            }

            return result;
        }

        public bool HasEnabledProperty(UnitPropertyType property)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m.Type == property &&
                        (m.useCount == null || m.useCount.UseEnabled))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public AbsUnitProperty Get(UnitPropertyType property)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m.Type == property)
                    {
                        return m;
                    }
                }
            }

            return null;
        }

        public bool HasProperty(params UnitPropertyType[] properties)
        {
            if (members != null)
            {
                foreach (var m in properties)
                {
                    if (HasProperty(m))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool HasProperty(out UnitPropertyType found, params UnitPropertyType[] properties)
        {
            if (members != null)
            {
                foreach (var m in properties)
                {
                    if (HasProperty(m))
                    {
                        found = m;
                        return true;
                    }
                }
            }
            found = UnitPropertyType.Num_Non;
            return false;
        }

        public HeroQuest.Players.Ai.AbsUnitAiProperty aiObjective()
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m.IsAiState)
                    {
                        return (HeroQuest.Players.Ai.AbsUnitAiProperty)m;
                    }
                }
            }

            return null;
        }

        public bool overridingAiActions(
            HeroQuest.Players.Ai.UnitAiActions aiActions,
            HeroQuest.Players.Ai.UnitActionCount actionCount,
            out HeroQuest.Players.AiState result)
        {
            if (members != null)
            {
                foreach (var m in members)
                {
                    if (m.overridingAiActions(aiActions, actionCount, out result))
                    {
                        return true;
                    }
                }
            }

            result = HeroQuest.Players.AiState.None;
            return false;
        }

        public void removeAiState()
        {
            for (int i = members.Count - 1; i >= 0; --i)
            {
                if (members[i].IsAiState)
                {
                    members.RemoveAt(i);
                }
            }
        }

        public bool HasMembers()
        {
            return members != null && members.Count > 0;
        }

        public List<AbsMonsterAction> specialActions(SpecialActionClass filter)
        {
            List<AbsMonsterAction> result = null;

            if (HasMembers())
            {
                foreach (var m in members)
                {
                    var action = m.Action;
                    if (action != null &&
                        (filter == SpecialActionClass.Any || filter == action.ActionClass))
                    {
                        arraylib.AddOrCreate(ref result, action);
                    }
                }
            }
            return result;
        }

        public void OnEvent(EventType eventType, bool local, object tag, ToggEngine.GO.AbsUnit unit)
        {
            if (members != null)
            {
                for (int i = members.Count -1; i >= 0; --i)
                {
                    var m = members[i];

                    if (m.useCount != null)
                    {
                        var action = m.useCount.OnEvent(eventType);

                        switch (action)
                        {
                            case PropertyEventAction.Remove:
                                members.RemoveAt(i);
                                break;
                            case PropertyEventAction.Reset:
                                m.useCount.reset();
                                break;
                        }
                    }

                    m.OnEvent(eventType, local, tag, unit);
                }
            }
        }

        public void collectDefence(DefenceData defence, bool onCommit)
        {
            if (members != null)
            {
                for (int i = members.Count - 1; i >= 0; --i)
                {
                    members[i].collectDefence(defence, onCommit);
                }
            }
        }

        public void collectOpponentDefence(DefenceData defence, bool onCommit)
        {
            if (members != null)
            {
                for (int i = members.Count - 1; i >= 0; --i)
                {
                    members[i].collectOpponentDefence(defence, onCommit);
                }
            }
        }

    }
}
