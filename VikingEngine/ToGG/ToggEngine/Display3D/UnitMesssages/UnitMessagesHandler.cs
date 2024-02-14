using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Display3D
{
    class UnitMessagesHandler
    {
        List2<UnitMessageGroup> messageGroups = new List2<UnitMessageGroup>(4);

        public UnitMessagesHandler()
        {   
        }

        public void add(AbsUnit unit, AbsUnitMessage message)
        {
            const float Spacing = 0.02f;

            UnitMessageGroup group = tryGet(unit);
            if (group == null)
            {
                group = new UnitMessageGroup(unit);
                messageGroups.Add(group);
            }
            else
            {
                float height = message.MessageHeight + Spacing;

                for (int i = group.messages.Count -1; i >= 0; --i)
                {
                    group.messages[i].refreshGoalPos(height);
                    height += group.messages[i].MessageHeight + Spacing;
                }
            }
            group.messages.Add(message);
        }

        public void update()
        {
            messageGroups.loopBegin();
            while (messageGroups.loopNext())
            {
                if (messageGroups.sel.update())
                {
                    messageGroups.loopRemove();
                }
            }
        }

        UnitMessageGroup tryGet(AbsUnit unit)
        {
            foreach (var m in messageGroups)
            {
                if (m.unit == unit)
                {
                    return m;
                }
            }

            return null;
        }
    }

    class UnitMessageGroup
    {
        public AbsUnit unit;
        public List<AbsUnitMessage> messages = new List<AbsUnitMessage>(2);

        public UnitMessageGroup(AbsUnit unit)
        {
            this.unit = unit;
        }

        public bool update()
        {
            
            foreach (var m in messages)
            {
                m.update();
            }

            if (messages[0].TimedOut)
            {
                arraylib.PullFirstMember(messages).DeleteMe();
                return messages.Count == 0;
            }

            return false;
        }
    }
}
