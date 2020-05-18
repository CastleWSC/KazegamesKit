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


        #region Management
        private static Dictionary<Type, ISingleton> _singletons = new Dictionary<Type, ISingleton>();

        public static ISingleton GetInstance(Type type)
        {
            ISingleton singleton = null;
            
            if(!_singletons.TryGetValue(type, out singleton))
            {
                singleton = CreateInstance(type);
                
                singleton.Init();

                _singletons.Add(type, singleton);

                DebugEx.Log(ELogType.System, $"Singleton: Create instance, type= {type.Name}.");
            }

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

                if (type.IsSameOrSubClass(typeof(SingletonMono)))
                    UnityEngine.Object.Destroy((singleton as SingletonMono).gameObject);

                DebugEx.Log(ELogType.System, $"Singleton: Destroy instance, type= {type.Name}.");
            }
        }

        public static void DestroyInstance<T>() where T : class, ISingleton
        {
            DestroyInstance(typeof(T));
        }

        private static ISingleton CreateInstance(Type type)
        {
            ISingleton singleton = null;

            if(type.IsSameOrSubClass(typeof(Singleton)))
            {
                singleton = Activator.CreateInstance(type) as Singleton;
            }
            else if(type.IsSameOrSubClass(typeof(SingletonMono)))
            {
                var component = UnityEngine.Object.FindObjectOfType(type);

                if (UnityEngine.Object.FindObjectsOfType(type).Length > 1)
                    throw new Exception($"Singleton: There should never be more than one SingletonMono instacne, type= {type.Name}.");

                if(component == null)
                {
                    GameObject go = new GameObject();
                    go.name = "[Singelton] " + type.Name;
                    go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

                    component = go.AddComponent(type);
                }

                singleton = component as SingletonMono;
            }

            return singleton;
        }
        #endregion
    }
}
