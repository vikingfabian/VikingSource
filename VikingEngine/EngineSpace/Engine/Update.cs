using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.Engine
{
    delegate void TimeUpdate(float time);

    class Update
    {
        const Keys DebugNormalSpeed = Keys.D1;
        const Keys DebugSlowSpeed = Keys.D2;
        const Keys DebugStepSpeed = Keys.D3;

        const float DebugSlowFrameTime = 1000;


        public LasyUpdatePart LasyUpdatePart = LasyUpdatePart.Part1;
        public float LazyUpdateTime = 0;
        float time_16msCountDown = 0;
        float gametime_16msCountDown = 0;
        public float TotalGameTime = 0;
        public bool exitApplication = false;
        
        SpottedArray<IUpdateable>[] updateLists;
        SpottedArray<IUpdateable> oneTimeTriggers;
        List<ISyncAction> syncQue = new List<ISyncAction>();
        
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
            name = "Update for " + parentState.ToString();
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

        public bool MainUpdate(GameTime gameTime)
        {
            CalcDeltaTime(gameTime);
#if PCGAME
            Ref.steam.Update();
#endif
#if XBOX
            Ref.xbox.update();
#endif
            Time_Update(Ref.DeltaTimeMs);
            TaskExt.Update();//Ref.asynchUpdate.update();


            VikingEngine.Input.InputLib.Update();
            Sound.Update();
            if (Ref.gamestate.UpdateCount == 0)
            {
                Ref.gamestate.FirstUpdate();
            }
            ++Ref.gamestate.UpdateCount;
            ++Ref.TotalFrameCount;
            Ref.gamestate.Time_Update(Ref.DeltaTimeMs);

            if (LasyUpdatePart == Engine.LasyUpdatePart.Part8_LasyUpdateList)
            {
                Time_UpdateLasyList();
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

        void Time_Update(float time)
        {
            lazyUpdateAccumulatedTime_next += time;
            TotalGameTime += time;

            {//Calc Ref.TimePassed16ms
                time_16msCountDown += time;
                if (time_16msCountDown >= Time16ms)
                {
                    time_16msCountDown -= Time16ms;
                    Ref.TimePassed16ms = true;
                }
                else
                {
                    Ref.TimePassed16ms = false;
                }
            }

            {//Calc Ref.GameTimePassed16ms
                Ref.GameTimePassed16ms = 0;

                gametime_16msCountDown += Ref.DeltaGameTimeMs;

                while(gametime_16msCountDown >= Time16ms)
                {
                    gametime_16msCountDown -= Time16ms;
                    ++Ref.GameTimePassed16ms;
                }
            }

            LasyUpdatePart++;
            if (LasyUpdatePart >= LasyUpdatePart.NUM)
            {
                LasyUpdatePart = LasyUpdatePart.Part1;
                LazyUpdateTime = lazyUpdateAccumulatedTime_next;
                lazyUpdateAccumulatedTime_next = 0;
            }

            XGuide.Update();
            if (Ref.netSession != null)
                Ref.netSession.Time_Update(time);
            ParticleHandler.Update(time);


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

            lock (syncQue)
            {
                for (int i = 0; i < syncQue.Count;++i)
                {
                    syncQue[i].execute();
                }
                syncQue.Clear();
            }
        }

        public void AddSyncAction(ISyncAction syncAction)
        {
            lock (syncQue)
            {
                syncQue.Add(syncAction);
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
                OneSecondCounter += Ref.DeltaTimeSec;

                if (OneSecondCounter >= 1)
                {
                    OneSecondCounter = 0;
                    StateHandler.OneSecUpdate();
                }
            }

        }

        public void ResetGameTime()
        {
            Ref.PrevTotalGameTimeSec = 0;
            Ref.TotalGameTimeSec = 0;
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
            Ref.main.TargetElapsedTime = new TimeSpan((long)(TimeSpan.TicksPerMillisecond * (1000.0 / (double)fps)));
            Ref.UpdateTimes30FPS = fps / 30;
            Ref.TargetDeltaTimeMs = (float)Ref.main.TargetElapsedTime.TotalMilliseconds;

        }

        public static int MillisecToFrames(float ms)
        {
            return Convert.ToInt32(ms / Ref.TargetDeltaTimeMs);
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
