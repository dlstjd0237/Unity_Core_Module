using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static object s_lockObject = new object();
    private static T s_instance = null;
    private static bool s_isQuitting = false;

    public static T Instance
    {
        get
        {
            lock (s_lockObject)
            {
                if (s_isQuitting == true)
                    return null;

                if (s_instance == null)
                {
                    s_instance = GameObject.Instantiate(Resources.Load<T>($"{nameof(MonoSingleton<T>)}" + typeof(T).Name));
                    DontDestroyOnLoad(s_instance.gameObject);
                }

                return s_instance;
            }
        }
    }

    protected virtual void OnDisable()
    {
        s_isQuitting = true;
        s_instance = null;
    }
}