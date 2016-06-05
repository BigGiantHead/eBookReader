using UnityEngine;
using System.Collections;

public class SingletonBehaviour<T> : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = GetComponent<T>();
    }

    protected virtual void OnDestroy()
    {
        instance = default(T);
    }
}
