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

    public Player AddItemToInventory(ItemBase item, int amount)
    {
        Inventory.AddItem(item, amount);

        return this;
    }
}