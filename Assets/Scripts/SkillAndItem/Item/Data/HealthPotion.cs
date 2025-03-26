public class HealthPotion : ItemBase, IUsable<Player>, ICooldown
{
    #region ItemBase Field
    public override string Name => "Health Potion";

    public override string Description => "Regerate 100 HP";

    public override int Weight => 1;

    public override int Amount { get; set; }

    public override ItemRarity Rarity => ItemRarity.Normal;

    protected override string iconPath => "Sprites/Potion";
    #endregion

    #region IUsable Field
    public ActionType MyActionType => ActionType.Use;
    #endregion

    #region ICoolDown Field
    public CooldownType MyCooldownType => CooldownType. Health;

    public float MyUseCooltime => 1.0f;

    public float CurrentCooltime { get; set; }
    #endregion

    public int UseAction(Player target)
    {
        if (CurrentCooltime <= 0.0f)
        {
            CurrentCooltime = MyUseCooltime;
            target.GetHeal(100);
            Amount -= 1;
        }
        return Amount;
    }
}
