using System;
using KazegamesKit.Collections;

namespace KazegamesKit
{
    public class FSMLite<T>
    {
        private Queue<T> _stateQueue;

        public T state { get; private set; }


        public event Action<FSMLite<T>> onStateEnter;
        public event Action<FSMLite<T>> onStateExit;


        public FSMLite(T initialState)
        {
            _stateQueue = new Queue<T>();
            
            Reset(initialState);
        }

        public void Reset(T state)
        {
            this.state = state;

            if (!_stateQueue.Empty) _stateQueue.Clear();
        }

        public void GoToState(T state)
        {
            _stateQueue.Push(state);
        }

        public T UpdateState()
        {
            if(!_stateQueue.Empty)
            {
                T next = _stateQueue.Pop();

                if (!next.SafeEquals(state))
                {
                    onStateExit?.Invoke(this);

                    state = next;

                    onStateEnter?.Invoke(this);
                }
            }

            return state;
        }
    }
}
