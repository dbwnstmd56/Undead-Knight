using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int EXP;
    public int monsterID;
    public float maxHp;
    public float currentHp;
    public int attack;
    public Image hpBar;
    public Animator anim;
    public NavMeshAgent nav;
    public GameObject player;
    public float wanderRadius = 0.5f;
    IEnumerator wanderCoroutine;
    public GameObject dieEffectParticle;
    public Transform spawnPos;
    public string monsterName;

    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        StartWandering();
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        hpBar.fillAmount = currentHp / maxHp;

        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }
    public void MonsterInit()
    {
        currentHp = maxHp;
        hpBar.fillAmount = currentHp / maxHp;
    }

    public virtual void Die()
    {
        // ���Ͱ� ���� ���� ���� ����
        anim.SetTrigger("Die");
        anim.SetBool("Attack", false);
        nav.speed = 0;
        StopWandering();
        player.GetComponent<PlayerStatus>().currentEXP += EXP;
        dieEffectParticle.SetActive(true);
        StartCoroutine(DieDelay(3.0f));
        QuestManager.Instance.UpdateQuest(monsterID);
    }

    public void Tracking()
    {
        float targetDistance = Vector3.Distance(transform.position, player.transform.position);
        if (targetDistance <= 2.0f && currentHp != 0)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", true);
            StopWandering();
            transform.LookAt(player.transform.position);
        }
        else if (targetDistance < 10f && targetDistance > 2.0f && currentHp != 0)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Attack", false);
            nav.speed = 2.5f;
            nav.SetDestination(player.transform.position);
        }
        else if(targetDistance > 10f)
        {
            StartWandering();
        }
    }
    IEnumerator Wander()
    {
        nav.enabled = true;

        while (true)
        {
            // �������� ��ġ ����
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
            var wanderPoint = hit.position;
            nav.speed = 2.5f;

            // ���Ͱ� �̵��� ��ǥ ��ġ ����
            nav.SetDestination(wanderPoint);

            // �̵��� �Ϸ�� ������ ����մϴ�.
            while (nav.remainingDistance > nav.stoppingDistance)
            {
                anim.SetBool("Walk", true);
                yield return null;
            }

            // �̵��� �Ϸ�Ǹ� �ִϸ��̼��� ����մϴ�.
            anim.SetBool("Walk", false);

            // ���ð� �Ŀ� �ٽ� �̵��մϴ�.
            yield return new WaitForSeconds(4.0f);
        }
    }
    public void StartWandering()
    {
        if (wanderCoroutine == null)
        {
            wanderCoroutine = Wander();
            StartCoroutine(wanderCoroutine);
        }
    }

    public void StopWandering()
    {
        if (wanderCoroutine != null)
        {
            nav.speed = 0f;
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }
    }
    IEnumerator DieDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ���� �ð� ���

        dieEffectParticle.SetActive(false);
        gameObject.SetActive(false);

        MonsterInit();

        GameManager.Instance.FillMonsterPool();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Hitbox") && currentHp != 0)
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                anim.SetTrigger("Hit");
            }
            TakeDamage(other.GetComponent<Hitbox>().attack);
        }
    }

}
