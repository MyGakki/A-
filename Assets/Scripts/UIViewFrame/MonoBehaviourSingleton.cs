using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    public void Awake()
    {
        instance = this as T;
    }
}