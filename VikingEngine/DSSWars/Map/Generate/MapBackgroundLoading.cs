using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.Data;
using static VikingEngine.DSSWars.Map.Generate.MapBackgroundLoading;

namespace VikingEngine.DSSWars.Map.Generate
{
    class MapBackgroundLoading
    {
        WorldDataStorage storage;
        LoadingState loadingState = 0;
        bool abort = false;
        GenerateMap dataGenerate = null;
        GenerateMap postGenerate;
        int failCount = 0;
        bool generateSuccess =false;
        CancellationTokenSource tokenSource;
        SaveStateMeta loadMeta;

        public MapBackgroundLoading(SaveStateMeta loadMeta)
        {
            this.loadMeta = loadMeta;
            if (loadMeta != null)
            {
                

                DssRef.storage.generateNewMaps = loadMeta.worldmeta.IsGenerated;
                DssRef.storage.mapSize = loadMeta.worldmeta.mapSize;

                //if (loadMeta.metaVersion >= 3)
                //{

                //}
            }

            if (DssRef.storage.generateNewMaps)
            {
                loadingState = LoadingState.StorageDone;
                generateLoopUntilSuccess(loadMeta);
            }
            else
            {
                int loadingNumber = Ref.rnd.Int(MapFileGeneratorState.MapCountPerSize) + 1;

                WorldMetaData worldMeta;

                if (loadMeta == null)
                {
                    worldMeta = new WorldMetaData(0, DssRef.storage.mapSize, loadingNumber);
                }
                else
                {
                    worldMeta = loadMeta.worldmeta;
                    loadingNumber = loadMeta.worldmeta.saveIndex;
                }

                storage = new WorldDataStorage();

                if (StartupSettings.SaveLoadSpecificMap.HasValue)
                {
                    DssRef.storage.mapSize = StartupSettings.SaveLoadSpecificMap.Value;
                    loadingNumber = 1;
                }
                storage.loadMap(worldMeta);//DssRef.storage.mapSize, loadingNumber);
            }
        }

        void generateLoopUntilSuccess(SaveStateMeta loadMeta)
        {
            tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;

            Task task = Task.Factory.StartNew(() =>
            {
                while (!abort)
                {
                    dataGenerate = new GenerateMap();

                    WorldMetaData world;
                    ushort seed;
                    if (loadMeta != null)
                    {
                        world = loadMeta.worldmeta;
                        //seed = loadMeta.world.seed;
                    }
                    else
                    {
                        world = new WorldMetaData(Ref.rnd.Ushort(), DssRef.storage.mapSize, -1);
                        seed = Ref.rnd.Ushort();
                    }

                    bool success = dataGenerate.Generate(false, world);

                    if (success)
                    {
                        if (!abort)
                        {
                            DssRef.world = dataGenerate.world;
                        }
                        generateSuccess = true;
                        return;
                    }
                    else
                    {
                        failCount++;
                    }
                }
            }, cancellationToken);

            
        }

        public void Update()
        {
            if (abort)
            {
                return;
            }

            if (DssRef.storage.generateNewMaps)
            {
                if (generateSuccess)
                {
                    postGenerateUpdate();
                }                
            }
            else
            {
                if (storage.loadComplete)
                {
                    postGenerateUpdate();
                }
                else if (storage.LoadingStarted)
                {
                    loadingState = LoadingState.Storage;
                }
            }
        }

        void postGenerateUpdate()
        {
            try
            {
                if (loadingState <= LoadingState.StorageDone)
                {
                    loadingState = LoadingState.Post1Started;
                    postGenerate = new Map.Generate.GenerateMap();
                    postGenerate.postLoadGenerate_Part1(DssRef.world);
                }
                else if (loadingState == LoadingState.Post1Started)
                {
                    if (postGenerate.postComplete)
                    {
                        loadingState = LoadingState.Post2Started;
                        postGenerate = new Map.Generate.GenerateMap();
                        postGenerate.postLoadGenerate_Part2(DssRef.world, loadMeta);
                    }
                    //loadingState = LoadingState.Complete;
                }
                else if (loadingState == LoadingState.Post2Started)
                {
                    if (postGenerate.postComplete)
                    {
                        loadingState = LoadingState.Complete;
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        public void Abort()
        { 
            abort = true;
            if (storage != null)
            {
                storage.worldData.abortLoad = true;
            }
            tokenSource?.Cancel();
        }

        public string ProgressString()
        {
            //string maploading = "Map loading: ";
            if (loadingState == LoadingState.Complete)
            {
                return string.Format(DssRef.lang.Progressbar_MapLoadingState, DssRef.lang.Progressbar_ProgressComplete);
            }
            else
            {
                if (DssRef.storage.generateNewMaps && !generateSuccess)
                {
                    return string.Format(DssRef.lang.Progressbar_MapLoadingState_GeneratingPercentage, GenerateMap.LoadStatus, failCount);//"Generating: " + GenerateMap.LoadStatus.ToString() + "%. (Fails " + failCount.ToString()+ ")";
                }

                string part = string.Format(DssRef.lang.Progressbar_MapLoadingState_LoadPart, (int)loadingState, (int)LoadingState.Complete);
                return string.Format(DssRef.lang.Progressbar_MapLoadingState, part);
            }
        }

        public bool Complete()
        {
            if (abort)
            {
                return true;
            }

            if (loadMeta != null && loadMeta.metaVersion >= 3)
            { 
                return true;
            }

            if (loadingState == LoadingState.Complete)
            {
                if (!DssRef.storage.generateNewMaps)
                {
                    DssRef.world = storage.worldData;
                }
                return true;
            }

            return false;
        }

         

        enum LoadingState
        { 
            StorageQue,
            Storage,
            StorageDone,
            Post1Started,
            PostPart1Done,
            Post2Started,
            PostPart2Done,
            Complete,
        }
    }
}
