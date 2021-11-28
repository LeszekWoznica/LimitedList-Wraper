using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MadSolution.DataAccessLayer.Commons;

namespace MadSolution.DataAccessLayer
{
    public class LimitedListWrapper<T>
    {
        private static LimitedListEventArgs _onMaxCountReachEventArgs = new(Messages.MaxCountReached);
        private static LimitedListEventArgs _onObjectFromWaitingRoomWasTaken = new(Messages.ItemWasTakenFromWaitingRoom);
        private static LimitedListEventArgs _onListIsEmpty = new(Messages.MaxCountReached); //
        private static LimitedListEventArgs _onWaitingRoomIsEmpty = new(Messages.WaitingRoomIsEmpty);

        private readonly Queue<T> _queue;
        private readonly Stack<T> _stack;

        public event Action<object, LimitedListEventArgs> OnMaxCountReached;

        public event Action<object, LimitedListEventArgs> OnObjectFromWaitingRoomWasTaken;

        public event Action<object, LimitedListEventArgs> OnWaitingRoomEmptied;

        public int MaxCount { get; }
        public WaitingRoomType WaitingRoomType { get; }
        public string Name { get; }

        private List<T> _buffor;
        public IEnumerable<T> List { get => _buffor; }
        public int Count { get => _buffor.Count; }

        public LimitedListWrapper(List<T> list, int maxCount, WaitingRoomType waitingRoomType = WaitingRoomType.FIFO, string name = "Justyna")
        {
            MaxCount = maxCount >= 1 ? maxCount : throw new InvalidCastException(Messages.MaxCountSmallerThenOne);
            WaitingRoomType = waitingRoomType;
            Name = name;

            switch (WaitingRoomType)
            {
                case WaitingRoomType.FIFO:
                    _queue = new Queue<T>();
                    break;

                case WaitingRoomType.LIFO:
                    _stack = new Stack<T>();
                    break;

                default:
                    throw new InvalidOperationException(Messages.InvalidRoomType);
            }

            if (list.Count > maxCount)
            {
                _buffor = new List<T>();
                for (int i = 0; i < maxCount; i++)
                {
                    _buffor.Add(list.ElementAt(i));
                }
                for (int i = maxCount; i < list.Count; i++)
                {
                    Add(list.ElementAt(i));
                }
            }
            else

            {
                _buffor = list;
            }
        }

        public LimitedListWrapper(int maxCount, WaitingRoomType waitingRoomType = WaitingRoomType.FIFO, string name = "Justyna") : this(new List<T>(), maxCount, waitingRoomType, name)
        {
        }

        public LimitedListWrapper() : this(5, name: "Justyna bez parametrów")
        {
        }

        public void Add(T item)
        {
            if (_buffor.Count < MaxCount)
            {
                _buffor.Add(item);
                if (_buffor.Count == MaxCount) OnMaxCountReached?.Invoke(this, _onMaxCountReachEventArgs);
                return;
            }
            if (WaitingRoomType == WaitingRoomType.FIFO)
            {
                _queue.Enqueue(item);
            }
            if (WaitingRoomType == WaitingRoomType.LIFO)
            {
                _stack.Push(item);
            }
        }

        public bool Remove(T item)
        {
            _buffor.Remove(item);
            if (WaitingRoomType == WaitingRoomType.FIFO)
            {
                if (_queue.Count != 0)
                {
                    OnObjectFromWaitingRoomWasTaken?.Invoke(this, _onObjectFromWaitingRoomWasTaken);
                    _buffor.Add(_queue.Dequeue());
                    if (_queue.Count == 0) OnWaitingRoomEmptied?.Invoke(this, _onWaitingRoomIsEmpty);
                    return true;
                }
            }
            if (WaitingRoomType == WaitingRoomType.LIFO)
            {
                if (_stack.Count != 0)
                {
                    OnObjectFromWaitingRoomWasTaken?.Invoke(this, _onObjectFromWaitingRoomWasTaken);
                    _buffor.Add(_stack.Pop());
                    if (_stack.Count == 0) OnWaitingRoomEmptied?.Invoke(this, _onWaitingRoomIsEmpty);
                    return true;
                }
            }
            return true;
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