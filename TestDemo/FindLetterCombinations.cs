using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    [TestClass]
    public class FindLetterCombinations {
        [TestMethod]
        public void TestLetterCombinations() {
            var str = "2344488";
            var times = 10000;
            var ts0 = StopwatchHelper.Calculate(times, () => {
                var s = LetterCombinations(str);
            });

            Trace.WriteLine($"Old : {ts0.TotalMilliseconds}");
            var ts1 = StopwatchHelper.Calculate(times, () => {
                var s = LetterCombinations2(str);
            });
            Trace.WriteLine($"New : {ts1.TotalMilliseconds}");
        }

        private static readonly IReadOnlyDictionary<char, string> _phoneLetterDict = new Dictionary<char, string> {
            { '2',"abc"},
            { '3',"def"},
            { '4',"ghi"},
            { '5',"jkl"},
            { '6',"mno"},
            { '7',"pqrs"},
            { '8',"tuv"},
            { '9',"wxyz"},
        };

        private static readonly Dictionary<char, char[]> dictionary = new Dictionary<char, char[]> {
            {'2', new[] { 'a', 'b', 'c' }},
            {'3', new[] { 'd', 'e', 'f' }},
            {'4', new[] { 'g', 'h', 'i' }},
            {'5', new[] { 'j', 'k', 'l' }},
            {'6', new[] { 'm', 'n', 'o' }},
            {'7', new[] { 'p', 'q', 'r', 's' }},
            {'8', new[] { 't', 'u', 'v' }     },
            {'9', new[] { 'w', 'x', 'y', 'z' }}
        };
        
        public IList<string> LetterCombinations(string digits) {
            if (string.IsNullOrEmpty(digits)) {
                return new string[0];
            }
            var stringCount = 1;
            for (int i = 0; i < digits.Length; i++) {
                stringCount *= _phoneLetterDict[digits[i]].Length;
            }

            var stringArr = new string[stringCount];
            var stringIndex = 0;
            var charArr = new char[digits.Length];
            
            //初始化;
            for (int i = 0; i < charArr.Length; i++) {
                charArr[i] = _phoneLetterDict[digits[i]][0];
            }
            
            while(stringIndex < stringCount) {
                stringArr[stringIndex] = new string(charArr);

                var charIndex = charArr.Length - 1;
                while (charIndex >= 0) {
                    var thisStr = _phoneLetterDict[digits[charIndex]];
                    if (charArr[charIndex] != thisStr[thisStr.Length - 1]) {
                        charArr[charIndex] = (char)(charArr[charIndex] + 1);
                        break;
                    }
                    else {
                        charArr[charIndex] = thisStr[0];
                    }
                    charIndex--;
                }

                stringIndex++;
            }
            
            return stringArr;
        }

        public IList<string> LetterCombinations2(string digits) {
            List<string> list = new List<string>();
            if (digits.Length == 0)
                return list;



            LetterCombinations(dictionary, list, digits, new List<char>(), 0);

            return list;
        }

        void LetterCombinations(Dictionary<char, char[]> dictionary, List<string> list, string digits, List<char> tempString, int index) {
            if (tempString.Count == digits.Length) {
                list.Add(new string(tempString.ToArray()));
            }
            else {
                foreach (char c in dictionary[digits[index]]) {
                    tempString.Add(c);

                    LetterCombinations(dictionary, list, digits, tempString, index + 1);

                    tempString.RemoveAt(tempString.Count - 1);
                }
            }
        }

    }
}
