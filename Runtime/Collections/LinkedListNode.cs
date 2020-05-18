using System;

namespace KazegamesKit.Collections
{
    [System.Serializable]
    public class LinkedListNode<T> : IDisposable
    {
        public T data;
        public LinkedListNode<T> next;
        public LinkedListNode<T> prev;


        public void Dispose()
        {
            this.data = default(T);
            this.next = null;
            this.prev = null;
        }
    }
}
