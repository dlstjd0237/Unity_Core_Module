using System;
using System.Diagnostics;
using UnityDebug = UnityEngine.Debug;

public static class Stdout
{
    public enum LogLevel
    {
        Verbose = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        Fatal = 5,
        None = 99,
    }

    public static LogLevel MinimumLevel { get; set; } = LogLevel.Info;
    public static bool ShowTimestamp { get; set; } = false;
    public static bool UseColors { get; set; } = true;

    public static event Action<LogLevel, string, string> OnLog;

    public static bool IsEnabled(LogLevel level) => level >= MinimumLevel;

    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogVerbose(string message, string tag = null, UnityEngine.Object context = null) =>
        Write(LogLevel.Verbose, tag, message, context);

    [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogDebug(string message, string tag = null, UnityEngine.Object context = null) =>
        Write(LogLevel.Debug, tag, message, context);

    public static void LogInfo(string message, string tag = null, UnityEngine.Object context = null) =>
        Write(LogLevel.Info, tag, message, context);

    public static void LogWarning(string message, string tag = null, UnityEngine.Object context = null) =>
        Write(LogLevel.Warning, tag, message, context);

    public static void LogError(string message, string tag = null, UnityEngine.Object context = null) =>
        Write(LogLevel.Error, tag, message, context);

    public static void LogFatal(string message, string tag = null, UnityEngine.Object context = null) =>
        Write(LogLevel.Fatal, tag, message, context);

    public static void LogException(Exception exception, string tag = null, UnityEngine.Object context = null)
    {
        if (exception == null || LogLevel.Error < MinimumLevel)
            return;
        Write(LogLevel.Error, tag, exception.ToString(), context);
    }

    public static void Log(LogLevel level, string message, string tag = null, UnityEngine.Object context = null) =>
        Write(level, tag, message, context);

    private static void Write(LogLevel level, string tag, string message, UnityEngine.Object context)
    {
        if (level < MinimumLevel)
            return;

        string formatted = Format(level, tag, message);

        switch (level)
        {
            case LogLevel.Warning:
                UnityDebug.LogWarning(formatted, context);
                break;
            case LogLevel.Error:
            case LogLevel.Fatal:
                UnityDebug.LogError(formatted, context);
                break;
            default:
                UnityDebug.Log(formatted, context);
                break;
        }

        OnLog?.Invoke(level, tag, message);
    }

    private static string Format(LogLevel level, string tag, string message)
    {
        string timestamp = ShowTimestamp ? $"{DateTime.Now:HH:mm:ss.fff} " : string.Empty;
        string levelLabel = UseColors
            ? $"<color={LevelColor(level)}>[{LevelLabel(level)}]</color>"
            : $"[{LevelLabel(level)}]";
        string tagPart = string.IsNullOrEmpty(tag) ? string.Empty : $"[{tag}]";
        return $"{timestamp}{levelLabel}{tagPart} {message}";
    }

    private static string LevelLabel(LogLevel level)
    {
        switch (level)
        {
            case LogLevel.Verbose: return "VERB";
            case LogLevel.Debug:   return "DBUG";
            case LogLevel.Info:    return "INFO";
            case LogLevel.Warning: return "WARN";
            case LogLevel.Error:   return "ERR ";
            case LogLevel.Fatal:   return "FATL";
            default:               return "?";
        }
    }

    private static string LevelColor(LogLevel level)
    {
        switch (level)
        {
            case LogLevel.Verbose: return "#888888";
            case LogLevel.Debug:   return "#80C0FF";
            case LogLevel.Info:    return "#FFFFFF";
            case LogLevel.Warning: return "#FFD580";
            case LogLevel.Error:   return "#FF8080";
            case LogLevel.Fatal:   return "#FF40C0";
            default:               return "#FFFFFF";
        }
    }
}
