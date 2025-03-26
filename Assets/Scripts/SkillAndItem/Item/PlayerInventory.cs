using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory 
{
    private Player myPlayer;

    public readonly List<ItemBase> Items;
    public readonly List<ItemBase> CooldownItems;

    public int MaxCapacity = 500;
    public int CurrentCapacity = 200;

    public UnityEvent<ItemBase> OnAddNewItem = new();

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

    public void UseItem(ItemBase item) 
    {
        if (item is IUsable<Player> usable)
        {
            var remain = usable.MyAction.Invoke(myPlayer);

            if (usable is ICooldown cooldownItem)
            {
                SetTypeCooldown(cooldownItem.MyCooldownType, 1.0f);
            }

            if (remain <= 0)
            {
                CooldownItems.Remove(item);
                Items.Remove(item);
            }
        }
    }

    public void SetTypeCooldown(CooldownType type, float value)
    {
        foreach (var item in CooldownItems)
        {
            if (item is ICooldown cooldownItem)
            {
                if (cooldownItem.MyCooldownType == type && cooldownItem.CurrentCooltime == 0.0f)
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
                if (cooldownItem.CurrentCooltime > 0.0f)
                {
                    cooldownItem.CurrentCooltime -= Time.deltaTime;
                }
                else
                {
                    cooldownItem.CurrentCooltime = 0.0f;
                }
            }
        }
    }

    public void AddItem<T>(int amount = 1) where T : ItemBase, new()
    {
        var index = Items.FindIndex(elem => elem.GetType() == typeof(T));

        if (index >= 0)
        {
            Items[index].AddItem(amount);
        }
        else
        {
            var newItem = new T();

            newItem.AddItem(amount);
            Items.Add(newItem);
            if(newItem is ICooldown)
            {
                CooldownItems.Add(newItem);
            }
            OnAddNewItem?.Invoke(newItem);
        }
    }
}
