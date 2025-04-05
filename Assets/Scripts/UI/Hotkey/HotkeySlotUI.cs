using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class HotkeySlotUI : SlotUIBase<Object>, IPointerDownHandler
{
    public Func<HotkeySlotUI, bool> RegistUsableObject;

    public Func<bool> IsInventoryActivated;

    public Func<HotkeySlotUI> GetFocus;
    public Action<HotkeySlotUI> SetFocus;
    public Action<HotkeySlotUI> SwapSlot;

    public GameObject OutOfStockParent;
    public GameObject UnbindButtonParent;
    public GameObject SlotParent;

    private Vector3 clickPosition;

    private bool isClicked = false;

    protected override void Update()
    {
        base.Update();

        if (isClicked)
        {
            var spriteHeight = Icon.sprite.texture.height;
            var delta = Input.mousePosition.y - clickPosition.y;

            if (Mathf.Abs(delta) > spriteHeight / 2)
            {
                SetIsAutoUse.Invoke(delta < 0 ? true : false);

                isClicked = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isClicked = false;
        }

        if (GetFocus.Invoke() == null)
        {
            UnbindButtonParent.SetActive(false);
        }

        if (GetIsAutoUse != null)
        {
            if (GetIsAutoUse.Invoke() && GetAmount.Invoke() > 0)
            {
                SlotParent.transform.localPosition = Vector3.down * 8.0f;
            }
            else
            {
                SetIsAutoUse.Invoke(false);

                SlotParent.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            SlotParent.transform.localPosition = Vector3.zero;
        }

        if (ItemName != "")
        {
            if (GetAmount?.Invoke() <= 0)
            {
                OutOfStockParent.SetActive(true);
            }
            else
            {
                OutOfStockParent.SetActive(false);
            }
        }
    }

    public void Onclick()
    {
        var isRegist = RegistUsableObject.Invoke(this);

        DisableUnbindButton();

        if (!isRegist)
        {
            if (IsInventoryActivated.Invoke())
            {
                if (GetFocus.Invoke() == this)
                {
                    UseSlotItem();
                    SetFocus.Invoke(null);
                }
                else if (GetFocus.Invoke() != null)
                {
                    SwapSlot.Invoke(this);
                }
                else
                {
                    if (ItemName != "")
                    {
                        UnbindButtonParent.SetActive(true);
                        SetFocus.Invoke(this);
                    }
                }
            }
            else
            {
                UseSlotItem();
            }
        }
    }

    public void DisableUnbindButton()
    {
        UnbindButtonParent.SetActive(false);
    }

    public override void UnBindItem()
    {
        base.UnBindItem();

        isClicked = false;
    }

    private void UseSlotItem()
    {
        if (GetAmount?.Invoke() > 0)
        {
            UseItem?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ItemName != "")
        {
            isClicked = true;
            clickPosition = Input.mousePosition;
        }
    }
}
