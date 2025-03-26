public class SpeedScroll : ItemBase, IUsable<Player>, ICooldown
{
    #region ItemBase Field
    public override string Name => "Speed Scroll";

    public override string Description => "Increase move and attack speed for 20 minute";

    public override int Weight => 0;

    public override int Amount { get; set; }

    public override ItemRarity Rarity => ItemRarity.Normal;

    protected override string iconPath => "Sprites/SpeedScroll";

    #endregion

    #region IUsable Field
    public ActionType MyActionType => ActionType.Use;
    #endregion

    #region ICoolDown Field
    public CooldownType MyCooldownType => CooldownType.Scroll;

    public float MyUseCooltime => 5.0f;

    public float CurrentCooltime { get; set; }
    #endregion

    public int UseAction(Player target)
    {
        if (CurrentCooltime <= 0.0f)
        {
            CurrentCooltime = MyUseCooltime;
            Amount -= 1;
        }
        return Amount;
    }
}
