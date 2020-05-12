using System;
using System.Collections;
using System.Collections.Generic;

namespace KazegamesKit
{
    public class Array<T> : IEnumerable<T>
    {
        private T[] _data;

        private int _len; // length
        private int _cap; // capacity
        private int _gsize; // grow size

        public int Length { get { return _len; } }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _len)
                    throw new IndexOutOfRangeException($"Array: Index= {index}, Length= {Length}.");

                return _data[index];
            }

            set
            {
                if (index < 0 || index >= _len)
                    throw new IndexOutOfRangeException($"Array: Index= {index}, Length= {Length}.");

                _data[index] = value;
            }
        }


        private Array()
        {
            _len = 0;
            _cap = 0;
            _gsize = 1;
            _data = null;
        }

        public Array(int n) : this()
        {
            _cap = n;
            if (_cap > 0)
            {
                _data = new T[_cap];
            }
        }

        public Array(int n, T val) : this()
        {
            _len = _cap = n;
            if (_cap > 0)
            {
                _data = new T[_cap];
                for (int i = 0; i < _len; i++) _data[i] = val;
            }
        }

        public Array(T[] src) : this()
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (src.Length == 0)
                throw new InvalidOperationException("Source is empty.");

            _len = _cap = src.Length;
            _data = new T[_cap];
            for (int i = 0; i < _len; i++) _data[i] = src[i];
        }


        public bool IsEmpty()
        {
            return _len == 0;
        }

        public T Front()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Array is empty.");

            return _data[0];
        }

        public T Back()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Array is empty.");

            return _data[_len - 1];
        }

        public void Clear()
        {
            _len = 0;
            _cap = 0;
            _data = null;
        }

        public int GetGrowSize()
        {
            return _gsize;
        }

        public void SetGrowSize(uint n)
        {
            _gsize = (int)Math.Max(1, n);
        }

        public int GetCapacity()
        {
            return _cap;
        }

        public void Push(T val)
        {
            if (_len >= _cap)
                Grow(_gsize);

            _data[_len] = val;
            ++_len;
        }

        public T Pop()
        {
            if(_len > 0)
            {
                T obj = _data[_len - 1];
                Erase(_len - 1);

                return obj;
            }

            return default(T);
        }

        public void Grow(int n)
        {
            if (n <= 0) return;

            T[] new_data = new T[_cap + n];
            if (_data != null && _len > 0) Array.Copy(_data, new_data, _len);

            _cap += n;
            _data = new_data;
        }

        public void Insert(int index, T val)
        {
            Insert(index, 1, val);
        }

        public void Insert(int index, int n, T val)
        {
            if (index < 0 || index >= _len)
                throw new IndexOutOfRangeException($"Array: Index= {index}, Length= {_len}.");

            if (n <= 0) return;

            int new_len = _len + n;
            if (new_len >= _cap) Grow(n);

            int d = new_len - 1;
            int s = _len - 1;

            for (; s > index - 1; --s, --d) _data[d] = _data[s];
            for (d = index; d < index + n; d++) _data[d] = val;

            _len = new_len;
        }

        public void Erase(int index)
        {
            Erase(index, index);
        }

        public void Erase(int begin, int end)
        {
            if (begin < 0 || end < 0 || begin >= _len || end >= _len || begin > end)
                throw new IndexOutOfRangeException($"Array: Begin= {begin}, End= {end}, Length= {_len}.");

            int n = end - begin + 1;
            int d = begin;
            int s = end + 1;

            for (; s < _len; ++s, ++d) _data[d] = _data[s];
            for (int i = _len - 1; i >= _len - n; i--) _data[i] = default(T);

            _len -= n;
        }

        public int IndexOf(T val)
        {
            if(_data!= null && _len > 0)
            {
                for (int i = 0; i < _len; i++)
                    if (ReferenceEquals(_data[i], val))
                        return i;
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
            if(_data != null && _len > 0)
            {
                T[] result = new T[_len];
                for (int i = 0; i < _len; i++) result[i] = _data[i];

                return result;
            }

            return null;
        }

        public List<T> ToList()
        {
            if (_data != null && _len > 0)
            {
                List<T> result = new List<T>(_len);
                for (int i = 0; i < _len; i++) result.Add(_data[i]);

                return result;
            }

            return null;
        }

        public void ForEach(Action<T> act)
        {
            if(_data != null && _len > 0)
            {
                for(int i=0; i<_len; i++)
                {
                    act?.Invoke(_data[i]);
                }
            }
        }

        public void Shuffle()
        {
            int n = _len;
            while(n>1)
            {
                n--;
                int k = RandomEx.GetRange(0, n + 1);
                T tmp = this[k];
                this[k] = this[n];
                this[n] = tmp;
            }
        }

        public void Reverse()
        {
            int start = 0;
            int end = _len-1;

            while(end > start)
            {
                T tmp = this[start];
                this[start] = this[end];
                this[end] = tmp;
                start++;
                end--;
            }
        }

        class ArrayEnumerator : IEnumerator<T>
        {
            int _index;
            Array<T> _array;

            public T Current { get { return _array[_index]; } }

            public ArrayEnumerator(Array<T> array)
            {
                Reset();
                _array = array;
            }

            public bool MoveNext()
            {
                ++_index;
                return (_index >= 0) && (_index < _array.Length);
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose()
            {
                _array = null;
            }

            object IEnumerator.Current { get; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ArrayEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ArrayEnumerator(this);
        }
    }
}
