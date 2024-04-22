using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDragSlot : MonoBehaviour
{
    static public SkillDragSlot instance;

    public Skill dragSlot;

    [SerializeField]
    private Image slotImage;

    void Start()
    {
        instance = this;
        slotImage = GetComponent<Image>();
    }
    public void DragSetImage()
    {
        slotImage.sprite = dragSlot.SkillImage;
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
    }
}
