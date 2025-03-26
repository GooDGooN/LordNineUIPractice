using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemAndSkillSlotUI : MonoBehaviour
{
    public Image ChargeLineImage;
    public Image TargetIcon;
    public Image CooltimeCover;

    public TMP_Text AmountText;
    public TMP_Text CooltimeText;

    public GameObject ItemInfoParent;
    public GameObject FocusButtonParent;
    public GameObject CooldownParent;

    public ItemBase MyItem;

    public Action UseItem;
    public Action<ItemAndSkillSlotUI> PopSlot;
    public Action<ItemAndSkillSlotUI> SetFocusItem;
    public Func<ItemAndSkillSlotUI, bool> IsOnFucusing; 

    private void Update()
    {
        if (MyItem != null)
        {
            ItemInfoParent.SetActive(true);

            AmountText.text = $"{MyItem.Amount}";
            TargetIcon.sprite = MyItem.IconSprite;

            if (MyItem is ICooldown cooldown)
            {
                ChargeLineImage.fillAmount = cooldown.CurrentCooltime / cooldown.MyUseCooltime;
                if (cooldown.CurrentCooltime > 0.0f)
                {
                    CooldownParent.SetActive(true);
                    FocusButtonParent.SetActive(false);

                    var text = (cooldown.CurrentCooltime) switch
                    {
                        (> 60.0f) => $"{(int)cooldown.CurrentCooltime / 60}min",
                        (> 1.0f) => $"{(int)cooldown.CurrentCooltime}s",
                        _ => $"{cooldown.CurrentCooltime:F1}s"
                    };
               
                    CooltimeText.text = text;
                }
                else
                {
                    if (IsOnFucusing.Invoke(this))
                    {
                        FocusButtonParent.SetActive(true);
                    }
                    CooldownParent.SetActive(false);
                }
            }
        }
        else
        {
            ChargeLineImage.fillAmount = 0.0f;
            ItemInfoParent.SetActive(false);
            UseItem = null;
        }
    }

    public void OnClick()
    {
        if (FocusButtonParent.activeSelf)
        {
            UseItem?.Invoke();

            if (MyItem?.Amount <= 0)
            {
                MyItem = null;
                PopSlot.Invoke(this);

                if (IsOnFucusing.Invoke(this))
                {
                    SetFocusItem.Invoke(null);
                }
            }
        }
        else
        {
            FocusButtonParent.SetActive(true);

            if (MyItem != null)
            {
                SetFocusItem?.Invoke(this);
            }
        }
    }

    public void OutFucus()
    {
        FocusButtonParent.SetActive(false);
    }


}
