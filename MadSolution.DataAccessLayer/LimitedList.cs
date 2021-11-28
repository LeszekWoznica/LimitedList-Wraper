using MadSolution.DataAccessLayer.Commons;
using System;
using System.Collections.Generic;

namespace MadSolution.DataAccessLayer
{
    public class LimitedList<T> : List<T>
    {
        private static LimitedListEventArgs _onMaxCountReachEventArgs = new(Messages.MaxCountReached);
        private static LimitedListEventArgs _onObjectFromWaitingRoomWasTaken = new(Messages.MaxCountReached);
        private static LimitedListEventArgs _onListIsEmpty = new(Messages.MaxCountReached);

        private readonly Queue<T> _queue;
        private readonly Stack<T> _stack;

        public event Action<object, LimitedListEventArgs> OnMaxCountReached;
        public event Action<object, LimitedListEventArgs> OnObjectFromWaitingRoomWasTaken;

        public int MaxCount { get; }
        public WaitingRoomType WaitingRoomType { get; }
        public string Name { get; }

        public LimitedList(int maxCount, WaitingRoomType waitingRoomType=WaitingRoomType.FIFO, string name = "Justyna")
        {
            MaxCount = maxCount;
            WaitingRoomType = waitingRoomType;
            Name = name;

            switch (WaitingRoomType)
            {
                case WaitingRoomType.FIFO:
                    break;
                case WaitingRoomType.LIFO:
                    break;
                default:
                    throw new InvalidOperationException(Messages.InvalidRoomType);
            }
        }
        //public LimitedList() : this(100, default, "Justyna bez parametrów") { }
        public LimitedList() : this(100, name: "Justyna bez parametrów") { }

        public new void Add(T item)
        {
            if (Count < MaxCount)
            {
                base.Add(item);
                if (Count == MaxCount) OnMaxCountReached?.Invoke(this, _onMaxCountReachEventArgs);
                return;
            }

            // Do poczekalni .Add(item);

        }
        public new void Insert(int index, T item) { throw new NotImplementedException(); }
        public new bool Remove(T item)
        {
            return base.Remove(item);
        }
        public new void RemoveAt(int index) { throw new NotImplementedException(); }

        public new int RemoveAll(Predicate<T> match) { throw new NotImplementedException(); }
        public new void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection) Add(item);
        }
        public new void RemoveRange(int index, int count) { throw new NotImplementedException(); }
        public new void Clear() => throw new NotImplementedException();
        public void AddItem(T item)
        {

        }

        private void ToWaitingRoomWithInvokeEvent(T item)
        {
            switch (WaitingRoomType)
            {
                case WaitingRoomType.FIFO:
                    break;
                case WaitingRoomType.LIFO:
                    break;
                default:
                    throw new InvalidOperationException(Messages.InvalidRoomType);
            }

        }
    }
}
