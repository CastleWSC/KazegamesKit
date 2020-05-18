using System;
using System.Collections;
using System.Collections.Generic;

namespace KazegamesKit.Collections
{
    [System.Serializable]
    public class Array<T>
    {
       
        [UnityEngine.SerializeField]private T[] _buffer;

        private int _len;
        private int _cap;
        private int _gsize;

        public int Length { get { return _len; } }

        public bool Empty { get { return _len == 0; } }

        public int growSize
        {
            get { return _gsize; }
            set { _gsize = value; }
        }

        public int capacity
        {
            get { return _cap; }
        }

        public virtual T this[int index]
        {
            get
            {
                if (index < 0 || index >= _len)
                    throw new IndexOutOfRangeException($"Index out of range, index= {index}, length= {_len}.");

                return _buffer[index];
            }
            set
            {
                if (index < 0 || index >= _len)
                    throw new IndexOutOfRangeException($"Index out of range, index= {index}, length= {_len}.");

                _buffer[index] = value;
            }
        }

        public T First
        {
            get
            {
                if (_len == 0)
                    throw new InvalidOperationException("Array is empty.");

                return _buffer[0];
            }
        }

        public T Last
        {
            get
            {
                if (_len == 0)
                    throw new InvalidOperationException("Array is empty.");

                return _buffer[_len - 1];
            }
        }


        private Array()
        {
            _len = 0;
            _cap = 0;
            _gsize = 1;
            _buffer = null;
        }

        public Array(int capacity) : this()
        {
            _cap = capacity;
            if(_cap > 0)
            {
                _buffer = new T[_cap];
            }
        }

        public Array(int n, T val) : this()
        {
            _len = _cap = n;
            if(_cap > 0)
            {
                _buffer = new T[_cap];
                
                for (int i = 0; i < _len; i++)
                    _buffer[i] = val;
            }
        }

        public Array(IList<T> src) : this()
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if(src.Count > 0)
            {
                _len = _cap = src.Count;
                _buffer = new T[_cap];
                
                for (int i = 0; i < _len; i++)
                    _buffer[i] = src[i];
            }
        }

        public void Clear()
        {
            _len = 0;

            for (int i = 0; i < _cap; i++)
                _buffer[i] = default;
        }

        public void Push(T val)
        {
            if (_len >= _cap)
                GrowBuffer(_gsize);

            _buffer[_len] = val;
            ++_len;
        }

        public T Pop()
        {
            if (_len == 0)
                throw new InvalidOperationException("Array is empty.");

            T o = _buffer[_len - 1];
            _buffer[_len - 1] = default;

            --_len;

            return o;
        }

        public void Insert(int index, T val)
        {
            Insert(index, 1, val);
        }

        public void Insert(int index, int n, T val)
        {
            if (index < 0 || index >= _len)
                throw new IndexOutOfRangeException($"Index out of range, index= {index}, length= {_len}.");

            if(n > 0)
            {
                int new_len = _len + n;
                if (new_len >= _cap) GrowBuffer(n);

                int d = new_len - 1;
                int s = _len - 1;

                for (; s > index - 1; --s, --d) _buffer[d] = _buffer[s];
                for (d = index; d < index + n; d++) _buffer[d] = val;

                _len = new_len;
            }
        }

        public void Erase(int index)
        {
            Erase(index, index);
        }

        public void Erase(int begin, int end)
        {
            if (begin < 0 || begin >= _len)
                throw new IndexOutOfRangeException($"Index out of range, begin= {begin}, length= {_len}.");

            if (end < 0 || end >= _len)
                throw new IndexOutOfRangeException($"Index out of range, end= {end}, length= {_len}.");

            int n = end - begin + 1;
            int d = begin;
            int s = end + 1;

            for (; s < _len; ++s, ++d) _buffer[d] = _buffer[s];
            for (int i = _len - 1; i >= _len - n; i--) _buffer[i] = default(T);

            _len -= n;
        }

        public bool Exists(T val)
        {
            return IndexOf(val) >= 0;
        }

        public int IndexOf(T val)
        {
            if (val == null)
                throw new ArgumentNullException(nameof(val));

            if(_buffer != null && _buffer.Length > 0)
            {
                for(int i=0; i<_len; i++)
                {
                    if(_buffer[i].SafeEquals(val))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public bool Remove(T val)
        {
            int indx = IndexOf(val);
            if(indx >= 0)
            {
                Erase(indx);
                return true;
            }

            return false;
        }

        public T[] ToArray()
        {
            T[] result = null;

            if(_len > 0)
            {
                result = new T[_len];
                
                for (int i = 0; i < _len; i++) 
                    result[i] = _buffer[i];
            }

            return result;
        }

        public void Shuffle()
        {
            int n = _len;
            while(n > 1)
            {
                n--;
                int k = RandomEx.GetRange(0, n + 1);
                T temp = _buffer[k];
                _buffer[k] = _buffer[n];
                _buffer[n] = temp;
            }
        }

        public void Reverse()
        {
            int start = 0;
            int end = _len - 1;
            
            while(end > start)
            {
                T temp = _buffer[start];
                _buffer[start] = _buffer[end];
                _buffer[end] = temp;

                start++;
                end--;
            }
        }

        public void Sort(System.Comparison<T> comparer)
        {
            bool changed = true;

            while(changed)
            {
                changed = false;

                for(int i=1; i<_len; i++)
                {
                    if(comparer.Invoke(_buffer[i-1], _buffer[i]) > 0)
                    {
                        T temp = _buffer[i];
                        _buffer[i] = _buffer[i - 1];
                        _buffer[i - 1] = temp;
                        changed = true;
                    }
                }
            }
        }

        private void GrowBuffer(int n)
        {
            if(n > 0)
            {
                T[] temp = new T[_cap + n];

                if(_buffer != null && _buffer.Length > 0)
                    Array.Copy(_buffer, temp, _len);

                _cap += n;
                _buffer = temp;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if(_len > 0)
            {
                for(int i=0; i<_len; i++)
                {
                    yield return _buffer[i];
                }
            }
        }
    }
}
