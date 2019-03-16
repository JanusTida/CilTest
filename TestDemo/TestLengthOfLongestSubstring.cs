using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    [TestClass]
    public class TestLengthOfLongestSubstring {
        [TestMethod]
        public void TestLengthOfLongestSubString() {
            Assert.AreEqual(LengthOfLongestSubstring("au"),2);
            Assert.AreEqual(LengthOfLongestSubstring("a"), 1);
            Assert.AreEqual(LengthOfLongestSubstring("aaaa"), 1);
            Assert.AreEqual(LengthOfLongestSubstring("abcabcbb"), 3);
            Assert.AreEqual(LengthOfLongestSubstring("pwwkew"), 3);
            Assert.AreEqual(LengthOfLongestSubstring("dvdf"), 3);
        }

        public int LengthOfLongestSubstring(string s) {
            if(s.Length == 0) {
                return 0;
            }

            var currentSubLength = 1;
            var currentStartIndex = 0;

            var longestLength = currentSubLength;

            for (int i = 1; i < s.Length; i++) {
                int? repeatedIndex = null;
                for (int j = currentStartIndex; j < currentStartIndex + currentSubLength; j++) {
                    if(s[j] == s[i]) {
                        repeatedIndex = j;
                        break;
                    }
                }

                if(repeatedIndex != null) {
                    if (currentSubLength > longestLength) {
                        longestLength = currentSubLength;
                    }

                    currentSubLength = i - repeatedIndex.Value;
                    currentStartIndex = repeatedIndex.Value + 1;
                }
                else {
                    currentSubLength++;
                }
            }

            if (currentSubLength > longestLength) {
                longestLength = currentSubLength;
            }

            return longestLength;
        }
    }
}
