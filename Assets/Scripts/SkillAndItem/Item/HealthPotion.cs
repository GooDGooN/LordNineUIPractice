using System.Collections;
using UnityEngine;

public class HealthPotion : ItemBase, IUsable, ICooldown
{
    public ActionType MyActionType => ActionType.Use;
    public CooldownType MyCooldownType => CooldownType.Health;
    public override ItemRarity Rarity => ItemRarity.Normal;

    public float MyCooltime => 3.0f;
    public float RemainCoolDown { get; set; }

    public override string Name => "회복 물약";
    public override string Description => "사용시 체력을 100 회복한다.";
    public override int Weight => 1;
    public override int Amount { get; set; }

    public void AddItem(int amount)
    {
        Amount += amount;
    }

    public void UseAction()
    {
        if (RemainCoolDown <= 0.0f)
        {
            RemainCoolDown = MyCooltime;
            Amount -= 1;
        }
    }

    public IEnumerable Test()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
