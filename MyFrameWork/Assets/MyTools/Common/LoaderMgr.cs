using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoaderMgr : MonoSingleton<LoaderMgr>
{
    private Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();
    private Dictionary<string, object> _cachedResources = new Dictionary<string, object>();
    private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    // 初始化 Addressables
    public void Init(Action Completed = null)
    {
        // Addressables.InitializeAsync().Completed += (AsyncOperationHandle<IResourceLocator> op) =>
        // {
        //     if (op.Status == AsyncOperationStatus.Succeeded)
        //     {
        //         Debug.Log("Addressables initialized successfully.");
        //         Completed?.Invoke();
        //     }
        //     else
        //     {
        //         Debug.LogError($"Failed to initialize Addressables: {op.OperationException}");
        //     }
        // };
        Addressables.InitializeAsync().WaitForCompletion();
    }


    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="address"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T LoadAssetSync<T>(string address) where T : class
    {
        _lock.EnterReadLock();
        try
        {
            if (_cachedResources.TryGetValue(address, out var cachedResource) && cachedResource is T castedResource)
            {
                return castedResource;
            }
        }
        finally
        {
            _lock.ExitReadLock();
        }

        _lock.EnterWriteLock();
        try
        {
            // 检查是否已经在加载中
            if (_handles.ContainsKey(address))
            {
                throw new InvalidOperationException($"Resource at {address} is already being loaded.");
            }

            var handle = Addressables.LoadAssetAsync<T>(address);
            _handles[address] = handle;

            // 阻塞等待直到加载完成
            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var resource = handle.Result;
                _cachedResources[address] = resource;
                _handles.Remove(address);
                return resource;
            }
            else
            {
                Debug.LogError($"Failed to load resource: {address}. Error: {handle.OperationException}");
                _handles.Remove(address);
                return null;
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }


    /// <summary>
    ///  同步实例化预制体
    /// </summary>
    /// <param name="address"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public GameObject InstantiatePrefabSync(string address, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        _lock.EnterWriteLock();
        try
        {
            // 检查是否已经在加载中
            if (_handles.ContainsKey(address))
            {
                throw new InvalidOperationException($"Prefab at {address} is already being instantiated.");
            }

            var handle = Addressables.InstantiateAsync(address, position, rotation, parent);
            _handles[address] = handle;

            // 阻塞等待直到实例化完成
            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var instance = handle.Result;
                _handles.Remove(address);
                return instance;
            }
            else
            {
                Debug.LogError($"Failed to instantiate prefab: {address}. Error: {handle.OperationException}");
                _handles.Remove(address);
                return null;
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    ///  异步加载资源
    /// </summary>
    /// <param name="address"></param>
    /// <param name="onLoaded"></param>
    /// <param name="onProgress"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T LoadAssetAsync<T>(string address, Action<T> onLoaded = null, Action<float> onProgress = null)
        where T : class
    {
        _lock.EnterReadLock();
        try
        {
            if (_cachedResources.TryGetValue(address, out var cachedResource) && cachedResource is T castedResource)
            {
                onLoaded?.Invoke(castedResource);
                return castedResource;
            }
        }
        finally
        {
            _lock.ExitReadLock();
        }

        _lock.EnterWriteLock();
        try
        {
            if (_handles.ContainsKey(address))
            {
                // 如果已经有加载请求，则直接返回默认值
                return null;
            }

            var handle = Addressables.LoadAssetAsync<T>(address);
            _handles[address] = handle;

            handle.Completed += (AsyncOperationHandle<T> loadOp) =>
            {
                _lock.EnterWriteLock();
                try
                {
                    if (loadOp.Status == AsyncOperationStatus.Succeeded)
                    {
                        var resource = loadOp.Result;
                        _cachedResources[address] = resource;
                        onLoaded?.Invoke(resource);
                    }
                    else
                    {
                        Debug.LogError($"Failed to load resource: {address}. Error: {loadOp.OperationException}");
                    }

                    _handles.Remove(address);
                }
                catch (Exception ex)
                {
                    Debug.LogError(
                        $"An error occurred while handling the completion of loading for address: {address}. Error: {ex.Message}");
                    _handles.Remove(address);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            };

            // 进度报告
            // handle.Completed += (AsyncOperationHandle<T> loadOp) =>
            // {
            //     if (onProgress != null && loadOp.OperationException == null)
            //     {
            //         onProgress(loadOp.PercentComplete);
            //     }
            // };
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        return null;
    }

    /// <summary>
    /// 异步实例化预制体
    /// </summary>
    /// <param name="address"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="parent"></param>
    /// <param name="onInstantiated"></param>
    /// <param name="onProgress"></param>
    /// <returns></returns>
    public GameObject InstantiatePrefabAsync(string address, Vector3 position, Quaternion rotation,
        Transform parent = null,
        Action<GameObject> onInstantiated = null, Action<float> onProgress = null)
    {
        _lock.EnterWriteLock();
        try
        {
            var handle = Addressables.InstantiateAsync(address, position, rotation, parent);
            _handles[address] = handle;

            handle.Completed += (AsyncOperationHandle<GameObject> instantiateOp) =>
            {
                _lock.EnterWriteLock();
                try
                {
                    if (instantiateOp.Status == AsyncOperationStatus.Succeeded)
                    {
                        var instance = instantiateOp.Result;
                        onInstantiated?.Invoke(instance);
                    }
                    else
                    {
                        Debug.LogError(
                            $"Failed to instantiate prefab: {address}. Error: {instantiateOp.OperationException}");
                    }

                    _handles.Remove(address);
                }
                catch (Exception ex)
                {
                    Debug.LogError(
                        $"An error occurred while handling the completion of instantiation for address: {address}. Error: {ex.Message}");
                    _handles.Remove(address);
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            };

            // 进度报告
            // handle.Completed += (AsyncOperationHandle<GameObject> instantiateOp) =>
            // {
            //     if (onProgress != null && instantiateOp.OperationException == null)
            //     {
            //         onProgress(instantiateOp.PercentComplete);
            //     }
            // };
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        return null;
    }

    // 释放单个资源
    public void Release(string address)
    {
        _lock.EnterWriteLock();
        try
        {
            if (_handles.TryGetValue(address, out var handle) && handle.IsValid())
            {
                Addressables.Release(handle);
                _handles.Remove(address);
                // 检查并移除缓存中的资源
                if (_cachedResources.ContainsKey(address))
                {
                    _cachedResources.Remove(address);
                }
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    // 释放所有资源
    public void ReleaseAllResources()
    {
        _lock.EnterWriteLock();
        try
        {
            foreach (var handle in _handles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }

            _handles.Clear();
            _cachedResources.Clear();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    // 清理
    private void OnDestroy()
    {
        ReleaseAllResources();
    }
}