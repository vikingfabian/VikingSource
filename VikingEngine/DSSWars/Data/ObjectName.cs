
using VikingEngine.Engine;

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
            StreamLib.WriteString2(w, custom ? name : null);
        }

        public void read(System.IO.BinaryReader r, int subversion)
        {
            string readname;

            if (subversion < 139)
            {
                readname = StreamLib.ReadString(r);                
            }
            else
            {
                readname = StreamLib.ReadString2(r);
            }
            if (readname != null)
            {
                name = readname;
                custom = true;
            }
        }

        public string GetName()
        {
            if (custom)
            {
                return LoadContent.CheckCharsSafety(name, LoadedFont.Regular);
            }
            else
            {
                return name;
            }
        }
    }
}
