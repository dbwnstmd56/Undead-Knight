using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameSkillSlot : MonoBehaviour , IDropHandler
{
    static public InGameSkillSlot instance;

    public Skill skillObject;
    public Image skillImage;
    public int slotIndex;
    public bool isSkill;

    void Start()
    {
        instance = this;
    }
    public void OnDrop(PointerEventData eventData) // �ٸ� ���� ������ ��� ������ ȣ��
    {
        if (SkillDragSlot.instance.dragSlot != null)
        {
            skillObject = SkillDragSlot.instance.dragSlot;
            skillImage.sprite = SkillDragSlot.instance.dragSlot.SkillImage;
            SetColor(1);
        }
    }
    private void SetColor(float _alpha)
    {
        Color color = skillImage.color;
        color.a = _alpha;
        skillImage.color = color;
    }
}
