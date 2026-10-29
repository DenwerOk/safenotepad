using System;
using System.Diagnostics;

namespace SafeNotePad.Utils
{
    public static class PerformanceHelper
    {
        private static Stopwatch _stopWatch;

        public static void Start()
        {
            _stopWatch = Stopwatch.StartNew();
        }

        public static void End(string id)
        {
            _stopWatch.Stop();
            Console.WriteLine("MS elapsed on method {0}: {1}", id, _stopWatch.ElapsedMilliseconds);
        }
    }
}
