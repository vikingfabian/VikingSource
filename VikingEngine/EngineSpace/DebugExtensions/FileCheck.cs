using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    struct FileCheck
    {
        bool hasStart;
        bool hasEnd;
        int readVersion;
        int buildVersion;
        public Exception exception;

        public void start(int readVersion, int buildVersion)
        {
            hasStart = true;
            this.readVersion = readVersion;
            this.buildVersion = buildVersion;
        }

        public void end()
        {
            hasEnd = true;
        }

        public override string ToString()
        {
            return $"{(hasStart ? 'T' : 'F')}-{(hasEnd ? 'T' : 'F')}: {readVersion}/{buildVersion}";
        }
    }

    struct ReadWriteCheck
    {
        public bool read;
        public bool write;
        public bool writeFail;

        public override string ToString()
        {
            return $"R{(read ? 'T' : 'F')}, W{(write ? 'T' : 'F')}, F{(writeFail ? 'T' : 'F')}";
        }
    }
}
