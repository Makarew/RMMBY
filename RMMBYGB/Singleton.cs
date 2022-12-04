using System;

namespace RMMBYGB
{
    public struct Singleton
    {
        public static T GetInstance<T>()
        {
            return Singleton<T>.GetInstance();
        }

        public static void SetInstance<T>(T instance)
        {
            Singleton<T>.SetInstance(instance);
        }

        public static bool HasInstance<T>()
        {
            return Singleton<T>.HasInstance();
        }
    }

    public struct Singleton<T>
    {
        private static T StaticInstance { get; set; }

        public T Instance
        {
            get
            {
                return Singleton<T>.StaticInstance;
            }
        }

        public Singleton(T instance)
        {
            Singleton<T>.SetInstance(instance);
        }

        public static void SetInstance(T instance)
        {
            Singleton<T>.StaticInstance = instance;
        }

        public static T GetInstance()
        {
            return Singleton<T>.StaticInstance;
        }

        public static bool HasInstance()
        {
            return Singleton<T>.StaticInstance != null;
        }

        public static implicit operator T(Singleton<T> singleton)
        {
            return Singleton<T>.GetInstance();
        }
    }
}
