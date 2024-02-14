//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.LootFest.Map
//{
//    class ReloadScreenDelay : Timer.AbsTimer
//    {
//        IntVector2 index;
//        public ReloadScreenDelay(IntVector2 index)
//            : base(2000, UpdateType.Lazy)
//        {
//            this.index = index;
//        }
//        protected override void timeTrigger()
//        {
//            //if (LfRef.chunks.GetScreen(index).Openstatus == ScreenOpenStatus.DotMapDone)
//            World.ReloadChunkMesh(index);
//            DeleteMe();
//        }
//    }
//}
