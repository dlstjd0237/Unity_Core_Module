using UnityEngine;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent<T>(out T outComponent) == false)
            return gameObject.AddComponent<T>();
        return outComponent;
    }

    public static bool IsValid(this GameObject gameObject)
    {
        return gameObject != null && gameObject.activeSelf;
    }

    public static bool IntToBool(this int value)
    {
        return value == 0 ? false : true;
    }

    public static int BoolIsInt(this bool value)
    {
        return value ? 1 : 0;
    }
}
