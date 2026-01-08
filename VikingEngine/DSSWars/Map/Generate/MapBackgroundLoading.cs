using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using VikingEngine.DebugExtensions;
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
            generateLoopUntilSuccess(null, pass, true);
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
        public GenerateMap dataGenerate = null;
        GenerateMap postGenerate;
        int failCount = 0;
        bool generateSuccess =false;
        CancellationTokenSource tokenSource;
        public SaveStateMeta loadMeta;
        public MapGenerateSettings generateSettings = new MapGenerateSettings();

        public MapBackgroundLoading()
        { }
        public MapBackgroundLoading(MapGenerateSettings generateSettings)
        { 
            this.generateSettings = generateSettings;
            begin();
        }
        public MapBackgroundLoading(SaveStateMeta loadMeta)
        {
            this.loadMeta = loadMeta;
            begin();
        }

        void begin()
        {
            if (loadMeta != null)
            {
                DssRef.storage.generateNewMaps = loadMeta.worldmeta.IsGenerated;
                DssRef.storage.gameRuleset.mapSize = loadMeta.worldmeta.mapSize;
            }

            if (GenerateNewMap())
            {

                loadingState = LoadingState.StorageDone;
                generateLoopUntilSuccess(loadMeta, GenerateMapPass.All, false);
            }
            else
            {
                int loadingNumber = Ref.rnd.Int(MapFileGeneratorState.MapCountPerSize) + 1;

                WorldMetaData worldMeta;

                if (loadMeta == null)
                {
                    worldMeta = new WorldMetaData(0, DssRef.storage.gameRuleset.mapSize, loadingNumber);
                }
                else
                {
                    worldMeta = loadMeta.worldmeta;
                    loadingNumber = loadMeta.worldmeta.saveIndex;
                }

                storage = new WorldDataStorage();

                if (StartupSettings.SaveLoadSpecificMap.HasValue)
                {
                    DssRef.storage.gameRuleset.mapSize = StartupSettings.SaveLoadSpecificMap.Value;
                    loadingNumber = 1;
                }
                storage.loadMap(worldMeta);
            }
        }


        public WorldData WorldData()
        { 
            if (DssRef.world == null)
                return null;

            return DssRef.world;
        }

        protected void generateLoopUntilSuccess(SaveStateMeta loadMeta, GenerateMapPass generatePass, bool customEditorMap)
        {
            generateSuccess = false;
            tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;

            Task task = Task.Factory.StartNew(async () =>
            {
                try
                {
                    while (!abort && failCount < 10)
                    {
                        List<Task> extraTasks = new List<Task>();

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
                            worldmeta = new WorldMetaData(Ref.rnd.Ushort(), DssRef.storage.gameRuleset.mapSize, -1);
                            worldmeta.customEditorMap = customEditorMap;
                            seed = Ref.rnd.Ushort();
                        }

                        bool success;
                        if (generatePass == GenerateMapPass.All)
                        {
                            List<Task> tasks = new List<Task>();
                            success = dataGenerate.Generate(false, worldmeta, generateSettings, tasks);
                            await Task.WhenAll(tasks);
                        }
                        else
                        {
                            success = dataGenerate.GeneratePass(worldmeta, generateSettings, generatePass, extraTasks);
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

                        await Task.WhenAll(extraTasks);
                    }
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
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
                    if (dataGenerate.world.generatePassCompleted >= GenerateMapPass.Countries)
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
                    //TODO WHY NULL
                    if (dataGenerate != null)
                    {
                        loadingState = LoadingState.Post1Started;
                        postGenerate = new Map.Generate.GenerateMap();
                        postGenerate.postLoadGenerate_Part1(dataGenerate.world);
                    }
                }
                else if (loadingState == LoadingState.Post1Started)
                {
                    if (postGenerate.postComplete)
                    {
                        loadingState = LoadingState.Post2Started;
                        postGenerate = new Map.Generate.GenerateMap();
                        postGenerate.postLoadGenerate_Part2(dataGenerate.world, loadMeta);
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
            return DssRef.storage.generateNewMaps || generateSettings.useGenerate;
        }

        public void Abort()
        { 
            abort = true;
            if (storage != null)
            {
                storage.worldData.abortLoad = true;
            }
            tokenSource?.Cancel();

            System.Threading.Thread.Sleep(100);
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
                if (GenerateNewMap())
                {
                    if (!abort)
                    { DssRef.world = dataGenerate.world; }
                }
                else
                {
                    if (!abort)
                    { DssRef.world = storage.worldData; }
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
