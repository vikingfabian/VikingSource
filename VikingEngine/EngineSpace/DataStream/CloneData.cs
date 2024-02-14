using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataStream
{
    class CloneData
    {
        System.IO.MemoryStream s;
        public CloneData()
        {
           s = new System.IO.MemoryStream();
        }
        public System.IO.BinaryReader PasteData
        {
            get {
                s.Position = 0;
                return new System.IO.BinaryReader(s); }
        }
        public System.IO.BinaryWriter CopyData
        {
            get { 
                
                return new System.IO.BinaryWriter(s); 
            }
        }
        

    }
}
