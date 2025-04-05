using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlotUI : SlotUIBase<ItemBase>
{
    public Action<InventorySlotUI> PopSlot;
    public Action<InventorySlotUI> SetFocusItem;
    public Func<InventorySlotUI, bool> IsOnFucusing; 

    protected override void Update()
    {
        base.Update();
        if (ItemName != "")
        {
            var cooltime = GetCooltime.Invoke();

            if (cooltime <= 0 && IsOnFucusing.Invoke(this))
            {
                FocusButtonParent.SetActive(true);
            }

            if (GetAmount.Invoke() <= 0)
            {
                UnBindItem();

                PopSlot.Invoke(this);

                if (IsOnFucusing.Invoke(this))
                {
                    SetFocusItem.Invoke(null);
                }
            }
        }
    }

    public void OnClick()
    {
        if (FocusButtonParent.activeSelf)
        {
            UseItem?.Invoke();
        }
        else
        {
            FocusButtonParent.SetActive(true);

            if (ItemName != "")
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
