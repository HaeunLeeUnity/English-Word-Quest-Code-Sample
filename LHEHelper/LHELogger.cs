using UnityEngine;

namespace LHEPackage.Helper
{
    public static class LHELogger
    {
        public static void Log(object message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(message);
#endif
        }

        public static void LogWarning(object message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning(message);
#endif
        }

        public static void LogError(object message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogError(message);
#endif
        }

        public static void Assert(bool condition, string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Assert(condition, message);
#endif
        }

        public static void Assert(bool condition)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Assert(condition);
#endif
        }
    }
}