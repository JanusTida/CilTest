using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    [TestClass]
    public class TwoContainer {
        [TestMethod]
        public void TestMaxArea() {
            var height = new int[] { 1, 8, 6, 2, 5, 4, 8, 3, 7 };
            Assert.AreEqual(49, MaxArea(height));

        }

        public int MaxArea(int[] height) {
            int i = 0, j = height.Length - 1;
            int area = 0;

            while (i < j) {
                area = Math.Max(area, Math.Min(height[i], height[j]) * (j - i));
                if (height[i] < height[j]) {
                    i++;
                }
                else {
                    j--;
                }
            }

            return area;
        }

        private static (int number, string str)[] RomanNumberStrings = new (int number, string str)[] {
            (1,"I"),
            (4,"IV"),
            (5,"V"),

            (9,"IX"),
            (10,"X"),
            (40,"XL"),
            (50,"L"),
            (90,"XC"),

            (100,"C"),
            (400,"CD"),
            (500,"D"),
            (900,"CM"),
            (1000,"M")
        };

        private static (int number, string str)[] RomanNumberStrings2 = new (int number, string str)[] {
            (4,"IV"),
            (9,"IX"),
            (40,"XL"),
            (90,"XC"),
            (400,"CD"),
            (900,"CM"),

            (1,"I"),
            (5,"V"),
            (10,"X"),

            (50,"L"),
            (100,"C"),
            (500,"D"),
            (1000,"M"),
        };

        private static readonly Dictionary<char, int> Roman = new Dictionary<char, int> {
                { 'I', 1 },
                { 'V', 5 },
                { 'X', 10 },
                { 'L', 50 },
                { 'C', 100 },
                { 'D', 500 },
                { 'M', 1000 }
            };

        public string IntToRoman(int num) {
            var sb = new StringBuilder();
            for (int i = RomanNumberStrings.Length - 1; i >= 0; i--) {
                while (num / RomanNumberStrings[i].number != 0) {
                    sb.Append(RomanNumberStrings[i].str);
                    num -= RomanNumberStrings[i].number;
                }
            }

            return sb.ToString();
        }

        public int RomanToInt(string s) {
            var roman = 0;
            var chIndex = s.Length - 1;
            while (chIndex >= 0) {
                for (int i = 0; i < RomanNumberStrings2.Length; i++) {
                    var (number, str) = RomanNumberStrings2[i];

                    if (chIndex >= str.Length - 1 &&
                        CheckEqual(s, chIndex - str.Length + 1, str.Length, str)) {

                        do {
                            roman += number;
                            chIndex -= str.Length;
                        } while (
                            chIndex >= str.Length - 1 &&
                            CheckEqual(s, chIndex - str.Length + 1, str.Length, str)
                        );

                        break;
                    }
                    else if (i == RomanNumberStrings2.Length - 1) {
                        throw new FormatException();
                    }
                }
            }


            return roman;
        }

        public int RomanToInt2(string s) {
            int result = Roman[s[s.Length - 1]];
            char preChar = s[s.Length - 1];
            for (int i = s.Length - 2; i >= 0; i--) {
                if (Roman[s[i]] >= Roman[preChar]) {
                    result += Roman[s[i]];
                }
                else {
                    result -= Roman[s[i]];
                }
                preChar = s[i];
            }
            return result;
        }
        private static bool CheckEqual(string str, int startIndex, int length, string compareString) {
            for (int i = startIndex, comIndex = 0; comIndex < length; i++, comIndex++) {
                if (compareString[comIndex] != str[i]) {
                    return false;
                }
            }

            return true;
        }

        [TestMethod]
        public void TestIntToRoman() {
            Assert.AreEqual(IntToRoman(3), "III");
            Assert.AreEqual(IntToRoman(4), "IV");
            Assert.AreEqual(IntToRoman(5), "V");
            Assert.AreEqual(IntToRoman(9), "IX");
            Assert.AreEqual(IntToRoman(10), "X");
        }


        [TestMethod]
        public void TestRomanToInt() {
            Assert.AreEqual(RomanToInt("III"), 3);
            Assert.AreEqual(RomanToInt("IV"), 4);
            Assert.AreEqual(RomanToInt("V"), 5);
            Assert.AreEqual(RomanToInt("VII"), 7);
            Assert.AreEqual(RomanToInt("X"), 10);
            Assert.AreEqual(RomanToInt("LVIII"), 58);
            Assert.AreEqual(RomanToInt("MCMXCVI"), 1996);
        }

        [TestMethod]
        public void TestLongestCommonPrefix() {
            var paras = new (string[] strs, string prefix)[] {
                (new string[] { "flower", "flow", "flight" },"fl"),
                (new string[] { "dog", "racecar", "car" },string.Empty),
                (new string[] {},string.Empty)
            };

            var times = 1000000;
            var ts2 = StopwatchHelper.Calculate(times, () => {
                foreach (var item in paras) {
                    Assert.AreEqual(LongestCommonPrefix2(item.strs),item.prefix );
                }
            });
            Trace.WriteLine($"New : {ts2.TotalMilliseconds}");

            var ts = StopwatchHelper.Calculate(times, () => {
                foreach (var item in paras) {
                    Assert.AreEqual(LongestCommonPrefix(item.strs), item.prefix);
                }
            });

            Trace.WriteLine($"Origin : {ts.TotalMilliseconds}");
        }


        public string LongestCommonPrefix(string[] strs) {
            if (strs.Length == 0) {
                return string.Empty;
            }

            var minLength = strs[0].Length;
            for (int i = 1; i < strs.Length; i++) {
                if (strs[i].Length < minLength) {
                    minLength = strs[i].Length;
                }
            }

            for (int prefixEndIndex = 0; prefixEndIndex < minLength; prefixEndIndex++) {
                var ch = strs[0][prefixEndIndex];
                for (int i = 1; i < strs.Length; i++) {
                    if (strs[i][prefixEndIndex] != ch) {
                        return strs[0].Substring(0, prefixEndIndex);
                    }
                }
            }


            return strs[0].Substring(0, minLength);
        }

        public string LongestCommonPrefix2(string[] strs) {
            /*------------方法一  暴力破解-----------------*/
            //思路，先找到前两个最长的子字符串//
            string str = "";
            if (strs.Length == 1)
                return strs[0];
            if (strs.Length >= 2) {
                //找到前两个相同的最长子字符串
                str = Prefix(strs[0], strs[1]);
                //在后面的字符串中判断
                for (int k = 2; k < strs.Length; k++) {
                    while (!strs[k].Contains(str) || strs[k].Substring(0, str.Length) != str) { str = str.Substring(0, str.Length - 1); }
                }
            }
            return str;
            /*---------------------------------------*/
        }


        //计算两个字符串的最长子串
        public string Prefix(string str1, string str2) {
            //int start= 0,end = 0;
            int i = 0, j = 0;
            while (i < str1.Length && j < str2.Length) {
                if (str1[i] == str2[j]) { i++; j++; }
                else break;
            }
            return str1.Substring(0, i);
        }

    }




}
