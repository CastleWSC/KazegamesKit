using UnityEngine;

namespace KazegamesKit
{
    public abstract class SingletonMono : MonoBehaviour, ISingleton
    {
        public abstract void Init();
        public abstract void Dispose();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
