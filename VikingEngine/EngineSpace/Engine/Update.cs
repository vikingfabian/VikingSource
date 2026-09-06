using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Concurrent;
using VikingEngine.Input;

namespace VikingEngine.Engine
{
    delegate void TimeUpdate(float time);

    class Update
    {
        //const Keys DebugNormalSpeed = Keys.D1;
        //const Keys DebugSlowSpeed = Keys.D2;
        //const Keys DebugStepSpeed = Keys.D3;

        //const float DebugSlowFrameTime = 1000;


        public LasyUpdatePart LasyUpdatePart = LasyUpdatePart.Part1;
        public float LazyUpdateTime = 0;
        //float time_16msCountDown = 0;
        float gametime_16msCountDown = 0;
        float time_16msCountDown = 0;
        public float TotalGameTime = 0;
        public bool exitApplication = false;

        private float _lastUpdateListMs = 0f;
        private float _lastSyncQueMs = 0f;

        // TODO: Tie this to framerate.
        // TODO: Should we allow disabling budget?
        public static readonly double IdealSyncActionBudgetMs = 2.0;
        public static readonly double IncreasedSyncActionBudgetMs = 6.0;
        public static readonly double MaxSyncActionBudgetMs = 12.0d;
        public TextInput textInput = null;
        //public bool blockGameInput = false;
        //public string blockGameInputId = null;

        SpottedArray<IUpdateable>[] updateLists;
        SpottedArray<IUpdateable> oneTimeTriggers;

        // No lock necessary. Inherently thread sage.
        ConcurrentQueue<ISyncAction> _syncQue = new();
        public int SyncQueCount => _syncQue.Count;
        
        public int GetUpdateListCount(UpdateType updateType)
        {
            return updateLists[(int)updateType].Count;
        }

        public static bool IsRunningSlow = false;
        public static int SlowDownMarker = 0;

        public ISpottedArrayCounter<IUpdateable> updateCounter;
        ISpottedArrayCounter<IUpdateable> lazyUpdateCounter;
        SpottedArrayCounter<IUpdateable> oneTimeTriggersCounter;

        string name;

        public Update(GameState parentState)
        {
            name = "Update for " + (parentState != null ? parentState.ToString() : "TestState");
            updateLists = new SpottedArray<IUpdateable>[(int)UpdateType.NUM];
            for (int i = 0; i < (int)UpdateType.NUM; i++)
            {
                updateLists[i] = new SpottedArray<IUpdateable>();// new SpottedArray<IUpdateable>();
            }
            updateCounter = new SpottedArrayCounter<IUpdateable>(updateLists[(int)UpdateType.Full]);
            lazyUpdateCounter = new SpottedArrayCounter<IUpdateable>(updateLists[(int)UpdateType.Lazy]);
            oneTimeTriggers = new SpottedArray<IUpdateable>();
            oneTimeTriggersCounter = new SpottedArrayCounter<IUpdateable>(oneTimeTriggers);

        }
        
        
        public void AddToOrRemoveFromUpdate(VikingEngine.IUpdateable obj, bool add)
        {
            if (obj.UpdateType == UpdateType.OneTimeTrigger)
            {
                oneTimeTriggers.Add(obj);
            }
            else
            {
                if (add)
                {
                    updateLists[(int)obj.UpdateType].Add(obj);
                }
                else
                {
                    updateLists[(int)obj.UpdateType].Remove(obj);
                }
            }
        }

        /// <summary>
        /// View the objects in update for debug purposes
        /// </summary>
        public void UpdateListToFile(HUD.Gui menu)
        {
            VikingEngine.HUD.GuiLayout layout = new HUD.GuiLayout("Update list", menu);
            {
                for (int type = 0; type < (int)UpdateType.NUM; type++)
                {
                    //new HUD.GuiLabel(((UpdateType)type).ToString(), layout);
                    ISpottedArrayCounter<VikingEngine.IUpdateable> counter = new SpottedArrayCounter<VikingEngine.IUpdateable>(updateLists[type]);
                    while (counter.Next())
                    {
                        new HUD.GuiLabel(counter.GetSelection.ToString(), false, layout.gui.style.textFormatDebug, layout);
                    }
                }
            }
            layout.End();
        }

