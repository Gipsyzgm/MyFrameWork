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
	/// <summary>
	/// 玩家角色积分改变
	/// </summary>
	PlayScoreChange,
	/// <summary>
	/// 玩家角色快速下坠
	/// </summary>
	PlayRapidDrop,
	/// <summary>
	/// 玩家能量值改变
	/// </summary>
	PlayEnergyChange,
	/// <summary>
	/// 玩家自由落体状态结束
	/// </summary>
	PlayFreeFallEnd,
	/// <summary>
	/// 更换皮肤
	/// </summary>
	PlayChangeSkin,


	/// <summary>
	/// 更新每日奖励
	/// </summary>
	UpDateDailyBonus = 2001,
	/// <summary>
	/// 更新每日奖励时间
	/// </summary>
	UpDateDailyBonusTime,
	/// <summary>
	/// 更新在线奖励
	/// </summary>
	UpDatePackage,


	/// <summary>
	/// 生成关卡
	/// </summary>
	CreateCheckPoint = 3001,
	/// <summary>
	/// 通过关卡
	/// </summary>
	ThroughCheckPoint,
	/// <summary>
	/// 清理存活建筑
	/// </summary>
	CleanSurviveBuilding,


	/// <summary>
	/// 进入赛场
	/// </summary>
	GameEnterArena = 4001,
	/// <summary>
	/// 赛场晋级
	/// </summary>
	GameArenaBePromoted,
	/// <summary>
	/// 继续游戏
	/// </summary>
	GameContinue,
	/// <summary>
	/// 结算游戏
	/// </summary>
	GameClearing,
	/// <summary>
	/// 返回大厅
	/// </summary>
	ReturnToTheHall,
	/// <summary>
	/// 改变背景
	/// </summary>
	ChangeBackground,

	/// <summary>
	/// 激励视频状态变化
	/// </summary>
	RewardVideoAdStatusChange = 5001,
		/// <summary>
		/// 激励视频状态变化
		/// </summary>
	StartSprintMode,
	HideSprintBtn,
	ShowSprintBtn,
	SprintAddScore
}
