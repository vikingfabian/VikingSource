using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map
{
    class ScreenSaveQuer : DataLib.SaveStreamWithQuedProcess
    {
        Chunk screen;
        
        public ScreenSaveQuer(bool save, Chunk screen, DataLib.ISaveTostorageCallback callBack, bool backup)
            : base(save, screen.Path, true)
        {
            if (screen.WriteProtected)
                throw new Exception("Saving protected screen: " + screen.ToString());
            this.screen = screen;
            this.callBack = callBack;
            start();
        }

        //protected override bool readWriteAction()
        //{
        //    base.readWriteAction();
        //    return true;
        //}
        public override void WriteStream(System.IO.BinaryWriter w)
        {
            System.Diagnostics.Debug.WriteLine("Saving screen:" + screen.ToString());
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
        protected override bool readWriteAction()
        {
            return base.readWriteAction();
        }
    }
}
