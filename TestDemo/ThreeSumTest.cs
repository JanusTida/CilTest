using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    [TestClass]
    public class ThreeSumTest {

        [TestMethod]
        public void TestThreeSum() {
            var nums = new int[] { -1, 0, 1, 2, -1,-2,2, -4 ,-4 ,4,-2};
            var times = 1000000;
            //var ts = StopwatchHelper.Calculate(times, () => ThreeSum2(nums));
            //Trace.WriteLine(ts.TotalMilliseconds);

            //var ts2 = StopwatchHelper.Calculate(times, () => ThreeSum(nums));
            //Trace.WriteLine(ts2.TotalMilliseconds);
            var ss = ThreeSum3(nums);
            Assert.AreEqual(ThreeSum3(nums).Count, 6);
            var ts3 = StopwatchHelper.Calculate(times, () => ThreeSum3(nums));
            Trace.WriteLine(ts3.TotalMilliseconds);

            
          
        }

        [TestMethod]
        public void TestTwoSum() {
            var numList = new List<int>();
            for (int i = 0; i < 55; i++) {
                for (int j = 0; j < 10; j++) {
                    numList.Add(i);
                    numList.Add(-i);
                }
            }
            var nums = numList.ToArray();

            var times = 10000;
            var s2 = TwoSum(nums, target, 0);

            var se = TwoSumEnumerable(nums, target, 0);

            var hashS = TwoSumWithHashSet(nums, target, 0);

            Assert.AreEqual(s2.Count, se.Count());
            Assert.AreEqual(hashS.Count, s2.Count);

            var tsTwoSum = StopwatchHelper.Calculate(times, () => {
                var resNums = TwoSum(nums, target, 0);
                var sum = 0;
                for (int i = 0; i < resNums.Count; i++) {
                    for (int j = 0; j < resNums[i].Count; j++) {
                        sum += resNums[i][j];
                    }
                }
            });

            var tsTwoSumE = StopwatchHelper.Calculate(times, () => {
                var resNums = TwoSumEnumerable(nums, target, 0);
                var sum = 0;
                sum += resNums.SelectMany(p => p).Sum();
            });

            var tsTwoSumHash = StopwatchHelper.Calculate(times, () => {
                var resNums = TwoSumWithHashSet(nums, target, 0);
                var sum = 0;
                for (int i = 0; i < resNums.Count; i++) {
                    for (int j = 0; j < resNums[i].Count; j++) {
                        sum += resNums[i][j];
                    }
                }
            });


            Trace.WriteLine($"TwoSum : {tsTwoSum.TotalMilliseconds}");
            Trace.WriteLine($"TwoSumE : {tsTwoSumE.TotalMilliseconds}");
            Trace.WriteLine($"TwoSumHash : {tsTwoSumHash.TotalMilliseconds}");
        }

        private const int target = 0;
        public IList<IList<int>> ThreeSum(int[] nums) {
            var res = new List<IList<int>>();
            var appliedCells = new HashSet<ComplementCell>();

            for (int i = 0; i < nums.Length - 2; i++) {
                for (int j = i + 1; j < nums.Length - 1; j++) {
                    for (int t = j + 1; t < nums.Length; t++) {
                        if (nums[i] + nums[j] + nums[t] != target) {
                            continue;
                        }

                        if (appliedCells.Contains(new ComplementCell(nums[i], nums[j]))) {
                            continue;
                        }

                        var arr = new int[] {
                            nums[i] ,
                            nums[j] ,
                            nums[t]
                        };

                        res.Add(arr);

                        for (int index = 0; index < arr.Length - 1; index++) {
                            for (int innerIndex = index + 1; innerIndex < arr.Length; innerIndex++) {
                                appliedCells.Add(new ComplementCell(arr[index], arr[innerIndex]));
                                appliedCells.Add(new ComplementCell(arr[innerIndex], arr[index]));
                            }
                        }

                    }
                }
            }

            return res;
        }

        public IList<IList<int>> ThreeSum2(int[] nums) {
            var complementDict = new Dictionary<int, List<ComplementCell>>();
            var res = new List<IList<int>>();
            var appliedCells = new HashSet<ComplementCell>();

            for (int i = 0; i < nums.Length - 1; i++) {
                for (int j = i + 1; j < nums.Length; j++) {
                    var sum = nums[i] + nums[j];
                    if (complementDict.TryGetValue(sum, out var list)) {
                        list.Add(new ComplementCell { Num0 = i, Num1 = j });
                    }
                    else {
                        complementDict.Add(sum, new List<ComplementCell> { new ComplementCell { Num0 = i, Num1 = j } });
                    }
                }
            }

            for (int i = 0; i < nums.Length; i++) {
                var complement = target - nums[i];
                if (!complementDict.TryGetValue(complement, out var list)) {
                    continue;
                }

                list.ForEach(cell => {
                    if (cell.Num0 == i || cell.Num1 == i) {
                        return;
                    }

                    if (appliedCells.Contains(new ComplementCell(nums[cell.Num0], nums[cell.Num1]))) {
                        return;
                    }

                    var arr = new int[] {
                        nums[cell.Num0],
                        nums[cell.Num1],
                        nums[i]
                    };

                    res.Add(arr);

                    for (int index = 0; index < arr.Length - 1; index++) {
                        for (int innerIndex = index + 1; innerIndex < arr.Length; innerIndex++) {
                            appliedCells.Add(new ComplementCell(arr[index], arr[innerIndex]));
                            appliedCells.Add(new ComplementCell(arr[innerIndex], arr[index]));
                        }
                    }
                });

                complementDict.Remove(complement);
            }

            return res;
        }

        public IList<IList<int>> ThreeSum3(int[] nums) {
            var ret = new List<IList<int>>();
            Array.Sort(nums);
            for (int i = 0; i < nums.Length - 2; i++) {
                var numi = nums[i];
                if (nums[i] > 0) {
                    break;
                }

                if (i > 0 && numi == nums[i - 1]) {
                    continue;
                }

                int indexLeft = i + 1, indexRight = nums.Length - 1;
                int complementSum = target - numi;

                while (indexLeft < indexRight) {
                    var numLeft = nums[indexLeft];
                    var numRight = nums[indexRight];
                    var sum = numLeft + numRight;

                    if (sum < complementSum) {
                        indexLeft++;
                    }
                    else if (sum == complementSum) {
                        if (ret.Count == 0 || !(ret[ret.Count - 1][0] == numi && ret[ret.Count - 1][1] == numLeft)) {
                            var temp = new int[] {
                                numi,
                                numLeft,
                                numRight,
                            };
                            ret.Add(temp);
                        }
                        indexLeft++;
                        indexRight--;
                    }
                    else {
                        indexRight--;
                    }
                }
            }

            return ret;

        }

        public IList<IList<int>> ThreeSum32(int[] nums) {
            IList<IList<int>> ret = new List<IList<int>>();
            Array.Sort(nums);
            for (int i = 0; i < nums.Length - 2; i++) {
                if (nums[i] > 0) {
                    break;
                }
                if (i > 0 && nums[i] == nums[i - 1]) {
                    continue;
                }
                int indexLeft = i + 1, indexRight = nums.Length - 1;
                while (indexLeft < indexRight) {
                    if (nums[i] + nums[indexLeft] + nums[indexRight] < 0) {
                        indexLeft++;
                        continue;
                    }

                    if (nums[i] + nums[indexLeft] + nums[indexRight] > 0) {
                        indexRight--;
                        continue;
                    }

                    if (nums[i] + nums[indexLeft] + nums[indexRight] == 0) {
                        IList<int> temp = new List<int>();
                        temp.Add(nums[i]);
                        temp.Add(nums[indexLeft]);
                        temp.Add(nums[indexRight]);
                        if (ret.Count == 0 || !(ret[ret.Count - 1][0] == temp[0] && ret[ret.Count - 1][1] == temp[1] && ret[ret.Count - 1][2] == temp[2])) {
                            ret.Add(temp);
                        }
                        indexLeft++;
                        indexRight--;
                    }
                }
            }

            return ret;
        }

        public IList<IList<int>> ThreeSum4(int[] nums) {
            var res = new List<IList<int>>();
            Array.Sort(nums);

            var maxCompareTimes = nums.Length - 2;
            var lastIndex = nums.Length - 1;

            for (int i = 0; i < maxCompareTimes; i++) {
                var numi = nums[i];

                if (numi > 0) {
                    continue;
                }

                var left = i + 1;
                var right = lastIndex;



                while (left < right) {
                    var numLeft = nums[left];
                    var numRight = nums[right];

                    var sum = numLeft + numRight + numi;

                    if (sum < target) {
                        left++;
                    }
                    else if (sum == target) {
                        var lastArr = res[res.Count - 1];
                        if (res.Count == 0 || (lastArr[0] != numi && lastArr[1] != numLeft)) {

                        }
                    }
                    else {
                        right--;
                    }

                }

            }

            return res;
        }


        public IList<IList<int>> TwoSum(int[] sortedNums, int target,int startIndex) {
            Array.Sort(sortedNums);
            
            var indexLeft = startIndex;
            var indexRight = sortedNums.Length - 1;

            var ret = new List<IList<int>>();
            int? lastNumLeft = null;

            while (indexLeft < indexRight) {
                var numLeft = sortedNums[indexLeft];
                var numRight = sortedNums[indexRight];

                if (indexLeft > 0 && sortedNums[indexLeft - 1] == numLeft) {
                    indexLeft++;
                    continue;
                }

                if (numLeft + numRight < target) {
                    indexLeft++;
                }
                else if (numLeft + numRight == target) {
                    if (lastNumLeft == null || lastNumLeft != numLeft) {
                        var temp = new int[] {
                            numLeft,
                            numRight
                        };
                        ret.Add(temp);
                        lastNumLeft = numLeft;
                    }
                    indexLeft++;
                    indexRight--;
                }
                else {
                    indexRight--;
                }
            }
            return ret;
        }

        public IEnumerable<IList<int>> TwoSumEnumerable(int[] sortedNums, int target, int startIndex,int[] buffer = null) {
            if(buffer == null || buffer.Length < 2) {
                buffer = new int[2];
            }

            Array.Sort(sortedNums);

            var indexLeft = startIndex;
            var indexRight = sortedNums.Length - 1;

            int? lastNumLeft = null;

            while (indexLeft < indexRight) {
                if (indexLeft > 0 && sortedNums[indexLeft - 1] == sortedNums[indexLeft]) {
                    indexLeft++;
                    continue;
                }

                if (sortedNums[indexLeft] + sortedNums[indexRight] < target) {
                    indexLeft++;
                }
                else if (sortedNums[indexLeft] + sortedNums[indexRight] == target) {
                    if (lastNumLeft == null || lastNumLeft != sortedNums[indexLeft]) {
                        buffer[0] = sortedNums[indexLeft];
                        buffer[1] = sortedNums[indexRight];
                        yield return buffer;
                        lastNumLeft = sortedNums[indexLeft];
                    }
                    indexLeft++;
                    indexRight--;
                }
                else {
                    indexRight--;
                }
            }
        }

        public IList<IList<int>> TwoSumWithHashSet(int[] sortedNums, int target, int startIndex, int[] buffer = null) {
            var res = new List<IList<int>>();
            var numHashSet = new HashSet<int>(sortedNums.Length - startIndex,null);
            
            var equalTargetCount = 0;

            for (int i = 0; i < sortedNums.Length; i++) {
                if(sortedNums[i] + sortedNums[i] == target) {
                    if (equalTargetCount == 1) {
                        res.Add(new int[] { sortedNums[i], sortedNums[i] });
                    }
                    equalTargetCount++;
                    continue;
                }
                numHashSet.Add(sortedNums[i]);
            }

            foreach (var num in numHashSet) {
                if(num > target) {
                    continue;
                }
                var complement = target - num;
                if (numHashSet.Contains(complement)) {
                    res.Add(new int[] { num , complement });
                }
            }

            return res;
        }
        struct ComplementCell {
            public ComplementCell(int num0, int num1) {
                this.Num0 = num0;
                this.Num1 = num1;
            }
            public int Num0 { get; set; }
            public int Num1 { get; set; }
        }

        [TestMethod]
        public void TestThreeSumClosest() {
            var nums = new int[] { -1,2,1,-4};
            Assert.AreEqual(ThreeSumClosest(nums, 1), 2);
        }

        public int ThreeSumClosest(int[] nums, int target) {
            if(nums.Length < 3) {
                throw new ArgumentException(nameof(nums));
            }

            Array.Sort(nums);

            int? closestDis = null;
            int? closestSum = null;
            for (int i = 0; i < nums.Length - 2; i++) {
                if(i > 0 && nums[i - 1] == nums[i]) {
                    continue;
                }

                var left = i + 1;
                var right = nums.Length - 1;
                while(left < right) {
                    var thisSum = nums[left] + nums[right] + nums[i];
                    if (thisSum == target) {
                        return thisSum;
                    }

                    var thisDis = Math.Abs(thisSum - target);
                    if (closestDis == null || thisDis < closestDis.Value) {
                        closestSum = thisSum;
                        closestDis = thisDis;
                    }
                    
                    if(thisSum < target) {
                        left++;
                    }
                    else {
                        right--;
                    }
                }
            }

            return closestSum.Value;
        }
    }
}
