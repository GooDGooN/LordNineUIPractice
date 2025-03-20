using UnityEngine;


interface IUsable
{
    public ActionType MyActionType { get; }
    public void UseAction();
}
interface ICooldown
{
    public CooldownType MyCooldownType { get; }
    public float MyCooltime { get; }
    public float RemainCoolDown { get; set; }

}
