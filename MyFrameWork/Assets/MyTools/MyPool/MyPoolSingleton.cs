/*
 *  项目名字：MyFrameWork
 *  创建时间：2020.1.17
 *  描述信息：UI页面控制。
 *  使用说明：
 *  1：把对象池名称定义在最下方PoolName内，仅为方便管理。
 *  2：初始化Pool，可直接挂载在Awake里执行初始化方法。也可以直接 MyPoolSingleton.Instance.AddSpawnPool MyPoolSingleton.Instance.AddSpawnPool(PoolName.TestPool,最大数量, 对应预制体);
 *  3：支持自动定时删除对象。
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 统一的对象池管理
/// </summary>
public class MyPoolSingleton  : MonoSingleton<MyPoolSingleton>
{
	private GameObject _go = null;
	private Dictionary<string,SpawnPool> _dicSpawn;
    /// <summary>
    /// 初始化，Pool的初始化添加在这里也可以
    /// </summary>
	public void Awake()
	{
		this._dicSpawn = new Dictionary<string, SpawnPool> ();
		this._go = this.gameObject;
	}
    /// <summary>
    /// 快速添加一个新pool，自己控制显示和隐藏
    /// </summary>
    /// <param name="_GoName"></param>
    /// <param name="_MaxCount"></param>
    /// <param name="_Prefab"></param>
	public void AddSpawnPool (string _GoName, int _MaxCount, GameObject _Prefab)
	{
        if (this._dicSpawn.ContainsKey(_GoName + "Pool"))
        {
            Debug.LogError("添加失败，已经添加同名pool");
            return;
            //this._dicSpawn.Remove(_GoName + "Pool");
        }
        GameObject go = new GameObject (_GoName+"Pool");
		go.transform.parent = this._go.transform;
        SpawnPool pool = new SpawnPool (_GoName,_MaxCount, _Prefab, go.transform);       
        this._dicSpawn.Add(_GoName + "Pool", pool);
	}
    /// <summary>
    /// 添加一个新pool，可以设置是否自动隐藏和自动隐藏的时间，自动隐藏必须设置为true，隐藏时间才生效
    /// </summary>
    /// <param name="_GoName"></param>
    /// <param name="_MaxCount"></param>
    /// <param name="_Prefab"></param>
    /// <param name="_IsAutoHide"></param>
    /// <param name="_AutoHideTimer"></param>
    public void AddSpawnPool (string _GoName, int _MaxCount, GameObject _Prefab,bool _IsAutoHide,float _AutoHideTimer)
    {
        if (this._dicSpawn.ContainsKey(_GoName + "Pool"))
        {
            Debug.LogError("添加失败，已经添加同名pool");
            return;
            //this._dicSpawn.Remove(_GoName + "Pool");
        }
        GameObject go = new GameObject (_GoName+"Pool");
        go.transform.parent = this._go.transform;
        SpawnPool pool = new SpawnPool (_GoName,_MaxCount, _Prefab, go.transform,_IsAutoHide,_AutoHideTimer);
        this._dicSpawn.Add (_GoName+"Pool", pool);
    }
    /// <summary>
    /// 生成一个对象。
    /// </summary>
    /// <param name="_GoName"></param>
    /// <returns></returns>
	public GameObject SpawnGo(string _GoName)
	{
        if (this._dicSpawn.ContainsKey(_GoName + "Pool"))
            return this._dicSpawn[_GoName + "Pool"].SpawnGo();
        else
        {
            Debug.LogError("没有叫:" + _GoName + "的对象");
            return null;
        }
	}
    /// <summary>
    /// 隐藏所传对应Pool中的_Go对象
    /// </summary>
    /// <param name="_GoName"></param>
    /// <param name="_Go"></param>
	public void DisSpawnGo(string _GoName,GameObject _Go)
	{
		if (this._dicSpawn.ContainsKey (_GoName+"Pool")) {
            this._dicSpawn [_GoName+"Pool"].DisSpawnGo (_Go);
		} else 
		{
			Debug.LogError ("没有叫:"+_GoName+"的对象");
		}
	}
    /// <summary>
    /// 隐藏所有对象
    /// </summary>
    /// <param name="_GoName"></param>
    public void DisSpawnGo(string _GoName)
    {
        if (this._dicSpawn.ContainsKey(_GoName + "Pool"))
        {
            this._dicSpawn[_GoName + "Pool"].DisSpawnGo();
        }
        else
        {
            Debug.LogError("没有叫:" + _GoName + "的对象");
        }
    }
    /// <summary>
    /// 获取当前显示对象的数量
    /// </summary>
    /// <param name="_GoName"></param>
    /// <returns></returns>
    public int GetPoolActiveCount(string _GoName)
    {
        if (this._dicSpawn.ContainsKey(_GoName + "Pool"))
        {
            return this._dicSpawn[_GoName + "Pool"].TrueCount;
        }
        else
        {       
            Debug.LogError("没有叫:" + _GoName + "的对象");
            return -1;
        }
    }

	public class SpawnPool
	{
        private string _goName = string.Empty;
        //Pool中当前的对象数量
		private int _nowCount;
        //Pool中对象的最大数量
        private int _maxCount;
        private GameObject _prefab;
		private Transform _parent = null;
        private bool _isAutoHide = false;
        private float _autoHideTimer = -1;
        //显示对应的list
		private List<GameObject> _trueListGo;
        //隐藏物体的list
		private List<GameObject> _falseListGo;
        public int TrueCount { get { return this._trueListGo.Count; } }
        public int FalseCount { get { return this._falseListGo.Count; } }


        public SpawnPool(string _GoName,int _MaxCount, GameObject _Prefab, Transform _Tra):this(_GoName,_MaxCount,_Prefab,_Tra,false,-1)
        {

            
        }

        public SpawnPool (string _GoName,int _MaxCount, GameObject _Prefab, Transform _Tra,bool _IsAutoHide,float _AutoHideTimer)
		{
            this._goName = _GoName;
			this._parent = _Tra;
			this._maxCount = _MaxCount;
			this._prefab = _Prefab;
            this._isAutoHide = _IsAutoHide;
            this._autoHideTimer =_AutoHideTimer;

			this._trueListGo = new List<GameObject>();
			this._falseListGo = new List<GameObject>();
			this._dicGoSurviveTimer = new Dictionary<GameObject,int>();
            //每次克隆物体会+1
            this._nowCount = 0;
            //生成的pool时初始化一个隐藏物体。
            GameObject go = this.InsGo ();
			go.transform.SetParent(_Tra);
			go.transform.localPosition = Vector3.zero;		
			this.HideGo(go);
		}
        /// <summary>
        /// 供外部调用的生成对象的方法
        /// </summary>
        /// <returns></returns>
		public GameObject SpawnGo()
		{
			GameObject go = null;
			if (this._falseListGo.Count < 1)
			{
                if (this._nowCount < this._maxCount)
                    go = this.InsGo();    
				else 
				{
                    go = this.GetSurviveTimeBest();
                    this.ShowGo (go);
				}
				return go;
			} 
			else 
			{
				int index = UnityEngine.Random.Range (0,this._falseListGo.Count);
				go = this._falseListGo [index];
				this.ShowGo (go);
				return go;
			}
		}
        /// <summary>
        /// 供外部调用的隐藏单个对象的方法
        /// </summary>
        /// <param name="_Go"></param>
		public void DisSpawnGo (GameObject _Go)
		{
			if (this._trueListGo.Contains (_Go))
				this.HideGo (_Go);
		}
        /// <summary>
        /// 供外部调用的隐藏所有对象的方法
        /// </summary>
        public void DisSpawnGo()
        {
            int lenth = this._trueListGo.Count;
            List<GameObject> go = new List<GameObject>();
            for (int i = 0; i < lenth; i++)
            {
                go.Add(this._trueListGo[i]);
            }
            for (int i = 0; i < lenth;i++ )
            {
                this.HideGo(go[i]);
            }
        }

		GameObject InsGo ()
		{
			this._nowCount++;
			GameObject go = GameObject.Instantiate (_prefab) as GameObject;
			go.transform.SetParent(this._parent);
			this.ShowGo (go);
			return go;
		}
        /// <summary>
        /// 这个list用于临时储存一下_dicGoSurviveTimer以更新对象存活时长
        /// </summary>
		private List<GameObject> _goList ;
        /// <summary>
        /// 显示物体
        /// </summary>
        /// <param name="_Go"></param>
		void ShowGo(GameObject _Go)
		{
			if (this._goList == null)
				this._goList = new List<GameObject> ();
			if (0 < this._goList.Count)
				this._goList.Clear ();
			
     
			if (this._dicGoSurviveTimer.ContainsKey (_Go)) 
			{
				this._dicGoSurviveTimer [_Go] = 0;
				foreach(var temp in this._dicGoSurviveTimer)
				{
					if(temp.Key != _Go)
					{
						this._goList.Add (temp.Key);
					}
				}
				int lentj = this._goList.Count;
                for (int i =0;i<lentj;i++)
				{
					this._dicGoSurviveTimer [this._goList [i]] +=1;
				}
			}	
			else
			{
				this._dicGoSurviveTimer.Add (_Go,0);
				foreach(var temp in this._dicGoSurviveTimer)
				{
                    if (temp.Key != _Go)
					{
                        this._goList.Add (temp.Key);
					}
				}
				int lentj = this._goList.Count;
             
				for(int i =0;i<lentj;i++)
				{
					this._dicGoSurviveTimer [this._goList [i]] +=1;
                }
			}
            if (this._isAutoHide)
            {
                PoolGoAutoHide autoHide = _Go.GetComponent<PoolGoAutoHide>();
                if (autoHide == null)
                    autoHide= _Go.AddComponent<PoolGoAutoHide>();
                autoHide.StartAutoHide(this._goName, this._autoHideTimer);
            }
			_Go.SetActive (true);
			if(!this._trueListGo.Contains(_Go))
                this._trueListGo.Add (_Go);
			if (this._falseListGo.Contains (_Go))
				this._falseListGo.Remove (_Go);
		}
        /// <summary>
        /// 隐藏物体，字典位置转换
        /// </summary>
        /// <param name="_Go"></param>
		void HideGo(GameObject _Go)
		{
			if(this._dicGoSurviveTimer.ContainsKey(_Go))
			{
				this._dicGoSurviveTimer.Remove (_Go);
			}
            if (this._isAutoHide)
            {
                PoolGoAutoHide autoHide = _Go.GetComponent<PoolGoAutoHide>();
                if (autoHide != null)
                    autoHide.EndAutoHide();
            }
			_Go.SetActive (false);	
			this._falseListGo.Add (_Go);
			if (this._trueListGo.Contains (_Go))
				this._trueListGo.Remove (_Go);
		}
        /// <summary>
        /// 获得存在时间最长的物体
        /// </summary>
        /// <returns>The survive time best.</returns>
        GameObject GetSurviveTimeBest()
        {
            int max = -1;
            GameObject _surverBestLong = null;
            foreach (var temp in this._dicGoSurviveTimer) 
            {
                if (max < temp.Value)
                {
                    max = temp.Value;   
                    _surverBestLong = temp.Key;
                }
            }
            return _surverBestLong;
        }
        /// <summary>
        /// 用来记录对象存活时长的字典,和上面方法配合来获取存活最久的对象
        /// </summary>
		private Dictionary<GameObject,int> _dicGoSurviveTimer ;
	}
}
/// <summary>
/// 隐藏物体，可以传时间用来控制物体隐藏
/// </summary>
public class PoolGoAutoHide:MonoBehaviour
{
    private float _autoHideTimer = 0;
    private string _goName=string.Empty;
    public void StartAutoHide(string _GoName,float _Timer)
    {
        this.EndAutoHide();
        this._goName = _GoName;
        this._autoHideTimer = _Timer;
        Invoke("HideMySelf",this._autoHideTimer);
    }
    public void EndAutoHide()
    {
        CancelInvoke("HideMySelf");
    }
    void HideMySelf()
    {
        MyPoolSingleton.Instance.DisSpawnGo(this._goName,this.gameObject);
    }
}
/// <summary>
/// 可以把需要的Pool名字都定义在这个地方方便管理
/// </summary>
public class PoolName
{
    public const string TestPool = "TestPool";
  
}
