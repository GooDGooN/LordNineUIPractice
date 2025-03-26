using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthPotion : ItemBase, IUsable<Player>, ICooldown
{
    public ActionType MyActionType => ActionType.Use;
    public CooldownType MyCooldownType => CooldownType.Health;
    public override ItemRarity Rarity => ItemRarity.Normal;

    public float MyUseCooltime => 1.0f;
    public float CurrentCooltime { get; set; }

    public override string Name => "ȸ�� ����";
    public override string Description => "���� ü���� 100 ȸ���Ѵ�.";
    public override int Weight => 1;
    public override int Amount { get; set; }
    protected override string iconPath => "Sprites/Potion";

    public Func<Player, int> MyAction => UseAction;

    public int UseAction(Player player)
    {
        if (CurrentCooltime <= 0.0f)
        {
            CurrentCooltime = MyUseCooltime;
            Amount -= 1;
            player.GetHeal(100);
        }
        return Amount;
    }
}
