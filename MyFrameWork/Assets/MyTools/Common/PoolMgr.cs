/*
 *  项目名字：MyFrameWork
 *  创建时间：2020.1.17
 *  描述信息：UI页面控制。
 *  使用说明：
 *  1：把对象池名称定义在最下方PoolName内，仅为方便管理。
 *  2：初始化Pool，可直接挂载在Awake里执行初始化方法。也可以直接 MyPoolSingleton.Instance.AddSpawnPool MyPoolSingleton.Instance.AddSpawnPool(PoolName.TestPool,最大数量, 对应预制体);
 *  3：支持自动定时删除对象。
 */

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 统一的对象池管理
/// </summary>
public class PoolMgr : MonoSingleton<PoolMgr>
{
    private GameObject _poolContainer = null;
    private Dictionary<string, SpawnPool> _pools;

    /// <summary>
    /// 初始化，Pool的初始化添加在这里也可以
    /// </summary>
    public void Awake()
    {
        _pools = new Dictionary<string, SpawnPool>();
        _poolContainer = gameObject;
    }

    // 批量初始化池
    public void InitializePools(List<PoolInitializationData> poolData)
    {
        foreach (var data in poolData)
        {
            AddSpawnPool(data.GoName, data.MaxCount, data.Prefab, data.IsAutoHide, data.AutoHideTimer);
        }
    }

    /// <summary>
    /// 快速添加一个新pool，自己控制显示和隐藏
    /// </summary>
    /// <param name="goName"></param>
    /// <param name="maxCount"></param>
    /// <param name="prefab"></param>
    public void AddSpawnPool(string goName, int maxCount, GameObject prefab)
    {
        if (_pools.ContainsKey(goName + "Pool"))
        {
            Debug.LogError("添加失败，已经添加同名pool");
            return;
        }

        var poolGo = new GameObject(goName + "Pool");
        poolGo.transform.SetParent(_poolContainer.transform);
        var pool = new SpawnPool(goName, maxCount, prefab, poolGo.transform);
        _pools.Add(goName + "Pool", pool);
    }

    /// <summary>
    /// 添加一个新pool，可以设置是否自动隐藏和自动隐藏的时间，自动隐藏必须设置为true，隐藏时间才生效
    /// </summary>
    /// <param name="goName"></param>
    /// <param name="maxCount"></param>
    /// <param name="prefab"></param>
    /// <param name="isAutoHide"></param>
    /// <param name="autoHideTimer"></param>
    public void AddSpawnPool(string goName, int maxCount, GameObject prefab, bool isAutoHide, float autoHideTimer)
    {
        if (_pools.ContainsKey(goName + "Pool"))
        {
            Debug.LogError("添加失败，已经添加同名pool");
            return;
        }

        var poolGo = new GameObject(goName + "Pool");
        poolGo.transform.SetParent(_poolContainer.transform);
        var pool = new SpawnPool(goName, maxCount, prefab, poolGo.transform, isAutoHide, autoHideTimer);
        _pools.Add(goName + "Pool", pool);
    }

    /// <summary>
    /// 生成一个对象。
    /// </summary>
    /// <param name="goName"></param>
    /// <returns></returns>
    public GameObject SpawnGo(string goName)
    {
        if (_pools.TryGetValue(goName + "Pool", out var pool))
        {
            return pool.SpawnGo();
        }
        else
        {
            Debug.LogError($"没有叫 {goName} 的对象池");
            return null;
        }
    }

    public void SpawnGo(string goName, Action<GameObject> func)
    {
        if (_pools.TryGetValue(goName + "Pool", out var pool))
        {
            var obj = pool.SpawnGo();
            func?.Invoke(obj);
        }
        else
        {
            Debug.LogError($"没有叫 {goName} 的对象池");
        }
    }


    /// <summary>
    /// 隐藏所传对应Pool中的_Go对象
    /// </summary>
    /// <param name="goName"></param>
    /// <param name="go"></param>
    public void DisSpawnGo(string goName, GameObject go)
    {
        if (_pools.TryGetValue(goName + "Pool", out var pool))
        {
            pool.DisSpawnGo(go);
        }
        else
        {
            Debug.LogError($"没有叫 {goName} 的对象池");
        }
    }

    /// <summary>
    /// 隐藏所有对象
    /// </summary>
    /// <param name="goName"></param>
    public void DisSpawnGo(string goName)
    {
        if (_pools.TryGetValue(goName + "Pool", out var pool))
        {
            pool.DisSpawnGo();
        }
        else
        {
            Debug.LogError($"没有叫 {goName} 的对象池");
        }
    }

    /// <summary>
    /// 获取当前显示对象的数量
    /// </summary>
    /// <param name="goName"></param>
    /// <returns></returns>
    public int GetPoolActiveCount(string goName)
    {
        if (_pools.TryGetValue(goName + "Pool", out var pool))
        {
            return pool.TrueCount;
        }
        else
        {
            Debug.LogError($"没有叫 {goName} 的对象池");
            return -1;
        }
    }

