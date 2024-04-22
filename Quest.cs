using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "New Quest/Quest")]
public class Quest : ScriptableObject
{
    public int questId;
    public string questName;
    public bool succeedQuest;
    public Item[] questReward;
    public float rewardGold;

    //Hunt
    public Enemy[] monstersId;
    public int[] monsterKill;
    public int[] currentMonsterKill;
    public int succeedKillCount;

    //Growth
    public int growthLevel;
    public int currentGrowthLevel;

    //Conversation
    public int npcId;

    public QuestType questType;
    public enum QuestType
    {
        Hunt,
        Growth,
        Conversation
    }
}
