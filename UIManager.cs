using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Slider HPslider;
    public Slider MPslider;
    public Slider EXPslider;

    public TMP_Text PlayerName;
    public TMP_Text GoldText;

    public GameObject DialogueUI;

    public GameObject player;
    void Start()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        PlayerName.text = DataManager.Instance.selectPlayer.playerName;
        initStatus();
    }
    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            UpdateStatus();
        }
    }
    public void initStatus()
    {
        HPslider.maxValue = player.GetComponent<PlayerStatus>().maxHP;
        MPslider.maxValue = player.GetComponent<PlayerStatus>().maxMP;
        EXPslider.maxValue = player.GetComponent<PlayerStatus>().maxEXP;
        player.GetComponent<PlayerStatus>().currentHP = player.GetComponent<PlayerStatus>().maxHP;
        player.GetComponent<PlayerStatus>().curretnMP = player.GetComponent<PlayerStatus>().maxMP;
    }
    void UpdateStatus()
    {
        HPslider.value = player.GetComponent<PlayerStatus>().currentHP;
        MPslider.value = player.GetComponent<PlayerStatus>().curretnMP;
        EXPslider.value = player.GetComponent<PlayerStatus>().currentEXP;
        GoldText.text = player.GetComponent<PlayerController>().Gold.ToString() + " G";

        if (player.GetComponent<PlayerStatus>().currentHP < 0)
        {
            player.GetComponent<PlayerStatus>().currentHP = 0;
        }
        if (player.GetComponent<PlayerStatus>().curretnMP < 0)
        {
            player.GetComponent<PlayerStatus>().curretnMP = 0;
        }
    }
}
