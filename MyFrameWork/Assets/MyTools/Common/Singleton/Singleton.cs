using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Threading;

/// <summary>
/// 单例类模板
/// </summary>
public abstract class Singleton<T> where T : Singleton<T>, new()
{
    private static T instance = default(T);
    private static object s_objectLock = new object();
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                object obj;
                Monitor.Enter(obj = s_objectLock);
                try
                {
                    if (instance == null)
                    {
                       instance = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
                    }
                }
                finally
                {
                    Monitor.Exit(obj);
                }
            }

            return instance;
        }
    }
}