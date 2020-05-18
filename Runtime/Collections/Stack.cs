﻿using System;

namespace KazegamesKit.Collections
{
    public class Stack<T>
    {
        private LinkedList<T> _buffer;

        public int Size { get { return _buffer.Size; } }

        public bool Empty { get { return _buffer.Empty; } }


        public Stack()
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
                throw new InvalidOperationException("Stack is empty.");

            T data = _buffer.FrontNode.data;
            _buffer.PopFront();
            
            return data;
        }

        public T Peek()
        {
            if(_buffer.Empty)
                throw new InvalidOperationException("Stack is empty.");

            return _buffer.FrontNode.data;
        }
    }
}
