using System;
using TMPro;
using UnityEngine;

using Random = UnityEngine.Random;


public enum TestItemType
{
    Potion,
}

public class UITest : MonoBehaviour
{
    public Player MyPlayer;
    public TestItemType ItemType;

    public TMP_Text HealthText;
    public TMP_Text ManaText;

    private void Update()
    {
        HealthText.text = $"HP: {MyPlayer.Health}";
        ManaText.text = $"MP: {MyPlayer.Mana}";
    }

    public void AddRandomItem()
    {
        var max = Enum.GetNames(typeof(TestItemType));
        var rn = (TestItemType)Random.Range(0, max.Length);
        var amount = Random.Range(1, 2);

        switch (rn)
        {
            case TestItemType.Potion: MyPlayer.Inventory.AddItem<HealthPotion>(amount); break;
        }
    }
}

