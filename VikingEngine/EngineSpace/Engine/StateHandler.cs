using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Engine
{
    static class StateHandler
    {
        //public static Engine.GameState CurrentState;
        static double renderTimePeak = 0;
        static double updateTimePeak = 0;
        static int frameCount = 0;

        /// <summary>
        /// Add one gamestate on top of the current
        /// </summary>
        public static void PushGamestate(GameState newstate)
        {
            Ref.gamestate.StoreView(true);
            Ref.gamestate.LostFocus();
            newstate.GotFocus(Ref.gamestate);
        }

        /// <summary>
        /// Bring up the gamestate below the current
        /// </summary>
        public static void PopGamestate()
        {
            if (Ref.gamestate.previousGameState == null)
            {
                throw new NullReferenceException("Cant pop gamestate from " + Ref.gamestate.ToString());
            }

            Ref.gamestate.OnDestroy();
            Ref.gamestate.previousGameState.GotFocus(null);
            Ref.gamestate.StoreView(false);
        }

        /// <summary>
        /// Clear out the current gamestate
        /// </summary>
        public static void ReplaceGamestate(GameState newstate)
        {
            if (Ref.gamestate != null)
            {
                Ref.gamestate.OnDestroy();
                Ref.update.AbortThreads();
            }
            newstate.GotFocus(null);
            Ref.ResetGameTime();

            //new Timer.CheckTrialStatus();
            Debug.OnNewGameState();
            
        }

        public static void RenderTimePass(double time)
        {
            if (time > renderTimePeak) { renderTimePeak = time; }
        }
        public static void UpdateTimePass(double time)
        {
            if (time > updateTimePeak) { updateTimePeak = time; }
        }
        public static void RenderLoop()
        { 
            frameCount++;
        }
        public static void OneSecUpdate()
        {
            if (PlatformSettings.DebugPerformanceText)
            {
                int gccount = Debug.GarbageCollectionCount();

                Engine.Draw.DebugUpdateTimeText = Convert.ToString(frameCount) +
                    "(r" + Convert.ToString(renderTimePeak) +
                    ", u" + Convert.ToString(updateTimePeak) + 
                    "), GC colls:" + gccount.ToString() + ", GC alo:" + TextLib.FileSizeText(GC.GetTotalMemory(false));
            }
            frameCount = 0;
            updateTimePeak = 0;
            renderTimePeak = 0;
        }

        
    }
    enum Language
    {
        English,
        German,
        French,
    }
}
