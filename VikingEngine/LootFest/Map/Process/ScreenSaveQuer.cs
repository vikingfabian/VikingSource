using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Map
{
    
    class ScreenSaveQuer2 : DataLib.StorageTaskWithQuedProcess
    {
        Chunk screen;

        public ScreenSaveQuer2(bool save, Chunk screen, BlockMap.DesignAreaStorage area)
            : base(save, area.ChunkPath(screen.Index), true)
        {
            if (screen.WriteProtected)
                throw new Exception("Saving protected screen: " + screen.ToString());
            this.screen = screen;
            beginAutoTasksRun();//beginStorageTask();
        }

        public override void WriteStream(System.IO.BinaryWriter w)
        {
            Debug.Log("Saving screen:" + screen.ToString());
            screen.WriteChunk(w);
        }
        public override void ReadStream(System.IO.BinaryReader r)
        {
            screen.BeginReadChunk(r);
        }

        public override string ToString()
        {
            return "Saving screen:" + screen.ToString();
        }

        protected override void IOFailedEvent()
        {
            //ta bort från changed chunk data
            //ladda om, outdated chunk

            LfRef.gamestate.ChunkIsMissingFile(screen.Index);
        }
        //protected override bool readWriteAction()
        //{
        //    return base.readWriteAction();
        //}
    }
}
