using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    public Player Player;

    public RectTransform FrameRectTransform;
    public RectTransform ContentRectTransform;

    public GameObject ItemSlotPrefab;
    public GameObject Container;

    public InventorySlotUI FocusingSlot;

    public PlayerHotkeyUI HotkeyUI;

    public ItemBase FocusingItem {
        get
        {
            if (FocusingSlot != null)
            {
                return data.GetUsableItem(FocusingSlot.ItemName);
            }
            return null;
        }
    }

    private CustomCircleLinkedList<InventorySlotUI> mySlots;
    private PlayerInventory data;
    private Rect frameRect;

    private float slotSize;
    private float slotGap = 4.0f;
    private int slotColAmount = 4;

    private int savedRowIndex = 0;
    private int containerRowSlotAmount;
    private int contentRowSlotAmount;

    private void Start()
    {
        mySlots = new();
        data = Player.Inventory;
        slotSize = ItemSlotPrefab.GetComponent<RectTransform>()
            .rect
            .height + slotGap;

        data.OnAddNewItem.AddListener(AddNewItemToUISlot);
        
        contentRowSlotAmount = Mathf.CeilToInt(data.CurrentCapacity / slotColAmount);
        ContentRectTransform.sizeDelta = new Vector2(0.0f, slotSize * (contentRowSlotAmount + 1.25f));

        frameRect = FrameRectTransform.rect;
        containerRowSlotAmount = (int)(frameRect.height / slotSize);

        var SlotFullAmount = (slotColAmount * 2) + (slotColAmount * containerRowSlotAmount);

        for (int i = 0; i < SlotFullAmount; i++)
        {
            var inst = Instantiate(ItemSlotPrefab, Container.transform);
            var nodeValue = inst.GetComponent<InventorySlotUI>();

            mySlots.AddNode(nodeValue);

            nodeValue.SetFocusItem = SetFocusingItem;
            nodeValue.IsOnFucusing = IsSlotFocusing;
            nodeValue.PopSlot = PopSlot;
        }

        ResetSlots();
    }

    private void OnEnable()
    {
        if (mySlots != null)
        {
            ResetSlots();
        }
    }

    private void Update()
    {
        UpdateScroll();
    }

    private void UpdateScroll()
    {
        var contentYPos = ContentRectTransform.localPosition.y;
        var contentYPosClamp = Mathf.Clamp(contentYPos, 0, ContentRectTransform.sizeDelta.y - frameRect.height);
        var currentRowIndex = (int)(contentYPosClamp / slotSize);

        // currentRowIndex is more than saved, it means scoll down.
        if (currentRowIndex != savedRowIndex)
        {
            if (currentRowIndex > savedRowIndex && contentRowSlotAmount > savedRowIndex + containerRowSlotAmount + 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    mySlots.HeadToTail()
                        .GetTailValue()
                        .transform
                        .SetAsLastSibling();

                    SetItemSlot(mySlots.GetTailValue(), i);
                }

                savedRowIndex++;
            }
            else if (currentRowIndex < savedRowIndex)
            {
                for (int i = 3; i >= 0; i--)
                {
                    mySlots.TailToHead()
                        .GetHeadValue()
                        .transform
                        .SetAsFirstSibling();

                    SetItemSlot(mySlots.GetHeadValue(), i);
                }

                savedRowIndex--;
            }

            Container.transform.localPosition = Vector3.down * savedRowIndex * slotSize;
        }

        void SetItemSlot(InventorySlotUI slot, int index)
        {
            var row = currentRowIndex > savedRowIndex ? containerRowSlotAmount + savedRowIndex + 2 : savedRowIndex - 1;
            var itemIndex = (row * 4) + index;

            if (data.Items.Count > itemIndex)
            {
                var name = data[itemIndex].Name;
                var item = data.Items[itemIndex];

                BindItemToSlot(item, slot);
            }
            else
            {
                slot.UnBindItem();
            }

        }
    }

    private void ResetSlots()
    {
        ContentRectTransform.localPosition = Vector3.right * 2.0f;
        savedRowIndex = 0;

        var node = mySlots.Head;
        var index = 0;

        while (node.value.ItemName != "" || index < data.Items.Count)
        {
            if (index < data.Items.Count)
            {
                BindItemToSlot(data.Items[index], node.value);
            }
            else
            {
                node.value.UnBindItem();
            }
            
            node = node.next;
            index++;
        }
    }

    private void BindItemToSlot(ItemBase item, InventorySlotUI slot)
    {
        slot.ItemName = item.Name;
        slot.Icon.sprite = item.IconSprite;

        slot.GetAmount = () => item?.Amount ?? 0;
        slot.UseItem = () => (item as IUsable)?.MyAction.Invoke();

        if (item is ICooldown cooldown)
        {
            slot.GetCooltime = () => cooldown?.CurrentCooltime ?? 0;
            slot.GetMaxCooltime = () => cooldown?.MaxCooltime ?? 1;
        }
    }

    private void SetFocusingItem(InventorySlotUI slot)
    {
        FocusingSlot?.OutFucus();
        FocusingSlot = slot;
    }

    private bool IsSlotFocusing(InventorySlotUI slot)
    {
        if (slot == FocusingSlot)
        {
            return true;
        }

        return false;
    }

    private void PopSlot(InventorySlotUI slot)
    {
        mySlots.GoToTail(slot);

        slot.transform.SetAsLastSibling();
    }

    public void AddNewItemToUISlot(ItemBase item)
    {
        var node = mySlots.Head;

        if (data.Items.Count / 4 >= savedRowIndex)
        {
            while (GetSlotComponent().ItemName != "")
            {
                if (node == mySlots.Tail)
                {
                    return;
                }

                node = node.next;
            }

            BindItemToSlot(item, GetSlotComponent());

            InventorySlotUI GetSlotComponent() => node.value.GetComponent<InventorySlotUI>();
        }
    }
}
