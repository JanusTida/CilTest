using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    [TestClass]
    public class FindFourSum {
        [TestMethod]
        public void TestFourSum() {
            var nums = new int[] { 1, 0, -1, 0, -2, 2 };
            var s = FourSum(nums, 0);

        }

        public IList<IList<int>> FourSum(int[] nums, int target) {
            Array.Sort(nums);

            var res = new List<IList<int>>();
            for (int i = 0; i < nums.Length - 3; i++) {
                if(i != 0 && nums[i] == nums[i - 1]) {
                    continue;
                }
                for (int j = i + 1; j < nums.Length - 2; j++) {
                    if(j != i + 1 && nums[j] == nums[j - 1]) {
                        continue;
                    }

                    int indexLeft = j + 1, indexRight = nums.Length - 1;
                    int complementSum = target - nums[i] - nums[j];
                    while(indexLeft < indexRight) {
                        var numLeft = nums[indexLeft];
                        var numRight = nums[indexRight];
                        var sum = numLeft + numRight;
                        
                        if (sum < complementSum) {
                            indexLeft++;
                        }
                        else if(sum == complementSum) {
                            var isRepeated = true;
                            if(res.Count == 0) {
                                isRepeated = false;
                            }
                            else {
                                var lastArr = res[res.Count - 1];
                                if(!(lastArr[0] == nums[i] && lastArr[1] == nums[j] && lastArr[2] == nums[indexLeft])) {
                                    isRepeated = false;
                                }
                            }
                            if (!isRepeated) {
                                var newArr = new int[] {
                                    nums[i],
                                    nums[j],
                                    nums[indexLeft],
                                    nums[indexRight]
                                };
                                res.Add(newArr);
                            }
                            
                            indexLeft++;
                            indexRight--;
                        }
                        else {
                            indexRight--;
                        }
                    }
                }
            }

            return res;
        }
    }
}
