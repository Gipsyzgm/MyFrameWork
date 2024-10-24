using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///简单的事件处理系统，即通过添加Event和移除Event来实现。事件名须添加到对应的EventConst类内
/// </summary>
using UnityEngine;
using System;
using System.Collections.Generic;

public class EventMgr : MonoSingleton<EventMgr>
{
    public delegate void Action0();
    public delegate void Action1<T1>(T1 p1);
    public delegate void Action2<T1, T2>(T1 p1, T2 p2);
    public delegate void Action3<T1, T2, T3>(T1 p1, T2 p2, T3 p3);
    public delegate void Action4<T1, T2, T3, T4>(T1 p1, T2 p2, T3 p3, T4 p4);
    public delegate void Action5<T1, T2, T3, T4, T5>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5);
    public delegate void Action6<T1, T2, T3, T4, T5, T6>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6);

    private Dictionary<string, Delegate> delegates = new Dictionary<string, Delegate>();

    // 泛型方法用于添加事件监听器
    public void AddEventListener(string key, Action0 action)
    {
        AddEventListenerInternal(key, action);
        delegates[key] = (Action0)delegates[key] + action;
    }

    public void AddEventListener<T1>(string key, Action1<T1> action)
    {
        AddEventListenerInternal(key, action);
        delegates[key] = (Action1<T1>)delegates[key] + action;
    }

    public void AddEventListener<T1, T2>(string key, Action2<T1, T2> action)
    {
        AddEventListenerInternal(key, action);
        delegates[key] = (Action2<T1, T2>)delegates[key] + action;
    }

    public void AddEventListener<T1, T2, T3>(string key, Action3<T1, T2, T3> action)
    {
        AddEventListenerInternal(key, action);
        delegates[key] = (Action3<T1, T2, T3>)delegates[key] + action;
    }

    public void AddEventListener<T1, T2, T3, T4>(string key, Action4<T1, T2, T3, T4> action)
    {
        AddEventListenerInternal(key, action);
        delegates[key] = (Action4<T1, T2, T3, T4>)delegates[key] + action;
    }

    public void AddEventListener<T1, T2, T3, T4, T5>(string key, Action5<T1, T2, T3, T4, T5> action)
    {
        AddEventListenerInternal(key, action);
        delegates[key] = (Action5<T1, T2, T3, T4, T5>)delegates[key] + action;
    }

    public void AddEventListener<T1, T2, T3, T4, T5, T6>(string key, Action6<T1, T2, T3, T4, T5, T6> action)
    {
        AddEventListenerInternal(key, action);
        delegates[key] = (Action6<T1, T2, T3, T4, T5, T6>)delegates[key] + action;
    }

    // 内部方法用于实际添加监听器
    private void AddEventListenerInternal(string key, Delegate action)
    {
        if (!delegates.ContainsKey(key))
        {
            delegates.Add(key, null);
        }

        Delegate d = delegates[key];
        if (d != null && d.GetType() != action.GetType())
        {
            throw new ArgumentException(string.Format(
                "Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}",
                key, d.GetType().Name, action.GetType().Name));
        }
    }

    // 泛型方法用于移除事件监听器
    public void RemoveEventListener(string key, Action0 action)
    {
        if (delegates.ContainsKey(key))
        {
            delegates[key] = (Action0)delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1>(string key, Action1<T1> action)
    {
        if (delegates.ContainsKey(key))
        {
            delegates[key] = (Action1<T1>)delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1, T2>(string key, Action2<T1, T2> action)
    {
        if (delegates.ContainsKey(key))
        {
            delegates[key] = (Action2<T1, T2>)delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1, T2, T3>(string key, Action3<T1, T2, T3> action)
    {
        if (delegates.ContainsKey(key))
        {
            delegates[key] = (Action3<T1, T2, T3>)delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1, T2, T3, T4>(string key, Action4<T1, T2, T3, T4> action)
    {
        if (delegates.ContainsKey(key))
        {
            delegates[key] = (Action4<T1, T2, T3, T4>)delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1, T2, T3, T4, T5>(string key, Action5<T1, T2, T3, T4, T5> action)
    {
        if (delegates.ContainsKey(key))
        {
            delegates[key] = (Action5<T1, T2, T3, T4, T5>)delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1, T2, T3, T4, T5, T6>(string key, Action6<T1, T2, T3, T4, T5, T6> action)
    {
        if (delegates.ContainsKey(key))
        {
            delegates[key] = (Action6<T1, T2, T3, T4, T5, T6>)delegates[key] - action;
        }
    }

    // 泛型方法用于触发事件
    public void InvokeEvent(string key)
    {
        Delegate d = delegates[key];
        if (d is Action0)
        {
            ((Action0)d)();
        }
    }

    public void InvokeEvent<T1>(string key, T1 arg1)
    {
        Delegate d = delegates[key];
        if (d is Action1<T1>)
        {
            ((Action1<T1>)d)(arg1);
        }
    }

    public void InvokeEvent<T1, T2>(string key, T1 arg1, T2 arg2)
    {
        Delegate d = delegates[key];
        if (d is Action2<T1, T2>)
        {
            ((Action2<T1, T2>)d)(arg1, arg2);
        }
    }

    public void InvokeEvent<T1, T2, T3>(string key, T1 arg1, T2 arg2, T3 arg3)
    {
        Delegate d = delegates[key];
        if (d is Action3<T1, T2, T3>)
        {
            ((Action3<T1, T2, T3>)d)(arg1, arg2, arg3);
        }
    }

    public void InvokeEvent<T1, T2, T3, T4>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        Delegate d = delegates[key];
        if (d is Action4<T1, T2, T3, T4>)
        {
            ((Action4<T1, T2, T3, T4>)d)(arg1, arg2, arg3, arg4);
        }
    }

    public void InvokeEvent<T1, T2, T3, T4, T5>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        Delegate d = delegates[key];
        if (d is Action5<T1, T2, T3, T4, T5>)
        {
            ((Action5<T1, T2, T3, T4, T5>)d)(arg1, arg2, arg3, arg4, arg5);
        }
    }

    public void InvokeEvent<T1, T2, T3, T4, T5, T6>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        Delegate d = delegates[key];
        if (d is Action6<T1, T2, T3, T4, T5, T6>)
        {
            ((Action6<T1, T2, T3, T4, T5, T6>)d)(arg1, arg2, arg3, arg4, arg5, arg6);
        }
    }

    // 清理所有事件监听器
    public void ClearAllListeners()
    {
        delegates.Clear();
    }
}