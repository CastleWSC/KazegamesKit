using System;

namespace KazegamesKit
{
    public class Stack<T>
    {
        private LinkedList<T> _list;

        public int Size { get { return _list.Size; } }


        public Stack()
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
            if (IsEmpty())
                throw new InvalidOperationException("Stack is empty.");

            return _list.PopBack();
        }

        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Stack is empty.");

            return _list.Back.Data;
        }
    }
}