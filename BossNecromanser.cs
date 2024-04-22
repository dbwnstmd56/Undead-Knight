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

    // 보스의 현재 상태
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

        // 현재 상태에 따라 다른 동작 실행
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

    // Chase 상태 업데이트
    void UpdateChaseState()
    {
        if (PlayerIsWithinAttackRange())
        {
            anim.SetBool("Walk", false);
            HandleAttack();
        }
        else
        {
            // 공격 범위 내에 플레이어가 없으면 플레이어를 향해 이동
            anim.SetBool("Walk", true);
            nav.SetDestination(player.transform.position);
        }
    }

    // Attack 상태 업데이트
    void UpdateAttackState()
    {
        // 공격 범위를 벗어나면 Chase 상태로 전이
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

    // Dead 상태 업데이트
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

    // 플레이어가 공격 범위 내에 있는지 확인하는 메서드
    bool PlayerIsWithinAttackRange()
    {
        float targetDistance = Vector3.Distance(transform.position, player.transform.position);
        return targetDistance <= 5.0f;
    }

    // 공격 처리 메서드
    void HandleAttack()
    {
        // 공격 상태로 변경
        if (currentState != BossState.Attack)
        {
            StartCoroutine(RepeatAttack(BasicAttack, 1.5f));
            StartCoroutine(RepeatAttack(SkillAttack, 10.0f));
            StartCoroutine(SkillActive2());
            currentState = BossState.Attack;
        }
    }

    // 파티클 생성
    public void CreateParticle()
    {
        // 파티클을 담을 큐 배열 초기화
        particleQueues = new Queue<GameObject>[attackParticle.Length];

        // 파티클 종류마다 큐 생성 및 초기화
        for (int i = 0; i < attackParticle.Length; i++)
        {
            particleQueues[i] = new Queue<GameObject>();

            // 각 파티클 종류에 대해 초기 큐에 오브젝트 생성하여 추가
            for (int j = 0; j < 15; j++) // 10개씩 큐에 넣음
            {
                GameObject particlePool = GameObject.Find("ParticlePool");
                GameObject particleInstance = Instantiate(attackParticle[i], transform.position, Quaternion.identity);
                particleInstance.transform.parent = particlePool.transform.GetChild(0);
                particleInstance.SetActive(false); // 활성화되지 않은 상태로 시작
                particleQueues[i].Enqueue(particleInstance); // 큐에 파티클 추가
            }
        }
    }

    // 기본 공격 코루틴
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

    // 스킬 공격 코루틴
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
            // 활성화되지 않은 파티클 중에서 랜덤한 하나 선택하여 활성화
            GameObject inactiveParticle = GetInactiveParticle(particlePool.transform.GetChild(0));
            if (inactiveParticle != null && PlayerIsWithinAttackRange())
            {
                Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                Vector3 randomPosition = transform.position + randomOffset;
                randomPosition.y = 4.5f;

                inactiveParticle.transform.position = randomPosition;
                inactiveParticle.SetActive(true);

                // 활성화된 파티클을 추적하는 큐에 추가
                activeParticles.Enqueue(inactiveParticle);

                if (activeParticles.Count > 7) // 최대 활성화 파티클 수
                {
                    GameObject oldestParticle = activeParticles.Dequeue();
                    oldestParticle.SetActive(false);
                }
            }
            else
            {
                if (activeParticles.Count > 0) // 최대 활성화 파티클 수
                {
                    GameObject oldestParticle = activeParticles.Dequeue();
                    oldestParticle.SetActive(false);
                }
            }
     
            // 일정 시간 대기
            yield return new WaitForSeconds(1.2f);
        }
    }

    // 비활성화된 파티클 중에서 랜덤하게 선택하는 메서드
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

    // 공격 반복을 위한 코루틴
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
        yield return new WaitForSeconds(delay); // 일정 시간 대기

        dieEffectParticle.SetActive(false);
        gameObject.SetActive(false);

        GameManager.Instance.FillMonsterPool();
    }
}
