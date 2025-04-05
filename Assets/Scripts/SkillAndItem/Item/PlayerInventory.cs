using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory 
{
    private Player myPlayer;
    private List<ItemBase> removeList;

    public readonly List<ItemBase> Items;

    public readonly Dictionary<string, ItemBase> CooldownItems;
    public readonly Dictionary<string, ItemBase> UsableItems;

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

        removeList = new();
        CooldownItems = new();
        UsableItems = new();
    }

    public ItemBase GetCooldownItem(string name)
    {
        CooldownItems.TryGetValue(name, out var result);
        return result;
    }

    public ItemBase GetUsableItem(string name)
    {
        UsableItems.TryGetValue(name, out var result);
        return result;
    }

    public void SetTypeCooldown(CooldownType type)
    {
        foreach (var item in CooldownItems)
        {
            if (item.Value is ICooldown cooldownItem)
            {
                if (cooldownItem.MyCooldownType == type && cooldownItem.CurrentCooltime < cooldownItem.SetTypeUseCooltime())
                {
                    cooldownItem.CurrentCooltime = cooldownItem.SetTypeUseCooltime();
                }
            }
        }
    }

    public void CooldownFlow()
    {
        foreach (var item in CooldownItems)
        {
            if (item.Value is ICooldown cooldownItem)
            {
                if (cooldownItem.CurrentCooltime > 0.0f)
                {
                    cooldownItem.CurrentCooltime -= Time.deltaTime;
                }
                else
                {
                    cooldownItem.CurrentCooltime = 0.0f;

                    if (cooldownItem.IsAutoUse)
                    {
                        (item.Value as IUsable).MyAction.Invoke();
                    }
                    else if(item.Value.Amount <= 0)
                    {
                        removeList.Add(item.Value);
                    }
                }
            }
        }

        while (removeList.Count > 0)
        {
            var index = removeList.Count - 1;
            var name = removeList[removeList.Count - 1].Name;

            CooldownItems.Remove(name);
            removeList.RemoveAt(index);
        }
    }

    public void AddItem(ItemBase newItem, int amount = 1)
    {
        if (newItem.Amount <= 0)
        {
            Items.Add(newItem);

            OnAddNewItem?.Invoke(newItem);
        }

        newItem.AddItem(amount);
        newItem.RequestRemove = RemoveItem;

        if (!CooldownItems.ContainsKey(newItem.Name))
        {
            if (newItem is ICooldown cooldown)
            {
                CooldownItems.Add(newItem.Name, newItem);

                cooldown.SetTypeCooldown = SetTypeCooldown;
            }
        }

        if (!UsableItems.ContainsKey(newItem.Name))
        {
            if (newItem is IUsable usable)
            {
                UsableItems.Add(newItem.Name, newItem);

                usable.MyAction = () => usable.UseAction(myPlayer);
            }
        }
    }

    public void RemoveItem(ItemBase item)
    {
        if (item is ICooldown cooldown)
        {
            if (cooldown.CurrentCooltime <= 0)
            {
                CooldownItems.Remove(item.Name);
            }
        }

        UsableItems.Remove(item.Name);
        Items.Remove(item);
    }
}
