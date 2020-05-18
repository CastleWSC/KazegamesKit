using System;

namespace KazegamesKit
{
    public abstract class UTask
    {
        public enum ETaskState
        {
            Ready,
            Running,
            Paused,
            Canceled,
            Finished,
            Error
        }

        
        public ETaskState state { get; internal set; }

        private Exception _error;
        public Exception error
        {
            get 
            {
                return _error; 
            }

            protected set
            {
                _error = value;
                state = ETaskState.Error;
            }
        }


        public event Action<UTask> onFinished;


        public abstract void Start();
        public abstract void Tick();

        public UTask()
        {
            state = ETaskState.Ready;
        }

        public void Cancel()
        {
            state = ETaskState.Canceled;
        }

        public void Pause()
        {
            if(state == ETaskState.Running)
            {
                state = ETaskState.Paused;
            }
        }

        public void UnPause()
        {
            if(state == ETaskState.Paused)
            {
                state = ETaskState.Running;
            }
        }

        public virtual void OnTriggerEvents()
        {
            onFinished?.Invoke(this);
        }
    }
}
