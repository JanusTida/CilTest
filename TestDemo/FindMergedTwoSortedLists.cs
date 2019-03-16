using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    [TestClass]
    public class FindMergedTwoSortedLists {
        [TestMethod]
        public void TestMergeTwoLists() {
            var node = MergeTwoLists(ListNode.CreateListByNumbers(new int[] { 1, 2, 4 }), ListNode.CreateListByNumbers(new int[] { 1, 3, 4 }));
        }

        
        public ListNode MergeTwoLists(ListNode l1, ListNode l2) {
            if(l1 == null) {
                if(l2 == null) {
                    return null;
                }
                else {
                    return l2;
                }
            }
            else if(l2 == null) {
                return l1;
            }

            ListNode node1 = null;
            ListNode node2 = null;
            ListNode node = null;
            ListNode head = null;
            if(l1.val < l2.val) {
                head = l1;
                node1 = l1.next;
                node2 = l2;
            }
            else {
                head = l2;
                node1 = l1;
                node2 = l2.next;
            }

            node = head;

            while(true) {
                if(node1 == null) {
                    node.next = node2;
                    return head;
                }
                else if(node2 == null){
                    node.next = node1;
                    return head;
                }
                else {
                    if(node1.val < node2.val) {
                        node.next = node1;
                        node1 = node1.next;
                    }
                    else {
                        node.next = node2;
                        node2 = node2.next;
                    }
                    node = node.next;
                }
            }
            
        }
    }
}
