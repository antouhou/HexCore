using System.Collections.Generic;

namespace HexCore
{
    internal struct QueueItem<T>
    {
        public T Value;
        public double Priority;
    }

    public class PriorityQueue<T>
    {
        private readonly List<QueueItem<T>> _items = new List<QueueItem<T>>();

        public int Count => _items.Count;

        public void Enqueue(T item, double priority)
        {
            _items.Add(new QueueItem<T> {Value = item, Priority = priority});
        }

        public T Dequeue()
        {
            var bestIndex = 0;

            for (var i = 0; i < _items.Count; i++)
                if (_items[i].Priority < _items[bestIndex].Priority)
                    bestIndex = i;

            var bestItem = _items[bestIndex].Value;
            _items.RemoveAt(bestIndex);
            return bestItem;
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}