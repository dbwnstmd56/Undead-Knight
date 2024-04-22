using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManager;

public class BossGate : MonoBehaviour
{
    public GameObject openDoor;
    public GameObject closeDoor;
    public GameObject boss;
    public GameObject HpBar;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            openDoor.SetActive(false);
            closeDoor.SetActive(true);
            HpBar.SetActive(true);
            SoundManager.Instance.PlayBgm(true, 0, Bgm.BossBgm);
            boss.GetComponent<BossNecromanser>().currentState = BossNecromanser.BossState.Chase;
        }

    }
}
