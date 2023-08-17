using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Text;
[Serializable]
public class BlendIndexMapping
{
    public String From, To;
    [HideInInspector]
    public int IdFrom, IdTo;
}

[CreateAssetMenu(fileName = "BlendShapeMapping", menuName = "ScriptableObject/BlendShapeMapping", order = 0)]
public class BlendShapeMapping : ScriptableObject
{
    [HideInInspector]
    public SkinnedMeshRenderer Original, Target;

    public List<BlendIndexMapping> IndexMapping;

    public void InitMapIndex(SkinnedMeshRenderer Original, SkinnedMeshRenderer Target)
    {
        this.Original = Original;
        this.Target = Target;
        for (int i = 0; i < IndexMapping.Count; i++)
        {

            IndexMapping[i].IdFrom = Original.sharedMesh.GetBlendShapeIndex(IndexMapping[i].From);

            int Index = Target.sharedMesh.GetBlendShapeIndex(IndexMapping[i].To);
            IndexMapping[i].IdTo = Index;
        }

    }


}