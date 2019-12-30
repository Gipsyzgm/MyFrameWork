using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承了MonoBehaviour的基础管理类
/// 带有一个单例,并且在加载的时候不会被销毁
/// </summary>
public abstract class BaseManage<T> : MonoBehaviour where T : BaseManage<T>
{
	private static T _manage;    
	public static T Manage    
	{    
		get    
		{    
			if (_manage == null) 
			{
				GameObject go = GameObject.Find ("AllManage");
				if (go == null) 
				{
					go = new GameObject ();
					go.name = "AllManage";

					DontDestroyOnLoad (go);
				}


				_manage = go.AddComponent<T>();
			}

			return _manage;
		}    
	}

	void OnDestroy ()
	{
		_manage = null;
	}
}
