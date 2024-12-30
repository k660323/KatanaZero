using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public Dictionary<string, Queue<GameObject>> poolObjects { get; protected set; } = new Dictionary<string, Queue<GameObject>>();

    Dictionary<string, Object> _resources = new Dictionary<string, Object>();

    public T Load<T>(string path) where T : Object
    {
        // ĳ�̵Ǿ������� ����
        if (_resources.ContainsKey(path))
            return _resources[path] as T;

        // ������ ������ ĳ���� ����
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

        // ������Ʈ Ǯ��
        if(go.TryGetComponent(out Pooling pooling))
        {
            // ���ӿ�����Ʈ ��Ȱ��ȭ
            go.SetActive(false);
            Push(go);
            return;
        }

        Object.Destroy(go, time);
    }

    public void Push(GameObject go)
    {
        // ���� ������Ʈ ��Ȱ��ȭ
        go.SetActive(false);
        // �Ŵ��� �ڽĿ� ��ġ�ϵ��� ����
        go.transform.SetParent(Managers.Instance.gameObject.transform);

        // Ű ��ϵǾ� ������ �״�� �־��ش�.
        if (poolObjects.ContainsKey(go.name))
        {
            poolObjects[go.name].Enqueue(go);
        }
        // Ű ����� �ȵǾ������� ���
        else
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            objectQueue.Enqueue(go);
            poolObjects.Add(go.name, objectQueue);
        }
    }

    GameObject Pop(GameObject prefab, Transform parent)
    {
        // Ǯ ������Ʈ�� ����
        if (poolObjects.TryGetValue(prefab.name, out Queue<GameObject> objectQueue))
        {
            if(objectQueue.Count > 0)
            {
                GameObject gameObject = objectQueue.Dequeue();

                // ť�� ������Ʈ�� ������ ������ �� ������Ʈ ����
                if (gameObject != null)
                {
                    return gameObject;
                }
            }
        }// ���� ���� ���� �� Ǯ ����
        else
        {
            objectQueue = new Queue<GameObject>();
            poolObjects.Add(prefab.name, objectQueue);
        }

        // ������ ���ų�, ���� ���� ���� �� ���� �����ؼ� ������.
        GameObject createObj = Object.Instantiate(prefab, parent);
        createObj.name = prefab.name;
        // �Ŵ��� �ڽĿ� ��ġ�ϵ��� ����
        createObj.transform.SetParent(Managers.Instance.gameObject.transform);
        return createObj;
    }
}
