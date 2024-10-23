using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///简单的事件处理系统，即通过添加Event和移除Event来实现。事件名须添加到对应的EventConst类内
/// </summary>
public class EventMgr : MonoSingleton<EventMgr>
{
    private Dictionary<string, HashSet<Action<object[]>>> eventDictionary =
        new Dictionary<string, HashSet<Action<object[]>>>();

    /// <summary>
    /// 添加一个事件监听
    /// </summary>
    public void AddEventListener(string key, Action<object[]> action)
    {
        if (action == null)
        {
            Debug.LogWarning("Trying to add a null action to the event system.");
            return;
        }

        if (!eventDictionary.ContainsKey(key))
        {
            eventDictionary[key] = new HashSet<Action<object[]>>();
        }

        if (!eventDictionary[key].Contains(action))
        {
            eventDictionary[key].Add(action);
        }
    }

    /// <summary>
    /// 清理事件监听
    /// </summary>
    public void RemoveEventListener(string key, Action<object[]> action)
    {
        if (eventDictionary.ContainsKey(key) && eventDictionary[key].Contains(action))
        {
            eventDictionary[key].Remove(action);

            // 如果没有更多的监听器了，移除整个条目
            if (eventDictionary[key].Count == 0)
            {
                eventDictionary.Remove(key);
            }
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    public void InvokeEvent(string key, params object[] args)
    {
        if (eventDictionary.TryGetValue(key, out var actions))
        {
            foreach (var action in actions)
            {
                action?.Invoke(args);
            }
        }
    }
}