using System;
using UnityEngine;

namespace KazegamesKit.Pool
{
    [System.Serializable]
    public class PrefabPool : IDisposable
    {

        public GameObject prefab;

        public int instancesToPrelocate = 1;
        public int instnacesToAllocate = 1;
        public int instancesMaxLimit = -1;
        public bool dontDestoryOnLoad = true;

        private Stack<GameObject> _pooling;
        private int _numOfSpawned = 0;


        public event Action<GameObject> onSpawned;
        public event Action<GameObject> onDespawned;
        

        public void Init()
        {
            _pooling = new Stack<GameObject>(instancesToPrelocate);
            AllocateGameObjects(instancesToPrelocate);
        }

        public GameObject Spawn()
        {
            GameObject go = Pop();

            if(go != null)
            { 
                onSpawned?.Invoke(go);
            }

            return go;
        }

        public void Despawn(GameObject go)
        {
            go.SetActive(false);

            _numOfSpawned--;
            _pooling.Push(go);

            onDespawned?.Invoke(go);
        }

        private void AllocateGameObjects(int n)
        {
            while(n > 0)
            {
                n--;
                
                GameObject go = GameObject.Instantiate(prefab);
                go.name = prefab.name;

                if (go.transform as RectTransform)
                    go.transform.SetParent(PoolManager.instance.transform, false);
                else
                    go.transform.parent = PoolManager.instance.transform;

                go.SetActive(false);
                _pooling.Push(go);
            }
        }

        private GameObject Pop()
        {
            if (instancesMaxLimit > 0 && _numOfSpawned >= instancesMaxLimit)
                return null;

            if(!_pooling.IsEmpty())
            {
                _numOfSpawned++;
                return _pooling.Pop();
            }

            AllocateGameObjects(instnacesToAllocate);

            return Pop();
        }

        public void Dispose()
        {
            while(!_pooling.IsEmpty())
            {
                var go = _pooling.Pop();

                GameObject.Destroy(go);
            }

            prefab = null;
            _pooling = null;
        }
    }
}
