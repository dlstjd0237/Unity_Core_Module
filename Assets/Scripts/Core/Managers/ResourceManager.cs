using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    private const string SpriteSubAssetTag = ".sprite";

    private readonly Dictionary<string, Object> _resources = new Dictionary<string, Object>();
    private readonly Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();
    private readonly Dictionary<string, List<Action<Object>>> _pending = new Dictionary<string, List<Action<Object>>>();
    private int _generation;

    public bool IsLoaded { get; private set; }
    public int CachedCount => _resources.Count;

    public bool Has(string key) =>
        !string.IsNullOrEmpty(key) && _resources.ContainsKey(key);

    public T Load<T>(string key) where T : Object
    {
        if (string.IsNullOrEmpty(key))
            return null;

        return _resources.TryGetValue(key, out var resource) ? resource as T : null;
    }

    public bool TryLoad<T>(string key, out T resource) where T : Object
    {
        resource = Load<T>(key);
        return resource != null;
    }

    public GameObject Instantiate(string key, Vector3 position) =>
        Instantiate(key, position, Quaternion.identity, null);

    public GameObject Instantiate(string key, Vector3 position, Quaternion rotation) =>
        Instantiate(key, position, rotation, null);

    public GameObject Instantiate(string key, Vector3 position, Quaternion rotation, Transform parent)
    {
        var prefab = Load<GameObject>(key);
        if (prefab == null)
        {
            Stdout.LogError($"Can't find '{key}'", nameof(ResourceManager));
            return null;
        }

        //Todo Pooling
        var instance = Object.Instantiate(prefab, position, rotation, parent);
        instance.name = prefab.name;
        return instance;
    }

    public void Destroy(GameObject instance)
    {
        if (instance == null)
            return;

        //Todo Pooling
        Object.Destroy(instance);
    }

    public void LoadAsync<T>(string key, Action<T> callback = null) where T : Object
    {
        if (string.IsNullOrEmpty(key))
        {
            callback?.Invoke(null);
            return;
        }

        if (_resources.TryGetValue(key, out var cached))
        {
            callback?.Invoke(cached as T);
            return;
        }

        if (_pending.TryGetValue(key, out var pendingList))
        {
            if (callback != null)
                pendingList.Add(obj => callback(obj as T));
            return;
        }

        var subscribers = new List<Action<Object>>();
        if (callback != null)
            subscribers.Add(obj => callback(obj as T));
        _pending[key] = subscribers;

        int generation = _generation;
        var handle = Addressables.LoadAssetAsync<T>(ResolveLoadKey(key));
        handle.Completed += op =>
        {
            // ReleaseAll이 로드 도중 호출되면 generation이 증가하므로 결과 폐기
            if (generation != _generation)
            {
                Addressables.Release(handle);
                foreach (var sub in subscribers)
                    sub(null);
                return;
            }

            _pending.Remove(key);

            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                Stdout.LogError($"Failed to load '{key}': {op.OperationException?.Message}", nameof(ResourceManager));
                foreach (var sub in subscribers)
                    sub(null);
                return;
            }

            _resources[key] = op.Result;
            _handles[key] = handle;
            foreach (var sub in subscribers)
                sub(op.Result);
        };
    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> onProgress = null, Action onComplete = null) where T : Object
    {
        var locationsHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        locationsHandle.Completed += op =>
        {
            try
            {
                if (op.Status != AsyncOperationStatus.Succeeded || op.Result == null)
                {
                    Stdout.LogError($"Failed to resolve label '{label}'", nameof(ResourceManager));
                    onComplete?.Invoke();
                    return;
                }

                int total = op.Result.Count;
                if (total == 0)
                {
                    IsLoaded = true;
                    onComplete?.Invoke();
                    return;
                }

                int loaded = 0;
                foreach (var location in op.Result)
                {
                    string primaryKey = location.PrimaryKey;

                    if (primaryKey.EndsWith(SpriteSubAssetTag, StringComparison.Ordinal))
                        LoadAsync<Sprite>(primaryKey, _ => OnAssetLoaded(primaryKey));
                    else
                        LoadAsync<T>(primaryKey, _ => OnAssetLoaded(primaryKey));
                }

                void OnAssetLoaded(string key)
                {
                    loaded++;
                    onProgress?.Invoke(key, loaded, total);
                    if (loaded >= total)
                    {
                        IsLoaded = true;
                        onComplete?.Invoke();
                    }
                }
            }
            finally
            {
                Addressables.Release(locationsHandle);
            }
        };
    }

    public void Release(string key)
    {
        if (string.IsNullOrEmpty(key))
            return;

        if (_handles.TryGetValue(key, out var handle))
        {
            Addressables.Release(handle);
            _handles.Remove(key);
        }

        _resources.Remove(key);
    }

    public void ReleaseAll()
    {
        foreach (var handle in _handles.Values)
            Addressables.Release(handle);

        _handles.Clear();
        _resources.Clear();
        _pending.Clear();
        _generation++;
        IsLoaded = false;
    }

    protected override void OnDisable()
    {
        ReleaseAll();
        base.OnDisable();
    }

    private static string ResolveLoadKey(string key)
    {
        if (!key.EndsWith(SpriteSubAssetTag, StringComparison.Ordinal))
            return key;

        string atlasName = key.Substring(0, key.Length - SpriteSubAssetTag.Length);
        return $"{key}[{atlasName}]";
    }
}
