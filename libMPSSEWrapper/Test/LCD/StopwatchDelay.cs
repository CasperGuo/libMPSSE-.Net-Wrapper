using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.LCD
{
    public static class StopwatchDelay
    {
        public static void Delay(int ms)
        {
            var sw = new Stopwatch();

            sw.Start();

            while (sw.ElapsedMilliseconds < ms)
            {
                continue;
            }
        }

        public static void DelayMicroSeconds(int ms)
        {
            var sw = new Stopwatch();

            sw.Start();

            while (sw.ElapsedMicroseconds() < ms)
            {
                continue;
            }
        }

        public static double ElapsedMilliseconds(this Stopwatch stopwatch)
        {
            if (stopwatch == null)
                throw new ArgumentException("Stopwatch passed cannot be null!");

            return 1000 * stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;
        }

        public static double ElapsedMicroseconds(this Stopwatch stopwatch)
        {
            if (stopwatch == null)
                throw new ArgumentException("Stopwatch passed cannot be null!");

            return 1e6 * stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;
        }
    }
}
