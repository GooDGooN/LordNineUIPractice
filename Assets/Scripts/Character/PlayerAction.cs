using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Player
{
    public Player GetDamage(int value)
    {
        Health = Math.Clamp(Health - value, 0, MaxHealth);
        return this;
    }

    public Player GetHeal(int value)
    {
        Health = Math.Clamp(Health + value, 0, MaxHealth);
        return this;
    }

    public Player AddItemToInventory<T>(int amount) where T : ItemBase, new()
    {
        Inventory.AddItem<T>(amount);
        return this;
    }
}