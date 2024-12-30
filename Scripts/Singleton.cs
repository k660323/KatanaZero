using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    public static T Instance
    {
        get 
        { 
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if(instance == null)
                {
                    GameObject managerObject = new GameObject(typeof(T).Name, typeof(T));
                    instance = managerObject.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (Instance != this as T)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
