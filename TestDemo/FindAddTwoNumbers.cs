using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDemo {
    [TestClass]
    public class FindAddTwoNumbers {
        [TestMethod]
        public void TestMethod1() {
        }

        [TestMethod]
        public void TestAddTwoNumber() {

            var listNode1 = GetListNodeByArray(new int[] { 5 });
            var listNode2 = GetListNodeByArray(new int[] { 5 });

            var res = AddTwoNumbers(listNode1, listNode2);

        }

        public ListNode AddTwoNumbers(ListNode l1, ListNode l2) {
            if (l1 == null) {
                throw new ArgumentNullException(nameof(l1));
            }

            if (l2 == null) {
                throw new ArgumentNullException(nameof(l2));
            }

            var tempNode1 = l1;
            var tempNode2 = l2;

            var firstIndexNumber1 = tempNode1.val;
            var firstIndexNumber2 = tempNode2.val;

            var firstIndexSum = firstIndexNumber1 + firstIndexNumber2;

            tempNode1 = tempNode1.next;
            tempNode2 = tempNode2.next;

            var node = new ListNode(firstIndexSum % 10);

            var headNode = node;

            var promotedNumber = firstIndexSum / 10;

            while (tempNode1 != null || tempNode2 != null) {
                var thisIndexNumber1 = tempNode1?.val ?? 0;
                var thisIndexNumber2 = tempNode2?.val ?? 0;

                var thisIndexSum = thisIndexNumber1 + thisIndexNumber2 + promotedNumber;

                var thisNode = new ListNode(thisIndexSum % 10);
                node.next = thisNode;

                node = thisNode;

                promotedNumber = thisIndexSum / 10;

                tempNode1 = tempNode1?.next;
                tempNode2 = tempNode2?.next;

            }

            if (promotedNumber != 0) {
                var lastNode = new ListNode(promotedNumber);
                node.next = lastNode;
            }

            return headNode;
        }

        private static int[] GetArrayByListNode(ListNode listNode) {
            var tempNode = listNode;
            var count = 0;

            while (tempNode != null) {
                count++;
                tempNode = tempNode.next;
            }

            tempNode = listNode;
            var numbers = new int[count];
            var index = 0;

            while (tempNode != null) {
                numbers[index++] = tempNode.val;

                tempNode = tempNode.next;
            }

            return numbers;
        }

        private static ListNode GetListNodeByArray(int[] numbers) {
            if (numbers.Length == 0) {
                return null;
            }
            var headNode = new ListNode(numbers[0]);
            var node = headNode;

            for (int i = 0; i < numbers.Length - 1; i++) {
                var thisNode = new ListNode(numbers[i + 1]);
                node.next = thisNode;

                node = thisNode;
            }

            return headNode;
        }


        public class ListNode {
            public int val;
            public ListNode next;
            public ListNode(int x) { val = x; }
        }

    }
}
