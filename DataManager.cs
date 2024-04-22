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

    //파일 저장 위치 및 이름
    private string filePath;
    string fileName;

    public GameObject[] characterImage;

    //캐릭터 네임 및 레벨
    public PlayerData[] playerDatas;
    public Button[] selectPlayers;
    public TMP_Text[] playerNames;
    public TMP_Text[] playerLevels;

    //게임시작전 선택한 캐릭터 데이터를 저장함
    public PlayerData selectPlayer;

    public Button createCharacterBt;

    public TMP_InputField createCharacterName;

    public string[] characterJsonFiles;

    //현재 생성된 캐릭터수
    public int characterNumber;

    //플레이 이름 및 스테이지 정보
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

        //파일 경로
        SetFilePath(character);

        //경로에다가 데이터 저장
        SaveCharacterData(character);

        //캐릭카운터 1상승
        characterNumber++;

        //생성한 캐릭터 characterJsonFiles등록
        UpdateLoadData();

        if (characterNumber >= 3)
        {
            createCharacterBt.interactable = false;
        }
    }
    public void UpdateLoadData()
    {
        // JSON 파일을 저장한 폴더 경로
        filePath = Application.persistentDataPath;

        if (Directory.GetFiles(filePath, "*.json").Length == 0)
        {
            characterNumber = 0;
        }
        else
        {
            // 폴더 내의 모든 JSON 파일 목록을 가져옵니다.
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
    //캐릭터 선택창에 이름 넣어주는거 
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
    //캐릭터 선택창에 있는 것중 selectNumber에따라 데이터를 넘겨줌
    public void SetSelectPlayer(int selectNumber)
    {
        if(playerDatas.Length != 0)
        {
            selectPlayer = playerDatas[selectNumber];
        } 
    }
    public void GetPlayerStatus()
    {
        // CSV 파일 읽기
        string[] lines = File.ReadAllLines("Assets/Resources/status.csv");

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            // 각 라인을 쉼표(,)로 구분하여 필드를 추출
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
        // 게임 종료 시 플레이어 저장
        //SaveCharacterData(playerData);
    }
}
