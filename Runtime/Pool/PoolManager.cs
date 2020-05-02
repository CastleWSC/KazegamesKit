using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KazegamesKit.Pool
{
    public class PoolManager : SingletonMono
    {
        private Array<PrefabPool> _poolArray;
        private Dictionary<string, PrefabPool> _pools;

        private static PoolManager _instance = null;
        
        internal static PoolManager instance
        {
            get
            {
                if (_instance == null)
                    _instance = Singleton.GetInstance<PoolManager>();

                return _instance;
            }
        }
        

        public static void AddPrefabPool(PrefabPool pool)
        {
            if (pool == null)
                throw new ArgumentNullException(nameof(pool));

            if (pool.prefab == null)
                throw new NullReferenceException("Prefab is null.");

            if(instance._pools.ContainsKey(pool.prefab.name))
            {
                DebugEx.Log(ELogType.Error, $"Cannot add the prefab pool because there is already a pool with name ({pool.prefab.name}).");
                return;
            }
            else
            {
                pool.Init();

                instance._poolArray.Push(pool);
                instance._pools.Add(pool.prefab.name, pool);
            }
        }

        public static void RemovePrefabPool(PrefabPool pool)
        {
            if(instance._pools.ContainsKey(pool.prefab.name))
            {
                instance._pools.Remove(pool.prefab.name);
                instance._poolArray.Remove(pool);

                pool.Dispose();
            }
        }

        public static GameObject Spawn(GameObject go, Vector3 position, Quaternion rotation)
        {
            if(instance._pools.ContainsKey(go.name))
            {
                return Spawn(go.name, position, rotation);
            }
            else
            {
                DebugEx.Log(ELogType.Warning, $"PoolManager: Attempted to spawn {go.name} gameobject but there is no pool for it.");
                
                return null;
            }

        }

        public static GameObject Spawn(string name, Vector3 position, Quaternion rotation)
        {
            PrefabPool pool = null;
            if (instance._pools.TryGetValue(name, out pool))
            {
                GameObject newGo = pool.Spawn();

                if (newGo != null)
                {
                    Transform newTransform = newGo.transform;

                    if (newTransform as RectTransform)
                    {
                        newTransform.SetParent(null, false);
                    }
                    else
                    {
                        newTransform.parent = null;
                    }

                    newTransform.position = position;
                    newTransform.rotation = rotation;

                    newGo.SetActive(true);
                }

                return newGo;
            }
            else
            {
                DebugEx.Log(ELogType.Warning, $"PoolManager: Attempted to spawn {name} gameObject but there is no pool for it.");
                
                return null;
            }
        }

        public static void Despawn(GameObject go)
        {
            if (go == null)
                return;

            if(instance._pools.ContainsKey(go.name))
            {
                instance._pools[go.name].Despawn(go);

                if (go.transform as RectTransform)
                    go.transform.SetParent(instance.transform, false);
                else
                    go.transform.parent = instance.transform;
            }
            else
            {
                Destroy(go);

                DebugEx.Log(ELogType.Warning, $"PoolManager: Attempted to despawn {go.name} gameObject but there is no pool for it.");
            }
        }


        void ActiveSceneChanged(Scene scene1, Scene scene2)
        {
            if (scene1.name == null)
                return;

            for(int i=_poolArray.Length-1; i>=0; i--)
            {
                if(!_poolArray[i].dontDestoryOnLoad)
                {
                    RemovePrefabPool(_poolArray[i]);
                }
            }
        }

        public override void Init()
        {
            _poolArray = new Array<PrefabPool>(10);
            _pools = new Dictionary<string, PrefabPool>();

            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }

        public override void Dispose()
        {
            for (int i = _poolArray.Length - 1; i >= 0; i--)
            {
                RemovePrefabPool(_poolArray[i]);
            }

            _instance = null;
            _poolArray = null;
            _pools = null;
        }
    }
}
