using UnityEngine;

namespace KazegamesKit.Pool
{
    [AddComponentMenu("Kazegames Kit/PrefabPool Helper")]
    public class PrefabPoolHelper : MonoBehaviour
    {
        [SerializeField]
        private PrefabPool[] pools;

        private void Start()
        {
            if(pools != null && pools.Length > 0)
            {
                
                for(int i=0; i<pools.Length; i++)
                {
                    PoolManager.AddPrefabPool(pools[i]);
                }
            }
        }
    }
}