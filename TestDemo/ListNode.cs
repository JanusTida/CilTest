using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo {
    public class ListNode {
        public int val;
        public ListNode next;
        public ListNode(int x) { val = x; }

        public static ListNode CreateListByNumbers(IEnumerable<int> numbers) {
            ListNode node = null, head = null;
            foreach (var num in numbers) {
                if (head == null) {
                    head = new ListNode(num);
                    node = head;
                }
                else {
                    node.next = new ListNode(num);
                    node = node.next;
                }
            }

            return head;
        }
    }

}
