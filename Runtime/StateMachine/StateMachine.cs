using System;
using System.Collections.Generic;
using UnityEngine;

namespace KazegamesKit
{
    public sealed class StateMachine<T>
    {
        private T _context;

        private State<T> _activeState;
        public State<T> State { get { return _activeState; } }

        private Dictionary<Type, State<T>> _states = new Dictionary<Type, State<T>>();

        public event Action onStateChanged;

        public StateMachine(T context, State<T> initialState)
        {
            _context = context;

            AddState(initialState);
            
            _activeState = initialState;
            _activeState.OnEnter();
        }

        public void AddState(State<T> state)
        {
            state.SetMachineAndContext(this, _context);
            _states.Add(state.GetType(), state);
        }

        public void Update(float deltaTime)
        {
            if(_activeState != null)
            {
                _activeState.Update(deltaTime);
            }
        }

        public void GoToState<K>() where K : State<T>
        {
            Type key = typeof(K);

            if (_activeState != null && _activeState.GetType() == key)
                return;


            State<T> state;
            if(_states.TryGetValue(key, out state))
            {
                if (_activeState != null)
                    _activeState.OnExit();

                _activeState = state;

                if(_activeState != null)
                    _activeState.OnEnter();

                onStateChanged?.Invoke();
            }
        }
    }
}