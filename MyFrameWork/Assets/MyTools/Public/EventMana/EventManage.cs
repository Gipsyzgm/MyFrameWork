using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EventManage : BaseManage<EventManage>
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
	public void clearEventListener (EventKey key)
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
	/// <summary>
	/// 玩家角色无敌状态改变
	/// </summary>
	PlayStateChange,

}
