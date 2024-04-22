using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SKill", menuName = "New SKill/SKill")]
public class Skill : ScriptableObject
{
    public string SkillName;
    public string SkillInfo;
    public Sprite SkillImage;
    public float SkillMp;
    public float SkillCoolTime;
}
