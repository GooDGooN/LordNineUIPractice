using System;

public class HealthPotion : ItemBase, IUsable, ICooldown
{
    #region ItemBase Field
    public override string Name => "Small Health Potion";

    public override string Description => "Regerate 100 HP";

    public override int Weight => 1;

    public override int Amount { get; set; }

    public override Action<ItemBase> RequestRemove { get; set; }

    public override ItemRarity Rarity => ItemRarity.Normal;

    protected override string iconPath => "Sprites/Potion";

    #endregion

    #region IUsable Field
    public ActionType MyActionType => ActionType.Use;

    public object Target { get; set; }

    public Func<int> MyAction { get; set; }

    #endregion

    #region ICoolDown Field
    public CooldownType MyCooldownType => CooldownType. Health;

    public Action<CooldownType> SetTypeCooldown { get; set; }

    public float MaxCooltime => 1.0f;

    public float CurrentCooltime { get; set; }

    public bool IsAutoUse { get; set; }

    #endregion

    public int UseAction(object target)
    {
        if (target is Player player)
        {
            if (CurrentCooltime <= 0.0f)
            {
                CurrentCooltime = MaxCooltime;
                Amount -= 1;

                player.GetHeal(100);
                SetTypeCooldown.Invoke(MyCooldownType);
            }

            if (Amount <= 0)
            {
                RequestRemove.Invoke(this);
            }
        }
        return Amount;
    }
}
