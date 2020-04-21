using UnityEngine;

namespace KazegamesKit
{
    public interface IRandomNumberGenerator
    {
        void Init(int seed);
        float GetNext();
    }

    public static class RandomEx
    {
        class RandomNumberGenerator : IRandomNumberGenerator
        {
            public void Init(int seed)
            {
                UnityEngine.Random.InitState(seed);
            }

            public float GetNext()
            {
                return UnityEngine.Random.value;
            }
        }

        public static IRandomNumberGenerator RNG { get; set; }

        static RandomEx()
        {
            RNG = new RandomNumberGenerator();
        }

        public static float GetValue()
        {
            return RNG.GetNext();
        }

        public static float GetRange(float min, float max)
        {
            return GetValue() * (max - min) + min;
        }

        public static int GetRange(int min, int max)
        {
            return (int)(GetValue() * (max - min) + min);
        }

        public static Vector2 GetUnitVector2()
        {
            return new Vector2(GetValue(), GetValue());
        }

        public static Vector3 GetUnitVector3()
        {
            return new Vector3(GetValue(), GetValue(), GetValue());
        }

        public static Quaternion GetRotation()
        {
            return Quaternion.Euler(GetUnitVector3());
        }
    }
}
