using System;

namespace KazegamesKit
{
    public class Queue<T>
    {

        private Array<T> _array;

        public int Size { get { return _array.Length; } }


        public Queue()
        {
            _array = new Array<T>(10);
        }

        public Queue(int n)
        {
            _array = new Array<T>(n);
        }

        public bool IsEmpty()
        {
            return _array.IsEmpty();
        }

        public void Clear()
        {
            _array.Clear();
        }

        public void Push(T val)
        {
            _array.Push(val);
        }

        public T Pop()
        {
            if (_array.IsEmpty())
                throw new InvalidOperationException("Queue is empty.");

            T result = _array.Front();
            _array.Erase(0);

            return result;
        }

        public T Peek()
        {
            if (_array.IsEmpty())
                throw new InvalidOperationException("Queue is empty.");

            return _array.Front();
        }
    }
}