        public string DumpUpdateListSummary(UpdateType updateType = UpdateType.Full)
        {
            var typeCounts = new Dictionary<string, int>();
            int total = 0;
            var counter = new SpottedArrayCounter<IUpdateable>(updateLists[(int)updateType]);
            while (counter.Next())
            {
                var item = counter.GetSelection;
                if (item != null)
                {
                    string typeName = item.GetType().Name;
                    if (!typeCounts.TryGetValue(typeName, out int currentCount))
                    {
                        currentCount = 0;
                    }
                    typeCounts[typeName] = currentCount + 1;
                    total++;
                }
            }

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"=== UpdateList ({updateType}) Dump - Total Items: {total} ===");
            var sorted = new List<KeyValuePair<string, int>>(typeCounts);
            sorted.Sort((a, b) => b.Value.CompareTo(a.Value));
            foreach (var kv in sorted)
            {
                sb.AppendLine($"{kv.Key}: {kv.Value}");
            }
            sb.AppendLine("==================================================");
            return sb.ToString();
        }

        public string DumpUpdateListToFile(UpdateType updateType = UpdateType.Full)
        {
            try
            {
                string text = DumpUpdateListSummary(updateType);
                string baseDir = VikingEngine.DataStream.FilePath.StorageDirectory();
                if (string.IsNullOrEmpty(baseDir))
                {
                    baseDir = System.IO.Directory.GetCurrentDirectory();
                }

                string dir = System.IO.Path.Combine(baseDir, "DebugDumps");
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }

                string fileName = $"UpdateList_{updateType}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
                string filePath = System.IO.Path.Combine(dir, fileName);
                System.IO.File.WriteAllText(filePath, text);
                VikingEngine.Debug.Log($"Update list dumped to: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                VikingEngine.Debug.LogWarning($"Failed to dump update list: {ex.Message}");
                return null;
            }
        }

        public bool MainUpdate(GameTime gameTime)
        {
            long tCalc = 0;
            if (PlatformSettings.DebugPerformanceText)
            {
                tCalc = Stopwatch.GetTimestamp();
            }

            CalcDeltaTime(gameTime);

            float calcDeltaMs = 0f;
            if (PlatformSettings.DebugPerformanceText)
            {
                calcDeltaMs = (float)Stopwatch.GetElapsedTime(tCalc).TotalMilliseconds;
            }

            long tPreInput = 0;
            if (PlatformSettings.DebugPerformanceText)
            {
                tPreInput = Stopwatch.GetTimestamp();
            }

#if PCGAME
            Ref.steam?.Update();
#endif
#if XBOX
            Ref.xbox.update();
#endif

            float preInputMs = 0f;
            if (PlatformSettings.DebugPerformanceText)
            {
                preInputMs = (float)Stopwatch.GetElapsedTime(tPreInput).TotalMilliseconds;
            }

            Time_Update(Ref.DeltaTimeMs);

            long tPostInput = 0;
            if (PlatformSettings.DebugPerformanceText)
            {
                tPostInput = Stopwatch.GetTimestamp();
            }

            TaskExt.Update();//Ref.asynchUpdate.update();

            VikingEngine.Input.InputLib.Update();
            Sound.Update();

            float inputSoundMs = preInputMs;
            if (PlatformSettings.DebugPerformanceText)
            {
                inputSoundMs += (float)Stopwatch.GetElapsedTime(tPostInput).TotalMilliseconds;
            }

            if (Ref.gamestate.UpdateCount == 0)
            {
                Ref.gamestate.FirstUpdate();
            }
            ++Ref.gamestate.UpdateCount;
            ++Ref.TotalFrameCount;

            long tState = 0;
            if (PlatformSettings.DebugPerformanceText)
            {
                tState = Stopwatch.GetTimestamp();
            }

            Ref.gamestate.Time_Update(Ref.DeltaTimeMs);

            float gameStateMs = 0f;
            if (PlatformSettings.DebugPerformanceText)
            {
                gameStateMs = (float)Stopwatch.GetElapsedTime(tState).TotalMilliseconds;
            }

            long tLazy = 0;
            if (PlatformSettings.DebugPerformanceText)
            {
                tLazy = Stopwatch.GetTimestamp();
            }

            if (LasyUpdatePart == Engine.LasyUpdatePart.Part8_LasyUpdateList)
            {
                Time_UpdateLasyList();
            }

            float lazyUpdateMs = 0f;
            if (PlatformSettings.DebugPerformanceText)
            {
                lazyUpdateMs = (float)Stopwatch.GetElapsedTime(tLazy).TotalMilliseconds;
            }

            if (PlatformSettings.DebugPerformanceText)
            {
                DebugExtensions.RenderOverlay.Instance.RecordEngineSubsystems(
                    calcDeltaMs,
                    _lastUpdateListMs,
                    _lastSyncQueMs,
                    gameStateMs,
                    inputSoundMs,
                    lazyUpdateMs
                );
            }

            if (PlatformSettings.ViewSlowDown)
            {
                if (Ref.DeltaTimeMs < Ref.TargetDeltaTimeMs)
                {
                    SlowDownMarker = MillisecToFrames(500);
                }
            }
            return exitApplication;
        }

