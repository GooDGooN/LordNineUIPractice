using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Random = UnityEngine.Random;


public enum TestItemType
{
    Potion,
    SpeedScroll,
    RegenerateScroll,
    WeightScroll,

}

public class UITest : MonoBehaviour
{
    public Player MyPlayer;
    public TestItemType ItemType;

    public TMP_Text HealthText;
    public TMP_Text ManaText;

    private CooldownItemPool itemPool;

    private void Awake()
    {
        itemPool = new();
    }

    private void Update()
    {
        HealthText.text = $"HP: {MyPlayer.Health}";
        ManaText.text = $"MP: {MyPlayer.Mana}";
    }

    public void AddRandomItem()
    {
        var amount = Random.Range(1, 2);

        MyPlayer.AddItemToInventory(itemPool.GetRandomItemFromPool(), amount);
    }
}

