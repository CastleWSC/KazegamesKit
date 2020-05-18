using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KazegamesKit
{
    public class UnloadSceneTask : UTask
    {
        public static UnloadSceneTask CreateTask(string sceneName, bool autoStart = true)
        {
            UnloadSceneTask newTask = new UnloadSceneTask(sceneName);
            if (autoStart) newTask.Start();

            return newTask;
        }

        public static UnloadSceneTask CreateTask(int sceneIndex, bool autoStart = true)
        {
            UnloadSceneTask newTask = new UnloadSceneTask(sceneIndex);
            if (autoStart) newTask.Start();

            return newTask;
        }


        private AsyncOperation _op;

        public string SceneName { get; private set; }
        public int SceneIndex { get; private set; }
        public float Progress { get; private set; }
        public AsyncOperation Operation { get { return _op; } }

        public event Action<UnloadSceneTask> onUnloaded;

        private UnloadSceneTask(string sceneName)
        {
            this.SceneIndex = -1;
            this.SceneName = sceneName;
        }

        private UnloadSceneTask(int sceneIndex)
        {
            this.SceneIndex = sceneIndex;
        }

        public override void OnTriggerEvents()
        {
            base.OnTriggerEvents();

            if (state == ETaskState.Finished)
            {
                DebugEx.Log(ELogType.System, $"LoadSceneTask: Scene is loaded, name= {SceneName}, index= {SceneIndex}.");
                onUnloaded?.Invoke(this);
            }
        }

        public override void Start()
        {
            if (state != ETaskState.Ready)
            {
                Debug.LogError($"Scene isn't ready to start, name= {SceneName}, index= {SceneIndex}.");
            }
            else
            {
                Progress = 0;
                state = ETaskState.Running;

                Singleton.GetInstance<TaskManager>().AddTask(this);

                if (SceneIndex >= 0)
                {
                    _op = SceneManager.UnloadSceneAsync(SceneIndex);
                }
                else
                {
                    _op = SceneManager.UnloadSceneAsync(SceneName);
                }
            }
        }

        public override void Tick()
        {
            if (_op == null)
            {
                error = new Exception("The AsyncOperation is null.");
                return;
            }

            if (state == ETaskState.Running)
            {
                if (_op.isDone)
                {
                    Progress = 1.0f;
                    state = ETaskState.Finished;
                }
                else
                {
                    Progress = _op.progress;
                }
            }
        }
    }
}
