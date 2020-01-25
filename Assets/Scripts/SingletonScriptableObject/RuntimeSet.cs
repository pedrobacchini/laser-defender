using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public abstract class RuntimeSet<T> : SingletonScriptableObject<RuntimeSet<T>>
{
    
    [OdinSerialize] [ReadOnly] private List<T> _items = new List<T>();
    public static List<T> Items => Instance._items;

    public static void Add(T thing)
    {
        if (!Instance._items.Contains(thing))
            Instance._items.Add(thing);
    }

    public static void Remove(T thing)
    {
        if (Instance._items.Contains(thing))
            Instance._items.Remove(thing);
    }
}