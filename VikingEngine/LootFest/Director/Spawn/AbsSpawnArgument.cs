using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest
{
    struct SpawnArgumentCounter
    {
        public AbsSpawnArgument current;
        AbsSpawnArgument next;
        public SpawnArgumentCounter(AbsSpawnArgument arg)
        {
            next = arg;
            current = null;
        }

        public bool Next()
        {
            current = next;
            if (next != null)
            {
                next = next.linkedArgs;
            }
            return current != null;
        }
    }

    class AbsSpawnArgument
    {
        public AbsSpawnArgument linkedArgs;

        public void AddArg(AbsSpawnArgument arg)
        {
            if (linkedArgs == null)
            {
                linkedArgs = arg;
            }
            else
            {
                linkedArgs.AddArg(arg);
            }
        }

        virtual public void ApplyTo(GO.AbsUpdateObj go)
        { }
    }

    class SleepingSpawnArg : AbsSpawnArgument
    {
        public static SleepingSpawnArg ins = new SleepingSpawnArg();
    }
}
