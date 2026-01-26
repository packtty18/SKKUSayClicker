using UnityEngine;

public abstract class GlobalSingleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
        Init();
        DontDestroyOnLoad(gameObject);

        Debug.Log($"[GlobalSingleton] {typeof(T).Name} initialized");
    }

    protected abstract void Init();
}
