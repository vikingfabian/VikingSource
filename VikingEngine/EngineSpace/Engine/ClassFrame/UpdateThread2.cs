using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;

namespace VikingEngine
{
    delegate bool AsynchUpdateAction(int id, float time);

    class AsynchUpdateable : AbsUpdateable
    {
        protected AsynchUpdateAction updateAction;

        //System.Threading.Thread thread;
        Task task;

        protected float time = 0, asynchTime = 0;
        protected string name;
        protected int id;

        int startDelay;
        float updateDelays;

        public bool end = false;

        public AsynchUpdateable(AsynchUpdateAction updateAction, string name, int id = 0,
            int startDelay = 0, float updateDelays = 0, bool addToUpdate = true)
            : base(addToUpdate)
        {
            this.name = name;
            this.id = id;
            this.updateAction = updateAction;

            this.startDelay = startDelay;
            this.updateDelays = updateDelays;

            if (addToUpdate)
            {
                startNewUpdate();
            }
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

//#if XBOX
            if (time >= updateDelays)
            {
                if (task == null || task.IsCompleted)
                {
                    startNewUpdate();
                }
            }
//#endif
        }

        void startNewUpdate()
        {
            if (--startDelay < 0)
            {
                asynchTime = time;
                time = 0;

                //try
                //{
                task = Task.Factory.StartNew(asynchAction);
                //}
                //catch (Exception e)
                //{
                //    end = true;
                //    new DebugExtensions.TheadedCrash(e);
                //}
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
        }

        public override string ToString()
        {
            return "Asynch update (" + name + ")";
        }
        
    }

    class AsynchUpdateable_TryCatch : AsynchUpdateable
    {
        public AsynchUpdateable_TryCatch(AsynchUpdateAction updateAction, string name, int id = 0)
            : base(updateAction, name, id, 0, 0, true)
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
