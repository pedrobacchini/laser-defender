using Sirenix.OdinInspector;
using UnityEngine;

public class SingletonScriptableObject<T> : SerializedScriptableObject where T : SerializedScriptableObject
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance) return _instance;

            // Get all asset of type T from Resources or loaded assets.
            var objs = Resources.FindObjectsOfTypeAll<T>();

            // If no asset of type T was found...
            if (objs.Length == 0)
                Debug.LogError("No asset of type \"" + typeof(T).Name +
                               "\" has been found in loaded resources. Please create a new one and add it to the \"Preloaded Assets\" array in Edit > Project Settings > Player > Other Settings.");

            // If more than one asset of type T was found...
            else if (objs.Length > 1)
                Debug.LogError("There's more than one asset of type \"" + typeof(T).Name +
                               "\" loaded in this project. We expect it to have a Singleton behaviour. Please remove other assets of that type from this project.");

            _instance = (objs.Length > 0) ? objs[0] : null;

            return _instance;
        }
    }
}