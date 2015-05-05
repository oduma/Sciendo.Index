using System.Collections.Concurrent;

namespace Sciendo.Music.Agent.Service.Monitoring
{
    public class FixedSizedQueue<T>
    {
        readonly ConcurrentQueue<T> _q = new ConcurrentQueue<T>();

        public int Limit { get; private set; }

        public FixedSizedQueue(int limit)
        {
            Limit = limit;
        }

        public void Enqueue(T obj)
        {
            _q.Enqueue(obj);
            lock (this)
            {
                T overflow;
                while (_q.Count > Limit && _q.TryDequeue(out overflow))
                {
                }
            }
        }

        public T[] GetAllInQueue()
        {
            lock (this)
            {
                return _q.ToArray();
            }
        }
    }
}
