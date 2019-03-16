using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    [TestClass]
    public class FindGenerateParenthesis {
        [TestMethod]
        public void TestGenerateParenthesis() {
            var n = 8;
            var times = 1000;
            var ts0 = StopwatchHelper.Calculate(times, () => {
                var s = GenerateParenthesis(n);
            });
            Trace.WriteLine($"Old:{ts0.TotalMilliseconds}");
            var ts2 = StopwatchHelper.Calculate(times, () => {
                var s = GenerateParenthesis2(n);
            });
            Trace.WriteLine($"New:{ts2.TotalMilliseconds}");
        }

        [TestMethod]
        public void TestIsValid() {
            Assert.IsTrue(IsValid(new char[] { '(', ')' }));
            Assert.IsTrue(IsValid(new char[] { '(', '(', ')', ')' }));
            Assert.IsTrue(IsValid(new char[] { '(', ')', '(', ')' }));
        }

        public IList<string> GenerateParenthesis(int n) {
            var buffer = new char[n * 2];
            var res = new List<string>();
            var currentIndex = 1;
            var value = 1;

            buffer[0] = '(';
            buffer[buffer.Length - 1] = ')';
            HandleWithParenthesis(res, buffer, currentIndex, value);
            
            return res;
        }

        private static void HandleWithParenthesis(IList<string> res, char[] buffer, int currentIndex, int value) {
            if (currentIndex == buffer.Length - 1 && value == 1) {
                res.Add(new string(buffer));
                return;
            }

            if (value < 0) {
                return;
            }

            if (buffer.Length - currentIndex < value) {
                return;
            }

            buffer[currentIndex] = '(';
            HandleWithParenthesis(res, buffer, currentIndex + 1, value + 1);

            buffer[currentIndex] = ')';
            HandleWithParenthesis(res, buffer, currentIndex + 1, value - 1);
        }

        public IList<string> GenerateParenthesis2(int n) {
            var ans = new List<string>();
            if (n == 0) {
                ans.Add("");
            }
            else {
                for (int c = 0; c < n; ++c)
                    foreach (var left in GenerateParenthesis2(c)) {
                        foreach (var right in GenerateParenthesis2(n - 1 - c))
                            ans.Add("(" + left + ")" + right);
                    }
                   
                        
            }
            return ans;
        }

        

   
        public bool IsValid(IList<char> s) {
            var maxLeftCount = s.Count / 2;
            var leftCount = 0;

            for (int i = 0; i < s.Count; i++) {
                if(s[i] == '(') {
                    if (leftCount == maxLeftCount) {
                        return false;
                    }
                    else {
                        leftCount++;
                    }
                }
                else if(s[i] == ')') {
                    if(leftCount == 0) {
                        return false;
                    }

                    leftCount--;
                }
                else{
                    return false;
                }
            }

            return leftCount == 0;
        }
    }
}
