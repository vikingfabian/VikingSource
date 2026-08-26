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
        public EcsStaticIndexArray neighborCities;
        public GroupedResource[] cityResouces;
        public WorkPriority[] cityWork;
        public StorageSize[] cityStorage;

        public TechTreeNodeProgress_City[] city_techNodes;

        const int WorkerXpCOUNT = (int)WorkExperienceType.NUM_NONE;
        int nextXpIndex = 0;
        bool[] WorkXpInUse;
        public StructList<WorkExperience> workerXp;
        
        public void InitCity(City city)
        {
            city.resourceComponentStartIndex = CityResourceIndex.COUNT * city.myIndex;
           
        }

        public TechTreeNodeProgress_City GetTech(int city, TechNodeType nodeType)
        {
            return city_techNodes[city * TechTreeNodeProgress_City.NodeCount + (int)nodeType];
        }
        public void SetTech(int city, TechNodeType nodeType, TechTreeNodeProgress_City value)
        { 
            city_techNodes[city * TechTreeNodeProgress_City.NodeCount + (int)nodeType] = value;
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
            while (nextXpIndex >= WorkXpInUse.Length || WorkXpInUse[nextXpIndex])
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
            
            return nextXpIndex++;
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
            int ex_end = city.resourceComponentStartIndex + CityResourceIndex.COUNT;
            for (int i = city.resourceComponentStartIndex; i < ex_end; i++)
            {
                cityResouces[i].amount = 0;
            }
        }

        public void setCityStockPile(City city, int limit)
        {
            int ex_end = city.resourceComponentStartIndex + CityResourceIndex.COUNT;
            for (int i = city.resourceComponentStartIndex; i < ex_end; i++)
            {
                cityResouces[i].setLimit(limit);
            }
        }

        public const int DefaultBuffer_Wood = 300;
        public const int DefaultBuffer_SkinLinnen = 300;

        public void Init_CityComponents(int cityCount)
        {
            initWorkerXp(cityCount);

            cityResouces = new GroupedResource[CityResourceIndex.COUNT * cityCount];
            neighborCities = new EcsStaticIndexArray(18, cityCount);
            cityWork = new WorkPriority[WorkTemplate.COUNT * cityCount];
            cityStorage = new StorageSize[StorageSize.COUNT * cityCount];
            city_techNodes = new TechTreeNodeProgress_City[TechTreeNodeProgress_City.NodeCount * cityCount];

            int resourceStart = 0;
            //int workStart = 0;

            int startWood, startLinnen, startFood;
            if (DssRef.storage.ruleset.factionStartSize == FactionStartSize.Settler)
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
                for (int resourceIx = 0; resourceIx < CityResourceIndex.COUNT; ++resourceIx)
                {
                    cityResouces[resourceStart + resourceIx] = new GroupedResource();
                }

                cityResouces[resStartIndex + CityResourceIndex.wood].amount = startWood;
                cityResouces[resStartIndex + CityResourceIndex.fuel].amount = 100;
                cityResouces[resStartIndex + CityResourceIndex.stone].amount = 20;
                cityResouces[resStartIndex + CityResourceIndex.food].amount = startFood;
                cityResouces[resStartIndex + CityResourceIndex.skinLinnen].amount = startLinnen;                
                cityResouces[resStartIndex + CityResourceIndex.iron].amount = 20;

                resourceStart += CityResourceIndex.COUNT;

                WorkTemplate.InitComponents(cityWork, startIndex);

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
