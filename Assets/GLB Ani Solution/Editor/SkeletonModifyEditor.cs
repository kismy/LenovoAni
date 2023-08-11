using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkeletonModify))]
public class SkeletonModifyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SkeletonModify skeletonModify = (SkeletonModify)target;
        EditorGUILayout.Space(25);
        if (GUILayout.Button("SaveCurAnimationModify"))
        {
            skeletonModify.SaveCurAnimationModify();
        }
        EditorGUILayout.Space(10);

        if (GUILayout.Button("LoadCurAnimationModify"))
        {
            skeletonModify.LoadCurAnimationModify();
        }
        EditorGUILayout.Space(10);

        if (GUILayout.Button("ClearCurAnimationModify"))
        {
            skeletonModify.ClearCurAnimationModify();
        }
    }
}
