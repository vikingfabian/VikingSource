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

        ManualResetEvent resetEvent = new ManualResetEvent(false);
        System.Threading.Thread thread;
        //Task task;

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

            //this.startDelay = startDelay;
            //this.updateDelays = updateDelays;

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
            Thread thread = new Thread(() =>
            {
                while (!End())
                {                    
                    resetEvent.WaitOne(16); // Blocks until the event is signaled
                    if (End())
                    {
                        return;
                    }
                    asynchTime = time;
                    time -= asynchTime;

                    //if (asynchTime > 0)
                    //{
                    busyThread = true;
                    {
                        asynchAction();
                    }
                    busyThread = false;
                    //}
                }
            });

            thread.Start();
            thread.Priority = priority;
        }

        //void threadAction()
        //{
        //    while (!end)
        //    {
        //        asynchTime = time;
        //        time = 0;
        //        asynchAction();

        //        if (time == 0)
        //        {
        //            System.Threading.Thread.Sleep(16);
        //        }
        //    }
        //}

        public override void Time_Update(float time_ms)
        {
            if (end)
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

        //void startNewUpdate()
        //{
        //    if (--startDelay < 0)
        //    {
        //        asynchTime = time;
        //        time = 0;

        //        task = Task.Factory.StartNew(asynchAction);
                
        //    }
        //}

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

        public override void AbortThreads()
        {
            end = true;
            resetEvent.Set();
            thread?.Join();
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
#if DEBUG
            if (updateAction != null)
            {
                end = updateAction(id, asynchTime);
            }
#else
            try
            {
                if (updateAction != null)
                {
                    end = updateAction(id, asynchTime);
                }
            }
            catch (Exception e)
            {
                BlueScreen.ThreadException = e;
            }
#endif

        }
    }
}
