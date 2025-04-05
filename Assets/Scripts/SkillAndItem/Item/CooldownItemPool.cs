using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public class CooldownItemPool
{
    public Dictionary<Type, ItemBase> ItemPool = new();

    // TEMP!!

    public CooldownItemPool()
    {
        ItemPool.Add(typeof(HealthPotion), new HealthPotion());
        ItemPool.Add(typeof(RegenerateScroll), new RegenerateScroll());
        ItemPool.Add(typeof(SpeedScroll), new SpeedScroll());
        ItemPool.Add(typeof(WeightScroll), new WeightScroll());
    }

    public ItemBase GetItemFromPool<T>() where T : ItemBase
    {
        ItemPool.TryGetValue(typeof(T), out var result);
        return result;
    }

    public ItemBase GetRandomItemFromPool()
    {
        var arr = ItemPool.ToArray();
        var rn = Random.Range(0, arr.Length);
        return arr[rn].Value;
    }
}
