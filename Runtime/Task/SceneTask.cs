using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KazegamesKit
{
    public class SceneTask : UTask
    {
        public enum EOperation
        {
            LoadSingle,
            LoadAdditive,
            Unload
        }

        public static SceneTask CreateTask(string sceneName, EOperation operation, bool autoStart = true)
        {
            SceneTask newTask = new SceneTask(sceneName, operation);
            if (autoStart) newTask.Start();

            return newTask;
        }

        public static SceneTask CreateTask(int buildIndex, EOperation operation, bool autoStart = true)
        {
            SceneTask newTask = new SceneTask(buildIndex, operation);
            if (autoStart) newTask.Start();

            return newTask;
        }


        private AsyncOperation _async;

        private EOperation _op;
        private float _progress;

        public string SceneName { get; private set; }
        public int BuildIndex { get; private set; }
        public EOperation Operation { get { return _op; } }
        public float Progress { get { return _progress; } }


        private SceneTask(string sceneName, EOperation op)
        {
            _op = op;
            this.BuildIndex = -1;
            this.SceneName = sceneName;
        }

        private SceneTask(int buildIndex, EOperation op)
        {
            _op = op;
            this.BuildIndex = buildIndex;
        }

        public void SetSceneActive()
        {
            if (state == ETaskState.Completed)
            {
                _async.allowSceneActivation = true;
            }
        }

        public override void Start()
        {
            if (state != ETaskState.Ready)
            {

                Debug.LogWarning("SceneTask: Scene is loaded.");
            }
            else
            {
                _progress = 0;
                state = ETaskState.Running;

                Singleton.GetInstance<TaskManager>().AddTask(this);

                if (_op == EOperation.Unload)
                {
                    _async = SceneManager.UnloadSceneAsync(BuildIndex > 0 ? BuildIndex : SceneName);
                }
                else
                {
                    _async = SceneManager.LoadSceneAsync(BuildIndex > 0 ? BuildIndex : SceneName, _op == EOperation.LoadSingle ? LoadSceneMode.Single : LoadSceneMode.Additive);

                    _async.allowSceneActivation = false;
                }
            }
        }

        public override void Tick()
        {
            if (_async == null)
            {
                error = new NullReferenceException("The AsyncOperation is null.");
                return;
            }

            if (state == ETaskState.Running)
            {
                if (_async.isDone)
                {
                    _progress = 1.0f;
                    state = ETaskState.Completed;
                }
                else
                {
                    _progress = _async.progress < 0.9f ? _async.progress : 1.0f;

                    if (_progress >= 1.0f)
                    {
                        state = ETaskState.Completed;
                    }
                }
            }
        }
    }
}
