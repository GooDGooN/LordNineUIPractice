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

    public override string Name => "회복 물약";
    public override string Description => "사용시 체력을 100 회복한다.";
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
