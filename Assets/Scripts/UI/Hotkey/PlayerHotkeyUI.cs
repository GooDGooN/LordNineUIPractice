using UnityEngine;

public class PlayerHotkeyUI : MonoBehaviour
{
    public HotkeySlotUI[] Slots;

    public HotkeySlotUI FocusSlot { get; set; }   

    public PlayerInventoryUI InventoryUI;
    public Player MyPlayer;

    private PlayerInventory inventory;
    [SerializeField] private HotkeySlotUI dummySlot;

    private void Start()
    {
        foreach(var slot in Slots)
        {
            slot.RegistUsableObject = RegisterObjectToSlot;
            slot.IsInventoryActivated = IsInventoryActivated;
            slot.SwapSlot = SwapSlot;

            slot.GetFocus = () => FocusSlot;
            slot.SetFocus = (slot) => FocusSlot = slot;
        }

        inventory = MyPlayer.Inventory;

        FocusSlot = null;
    }

    private void Update()
    {
        if (FocusSlot != null && InventoryUI.FocusingSlot != null)
        {
            var index = FocusSlot.transform.GetSiblingIndex();

            Slots[index].RegistUsableObject(Slots[index]);

            FocusSlot = null;
        }
    }

    private bool RegisterObjectToSlot(HotkeySlotUI slot)
    {
        var focus = InventoryUI.FocusingItem ?? null;

        if (focus != null)
        {
            slot.ItemName = focus.Name;
        }

        var item = inventory.GetCooldownItem(slot.ItemName);

        if (item == null || focus == null)
        {
            return false;
        }

        slot.ItemName = item.Name;
        slot.Icon.sprite = item.IconSprite;
        slot.GetAmount = () => item?.Amount ?? 0;

        if (item is IUsable usable)
        {
            slot.UseItem = () => usable.MyAction.Invoke();
        }

        if (item is ICooldown cooldown)
        {
            slot.GetCooltime = () => cooldown?.CurrentCooltime ?? 0;
            slot.GetMaxCooltime = () => cooldown?.MaxCooltime ?? 0;
            slot.GetIsAutoUse = () => cooldown.IsAutoUse;
            slot.SetIsAutoUse = (value) => cooldown.IsAutoUse = value;
        }
        InventoryUI.FocusingSlot.OutFucus();
        InventoryUI.FocusingSlot = null;

        return true;
    }

    public bool IsInventoryActivated() => InventoryUI.gameObject.activeSelf;

    public void SwapSlot(HotkeySlotUI slot)
    {
        RebindSlot(slot, dummySlot);
        RebindSlot(FocusSlot, slot);
        RebindSlot(dummySlot, FocusSlot);

        FocusSlot = null;
        dummySlot.UnBindItem();

        void RebindSlot(HotkeySlotUI from, HotkeySlotUI to)
        {
            to.ItemName = from.ItemName;
            to.Icon.sprite = from.Icon.sprite;
            to.GetAmount = from.GetAmount;
            to.UseItem = from.UseItem;
            to.GetCooltime = from.GetCooltime;
            to.GetMaxCooltime = from.GetMaxCooltime;
            to.GetIsAutoUse = from.GetIsAutoUse;
            to.SetIsAutoUse = from.SetIsAutoUse;
        }
    }
    public void DisableFocus()
    {
        foreach (var slot in Slots)
        {
            slot.DisableUnbindButton();
        }

        FocusSlot = null;
    }
}

