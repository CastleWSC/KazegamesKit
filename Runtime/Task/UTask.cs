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
            Completed,
            Error
        }

        public ETaskState state { get; internal set; }

        private Exception _error;
        public Exception error
        {
            get { return _error; }

            internal set
            {
                _error = value;
                state = ETaskState.Error;
            }
        }

        public event Action<UTask> onFinished;

        public abstract void Tick();

        public abstract void Start();

        public void Cancel()
        {
            state = ETaskState.Canceled;
        }

        public void Pause()
        {
            if (state == ETaskState.Running)
                state = ETaskState.Paused;
        }

        public void UnPause()
        {
            if (state == ETaskState.Paused)
                state = ETaskState.Paused;
        }

        internal void InvokeFinishedEvent()
        {
            onFinished?.Invoke(this);
        }
    }
}
