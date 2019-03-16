using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {


    [TestClass]
    public class FindRemoveNthFromEnd {
        [TestMethod]
        public void TestRemoveNthFromEnd() {
            var node = ListNode.CreateListByNumbers(new int[] { 1, 2, 3, 4, 5 });
            var res = RemoveNthFromEnd(node, 2);

            var node2 = ListNode.CreateListByNumbers(new int[] { 1,2 });
            var res2 = RemoveNthFromEnd(node2,2);
        }

        
        public ListNode RemoveNthFromEnd(ListNode head, int n) {
            if(head == null) {
                return null;
            }

            var sum = 0;
            var node = head;
            var preNode = head;

            while (node != null) {
                sum++;
                node = node.next;
                if (sum == n) {
                    break;
                }
            }

            if(sum != n) {
                throw new ArgumentException();
            }

            if(node == null) {
                return head.next;
            }
            
            while (node.next != null) {
                preNode = preNode.next;
                node = node.next;
            }
            
            preNode.next = preNode.next?.next;
            return head;
        }
    }
}
