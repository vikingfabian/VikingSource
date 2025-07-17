using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    static class Native
    {
        [DllImport("NativeCode.dll")]
        public static extern float add(float a, float b);
    }
}
