using System;
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
		private static LimitedListEventArgs _onObjectFromWaitingRoomWasTaken = new(Messages.MaxCountReached);
		private static LimitedListEventArgs _onListIsEmpty = new(Messages.MaxCountReached);

		private readonly Queue<T> _queue;
		private readonly Stack<T> _stack;

		public event Action<object, LimitedListEventArgs> OnMaxCountReached;
		public event Action<object, LimitedListEventArgs> OnObjectFromWaitingRoomWasTaken;

		public int MaxCount { get; }
		public WaitingRoomType WaitingRoomType { get; }
		public string Name { get; }

		private List<T> _buffor;
		public IEnumerable<T> List { get => _buffor; }
		public int Count { get => _buffor.Count; }
		public LimitedListWrapper(List<T> list, int maxCount, WaitingRoomType waitingRoomType = WaitingRoomType.FIFO, string name = "Justyna")
		{
			_buffor = list;
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
		public LimitedListWrapper(int maxCount, WaitingRoomType waitingRoomType = WaitingRoomType.FIFO, string name = "Justyna") : this(new List<T>(), maxCount, waitingRoomType, name) { }
		public LimitedListWrapper() : this(100, name: "Justyna bez parametrów") { }

		public void Add(T item)
		{
			if (_buffor.Count < MaxCount)
			{
				_buffor.Add(item);
				if (_buffor.Count == MaxCount) OnMaxCountReached?.Invoke(this, _onMaxCountReachEventArgs);
				return;
			}

			// Do poczekalni .Add(item);

		}
		public bool Remove(T item)
		{
			throw new NotImplementedException();
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
