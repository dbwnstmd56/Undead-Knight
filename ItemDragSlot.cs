using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDragSlot : MonoBehaviour
{
    static public ItemDragSlot instance;

    public Slot dragSlot;
    public Item item;
    public int itemCount;
    public GameObject dropItemSlot;

    [SerializeField]
    private Image slotImage;

    void Start()
    {
        instance = this;
        slotImage = GetComponent<Image>();
    }
    public void DragSetImage()
    {
        slotImage.sprite = item.ItemImage;
        SetColor(1);
    }
    public void SetColor(float _alpha)
    {
        Color color = slotImage.color;
        color.a = _alpha;
        slotImage.color = color;
    }
    public void ClearSlot()
    {
        dragSlot = null;
        item = null;
        itemCount = 0;
    }
}