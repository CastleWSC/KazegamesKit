using System;

namespace KazegamesKit
{
    public abstract class State<T> 
    {
        protected T _context;
        protected StateMachine<T> _stateMachine;
        

        public State()
        {

        }

        internal void SetMachineAndContext(StateMachine<T> stateMachine, T context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }

        public abstract void Update(float deltaTime);

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
    }
}
