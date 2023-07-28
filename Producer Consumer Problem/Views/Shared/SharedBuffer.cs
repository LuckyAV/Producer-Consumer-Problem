using System.Collections.Generic;
using System.Threading;

namespace Producer_Consumer_Problem.Views.Shared
{
    public class SharedBuffer
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly Queue<int> _buffer = new Queue<int>();
        private bool _producerFinished = false;


        public bool IsFull => _buffer.Count >= 10;
        public bool IsEmpty => _buffer.Count == 0;

        public void AddToBuffer(int value)
        {
            _semaphore.Wait();
            try
            {
                if (IsFull)
                    throw new InvalidOperationException("Buffer is full.");

                _buffer.Enqueue(value);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public int RemoveFromBuffer()
        {
            _semaphore.Wait();
            try
            {
                if (IsEmpty)
                    throw new InvalidOperationException("Buffer is empty.");

                return _buffer.Dequeue();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        // Expose the internal semaphore
        public SemaphoreSlim GetSemaphore()
        {
            return _semaphore;
        }

        public void SetProducerFinished()
        {
            _producerFinished = true;
        }

        public bool IsProducerFinished()
        {
            return _producerFinished;
        }

    }
}
