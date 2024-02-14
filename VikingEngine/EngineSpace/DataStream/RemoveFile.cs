
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DataLib;

namespace VikingEngine.DataStream
{
    delegate void FileRemovedCallback(bool succeded);

    class RemoveFile : StorageTask
    {
        FileRemovedCallback callback;
        //FilePath file;

        public RemoveFile(FilePath file, FileRemovedCallback callback, bool highPriority)
            : base()//true, !highPriority)
        {
            this.filePath = file;
            storagePriority = highPriority;
            beginStorageTask();
            //beginAutoTasksRun();
            //this.start();
        }

        protected override void runQuedStorageTask()
        {
            base.runQuedStorageTask();
 	        List<string> oldFiles = null;
            if (filePath.UseTimeMark)
                oldFiles = DataStreamHandler.GetTimeMarkedStoragePaths(filePath);

            foreach (string path in oldFiles)
                SaveLoad.RemoveFile2(path);

            //return callback != null;
        }

        public override void onStorageComplete()
        {
            base.onStorageComplete();
            callback?.Invoke(true);
        }
    }
}
