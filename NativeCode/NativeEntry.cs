using System.Runtime.InteropServices;

namespace NativeCode
{
    public static class NativeEntry
    {
        [UnmanagedCallersOnly(EntryPoint = "add")]
        public static float Add(float a, float b) => a + b;
    }
}
