using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using TMPro;
public class DialogueData
{
    public int questId;
    public int npcId;
    public string[] dialogue;

    public DialogueData(int _npcId, int _questId, string[] _dialogue)
    {
        npcId = _npcId;
        questId = _questId;
        dialogue = _dialogue;
    }
}
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    // CSV ������ ���
    string dialogueFilePath = "Assets/Resources/Dialogue.csv";

    private List<DialogueData> dialogueDataList = new List<DialogueData>();

    public TMP_Text dialogueText;

    public GameObject quest_Btn;

    private int currentDialogueIndex = 0;
    void Start()
    {
        instance = this;
        ReadDialogueCSV();
    }

    // CSV ������ �о ��ȭ ������ �������� �Լ�
    private void ReadDialogueCSV()
    {
        if (File.Exists(dialogueFilePath))
        {
            // CSV ���Ͽ��� ��ȭ ������ �о�� ����Ʈ�� �����մϴ�.
            string[] lines = File.ReadAllLines(dialogueFilePath);
            foreach (string line in lines.Skip(1))
            {
                string[] tokens = line.Split(',');
                int npcId = int.Parse(tokens[0]);
                int questId = int.Parse(tokens[1]);
                string[] dialogue = new string[tokens.Length - 2];
                Array.Copy(tokens, 2, dialogue, 0, tokens.Length - 2);
                DialogueData data = new DialogueData(npcId, questId, dialogue);
                dialogueDataList.Add(data);
            }
        }
        else
        {
            Debug.LogError("Dialogue CSV file not found!");
        }
    }
    //npc �� quest �� ��ġ�ϴ� ��ȭ ��������
    public string[] GetDialogueList(int npcId, int questId)
    {
        foreach (DialogueData data in dialogueDataList)
        {
            if (data.questId == questId && data.npcId == npcId)
            {
                return data.dialogue;
            }
        }
        return null; // �ش��ϴ� ��ȭ �����Ͱ� ���� ��� null ��ȯ
    }
    public string GetNextDialogue(string[] dialogueList)
    {
        // ���� ��ȭ �ε����� ��ȭ �������� ������ �Ѿ ��� null ��ȯ
        if (currentDialogueIndex >= dialogueList.Length -1)
        {
            quest_Btn.SetActive(true);
            return null;
        }

        // ���� ��ȭ ��ȯ �� �ε��� ������Ʈ
        string nextDialogue = dialogueList[currentDialogueIndex];
        currentDialogueIndex++;

        return nextDialogue;
    }
    
    public void ShowDialogue()
    {
        string[] dialogueList = GetDialogueList(QuestManager.Instance.NPC.GetComponent<NPC>().npcId, QuestManager.Instance.NPC.GetComponent<NPC>().questId);
        if (dialogueList != null)
        {
            // ���� ��ȭ ��������
            string nextDialogue = GetNextDialogue(dialogueList);
            if (nextDialogue != null)
            {
                // ������ ��ȭ ����ϱ�
                dialogueText.text = nextDialogue;
            }
            else
            {
                dialogueText.text = dialogueList[dialogueList.Length -1];
            }
        }
    }
    public void initDialogue()
    {
        currentDialogueIndex = 0;
        UIManager.instance.DialogueUI.SetActive(false);
        quest_Btn.SetActive(false);
        QuestManager.Instance.quests.Add(QuestManager.Instance.NPC.GetComponent<NPC>().quest);
    }
}