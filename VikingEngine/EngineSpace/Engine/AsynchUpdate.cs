using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VikingEngine.Engine
{
    //class AsynchUpdate
    //{
    //    const int NumThreadQues = 1;

    //    List<UpdateThread> updateThreads;
    //    //Task
    //    Thread[] processThreads;
    //    List<IQuedObject> threadedProcessQue = new List<IQuedObject>();

    //    public AsynchUpdate(GameState parentState)
    //    {
    //        ResetThreadQue(parentState);
    //    }

    //    public void AddUpdateThread(AsynchUpdateAction updateAction, string name, int id)
    //    {
    //        if (updateThreads == null)
    //        {
    //            updateThreads = new List<UpdateThread>();
    //        }
    //        updateThreads.Add(new UpdateThread(updateAction, name, id));
            
    //    }

    //    public void AddThreadQueObj(IQuedObject obj)
    //    {
    //        lock (threadedProcessQue)
    //        {
    //            threadedProcessQue.Add(obj);
    //        }
    //    }
    //    void threadQueLoop()
    //    {
    //        if (PlatformSettings.DevBuild)
    //        {
    //            try_ThreadQueLoop();
    //        }
    //        else
    //        {
    //            try
    //            {
    //                try_ThreadQueLoop();
    //            }
    //            catch (Exception e)
    //            {
    //                new DebugExtensions.TheadedCrash(e);
    //            }
    //        }
    //    }
    //    void try_ThreadQueLoop()
    //    {
    //        while (true)
    //        {
    //            IQuedObject obj = null;
    //            lock (threadedProcessQue)
    //            {
    //                if (threadedProcessQue.Count > 0)
    //                {
    //                    obj = threadedProcessQue[0];

    //                    threadedProcessQue.RemoveAt(0);
    //                }
    //            }
    //            if (obj == null)
    //            {
    //                Thread.Sleep(Ref.main.TargetElapsedTime);
    //            }
    //            else
    //            {
    //                obj.StartQuedProcess(false);
    //            }


    //            if (Ref.update != null && Ref.update.EndGame)
    //                System.Threading.Thread.CurrentThread.Abort();
    //        }
    //    }


    //    public void ResetThreadQue(GameState parentState)
    //    {
    //        AbortThreads();
    //        lock (threadedProcessQue)
    //        {
    //            processThreads = new Thread[NumThreadQues];
    //            for (int i = 0; i < NumThreadQues; i++)
    //            {
    //                processThreads[i] = new Thread(threadQueLoop);
    //                string name = parentState.ToString() + ", Update Que" + MainGame.NextThreadIx.ToString();
    //                processThreads[i].Name = name;
    //                processThreads[i].Start();

    //                Debug.LogThreadStart(processThreads[i]);
    //            }

    //            threadedProcessQue = new List<IQuedObject>();
    //        }
    //    }
    //    public void AbortThreads()
    //    {
    //        lock (threadedProcessQue)
    //        {
    //            if (processThreads != null)
    //            {
    //                for (int i = 0; i < processThreads.Length; i++)
    //                {
    //                    processThreads[i].Abort();
    //                }
    //            }
    //        }
    //        if (updateThreads != null)
    //        {
    //            foreach (UpdateThread t in updateThreads)
    //            {
    //                t.Abort();
    //            }
    //            updateThreads = null;
    //        }
    //    }


    //    public void update()
    //    {
    //        if (updateThreads != null)
    //        {
    //            //foreach (UpdateThread t in updateThreads)
    //            for (int i = updateThreads.Count-1; i>= 0; --i)
    //            {
    //                if (updateThreads[i].MainUpdate(Ref.DeltaTimeMs))
    //                {
    //                    updateThreads[i].Abort();
    //                    updateThreads.RemoveAt(i);
    //                }
    //            }
    //        }
    //    }

       
    //}
}
