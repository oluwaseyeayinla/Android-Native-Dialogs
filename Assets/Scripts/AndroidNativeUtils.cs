using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AndroidNativeUtils
{
    #if UNITY_ANDROID
    static Dictionary<string, AndroidJavaObject> pool = new Dictionary<string, AndroidJavaObject>();
    #endif

    public static void Call(string className, string methodName, params object[] args)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        Log(className, methodName);

        using (AndroidJavaObject bridge = new AndroidJavaObject(className))
        {
            bridge.Call(methodName, args);
        }
    }

    public static int CallReturnInt(string className, string methodName, params object[] args)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return int.MinValue;
        }

        Log(className, methodName);

        using (AndroidJavaObject bridge = new AndroidJavaObject(className))
        {
            return bridge.Call<int>(methodName, args);
        }
    }

    public static string CallReturnString(string className, string methodName, params object[] args)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return string.Empty;
        }

        Log(className, methodName);

        using (AndroidJavaObject bridge = new AndroidJavaObject(className))
        {
            return bridge.Call<string>(methodName, args);
        }
    }

    public static void CallStatic(string className, string methodName, params object[] args)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        Log(className, methodName);

        using (AndroidJavaObject bridge = new AndroidJavaObject(className))
        {
            bridge.CallStatic(methodName, args);
        }
    }

    public static int CallStaticReturnInt(string className, string methodName, params object[] args)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return int.MinValue;
        }

        Log(className, methodName);

        using (AndroidJavaObject bridge = new AndroidJavaObject(className))
        {
            return bridge.CallStatic<int>(methodName, args);
        }
    }

    public static string CallStaticReturnString(string className, string methodName, params object[] args)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return string.Empty;
        }

        Log(className, methodName);

        using (AndroidJavaObject bridge = new AndroidJavaObject(className))
        {
            return bridge.CallStatic<string>(methodName, args);
        }
    }

    public static void CallUsingUnityActivityOnUIThread(string className, string methodName, params object[] args)
    {
        #if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        Log(className, methodName);

        try
        {
            AndroidJavaObject bridge;
            if (pool.ContainsKey(className))
            {
                bridge = pool[className];
            }
            else
            {
                bridge = new AndroidJavaObject(className);
                pool.Add(className, bridge);
            }

            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject act = jc.GetStatic<AndroidJavaObject>("currentActivity");

            act.Call("runOnUiThread", new AndroidJavaRunnable(() => { bridge.Call(methodName, args); }));
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
        #endif
    }

    public static void CallStaticUsingUnityActivityOnUIThread(string className, string methodName, params object[] args)
    {
        #if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        Log(className, methodName);

        try
        {
            AndroidJavaObject bridge;
            if (pool.ContainsKey(className))
            {
                bridge = pool[className];
            }
            else
            {
                bridge = new AndroidJavaObject(className);
                pool.Add(className, bridge);
            }

            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject act = jc.GetStatic<AndroidJavaObject>("currentActivity");

            act.Call("runOnUiThread", new AndroidJavaRunnable(() => { bridge.CallStatic(methodName, args); }));
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
        #endif
    }

    public static void Log(string className, string methodName)
    {
        Debug.Log("AndroidNative: Using proxy for class: " + className + " method:" + methodName);
    }
}
