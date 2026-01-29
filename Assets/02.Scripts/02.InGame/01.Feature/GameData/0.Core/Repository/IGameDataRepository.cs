using System;
using System.Collections.Generic;
using UnityEngine;

// 제네릭 Repository 인터페이스
public interface IGameDataRepository<TKey, TValue>
    where TKey : Enum
{
    TValue Get(TKey key);
    void Set(TKey key, TValue value);
    bool TryGet(TKey key, out TValue value);
    IEnumerable<TKey> GetAllKeys();
}
