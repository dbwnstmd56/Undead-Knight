using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class SkillSlot : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    public Skill skillObject;
    public int slotIndex;
    public bool isSkill;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (skillObject != null)
        {
            SkillDragSlot.instance.dragSlot = skillObject;
            SkillDragSlot.instance.DragSetImage();
            SkillDragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skillObject != null)
        {
            SkillDragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) // �׳� �巹�װ� ������ ȣ��
    {
        SkillDragSlot.instance.SetColor(0);
        SkillDragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData) // �ٸ� ���� ������ ��� ������ ȣ��
    {
        if (SkillDragSlot.instance.dragSlot != null)
        {
            Debug.Log("aa");
            InGameSkillSlot.instance.skillObject = skillObject;
            InGameSkillSlot.instance.skillImage.sprite = skillObject.SkillImage;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }
}
