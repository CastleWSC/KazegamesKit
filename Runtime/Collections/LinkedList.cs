using System;
using System.Collections;
using System.Collections.Generic;

namespace KazegamesKit.Collections
{
    [System.Serializable]
    public class LinkedList<T>
    {
        private LinkedListNode<T> _front;
        private LinkedListNode<T> _back;
        private LinkedListNode<T> _freeNodes;

        public LinkedListNode<T> FrontNode { get { return _front; } }

        public LinkedListNode<T> BackNode { get { return _back; } }

        public int Size
        {
            get
            {
                int n = 0;
                LinkedListNode<T> node = _front;
                
                while(node != null)
                {
                    ++n;
                    node = node.next;
                }

                return n;
            }
        }

        public bool Empty { get { return _front == null; } }


        public LinkedList()
        {
            _front = null;
            _back = null;
            _freeNodes = null;
        }

        public LinkedList(int n, T val) : this()
        {
            for (int i = 0; i < n; i++)
                PushBack(val);
        }

        public LinkedList(IEnumerable<T> src) : this()
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            using (var e = src.GetEnumerator())
            {
                while(e.MoveNext())
                {
                    PushBack(e.Current);
                }
            }
        }

        public void Clear()
        {
            _front = null;
            _back = null;
            _freeNodes = null;
        }

        public void PushBack(T val)
        {
            LinkedListNode<T> node = NewNode();
            node.data = val;
            PushBack(node);
        }

        public void PushFront(T val)
        {
            LinkedListNode<T> node = NewNode();
            node.data = val;
            PushFront(node);
        }

        public void PushBack(LinkedListNode<T> node)
        {
            if (node == null)
                return;

            if(_back != null)
            {
                _back.next = node;
                node.prev = _back;
            }
            else
            {
                _front = node;
            }

            _back = node;
        }

        public void PushFront(LinkedListNode<T> node)
        {
            if (node == null)
                return;

            if (_front != null)
            {
                _front.prev = node;
                node.next = _front;
            }
            else
            {
                _back = node;
            }

            _front = node;
        }

        public void PopBack()
        {
            if (_back != null)
            {
                if(_front.next == null)
                {
                    FreeNode(_front);
                    _front = _back = null;
                }
                else
                {
                    LinkedListNode<T> node = _back;
                    _back.prev.next = null;
                    _back = _back.prev;

                    FreeNode(node);
                }
            }
        }

        public void PopFront()
        {
            if(_front != null)
            {
                if(_back.prev == null)
                {
                    FreeNode(_back);
                    _front = _back = null;
                }
                else
                {
                    LinkedListNode<T> node = _front;
                    _front.next.prev = null;
                    _front = _front.next;

                    FreeNode(node);
                }
            }
        }

        public bool Remove(T val)
        {
            LinkedListNode<T> find = _front;
            while(find != null)
            {
                if (find.data.SafeEquals(val))
                {
                    if (find == _front)
                    {
                        PopFront();
                    }
                    else if (find == _back)
                    {
                        PopBack();
                    }
                    else
                    {
                        find.next.prev = find.prev;
                        find.prev.next = find.next;
                        FreeNode(find);
                    }

                    return true;
                }
                else
                {
                    find = find.next;
                }
            }

            return false;
        }

        public bool Remove(LinkedListNode<T> node)
        {
            LinkedListNode<T> find = _front;
            while (find != null)
            {
                if(find == node)
                {
                    if(node == _front)
                    {
                        PopFront();
                    }
                    else if(node == _back)
                    {
                        PopBack();
                    }
                    else
                    {
                        node.next.prev = node.prev;
                        node.prev.next = node.next;
                        FreeNode(node);
                    }

                    return true;
                }
                else
                {
                    find = find.next;
                }
            }

            return false;
        }

        public LinkedListNode<T> Find(T val)
        {
            LinkedListNode<T> find = _front;
            while(find != null)
            {
                if(find.data.SafeEquals(val))
                {
                    return find;
                }
                else
                {
                    find = find.next;
                }
            }

            return null;
        }


        private LinkedListNode<T> NewNode()
        {
            if(_freeNodes == null)
            {
                return new LinkedListNode<T>();
            }
            else
            {
                LinkedListNode<T> node = _freeNodes;

                _freeNodes = _freeNodes.next;

                node.Dispose();
                
                return node;
            }
        }

        private void FreeNode(LinkedListNode<T> node)
        {
            if (node == null)
                return;

            node.Dispose();

            if(_freeNodes == null)
            {
                _freeNodes = node;
            }
            else
            {
                LinkedListNode<T> fNode = _freeNodes;
                while(fNode.next != null)
                {
                    fNode = fNode.next;
                }

                fNode.next = node;
            }
        }
    }
}
