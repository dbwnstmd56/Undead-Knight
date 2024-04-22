using UnityEngine;
using UnityEditor;
using static Item;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty itemTypeProp = serializedObject.FindProperty("itemType");

        // Enum 값에 따라 다른 속성을 표시
        switch ((ItemType)itemTypeProp.enumValueIndex)
        {
            case ItemType.Equipment:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("itemType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemID"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemName"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("EquipmentTypeNumber"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Attack"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Defense"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Price"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Sale"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemInfo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemImage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DropRate"));
                break;
            case ItemType.Used:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("itemType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemID"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemName"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Recovery"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Price"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Sale"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemInfo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemImage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DropRate"));
                break;
            case ItemType.Ingredient:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("itemType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemID"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemName"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Price"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Sale"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemInfo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemImage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DropRate"));
                break;
            case ItemType.ETC:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("itemType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemID"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemName"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Sale"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemInfo"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemImage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DropRate"));
                break;
                // 기타 아이템 종류에 따른 속성 처리
        }

        serializedObject.ApplyModifiedProperties();
    }
}