using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///简单的事件处理系统，即通过添加Event和移除Event来实现。事件名须添加到对应的EventKey枚举内
/// </summary>
public class MyEventMgr : MonoSingleton<MyEventMgr>
{
	private Dictionary<EventKey, Action<System.Object>> eventDictionary = null;
	public void Awake ()
	{
		eventDictionary = new Dictionary<EventKey, Action<System.Object>> ();
	}
	/// <summary>
	/// 添加一个事件监听
	/// </summary>
	public void AddEventListener (EventKey key, Action<System.Object> action)
	{
		if(eventDictionary.ContainsKey(key))
		{
			eventDictionary [key] += action;
		}
		else
		{
			eventDictionary.Add (key, action);
		}
	}
	/// <summary>
	/// 清理事件监听
	/// </summary>
	public void RemoveEventListener (EventKey key)
	{
		if(eventDictionary.ContainsKey(key))
		{
			eventDictionary [key] = null;
			eventDictionary.Remove (key);
		}
	}
	/// <summary>
	/// 触发事件
	/// </summary>
	public void InvokeEvent (EventKey key, System.Object obj)
	{
		if(eventDictionary.ContainsKey(key))
		{
			eventDictionary [key] (obj);
		}
	}
}

public enum EventKey : int
{	
	/// <summary>
	/// 玩家角色死亡
	/// </summary>
	PlayDie = 1001,

}
