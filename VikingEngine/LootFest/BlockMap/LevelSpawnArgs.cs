using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.BlockMap
{
    class LockGroupSpawnArgs : AbsSpawnArgument
    {
        AbsLevel level;
        int lockId;

        public LockGroupSpawnArgs(AbsLevel level, int lockId)
        {
            this.level = level;
            this.lockId = lockId;
        }

        public override void ApplyTo(GO.AbsUpdateObj go)
        {
            //base.ApplyTo(go);
            if (go is GO.EnvironmentObj.AreaLock)
            {
                level.AddConnectedAreaLock((GO.EnvironmentObj.AreaLock)go, lockId);
            }
            else
            {
                level.AddLockConnectedGo(go, lockId);
                go.levelCollider.SetLockedToArea();
            }
        }
    }
}
