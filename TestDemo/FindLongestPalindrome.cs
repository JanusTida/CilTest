using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Diagnostics;

namespace TestDemo {
    /// <summary>
    /// Summary description for FindLongestPalindrome
    /// </summary>
    [TestClass]
    public class FindLongestPalindrome {
        [TestMethod]
        public void TestLongestPalindrome() {
            var times = 1000;

            var ts1 = StopwatchHelper.Calculate(times, () => {
                Assert.AreEqual(LongestPalindrome2("babad"), "bab");
                Assert.AreEqual(LongestPalindrome2("cbbd"), "bb");
            });
            Trace.WriteLine($"New:{ts1.TotalMilliseconds}");

            var ts2 = StopwatchHelper.Calculate(times, () => {
                Assert.AreEqual(LongestPalindrome("babad"), "bab");
                Assert.AreEqual(LongestPalindrome("cbbd"), "bb");
            });
            
            
            Trace.WriteLine($"Origin:{ts2.TotalMilliseconds}");
        }

        [TestMethod]
        public void TestIsPalindrome() {
            Assert.IsFalse(IsPalindrome(10));

            Assert.IsTrue(IsPalindrome(121));
            Assert.IsTrue(IsPalindrome(2));
            Assert.IsTrue(IsPalindrome(1223221));

            Assert.IsFalse(IsPalindrome(23));
            
        }

        public string LongestPalindrome(string s) {
            if (string.IsNullOrEmpty(s)) {
                return string.Empty;
            }

            var currentStartIndex = 0;
            var longestPalinLength = 1;

            for (int i = 0; i < s.Length - 1; i++) {
                for (int j = i + 1; j < s.Length; j++) {
                    if (s[i] != s[j]) {
                        continue;
                    }

                    var tryLength = j - i + 1;
                    var isPalin = true;
                    if (tryLength <= longestPalinLength) {
                        continue;
                    }

                    for (int t = 0; t < tryLength / 2; t++) {
                        if (s[i + t] == s[i + tryLength - t - 1]) {
                            continue;
                        }

                        isPalin = false;
                        break;
                    }

                    if (isPalin) {
                        currentStartIndex = i;
                        longestPalinLength = tryLength;
                    }
                }
            }

            return s.Substring(currentStartIndex, longestPalinLength);
        }

        public string LongestPalindrome2(string s) {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            int start = 0, end = 0;
            for (int i = 0; i < s.Length; i++) {
                int len1 = ExpandAroundCenter(s, i, i);
                int len2 = ExpandAroundCenter(s, i, i + 1);
                int len = Math.Max(len1, len2);
                if (len > end - start) {
                    start = i - (len - 1) / 2;
                    end = i + len / 2;
                }
            }
            return s.Substring(start, end + 1);
        }

        private int ExpandAroundCenter(string s, int left, int right) {
            int L = left, R = right;
            while (L >= 0 && R < s.Length && s[L] == s[R]) {
                L--;
                R++;
            }
            return R - L - 1;
        }

        public bool IsPalindrome(int x) {
            if(x < 0) {
                return false;
            }

            var decBitNumber = 0;
            var temp = x;
            while(temp != 0) {
                decBitNumber ++;
                temp /= 10;
            }
            
            var multiNumber = 1;
            for (int i = 0; i < decBitNumber - 1; i++) {
                multiNumber *= 10;
            }

            var lowDevideNumber = 1;
            
            for (int i = 0; i < decBitNumber / 2; i++) {
                if(x / (multiNumber / lowDevideNumber) % 10 != x / lowDevideNumber % 10) {
                    return false;
                }

                lowDevideNumber *= 10;
            }

            return true;
        }
    }

    struct Entity33: IComparable, IFormattable, IConvertible, IComparable<Entity33>, IEquatable<Entity33> {
        public int S { get; set; }
        public override Int32 GetHashCode() {
            return 0;
        }
        public int CompareTo(object obj) {
            throw new NotImplementedException();
        }

        public int CompareTo(Entity33 other) {
            throw new NotImplementedException();
        }

       
        public bool Equals(Entity33 other) {
            throw new NotImplementedException();
        }

        public TypeCode GetTypeCode() {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider provider) {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider) {
            throw new NotImplementedException();
        }
    }
}
