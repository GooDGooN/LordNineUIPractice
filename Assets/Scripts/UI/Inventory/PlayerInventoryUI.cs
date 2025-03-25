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

    private CustomCircleLinkedList<GameObject> mySlots;
    private PlayerInventory data;
    private Rect frameRect;
    private float slotSize;

    private int savedRowIndex;
    private int containerRowSlotAmount;
    private int contentRowSlotAmount;

    private void Start()
    {
        mySlots = new();
        data = Player.Inventory;
        savedRowIndex = 0;

        frameRect = FrameRectTransform.rect;
        slotSize = ItemSlotPrefab.GetComponent<RectTransform>().rect.height + 4.0f;

        containerRowSlotAmount = (int)(frameRect.height / slotSize);
        contentRowSlotAmount = Mathf.CeilToInt(data.CurrentCapacity / 4);
        var SlotFullAmount = 8 + (containerRowSlotAmount * 4);

        ContentRectTransform.sizeDelta = new Vector2(0.0f, slotSize * (contentRowSlotAmount + 1.25f));

        for (int i = 0; i < SlotFullAmount; i++)
        {
            mySlots.AddNode(Instantiate(ItemSlotPrefab, Container.transform))
                .GetTailValue()
                .transform
                .GetChild(3)
                .GetComponent<TMP_Text>().text = $"{i}"; ;
        }
    }

    private void OnEnable()
    {
        ContentRectTransform.localPosition = Vector3.zero;
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

        // currentYIndex is more than saved, it means scolldown.
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

            slot.transform
                .GetChild(3)
                .GetComponent<TMP_Text>().text = $"{(row * 4) + index}";
        }
    }

    public void UpdateUI(int index = -1)
    {
        if (index > -1)
        {
            for (int i = 0; i < data.Items.Count; i++)
            {
                // Update UI
            }
        }
    }

}
