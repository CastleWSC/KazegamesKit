using System;
using System.Collections.Generic;
using UnityEngine;

namespace KazegamesKit
{
    public interface ISingleton : IDisposable
    {
        void Init();
    }

    public abstract class Singleton : ISingleton
    {
        public abstract void Init();
        public abstract void Dispose();



        static Dictionary<Type, ISingleton> _singletons = new Dictionary<Type, ISingleton>();

        public static ISingleton GetInstance(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            ISingleton singleton = null;
            if(!_singletons.TryGetValue(type, out singleton))
            {
                singleton = CreateInstance(type);

                singleton.Init();

                _singletons.Add(type, singleton);

                DebugEx.Log(ELogType.System, $"Singleton: Create instance, type= {type.Name}.");
            }

            if (singleton == null)
                throw new NullReferenceException();

            return singleton;
        }

        public static T GetInstance<T>() where T : class, ISingleton
        {
            return GetInstance(typeof(T)) as T;
        }

        public static void DestroyInstance(Type type)
        {
            ISingleton singleton = null;
            if(_singletons.TryGetValue(type, out singleton))
            {
                singleton.Dispose();

                _singletons.Remove(type);

                if (type.IsSubclassOf(typeof(SingletonMono)))
                    UnityEngine.Object.Destroy(singleton as SingletonMono);

                DebugEx.Log(ELogType.System, $"Singleton: Destroy instance, type= {type.Name}.");
            }
        }

        public static void DestroyInstance<T>() where T : ISingleton
        {
            DestroyInstance(typeof(T));
        }

        static ISingleton CreateInstance(Type type)
        {
            ISingleton singleton = null;

            if(type.IsSubclassOf(typeof(Singleton)))
            {
                singleton = Activator.CreateInstance(type) as Singleton;
            }
            else if(type.IsSubclassOf(typeof(SingletonMono)))
            {
                UnityEngine.Object com = UnityEngine.Object.FindObjectOfType(type);

                if(UnityEngine.Object.FindObjectsOfType(type).Length > 1)
                    throw new Exception($"Singleton: There should never be more than one SingletonMono instacne, type= {type.Name}.");

                if(com == null)
                {
                    GameObject go = new GameObject();
                    go.name = "[Singleton] " + type.Name;
                    go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

                    com = go.AddComponent(type);
                }

                singleton = com as SingletonMono;
            }
            else
            {
                throw new Exception();
            }

            return singleton;
        }
    }
}
