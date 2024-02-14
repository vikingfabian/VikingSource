using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataLib
{
    //interface IMainThreadReaderObj
    //{
    //    void SafeMainThreadRead(System.IO.BinaryReader r);
    //}

    class SafeMainThreadReader : OneTimeTrigger
    {
        byte[] data;
        ReadBinaryStream obj;

        public SafeMainThreadReader(System.IO.BinaryReader r, ReadBinaryStream obj)
            :base(false)
        {
            data = r.ReadBytes((int)(r.BaseStream.Length - r.BaseStream.Position));
            this.obj = obj;
            AddToOrRemoveFromUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            if (data.Length > 0)
            {
                //System.IO.BinaryReader r = new System.IO.BinaryReader();
                //r.BaseStream.Write(data, 0, data.Length);
                //r.BaseStream.Position = 0;
                DataStream.MemoryStreamHandler stream = new DataStream.MemoryStreamHandler();
                stream.SetByteArray(data);
                obj(stream.GetReader());
            }
            else
            {
                Debug.LogError("SafeMainThreadReader, empty file");
            }
        }
    }
}
