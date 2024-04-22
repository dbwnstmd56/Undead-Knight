using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    static public PlayerStatus instance;

    public int Level;
    public int attack;
    public int defense;

    public float maxHP;
    public float currentHP;

    public float maxMP;
    public float curretnMP;

    public float maxEXP;
    public float currentEXP;

    void Update()
    {
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        GameManager.Instance.equipmentStatus[0].text = "Level : " + Level.ToString();
        GameManager.Instance.equipmentStatus[1].text = "HP : " + maxHP.ToString();
        GameManager.Instance.equipmentStatus[2].text = "Attack : " + attack.ToString();
        GameManager.Instance.equipmentStatus[3].text = "MP : " + maxMP.ToString();
        GameManager.Instance.equipmentStatus[4].text = "Defense : " + defense.ToString();
    }
}
