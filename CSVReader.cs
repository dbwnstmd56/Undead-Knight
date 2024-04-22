using static Item;
using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using static UnityEditor.Progress;

public class CSVReader : MonoBehaviour
{
    public TextAsset csvFile; // CSV 파일을 저장할 변수

    [MenuItem("Tools/Create Scriptable Objects from CSV")]
    public static void CreateScriptableObjectsFromCSV()
    {
        // CSV 파일 읽기
        string[] lines = File.ReadAllLines("Assets/Resources/items.csv");

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            // 각 라인을 쉼표(,)로 구분하여 필드를 추출
            string[] fields = line.Split(',');

            // 추출한 필드를 기반으로 스크립터블 오브젝트 생성
            Item item = ScriptableObject.CreateInstance<Item>();
            string path;
            switch (fields[0])
            {
                case "Used":
                    item.itemType = ItemType.Used;
                    item.ItemID = int.Parse(fields[1]);
                    item.ItemName = fields[2];
                    item.Recovery = float.Parse(fields[3]);
                    item.Price = float.Parse(fields[4]);
                    item.Sale = float.Parse(fields[5]);
                    item.DropRate = float.Parse(fields[8]);
                    item.ItemInfo = fields[9];
                    path = "Assets/Resources/Items/Used/" + item.ItemName + ".asset";
                    AssetDatabase.CreateAsset(item, path);
                    break;
                case "Equipment":
                    item.itemType = ItemType.Equipment;
                    item.ItemID = int.Parse(fields[1]);
                    item.ItemName = fields[2];
                    item.Price = float.Parse(fields[4]);
                    item.Sale = float.Parse(fields[5]);
                    if (fields[6] != "F")
                        item.Attack = int.Parse(fields[6]);
                    if (fields[7] != "F")
                        item.Defense = int.Parse(fields[7]);
                    item.DropRate = float.Parse(fields[8]);
                    item.ItemInfo = fields[9];
                    item.EquipmentTypeNumber = int.Parse(fields[10]);
                    path = "Assets/Resources/Items/Equipment/" + item.ItemName + ".asset";
                    AssetDatabase.CreateAsset(item, path);
                    break;
                case "Ingredient":
                    item.itemType = ItemType.Used;
                    item.ItemID = int.Parse(fields[1]);
                    item.ItemName = fields[2];
                    item.Price = float.Parse(fields[4]);
                    item.Sale = float.Parse(fields[5]);
                    item.DropRate = float.Parse(fields[8]);
                    item.ItemInfo = fields[9];
                    path = "Assets/Resources/Items/Ingredient/" + item.ItemName + ".asset";
                    AssetDatabase.CreateAsset(item, path);
                    break;
                case "ETC":
                    item.itemType = ItemType.ETC;
                    item.ItemID = int.Parse(fields[1]);
                    item.ItemName = fields[2];
                    item.Sale = float.Parse(fields[5]);
                    item.DropRate = float.Parse(fields[8]);
                    item.ItemInfo = fields[9];
                    path = "Assets/Resources/Items/ETC/" + item.ItemName + ".asset";
                    AssetDatabase.CreateAsset(item, path);
                    break;
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
