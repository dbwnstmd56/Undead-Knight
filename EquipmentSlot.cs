using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour , IDropHandler
{
    public Item equipmentItem;
    public int slotNumber;
    public Image equipmentImage;
    public GameObject player;

    public void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (ItemDragSlot.instance.dragSlot != null && ItemDragSlot.instance.item.itemType == Item.ItemType.Equipment && ItemDragSlot.instance.item.EquipmentTypeNumber == slotNumber)
        {
            equipmentItem = ItemDragSlot.instance.item;
            equipmentImage.sprite = ItemDragSlot.instance.item.ItemImage;
            player.GetComponent<PlayerStatus>().attack += ItemDragSlot.instance.item.Attack;
            player.GetComponent<PlayerStatus>().defense += ItemDragSlot.instance.item.Defense;
            SetColor(1);
        }
    }
    private void SetColor(float _alpha)
    {
        Color color = equipmentImage.color;
        color.a = _alpha;
        equipmentImage.color = color;
    }
}
