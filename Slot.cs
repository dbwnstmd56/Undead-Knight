using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    static public Slot instance;
    public Item item;
    public int itemCount;
    public Image itemImage;

    [SerializeField]
    private TMP_Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    public GameObject itemInfo;
    public Image itemInfoImage;
    public Text itemNameText;
    public Text itemInfoText;
    public Text itemStatusText;

    //이미지의 투명도 조절
    void Start()
    {
        instance = this;
    }
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
    //아이템 추가
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = _item.ItemImage;
        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            SetColor(1);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            SetColor(1);
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
    }
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }
    //슬롯 초기화
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "";
        go_CountImage.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            ItemDragSlot.instance.item = item;
            ItemDragSlot.instance.dragSlot = this;
            ItemDragSlot.instance.DragSetImage();
            ItemDragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            ItemDragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) // 그냥 드레그가 끝나면 호출
    {
        ItemDragSlot.instance.SetColor(0);
        ItemDragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData) // 다른 슬롯 위에서 드롭 했을때 호출
    {
        if (ItemDragSlot.instance.dragSlot != null)
        {
            ChangeItemSlot();
        }

    }
    public void ChangeItemSlot()
    {
        Item _tempItem = item; // 임시 아이템에 b 를 넣어준다 복사본
        int _tempItemCount = itemCount;

        AddItem(ItemDragSlot.instance.item, ItemDragSlot.instance.dragSlot.itemCount); // b 에다 a 를 넣는과정


        if (_tempItem != null)
        {
            ItemDragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount); //복사해둔 b를 a자리에 additem 을 해준다
        }
        else
        {
            ItemDragSlot.instance.dragSlot.ClearSlot();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // itemInfo.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       
    }
}
