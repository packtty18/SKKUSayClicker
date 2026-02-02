using UnityEngine;

public abstract class LocalSingleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"[LocalSingleton] Duplicate {typeof(T).Name} destroyed");
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
        Init();

        Debug.Log($"[LocalSingleton] {typeof(T).Name} initialized");
    }

    protected abstract void Init();


    protected virtual void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
