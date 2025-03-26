using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlotUI : SlotUIBase<ItemBase>
{
    public Action<ItemBase> UseItem;
    public Action<InventorySlotUI> PopSlot;
    public Action<InventorySlotUI> SetFocusItem;
    public Func<InventorySlotUI, bool> IsOnFucusing; 

    private void Update()
    {
        if (MyItem != null)
        {
            ItemInfoParent.SetActive(true);

            AmountText.text = $"{MyItem.Amount}";
            Icon.sprite = MyItem.IconSprite;

            if (MyItem is ICooldown cooldown)
            {
                ChargeLine.fillAmount = cooldown.CurrentCooltime / cooldown.MyUseCooltime;

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
            ChargeLine.fillAmount = 0.0f;
            ItemInfoParent.SetActive(false);
            UseItem = null;
        }
    }

    public void OnClick()
    {
        if (FocusButtonParent.activeSelf)
        {
            UseItem?.Invoke(MyItem);

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
