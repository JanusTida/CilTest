using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TestDemo {
    /// <summary>
    /// Summary description for FindAtoi
    /// </summary>
    [TestClass]
    public class FindAtoi {
        [TestMethod]
        public void TestMyAtoi() {
            Assert.AreEqual(MyAtoi("   -423dasdasdsa"),-423);
            Assert.AreEqual(MyAtoi("   -423 "), -423);
            Assert.AreEqual(MyAtoi(" w  42 "), 0);
            Assert.AreEqual(MyAtoi(" -42"), -42);
            Assert.AreEqual(MyAtoi("000000000000000000"), 0);
            Assert.AreEqual(MyAtoi("    0000000000000   "), 0);
        }

        public int MyAtoi(string str) {
            int? numberStartIndex = null;
            int? numberLength = null;

            int? nonEmptyIndex = null;
            char ch = '0';
            bool? isSigned = null;

            for (int i = 0; i < str.Length; i++) {
                if (str[i] != ' ') {
                    nonEmptyIndex = i;
                    break;
                }
            }
            
            if(nonEmptyIndex == null) {
                return 0;
            }

            ch = str[nonEmptyIndex.Value];
            
            if(ch == '+') {
                isSigned = true;
            }
            else if(ch == '-') {
                isSigned = false;
            }

            if(isSigned != null) {
                if (nonEmptyIndex.Value == str.Length - 1 || !CheckCharIsNumber(str[nonEmptyIndex.Value + 1])) {
                    return 0;
                }
                
                numberStartIndex = nonEmptyIndex.Value + 1;
            }
            else if (CheckCharIsNumber(ch)) {
                numberStartIndex = nonEmptyIndex.Value;
            }
            else {
                return 0;
            }
            
            for (int i = numberStartIndex.Value + 1; i < str.Length; i++) {
                if (!CheckCharIsNumber(str[i])) {
                    numberLength = i - numberStartIndex.Value;
                    break;
                }
            }

            if (numberLength == null) {
                numberLength = str.Length - numberStartIndex.Value;
            }

            long longRes = 0;
            isSigned = isSigned ?? true;

            if (isSigned.Value) {
                for (int i = 0; i < numberLength.Value; i++) {
                    longRes *= 10;
                    longRes += ConvertFromCharToNumber(str[numberStartIndex.Value + i]);

                    if(longRes > int.MaxValue) {
                        return int.MaxValue;
                    }
                }
            }
            else {
                for (int i = 0; i < numberLength.Value; i++) {
                    longRes *= 10;
                    longRes -= ConvertFromCharToNumber(str[numberStartIndex.Value + i]);

                    if (longRes < int.MinValue) {
                        return int.MinValue;
                    }
                }
            }

            return (int)longRes;
        }

        private bool CheckCharIsNumber(char ch) {
            return ch >= '0' && ch <= '9';
        }

        private int ConvertFromCharToNumber(char ch) {
            return ch - 48;
        }
        
    }

}
