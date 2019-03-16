using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    [TestClass]
    public class ZConvert {
        [TestMethod]
        public void TestConvert() {
            var times = 1000000;

            var sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < times; i++) {
                Assert.AreEqual(Convert("LEETCODEISHIRING", 3), "LCIRETOESIIGEDHN");
                Assert.AreEqual(Convert("LEETCODEISHIRING", 4), "LDREOEIIECIHNTSG");
            }

            sw.Stop();
            Trace.WriteLine($"Origin:{sw.ElapsedMilliseconds}");

            sw.Restart();
            for (int i = 0; i < times; i++) {
                Assert.AreEqual(Convert2("LEETCODEISHIRING", 3), "LCIRETOESIIGEDHN");
                Assert.AreEqual(Convert2("LEETCODEISHIRING", 4), "LDREOEIIECIHNTSG");
            }

            sw.Stop();
            Trace.WriteLine($"New:{sw.ElapsedMilliseconds}");
        }

  
        public string Convert(string s, int numRows) {
            if (numRows < 1) {
                throw new ArgumentOutOfRangeException(nameof(numRows));
            }

            if(numRows == 1) {
                return s;
            }
            
            if (string.IsNullOrEmpty(s)) {
                throw new ArgumentNullException(nameof(s));
            }
            
            var chArr = new char[s.Length];

            var interval = 2 * numRows - 2;

            var subLeft = interval;
            var subRight = 0;
            var chIndex = 0;

            for (int i = 0; i < numRows; i++) {
                var charIndexInOrigin = i;

                if (subLeft == 0 || subRight == 0) {
                    while (charIndexInOrigin < s.Length) {

                        chArr[chIndex++] = s[charIndexInOrigin];

                        charIndexInOrigin += interval;
                    }
                }
                else {
                    var times = (s.Length - charIndexInOrigin) / interval;
                    for (int j = 0; j < times; j++) {

                        chArr[chIndex++] = s[charIndexInOrigin];

                        charIndexInOrigin += subLeft;

                        chArr[chIndex++] = s[charIndexInOrigin];

                        charIndexInOrigin += subRight;
                    }

                    if (charIndexInOrigin < s.Length) {
                        chArr[chIndex++] = s[charIndexInOrigin];

                        charIndexInOrigin += subLeft;
                    }

                    if (charIndexInOrigin < s.Length) {
                        chArr[chIndex++] = s[charIndexInOrigin];

                        charIndexInOrigin += subRight;
                    }
                    
                }
                
                
                subLeft -= 2;
                subRight += 2;
            }

            return new string(chArr);
        }

        public string Convert2(string s, int numRows) {
            if (numRows == 1) return s;

            StringBuilder ret = new StringBuilder();
            int n = s.Length;
            int cycleLen = 2 * numRows - 2;

            for (int i = 0; i < numRows; i++) {
                for (int j = 0; j + i < n; j += cycleLen) {
                    ret.Append(s[j + i]);
                    if (i != 0 && i != numRows - 1 && j + cycleLen - i < n)
                        ret.Append(s[j + cycleLen - i]);
                }
            }
            return ret.ToString();
        }

        [TestMethod]
        public void TestTraverse() {
            var sw = new Stopwatch();
            var times = 100000;
            
            var nums = new int[10240];

            var sub1 = 1231;
            var sub2 = 2313;

            sw.Start();

            for (int i = 0; i < times; i++) {
                for (int j = 0; j < nums.Length; j++) {
                    var s = sub1 + sub2;
                }
            }
            
            sw.Stop();
            Trace.WriteLine($"For Length {sw.ElapsedMilliseconds}");

            sw.Restart();
            for (int i = 0; i < times; i++) {
                var length = nums.Length;
                for (int j = 0; j < length; j++) {
                    var s = sub1 + sub2;
                }
            }

            Trace.WriteLine($"For length {sw.ElapsedMilliseconds}");

            sw.Restart();
            for (int i = 0; i < times; i++) {
                foreach (var num in nums) {
                    var ss = num;
                    var s = sub1 + sub2;
                }
            }

            Trace.WriteLine($"Foreach {sw.ElapsedMilliseconds}");
        }


        struct IndexCell {
            public char Character { get; set; }

            public int Col { get; set; }

            public int Row { get; set; }
        }
    }
}
