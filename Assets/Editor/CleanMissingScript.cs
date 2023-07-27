using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CleanMissingScript 
{
    [MenuItem("Tools/移除丢失的脚本")]
    public static void RemoveMissingScript()
    {
        var gos = GameObject.FindObjectsOfType<GameObject>();
        foreach (var item in gos)
        {
            Debug.Log(item.name);
            SerializedObject so = new SerializedObject(item);
            var soProperties = so.FindProperty("m_Component");
            var components = item.GetComponents<Component>();
            int propertyIndex = 0;
            foreach (var c in components)
            {
                if (c == null)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(item);
                    //soProperties.DeleteArrayElementAtIndex(propertyIndex);
                }
                ++propertyIndex;
            }
            so.ApplyModifiedProperties();
        }

        AssetDatabase.Refresh();
        Debug.Log("清理完成!");
        //Debug.Log(gos.Length);
        //var r= Resources.FindObjectsOfTypeAll<GameObject>();
        //foreach (var item in r)
        //{
        //    Debug.Log(item.name);
        //}
        //Debug.Log(r.Length);

    }

}
