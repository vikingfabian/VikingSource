using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataLib
{
   

    //abstract class AbsSaveWithQuedProcess : IQuedObject, IUpdateable
    //{
    //    protected bool save;
    //    protected bool failed = false;

    //    public AbsSaveWithQuedProcess(bool save)
    //    {
    //        this.save = save;
    //    }

    //    protected void start()
    //    {
    //        addToQue(!save);
    //    }

    //    bool saveQue;
    //    void addToQue(bool saveQue)
    //    {
    //        this.saveQue = saveQue;
    //        TaskExt.AddTask(StartQuedProcess, save);
    //        //if (saveQue)
    //        //{
    //        //    Engine.Storage.AddToSaveQue(StartQuedProcess, save);
    //        //}
    //        //else
    //        //{
    //        //    Ref.asynchUpdate.AddThreadQueObj(this);
    //        //}
    //    }

    //    public void runQuedTask(MultiThreadType threadType)
    //    {
    //        if (saveThread)
    //        {
    //            if (!readWriteAction())
    //            {
    //                //loading failed
    //                failed = true;

    //                Debug.LogError("Loading failed: " + this.ToString());
    //                new Timer.Action0ArgTrigger(IOFailedEvent);
    //                //IOFailedEvent_threaded();
    //                return;
    //            }
    //        }
    //        else
    //        {
    //            processAction();
    //        }

    //        if (saveQue != save)
    //        {
    //            addToQue(!saveQue);
    //        }
    //        else if (runSynchTrigger)
    //        {
    //            Ref.update.AddToOrRemoveFromUpdate(this, true);
    //        }
    //    }

    //    /// <returns>Read/Write without failure</returns>
    //    abstract protected bool readWriteAction();
    //    abstract protected void processAction();

    //    //TRIGGER
    //    protected bool runSynchTrigger = false;
    //    public UpdateType UpdateType { get { return VikingEngine.UpdateType.OneTimeTrigger; } }
    //    virtual public void Time_Update(float time) { }
    //    public void Time_LasyUpdate(float time) { }
    //    public bool SavingThread { get { return false; } }
    //    virtual protected void IOFailedEvent() { }

    //    public int SpottedArrayMemberIndex { get; set; }
    //    public bool SpottedArrayUseIndex { get { return true; } }
    //    public bool RunDuringPause { get { return true; } }
    //}

    
}
