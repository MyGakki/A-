using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UIViewState
{
    Invisible,
    Visible,
    Cache,
}

public class UIViewBase : MonoBehaviour
{
    public SO_UIViewConfig Config;

    [HideInInspector] public UIViewLayerController LayerController;

    private UIViewState viewState;

    public UIViewState ViewState
    {
        get => viewState;
        set => viewState = value;
    }

    private bool dirty;

    protected Canvas canvas;

    public int ViewOrder
    {
        get => canvas.sortingOrder;
        set => canvas.sortingOrder = value;
    }

    public void Init()
    {
        InitCanvas();
        InitUIObjects();
        InitBG();
    }

    private void InitCanvas()
    {
        canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
        }

        GraphicRaycaster raycaster = GetComponent<GraphicRaycaster>();
        if (raycaster == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();
        }
    }
    
    protected virtual void InitUIObjects() {}

    protected void InitBG()
    {
        if (Config.bgTriggerClose)
        {
            Transform bgTran = transform.Find("BG");
            if (bgTran == null)
            {
                GameObject bgObj = new GameObject("BG", typeof(RectTransform));
                bgTran = bgObj.transform;
                bgTran.SetParent(transform);
                bgTran.SetAsFirstSibling();
                RectTransform rt = bgObj.GetComponent<RectTransform>();
                rt.Normalize();
            }
            //查看BG上是否存在图片
            Image image = bgTran.GetComponent<Image>();
            if (image == null)
            {
                image = bgTran.gameObject.AddComponent<Image>();
                image.color = new Color(0, 0, 0, 0);
                CanvasRenderer cr = bgTran.GetComponent<CanvasRenderer>();
                cr.cullTransparentMesh = true;
            }

            image.raycastTarget = true;
            //BG是否存在点击事件
            EventTrigger eventTrigger = bgTran.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = bgTran.gameObject.AddComponent<EventTrigger>();
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(CloseWithEvent);
            eventTrigger.triggers.Add(entry);
        }
    }
    

    public void SetArguments(params object[] args)
    {
        dirty = true;
        UpdateArguments(args);

        if (LayerController == null)
        {
            return;
        }

        if (Config.alwaysUpdate || viewState == UIViewState.Visible)
        {
            UpdateView();
        }
    }

    protected virtual void UpdateArguments(params object[] args) {}
    
    //虚函数，不同界面有不同实现
    public virtual void UpdateView()
    {
        dirty = false;
    }
    
    //被压入窗口栈中
    public virtual void OnPush()
    {
        ViewState = UIViewState.Invisible;
        UpdateLayer();
    }

    public virtual void OnShow()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
            UpdateLayer();
        }

        if (ViewState != UIViewState.Visible)
        {
            Vector3 pos = transform.localPosition;
            pos.z = 0;
            transform.localPosition = pos;

            ViewState = UIViewState.Visible;
        }

        if (dirty)
            UpdateView();
    }
    
    public virtual void OnHide()
    {
        if (ViewState == UIViewState.Visible)
        {
            Vector3 pos = transform.localPosition;
            pos.z = 99999;
            transform.localPosition = pos;

            ViewState = UIViewState.Invisible;
        }
    }
    
    public virtual void OnPopup()
    {
        if (ViewState == UIViewState.Cache)
            return;
        
        if (ViewState == UIViewState.Visible)
            OnHide();

        ViewState = UIViewState.Cache;
    }
    
    public virtual void OnExit()
    {
        //如果不是缓存池状态，则需要先弹出
        if (ViewState != UIViewState.Cache)
            OnPopup();
    }
    
    public void Close()
    {
        UIViewManager.Instance.HideView(this);
    }
    
    private void UpdateLayer()
    {
        if (canvas.overrideSorting == false)
        {
            canvas.overrideSorting = true;
            switch (Config.viewLayer)
            {
                case UIViewLayer.Background:
                    canvas.sortingLayerID = SortingLayer.NameToID("View_Background");
                    break;
                case UIViewLayer.Base:
                    canvas.sortingLayerID = SortingLayer.NameToID("View_Base");
                    break;
                case UIViewLayer.HUD:
                    canvas.sortingLayerID = SortingLayer.NameToID("View_HUD");
                    break;
                case UIViewLayer.Popup:
                    canvas.sortingLayerID = SortingLayer.NameToID("View_Popup");
                    break;
                case UIViewLayer.Top:
                    canvas.sortingLayerID = SortingLayer.NameToID("View_Top");
                    break;
                case UIViewLayer.Debug:
                    canvas.sortingLayerID = SortingLayer.NameToID("View_Debug");
                    break;
                default:
                    break;
            }
        }
    }
    
    
    //点击背景关闭窗口的回调
    protected virtual void CloseWithEvent(BaseEventData eventData)
    {
        UIViewManager.Instance.HideView(this);
    }

}

public static class UnityUtility
{
    public static void Normalize(this RectTransform rectTransform)
    {
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;
    }
}
