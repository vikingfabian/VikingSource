using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;

namespace VikingEngine
{
    delegate bool AsynchUpdateAction(int id, float time);

    class AsynchUpdateable : AbsUpdateable
    {
        protected AsynchUpdateAction updateAction;

        AutoResetEvent resetEvent = new AutoResetEvent(false);
        System.Threading.Thread _thread;
       
        protected float time = 0, asynchTime = 0;
        protected string name;
        protected int id;

        bool busyThread = false;
        public bool end = false;

        public AsynchUpdateable(AsynchUpdateAction updateAction, string name, int id = 0,
            ThreadPriority priority = ThreadPriority.Normal, bool addToUpdate = true)
            : base(addToUpdate)
        {
            this.name = name;
            this.id = id;
            this.updateAction = updateAction;

            if (addToUpdate)
            {
                StartThread(priority);
                //startNewUpdate();
            }
        }

        bool End()
        {
            return end || Ref.update.exitApplication;
        }

        public void StartThread(ThreadPriority priority)
        {
            _thread = new Thread(() =>
            {
                while (!End())
                {                    
                    resetEvent.WaitOne(); // Blocks until the event is signaled
                    if (End())
                    {
                        return;
                    }
                    asynchTime = time;
                    time -= asynchTime;

                    busyThread = true;
                    {
                        asynchAction();
                    }
                    busyThread = false;
                }
            });
            
            _thread.Start();
            _thread.Priority = priority;
        }

        public override void Time_Update(float time_ms)
        {
            //if (end)
            //{
            //    DeleteMe();
            //    return;
            //}
            if (!_thread.IsAlive)
            {
                DeleteMe();
                return;
            }

            time += Ref.DeltaGameTimeMs;

            if (!busyThread)
            {
                resetEvent.Set(); // Signal the waiting thread
            }
        }
        virtual protected void asynchAction()
        {
            if (updateAction != null)
            {
                end = updateAction(id, asynchTime);
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            AbortThreads();
        }

        public override bool AbortThreads()
        {

            if (_thread.ThreadState == ThreadState.Running ||
                _thread.ThreadState == ThreadState.WaitSleepJoin)
            {
                end = true;
                resetEvent.Set();
                return true;
            }
            return false;
//#if DEBUG
//                thread?.Join();
//#endif
            

        }

        public bool Alive()
        { 
            return _thread != null && _thread.IsAlive;
        }

        public override string ToString()
        {
            return "Asynch update (" + name + ")";
        }
        
    }

    class AsynchUpdateable_TryCatch : AsynchUpdateable
    {
        public AsynchUpdateable_TryCatch(AsynchUpdateAction updateAction, string name, int id = 0, ThreadPriority priority = ThreadPriority.Normal)
            : base(updateAction, name, id, priority, true)
        { }

        override protected void asynchAction()
        {

#if !DEBUG
            try
            {
#endif
                if (updateAction != null)
                {
                    end = updateAction(id, asynchTime);
                }
#if !DEBUG
            }
            catch (Exception e)
            {
                if (!end)
                {
                    BlueScreen.ThreadException = e;
                }
            }
#endif

        }
    }
}
