using System;
using System.Collections;

namespace KazegamesKit
{
    public class CoroutineTask : UTask
    {

        public static CoroutineTask CreateTask(IEnumerator task, bool autoStart = true)
        {
            CoroutineTask newTask = new CoroutineTask(task);
            if (autoStart) newTask.Start();

            return newTask;
        }

        IEnumerator _task;
        
        private CoroutineTask(IEnumerator task)
        {
            _task = task;
        }

        internal IEnumerator Routine()
        {
            if(_task == null)
            {
                error = new Exception("The task is null");
            }
            else
            {
                while (state < ETaskState.Canceled)
                {
                    if (state == ETaskState.Paused)
                    {
                        yield return Yielders.EndOfFrame;
                    }
                    else
                    {
                        if (_task.MoveNext())
                        {
                            yield return _task.Current;
                        }
                        else
                        {
                            state = ETaskState.Finished;
                        }
                    }
                }
            }
        }

        public override void Start()
        {
            TaskManager mgr = Singleton.GetInstance<TaskManager>();

            if(state == ETaskState.Running || state == ETaskState.Paused)
            {
                mgr.StopCoroutineTask(this);
                mgr.RemoveTask(this);
            }

            state = ETaskState.Running;
            mgr.AddTask(this);
            mgr.StartCoroutineTask(this);
        }

        public override void Tick()
        {
            
        }
    }
}
