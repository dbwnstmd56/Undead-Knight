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

    // CSV 파일의 경로
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

    // CSV 파일을 읽어서 대화 정보를 가져오는 함수
    private void ReadDialogueCSV()
    {
        if (File.Exists(dialogueFilePath))
        {
            // CSV 파일에서 대화 라인을 읽어와 리스트에 저장합니다.
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
    //npc 및 quest 와 일치하는 대화 가져오기
    public string[] GetDialogueList(int npcId, int questId)
    {
        foreach (DialogueData data in dialogueDataList)
        {
            if (data.questId == questId && data.npcId == npcId)
            {
                return data.dialogue;
            }
        }
        return null; // 해당하는 대화 데이터가 없을 경우 null 반환
    }
    public string GetNextDialogue(string[] dialogueList)
    {
        // 현재 대화 인덱스가 대화 데이터의 범위를 넘어갈 경우 null 반환
        if (currentDialogueIndex >= dialogueList.Length -1)
        {
            quest_Btn.SetActive(true);
            return null;
        }

        // 다음 대화 반환 후 인덱스 업데이트
        string nextDialogue = dialogueList[currentDialogueIndex];
        currentDialogueIndex++;

        return nextDialogue;
    }
    
    public void ShowDialogue()
    {
        string[] dialogueList = GetDialogueList(QuestManager.Instance.NPC.GetComponent<NPC>().npcId, QuestManager.Instance.NPC.GetComponent<NPC>().questId);
        if (dialogueList != null)
        {
            // 다음 대화 가져오기
            string nextDialogue = GetNextDialogue(dialogueList);
            if (nextDialogue != null)
            {
                // 가져온 대화 사용하기
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