using UnityEngine;

public class BaseManager<T>: MonoBehaviourSingleton<T> where T: BaseManager<T>
{
    [SerializeField] public bool DebugMode = false;
    
    public virtual string MgrName => "BaseManager";

    public virtual void InitManager()
    {
        Debug.Log($"-->{MgrName}<-- 初始化");
    }

    public void MgrLog(string info)
    {
        if (DebugMode)
        {
            Debug.Log($"{MgrName} :::: {info}");
        }
    }
}