    public class SpawnPool
    {
        private string _goName = string.Empty;

        //Pool中对象的最大数量
        private int _maxCount;
        private GameObject _prefab;
        private Transform _poolTransform = null;
        private bool _isAutoHide;

        private float _autoHideTimer;

        //显示对应的list
        private List<GameObject> _activeObjects = new List<GameObject>();

        //隐藏物体的list
        private List<GameObject> _inactiveObjects = new List<GameObject>();

        // 是否允许动态扩展对象池
        private bool _allowDynamicExpansion;
        public int TrueCount => _activeObjects.Count;
        public int FalseCount => _inactiveObjects.Count;


        public SpawnPool(string goName, int maxCount, GameObject prefab, Transform poolTransform,
            bool isAutoHide = false, float autoHideTimer = 0f, bool allowDynamicExpansion = true)
        {
            _goName = goName;
            _maxCount = maxCount;
            _prefab = prefab;
            _poolTransform = poolTransform;
            _isAutoHide = isAutoHide;
            _autoHideTimer = autoHideTimer;
            _allowDynamicExpansion = allowDynamicExpansion;
            // 初始化时创建所有的隐藏对象
            for (int i = 0; i < _maxCount; i++)
            {
                GameObject go = Instantiate(_prefab, _poolTransform);
                HideGo(go);
            }
        }

        /// <summary>
        /// 供外部调用的生成对象的方法
        /// </summary>
        /// <returns></returns>
        public GameObject SpawnGo()
        {
            if (_inactiveObjects.Count > 0)
            {
                GameObject obj = _inactiveObjects[0];
                _inactiveObjects.RemoveAt(0);
                ShowGo(obj);
                return obj;
            }
            else if (_allowDynamicExpansion)
            {
                // 动态扩展对象池，创建新的对象
                GameObject newObj = Instantiate(_prefab, _poolTransform);
                ShowGo(newObj);
                return newObj;
            }
            else
            {
                Debug.LogWarning(
                    $"No more objects available in the {_goName} pool and dynamic expansion is not allowed.");
                return null; // 如果不允许动态扩展，则返回null
            }
        }

        /// <summary>
        /// 供外部调用的隐藏单个对象的方法
        /// </summary>
        /// <param name="obj"></param>
        public void DisSpawnGo(GameObject obj)
        {
            if (_activeObjects.Contains(obj))
            {
                HideGo(obj);
            }
            else
            {
                Debug.LogWarning($"{obj.name} is not in the active list of the {_goName} pool.");
            }
        }

        /// <summary>
        /// 供外部调用的隐藏所有对象的方法
        /// </summary>
        public void DisSpawnGo()
        {
            foreach (var obj in _activeObjects)
            {
                HideGo(obj);
            }
        }

        /// <summary>
        /// 显示物体
        /// </summary>
        /// <param name="go"></param>
        private void ShowGo(GameObject go)
        {
            if (_isAutoHide)
            {
                PoolGoAutoHide autoHide = go.GetComponent<PoolGoAutoHide>();
                if (autoHide == null)
                    autoHide = go.AddComponent<PoolGoAutoHide>();
                autoHide.StartAutoHide(_goName, _autoHideTimer);
            }

            go.SetActive(true);
            if (!_activeObjects.Contains(go))
                _activeObjects.Add(go);
            if (_inactiveObjects.Contains(go))
                _inactiveObjects.Remove(go);
        }

        /// <summary>
        /// 隐藏物体，字典位置转换
        /// </summary>
        /// <param name="go"></param>
        private void HideGo(GameObject go)
        {
            if (_isAutoHide)
            {
                PoolGoAutoHide autoHide = go.GetComponent<PoolGoAutoHide>();
                if (autoHide != null)
                    autoHide.EndAutoHide();
            }

            go.SetActive(false);
            if (!_inactiveObjects.Contains(go))
                _inactiveObjects.Add(go);
            if (_activeObjects.Contains(go))
                _activeObjects.Remove(go);
        }
    }
}

/// <summary>
/// 隐藏物体，可以传时间用来控制物体隐藏
/// </summary>
public class PoolGoAutoHide : MonoBehaviour
{
    private float _autoHideTimer = 0;
    private string _goName = string.Empty;

    public void StartAutoHide(string goName, float timer)
    {
        EndAutoHide();
        _goName = goName;
        _autoHideTimer = timer;
        Invoke(nameof(HideMySelf), _autoHideTimer);
    }

    public void EndAutoHide()
    {
        CancelInvoke(nameof(HideMySelf));
    }

    void HideMySelf()
    {
        PoolMgr.Instance.DisSpawnGo(_goName, gameObject);
    }
}

// 辅助类，用于初始化数据
[System.Serializable]
public class PoolInitializationData
{
    public string GoName;
    public int MaxCount;
    public GameObject Prefab;
    public bool IsAutoHide;
    public float AutoHideTimer;
}