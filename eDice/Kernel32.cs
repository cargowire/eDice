using System.Runtime.InteropServices;

namespace eDice
{
    /// <summary>
    /// pinvoke mappings for Kernel32 methods
    /// </summary>
    public static class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }
}
