using UnityEngine.EventSystems;

public partial class UIBase : UIBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        BindUIEvent();
    }

    protected override void OnDisable()
    {
        UnBindUIEvent();
        base.OnDisable();
    }

    protected virtual void Initialize()
    {

    }

    protected virtual void BindUIEvent()
    {

    }

    protected virtual void UnBindUIEvent()
    {

    }
}