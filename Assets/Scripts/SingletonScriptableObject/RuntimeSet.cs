using System.Collections.Generic;
using Sirenix.OdinInspector;

public abstract class RuntimeSet<T> : SingletonScriptableObject<RuntimeSet<T>>
{
    private readonly List<T> _items = new List<T>();

    [ReadOnly] public static List<T> Items => Instance._items;

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

    public static void Clear()
    {
        Instance._items.Clear();
    }
}