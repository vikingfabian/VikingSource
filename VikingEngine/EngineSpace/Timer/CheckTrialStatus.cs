//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;

//namespace VikingEngine.Timer
//{
//    /// <summary>
//    /// Will run a IsTrial check every fifth second
//    /// </summary>
//    class CheckTrialStatus : LazyUpdate
//    {

//        float currentTime = 0;
//        const float CheckRate = 5000;

//        public CheckTrialStatus()
//           : base(true)
//        {
            
//        }
//        public override void Time_Update(float time)
//        {
//            currentTime+= time;
//            if (currentTime > CheckRate)
//            {
//                currentTime = 0;
//                if (Engine.XGuide.IsTrial)
//                {
                    
//                    Thread t = new Thread(Engine.XGuide.UpdateTrialCheck);
//                    t.Name = "Update Trial Check";
//                    t.Start();
//                }
//                else
//                {
//                    //Engine.XGuide.IsTrial = false;
//                    DeleteMe();
//                }
//            }
//        }
//    }
//}
