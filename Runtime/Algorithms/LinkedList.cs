using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System;

namespace KazegamesKit
{
    public class LinkedListNode<T> : IDisposable
    {
        public T Data { get; set; }

        public LinkedListNode<T> Next { get; set; }
        public LinkedListNode<T> Prev { get; set; }

        
        public void Dispose()
        {
            Data = default(T);
            Next = null;
            Prev = null;
        }
    }


    public class LinkedList<T>
    {
        private LinkedListNode<T> _front;
        private LinkedListNode<T> _back;
        private LinkedListNode<T> _freeNodes;

        public int Size
        {
            get
            {
                int s = 0;
                LinkedListNode<T> n = _front;
                while (n != null)
                {
                    s++;
                    n = n.Next;
                }

                return s;
            }
        }

        public LinkedListNode<T> Front { get { return _front; } }

        public LinkedListNode<T> Back { get { return _back; } }

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

            using(var e = src.GetEnumerator())
            {
                while(e.MoveNext())
                {
                    PushBack(e.Current);
                }
            }
        }

        public bool IsEmpty()
        {
            return _front == null;
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
            node.Data = val;
            PushBackNode(node);
        }

        public void PushFront(T val)
        {
            LinkedListNode<T> node = NewNode();
            node.Data = val;
            PushFrontNode(node);
        }

        public T PopBack()
        {
            if (IsEmpty())
                throw new InvalidOperationException("LinkedList is empty.");

            T data = _back.Data;
            PopBackNode();

            return data;
        }

        public T PopFront()
        {
            if (IsEmpty())
                throw new InvalidOperationException("LinkedList is empty.");

            T data = _front.Data;
            PopFrontNode();

            return data;
        }

        public bool Remove(T val)
        {
            var node = FindNode(val);
            if(node != null)
            {
                RemoveNode(node);
            }

            return false;
        }

        public LinkedListNode<T> FindNode(T val)
        {
            LinkedListNode<T> find = _front;
            while(find != null)
            {
                if (ReferenceEquals(find.Data, val))
                    return find;

                find = find.Next;
            }

            return null;
        }

        public void PushBackNode(LinkedListNode<T> node)
        {
            if(_back != null)
            {
                _back.Next = node;
                node.Prev = _back;
            }
            else
            {
                _front = node;
            }

            _back = node;
        }

        public void PushFrontNode(LinkedListNode<T> node)
        {
            if(_front != null)
            {
                _front.Prev = node;
                node.Next = _front;
            }
            else
            {
                _back = node;
            }

            _front = node;
        }

        public void PopBackNode()
        {
            if (IsEmpty())
                throw new InvalidOperationException("LinkedList is empty.");

            if (_front.Next == null)
            {
                FreeNode(_front);
                _front = _back = null;
            }
            else
            {
                LinkedListNode<T> node = _back;
                _back.Prev.Next = null;
                _back = _back.Prev;

                FreeNode(node);
            }
        }

        public void PopFrontNode()
        {
            if (IsEmpty())
                throw new InvalidOperationException("LinkedList is empty.");

            if (_back.Prev == null)
            {
                FreeNode(_back);
                _front = _back = null;
            }
            else
            {
                LinkedListNode<T> node = _front;
                _front.Next.Prev = null;
                _front = _front.Next;

                FreeNode(node);
            }
        }

        public bool RemoveNode(LinkedListNode<T> node)
        {
            LinkedListNode<T> find = _front;
            while(find != null)
            {
                if(find == node)
                {
                    if(node == _front)
                    {
                        PopFrontNode();
                    }
                    else if(node == _back)
                    {
                        PopBackNode();
                    }
                    else
                    {
                        node.Next.Prev = node.Prev;
                        node.Prev.Next = node.Next;
                        FreeNode(node);
                    }

                    return true;
                }
                else
                {
                    find = find.Next;
                }
            }

            return false;
        }

        public bool InsertBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (newNode == null)
                throw new ArgumentNullException(nameof(newNode));

            LinkedListNode<T> find = _front;
            while(find != null)
            {
                if(find == node)
                {
                    newNode.Next = find;
                    newNode.Prev = find.Prev;
                    find.Prev = newNode;

                    return true;
                }
                else
                {
                    find = find.Next;
                }
            }

            return false;
        }

        public bool InsertAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (newNode == null)
                throw new ArgumentNullException(nameof(newNode));

            LinkedListNode<T> find = _front;
            while (find != null)
            {
                if (find == node)
                {
                    newNode.Prev = find;
                    newNode.Next = find.Next;
                    find.Next = newNode;

                    return true;
                }
                else
                {
                    find = find.Next;
                }
            }

            return false;
        }


        public LinkedListNode<T> NewNode()
        {
            if (_freeNodes == null)
            {
                return new LinkedListNode<T>();
            }
            else
            {
                LinkedListNode<T> node = _freeNodes;
                _freeNodes = _freeNodes.Next;
                node.Dispose();

                return node;
            }
        }

        public void FreeNode(LinkedListNode<T> node)
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
                while (fNode.Next != null)
                    fNode = fNode.Next;

                fNode.Next = node;
            }
        }
    }
}
