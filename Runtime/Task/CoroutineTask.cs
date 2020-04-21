using System;
using System.Collections;
using System.Threading.Tasks;

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

        IEnumerator Routine()
        {
            if (_task == null)
            {
                error = new NullReferenceException("The task is null.");
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
                            state = ETaskState.Completed;
                        }
                    }
                }

            }
        }

        public override void Start()
        {
            TaskManager taskManager = Singleton.GetInstance<TaskManager>();

            if(state == ETaskState.Running || state == ETaskState.Paused)
            {
                taskManager.StopCoroutineTask(Routine());
                taskManager.RemoveTask(this);
            }


            state = ETaskState.Running;

            taskManager.AddTask(this);
            taskManager.StartCoroutineTask(Routine());
        }

        public override void Tick()
        {
            
        }
    }
}
