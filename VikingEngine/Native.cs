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
        private const string Dll = "NativeCode.dll";

        [DllImport(Dll)] public static extern float add(float a, float b);
        [DllImport(Dll)] public static extern float heavyloop();
    }
}
