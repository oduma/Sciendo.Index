using System.Collections.Concurrent;

namespace Sciendo.Indexer.Agent.Service
{
    public class FixedSizedQueue<T>
    {
        ConcurrentQueue<T> q = new ConcurrentQueue<T>();

        public int Limit { get; private set; }

        public FixedSizedQueue(int limit)
        {
            Limit = limit;
        }

        public void Enqueue(T obj)
        {
            q.Enqueue(obj);
            lock (this)
            {
                T overflow;
                while (q.Count > Limit && q.TryDequeue(out overflow));
            }
        }

        public T[] GetAllInQueue()
        {
            lock (this)
            {
                return q.ToArray();
            }
        }
    }
}