        float lazyUpdateAccumulatedTime_next = 0;
        public const float Time16ms = 1000f / 30f;
        public const float Time16msInSeconds = 1f / 30f;
        public const float Time60Fps = 1000f / 60f;

        internal void Time_Update(float time)
        {
            lazyUpdateAccumulatedTime_next += time;
            TotalGameTime += time;

            //{//Calc Ref.TimePassed16ms
            //    time_16msCountDown += time;
            //    if (time_16msCountDown >= Time16ms)
            //    {
            //        time_16msCountDown -= Time16ms;
            //        Ref.TimePassed16ms = true;
            //    }
            //    else
            //    {
            //        Ref.TimePassed16ms = false;
            //    }
            //}

            {//Calc Ref.GameTimePassed16ms
                Ref.GameTimePassed16ms = 0;

                gametime_16msCountDown += Ref.DeltaGameTimeMs;

                while(gametime_16msCountDown >= Time16ms)
                {
                    gametime_16msCountDown -= Time16ms;
                    ++Ref.GameTimePassed16ms;
                }
            }
            {//Calc Ref.GameTimePassed16ms
                Ref.GameTimePassed16ms = 0;

                time_16msCountDown += Ref.DeltaTimeMs;

                while (time_16msCountDown >= Time16ms)
                {
                    time_16msCountDown -= Time16ms;
                    ++Ref.TimePassed16ms;
                }
            }

            LasyUpdatePart++;
            if (LasyUpdatePart >= LasyUpdatePart.NUM)
            {
                LasyUpdatePart = LasyUpdatePart.Part1;
                LazyUpdateTime = lazyUpdateAccumulatedTime_next;
                lazyUpdateAccumulatedTime_next = 0;
            }

            //XGuide.Update();
            if (Ref.netSession != null)
            {
                Ref.netSession.Time_Update(time);
            }
            ParticleHandler.Update(time);


            long tUpdList = 0;
            if (PlatformSettings.DebugPerformanceText)
            {
                tUpdList = Stopwatch.GetTimestamp();
            }

            IUpdateable updateMember;
            updateCounter.Reset();
            while (updateCounter.Next())
            {
                updateMember = updateCounter.GetSelection;
                if (!Ref.isPaused || updateMember.RunDuringPause)
                {
                    updateMember.Time_Update(time);
                }
            }

            if (oneTimeTriggers.Count > 0)
            {
                oneTimeTriggersCounter.Reset();
                while (oneTimeTriggersCounter.Next())
                {
                    oneTimeTriggersCounter.sel.Time_Update(time);
                    oneTimeTriggersCounter.RemoveAtCurrent();
                }
            }

            if (PlatformSettings.DebugPerformanceText)
            {
                _lastUpdateListMs = (float)Stopwatch.GetElapsedTime(tUpdList).TotalMilliseconds;
            }

            //var budget = IdealSyncActionBudgetMs;
            var queueSize = _syncQue.Count;
            var budget = queueSize > 200
                ? MaxSyncActionBudgetMs
                : queueSize > 50
                    ? IncreasedSyncActionBudgetMs
                    : IdealSyncActionBudgetMs;

            // Thread-safe dequeue with dynamic time budget throttling.
            var syncStartTimestamp = Stopwatch.GetTimestamp();
            while (_syncQue.TryDequeue(out var syncAction))
            {
                syncAction.runSyncAction();
                if (Stopwatch.GetElapsedTime(syncStartTimestamp).TotalMilliseconds >= budget)
                {
                    break;
                }
            }

            if (PlatformSettings.DebugPerformanceText)
            {
                _lastSyncQueMs = (float)Stopwatch.GetElapsedTime(syncStartTimestamp).TotalMilliseconds;
            }
        }

