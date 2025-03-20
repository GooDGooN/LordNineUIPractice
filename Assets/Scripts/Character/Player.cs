using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInventory Inventory;

    public int Health = 10000;
    public int Mana = 1000;

    private void Awake()
    {
        Inventory = new();
    }
}
