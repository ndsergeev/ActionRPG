
namespace Main.Core.Console
{
    using System.Diagnostics;
    using Debug = UnityEngine.Debug;
    
    public static class EditorLogger
    {
        private const string ConditionalString = "UNITY_EDITOR";

        [Conditional(ConditionalString)]
        public static void Log(object obj)
            => Debug.Log(obj);
        
        [Conditional(ConditionalString)]
        public static void Log(string format, params object[] args)
            => Debug.LogFormat(format, args);
        
        [Conditional(ConditionalString)]
        public static void LogWarning(object obj)
            => Debug.LogWarning(obj);
        
        [Conditional(ConditionalString)]
        public static void LogWarning(string format, params object[] args)
            => Debug.LogWarningFormat(format, args);
        
        [Conditional(ConditionalString)]
        public static void LogError(object obj)
            => Debug.LogError(obj);
        
        [Conditional(ConditionalString)]
        public static void LogError(string format, params object[] args)
            => Debug.LogErrorFormat(format, args);
        
        // add more Debug-related methods if needed
    }
}
