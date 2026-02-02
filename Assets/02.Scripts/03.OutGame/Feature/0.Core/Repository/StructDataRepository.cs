using System;
using System.Collections.Generic;
using UnityEngine;

public class StructDataRepository<TKey, TValue> : IGameDataRepository<TKey, TValue>
    where TKey : Enum
    where TValue : struct
{
    private readonly Dictionary<TKey, TValue> _data = new();

    public TValue Get(TKey key)
    {
        if (!_data.ContainsKey(key))
            throw new KeyNotFoundException($"Key {key} not found in {GetType().Name}");

        return _data[key];
    }

    public void Set(TKey key, TValue value)
    {
        _data[key] = value;
    }

    public bool TryGet(TKey key, out TValue value)
    {
        return _data.TryGetValue(key, out value);
    }

    public IEnumerable<TKey> GetAllKeys() => _data.Keys;
}