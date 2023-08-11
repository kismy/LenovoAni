using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DynamicAvatar))]

public class DynamicAvatarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DynamicAvatar dynamicAvatar = (DynamicAvatar)target;

    }
}
