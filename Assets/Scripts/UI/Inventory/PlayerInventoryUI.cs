using System;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    public class UpdateType
    {
        public void FullUpdate()
        {
            
        }
    }

    public Player Player;
    public GameObject ContentContainer;
    public Action ItemsAction;
    private PlayerInventory data;
    private GameObject ItemPrefab;

    private void Start()
    {
        data = Player.Inventory;
        for (int i = 0; i < data.MaxCapacity; i++)
        {
            var slot = Instantiate(ItemPrefab, ContentContainer.transform);
        }
    }

    public void UpdateUI(int index = -1)
    {
        if (index > -1)
        {
            for (int i = 0; i < data.Items.Count; i++)
            {
                // Update UI
            }
        }
    }

}
