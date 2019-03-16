using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    static class StopwatchHelper {
        public static TimeSpan Calculate(int count,Action action) {
            var sw = new Stopwatch();
            sw.Restart();
            for (int i = 0; i < count; i++) {
                action();
            }
            sw.Stop();
            return TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds);
        }
    }
}
