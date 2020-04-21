using System;

namespace KazegamesKit
{
    public class Queue<T>
    {

        private LinkedList<T> _list;

        public int Size { get { return _list.Size; } }


        public Queue()
        {
            _list = new LinkedList<T>();
        }

        public bool IsEmpty()
        {
            return _list.IsEmpty();
        }

        public void Clear()
        {
            _list.Clear();
        }

        public void Push(T val)
        {
            _list.PushBack(val);
        }

        public T Pop()
        {
            if (_list.IsEmpty())
                throw new InvalidOperationException("Queue is empty.");

            return _list.PopFront();
        }

        public T Peek()
        {
            if (_list.IsEmpty())
                throw new InvalidOperationException("Queue is empty.");

            return _list.Front.Data;
        }
    }
}
