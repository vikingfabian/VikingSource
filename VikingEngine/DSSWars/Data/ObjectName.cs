using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data
{
    struct ObjectName
    {
        public string name;
        public bool custom;

        public void setCustom(string customName)
        {
            if (!string.IsNullOrEmpty( customName))
            { 
                name = customName;
                custom = true;
            }
        }

        public void setDefault(string defaultName)
        {
            if (!custom)
            {
                name = defaultName;
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            SaveLib.WriteString(w, custom ? name : null);
        }

        public void read(System.IO.BinaryReader r, int subversion)
        {
            if (subversion >= 48)
            {
                string readname = SaveLib.ReadString(r);
                if (readname != null)
                {
                    name = readname;
                    custom = true;
                }
            }
        }
    }
}
