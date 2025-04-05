using System;

public class SpeedScroll : ItemBase, IUsable, ICooldown
{
    #region ItemBase Field
    public override string Name => "Speed Scroll";

    public override string Description => "Increase move and attack speed for 20 minute";

    public override int Weight => 0;

    public override int Amount { get; set; }

    public override ItemRarity Rarity => ItemRarity.Normal;

    public override Action<ItemBase> RequestRemove { get; set; }

    protected override string iconPath => "Sprites/SpeedScroll";

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
