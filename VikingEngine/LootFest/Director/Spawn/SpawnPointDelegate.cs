using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO;

namespace VikingEngine.LootFest
{
    class SpawnPointDelegate : AbsSpawnPoint
    {
        CreateGameObjectDelegate action;
        SpawnPointData spawnData;
        int spawnCount = 1;
        bool managedSpawn = false;
        public AbsSpawnArgument spawnArgs = null;

        public SpawnPointDelegate(IntVector2 posXZ, CreateGameObjectDelegate action, SpawnPointData spawnData, SpawnImportance importance, bool netSynched, int spawnCount, bool managedSpawn)
            : this(new Map.WorldPosition(posXZ, Map.WorldPosition.ChunkStandardHeight), action, spawnData, importance, netSynched, spawnCount, managedSpawn)
        { }
        public SpawnPointDelegate(IntVector2 posXZ, CreateGameObjectDelegate action, SpawnPointData spawnData, SpawnImportance importance, bool netSynched)
            : this(new Map.WorldPosition(posXZ, Map.WorldPosition.ChunkStandardHeight), action, spawnData, importance, netSynched, 1, false)
        { }

        public SpawnPointDelegate(Map.WorldPosition wp, CreateGameObjectDelegate action, SpawnPointData spawnData,
            SpawnImportance importance, bool netSynched)
            : this(wp, action, spawnData, importance, netSynched, 1, false)
        { }

        public SpawnPointDelegate(Map.WorldPosition wp, CreateGameObjectDelegate action, SpawnPointData spawnData,
            SpawnImportance importance, bool netSynched, int spawnCount, bool managedSpawn)
        {
            this.networkSynchedSpawn = netSynched;
            this.wp = wp;
            this.action = action;
            this.spawnData = spawnData;
            this.importance = importance;
            this.spawnCount = spawnCount;
            this.managedSpawn = managedSpawn;
            //LfRef.spawner.Add(this);
        }

        public override void Spawn()
        {
            if (spawnLock <= 0)
            {
                if (managedSpawn)
                {
                    spawnLock = 1;
                }

                base.Spawn();

                GoArgs args = new GoArgs(wp, spawnData.type, spawnData.level, spawnData.direction, this);
                args.linkedSpawnArgs = spawnArgs;

                if (action == null)
                {
                    for (int i = 0; i < spawnCount; ++i)
                    {
                        var obj = Director.GameObjectSpawn.Spawn(args);
                        if (managedSpawn)
                        {
                            obj.SetAsManaged();
                        }
                        if (spawnArgs != null)
                        {
                            obj.setSpawnArgument(spawnArgs);
                        }
                    }
                }
                else
                {
                    action(args);
                }
            }
        }

        public override float relevanceValue()
        {
            //
            throw new NotImplementedException();
        }

        override public bool spawnOnGoGenerate { get { return true; } }

        public override string ToString()
        {
            string name;
            if (action == null)
            {
                name = spawnData.type.ToString();
            }
            else
            {
                name = action.ToString();
            }
            return "Spawn Point: " + name + spawnData.level.ToString() + wp.ChunkGrindex.ToString();
        }
    }
}
