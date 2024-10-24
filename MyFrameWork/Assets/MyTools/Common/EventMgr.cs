using System.Collections.Generic;
using System;
/// <summary>
///简单的事件处理系统，即通过添加Event和移除Event来实现。事件名须添加到对应的EventConst类内
/// </summary>
public class EventMgr : MonoSingleton<EventMgr>
{
    public delegate void Action0();

    public delegate void Action1<T1>(T1 p1);

    public delegate void Action2<T1, T2>(T1 p1, T2 p2);

    public delegate void Action3<T1, T2, T3>(T1 p1, T2 p2, T3 p3);

    public delegate void Action4<T1, T2, T3, T4>(T1 p1, T2 p2, T3 p3, T4 p4);

    public delegate void Action5<T1, T2, T3, T4, T5>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5);

    public delegate void Action6<T1, T2, T3, T4, T5, T6>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6);

    private readonly Dictionary<string, Delegate> _delegates = new Dictionary<string, Delegate>();

    // 泛型方法用于添加事件监听器
    public void AddEventListener(string key, Action0 action)
    {
        AddEventListenerInternal(key, action);
        _delegates[key] = (Action0)_delegates[key] + action;
    }

    public void AddEventListener<T1>(string key, Action1<T1> action)
    {
        AddEventListenerInternal(key, action);
        _delegates[key] = (Action1<T1>)_delegates[key] + action;
    }

    public void AddEventListener<T1, T2>(string key, Action2<T1, T2> action)
    {
        AddEventListenerInternal(key, action);
        _delegates[key] = (Action2<T1, T2>)_delegates[key] + action;
    }

    public void AddEventListener<T1, T2, T3>(string key, Action3<T1, T2, T3> action)
    {
        AddEventListenerInternal(key, action);
        _delegates[key] = (Action3<T1, T2, T3>)_delegates[key] + action;
    }

    public void AddEventListener<T1, T2, T3, T4>(string key, Action4<T1, T2, T3, T4> action)
    {
        AddEventListenerInternal(key, action);
        _delegates[key] = (Action4<T1, T2, T3, T4>)_delegates[key] + action;
    }

    public void AddEventListener<T1, T2, T3, T4, T5>(string key, Action5<T1, T2, T3, T4, T5> action)
    {
        AddEventListenerInternal(key, action);
        _delegates[key] = (Action5<T1, T2, T3, T4, T5>)_delegates[key] + action;
    }

    public void AddEventListener<T1, T2, T3, T4, T5, T6>(string key, Action6<T1, T2, T3, T4, T5, T6> action)
    {
        AddEventListenerInternal(key, action);
        _delegates[key] = (Action6<T1, T2, T3, T4, T5, T6>)_delegates[key] + action;
    }

    // 内部方法用于实际添加监听器
    private void AddEventListenerInternal(string key, Delegate action)
    {
        _delegates.TryAdd(key, null);

        Delegate d = _delegates[key];
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
        if (_delegates.ContainsKey(key))
        {
            _delegates[key] = (Action0)_delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1>(string key, Action1<T1> action)
    {
        if (_delegates.ContainsKey(key))
        {
            _delegates[key] = (Action1<T1>)_delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1, T2>(string key, Action2<T1, T2> action)
    {
        if (_delegates.ContainsKey(key))
        {
            _delegates[key] = (Action2<T1, T2>)_delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1, T2, T3>(string key, Action3<T1, T2, T3> action)
    {
        if (_delegates.ContainsKey(key))
        {
            _delegates[key] = (Action3<T1, T2, T3>)_delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1, T2, T3, T4>(string key, Action4<T1, T2, T3, T4> action)
    {
        if (_delegates.ContainsKey(key))
        {
            _delegates[key] = (Action4<T1, T2, T3, T4>)_delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1, T2, T3, T4, T5>(string key, Action5<T1, T2, T3, T4, T5> action)
    {
        if (_delegates.ContainsKey(key))
        {
            _delegates[key] = (Action5<T1, T2, T3, T4, T5>)_delegates[key] - action;
        }
    }

    public void RemoveEventListener<T1, T2, T3, T4, T5, T6>(string key, Action6<T1, T2, T3, T4, T5, T6> action)
    {
        if (_delegates.ContainsKey(key))
        {
            _delegates[key] = (Action6<T1, T2, T3, T4, T5, T6>)_delegates[key] - action;
        }
    }

    // 泛型方法用于触发事件
    public void InvokeEvent(string key)
    {
        if (_delegates.TryGetValue(key, out var d))
        {
            if (d is Action0 action)
            {
                action();
            }
        }
    }

    public void InvokeEvent<T1>(string key, T1 arg1)
    {
        if (_delegates.TryGetValue(key, out var d))
        {
            if (d is Action1<T1> action)
            {
                action(arg1);
            }
        }
    }

    public void InvokeEvent<T1, T2>(string key, T1 arg1, T2 arg2)
    {
        if (_delegates.TryGetValue(key, out var d))
        {
            if (d is Action2<T1, T2> action)
            {
                action(arg1, arg2);
            }
        }
    }

    public void InvokeEvent<T1, T2, T3>(string key, T1 arg1, T2 arg2, T3 arg3)
    {
        if (_delegates.TryGetValue(key, out var d))
        {
            if (d is Action3<T1, T2, T3> action)
            {
                action(arg1, arg2, arg3);
            }
        }
    }

    public void InvokeEvent<T1, T2, T3, T4>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (_delegates.TryGetValue(key, out var d))
        {
            if (d is Action4<T1, T2, T3, T4> action)
            {
                action(arg1, arg2, arg3, arg4);
            }
        }
    }

    public void InvokeEvent<T1, T2, T3, T4, T5>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (_delegates.TryGetValue(key, out var d))
        {
            if (d is Action5<T1, T2, T3, T4, T5> action)
            {
                action(arg1, arg2, arg3, arg4, arg5);
            }
        }
    }

    public void InvokeEvent<T1, T2, T3, T4, T5, T6>(string key, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        if (_delegates.TryGetValue(key, out var d))
        {
            if (d is Action6<T1, T2, T3, T4, T5, T6> action)
            {
                action(arg1, arg2, arg3, arg4, arg5, arg6);
            }
        }
    }

    // 清理所有事件监听器
    public void ClearAllListeners()
    {
        _delegates.Clear();
    }
}