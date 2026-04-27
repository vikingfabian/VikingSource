using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.XP;
using VikingEngine.EngineSpace;
using VikingEngine.LootFest.Display;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars
{  
    partial class WorldData
    {
        public EcsStaticArray neighborCities;
        public GroupedResource[] cityResouces;
        public WorkPriority[] cityWork;
        public StorageSize[] cityStorage;

        const int WorkerXpCOUNT = (int)WorkExperienceType.NUM_NONE;
        int nextXpIndex = 0;
        bool[] WorkXpInUse;
        public StructList<WorkExperience> workerXp;
        
        public void InitCity(City city)
        {
            city.resourceComponentStartIndex = CityResoureIndex.COUNT * city.myIndex;
           
        }

        public void initWorkerXp(int cityCount)
        {
            int reserveWorkerCount = cityCount * 400;
            WorkXpInUse = new bool[reserveWorkerCount];
            workerXp = new StructList<WorkExperience>(reserveWorkerCount * WorkerXpCOUNT);
        }

        /// <returns>Entity index</returns>
        public int ReserveNextWorkXpIndex()
        {
            int loop = 0;
            while (WorkXpInUse[nextXpIndex])
            { 
                ++nextXpIndex;
                if (nextXpIndex >= WorkXpInUse.Length)
                {
                    nextXpIndex = 0;
                    loop++;
                    if (loop > 2)
                    {
                        //throw new Exception("Out of worker xp")
                        nextXpIndex = WorkXpInUse.Length;
                        workerXp.Resize();
                        Array.Resize(ref WorkXpInUse, WorkXpInUse.Length * 2);
                        break;
                    }
                }
            }

            WorkXpInUse[nextXpIndex] = true;
            return nextXpIndex;
        }

        public void FreeWorkerXp(int index)
        {
            if (index >= 0)
            {
                ResetWorkerXp(index);

                WorkXpInUse[index] = false;
            }
        }

        public void ResetWorkerXp(int index)
        {
            if (index >= 0)
            {
                int start = index * WorkerXpCOUNT;
                //Clear out!
                for (int i = 0; i < WorkerXpCOUNT; ++i)
                {
                    workerXp.array[i + start] = WorkExperience.Empty;
                }
            }
        }

        public WorkExperience GetWorkXp(int index, WorkExperienceType type)
        {
#if DEBUG
            if (!workerXp.InBound_Array(index * WorkerXpCOUNT + (int)type))
            {
                throw new Exception();
            }
#endif
            int arrayIx = index * WorkerXpCOUNT + (int)type;
            if (workerXp.InBound_Array(arrayIx))
            {
                return workerXp.array[arrayIx];
            }

            return empty;
        }
        public void SetWorkXp(int index, WorkExperienceType type, byte xp)
        {
#if DEBUG
            if (!workerXp.InBound_Array(index * WorkerXpCOUNT + (int)type))
            {
                throw new Exception();
            }
#endif
            workerXp.array[index * WorkerXpCOUNT + (int)type].xp = xp;
        }

        WorkExperience empty = new WorkExperience();
        public ref WorkExperience GetRefWorkXp(int index, WorkExperienceType type)
        {
            int arrayIx = index * WorkerXpCOUNT + (int)type;
#if DEBUG
            if (!workerXp.InBound_Array(arrayIx))
            {
                throw new Exception();
            }
#endif
            if (index >= 0)
            {
                return ref workerXp.array[arrayIx];
            }

            return ref empty;
        }

        public void writeWorkXp(int index, System.IO.BinaryWriter w)
        {
            if (index >= 0)
            {
                int start = index * WorkerXpCOUNT;

                for (int i = 0; i < WorkerXpCOUNT; ++i)
                {
                    workerXp.array[i + start].write(w);
                }
            }
            else
            {
                for (int i = 0; i < WorkerXpCOUNT; ++i)
                {
                    WorkExperience.Empty.write(w);
                }
            }
        }
        public void readWorkXp(int index, System.IO.BinaryReader r, int subVersion)
        {
            int start = index * WorkerXpCOUNT;

            for (int i = 0; i < WorkerXpCOUNT; ++i)
            {
                workerXp.array[i + start].read(r);
            }
        }

        public int GetWorkXpScore(int index)
        {
            int score = 0;
            if (index >= 0)
            {
                int start = index * WorkerXpCOUNT;

                for (int i = 0; i < WorkerXpCOUNT; ++i)
                {
                    score += MathExt.Square(workerXp.array[i + start].xp);
                } 
            }
            return score;
        }

        public List<(WorkExperience xp, WorkExperienceType type)> listWorkXp(int index)
        {
            var xpPairs = new List<(WorkExperience xp, WorkExperienceType type)>(8);

            int start = index * WorkerXpCOUNT;

            for (int i = 0; i < WorkerXpCOUNT; ++i)
            {
                if (workerXp.array[i + start].xp >= DssConst.WorkXpToLevel)
                {
                    xpPairs.Add(new (workerXp.array[i + start], (WorkExperienceType)i));
                }
            }
            
            // Sort the list by XP in descending order
            xpPairs.Sort((a, b) => b.xp.CompareTo(a.xp));

            return xpPairs;
        }

        public void clearCityResources(City city)
        {
            int ex_end = city.resourceComponentStartIndex + CityResoureIndex.COUNT;
            for (int i = city.resourceComponentStartIndex; i < ex_end; i++)
            {
                cityResouces[i].amount = 0;
            }
        }

        public const int DefaultBuffer_Wood = 300;
        public const int DefaultBuffer_SkinLinnen = 300;

        public void Init_CityComponents(int cityCount)
        {
            initWorkerXp(cityCount);

            cityResouces = new GroupedResource[CityResoureIndex.COUNT * cityCount];
            neighborCities = new EcsStaticArray(16, cityCount);
            cityWork = new WorkPriority[WorkTemplate.COUNT * cityCount];
            cityStorage = new StorageSize[StorageSize.COUNT * cityCount];

            int resourceStart = 0;
            //int workStart = 0;

            int startWood, startLinnen, startFood;
            if (DssRef.storage.gameRuleset.factionStartSize == FactionStartSize.Settler)
            {
                startWood = 120;
                startLinnen = 120;
                startFood = 120;
            }
            else
            {
                startWood = 20;
                startLinnen = 20;
                startFood = 200;
            }

            for (int i = 0; i < cityWork.Length; i++)
            {
                cityWork[i] = new WorkPriority(0);
            }

            int resStartIndex = 0;
            int startIndex = 0;
            for (int cityIx = 0; cityIx < cityCount; cityIx++)
            {
                for (int resourceIx = 0; resourceIx < CityResoureIndex.COUNT; ++resourceIx)
                {
                    cityResouces[resourceStart + resourceIx] = new GroupedResource();
                }

                cityResouces[resStartIndex + CityResoureIndex.wood].amount = startWood;
                cityResouces[resStartIndex + CityResoureIndex.fuel].amount = 100;
                cityResouces[resStartIndex + CityResoureIndex.stone].amount = 20;
                cityResouces[resStartIndex + CityResoureIndex.food].amount = startFood;
                cityResouces[resStartIndex + CityResoureIndex.skinLinnen].amount = startLinnen;                
                cityResouces[resStartIndex + CityResoureIndex.iron].amount = 20;

                resourceStart += CityResoureIndex.COUNT;
                //workStart += WorkTemplate.COUNT;

                //int exEnd = startIndex + WorkTemplate.COUNT;

                //for (int i = startIndex; i < exEnd; i++)
                //{
                //    cityWork[i] = new WorkPriority(0);
                //}
                WorkTemplate.InitComponents(cityWork, startIndex);
                //cityWork[startIndex + (int)WorkPriorityType.move].value = 3;
                //cityWork[startIndex + (int)WorkPriorityType.wood].value = 2;
                //cityWork[startIndex + (int)WorkPriorityType.stone].value = 2;
                //cityWork[startIndex + (int)WorkPriorityType.craftFuel].value = 1;
                //cityWork[startIndex + (int)WorkPriorityType.farmFood].value = 4;
                //cityWork[startIndex + (int)WorkPriorityType.farmRawFood].value = 1;
                //cityWork[startIndex + (int)WorkPriorityType.craftBeer].value = 1;

                //cityWork[startIndex + (int)WorkPriorityType.smeltIron].value = 3;
                //cityWork[startIndex + (int)WorkPriorityType.craftSharpStick].value = 1;
                //cityWork[startIndex + (int)WorkPriorityType.craftPaddedArmor].value = 1;
                //cityWork[startIndex + (int)WorkPriorityType.farmfuel].value = 2;
                //cityWork[startIndex + (int)WorkPriorityType.farmlinen].value = 1;
                //cityWork[startIndex + (int)WorkPriorityType.bogiron].value = 1;
                //cityWork[startIndex + (int)WorkPriorityType.miningIron].value = 3;
                //cityWork[startIndex + (int)WorkPriorityType.trading].value = 2;
                //cityWork[startIndex + (int)WorkPriorityType.autoBuild].value = 1;
                //cityWork[startIndex + (int)WorkPriorityType.buildOrders].value = 2;
                //cityWork[startIndex + (int)WorkPriorityType.smeltGold].value = 3;

                startIndex += WorkTemplate.COUNT;
            }

            for (int i = 0; i < cityStorage.Length; i++)
            {
                cityStorage[i] = new StorageSize();
            }

        }

        public void writeComponents(System.IO.BinaryWriter w)
        {
            for (int i = 0; i < cityStorage.Length; i++)
            {
                cityStorage[i].write(w);
            }
        }
        public void readComponents(System.IO.BinaryReader r, int subVersion)
        {
            for (int i = 0; i < cityStorage.Length; i++)
            {
                cityStorage[i].read(r, subVersion);
            }
        }
    }
}
