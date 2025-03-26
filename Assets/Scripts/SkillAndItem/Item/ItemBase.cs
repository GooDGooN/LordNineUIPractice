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

    protected abstract string iconPath { get; }
    public virtual Sprite IconSprite { get; protected set; }
    public ItemBase() 
    {
        IconSprite = Resources.Load<Sprite>(iconPath);
    }

    public virtual void AddItem(int amount)
    {
        Amount += amount;
    }
}
