using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDemo {
    [TestClass]
    public class Regex {
        [TestMethod]
        public void TestRegex() {
            var s = GetSerialPeriods("3312ss2312333");

            //Assert.IsTrue(IsMatch("aa", "a*"));
            //Assert.IsFalse(IsMatch("aa", "a"));

            //Assert.IsTrue(IsMatch("ab", ".*"));

            var s2 = System.Text.RegularExpressions.Regex.IsMatch("aa", "a");
            var s22 = System.Text.RegularExpressions.Regex.IsMatch("ab", ".*");
        }

        
        [TestMethod]
        public void TestStarPeriods() {
            var s = GetStarPeriods("1**1");
            var s2 = GetStarPeriods("**1**");
            var s3 = GetStarPeriods("3131*1*");

            var ss = GetNonStarPeriods("1***1");

            var ss2 = GetNonStarPeriods("**1***1");

            var ss3 = GetNonStarPeriods("*1**111**1*31");
        }

        [TestMethod]
        public void TestGetNonMultiStarString() {
            Assert.AreEqual(MergeSerialStarString("**1**1**1*31231"),"1*1*1*31231");
            Assert.AreEqual(MergeSerialStarString("231"), "231");
            Assert.AreEqual(MergeSerialStarString("231*"), "231*");
            Assert.AreEqual(MergeSerialStarString("**231"), "231");
            Assert.AreEqual(MergeSerialStarString("***"), string.Empty);
        }

        public bool IsMatch(string s, string pattern) {
            if (string.IsNullOrEmpty(pattern)) {
                return string.IsNullOrEmpty(s);
            }

            if (string.IsNullOrEmpty(s)) {
                return false;
            }

            //将表达式中所有连续星号段分别合并为一个星号;
            var handledPattern = MergeSerialStarString(pattern);
            //若返回为空字符串,则表达式中所有的字符均为星号;
            if(handledPattern == string.Empty) {
                return true;
            }
            
            int indexInPattern = 0;

            foreach (var (ch,length) in GetSerialPeriods(s)) {
                char? charInPattern = null;
                while (indexInPattern < handledPattern.Length) {
                    if(handledPattern[indexInPattern] != Char_Star) {
                        charInPattern = handledPattern[indexInPattern];
                        break;
                    }
                    indexInPattern++;
                }

                if(charInPattern == null) {
                    return false;
                }

                if (charInPattern != ch && charInPattern != Char_Any) {
                    return false;
                }

                indexInPattern++;

                if (length == 1) {
                    continue;
                }

                for (int i = 1; i < length; i++) {
                    if (indexInPattern == handledPattern.Length) {
                        return false;
                    }

                    charInPattern = handledPattern[indexInPattern];

                    if (charInPattern == Char_Star) {
                        break;
                    }

                    if (charInPattern != Char_Any && charInPattern != ch) {
                        return false;
                    }

                    indexInPattern++;
                }
            }


            return true;
        }

        private static IEnumerable<(char ch,int length)> GetSerialPeriods(string s) {
            if (string.IsNullOrEmpty(s)) {
                yield break;
            }

            var lastChar = s[0];
            var lastCharIndex = 0;

            for (int i = 1; i < s.Length; i++) {
                if (s[i] != lastChar) {
                    yield return (lastChar, i - lastCharIndex);

                    lastChar = s[i];
                    lastCharIndex = i;
                }
            }

            yield return (lastChar,s.Length - lastCharIndex);
        }
        
        /// <summary>
        /// 将所有连续的星号段分别合并为一个星号;
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private static string MergeSerialStarString(string pattern) {
            if (string.IsNullOrEmpty(pattern)) {
                return pattern;
            }
            
            var pArray = pattern.ToCharArray();

            (int start, int length)? lastPeriod = null;
            var operationIndex = 0;

            foreach (var (start,length) in GetNonStarPeriods(pattern)) {
                
                if(lastPeriod != null) {
                    Array.Copy(pArray, lastPeriod.Value.start, pArray, operationIndex, lastPeriod.Value.length + 1);
                    operationIndex += lastPeriod.Value.length + 1;
                }
                
                lastPeriod = (start, length);
            }

            if(lastPeriod != null) {
                Array.Copy(pArray, lastPeriod.Value.start, pArray, operationIndex, lastPeriod.Value.length);
                operationIndex += lastPeriod.Value.length;

                if(lastPeriod.Value.start + lastPeriod.Value.length < pArray.Length) {
                    pArray[operationIndex] = Char_Star;
                    operationIndex++;
                }
            }

            return new string(pArray,0,operationIndex);
        }

        private static IEnumerable<(int start, int length)> GetStarPeriods(string input){
            if (string.IsNullOrEmpty(input)) {
                yield break;
            }

            //去除所有连续重复的星号;
            int? lastStarIndex = null;
            for (int i = 0; i < input.Length; i++) {
                var ch = input[i];
                if (ch != '*') {
                    if (lastStarIndex != null) {
                        yield return (lastStarIndex.Value, i - lastStarIndex.Value);
                        lastStarIndex = null;
                    }
                }
                else if(lastStarIndex == null){
                    lastStarIndex = i;
                }
            }

            if(lastStarIndex != null) {
                yield return (lastStarIndex.Value, input.Length - lastStarIndex.Value);
            }
        }

        private const char Char_Star = '*';
        private const char Char_Any = '.';

        private static IEnumerable<(int start, int length)> GetNonStarPeriods(string input) {
            if (string.IsNullOrEmpty(input)) {
                yield break;
            }

            //去除所有连续重复的星号;
            int? lastNonStarIndex = null;
            for (int i = 0; i < input.Length; i++) {
                if (input[i] == Char_Star) {
                    if (lastNonStarIndex != null && i != lastNonStarIndex.Value) {
                        yield return (lastNonStarIndex.Value, i - lastNonStarIndex.Value);
                        lastNonStarIndex = null;
                    }
                }
                else if(lastNonStarIndex == null) {
                    lastNonStarIndex = i;
                }
            }

            if (lastNonStarIndex != null) {
                yield return (lastNonStarIndex.Value, input.Length - lastNonStarIndex.Value);
            }
        }
    }
}
