using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Director
{
    /// <summary>
    /// Wrapper to take care of boss death or restart
    /// </summary>
    class BossManager : AbsUpdateable
    {
        List<GO.AbsUpdateObj> boss;
        List<GO.AbsUpdateObj> bossMinions;
        BlockMap.AbsLevel level;
        VikingEngine.LootFest.GO.Physics.RectangleAreaBoundary boundary = null;
        public VikingEngine.LootFest.Players.BabyLocation location;
        public Map.WorldPosition babySpawnPos;

        public BossManager(GO.AbsUpdateObj boss, BlockMap.AbsLevel level, 
            VikingEngine.LootFest.Players.BabyLocation location)
            :base(true)
        {
            this.level = level;
            this.location = location;
            this.babySpawnPos = boss.WorldPos;//babySpawnPos;

            LfRef.progress.BeginBossBattle();
            //level.BossManager = this;
            this.boss = new List<GO.AbsUpdateObj>(2);
            addBossObject(boss, false);
            bossMinions = new List<GO.AbsUpdateObj>();

        }

        public void addBossObject(GO.AbsUpdateObj obj, bool minion)
        {
            if (minion)
            {
                bossMinions.Add(obj);
            }
            else
            {
                if (boundary != null)
                {
                    obj.boundary = boundary;
                }
                boss.Add(obj);
            }

            obj.SetAsManaged();
            obj.managedGameObject = true;
            obj.levelCollider.SetLockedToArea();
        }

        public override void Time_Update(float time)
        {
            for (int i = boss.Count - 1; i >= 0; --i)
            {
                if (boss[i].IsDeleted)
                {
                    if (boss.Count > 1)
                    { //There are more boss parts to kill
                        boss.RemoveAt(i);
                    }
                    else //boss count == 0
                    { //Boss completely removed
                        if (boss[i].IsKilled)
                        { //Is dead, spawn baby
                            if (babySpawnPos.IsZero())
                            {
                                throw new Exception("babySpawnPos Is Empty: " + location.ToString());
                            }

                            new Effects.BossDeathExplosion(new GO.GoArgs(boss[i].Position + Vector3.Up * 1.5f));
                            LfRef.progress.EndBossBattle(location, babySpawnPos);
                        }
                        else
                        { //Reset boss area
                            //level.BossLock.ResetLock();
                        }

                        foreach (var minion in bossMinions)
                        {
                            if (minion.Alive)
                            {
                                minion.DeleteMe();
                                GO.Characters.AbsCharacter c = minion as GO.Characters.AbsCharacter;
                                if (c != null)
                                {
                                    c.BlockSplatter();
                                }
                            }
                        }
                        //level.BossManager = null;
                        this.DeleteMe();
                    }

                    
                }
            }
        }

    }
}
