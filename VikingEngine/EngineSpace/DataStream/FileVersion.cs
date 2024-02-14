using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DataStream
{
    struct FileVersion
    {
        const bool TriggerReleaseException = true;
        public static readonly FileVersion Max = new FileVersion(int.MaxValue, int.MaxValue);
        
        public int release;
        public int dev;
        public bool isSaveState;

        public static void Write(System.IO.BinaryWriter w, int release, int dev)
        {
            w.Write(release);
            w.Write(dev);
        }

        public FileVersion(int release, int dev)
        {
            this.release = release;
            this.dev = dev;
            isSaveState = false;
        }

        public FileVersion(System.IO.BinaryReader r)
        {
            release = r.ReadInt32();
            dev = r.ReadInt32();
            isSaveState = false;
        }

        public bool Old(int oldVersion)
        {
            return oldVersion < release;
        }

        public bool New(int newVersion)
        {
            return newVersion >= release;
        }

        public bool Old_dev(int oldVersion)
        {
            checkReleaseExc();
            return oldVersion < dev;
        }

        public bool New_dev(int newVersion)
        {
            checkReleaseExc();
            return newVersion >= dev;
        }

        void checkReleaseExc()
        {
            if (TriggerReleaseException && PlatformSettings.ReleaseBuild)
            {
                throw new Exception("Contains file dev version");
            }
        }

    }
}
