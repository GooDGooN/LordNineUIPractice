using TMPro;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public PlayerInventory Inventory;

    public int MaxHealth = 10000;
    public int Health = 10000;
    public int Mana = 1000;

    private void Awake()
    {
        Inventory = new(this);
    }

    private void Update()
    {
        Inventory.CooldownFlow();
    }
}
