using System;

interface IUsable
{
    public ActionType MyActionType { get; }

    public int UseAction(object target);

    public object Target { get; set; }

    public Func<int> MyAction { get; set; }

    public void InitializeUsable(object target)
    {
        Target = target;
        MyAction = () => UseAction(target);
    }
}

interface ICooldown
{
    public CooldownType MyCooldownType { get; }

    public Action<CooldownType> SetTypeCooldown { get; set; }

    public void SetCooldown(float time)
    {
        CurrentCooltime = time;
    }
    
    public float MaxCooltime { get; }

    public float CurrentCooltime { get; set; }

    public bool IsAutoUse { get; set; }

    public float SetTypeUseCooltime()
    {
        switch (MyCooldownType)
        {
            case CooldownType.HoningStone:
            case CooldownType.Scroll:
            case CooldownType.Oil: 
                return 0.5f;

            case CooldownType.ActiveSkill:
            case CooldownType.ActiveAbility: 
                return 2.0f;

            case CooldownType.Elixer: 
                return 120.0f;

            default: return 1.0f; // health potion, ects..
        }
    }
}
