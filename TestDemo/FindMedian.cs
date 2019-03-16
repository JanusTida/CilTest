using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace TestDemo {
    /// <summary>
    /// Summary description for FindMedian
    /// </summary>
    [TestClass]
    public class FindMedian {

        [TestMethod]
        public void TestMedian() {
            var nums1 = new int[] { 1, 2, 3 };
            var nums2 = new int[] { 1, 2, 3,4,5,6 };

            var times = 1000000;
            var ts0 = StopwatchHelper.Calculate(times,() => FindMedianSortedArrays(nums1, nums2));
            Trace.WriteLine(ts0.TotalMilliseconds);
            var ts1 = StopwatchHelper.Calculate(times, () => FindMedianSortedArrays2(nums1, nums2));
            Trace.WriteLine(ts1.TotalMilliseconds);
            var ts2 = StopwatchHelper.Calculate(times, () => FindMedianSortedArrays3(nums1, nums2));
            Trace.WriteLine(ts2.TotalMilliseconds);
        }

        public double FindMedianSortedArrays2(int[] nums1, int[] nums2) {
            int m = nums1.Length;
            int n = nums2.Length;
            if (m > n) { // to ensure m<=n
                int[] temp = nums1; nums1 = nums2; nums2 = temp;
                int tmp = m; m = n; n = tmp;
            }
            int iMin = 0, iMax = m, halfLen = (m + n + 1) / 2;
            while (iMin <= iMax) {
                int i = (iMin + iMax) / 2;
                int j = halfLen - i;
                if (i < iMax && nums2[j - 1] > nums1[i]) {
                    iMin = i + 1; // i is too small
                }
                else if (i > iMin && nums1[i - 1] > nums2[j]) {
                    iMax = i - 1; // i is too big
                }
                else { // i is perfect
                    int maxLeft = 0;
                    if (i == 0) { maxLeft = nums2[j - 1]; }
                    else if (j == 0) { maxLeft = nums1[i - 1]; }
                    else { maxLeft = Math.Max(nums1[i - 1], nums2[j - 1]); }
                    if ((m + n) % 2 == 1) { return maxLeft; }

                    int minRight = 0;
                    if (i == m) { minRight = nums2[j]; }
                    else if (j == n) { minRight = nums1[i]; }
                    else { minRight = Math.Min(nums2[j], nums1[i]); }

                    return (maxLeft + minRight) / 2.0;
                }
            }
            return 0.0;
        }
        
        public double FindMedianSortedArrays(int[] nums1, int[] nums2) {
            var sumLength = nums1.Length + nums2.Length;
            if (sumLength == 0) {
                return 0;
            }

            var growingNums = CombineNumsGrowing2(nums1, nums2);

            var index = 0;

            if (sumLength % 2 == 0) {
                var targetNum1 = 0;
                var targetNum2 = 0;
                var targetIndex1 = sumLength / 2 - 1;
                var targetIndex2 = sumLength / 2;

                foreach (var num in growingNums) {
                    if (index == targetIndex1) {
                        targetNum1 = num;
                    }
                    else if (index == targetIndex2) {
                        targetNum2 = num;
                        break;
                    }

                    index++;
                }

                return (double)(targetNum1 + targetNum2) / 2;
            }
            else {
                var targetIndex = sumLength / 2;
                var targetNum = 0;
                foreach (var num in growingNums) {
                    if (index == targetIndex) {
                        targetNum = num;
                        break;
                    }
                    index++;
                }

                return targetNum;
            }
        }


        /// <summary>
        /// 根据有序的数组增减情况生成一个递增的迭代器;
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        private static IEnumerable<int> GetGrowingEnumerable(int[] nums) {
            if (nums.Length == 0) {
                yield break;
            }

            var firstNum = nums[0];
            var isGrowing = false;
            for (int i = 1; i < nums.Length; i++) {
                if (nums[i] == firstNum) {
                    continue;
                }

                if (nums[i] > firstNum) {
                    isGrowing = true;
                }
                break;
            }

            if (isGrowing) {
                for (int i = 0; i < nums.Length; i++) {
                    yield return nums[i];
                }
            }
            else {
                for (int i = nums.Length - 1; i >= 0; i--) {
                    yield return nums[i];
                }
            }
        }

        /// <summary>
        /// 将两个有序数组合并为一个递增的迭代器;
        /// </summary>
        /// <param name="nums1"></param>
        /// <param name="nums2"></param>
        /// <returns></returns>
        private static IEnumerable<int> CombineNumsGrowing(int[] nums1, int[] nums2) {
            var growingNums1 = GetGrowingEnumerable(nums1);
            var growingNums2 = GetGrowingEnumerable(nums2);

            return CombineNumsGrowing(growingNums1, growingNums2);
        }

        /// <summary>
        /// 将两个递增的迭代器合并为一个递增的迭代器;
        /// </summary>
        /// <param name="growingNums1"></param>
        /// <param name="growingNums2"></param>
        /// <returns></returns>
        private static IEnumerable<int> CombineNumsGrowing(IEnumerable<int> growingNums1, IEnumerable<int> growingNums2) {
            var isLastNumFromEnum1 = false;
            var lastNum = 0;

            using (
                IEnumerator<int> enumerator1 = growingNums1.GetEnumerator(), enumerator2 = growingNums2.GetEnumerator()
            ) {
                var enum1NextOk = enumerator1.MoveNext();
                var enum2NextOk = enumerator2.MoveNext();

                while (true) {
                    if (!enum1NextOk) {
                        yield return enumerator2.Current;
                        while (enumerator2.MoveNext()) {
                            yield return enumerator2.Current;
                        }

                        yield break;
                    }

                    if (!enum2NextOk) {
                        yield return enumerator1.Current;
                        while (enumerator1.MoveNext()) {
                            yield return enumerator1.Current;
                        }

                        yield break;
                    }

                    if (enumerator1.Current < enumerator2.Current) {
                        lastNum = enumerator1.Current;
                        isLastNumFromEnum1 = true;
                    }
                    else {
                        lastNum = enumerator2.Current;
                        isLastNumFromEnum1 = false;
                    }

                    yield return lastNum;

                    if (isLastNumFromEnum1) {
                        enum1NextOk = enumerator1.MoveNext();
                    }
                    else {
                        enum2NextOk = enumerator2.MoveNext();
                    }
                }
            }
        }

        private static IEnumerable<int> CombineNumsGrowing2(int[] nums0,int[] nums1) {
            int index0 = 0, index1 = 0;
            int num0Length = nums0.Length, num1Length = nums1.Length;

            while (index0 < num0Length || index1 < num1Length) {
                if(index0 < num0Length && index1 < num1Length) {
                    if(nums0[index0] < nums1[index1]) {
                        yield return nums0[index0];
                        index0++;
                    }
                    else {
                        yield return nums1[index1];
                        index1++;
                    }
                }
                else  {
                    while(index1 < num1Length) {
                        yield return nums1[index1];
                        index1++;
                    }
                    while (index0 < num0Length) {
                        yield return nums0[index0];
                        index0++;
                    }
                }
            }
        }

        static public List<int> MergeSortedArrays(int[] nums1, int[] nums2) {
            List<int> list = new List<int>(nums1.Length + nums2.Length);
            int i1 = 0, i2 = 0;
            int j1 = nums1.Length, j2 = nums2.Length;
            for (; i1 != j1 && i2 != j2;) {
                if (nums1[i1] < nums2[i2]) {
                    list.Add(nums1[i1]);
                    ++i1;
                }
                else {
                    list.Add(nums2[i2]);
                    ++i2;
                }
            }
            if (i1 == j1) {
                for (; i2 != j2; ++i2) {
                    list.Add(nums2[i2]);
                }
            }
            else {
                for (; i1 != j1; ++i1) {
                    list.Add(nums1[i1]);
                }
            }
            return list;
        }

        public double FindMedianSortedArrays3(int[] nums1, int[] nums2) {
            var list = MergeSortedArrays(nums1, nums2);
            if (list.Count % 2 == 0) {
                int middleIndex = list.Count / 2 - 1;
                return (double)(list[middleIndex] + list[middleIndex + 1]) / 2;
            }
            else {
                int middleIndex = list.Count / 2;
                return list[middleIndex];
            }
        }

        private static void Swap(ref int num0,ref int num1) {
            num0 = num0 + num1;
            num1 = num0 - num1;
            num0 = num0 - num1;
        }

        public double FindMedianSortedArrays4(int[] nums0, int[] nums1) {
            if(nums0[nums0.Length / 2] > nums1[nums1.Length / 2]) {
                var temp = nums0;
                nums1 = nums0;
                nums0 = temp;
            }

            var midIndex0 = nums0.Length / 2;
            var midIndex1 = nums1.Length / 2;

            while (nums0[midIndex0] < nums1[midIndex1]) {
                if(midIndex0 == nums0.Length || midIndex1 == -1) {
                    break;
                }

                midIndex0++;
                midIndex1--;
            }



            return 0;
        }
    }
}
