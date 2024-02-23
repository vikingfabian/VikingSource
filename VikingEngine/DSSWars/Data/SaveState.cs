using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DataStream;

namespace VikingEngine.DSSWars.Data
{
    class SaveGamestate : AbsUpdateable
    {
        MemoryStreamHandler memoryStream = new MemoryStreamHandler();

        public SaveGamestate() :
            base(false)
        { }

        public void save()
        {
            AddToUpdateList();

            var w = memoryStream.GetWriter();

            Task.Factory.StartNew(() =>
            {
                DssRef.storage.write(w);
            });
        }

        public override void Time_Update(float time_ms)
        {
            
        }

    }
}
