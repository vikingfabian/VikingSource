using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.BlockMap;

namespace VikingEngine.LootFest
{
    abstract class AbsSpawnPoint
    {
        public int spawnLock = 0;

        static int NextIndex = 0;
        public int uniqueIndex = NextIndex++;

        public bool inQue = false;
        public LevelEnum level = LevelEnum.NUM_NON;
        public int SpawnedCount;
        public Map.WorldPosition wp;
        public SpawnImportance importance;

        public bool networkSynchedSpawn = true;

        public bool earMark = false;

        virtual public float relevanceValue()
        {
            var c = LfRef.chunks.GetScreenUnsafe(wp);
            if (c == null || !c.generatedGameObjects)
            {
                return -1;
            }

            float value = 100;

            //for (int otherPIx = 0; otherPIx < LfRef.AllHeroes.Count; otherPIx++)
            //{
            //    int length = (wp.WorldXZ - LfRef.AllHeroes[otherPIx].WorldPos.WorldXZ).SideLength();
            //    if (length < 60)
            //    {
            //        return 0;
            //    }
            //    if (length < 90)
            //    {
            //        value /= 2;
            //    }
            //}

            return value;
        }

        protected float closestHeroDist()
        {
            float closest = float.MaxValue;

            for (int otherPIx = 0; otherPIx < LfRef.AllHeroes.Count; otherPIx++)
            {
                int length = (wp.WorldGrindex - LfRef.AllHeroes[otherPIx].WorldPos.WorldGrindex).LargestSideLength();
                if (length < closest)
                {
                    closest = length;
                }
            }

            return closest;
        }

        protected float heatDelta;

        virtual public void prepareSpawn_Asynch(float heatDelta)
        {
            inQue = true;
            this.heatDelta = heatDelta;
        }
        virtual public void Spawn()
        { inQue = false; }

        //public void addToDirector()
        //{
        //    LfRef.spawner.Add(this);
        //}

        abstract public bool spawnOnGoGenerate { get; }
    }
}
