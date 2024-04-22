using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Item;
using static Quest;

[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty itemTypeProp = serializedObject.FindProperty("questType");

        // Enum 값에 따라 다른 속성을 표시
        switch ((QuestType)itemTypeProp.enumValueIndex)
        {
            case QuestType.Hunt:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questId"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questName"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("monstersId"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("monsterKill"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("currentMonsterKill"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("succeedKillCount"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("succeedQuest"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questReward"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rewardGold"));

                break;
            case QuestType.Growth:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questId"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questName"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("growthLevel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("currentGrowthLevel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("succeedQuest"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questReward"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rewardGold"));

                break;
            case QuestType.Conversation:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questId"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questName"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("npcId"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("succeedQuest"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("questReward"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rewardGold"));

                break;
          
        }

        serializedObject.ApplyModifiedProperties();
    }
}
