using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        // CancellationToken cancellationToken;
        CancellationTokenSource tokenSource;
        public MapBackgroundLoading()
        {
            if (DssRef.storage.generateNewMaps)
            {
                loadingState = LoadingState.StorageDone;
                generateLoopUntilSuccess();
            }
            else
            {
                int loadingNumber = Ref.rnd.Int(MapFileGeneratorState.MapCountPerSize) + 1;

                storage = new WorldDataStorage();

                if (StartupSettings.SaveLoadSpecificMap.HasValue)
                {
                    DssRef.storage.mapSize = StartupSettings.SaveLoadSpecificMap.Value;
                    loadingNumber = 1;
                }
                storage.loadMap(DssRef.storage.mapSize, loadingNumber);
            }
        }

        void generateLoopUntilSuccess()
        {
            tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;

            Task task = Task.Factory.StartNew(() =>
            {
                while (!abort)
                {
                    dataGenerate = new GenerateMap();
                    bool success = dataGenerate.Generate(DssRef.storage.mapSize, 0, false);

                    if (success)
                    {
                        DssRef.world = dataGenerate.world;
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
                    postGenerate.postLoadGenerate_Part2(DssRef.world);
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
            string maploading = "Map loading: ";
            if (loadingState == LoadingState.Complete)
            {
                return maploading + "complete";
            }
            else
            {
                if (DssRef.storage.generateNewMaps && !generateSuccess)
                {
                    return "Generating: " + GenerateMap.LoadStatus.ToString() + "%. (Fails " + failCount.ToString()+ ")";
                }

                return maploading + "part " + ((int)loadingState).ToString() + "/" + ((int)LoadingState.Complete).ToString();
            }
        }

        public bool Complete()
        {
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

        public 

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
