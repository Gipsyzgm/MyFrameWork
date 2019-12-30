using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

/// <summary>
/// 单例类模板
/// </summary>
public abstract class Singleton<T> where T : Singleton<T>
{
	private static T _instance=null;
    
	protected Singleton()
	{
		
	}
	public static T Instance
	{
		get
		{ 
			if (_instance == null)
			{
				// 先获取所有非public的构造方法
				ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
				// 从ctors中获取无参的构造方法
				ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
				if (ctor == null)
					throw new Exception("没有私有的构造方法");
				// 调用构造方法
				_instance = ctor.Invoke(null) as T;
			}
			return _instance;
		}
	}
}
