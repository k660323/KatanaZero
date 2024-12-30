using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public Dictionary<string, Queue<GameObject>> poolObjects { get; protected set; } = new Dictionary<string, Queue<GameObject>>();

    Dictionary<string, Object> _resources = new Dictionary<string, Object>();

    public T Load<T>(string path) where T : Object
    {
        // 캐싱되어있으면 리턴
        if (_resources.ContainsKey(path))
            return _resources[path] as T;

        // 없으면 생성후 캐싱후 리턴
        T obj = Resources.Load<T>(path);
        _resources.Add(path, obj);

        return obj;
    }

    public GameObject Instantiate(string key, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>(key);
        if(prefab == null)
        {
            Debug.LogWarning($"Failed to load Prefab {key}");
            return null;
        }

        GameObject go;
        if(prefab.TryGetComponent(out Pooling pooling))
        {
            go = Pop(prefab, parent);
            go.SetActive(true);
            return go;
        }

        go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;

        return go;
    }

    public void Destory(GameObject go, float time = 0.0f)
    {
        if (go == null)
            return;

        // 오브젝트 풀링
        if(go.TryGetComponent(out Pooling pooling))
        {
            // 게임오브젝트 비활성화
            go.SetActive(false);
            Push(go);
            return;
        }

        Object.Destroy(go, time);
    }

    public void Push(GameObject go)
    {
        // 게임 오브젝트 비활성화
        go.SetActive(false);
        // 매니저 자식에 위치하도록 설정
        go.transform.SetParent(Managers.Instance.gameObject.transform);

        // 키 등록되어 있으면 그대로 넣어준다.
        if (poolObjects.ContainsKey(go.name))
        {
            poolObjects[go.name].Enqueue(go);
        }
        // 키 등록이 안되어있으면 등록
        else
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            objectQueue.Enqueue(go);
            poolObjects.Add(go.name, objectQueue);
        }
    }

    GameObject Pop(GameObject prefab, Transform parent)
    {
        // 풀 오브젝트에 존재
        if (poolObjects.TryGetValue(prefab.name, out Queue<GameObject> objectQueue))
        {
            if(objectQueue.Count > 0)
            {
                GameObject gameObject = objectQueue.Dequeue();

                // 큐에 오브젝트에 여부이 있으면 그 오브젝트 리턴
                if (gameObject != null)
                {
                    return gameObject;
                }
            }
        }// 존재 하지 않을 시 풀 생성
        else
        {
            objectQueue = new Queue<GameObject>();
            poolObjects.Add(prefab.name, objectQueue);
        }

        // 여분이 없거나, 존재 하지 않을 시 새로 생성해서 보낸다.
        GameObject createObj = Object.Instantiate(prefab, parent);
        createObj.name = prefab.name;
        // 매니저 자식에 위치하도록 설정
        createObj.transform.SetParent(Managers.Instance.gameObject.transform);
        return createObj;
    }
}
