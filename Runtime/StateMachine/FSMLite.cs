using System;

namespace KazegamesKit
{
    public class FSMLite<T>
    {

        private Queue<T> _queue;
        private T _activeState;

        public T State { get { return _activeState; } }

        public event Action<T> onStateEnter;
        public event Action<T> onStateExit;

        public FSMLite(T initialState)
        {
            Reset(initialState);
        }

        public void Reset(T state)
        {
            _activeState = state;

            if (_queue == null)
                _queue = new Queue<T>();
            else
                _queue.Clear();

        }

        public void GoToState(T state)
        {
            _queue.Push(state);
        }

        public void UpdateState()
        {
            if(!_queue.IsEmpty())
            {
                T next = _queue.Pop();

                if (ReferenceEquals(next, _activeState))
                    return;

                onStateExit?.Invoke(_activeState);

                _activeState = next;

                onStateEnter?.Invoke(_activeState);
            }
        }
    }
}
