using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory 
{
    private Player myPlayer;
    public readonly List<ItemBase> Items;
    public readonly List<ItemBase> CooldownItems;

    public int MaxCapacity = 500;
    public int CurrentCapacity = 200;

    public ItemBase this [int index]
    {
        get => Items[index];
    }

    public PlayerInventory(Player player)
    {
        myPlayer = player;
        Items = new(CurrentCapacity);
        CooldownItems = new();
    }

    public void UseItem(int index) 
    {
        if (Items[index] is IUsable<Player> item)
        {
            var remain = item.MyAction.Invoke(myPlayer);
            if(item is ICooldown cooldownItem)
            {
                SetTypeCooldown(cooldownItem.MyCooldownType, 1.0f);
            }
            if (remain <= 0)
            {
                Items.RemoveAt(index);
                CooldownItems.Remove(Items[index]);
            }
        }
    }

    public void SetTypeCooldown(CooldownType type, float value)
    {
        foreach (var item in CooldownItems)
        {
            if (item is ICooldown cooldownItem)
            {
                if (cooldownItem.MyCooldownType == type && cooldownItem.CurrentCoolDown == 0.0f)
                {
                    cooldownItem.SetTypeUseCooltime();
                }
            }
        }
    }

    public void CooldownFlow()
    {
        for (int i = 0; i < CooldownItems.Count; i++)
        {
            if (CooldownItems[i] is ICooldown cooldownItem)
            {
                if (cooldownItem.CurrentCoolDown > 0.0f)
                {
                    cooldownItem.CurrentCoolDown -= Time.deltaTime;
                }
                else
                {
                    cooldownItem.CurrentCoolDown = 0.0f;
                }
            }
        }
    }
}
