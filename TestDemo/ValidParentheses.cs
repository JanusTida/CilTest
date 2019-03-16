using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDemo {
    [TestClass]
    public class ValidParentheses {
        [TestMethod]
        public void TestMethod1() {
            Assert.IsTrue(IsValid("()"));
            Assert.IsTrue(IsValid("()[]{}"));

            Assert.IsFalse(IsValid("(]"));
            Assert.IsFalse(IsValid("([)]"));
            Assert.IsTrue(IsValid("{[]}"));
            Assert.IsFalse(IsValid("(()("));
        }
        
        private static readonly Dictionary<char,char> _parentDict = new Dictionary<char,char> {
            { '[',']' },
            { '{','}' },
            { '(',')' }
        };
        

        public bool IsValid(string s) {
            var stackCapacity = s.Length / 2;
            var chStack = new Stack<char>(stackCapacity);
            for (int i = 0; i < s.Length; i++) {
                if (_parentDict.ContainsKey(s[i])) {
                    if(chStack.Count == stackCapacity) {
                        return false;
                    }
                    else {
                        chStack.Push(s[i]);
                    }
                }
                else if(!(chStack.Count != 0 && _parentDict[chStack.Pop()] == s[i])) {
                    return false;
                }
            }

            return chStack.Count == 0;
        }
    }
}
