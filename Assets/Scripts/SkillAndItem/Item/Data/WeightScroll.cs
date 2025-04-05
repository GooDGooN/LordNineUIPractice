using System;

public class WeightScroll : ItemBase, IUsable, ICooldown
{
    #region ItemBase Field
    public override string Name => "Weight Scroll";

    public override string Description => "Increase maximum carry weight limit for 20 minute";

    public override int Weight => 0;

    public override int Amount { get; set; }

    public override Action<ItemBase> RequestRemove { get; set; }

    public override ItemRarity Rarity => ItemRarity.Normal;

    protected override string iconPath => "Sprites/WeightScroll";

    #endregion

    #region IUsable Field
    public ActionType MyActionType => ActionType.Use;

    public object Target { get; set; }

    public Func<int> MyAction { get; set; }

    #endregion

    #region ICoolDown Field
    public CooldownType MyCooldownType => CooldownType.Scroll;

    public Action<CooldownType> SetTypeCooldown { get; set; }

    public float MaxCooltime => 5.0f;

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
