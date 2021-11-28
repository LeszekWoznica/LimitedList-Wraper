using System;
using System.Collections.Generic;
using MadSolution.DataAccessLayer.Commons;
using NUnit.Framework;

namespace MadSolution.DataAccessLayer.Tests
{
    public class LimitedList_Tests
    {
        [Test]
        public void WhenAddWithMaxCountReached_ThenPutObjectIntoWaitingRoom()
        {
            LimitedList<int> testCase = new(100);
            List<int> tc2 = testCase;
            //testCase.Clear();
        }

        [Test]
        public void WhenAdd_ThenCountItemsChangeAppropriate()
        {
        }

        [Test]
        public void WhenObjectRemoved_ThenObjectIsTakenFromWaitingRoom()
        {
        }

        [Test]
        public void MockListUse_tests()
        {
            var list = new LimitedList<string>(2, name: "a"); ;
            var list_2 = new LimitedList<string>(2, name: "b");
            list.OnMaxCountReached += OnMaxCountREachedEventHandler;
            list_2.OnMaxCountReached += OnMaxCountREachedEventHandler;
            EngineImiting(list);
            EngineImiting(list_2);
        }

        private void OnMaxCountREachedEventHandler(object o, LimitedListEventArgs e)
        {
            Console.WriteLine($"Message: {e.Message}");
            if (o is LimitedList<string>) Console.WriteLine($"Name: {((LimitedList<string>)o).Name}");
        }

        private void EngineImiting(LimitedList<string> fakeList)
        {
            fakeList.Add("Fake 1");
            fakeList.Add("Fake 2");
            fakeList.Add("Fake 3");
        }
    }
}