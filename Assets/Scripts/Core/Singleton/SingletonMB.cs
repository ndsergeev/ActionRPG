using System;

namespace Main.Core
{
    using UnityEngine;

    using Console;
    
    public abstract class SingletonMB<T> : MonoBehaviour where T : SingletonMB<T>
    {
        private static readonly Lazy<T> LazyInstance = new Lazy<T>(InitSingleton);

        public static T instance => LazyInstance.Value;

        private static T InitSingleton()
        {
            var ownerGameObject = new GameObject($"{typeof(T).Name}");
            var singletonComponent = ownerGameObject.AddComponent<T>();
            DontDestroyOnLoad(ownerGameObject);
            
            EditorLogger.Log($"<color=green>{ownerGameObject.name} created!</color>");
            
            return singletonComponent;
        }
    }
}
