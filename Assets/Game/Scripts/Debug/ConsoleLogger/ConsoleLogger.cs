using UnityEngine;

#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;
using System.Reflection;
#endif

public class ConsoleLogger : MonoBehaviour
{

#if UNITY_EDITOR

    [Header("Global bool")]
    public bool enableLogging = true;

    [Header("Logger objects list")]
    public List<ConsoleLoggerCaller> loggerObjects;

    public static ConsoleLogger Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            loggerObjects = new List<ConsoleLoggerCaller>();
        }
    }

    public static void Log(string msg)
    {
        if (!Instance.enableLogging) return;

        StackTrace stackTrace = new StackTrace();
        StackFrame callingFrame = stackTrace.GetFrame(1);
        MethodBase callingMethod = callingFrame.GetMethod();
        Type callingType = callingMethod.DeclaringType;

        try
        {
            var caller = Instance.loggerObjects.First(o => o.name == callingType?.Name);
            if (caller.enabledLogging)
                UnityEngine.Debug.Log(string.Format("[{0}] {1}", callingType?.Name, msg));
        }
        catch (System.Exception)
        {
            UnityEngine.Debug.Log(string.Format("[{0}] {1}", callingType?.Name, msg));
        }
    }

#endif
}

#if UNITY_EDITOR

[System.Serializable]
public struct ConsoleLoggerCaller
{
    public string name;
    public bool enabledLogging;

    public ConsoleLoggerCaller(string name)
    {
        this.name = name;
        enabledLogging = true;
    }
}

#endif