using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class BossNecromanser : MonoBehaviour
{
    public static BossNecromanser instance;

    public Image hpBar;
    public float maxHp;
    public float currentHp;

    public GameObject attackPP1;
    public GameObject attackPP2;
    public GameObject[] attackParticle;
    public GameObject basicAttackHitbox;

    public Queue<GameObject>[] particleQueues;
    public GameObject dieEffectParticle;

    public GameObject player;
    public NavMeshAgent nav;
    public GameObject clearUI;
    public GameObject bossHpUI;

    public Animator anim;

    private Queue<GameObject> activeParticles = new Queue<GameObject>();

    public float spawnRadius = 5f;
    public enum BossState
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

    // ������ ���� ����
    public BossState currentState;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        CreateParticle();
        currentState = BossState.Idle;
    }

    void Update()
    {
        //hpBar.fillAmount = currentHp / maxHp;
        attackPP2.transform.position = transform.position;

        // ���� ���¿� ���� �ٸ� ���� ����
        switch (currentState)
        {
            case BossState.Idle:
                break;
            case BossState.Chase:
                UpdateChaseState();
                break;
            case BossState.Attack:
                UpdateAttackState();
                break;
            case BossState.Dead:
                UpdateDeadState();
                break;
        }
    }

    // Chase ���� ������Ʈ
    void UpdateChaseState()
    {
        if (PlayerIsWithinAttackRange())
        {
            anim.SetBool("Walk", false);
            HandleAttack();
        }
        else
        {
            // ���� ���� ���� �÷��̾ ������ �÷��̾ ���� �̵�
            anim.SetBool("Walk", true);
            nav.SetDestination(player.transform.position);
        }
    }

    // Attack ���� ������Ʈ
    void UpdateAttackState()
    {
        // ���� ������ ����� Chase ���·� ����
        if (!PlayerIsWithinAttackRange())
        {
            StopAllCoroutines();
            basicAttackHitbox.SetActive(false);
            StartCoroutine(SkillActive2());
            currentState = BossState.Chase;
        }
    }
    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        hpBar.fillAmount = currentHp / maxHp;

        if (currentHp <= 0)
        {
            currentHp = 0;
            anim.SetTrigger("Die");
            dieEffectParticle.SetActive(true);
            bossHpUI.SetActive(false);
            clearUI.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(DieDelay(3.0f));
            currentState = BossState.Dead;
        }
    }

    // Dead ���� ������Ʈ
    void UpdateDeadState()
    {
        GameObject particlePool = GameObject.Find("ParticlePool");
        foreach (Transform child in particlePool.transform.GetChild(0))
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // �÷��̾ ���� ���� ���� �ִ��� Ȯ���ϴ� �޼���
    bool PlayerIsWithinAttackRange()
    {
        float targetDistance = Vector3.Distance(transform.position, player.transform.position);
        return targetDistance <= 5.0f;
    }

    // ���� ó�� �޼���
    void HandleAttack()
    {
        // ���� ���·� ����
        if (currentState != BossState.Attack)
        {
            StartCoroutine(RepeatAttack(BasicAttack, 1.5f));
            StartCoroutine(RepeatAttack(SkillAttack, 10.0f));
            StartCoroutine(SkillActive2());
            currentState = BossState.Attack;
        }
    }

    // ��ƼŬ ����
    public void CreateParticle()
    {
        // ��ƼŬ�� ���� ť �迭 �ʱ�ȭ
        particleQueues = new Queue<GameObject>[attackParticle.Length];

        // ��ƼŬ �������� ť ���� �� �ʱ�ȭ
        for (int i = 0; i < attackParticle.Length; i++)
        {
            particleQueues[i] = new Queue<GameObject>();

            // �� ��ƼŬ ������ ���� �ʱ� ť�� ������Ʈ �����Ͽ� �߰�
            for (int j = 0; j < 15; j++) // 10���� ť�� ����
            {
                GameObject particlePool = GameObject.Find("ParticlePool");
                GameObject particleInstance = Instantiate(attackParticle[i], transform.position, Quaternion.identity);
                particleInstance.transform.parent = particlePool.transform.GetChild(0);
                particleInstance.SetActive(false); // Ȱ��ȭ���� ���� ���·� ����
                particleQueues[i].Enqueue(particleInstance); // ť�� ��ƼŬ �߰�
            }
        }
    }

    // �⺻ ���� �ڷ�ƾ
    IEnumerator BasicAttack()
    {
        Vector3 playerPosition = player.transform.position;
        playerPosition.y += 0.05f;

        anim.SetTrigger("Attack");

        attackPP1.transform.position = playerPosition;
        attackPP1.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(0.7f);
        basicAttackHitbox.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        basicAttackHitbox.SetActive(false);

        attackPP1.transform.position = attackPP1.transform.position;
        yield return new WaitForSeconds(0.5f);


    }

    // ��ų ���� �ڷ�ƾ
    IEnumerator SkillAttack()
    {
        attackPP2.SetActive(true);
        yield return new WaitForSeconds(4.0f);

        attackPP2.SetActive(false);
    }
    IEnumerator SkillActive2()
    {
        GameObject particlePool = GameObject.Find("ParticlePool");

        while (true)
        {
            // Ȱ��ȭ���� ���� ��ƼŬ �߿��� ������ �ϳ� �����Ͽ� Ȱ��ȭ
            GameObject inactiveParticle = GetInactiveParticle(particlePool.transform.GetChild(0));
            if (inactiveParticle != null && PlayerIsWithinAttackRange())
            {
                Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                Vector3 randomPosition = transform.position + randomOffset;
                randomPosition.y = 4.5f;

                inactiveParticle.transform.position = randomPosition;
                inactiveParticle.SetActive(true);

                // Ȱ��ȭ�� ��ƼŬ�� �����ϴ� ť�� �߰�
                activeParticles.Enqueue(inactiveParticle);

                if (activeParticles.Count > 7) // �ִ� Ȱ��ȭ ��ƼŬ ��
                {
                    GameObject oldestParticle = activeParticles.Dequeue();
                    oldestParticle.SetActive(false);
                }
            }
            else
            {
                if (activeParticles.Count > 0) // �ִ� Ȱ��ȭ ��ƼŬ ��
                {
                    GameObject oldestParticle = activeParticles.Dequeue();
                    oldestParticle.SetActive(false);
                }
            }
     
            // ���� �ð� ���
            yield return new WaitForSeconds(1.2f);
        }
    }

    // ��Ȱ��ȭ�� ��ƼŬ �߿��� �����ϰ� �����ϴ� �޼���
    GameObject GetInactiveParticle(Transform particlePool)
    {
        List<GameObject> inactiveParticles = new List<GameObject>();
        foreach (Transform child in particlePool)
        {
            if (!child.gameObject.activeSelf)
            {
                inactiveParticles.Add(child.gameObject);
            }
        }

        if (inactiveParticles.Count > 0)
        {
            int randomIndex = Random.Range(0, inactiveParticles.Count);
            return inactiveParticles[randomIndex];
        }

        return null;
    }

    // ���� �ݺ��� ���� �ڷ�ƾ
    IEnumerator RepeatAttack(Func<IEnumerator> attackCoroutine, float interval)
    {
        while (true)
        {
            yield return StartCoroutine(attackCoroutine());
            yield return new WaitForSeconds(interval);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Hitbox") && currentHp != 0)
        {
            TakeDamage(other.GetComponentInParent<PlayerStatus>().attack);
        }
    }
    IEnumerator DieDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ���� �ð� ���

        dieEffectParticle.SetActive(false);
        gameObject.SetActive(false);

        GameManager.Instance.FillMonsterPool();
    }
}
