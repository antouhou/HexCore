using System;
using System.Collections.Concurrent;

namespace HexCore
{
    public class ObjectPool<T>
    {
        private readonly Func<T> _createObject;
        private readonly ConcurrentBag<T> _objects;
        private readonly Func<T, T> _returnObject;

        public ObjectPool(Func<T> createObject, Func<T, T> returnObject)
        {
            _createObject = createObject ?? throw new ArgumentNullException(nameof(createObject));
            _returnObject = returnObject ?? throw new ArgumentNullException(nameof(returnObject));
            _objects = new ConcurrentBag<T>();
        }

        public T Get()
        {
            return _objects.TryTake(out var item) ? item : _createObject();
        }

        public void Return(T item)
        {
            _returnObject(item);
            _objects.Add(item);
        }

        public void Preallocate(int amountOfObjects)
        {
            for (var i = 0; i < amountOfObjects; i++)
            {
                var item = _createObject();
                _objects.Add(item);
            }
        }
    }
}