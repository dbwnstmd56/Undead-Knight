using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static SoundManager;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public GameObject[] monsterPrefabs; // 몬스터 프리팹 배열

    public GameObject[] ActiveUI;
    public bool isActiveUI;
    /// <summary>
    /// 1.렙 , 2.hp , 3.공격력, 4.mp ,5.방어력
    /// </summary>
    public TMP_Text[] equipmentStatus;

    //몬스터 정보 리스트
    List<MonsterInfo> monsterList = new List<MonsterInfo>();

    public Queue<GameObject> q_Monsters = new Queue<GameObject>();

    public int spawnCount = 10;

    public float spawnRadius = 30f;
    public class MonsterInfo
    {
        public int id;
        public string name;
        public int attack;
        public float hp;
        public int EXP;
    }
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject("GameManager");
                    instance = singleton.AddComponent<GameManager>();
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
        ReadMonsterDataFromCSV();
        SoundManager.Instance.PlayBgm(true, 0, Bgm.TownBgm);
    }
    private void Update()
    {
        IsActiveUI();
    }
    //몬스터 csv 파일 읽어서 정보 저장
    void ReadMonsterDataFromCSV()
    {
         string csvFilePath = "Assets/Resources/Monster.csv";

         // CSV 파일의 각 줄을 읽어옵니다.
         string[] lines = File.ReadAllLines(csvFilePath);

        // 각 줄을 파싱하여 몬스터 정보를 가져옵니다.
        for (int i = 1; i < lines.Length; i++) 
        {
            string[] fields = lines[i].Split(',');

            // CSV 파일에서 읽은 데이터를 MonsterInfo 객체에 저장합니다.
            MonsterInfo monsterInfo = new MonsterInfo();
            monsterInfo.id = int.Parse(fields[0]);
            monsterInfo.name = fields[1];
            monsterInfo.attack = int.Parse(fields[2]);
            monsterInfo.hp = float.Parse(fields[3]);
            monsterInfo.EXP = int.Parse(fields[4]);

            // 읽어온 몬스터 정보를 리스트에 추가합니다.
            monsterList.Add(monsterInfo);

            GetMonsterPrefabList();
        }
    }

    //몬스터 id 와 리스트에 있는 id 중 일치하는 애를 찾아서 반환해준다
    public MonsterInfo GetMonsterPrefabByID(int monsterID)
    {
        // 몬스터 ID에 해당하는 몬스터 정보를 찾습니다.
        MonsterInfo foundMonster = monsterList.Find(monster => monster.id == monsterID);

        // 몬스터 정보를 찾지 못했을 경우 null 반환
        if (foundMonster == null)
        {
            Debug.LogWarning("Monster with ID " + monsterID + " not found.");
            return null;
        }
        else
        {
            return foundMonster;
        }
    }

    //가져온 csv 를 monster폴더에 있는 몬스터 id 와 일치하는 프리펩에 정보를 넣어준다
    public void GetMonsterPrefabList()
    {
        string monsterPath = "monsters";

        monsterPrefabs = Resources.LoadAll<GameObject>(monsterPath);

        for(int i = 0; i < monsterPrefabs.Length; i++)
        {
            MonsterInfo getfoundMonster = GetMonsterPrefabByID(monsterPrefabs[i].GetComponent<Enemy>().monsterID);
            if (getfoundMonster == null)
            {
                Debug.Log("몬스터가 없습니다");
            }
            else
            {
                monsterPrefabs[i].GetComponent<Enemy>().name = getfoundMonster.name;
                monsterPrefabs[i].GetComponent<Enemy>().attack = getfoundMonster.attack;
                monsterPrefabs[i].GetComponent<Enemy>().maxHp = getfoundMonster.hp;
                monsterPrefabs[i].GetComponent<Enemy>().currentHp = getfoundMonster.hp;
                monsterPrefabs[i].GetComponent<Enemy>().EXP = getfoundMonster.EXP;
            }
        }
    }

    public void MonsterObjectPooling(int monsterid1, Transform spawnPoint)
    {
        GameObject monstersPool = GameObject.Find("MonstersPool");

        for (int i= 0; i < monsterPrefabs.Length; i++)
        {
            if(monsterPrefabs[i].GetComponent<Enemy>().monsterID == monsterid1)
            {
                for(int j = 0; j < spawnCount; j++)
                {
                    Vector3 centerPoint = spawnPoint.position; // 중심점

                    // 중심점을 기준으로 반경 내에서 랜덤한 위치를 생성합니다.
                    Vector3 randomOffset = Random.insideUnitSphere * spawnRadius * 3;
                    Vector3 randomPosition = centerPoint + randomOffset;

                    randomPosition.y = 4.5f;

                    GameObject pool_monster1 = Instantiate(monsterPrefabs[i], Vector3.zero, Quaternion.identity);
                    pool_monster1.transform.parent = monstersPool.transform;
                    pool_monster1.GetComponent<NavMeshAgent>().enabled = false;
                    pool_monster1.GetComponent<Enemy>().spawnPos = spawnPoint;
                    q_Monsters.Enqueue(pool_monster1);
                    pool_monster1.transform.position = randomPosition;
                    pool_monster1.SetActive(true);
                }
            }
        }
    }
    public void FillMonsterPool()
    {
        GameObject monstersPool = GameObject.Find("MonstersPool");

        GameObject[] monsterArray = new GameObject[monstersPool.transform.childCount];
        for (int i = 0; i < monstersPool.transform.childCount; i++)
        {
            monsterArray[i] = monstersPool.transform.GetChild(i).gameObject;
        }

        // 비활성화된 몬스터가 존재하는지 확인합니다.
        foreach (GameObject monster in monsterArray)
        {
            if (!monster.activeSelf)
            {
                ReactivateMonster(monster, monster.GetComponent<Enemy>().spawnPos);
                return; // 비활성화된 몬스터를 하나 활성화했으므로 함수를 종료합니다.
            }
        }
    }

    private void ReactivateMonster(GameObject monster, Transform spawnPoint)
    {
        // 죽은 몬스터의 위치를 랜덤하게 변경합니다.
        Vector3 centerPoint = spawnPoint.position; // 중심점
        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius * 3;
        Vector3 randomPosition = centerPoint + randomOffset;
        randomPosition.y = 4.5f;

        // 몬스터를 비활성화된 위치로 이동하고 다시 활성화합니다.
        monster.transform.position = randomPosition;
        monster.SetActive(true);
    }
    public void InsertQueue_Object(GameObject p_Object) //큐에 넣기
    {
        if (p_Object.transform.CompareTag("Enemy"))
        {
            q_Monsters.Enqueue(p_Object);
            StartCoroutine(DisableObjectAfterDelay(p_Object, 3f));
        }
    }
    private IEnumerator DisableObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay); // 일정 시간 대기

        // 객체를 비활성화
        obj.SetActive(false);
    }
    public void IsActiveUI()
    {
        bool anyActive = false;
        foreach (GameObject UI in ActiveUI)
        {
            if (UI.activeSelf)
            {
                anyActive = true;
                break; // 활성화된 UI가 하나라도 있으면 반복문 종료
            }
        }
        isActiveUI = anyActive; // 모든 UI의 활성화 여부를 반영
    }
}
