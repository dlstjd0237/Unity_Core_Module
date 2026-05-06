using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : IManager
{
    private int order = 10;

    private Stack<UIContentsBase> contentsStack = new Stack<UIContentsBase>();


    private GameObject root;

    public GameObject Root
    {
        get
        {
            if (root == null)
            {
                root = GameObject.Find("@UI_Root");

                if (root == null)
                {
                    root = new GameObject { name = "@UI_Root" };
                    SetCanvas(root);
                }
            }

            return root;
        }
    }

    public void SetCanvas(GameObject gameObject, bool sort = true, int sortOrder = 0)
    {
        Canvas canvas = gameObject.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = sort;
        

        CanvasScaler cs = gameObject.GetOrAddComponent<CanvasScaler>();
        if (cs != null)
        {
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1080, 1920);
            cs.matchWidthOrHeight = 0.5f;
        }

        gameObject.GetOrAddComponent<GraphicRaycaster>();

        if (sort == true)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else
        {
            canvas.sortingOrder = sortOrder;
        }
        
        root = gameObject;
    }

    public T MakeSupItem<T>(Transform parent, string name = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject gameObject = LogicContext.ResourceManager.Instantiate(name, parent);
        gameObject.transform.SetParent(parent, false);

        return gameObject.GetOrAddComponent<T>();
    }

    public T MakeWorldSpaceUI<T>(Transform InParent = null, string InName = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(InName))
            InName = typeof(T).Name;

        GameObject gameObject = LogicContext.ResourceManager.Instantiate(InName, InParent);
        if (InParent != null)
            gameObject.transform.SetParent(InParent, false);

        Canvas canvas = gameObject.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return gameObject.GetOrAddComponent<T>();
    }

    /// <summary>
    /// UI_GameScene
    /// Gold, Dia UI_Item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T OpenBaseUI<T>(string InName = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(InName))
            InName = typeof(T).Name;

        GameObject gameObject = LogicContext.ResourceManager.Instantiate(InName);
        T baseUI = gameObject.GetOrAddComponent<T>();

        gameObject.transform.SetParent(Root.transform, false);

        return baseUI;
    }

    public T OpenContentUI<T>(string InName = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(InName))
            InName = typeof(T).Name;

        GameObject contentUI = LogicContext.ResourceManager.Instantiate(InName, Root.transform);
        if (contentUI.TryGetComponent(out UIContentsBase OutContentsUI))
        {
            UIContentsBase popupUI = OutContentsUI;
            contentsStack.Push(popupUI);
        }

        T popup = contentUI.GetOrAddComponent<T>();
        return popup;
    }

    public void CloseContentsUI(UIContentsBase InContents)
    {
        if (contentsStack.Count == 0)
            return;

        if (contentsStack.Contains(InContents))
        {
            List<UIContentsBase> tempList = new List<UIContentsBase>(contentsStack);
            tempList.Remove(InContents);
            contentsStack = new Stack<UIContentsBase>(tempList);

            InContents.Close();
            LogicContext.ResourceManager.Destroy(InContents.gameObject);
            order--;

            Debug.Log("Selected popup closed successfully.");
        }
        else
        {
            Debug.Log("Popup not found in the stack.");
        }
    }


    public void CloseContentsUI()
    {
        if (contentsStack.Count == 0)
            return;

        UIContentsBase contents = contentsStack.Pop();
        contents.Close();
        LogicContext.ResourceManager.Destroy(contents.gameObject);
        order--;
    }


    public void CloseAllContentsUI()
    {
        while (contentsStack.Count > 0)
            CloseContentsUI();
    }

    public int GetContentsCount()
    {
        return contentsStack.Count;
    }

    public void Clear()
    {
        CloseAllContentsUI();
    }

    public void Initialization()
    {
    }

    public void OnEnable()
    {
    }

    public void OnDisable()
    {
    }
}