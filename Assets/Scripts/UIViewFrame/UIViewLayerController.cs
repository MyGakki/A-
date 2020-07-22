using System.Collections.Generic;
using UnityEngine;

public class UIViewLayerController: MonoBehaviour
{
    public UIViewLayer ViewLayer;
    private const int viewOrderStep = 100;
    private int topOrder = 0;
    
    private List<UIViewBase> views = new List<UIViewBase>();

    public void Init(params object[] args)
    {
        UIViewManager.Instance.MgrLog($"初始化{ViewLayer}");
    }
    
    public void Push(UIViewBase view)
    {
        if (view.LayerController != null)
        {
            if (view.ViewOrder == topOrder)
            {
                return;
            }
            else
            {
                views.Remove(view);
                views.Add(view);
                topOrder += viewOrderStep;
                view.ViewOrder = topOrder;
            }
        }
        else
        {
            views.Add(view);
            topOrder += viewOrderStep;
            PushSingleView(view);
        }
    }

    public void Popup(UIViewBase view)
    {
        if (view == null)
            return;

        bool error = true;
        for (int i = views.Count - 1; i >= 0; i--)
        {
            if (views[i].GetInstanceID() == view.GetInstanceID())
            {
                views.RemoveAt(i);
                PopupSingleView(view);
                error = false;
                break;
            }
        }

        if (error)
        {
            return;
        }

        RefreshTopOrder();
    }

    public UIViewBase[] PopupAll()
    {
        if (views.Count == 0)
        {
            return null;
        }

        UIViewBase current = null;
        UIViewBase[] allViews = views.ToArray();

        for (int i = views.Count - 1; i >= 0; i--)
        {
            current = views[i];
            views.RemoveAt(i);
            PopupSingleView(current);
        }

        topOrder = 0;
        return allViews;
    }

    private void PushSingleView(UIViewBase view)
    {
        if (view != null)
        {
            view.LayerController = this;
            view.OnPush();
            view.ViewOrder = topOrder;
        }
    }
    
    //弹出单个界面
    private void PopupSingleView(UIViewBase view)
    {
        if (view != null)
        {
            view.ViewOrder = 0;
            view.LayerController = null;
            view.OnPopup();
        }
    }
    
    private void RefreshTopOrder()
    {
        if (views.Count == 0)
            topOrder = 0;
        else
            topOrder = views[views.Count - 1].ViewOrder;
    }
}
