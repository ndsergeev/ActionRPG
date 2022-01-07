
using System.Diagnostics;

namespace Main.Core.Console
{
    using Debug = UnityEngine.Debug;
    
    /// <summary>
    /// UnityEngine.Debug.[Log, LogWarning, LogError] wrapper to exclude from builds.
    /// It is still better to remove from 
    /// </summary>
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
        
        [Conditional(ConditionalString)]
        public static void Assert(bool condition, string format)
            => Debug.Assert(condition, format);
        
        [Conditional(ConditionalString)]
        public static void Assert(bool condition, string format, params object[] args)
            => Debug.AssertFormat(condition, format, args);
        
        // add more Debug-related methods if needed
    }
}
