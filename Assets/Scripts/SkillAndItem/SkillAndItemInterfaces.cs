using System;
using UnityEngine;


interface IUsable<T>
{
    public ActionType MyActionType { get; }
    
    public void UseAction();

    public Func<T, int> MyAction { get; }
}
interface ICooldown
{
    public CooldownType MyCooldownType { get; }

    public void SetCooldown(float time)
    {
        CurrentCoolDown = time;
    }
    
    public float UseCooltime { get; }

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

    public float CurrentCoolDown { get; set; }

}
