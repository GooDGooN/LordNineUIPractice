using System;
using TMPro;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    public Player Player;

    public RectTransform FrameRectTransform;
    public RectTransform ContentRectTransform;

    public GameObject ItemSlotPrefab;
    public GameObject Container;

    public ItemAndSkillSlotUI FocusingSlot;

    private CustomCircleLinkedList<GameObject> mySlots;
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
            mySlots.AddNode(Instantiate(ItemSlotPrefab, Container.transform));

            var tail = mySlots.GetTailValue().GetComponent<ItemAndSkillSlotUI>();

            tail.SetFocusItem = SetFocusingItem;
            tail.IsOnFucusing = IsSlotFocusing;
            tail.PopSlot = PopSlot;
        }
    }

    private void OnEnable()
    {
        ContentRectTransform.localPosition = Vector3.right * 2.0f;
        savedRowIndex = 0;
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

        void SetItemSlot(GameObject slot, int index)
        {
            var row = currentRowIndex > savedRowIndex ? containerRowSlotAmount + savedRowIndex + 2 : savedRowIndex - 1;
            var itemIndex = (row * 4) + index;

            if (data.Items.Count > itemIndex)
            {
                GetSlotComponent().MyItem = data[itemIndex];
                GetSlotComponent().UseItem = () => data.UseItem(itemIndex);
            }
            else
            {
                GetSlotComponent().MyItem = null;
            }

            ItemAndSkillSlotUI GetSlotComponent() => slot.GetComponent<ItemAndSkillSlotUI>();
        }
    }

    private void SetFocusingItem(ItemAndSkillSlotUI slot)
    {
        FocusingSlot?.OutFucus();
        FocusingSlot = slot;
    }

    private bool IsSlotFocusing(ItemAndSkillSlotUI slot)
    {
        if (slot == FocusingSlot)
        {
            return true;
        }
        return false;
    }

    private void PopSlot(ItemAndSkillSlotUI slot)
    {
        mySlots.GoToTail(slot.gameObject);

        slot.transform.SetAsLastSibling();
    }

    public void AddNewItemToUISlot(ItemBase item)
    {
        var node = mySlots.Head;

        while (GetSlotComponent().MyItem != null)
        {
            if (node == mySlots.Tail)
            {
                return;
            }

            node = node.next;
        }

        var itemIndex = data.Items.Count - 1;
        GetSlotComponent().MyItem = item;
        GetSlotComponent().UseItem = () => data.UseItem(itemIndex);

        ItemAndSkillSlotUI GetSlotComponent() => node.value.GetComponent<ItemAndSkillSlotUI>();
    }
}
