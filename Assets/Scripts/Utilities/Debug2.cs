using UnityEngine;

namespace SpeedTypingGame
{
    public class Debug2 : Debug
    {
        public static void Log(object message, string group)
        {
            Log($"[{group}] {message}");
        }

        public static void Log(object message, Object context, string group)
        {
            Log($"[{group}] {message}", context);
        }

        public static void LogWarning(object message, string group)
        {
            LogWarning($"[{group}] {message}");
        }

        public static void LogWarning(object message, Object context, string group)
        {
            LogWarning($"[{group}] {message}", context);
        }

        public static void LogError(object message, string group)
        {
            LogError($"[{group}] {message}");
        }

        public static void LogError(object message, Object context, string group)
        {
            LogError($"[{group}] {message}", context);
        }
    }
}