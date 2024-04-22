using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolingTrigger : MonoBehaviour
{
    public int monsterid1;
    public Transform spawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
           GameManager.Instance.MonsterObjectPooling(monsterid1, spawnPoint);
            this.gameObject.SetActive(false);
        }

    }
}
