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

    public GameObject[] monsterPrefabs; // ���� ������ �迭

    public GameObject[] ActiveUI;
    public bool isActiveUI;
    /// <summary>
    /// 1.�� , 2.hp , 3.���ݷ�, 4.mp ,5.����
    /// </summary>
    public TMP_Text[] equipmentStatus;

    //���� ���� ����Ʈ
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
    //���� csv ���� �о ���� ����
    void ReadMonsterDataFromCSV()
    {
         string csvFilePath = "Assets/Resources/Monster.csv";

         // CSV ������ �� ���� �о�ɴϴ�.
         string[] lines = File.ReadAllLines(csvFilePath);

        // �� ���� �Ľ��Ͽ� ���� ������ �����ɴϴ�.
        for (int i = 1; i < lines.Length; i++) 
        {
            string[] fields = lines[i].Split(',');

            // CSV ���Ͽ��� ���� �����͸� MonsterInfo ��ü�� �����մϴ�.
            MonsterInfo monsterInfo = new MonsterInfo();
            monsterInfo.id = int.Parse(fields[0]);
            monsterInfo.name = fields[1];
            monsterInfo.attack = int.Parse(fields[2]);
            monsterInfo.hp = float.Parse(fields[3]);
            monsterInfo.EXP = int.Parse(fields[4]);

            // �о�� ���� ������ ����Ʈ�� �߰��մϴ�.
            monsterList.Add(monsterInfo);

            GetMonsterPrefabList();
        }
    }

    //���� id �� ����Ʈ�� �ִ� id �� ��ġ�ϴ� �ָ� ã�Ƽ� ��ȯ���ش�
    public MonsterInfo GetMonsterPrefabByID(int monsterID)
    {
        // ���� ID�� �ش��ϴ� ���� ������ ã���ϴ�.
        MonsterInfo foundMonster = monsterList.Find(monster => monster.id == monsterID);

        // ���� ������ ã�� ������ ��� null ��ȯ
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

    //������ csv �� monster������ �ִ� ���� id �� ��ġ�ϴ� �����鿡 ������ �־��ش�
    public void GetMonsterPrefabList()
    {
        string monsterPath = "monsters";

        monsterPrefabs = Resources.LoadAll<GameObject>(monsterPath);

        for(int i = 0; i < monsterPrefabs.Length; i++)
        {
            MonsterInfo getfoundMonster = GetMonsterPrefabByID(monsterPrefabs[i].GetComponent<Enemy>().monsterID);
            if (getfoundMonster == null)
            {
                Debug.Log("���Ͱ� �����ϴ�");
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
                    Vector3 centerPoint = spawnPoint.position; // �߽���

                    // �߽����� �������� �ݰ� ������ ������ ��ġ�� �����մϴ�.
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

        // ��Ȱ��ȭ�� ���Ͱ� �����ϴ��� Ȯ���մϴ�.
        foreach (GameObject monster in monsterArray)
        {
            if (!monster.activeSelf)
            {
                ReactivateMonster(monster, monster.GetComponent<Enemy>().spawnPos);
                return; // ��Ȱ��ȭ�� ���͸� �ϳ� Ȱ��ȭ�����Ƿ� �Լ��� �����մϴ�.
            }
        }
    }

    private void ReactivateMonster(GameObject monster, Transform spawnPoint)
    {
        // ���� ������ ��ġ�� �����ϰ� �����մϴ�.
        Vector3 centerPoint = spawnPoint.position; // �߽���
        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius * 3;
        Vector3 randomPosition = centerPoint + randomOffset;
        randomPosition.y = 4.5f;

        // ���͸� ��Ȱ��ȭ�� ��ġ�� �̵��ϰ� �ٽ� Ȱ��ȭ�մϴ�.
        monster.transform.position = randomPosition;
        monster.SetActive(true);
    }
    public void InsertQueue_Object(GameObject p_Object) //ť�� �ֱ�
    {
        if (p_Object.transform.CompareTag("Enemy"))
        {
            q_Monsters.Enqueue(p_Object);
            StartCoroutine(DisableObjectAfterDelay(p_Object, 3f));
        }
    }
    private IEnumerator DisableObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay); // ���� �ð� ���

        // ��ü�� ��Ȱ��ȭ
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
                break; // Ȱ��ȭ�� UI�� �ϳ��� ������ �ݺ��� ����
            }
        }
        isActiveUI = anyActive; // ��� UI�� Ȱ��ȭ ���θ� �ݿ�
    }
}
