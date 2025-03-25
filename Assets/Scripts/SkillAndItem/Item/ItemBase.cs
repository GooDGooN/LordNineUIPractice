using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class ItemBase
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int Weight { get; }
    public abstract int Amount { get; set; }
    public abstract ItemRarity Rarity { get; }

    public virtual void AddItem(int amount)
    {
        Amount += amount;
    }
}
