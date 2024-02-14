using VikingEngine.EngineSpace.Reflection;
using VikingEngine.LootFest.GO.Characters.AI;
using VikingEngine.LootFest.Map;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO
{
    class Spawner
    {
        /* Properties */
        public bool CanSpawn { get { return respawnsLeft != 0 && spawns.Count < multiInstanceCount; } }
        public int AliveCount { get { return spawns.Count; } }

        /* Properties */
        public WorldPosition WorldPos { get { return new WorldPosition(worldOffset + localOffset); } }

        /* Fields */
        DelayedCtor constructor;
        object boxedData;
        IntVector3 localOffset;
        IntVector3 worldOffset;

        List<AbsUpdateObj> spawns;
        int respawnCount;
        int respawnsLeft;
        int multiInstanceCount;
        Action<object> onCreatedDelegate;
        Type spawnType;

        /* Constructors */
        //public Spawner(Type spawnType, IntVector3 localOffset, int respawnCount, int multiInstanceCount, object boxedData)
        public Spawner(Type spawnType, IntVector3 localOffset, int respawnCount, int multiInstanceCount, object boxedData, Action<object> onCreatedDelegate)
        {
            this.spawnType = spawnType;
            this.localOffset = localOffset;
            this.respawnCount = respawnCount;
            this.multiInstanceCount = multiInstanceCount;
            Reset();

#if PCGAME
            if (!spawnType.IsSubclassOf(typeof(AbsUpdateObj)))
            {
                throw new Exception("A type passed to Spawner must inherit from AbsUpdateObj");
            }
#endif
            constructor = new DelayedCtor(spawnType, typeof(GoArgs));

            this.boxedData = boxedData;
            this.onCreatedDelegate = onCreatedDelegate;
        }

        /* Methods */
        public void Spawn()
        {
            GoArgs args = new GoArgs(WorldPos);
            AbsUpdateObj obj = (AbsUpdateObj)constructor.Invoke(args);
            spawns.Add(obj);
            --respawnsLeft;
            if (onCreatedDelegate != null)
                onCreatedDelegate(obj);
        }

        public void RefreshAliveCount()
        {
            for (int i = 0; i < spawns.Count; )
            {
                AbsUpdateObj spawn = spawns[i];

                if (spawn != null && spawn.Dead)
                {
                    spawns.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }
        }

        /* State initialization methods */
        public void TransformToWorldSpace(IntVector3 worldOffset)
        {
            this.worldOffset = worldOffset;
        }

        public void RotateAroundModelCenter(IntVector2 rotationOrigin, Dir4 forward)
        {
            // move to center as origin.
            IntVector2 planePos = localOffset.XZplaneCoords;
            planePos -= rotationOrigin;

            // rotate
            if (forward == Dir4.E)
            {
                planePos = VectorExt.RotateVector90DegreeRight(planePos);
            }
            else if (forward == Dir4.S)
            {
                planePos = -planePos;
            }
            else if (forward == Dir4.W)
            {
                planePos = VectorExt.RotateVector90DegreeLeft(planePos);
            }

            //move back to top left as origin.
            planePos += rotationOrigin;

            localOffset.XZplaneCoords = planePos;
        }

        public void Reset()
        {
            respawnsLeft = respawnCount;
            spawns = new List<AbsUpdateObj>(multiInstanceCount);
        }
    }

    class SpawnerCollection : AbsUpdateable
    {
        /* Properties */
        public override UpdateType UpdateType { get { return UpdateType.Lazy; } }

        /* Fields */
        List<Spawner> spawners;
        float spawnIntervalMilliseconds;
        float millisecondsSinceLastSpawn;
        int maxSpawns;

        /* Constructors */
        public SpawnerCollection(List<Spawner> spawners, float spawnIntervalSeconds, int maxSpawns)
            : base(true)
        {
            this.spawners = spawners;
            this.maxSpawns = maxSpawns;
            spawnIntervalMilliseconds = spawnIntervalSeconds * 1000;
        }

        /* Methods */
        public override void Time_Update(float time)
        {
            if (!Ref.isPaused)
            {
                int spawnCount = 0;
                // TODO(Martin): If this ever becomes a bottleneck, which
                // seems unlikely as of writing this, keep track of spawn
                // instead. But the below is safer in terms of state
                // synchronization.
                foreach (Spawner spawner in spawners)
                {
                    spawner.RefreshAliveCount();
                    spawnCount += spawner.AliveCount;
                }

                if (spawnCount < maxSpawns)
                {
                    millisecondsSinceLastSpawn += time;

                    if (millisecondsSinceLastSpawn > spawnIntervalMilliseconds)
                    {
                        Debug.Log("It's time!");
                        TryGenerateGameObjects();
                        millisecondsSinceLastSpawn = 0;
                    }
                }
            }
        }

        public void TryGenerateGameObjects()
        {
            List<Spawner> eligibleForSpawn = new List<Spawner>(spawners.Count);

            foreach (Spawner spawner in spawners)
            {
                if (spawner.WorldPos.Screen.CorrectLoaded && spawner.CanSpawn)
                {
                    Debug.Log("Found eligible");
                    eligibleForSpawn.Add(spawner);
                }
            }

            if (eligibleForSpawn.Count > 0)
            {
                Debug.Log("Spawning one");
                Spawner spawner = eligibleForSpawn[Ref.rnd.Int(eligibleForSpawn.Count)];

                //TODO(Martin): Elaborate on this GoArgs!
                //WorldPosition pos = new WorldPosition(wp.PlaneCoordinates + spawn.CalculateRotatedOffset().XZplaneCoords, 0);
                spawner.Spawn();
            }
        }
    }
}
