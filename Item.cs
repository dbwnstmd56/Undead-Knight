using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public string ItemInfo;

    public int ItemID;

    public float Recovery;  //Used
    public float Price;
    public float Sale;
    public int EquipmentTypeNumber;
    public int Attack;  //Equipment
    public int Defense; //Equipment
    public float DropRate;
    public GameObject ItemPrefab;

    public Sprite ItemImage;

    public ItemType itemType;
    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }
}
