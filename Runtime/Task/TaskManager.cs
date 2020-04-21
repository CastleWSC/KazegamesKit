using System;
using System.Collections;
using System.Diagnostics;

namespace KazegamesKit
{
    public class TaskManager : SingletonMono
    {
        Array<UTask> _runningTasks;


        internal void AddTask(UTask task)
        {
            _runningTasks.Push(task);
        }

        internal void RemoveTask(UTask task)
        {
            _runningTasks.Remove(task);
        }

        internal void StartCoroutineTask(IEnumerator routine)
        {
            StartCoroutine(routine);
        }

        internal void StopCoroutineTask(IEnumerator routine)
        {
            StopCoroutine(routine);
        }

        void ProcessFinishedTask(UTask task)
        {
            if(task.state == UTask.ETaskState.Error)
            {
                UnityEngine.Debug.LogException(task.error);
                return;
            }

            task.InvokeFinishedEvent();
        }

        public override void Init()
        {
            _runningTasks = new Array<UTask>(32);
            _runningTasks.SetGrowSize(10);
        }

        public override void Dispose()
        {
            _runningTasks = null;
        }

        private void Update()
        {
            if(!_runningTasks.IsEmpty())
            {
                for(int i=0; i<_runningTasks.Length; )
                {
                    if(_runningTasks[i].state == UTask.ETaskState.Running)
                    {
                        _runningTasks[i].Tick();
                    }

                    if(_runningTasks[i].state > UTask.ETaskState.Paused) // process finished task
                    {
                        ProcessFinishedTask(_runningTasks[i]);
                        _runningTasks.Erase(i);
                    }
                    else // process next task
                    {
                        i++;
                    }
                }
            }
        }
    }
}
