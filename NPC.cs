using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public int npcId;
    public int questId;
    public Quest quest;
    public bool conversation;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            QuestManager.Instance.NPC = transform.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            QuestManager.Instance.NPC = null;
        }
    }
}
