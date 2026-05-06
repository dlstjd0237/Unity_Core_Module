using UnityEngine;
using UnityEngine.UI;
using System;

public class UIContentsBase : UIBase
{
    [SerializeField] protected Button[] closeButtons = null;

    public event Action ContentsCloseEvent = null;

    protected override void BindUIEvent()
    {
        base.BindUIEvent();
        if (closeButtons == null || closeButtons.Length == 0) return;

        for (int i = 0; i < closeButtons.Length; ++i)
        {
            var button = closeButtons[i];
            BindEvent(button, Close);
        }
    }

    protected override void UnBindUIEvent()
    {
        if (closeButtons == null || closeButtons.Length == 0) return;
        for (int i = 0; i < closeButtons.Length; ++i)
        {
            var button = closeButtons[i];
            UnbindEvent(button, Close);
        }
    }

    protected virtual void UpdateContents()
    {

    }

    public virtual void Close()
    {
        ContentsCloseEvent?.Invoke();
    }
}