using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.Data;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.Map.Generate
{
    class MapGenerator_BackgroundLoading: MapBackgroundLoading
    {
        public MapGenerator_BackgroundLoading()
            :base()
        { }

        public void generate(GenerateMapPass pass)
        {
            loadingState = LoadingState.StorageDone;
            generateLoopUntilSuccess(null, pass);
        }

        protected override bool GenerateNewMap()
        {
            return true;
        }
    }


    class MapBackgroundLoading
    {
        WorldDataStorage storage;
        protected LoadingState loadingState = 0;
        bool abort = false;
        GenerateMap dataGenerate = null;
        GenerateMap postGenerate;
        int failCount = 0;
        bool generateSuccess =false;
        CancellationTokenSource tokenSource;
        SaveStateMeta loadMeta;
        public MapGenerateSettings generateSettings = new MapGenerateSettings();

        public MapBackgroundLoading()
        { }
        public MapBackgroundLoading(SaveStateMeta loadMeta)
        {
            this.loadMeta = loadMeta;
            if (loadMeta != null)
            {
                DssRef.storage.generateNewMaps = loadMeta.worldmeta.IsGenerated;
                DssRef.storage.mapSize = loadMeta.worldmeta.mapSize;
            }

            if (DssRef.storage.generateNewMaps)
            {
                loadingState = LoadingState.StorageDone;
                generateLoopUntilSuccess(loadMeta, GenerateMapPass.All);
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
                storage.loadMap(worldMeta);
            }
        }

        protected void generateLoopUntilSuccess(SaveStateMeta loadMeta, GenerateMapPass generatePass)
        {
            generateSuccess = false;
            tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;

            Task task = Task.Factory.StartNew(() =>
            {
                while (!abort && failCount < 10)
                {
                    if (dataGenerate == null ||
                        generatePass == GenerateMapPass.All || 
                        generatePass == GenerateMapPass.Clear || 
                        generatePass == GenerateMapPass.AllTerrain)
                    {
                        dataGenerate = new GenerateMap();
                    }

                    WorldMetaData worldmeta;
                    ushort seed;
                    if (loadMeta != null)
                    {
                        worldmeta = loadMeta.worldmeta;
                    }
                    else
                    {
                        worldmeta = new WorldMetaData(Ref.rnd.Ushort(), DssRef.storage.mapSize, -1);
                        seed = Ref.rnd.Ushort();
                    }

                    bool success;
                    if (generatePass == GenerateMapPass.All)
                    {
                        success = dataGenerate.Generate(false, worldmeta, generateSettings);
                    }
                    else
                    {
                        success = dataGenerate.GeneratePass(worldmeta, generateSettings, generatePass);
                    }

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

            if (GenerateNewMap())
            {
                if (generateSuccess)
                {
                    if (DssRef.world.generatePassCompleted >= GenerateMapPass.Countries)
                    {
                        postGenerateUpdate();
                    }
                    else
                    {
                        loadingState = LoadingState.Complete;
                    }
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

        virtual protected bool GenerateNewMap()
        {
            return DssRef.storage.generateNewMaps;
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
            if (loadingState == LoadingState.Complete)
            {
                return string.Format(DssRef.lang.Progressbar_MapLoadingState, DssRef.lang.Progressbar_ProgressComplete);
            }
            else
            {
                if (DssRef.storage.generateNewMaps && !generateSuccess)
                {
                    return string.Format(DssRef.lang.Progressbar_MapLoadingState_GeneratingPercentage, GenerateMap.LoadStatus, failCount);
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
                if (!GenerateNewMap())
                {
                    DssRef.world = storage.worldData;
                }
                return true;
            }

            return false;
        }

         

        protected enum LoadingState
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
