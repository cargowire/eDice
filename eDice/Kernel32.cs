using System.Runtime.InteropServices;

namespace eDice
{
    /// <summary>
    /// The pinvoke mappings for Kernel32 methods
    /// </summary>
    public static class Kernel32
    {
        /// <summary>
        /// Gets the id for the current thread
        /// </summary>
        /// <returns>The id</returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }
}
