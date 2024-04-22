using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    float h, v;
    float Speed = 5f;

    bool jDown; //점프
    bool isJump;
    bool fDown; //공격
    bool isAttack;

    Vector3 qir;

    public GameObject[] hitBox;

    public float Gold;

    public Animator anim;
    Rigidbody rb;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Jump();
        Attack();
        Dialogue();
    }
    void GetInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        jDown = Input.GetKeyDown(KeyCode.Space);
        fDown = Input.GetMouseButtonDown(0);
    }
    private void Move()
    {
        Vector3 dir = Camera.main.transform.position - transform.position;
        dir.y = 0f;
        dir = dir.normalized;

        Vector3 vir = dir * Time.deltaTime * Speed * v * -1f;
        Vector3 hir = Quaternion.Euler(0, 90, 0) * dir * Time.deltaTime * Speed * h * -1;

        qir = vir + hir;
        if (qir != Vector3.zero && !isAttack)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(qir), 10f);
            transform.position = transform.position + qir;
        }

        anim.SetBool("Run", qir != Vector3.zero);
    }
    private void Attack()
    {
        if(!isJump && fDown && !GameManager.Instance.isActiveUI)
        {
            anim.SetTrigger("Attack");
        }
    }
    private void Jump()
    {
        if (jDown && !isJump)
        {
            rb.AddForce(Vector3.up * 5.5f, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("Jump");
            isJump = true;
        }
    }
    public void Dialogue()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (QuestManager.Instance.NPC != null)
            {
                UIManager.instance.DialogueUI.SetActive(true);
                DialogueManager.instance.ShowDialogue();
            }
        }
    }
    //애니메이션 이벤트에서 공격중 움직임x 할때 사용중
    private void StartAttack()
    {
        isAttack = true;
    }
    private void EndAttack()
    {
        isAttack = false;
    }
    private void HitboxOn(int comboNumber)
    {
        hitBox[comboNumber].SetActive(true);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.Attack);
    }
    private void HitboxOff(int comboNumber)
    {
        hitBox[comboNumber].SetActive(false);
    }
    public void HitDamage(float damage)
    {
        transform.GetComponent<PlayerStatus>().currentHP -= damage;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Item"))
        {
            InventoryController.instance.AcquireItem(other.GetComponent<ItemInfo>().item);
        }
        if(other.transform.CompareTag("EnemyHitbox"))
        {
            HitDamage(other.GetComponent<Hitbox>().attack);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("EnemyHitbox2"))
        {
            HitDamage(other.GetComponent<Hitbox>().attack);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