        public void AddSyncAction(ISyncAction syncAction)
        {
            if (syncAction != null)
            {
                // Thread-safe enqueue.
                _syncQue.Enqueue(syncAction);
            }
        }

        public void AddSyncAction(Action action)
        {
            if (action != null)
            {
                AddSyncAction(new SyncAction(action));
            }
        }

        public void TriggerAllSteamWriters()
        {
            if (oneTimeTriggers.Count > 0)
            {
                oneTimeTriggersCounter.Reset();
                while (oneTimeTriggersCounter.Next())
                {
                    if (oneTimeTriggersCounter.sel is SteamWrapping.SteamWriter)
                    {
                        oneTimeTriggersCounter.sel.Time_Update(0);
                        oneTimeTriggersCounter.RemoveAtCurrent();
                    }
                }
            }
        }

        public void Time_UpdateLasyList()
        {
            lazyUpdateCounter.Reset();
            IUpdateable updateMember;
            while (lazyUpdateCounter.Next())
            {
                updateMember = lazyUpdateCounter.GetSelection;
                if (!Ref.isPaused || updateMember.RunDuringPause)
                {
                    updateMember.Time_Update(LazyUpdateTime);
                }
                
            }
        }

        
        float OneSecondCounter = 0;

        void CalcDeltaTime(GameTime gameTime)
        {
            Ref.DeltaTimeMs = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Ref.DeltaTimeSec = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Ref.PrevTotalTimeSec = Ref.TotalTimeSec;
            Ref.TotalTimeSec = (float)gameTime.TotalGameTime.TotalSeconds;

            Ref.PrevTotalGameTimeSec = Ref.TotalGameTimeSec;
            Ref.TotalGameTimeSec += (float)(gameTime.ElapsedGameTime.TotalSeconds * Ref.GameTimeSpeed);

            if (PlatformSettings.DebugPerformanceText)
            {
                DebugExtensions.MemoryOverlay.Instance.RecordFrame(Ref.DeltaTimeMs);
                OneSecondCounter += Ref.DeltaTimeSec;

                if (OneSecondCounter >= 1)
                {
                    OneSecondCounter = 0;
                    StateHandler.OneSecUpdate();
                }
            }

        }

        

        public void ExitToDash()
        {
            exitApplication = true;
        }

        public override string ToString()
        {
            return name;
        }

