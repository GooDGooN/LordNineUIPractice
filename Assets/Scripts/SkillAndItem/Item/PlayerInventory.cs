using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory 
{
    public List<ItemBase> Items = new();
    public int MaxCapacity = 100;

    public ItemBase this [int index]
    {
        get => Items[index];
    }

    public void SetTypeCooldown(Object sender, CooldownType type, float value)
    {
        foreach (var item in Items)
        {
            if (sender != item)
            {
                if (item is ICooldown cooldown)
                {
                    cooldown.RemainCoolDown = value;
                }
            }
        }
    }
}
