using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KazegamesKit
{
    public class LoadSceneTask : UTask
    {

        public static LoadSceneTask CreateTask(string sceneName, LoadSceneMode mode, bool autoStart = true)
        {
            LoadSceneTask newTask = new LoadSceneTask(sceneName, mode);
            if (autoStart) newTask.Start();

            return newTask;
        }

        public static LoadSceneTask CreateTask(int sceneIndex, LoadSceneMode mode, bool autoStart = true)
        {
            LoadSceneTask newTask = new LoadSceneTask(sceneIndex, mode);
            if (autoStart) newTask.Start();

            return newTask;
        }


        private AsyncOperation _op;

        public LoadSceneMode Mode { get; private set; }
        public string SceneName { get; private set; }
        public int SceneIndex { get; private set; }
        public float Progress { get; private set; }
        public AsyncOperation Operation { get { return _op; } }

        public event Action<LoadSceneTask> onLoaded;


        private LoadSceneTask(string sceneName, LoadSceneMode mode)
        {
            this.SceneIndex = -1;
            this.SceneName = sceneName;
            this.Mode = mode;
        }

        private LoadSceneTask(int sceneIndex, LoadSceneMode mode)
        {
            this.SceneIndex = sceneIndex;
            this.Mode = mode;
        }


        public override void OnTriggerEvents()
        {
            base.OnTriggerEvents();

            if (state == ETaskState.Finished)
            {
                DebugEx.Log(ELogType.System, $"LoadSceneTask: Scene is loaded, name= {SceneName}, index= {SceneIndex}.");
                onLoaded?.Invoke(this);
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
                    _op = SceneManager.LoadSceneAsync(SceneIndex, Mode);
                }
                else
                {
                    _op = SceneManager.LoadSceneAsync(SceneName, Mode);
                }
            }
        }

        public override void Tick()
        {
            if(_op == null)
            {
                error = new Exception("The AsyncOperation is null.");
                return;
            }
            
            if(state == ETaskState.Running)
            {
                if(_op.isDone)
                {
                    Progress = 1.0f;
                    state = ETaskState.Finished;
                }
                else
                {
                    Progress = _op.progress < 0.9f ? _op.progress : 1.0f;
                    
                    if (Progress >= 1.0f) 
                        state = ETaskState.Finished;
                }
            }
        }
    }
}