        public static void SetFrameRate(int fps)
        {
            var target = new TimeSpan((long)(TimeSpan.TicksPerMillisecond * (1000.0 / (double)fps)));
            var maxElapsed = TimeSpan.FromTicks(target.Ticks * 2);

            if (Ref.main != null)
            {
                // MonoGame enforces TargetElapsedTime <= MaxElapsedTime.
                // Order assignments to prevent ArgumentOutOfRangeException on any FPS setting:
                if (target > Ref.main.MaxElapsedTime)
                {
                    Ref.main.MaxElapsedTime = maxElapsed;
                    Ref.main.TargetElapsedTime = target;
                }
                else
                {
                    Ref.main.TargetElapsedTime = target;
                    Ref.main.MaxElapsedTime = maxElapsed;
                }

                Ref.TargetDeltaTimeMs = (float)Ref.main.TargetElapsedTime.TotalMilliseconds;
                Ref.TargetDeltaTimeSec = (float)Ref.main.TargetElapsedTime.TotalSeconds;
            }
            else
            {
                Ref.TargetDeltaTimeMs = (float)target.TotalMilliseconds;
                Ref.TargetDeltaTimeSec = (float)target.TotalSeconds;
            }

            Ref.UpdateTimes30FPS = fps / 30;
            Ref.UpdateTimes60FPS = fps / 60f;
        }

        public static int MillisecToFrames(float ms)
        {
            return Convert.ToInt32(ms / Ref.TargetDeltaTimeMs);
        }

        public int AbortThreads() 
        {
            int count = 0;
            var upateC = updateLists[(int)UpdateType.Full].counter();
            
            while (upateC.Next())
            {
                var updateable= upateC.sel as AbsUpdateable;
                if (updateable != null)
                {
                    if (updateable.AbortThreads())
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public bool HaveLiveThreads()
        {
            var upateC = updateLists[(int)UpdateType.Full].counter();

            while (upateC.Next())
            {
                var threadedUpdate = upateC.sel as AsynchUpdateable;
                if (threadedUpdate != null && threadedUpdate.Alive())
                    return true;
            }

            return false;
        }

        public void Exit()
        {
            exitApplication = true;
        }
    }

    //class UpdateThread
    //{
    //    AsynchUpdateAction updateAction;
    //    Thread thread;
    //    //bool run = true;
    //    float time;
    //    int id;
    //    public bool end = false;

    //    public UpdateThread(AsynchUpdateAction updateAction, string name, int id)
    //    {
    //        this.id = id;
    //        this.updateAction = updateAction;
    //        thread = new Thread(updateLoop);
    //        thread.Name = name;
    //        thread.Start();
    //    }

    //    //static float testCrash = 0;
    //    void updateLoop()
    //    {
    //        if (PlatformSettings.DevBuild)
    //        {
    //            try_updateLoop();
    //        }
    //        else
    //        {
    //            try
    //            {
    //                try_updateLoop();
    //            }
    //            catch (Exception e)
    //            {
    //                new DebugExtensions.TheadedCrash(e);
    //            }
    //        }
    //    }

    //    void try_updateLoop()
    //    {
    //        while (!end)
    //        {
    //            if (time <= 0)
    //            {
    //                Thread.Sleep(Ref.main.TargetElapsedTime);
    //            }

    //            float updateTime = time;
    //            time -= updateTime;

    //            end = updateAction(id, updateTime);

    //            if (Ref.update.EndGame)
    //            {
    //                Abort();
    //            }
    //        }
    //    }

    //    public bool MainUpdate(float time)
    //    {
    //        this.time += time;
    //        return end;
    //    }
    //    public void Abort()
    //    {
    //        end = true;
    //        thread.Abort();
    //    }

        
    //}

    //struct WatchData
    //{
    //    public string Name;
    //    public float Time;

    //    public WatchData(string name, float time)
    //    {
    //        Name = name;
    //        Time = time;
    //    }

    //    public override string ToString()
    //    {
    //        return Name + "[" + Time.ToString() + "] ";
    //    }
    //}
    enum LasyUpdatePart
    {
        Part1,
        Part2,
        Part3,
        Part4,
        Part5,
        Part6_Player,
        Part7_GameState,
        Part8_LasyUpdateList,
        NUM,
    }

    enum DebugTime
    {
        Normal,
        Slow,
        FrameByFrame,
    }
}
