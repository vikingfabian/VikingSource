using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;

namespace VikingEngine.LootFest.Process
{
    delegate ReturnVal AsynchMenuPageFunc<ReturnVal, Arg1>(Arg1 val1);

    class AsynchMenuPage<ReturnVal, Arg1> : StorageTask//AbsQuedTasks//QueAndSynch
    {
        AsynchMenuPageFunc<ReturnVal, Arg1> asynchProcess;
        Arg1 value1;
        ReturnVal returnVal;
        Action<ReturnVal, Arg1> createPage;
        Gui menu;
        int menuPageId;

        public AsynchMenuPage(AsynchMenuPageFunc<ReturnVal, Arg1> asynchProcess, Arg1 value1, Action<ReturnVal, Arg1> createPage, Gui menu)
            :base()//true, false)
        {
            this.asynchProcess = asynchProcess;
            this.value1 = value1;
            this.createPage = createPage;
            this.menu = menu;
            
            var layout = new GuiLayout("Loading...", menu);
            {
                new GuiTextButton("Cancel", null, menu.PopLayout, false, layout);
            } layout.End();

            this.menuPageId = menu.PageId;

            storagePriority = true;
            //start();
            beginStorageTask();
        }

        protected override void runQuedStorageTask()
        {
            base.runQuedStorageTask();
            returnVal = asynchProcess(value1);
            //return true;
        }
        //protected override void SynchedEvent()
        //{
        public override void onStorageComplete()
        {
            base.onStorageComplete();
            if (!menu.IsDeleted && menu.PageId == menuPageId)
            {
                menu.PopLayout();    
                createPage(returnVal, value1); 
            }
        }
    }
}
