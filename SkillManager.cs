using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public GameObject[] skillSlot;
    public Image[] skillsCoolTime;

    public GameObject PowerAttackEffect;
    public GameObject SlashAttackEffect;

    public GameObject PowerAttackHitbox;
    public GameObject SlashAttackHitbox;

    public GameObject player;
    void Start()
    {
        initSkill();
    }
    void initSkill()
    {
        player = GameObject.FindWithTag("Player");
   
        Transform powerAttackEffect = player.transform.Find("PowerAttackEffect");
        PowerAttackEffect = powerAttackEffect.gameObject;

        Transform powerHitbox = PowerAttackEffect.transform.Find("Hitbox");
        PowerAttackHitbox = powerHitbox.gameObject;

        Transform slashAttackEffect = player.transform.Find("SlashAttackEffect");
        SlashAttackEffect = slashAttackEffect.gameObject;

        Transform slashHitbox = SlashAttackEffect.transform.Find("Pivot").transform.Find("Hitbox");
        SlashAttackHitbox = slashHitbox.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        UseSkill();
    }
    public void UseSkill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (skillSlot[0].GetComponent<InGameSkillSlot>().skillObject == null) return;
            float skillCoolTime = skillSlot[0].GetComponent<InGameSkillSlot>().skillObject.SkillCoolTime;
            StartCoroutine(ReduceCooldown(skillCoolTime, skillCoolTime, 0));
            StartCoroutine(useSkill(skillSlot[0].GetComponent<InGameSkillSlot>().skillObject , skillSlot[0].GetComponent<InGameSkillSlot>().isSkill, skillSlot[0].GetComponent<InGameSkillSlot>().slotIndex));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (skillSlot[1].GetComponent<InGameSkillSlot>().skillObject == null) return;
            float skillCoolTime = skillSlot[1].GetComponent<InGameSkillSlot>().skillObject.SkillCoolTime;
            StartCoroutine(ReduceCooldown(skillCoolTime, skillCoolTime, 1));
            StartCoroutine(useSkill(skillSlot[1].GetComponent<InGameSkillSlot>().skillObject, skillSlot[1].GetComponent<InGameSkillSlot>().isSkill, skillSlot[1].GetComponent<InGameSkillSlot>().slotIndex));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (skillSlot[2].GetComponent<InGameSkillSlot>().skillObject == null) return;
            StartCoroutine(useSkill(skillSlot[2].GetComponent<InGameSkillSlot>().skillObject, skillSlot[2].GetComponent<InGameSkillSlot>().isSkill, skillSlot[2].GetComponent<InGameSkillSlot>().slotIndex));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (skillSlot[3].GetComponent<InGameSkillSlot>().skillObject == null) return;
            StartCoroutine(useSkill(skillSlot[3].GetComponent<InGameSkillSlot>().skillObject, skillSlot[3].GetComponent<InGameSkillSlot>().isSkill, skillSlot[3].GetComponent<InGameSkillSlot>().slotIndex));
        }
    }
    IEnumerator useSkill(Skill skill ,bool isSkill, int slotIndex)
    {
        if (skill.SkillName =="PowerAttack" && isSkill && player.GetComponent<PlayerStatus>().curretnMP >= skill.SkillMp)
        {
            skillSlot[slotIndex].GetComponent<InGameSkillSlot>().isSkill = false;

            player.GetComponent<PlayerController>().anim.SetTrigger("PowerAttack");
            player.GetComponent<PlayerStatus>().curretnMP -= skill.SkillMp;

            yield return new WaitForSeconds(1.0f);

            PowerAttackEffect.GetComponent<ParticleSystem>().Play();

            PowerAttackHitbox.SetActive(true);

            SoundManager.Instance.PlaySfx(SoundManager.Sfx.PowerAttack);
            yield return new WaitForSeconds(0.3f);

            PowerAttackHitbox.SetActive(false);
            yield return new WaitForSeconds(skill.SkillCoolTime - 0.3f);

            skillSlot[slotIndex].GetComponent<InGameSkillSlot>().isSkill = true;

        }
        if (skill.SkillName == "SlashAttack" && isSkill && player.GetComponent<PlayerStatus>().curretnMP >= skill.SkillMp)
        {
            skillSlot[slotIndex].GetComponent<InGameSkillSlot>().isSkill = false;

            player.GetComponent<PlayerController>().anim.SetTrigger("SlashAttack");
            player.GetComponent<PlayerStatus>().curretnMP -= skill.SkillMp;

            SoundManager.Instance.PlaySfx(SoundManager.Sfx.SlashAttack);

            yield return new WaitForSeconds(0.7f);

            SlashAttackEffect.GetComponent<ParticleSystem>().Play();

            SlashAttackHitbox.SetActive(true);

            yield return new WaitForSeconds(0.3f);

            SlashAttackHitbox.SetActive(false);

            yield return new WaitForSeconds(skill.SkillCoolTime - 0.3f);

            skillSlot[slotIndex].GetComponent<InGameSkillSlot>().isSkill = true;

        }
    }
    IEnumerator ReduceCooldown(float coolTime,float maxCoolTime, int index)
    {
        skillsCoolTime[index].fillAmount = 1;

        while (skillsCoolTime[index].fillAmount != 0)
        {
            // 쿨타임을 감소시킵니다.
            coolTime -= Time.deltaTime;

            // 스킬 슬롯의 쿨타임 이미지를 갱신합니다.
            skillsCoolTime[index].fillAmount = coolTime / maxCoolTime;

            yield return null;
        }

        // 쿨타임이 종료되면 쿨타임 이미지를 숨깁니다.
        skillsCoolTime[index].fillAmount = 0;
    }

}
