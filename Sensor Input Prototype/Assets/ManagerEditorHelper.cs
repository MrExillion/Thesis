using UnityEngine;
using UnityEditor;
using System.Runtime.CompilerServices;

[CustomEditor(typeof(IGlobalReferenceManager))]
[CanEditMultipleObjects]


public class ManagerEditorHelper : Editor,IGlobalReferenceManager
{

    SerializedProperty comicManagerProperty1;

    void OnEnable()
    {
        comicManagerProperty1 = serializedObject.FindProperty("testWCT");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(comicManagerProperty1);
        serializedObject.ApplyModifiedProperties();
    }

    //[MenuItem("Tools/MyTool/Do It in C#")]
    //    static void DoIt()
    //    {
    //        EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    //    }
}


