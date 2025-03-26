using System;

/// <typeparam name="T">T is binding target</typeparam>
interface IUsable<T>
{
    public ActionType MyActionType { get; }

    public int UseAction (T target);

    public Func<T, int> MyAction => UseAction;
}
interface ICooldown
{
    public CooldownType MyCooldownType { get; }

    public void SetCooldown(float time)
    {
        CurrentCooltime = time;
    }
    
    public float MyUseCooltime { get; }

    public float CurrentCooltime { get; set; }

    public float SetTypeUseCooltime()
    {
        switch (MyCooldownType)
        {
            case CooldownType.HoningStone:
            case CooldownType.Scroll:
            case CooldownType.Oil: return 0.5f;

            case CooldownType.ActiveSkill:
            case CooldownType.ActiveAbility: return 2.0f;

            case CooldownType.Elixer: return 120.0f;

            default: return 1.0f; // health potion, ects..
        }
    }
}
