using System;
using System.Collections.Generic;

namespace HexCore.DataStructures
{
    public class PriorityQueue<T>
    {
        private readonly List<Tuple<T, double>> _elements = new List<Tuple<T, double>>();

        public int Count => _elements.Count;

        public void Enqueue(T item, double priority)
        {
            _elements.Add(Tuple.Create(item, priority));
        }

        public T Dequeue()
        {
            var bestIndex = 0;

            for (var i = 0; i < _elements.Count; i++)
                if (_elements[i].Item2 < _elements[bestIndex].Item2)
                    bestIndex = i;

            var bestItem = _elements[bestIndex].Item1;
            _elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }
}