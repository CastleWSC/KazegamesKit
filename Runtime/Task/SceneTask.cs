using System;
using System.Collections;
using System.Net.NetworkInformation;
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


        private AsyncOperation _async;

        private string _name;
        private EOperation _op;
        private float _progress;

        public string SceneName { get { return _name; } }
        public EOperation Operation { get { return _op; } }
        public float Progress { get { return _progress; } }

        private SceneTask(string name, EOperation op)
        {
            _name = name;
            _op = op;
        }

        public void SetSceneActive()
        {
            if(state == ETaskState.Completed)
            {
                _async.allowSceneActivation = true;
            }
        }

        public override void Start()
        {
            if(state != ETaskState.Ready)
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
                    _async = SceneManager.UnloadSceneAsync(_name);
                }
                else
                {
                    _async = SceneManager.LoadSceneAsync(_name, _op == EOperation.LoadSingle ? LoadSceneMode.Single : LoadSceneMode.Additive);

                    _async.allowSceneActivation = false;
                }
            }
        }

        public override void Tick()
        {
            if(_async == null)
            {
                error = new NullReferenceException("The AsyncOperation is null.");
                return;
            }

            if(state == ETaskState.Running)
            {
                if(_async.isDone)
                {
                    _progress = 1.0f;
                    state = ETaskState.Completed;
                }
                else
                {
                    _progress = _async.progress < 0.9f ? _async.progress : 1.0f;

                    if(_progress >= 1.0f)
                    {
                        state = ETaskState.Completed;
                    }
                }
            }
        }
    }
}
