using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Effects
{
    class ZombieSpawn : AbsInGameUpdateable
    {
        const float SpawnTimeSec = 2f;
        Time effectTimeLeft = new Time(SpawnTimeSec + 0.5f, TimeUnit.Seconds);
        Time spawnTimer = new Time(SpawnTimeSec, TimeUnit.Seconds);

        Map.WorldPosition wp;
        Vector3 position;

        public ZombieSpawn(Vector3 pos)
            : base(true)
        {
            wp = new Map.WorldPosition(pos);
            wp.SetAtClosestFreeY(1);

            position = wp.PositionV3;
        }

        public override void Time_Update(float time)
        {
            if (Ref.TimePassed16ms && Ref.rnd.Chance(0.6f))
            {
                new BouncingBlock2Dummie(position, Data.MaterialType.dirt_brown, 0.5f);
                Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.Dust, position, 1f, 16);
            }

            var spawnArgs = new GO.GoArgs(wp);

            if (spawnTimer.MilliSeconds > 0 && spawnTimer.CountDown())
            {
                if (Ref.rnd.Chance(0.7f))
                {
                    new GO.Characters.Monsters.Zombie(spawnArgs); 
                }
                else
                { new GO.Characters.Monsters.Skeleton(spawnArgs, null); }
            }
            if (effectTimeLeft.CountDown())
            {
                DeleteMe();
            }
        }
    }
}
