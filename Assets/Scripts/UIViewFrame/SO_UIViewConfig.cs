using UnityEngine;

public enum UIViewName
{
    Main,
    HpMpBar,
    DamageFigure,
    StartGame,
    TopologicalMap,
}

public enum UIViewLayer
{
    None,
    
    Background,
    Base,
    HUD,
    Popup,
    Top,
    Debug,
}

public enum UIViewCacheScheme
{
    AutoRemove,
    TempCache,
    Cache,
}

[CreateAssetMenu(menuName = "ScriptableObject/UI View Config")]
public class SO_UIViewConfig : ScriptableObject
{
    [Tooltip("是否唯一打开")] public bool unique = true;
    [Tooltip("界面名称")] public UIViewName viewName;
    [Tooltip("所在层")] public UIViewLayer viewLayer;
    [Tooltip("缓存策略")] public UIViewCacheScheme cacheScheme;
    [Tooltip("资源地址")] public string assetName;
    [Tooltip("是否遮挡整个屏幕")] public bool coverScreen;
    [Tooltip("被遮挡后是否还更新")] public bool alwaysUpdate;
    [Tooltip("点击背景后是否关闭")] public bool bgTriggerClose;

    public string Address
    {
        get { return $"界面/{assetName}.prefab";  }
    }
}