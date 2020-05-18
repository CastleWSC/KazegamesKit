using KazegamesKit.Collections;
using System.Collections;

namespace KazegamesKit
{
    public class TaskManager : SingletonMono
    {
        private Array<UTask> _tasks;

        internal void AddTask(UTask task)
        {
            _tasks.Push(task);
        }

        internal void RemoveTask(UTask task)
        {
            _tasks.Remove(task);
        }

        internal void StartCoroutineTask(CoroutineTask task)
        {
            StartCoroutine(task.Routine());
        }

        internal void StopCoroutineTask(CoroutineTask task)
        {
            StopCoroutine(task.Routine());
        }


        private void ProcessFinishedTask(UTask task)
        {
            if(task.state == UTask.ETaskState.Error)
            {
                UnityEngine.Debug.LogException(task.error);
                return;
            }

            task.OnTriggerEvents();
        }

        public override void Init()
        {
            _tasks = new Array<UTask>(16);
            _tasks.growSize = 16;
        }

        public override void Dispose()
        {
            _tasks = null;
        }

        private void Update()
        {
            if(!_tasks.Empty)
            {
                for(int i=0; i<_tasks.Length; i++)
                {
                    if(_tasks[i].state == UTask.ETaskState.Running)
                    {
                        _tasks[i].Tick();
                    }

                    if(_tasks[i].state == UTask.ETaskState.Canceled ||
                        _tasks[i].state == UTask.ETaskState.Finished ||
                        _tasks[i].state == UTask.ETaskState.Error)
                    {
                        ProcessFinishedTask(_tasks[i]);
                        _tasks.Erase(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
    }
}
