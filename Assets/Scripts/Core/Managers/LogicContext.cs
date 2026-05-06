using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicContext : MonoSingleton<LogicContext>
{
    public static ResourceManager ResourceManager { get; private set; } = new ResourceManager();
    public static UIManager UIManager { get; private set; } = new UIManager();

    private List<IManager> managers = new List<IManager>()
    {
        ResourceManager, UIManager
    };

    private void Awake()
    {
        UIManager.SetCanvas(gameObject);

        for (int i = 0; i < managers.Count; ++i)
        {
            managers[i].Initialization();
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < managers.Count; ++i)
        {
            managers[i].OnEnable();
        }
    }

    protected override void OnDisable()
    {
        for (int i = 0; i < managers.Count; ++i)
        {
            managers[i].OnDisable();
        }
        base.OnDisable();
    }
}
