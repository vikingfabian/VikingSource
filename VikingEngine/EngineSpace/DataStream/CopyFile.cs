//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.DataStream
//{
    
//    class CopyFile : QueAndSynch
//    {
//        FilePath fromPath, toPath;
//        IStreamIOCallback callbackObj;
//        byte[] data;

//        public CopyFile(FilePath fromPath, FilePath toPath, IStreamIOCallback callbackObj)//, bool fromStorage)
//            :base(true, true)
//        {
//            this.callbackObj = callbackObj;
//            this.toPath = toPath;
//            this.fromPath = fromPath;
//            start();
//        }
//        protected override bool quedEvent()
//        {
//            data = Action(fromPath, toPath);
//            return true;
//        }

//        public static byte[] Action(FilePath fromPath, FilePath toPath)
//        {
//            byte[] data = DataStreamHandler.Read(fromPath);
//            if (data != null)
//                DataStreamHandler.Write(toPath, data);
//            return data;
//        }

//        protected override void SynchedEvent()//public override void Time_Update(float time)
//        {
//            if (callbackObj != null)
//            {
//                callbackObj.SaveComplete(true, -1, data != null, data);
//            }
//        }
//    }
//}
