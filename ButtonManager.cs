using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManager;

public class ButtonManager : MonoBehaviour
{
    public GameObject player;
    public Transform TownPos;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public void ClearBoss()
    {
        player.transform.position = TownPos.position;
        SoundManager.Instance.PlayBgm(true, 0, Bgm.TownBgm);
    }
}
