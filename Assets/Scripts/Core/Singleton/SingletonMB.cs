
namespace Core
{
    using UnityEngine;
    
    public abstract class SingletonMB<T> : MonoBehaviour where T : SingletonMB<T>
    {
        /*
         * ToDo: make it lazy
         */
        public static T instance { get; private set; }

        protected virtual void Awake()
        {
            InitSingleton();
        }

        private void InitSingleton()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = (T) this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
