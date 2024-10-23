using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 继承自Mono的模板类单例 
/// where T约束 使用必须继承HwMonoSingleton<T>
/// abstract; abstract修饰方法，会使这个方法变成抽象方法，也就是只有声明（定义）而没有实现，实现部分以"；"代替。需要子类继承实现（覆盖）。
/// 注意：父类是抽象类，其中有抽象方法，那么子类继承父类，并把父类中的所有抽象方法都实现（覆盖）了，
/// 子类才有创建对象的实例的能力，否则子类也必须是抽象类。抽象类中可以有构造方法，
/// 是子类在构造子类对象时需要调用的父类（抽象类）的构造方法。
/// 试用于单线程工程，多线程需要添加lock判断。
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	protected static T _instance= null;

	public static T Instance
	{
		get
		{ 
			if (_instance ==null)
			{
				_instance = GameObject.FindObjectOfType<T>();
				if (1 < GameObject.FindObjectsOfType<T> ().Length) 
				{
					Debug.Log ("该类型存在不止一个");
					return _instance;
				}
				if (_instance == null) 
				{//游戏中不存在这个单例类 重新生成一个物体 
					string instanceName = typeof(T).Name;
					GameObject instanceGo = GameObject.Find (instanceName);
					if (instanceGo == null)
						instanceGo = new GameObject (instanceName);
					_instance = instanceGo.AddComponent <T>();
					DontDestroyOnLoad (instanceGo);
				}
			}
			return _instance;
		}
	}
}
