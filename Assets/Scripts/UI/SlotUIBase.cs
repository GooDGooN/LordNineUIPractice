using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUIBase<T> : MonoBehaviour
{
    public Image ChargeLine;
    public Image Icon;

    public TMP_Text AmountText;
    public TMP_Text CooltimeText;

    public GameObject ItemInfoParent;
    public GameObject FocusButtonParent;
    public GameObject CooldownParent;

    public Action UseItem;
    public Action<bool> SetIsAutoUse;

    public Func<int> GetAmount;
    public Func<float> GetCooltime;
    public Func<float> GetMaxCooltime;
    public Func<bool> GetIsAutoUse;
   

    public string ItemName = "";


    protected virtual void Update()
    {
        if (ItemName != "")
        {
            ItemInfoParent.SetActive(true);

            if (GetAmount.Invoke() > 0)
            {
                AmountText.text = $"{GetAmount.Invoke()}";
            }
            else
            {
                AmountText.text = "";
            }

            var cooltime = GetCooltime.Invoke();
            ChargeLine.fillAmount = cooltime / GetMaxCooltime.Invoke();

            if (cooltime > 0.0f)
            {
                CooldownParent.SetActive(true);
                FocusButtonParent.SetActive(false);

                var text = (cooltime) switch
                {
                    (> 60.0f) => $"{(int)cooltime / 60}min",
                    (> 1.0f) => $"{(int)cooltime}s",
                    _ => $"{cooltime:F1}s"
                };

                CooltimeText.text = text;
            }
            else
            {
                CooldownParent.SetActive(false);
            }
        }
        else
        {
            ChargeLine.fillAmount = 0.0f;
            ItemInfoParent.SetActive(false);
            FocusButtonParent.SetActive(false);
            UseItem = null;
        }
    }


    public virtual void UnBindItem()
    {
        UseItem = null;
        GetAmount = null;
        GetMaxCooltime = null;
        Icon.sprite = null;
        ItemName = "";
    }
}
