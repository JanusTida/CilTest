using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    [TestClass]
    public class FindMergeKSrtedLists {
        [TestMethod]
        public void TestMergeKLists() {
            var list = new List<ListNode>();
            list.Add(ListNode.CreateListByNumbers(new int[] { 1, 4, 5 }));
            list.Add(ListNode.CreateListByNumbers(new int[] { 1, 3, 4 }));
            list.Add(ListNode.CreateListByNumbers(new int[] { 2, 6 }));
            //var s = MergeKLists(list.ToArray());

            var s2 = MergeKLists2(list.ToArray());
        }

        public ListNode MergeKLists(ListNode[] lists) {
            var nodeContainerList = new List<ListNodeContainer>(lists.Length);
            nodeContainerList.AddRange(lists.Where(p => p != null).Select(p => new ListNodeContainer { ListNode = p }));

            ListNode head = null;
            ListNode node = null;

            while(nodeContainerList.Count != 0) {
                ListNodeContainer containerWithMinValue = null;
                nodeContainerList.ForEach(p => {
                    if(containerWithMinValue == null || p.ListNode.val < containerWithMinValue.ListNode.val) {
                        containerWithMinValue = p;
                    }
                });
                
                if(node == null) {
                    node = containerWithMinValue.ListNode;
                    head = node;
                }
                else {
                    node.next = containerWithMinValue.ListNode;
                    node = node.next;
                }

                containerWithMinValue.ListNode = containerWithMinValue.ListNode.next;
                if(containerWithMinValue.ListNode == null) {
                    nodeContainerList.Remove(containerWithMinValue);
                }
            }

            return head;
        }
        public ListNode MergeKLists2(ListNode[] lists) {
            var dict = new SortedDictionary<int, int>();
            foreach (var head in lists) {
                var node = head;
                while(node != null) {
                    if (dict.ContainsKey(node.val)) {
                        dict[node.val]++;
                    }
                    else {
                        dict.Add(node.val, 1);
                    }
                    node = node.next;
                }
            }


            ListNode resHead = null;
            ListNode resNode = null;

            foreach (var tuple in dict) {
                ListNode thisHead = null;
                ListNode thisNode = null;

                thisNode = new ListNode(tuple.Key);
                thisHead = thisNode;
                for (int i = 0; i < tuple.Value - 1; i++) {
                    thisNode.next = new ListNode(tuple.Key);
                    thisNode = thisNode.next;
                }

                if(resHead == null) {
                    resHead = thisHead;
                    resNode = thisNode;
                }
                else {
                    resNode.next = thisHead;
                    resNode = thisNode;
                }
            }

            return resHead;
        }

        class ListNodeContainer {
            public ListNode ListNode { get; set; }
        }

    }
}
