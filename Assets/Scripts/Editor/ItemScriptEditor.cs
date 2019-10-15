using Assets.Scripts.ScriptableComponents.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts
{
    [CustomEditor(typeof(ItemScriptable))]
    public class ItemScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ItemScriptable item = (ItemScriptable)target;
            EditorGUILayout.HelpBox("Common Fields", MessageType.None);
            item.Name = EditorGUILayout.TextField("Name", item.Name);
            EditorGUILayout.LabelField("Description");
            item.Description = EditorGUILayout.TextArea("", GUILayout.Height(40));

            EditorGUILayout.HelpBox("Configuration Fields", MessageType.None);
            item.Stackable = EditorGUILayout.Toggle("Stackable", item.Stackable);
            if (item.Stackable) item.StackableAmout = EditorGUILayout.IntSlider("Amount",item.StackableAmout,2, 10);

            EditorGUILayout.HelpBox("Graphic Configuration", MessageType.None);
            item.InventorySprite =  (Sprite)EditorGUILayout.ObjectField(item.InventorySprite, typeof(Sprite), allowSceneObjects: true);


            base.OnInspectorGUI();
        }
    }
}
