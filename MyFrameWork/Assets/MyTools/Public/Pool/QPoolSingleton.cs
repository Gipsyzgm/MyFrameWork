using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 统一的对象池管理
/// </summary>
public class PoolSingleton  : MonoSingleton<PoolSingleton>
{
	private GameObject _go = null;
	private Dictionary<string,SpawnPool> _dicSpawn;
	public void Init()
	{
		this._dicSpawn = new Dictionary<string, SpawnPool> ();
		this._go = this.gameObject;
	}
	public void AddSpawnPool (string _GoName, int _MaxCount, GameObject _Prefab)
	{
		GameObject go = new GameObject (_GoName+"Pool");
		go.transform.parent = this._go.transform;
        SpawnPool pool = new SpawnPool (_GoName,_MaxCount, _Prefab, go.transform);

        if (this._dicSpawn.ContainsKey(_GoName + "Pool"))
            this._dicSpawn.Remove(_GoName + "Pool");
        this._dicSpawn.Add(_GoName + "Pool", pool);
	}
    public void AddSpawnPool (string _GoName, int _MaxCount, GameObject _Prefab,bool _IsAutoHide,float _AutoHideTimer)
    {
        GameObject go = new GameObject (_GoName+"Pool");
        go.transform.parent = this._go.transform;
        SpawnPool pool = new SpawnPool (_GoName,_MaxCount, _Prefab, go.transform,_IsAutoHide,_AutoHideTimer);
        if (this._dicSpawn.ContainsKey(_GoName + "Pool"))
            this._dicSpawn.Remove(_GoName + "Pool");
        this._dicSpawn.Add (_GoName+"Pool", pool);

    }
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
	public void DisSpawnGo(string _GoName,GameObject _Go)
	{
		if (this._dicSpawn.ContainsKey (_GoName+"Pool")) {
            this._dicSpawn [_GoName+"Pool"].DisSpawnGo (_Go);
		} else 
		{
			Debug.LogError ("没有叫:"+_GoName+"的对象");
		}
	}
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
        private string _goName=string.Empty;
		private int _nowCount;
		private int _maxCount;
		private GameObject _prefab;
		private Transform _parent = null;
        private bool _isAutoHide=false;
        private float _autoHideTimer=-1;

		private List<GameObject> _trueListGo;
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

			GameObject go = this.InsGo ();
			go.transform.parent = _Tra;
			go.transform.localPosition = Vector3.zero;
			this._nowCount =0;
			this.HideGo(go);
		}
		public GameObject SpawnGo ()
		{
			GameObject go = null;
			if (this._falseListGo.Count < 1)
			{
				if (this._nowCount <= this._maxCount)
					go = this.InsGo ();
				else 
				{
                    go= this.GetSurviveTimeBest();
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

		public void DisSpawnGo (GameObject _Go)
		{
			if (this._trueListGo.Contains (_Go))
				this.HideGo (_Go);
		}
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
			go.transform.parent = this._parent;
			this.ShowGo (go);
			return go;
		}

		private List<GameObject> _goList ;
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
				for(int i =0;i<lentj;i++)
				{
					this._dicGoSurviveTimer [this._goList [i]] +=1;
				}
			}	
			else
			{
				this._dicGoSurviveTimer.Add (_Go,0);
				foreach(var temp in this._dicGoSurviveTimer)
				{
					if(temp.Key != _Go)
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
			if(!this._trueListGo.Contains(_Go)) this._trueListGo.Add (_Go);
			if (this._falseListGo.Contains (_Go))
				this._falseListGo.Remove (_Go);
		}
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
		private Dictionary<GameObject,int> _dicGoSurviveTimer ;
	}
}
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
        PoolSingleton.Instance.DisSpawnGo(this._goName,this.gameObject);
    }
}
public class PoolName
{
    public const string Fruit_ShowIcon = "ShowIcon";
    public const string NiuNiu_Chip = "NiuNiu_Chip";
}
