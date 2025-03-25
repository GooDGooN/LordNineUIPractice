using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthPotion : ItemBase, IUsable, ICooldown
{
    public ActionType MyActionType => ActionType.Use;
    public CooldownType MyCooldownType => CooldownType.Health;
    public override ItemRarity Rarity => ItemRarity.Normal;

    public float UseCooltime => 1.0f;
    public float CurrentCoolDown { get; set; }

    public override string Name => "ȸ�� ����";
    public override string Description => "���� ü���� 100 ȸ���Ѵ�.";
    public override int Weight => 1;
    public override int Amount { get; set; }

    public Func<Player, int> MyAction => UseAction;

    private int UseAction(Player player)
    {
        if (CurrentCoolDown <= 0.0f)
        {
            CurrentCoolDown = UseCooltime;
            Amount -= 1;
            player.GetHeal(100);
        }
        return Amount;
    }
}
