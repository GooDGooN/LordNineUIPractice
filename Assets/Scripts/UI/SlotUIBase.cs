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

    public T MyItem;
}
