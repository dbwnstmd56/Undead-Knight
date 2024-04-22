using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class UIActionController : MonoBehaviour
{
    public static bool ActionActivated;

    bool isInventory = false;
    bool isSkill = false;
    bool isEquipment = false;


    [SerializeField]
    private GameObject UI_inventory;
    [SerializeField]
    private GameObject UI_equipment;
    [SerializeField]
    private GameObject UI_skill;

    void Update()
    {
        UI_Equipment();
        UI_Skill();
        UI_Inventory();
    }

    private void UI_Equipment()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isEquipment = !isEquipment;

            if (isEquipment)
            {
                OpenEquipment();
            }
            else
            {
                CloseEquipment();
                //itemInfo.SetActive(false);
            }
        }

    }
    private void UI_Skill()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isSkill = !isSkill;

            if (isSkill)
                OpenSkill();
            else
                CloseSkill();
        }

    }
    private void UI_Inventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventory = !isInventory;

            if (isInventory)
                OpenInventory();
            else
            {
                CloseInventory();
                //itemInfo.SetActive(false);
            }
        }

    }
    private void OpenEquipment()
    {
        UI_equipment.SetActive(true);
    }
    private void CloseEquipment()
    {
        UI_equipment.SetActive(false);
    }
    private void OpenSkill()
    {
        UI_skill.SetActive(true);
    }
    private void CloseSkill()
    {
        UI_skill.SetActive(false);
    }
    private void OpenInventory()
    {
        UI_inventory.SetActive(true);
    }
    private void CloseInventory()
    {
        UI_inventory.SetActive(false);
    }
}
