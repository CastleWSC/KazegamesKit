using System;

namespace KazegamesKit.Collections
{
    public class Queue<T>
    {
        private LinkedList<T> _buffer;

        public int Size { get { return _buffer.Size; } }

        public bool Empty { get { return _buffer.Empty; } }

        public Queue()
        {
            _buffer = new LinkedList<T>();
        }

        public void Clear()
        {
            _buffer.Clear();
        }

        public void Push(T val)
        {
            _buffer.PushBack(val);
        }

        public T Pop()
        {
            if (_buffer.Empty)
                throw new InvalidOperationException("Queue is empty.");

            T data = _buffer.BackNode.data;
            _buffer.PopBack();

            return data;
        }

        public T Peek()
        {
            if (_buffer.Empty)
                throw new InvalidOperationException("Queue is empty.");

            return _buffer.BackNode.data;
        }
    }
}
