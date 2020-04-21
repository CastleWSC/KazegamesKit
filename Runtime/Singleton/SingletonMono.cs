using UnityEngine;

namespace KazegamesKit
{
    public abstract class SingletonMono : MonoBehaviour, ISingleton
    {
        public abstract void Init();
        public abstract void Dispose();


        public virtual bool isPersistent { get { return true; } }

        private void Awake()
        {
            if (isPersistent)
                DontDestroyOnLoad(gameObject);
        }
    }
}
