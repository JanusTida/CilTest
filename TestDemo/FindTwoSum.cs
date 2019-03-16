using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {

    [TestClass]
    public class FindTwoSum {
        [TestMethod]
        public void TwoSumTest() {
            var times = 1000;
            var sw = new Stopwatch();

            var target = 9;
            var numsLength = 1024;

            var nums = new int[numsLength];
            var preNums = new int[] { 11, 15, 7 , 7, 7, 5, 7, 1, 31, 7, 7 };

            Array.Copy(preNums,0, nums,numsLength - preNums.Length, preNums.Length);

            //var rand = new Random();
            //for (int i = 0; i < numsLength - preNums.Length; i++) {
            //    nums[i] = rand.Next(target + 1, target + 1024);
            //}

            var randStart = target + 1;
            for (int i = 0; i < numsLength - preNums.Length; i++) {
                nums[i] = randStart ++;
            }

            nums[numsLength / 2] = 2;

            sw.Start();
            for (int i = 0; i < times; i++) {
                var indexes = TwoSum2(nums, target);
                Assert.AreEqual(indexes.Length, 2);
                Assert.AreEqual(indexes.Select(p => nums[p]).Sum(), target);
            }
            sw.Stop();
            Trace.WriteLine($"New:{sw.ElapsedMilliseconds}");
            

            sw.Restart();
            for (int i = 0; i < times; i++) {
                var indexes = TwoSum(nums, target);
                Assert.AreEqual(indexes.Length, 2);
                Assert.AreEqual(indexes.Select(p => nums[p]).Sum(), target);
            }
            sw.Stop();
            Trace.WriteLine($"Origin:{sw.ElapsedMilliseconds}");
        }

        public int[] TwoSum(int[] nums, int target) {
            
            for (int index = 0; index < nums.Length - 1; index++) {
                for (int innerIndex = index + 1; innerIndex < nums.Length; innerIndex++) {
                    if (nums[index] + nums[innerIndex] == target) {
                        return new int[] { index, innerIndex };
                    }
                }
            }

            return null;
        }

        public int[] TwoSum2(int[] nums, int target) {
            var map = new Dictionary<int,int>();
            for (int i = 0; i < nums.Length; i++) {
                var thisNum = nums[i];
                var complement = target - thisNum;
                if (map.TryGetValue(complement,out var value)) {
                    return new int[] { value, i };
                }
                
                if (!map.ContainsKey(thisNum)){
                    map.Add(thisNum, i);
                }
                
            }
            return null;
        }
    }
}
