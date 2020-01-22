using System.Collections.Generic;
using Sirenix.Serialization;

public abstract class RuntimeSet<T> : SingletonScriptableObject<RuntimeSet<T>>
{
    [OdinSerialize] private List<T> Items = new List<T>();

    public static void Add(T thing)
    {
        if (!Instance.Items.Contains(thing))
            Instance.Items.Add(thing);
    }

    public static void Remove(T thing)
    {
        if (Instance.Items.Contains(thing))
            Instance.Items.Remove(thing);
    }

    public static void Clear()
    {
        Instance.Items.Clear();
    }
}