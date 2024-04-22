using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;

    //public Quest[] quests;
    public List<Quest> quests;

    public GameObject questPanel;
    public TMP_Text questName;
    public TMP_Text[] questGoal;

    public GameObject player;

    public GameObject NPC;

    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject("QuestManager");
                    instance = singleton.AddComponent<QuestManager>();
                }
            }
            return instance;
        }
    }
    private void Start()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
    }
    public void Update()
    {
        QuestUpdate();
    }
    public void UpdateQuest(int id)
    {
        if (quests != null)
        {
            foreach (var quest in quests)
            {
                if (quest.questType == Quest.QuestType.Hunt)
                {
                    var foundEnemy = quest.monstersId.FirstOrDefault(enemy => enemy.monsterID == id);
                    if (foundEnemy != null)
                    {
                        var index = Array.IndexOf(quest.monstersId, foundEnemy);

                        quest.currentMonsterKill[index]++;

                       for(int i = 0; i < quest.monsterKill.Length; i++)
                        {
                            if(quest.monsterKill[i] == quest.currentMonsterKill[i])
                            {
                                quest.succeedKillCount++;
                                if(quest.succeedKillCount == quest.monsterKill.Length)
                                {
                                    quest.succeedQuest = true;
                                    ClearQuest(quest);

                                    if (quest.questReward != null)
                                    {
                                        foreach(var item in quest.questReward)
                                        {
                                            InventoryController.instance.AcquireItem(item);
                                            player.GetComponent<PlayerController>().Gold += quest.rewardGold;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if(quest.questType == Quest.QuestType.Growth)
                {
                    if(player.GetComponent<PlayerStatus>().Level > quest.growthLevel)
                    {
                        quest.succeedQuest = true;
                        if (quest.questReward != null)
                        {
                            foreach (var item in quest.questReward)
                            {
                                InventoryController.instance.AcquireItem(item);
                                player.GetComponent<PlayerController>().Gold += quest.rewardGold;
                            }
                        }
                    }
                }
                else
                {
                    if(quest.questId == NPC.GetComponent<NPC>().npcId && NPC.GetComponent<NPC>().conversation)
                    {
                        quest.succeedQuest = true;
                        if (quest.questReward != null)
                        {
                            foreach (var item in quest.questReward)
                            {
                                InventoryController.instance.AcquireItem(item);
                                player.GetComponent<PlayerController>().Gold += quest.rewardGold;
                            }
                        }
                    }
                }
            }
        }
    }
    public void QuestUpdate()
    {
        if (quests != null && quests.Count > 0 && quests[0].questType == Quest.QuestType.Hunt)
        {
            questPanel.SetActive(true);
            questName.text = quests[0].questName;
            for(int i = 0; i < quests[0].monstersId.Length; i++)
            {
                questGoal[i].text = quests[0].monstersId[i].monsterName + "  " + quests[0].currentMonsterKill[i].ToString() + " / " + quests[0].monsterKill[i].ToString();
                questGoal[i].gameObject.SetActive(true);
            }
        }
        else
        {
            questGoal[0].gameObject.SetActive(false);
            questGoal[1].gameObject.SetActive(false);
            questPanel.SetActive(false);
        }
    }
    public void ClearQuest(Quest quest)
    {
        quests.Remove(quest);
    }
}
