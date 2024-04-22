using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class Skeleton : Enemy
{
    public GameObject AttackBox;
    public Item[] dropItem;
    int random;
    void Update()
    {
        Tracking();
    }

    public void AttackBoxOn()
    {
        AttackBox.SetActive(true);
    }
    public void AttackBoxOff()
    {
        AttackBox.SetActive(false);
    }
    public override void Die()
    {
        base.Die();
        foreach (Item item in dropItem)
        {
            random = Random.Range(0, 100);
            if (random <= item.DropRate)
            {
                // ������ ���
                InventoryController.instance.AcquireItem(item);
            }
        }
        player.GetComponent<PlayerController>().Gold += random * 10;
    }
}
