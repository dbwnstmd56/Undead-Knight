using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;

    public GameObject player;

    //���� ���� ��ġ �� �̸�
    private string filePath;
    string fileName;

    public GameObject[] characterImage;

    //ĳ���� ���� �� ����
    public PlayerData[] playerDatas;
    public Button[] selectPlayers;
    public TMP_Text[] playerNames;
    public TMP_Text[] playerLevels;

    //���ӽ����� ������ ĳ���� �����͸� ������
    public PlayerData selectPlayer;

    public Button createCharacterBt;

    public TMP_InputField createCharacterName;

    public string[] characterJsonFiles;

    //���� ������ ĳ���ͼ�
    public int characterNumber;

    //�÷��� �̸� �� �������� ����
    public PlayerData playerData;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject("DataManager");
                    instance = singleton.AddComponent<DataManager>();
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateLoadData();
        playerDatas = new PlayerData[3];
        createCharacterName.characterLimit = 8;
    }
    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void SetFilePath(PlayerData playerData)
    {
        fileName = characterNumber + ".json";
        filePath = Path.Combine(Application.persistentDataPath, fileName);
    }
    public void SaveCharacterData(PlayerData playerData)
    {
        string jsonData = JsonUtility.ToJson(playerData);
        File.WriteAllText(filePath, jsonData);
    }
    public void LoadCharacterData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            //playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        }
        else
        {
            
        }
    }

    public void CreateCharacter(TextMeshProUGUI CharacterName)
    {

        PlayerData character = new PlayerData();
        character.playerName = CharacterName.text;
        character.playerLevel = 1;
        character.characterNumber = characterNumber;

        //���� ���
        SetFilePath(character);

        //��ο��ٰ� ������ ����
        SaveCharacterData(character);

        //ĳ��ī���� 1���
        characterNumber++;

        //������ ĳ���� characterJsonFiles���
        UpdateLoadData();

        if (characterNumber >= 3)
        {
            createCharacterBt.interactable = false;
        }
    }
    public void UpdateLoadData()
    {
        // JSON ������ ������ ���� ���
        filePath = Application.persistentDataPath;

        if (Directory.GetFiles(filePath, "*.json").Length == 0)
        {
            characterNumber = 0;
        }
        else
        {
            // ���� ���� ��� JSON ���� ����� �����ɴϴ�.
            characterJsonFiles = Directory.GetFiles(filePath, "*.json");

            characterNumber = Directory.GetFiles(filePath, "*.json").Length;

            for(int i = 0; i < characterNumber; i++)
            {
                selectPlayers[i].interactable = true;
            }

            if (characterNumber >= 3)
            {
                createCharacterBt.interactable = false;
            }
        }
    }
    //ĳ���� ����â�� �̸� �־��ִ°� 
    public void SetLoadData()
    {
        if (characterJsonFiles.Length != 0)
        {
            for (int i = 0; i < characterJsonFiles.Length; i++)
            {
                string character = characterJsonFiles[i];
                string jsonText = File.ReadAllText(character);
                PlayerData characterData = JsonUtility.FromJson<PlayerData>(jsonText);
                playerDatas[i] = characterData;
                playerNames[characterData.characterNumber].text = "Name: " + " " + characterData.playerName;
                playerLevels[characterData.characterNumber].text = "Level: " + " " + characterData.playerLevel;
                characterImage[i].SetActive(true);
            }
        }
    }
    //ĳ���� ����â�� �ִ� ���� selectNumber������ �����͸� �Ѱ���
    public void SetSelectPlayer(int selectNumber)
    {
        if(playerDatas.Length != 0)
        {
            selectPlayer = playerDatas[selectNumber];
        } 
    }
    public void GetPlayerStatus()
    {
        // CSV ���� �б�
        string[] lines = File.ReadAllLines("Assets/Resources/status.csv");

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            // �� ������ ��ǥ(,)�� �����Ͽ� �ʵ带 ����
            string[] fields = line.Split(',');

            if (fields.Length > 0 && DataManager.Instance.selectPlayer.playerLevel == int.Parse(fields[0]))
            {
                GameObject newPlayer = Instantiate(player);
                newPlayer.GetComponent<PlayerStatus>().Level = int.Parse(fields[0]);
                newPlayer.GetComponent<PlayerStatus>().maxHP = float.Parse(fields[1]);
                newPlayer.GetComponent<PlayerStatus>().maxMP = float.Parse(fields[2]);
                newPlayer.GetComponent<PlayerStatus>().attack = int.Parse(fields[3]);
                newPlayer.GetComponent<PlayerStatus>().maxEXP = float.Parse(fields[4]);

                DontDestroyOnLoad(newPlayer);
            }
        }
    }
    private void OnApplicationQuit()
    {
        // ���� ���� �� �÷��̾� ����
        //SaveCharacterData(playerData);
    }
}
