using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Physics;
using VikingEngine.LootFest.GO.Characters;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    abstract class AbsEnemySpawner : AbsEnemy
    {
        static readonly IntervalF SpawnRate = new IntervalF(3000, 6000);

        Time spawnTime = SpawnRate.GetRandom();
        List<AbsCharacter> spawns = new List<AbsCharacter>();
        int MaxSpawns = 2;

        public AbsEnemySpawner(GoArgs args)
            :base(args)
        {
            Health = 2f;
        }

        public override void Time_Update(UpdateArgs args)
        {
            //base.Time_Update(args);

            if (localMember)
            {
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                    return;
                }

            }

            immortalityTime.CountDown();

            if (spawnTime.CountDown())
            {
                spawnTime.MilliSeconds = SpawnRate.GetRandom();
                for (int i = spawns.Count - 1; i >= 0; --i)
                {
                    if (!spawns[i].Alive)
                    {
                        spawns.RemoveAt(i);
                    }
                }

                if (spawns.Count < MaxSpawns)
                {
                    AbsCharacter spawn = spawnEnemy();//Director.MonsterSpawn.SpawnMonster(monsterType, WorldPosition, characterLevel);
                    if (spawn != null)
                    {
                        spawns.Add(spawn);
                    }
                }
            }
        }

        abstract protected AbsCharacter spawnEnemy();
        protected override bool pushable
        {
            get
            {
                return false;
            }
        }
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.NO_PHYSICS;
            }
        }

        public override bool canBeCardCaptured
        {
            get
            {
                return false;
            }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            return base.handleCharacterColl(character, collisionData, usesMyDamageBound);
        }
        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            //do nothing
        }

        override protected bool givesContactDamage
        { get { return false; } }
    }
}
