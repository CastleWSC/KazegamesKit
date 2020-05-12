using System;

namespace KazegamesKit
{
    public class Stack<T>
    {
        private Array<T> _array;

        public int Size { get { return _array.Length; } }


        public Stack()
        {
            _array = new Array<T>(10);
        }

        public Stack(int n)
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
            if (IsEmpty())
                throw new InvalidOperationException("Stack is empty.");

            return _array.Pop();
        }

        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Stack is empty.");

            return _array.Back();
        }
    }
}