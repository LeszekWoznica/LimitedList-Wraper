using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MadSolution.DataAccessLayer.Tests
{
    internal class LimitedListWrapper_Tests
    {
        [Test]
        public void Foo()
        {
            var list = new LimitedListWrapper<int>(2);
        }

        [Test]
        public void TestCount()
        {
            var list = new LimitedListWrapper<string>(2);
            Assert.AreEqual(0, list.Count);
            list.Add("a");
            Assert.AreEqual(1, list.Count);
            list.Add("b");
            Assert.AreEqual(2, list.Count);
            list.Add("c");
            Assert.AreEqual(2, list.Count);
            List<string> mokeList = new();
            mokeList.Add("a");
            mokeList.Add("b");
            var list2 = new LimitedListWrapper<string>(mokeList, 2);
            Assert.AreEqual(2, list2.Count);
            mokeList.Add("c");
            mokeList.Add("d");

            var list3 = new LimitedListWrapper<string>(mokeList, 2);
            Assert.AreEqual(2, list3.Count);
        }

        [Test]
        public void WhenMaxCountRechedEvenHandler()
        {
            var list = new LimitedListWrapper<string>(1);
            list.OnMaxCountReached += List_OnMaxCountReached;
            list.Add("a");
        }

        private void List_OnMaxCountReached(object arg1, Commons.LimitedListEventArgs arg2)
        {
            Console.WriteLine($"{arg2.Message}");
            Assert.AreEqual("Max items number reached", arg2.Message.ToString());
        }

        [Test]
        public void WhenItemWasTakenFromWaitingRoomFIFO()
        {
            LimitedListWrapper<string> list = new LimitedListWrapper<string>(2);
            list.Add("1a");
            list.Add("2b");
            list.Add("3c");
            list.OnObjectFromWaitingRoomWasTaken += List_OnObjectFromWaitingRoomWasTaken;
            list.Remove("1a");
        }

        [Test]
        public void WhenItemWasTakenFromWaitingRoomLIFO()
        {
            LimitedListWrapper<string> list = new LimitedListWrapper<string>(2, Commons.WaitingRoomType.LIFO);
            list.Add("1a");
            list.Add("2b");
            list.Add("3c");
            list.OnObjectFromWaitingRoomWasTaken += List_OnObjectFromWaitingRoomWasTaken;
            list.Remove("1a");
        }

        private void List_OnObjectFromWaitingRoomWasTaken(object arg1, Commons.LimitedListEventArgs arg2)
        {
            Console.WriteLine($"{arg2.Message}");
            Assert.AreEqual("Items from waiting room was taken", arg2.Message.ToString());
        }

        [Test]
        public void WhenWaitingRoomEmptiedFIFO()
        {
            LimitedListWrapper<string> list = new LimitedListWrapper<string>(2);
            list.Add("1a");
            list.Add("2b");
            list.Add("3c");
            list.OnWaitingRoomEmptied += List_OnWaitingRoomEmptied; ;
            list.Remove("1a");
        }

        [Test]
        public void WhenWaitingRoomEmptiedLIFO()
        {
            LimitedListWrapper<string> list = new LimitedListWrapper<string>(2, Commons.WaitingRoomType.LIFO);
            list.Add("1a");
            list.Add("2b");
            list.Add("3c");
            list.OnWaitingRoomEmptied += List_OnWaitingRoomEmptied; ;
            list.Remove("1a");
        }

        private void List_OnWaitingRoomEmptied(object arg1, Commons.LimitedListEventArgs arg2)
        {
            Console.WriteLine($"{arg2.Message}");
            Assert.AreEqual("Waiting Room is empty", arg2.Message.ToString());
        }

        [Test]
        public void WhenObjectRemoved_ThenObjectIsTakenFromWaitingRoomFIFO()
        {
            LimitedListWrapper<string> list = new LimitedListWrapper<string>(2);
            list.Add("1a");
            list.Add("2b");
            list.Add("3c");
            list.Add("4d");
            list.Remove("1a");
            Assert.AreEqual("2b", list.List.ElementAt(0));
            Assert.AreEqual("3c", list.List.ElementAt(1));
        }

        [Test]
        public void WhenObjectRemoved_ThenObjectIsTakenFromWaitingRoomLIFO()
        {
            LimitedListWrapper<string> list = new LimitedListWrapper<string>(2, Commons.WaitingRoomType.LIFO);
            list.Add("1a");
            list.Add("2b");
            list.Add("3c");
            list.Add("4d");
            list.Remove("1a");
            Assert.AreEqual("2b", list.List.ElementAt(0));
            Assert.AreEqual("4d", list.List.ElementAt(1));
        }

        [Test]
        public void WhenObjectRemovedAndBufforIsEmpty_TheNumberOfItemInTheListWillDecreaseFIFO()
        {
            LimitedListWrapper<string> list = new LimitedListWrapper<string>(2, Commons.WaitingRoomType.FIFO);
            list.Add("1a");
            list.Add("2b");
            list.Add("3c");
            list.Add("4d");
            list.Remove("1a");
            list.Remove("2b");
            list.Remove("3c");
            Assert.AreEqual(1, list.Count);
            list.Remove("4d");
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void WhenObjectRemovedAndBufforIsEmpty_TheNumberOfItemInTheListWillDecreaseLIFO()
        {
            LimitedListWrapper<string> list = new LimitedListWrapper<string>(2, Commons.WaitingRoomType.LIFO);
            list.Add("1a");
            list.Add("2b");
            list.Add("3c");
            list.Add("4d");
            list.Remove("1a");
            list.Remove("2b");
            list.Remove("4d");
            Assert.AreEqual(1, list.Count);
            list.Remove("3c");
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void WhenMaxCountSmallerThenOne_ThrowExeption()
        {
            var actual = Assert.Throws<InvalidCastException>(() => { LimitedListWrapper<string> list = new LimitedListWrapper<string>(0); });
            Assert.AreEqual("Max Count Smaller Then One", actual.Message);
        }
    }
}