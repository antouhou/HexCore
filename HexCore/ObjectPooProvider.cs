using System.Collections.Generic;

namespace HexCore
{
    public class ObjectPoolProvider
    {
        public ObjectPool<List<CellState>> CellStatesListPool;
        public ObjectPool<Dictionary<Coordinate3D, Coordinate3D>> CoordinateDictionaryPool;
        public ObjectPool<HashSet<Coordinate3D>> CoordinateHashSetPool;
        public ObjectPool<Dictionary<Coordinate3D, int>> CoordinateIntDictionaryPool;

        public ObjectPool<List<Coordinate3D>> CoordinateListPool;
        public ObjectPool<PriorityQueue<Coordinate3D>> CoordinateQueuePool;
        public ObjectPool<List<Graph.Fringe>> FringeListPool;
        public ObjectPool<List<Coordinate2D>> OffsetCoordinateListPool;

        public ObjectPoolProvider()
        {
            Init();
        }

        public static ObjectPool<TCollection> CreateCollectionPool<TCollection, TValue>(int objectToPreallocate = 0)
            where TCollection : ICollection<TValue>, new()
        {
            TCollection CreateCollection()
            {
                return new TCollection();
            }

            TCollection ReturnCollection(TCollection collection)
            {
                collection.Clear();
                return collection;
            }

            var pool = new ObjectPool<TCollection>(CreateCollection, ReturnCollection);

            if (objectToPreallocate > 0) pool.Preallocate(objectToPreallocate);

            return pool;
        }

        public static ObjectPool<Dictionary<TKey, TValue>> CreateDictionaryPool<TKey, TValue>()
        {
            Dictionary<TKey, TValue> CoordinateHashSetCreate()
            {
                return new Dictionary<TKey, TValue>();
            }

            Dictionary<TKey, TValue> CoordinateHashSetOnReturn(Dictionary<TKey, TValue> dict)
            {
                dict.Clear();
                return dict;
            }

            return new ObjectPool<Dictionary<TKey, TValue>>(CoordinateHashSetCreate, CoordinateHashSetOnReturn);
        }

        private void Init()
        {
            CoordinateHashSetPool = CreateCollectionPool<HashSet<Coordinate3D>, Coordinate3D>();
            CoordinateListPool = CreateCollectionPool<List<Coordinate3D>, Coordinate3D>();
            OffsetCoordinateListPool = CreateCollectionPool<List<Coordinate2D>, Coordinate2D>();
            FringeListPool = CreateCollectionPool<List<Graph.Fringe>, Graph.Fringe>();
            CellStatesListPool = CreateCollectionPool<List<CellState>, CellState>();

            CoordinateIntDictionaryPool = CreateDictionaryPool<Coordinate3D, int>();
            CoordinateDictionaryPool = CreateDictionaryPool<Coordinate3D, Coordinate3D>();

            PriorityQueue<Coordinate3D> CreatePriorityQueue()
            {
                return new PriorityQueue<Coordinate3D>();
            }

            PriorityQueue<Coordinate3D> ReturnCollection(PriorityQueue<Coordinate3D> collection)
            {
                collection.Clear();
                return collection;
            }

            CoordinateQueuePool = new ObjectPool<PriorityQueue<Coordinate3D>>(CreatePriorityQueue, ReturnCollection);
        }
    }
}