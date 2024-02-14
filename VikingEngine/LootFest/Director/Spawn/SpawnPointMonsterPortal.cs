using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map;
using VikingEngine.EngineSpace.Reflection;
using VikingEngine.LootFest.GO;

namespace VikingEngine.LootFest
{
    class SpawnPointMonsterPortal : AbsSpawnPoint
    {
        /* Properties */
        //public bool CanSpawn { get { return respawnsLeft != 0 && spawns.Count < maxActiveInstances; } }
        //public int AliveCount { get { return spawns.Count; } }

        /* Properties */
        //public WorldPosition WorldPos { get { return new WorldPosition(worldOffset + localOffset); } }

        /* Fields */
        //DelayedCtor constructor;
        public AbsSpawnArgument boxedData;
        //IntVector3 localOffset;
        //IntVector3 worldOffset;
        SuggestedSpawns suggestedSpawns;

        List<AbsUpdateObj> activeInstances;
        //int respawnCount;
        public int respawnsLeft = 12;
        public int maxActiveInstances = 3;
        float timeStamp = 0;

        public float spawnRateSec = 2f;

        /* Constructors */
        public SpawnPointMonsterPortal(Map.WorldPosition wp, SuggestedSpawns spawnTypes)
            :this(wp, spawnTypes, null)
        { }

        public SpawnPointMonsterPortal(Map.WorldPosition wp, SuggestedSpawns spawnTypes, AbsSpawnArgument boxedData)
        {
            this.wp = wp;
            this.suggestedSpawns = spawnTypes;
            
            //LfRef.spawner.Add(this);
        }


        /* Methods */
        public override void  Spawn()
        {
            timeStamp = Ref.TotalTimeSec;
 	         base.Spawn();
            SpawnPointData pointData = suggestedSpawns.GetRandom();
            GoArgs args = new GoArgs(wp, pointData.type, pointData.level, pointData.direction, this);
            args.linkedSpawnArgs = boxedData;

            var go = Director.GameObjectSpawn.SpawnMonster(args);
            if (activeInstances == null)
            {
                activeInstances = new List<AbsUpdateObj>(maxActiveInstances);
            }
            activeInstances.Add(go);
            //spawns.Add((AbsUpdateObj)constructor.Invoke(args));
            --respawnsLeft;
        }

        public void RefreshAliveCount()
        {
            if (activeInstances != null)
            {
                for (int i = 0; i < activeInstances.Count; )
                {
                    AbsUpdateObj spawn = activeInstances[i];

                    if (spawn != null && spawn.Dead)
                    {
                        activeInstances.RemoveAt(i);
                    }
                    else
                    {
                        ++i;
                    }
                }
            }
        }

        ///* State initialization methods */
        //public void TransformToWorldSpace(IntVector3 worldOffset)
        //{
        //    this.worldOffset = worldOffset;
        //}

        //public void RotateAroundModelCenter(IntVector2 rotationOrigin, Dir4 forward)
        //{
        //    // move to center as origin.
        //    IntVector2 planePos = localOffset.XZplaneCoords;
        //    planePos -= rotationOrigin;

        //    // rotate
        //    if (forward == Dir4.E)
        //    {
        //        planePos = VectorExt.RotateVector90DegreeRight(planePos);
        //    }
        //    else if (forward == Dir4.S)
        //    {
        //        planePos = -planePos;
        //    }
        //    else if (forward == Dir4.W)
        //    {
        //        planePos = VectorExt.RotateVector90DegreeLeft(planePos);
        //    }

        //    //move back to top left as origin.
        //    planePos += rotationOrigin;

        //    localOffset.XZplaneCoords = planePos;
        //}

        //public void Reset()
        //{
            
        //}

        int instancesCount
        {
            get
            { return activeInstances == null? 0 : activeInstances.Count; }
        }

        public override float relevanceValue()
        {
            float timeDiff = Ref.TotalTimeSec - timeStamp;
            if (timeDiff < spawnRateSec)
            {
                return 0;
            }

            RefreshAliveCount();

            if (respawnsLeft == 0 || instancesCount >= maxActiveInstances)
            {
                return 0;
            }
            if (base.relevanceValue() <= 0)
            {
                return 0;
            }

            float heroDist = closestHeroDist();

            if (heroDist <= 4)
            {
                return 0;
            }
            return 100 - heroDist - instancesCount * 20;
        }

        override public bool spawnOnGoGenerate { get { return false; } }
    }
}
