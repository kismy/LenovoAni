using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using LitJson;
using System.IO;

[System.Serializable]
public class ModifyData
{
    public string ModifySkeleton;
    public Vector3 LocalPos;
    public Vector3 LocalRotation;
    public override string ToString()
    {
        return ModifySkeleton;
    }
}
public class SkeletonModifyData 
{
    public string roleName;
    public string aniStateName;
    public List<ModifyData> modifies;

    public override string ToString()
    {
        return aniStateName;
    }

}

[System.Serializable]
public class BoneMappingInfo
{
    public override string ToString()
    {
        return HumanBoneName;
    }
    public string HumanBoneName;
    public string TargetBoneName;
    [XmlArrayItem("Children")]
    public List<BoneMappingInfo> Children;
    [XmlElement("Child")]
    public BoneMappingInfo Child;
    public void SetChildren(params BoneMappingInfo[] children)
    {
        if (Children == null)
            Children = new List<BoneMappingInfo>();
        foreach (var child in children)
        {
            if (Children.FindIndex(0, Children.Count, (item) =>
            {
                if (item == child)
                    return true;
                else 
                    return false;
            }) < 0)
            {
                Children.Add(child);
            }
        }
    }
    /// <summary>
    ///  return child
    /// </summary>
    /// <param name="child"></param>
    /// <returns></returns>
    public BoneMappingInfo SetChild(BoneMappingInfo child)
    {
        this.Child = child;
        return child;
    }
}

[System.Serializable]

[XmlRoot("AvatarBoneMapping")]
public class AvatarBoneMappingInfo
{
    public BoneMappingInfo Root;
    public AvatarBoneMappingInfo()
    {
        #region Body
        Root = new BoneMappingInfo() { HumanBoneName= "Root", TargetBoneName= "Root" };
        BoneMappingInfo Hips = new BoneMappingInfo() { HumanBoneName="Hips",TargetBoneName= "Hips" };
        BoneMappingInfo Spine= new BoneMappingInfo() { HumanBoneName = "Spine", TargetBoneName = "Spine" };
        BoneMappingInfo Chest = new BoneMappingInfo() { HumanBoneName = "Chest", TargetBoneName = "Spine1" };
        BoneMappingInfo UpperChest = new BoneMappingInfo() { HumanBoneName = "UpperChest", TargetBoneName = "Spine2" };
        BoneMappingInfo Neck = new BoneMappingInfo() { HumanBoneName = "Neck" ,TargetBoneName= "Neck" };
        BoneMappingInfo Head = new BoneMappingInfo() { HumanBoneName = "Head" ,TargetBoneName= "Head" };

        BoneMappingInfo LeftShoulder = new BoneMappingInfo() { HumanBoneName = "LeftShoulder" ,TargetBoneName= "LeftShoulder" };
        BoneMappingInfo LeftUpperArm = new BoneMappingInfo() { HumanBoneName = "LeftUpperArm" ,TargetBoneName= "LeftArm" };
        BoneMappingInfo LeftLowerArm = new BoneMappingInfo() { HumanBoneName = "LeftLowerArm" ,TargetBoneName= "LeftForeArm" };
        BoneMappingInfo LeftHand = new BoneMappingInfo() { HumanBoneName = "LeftHand" ,TargetBoneName= "LeftHand" };

        BoneMappingInfo RightShoulder = new BoneMappingInfo() { HumanBoneName = "RightShoulder" ,TargetBoneName= "RightShoulder" };
        BoneMappingInfo RightUpperArm = new BoneMappingInfo() { HumanBoneName = "RightUpperArm" ,TargetBoneName= "RightArm" };
        BoneMappingInfo RightLowerArm = new BoneMappingInfo() { HumanBoneName = "RightLowerArm" ,TargetBoneName= "RightForeArm" };
        BoneMappingInfo RightHand = new BoneMappingInfo() { HumanBoneName = "RightHand" ,TargetBoneName= "RightHand" };

        BoneMappingInfo LeftUpperLeg = new BoneMappingInfo() { HumanBoneName = "LeftUpperLeg" ,TargetBoneName= "LeftUpLeg" };
        BoneMappingInfo LeftLowerLeg = new BoneMappingInfo() { HumanBoneName = "LeftLowerLeg" ,TargetBoneName= "LeftLeg" };
        BoneMappingInfo LeftFoot = new BoneMappingInfo() { HumanBoneName = "LeftFoot" ,TargetBoneName= "LeftFoot" };
        BoneMappingInfo LeftToes = new BoneMappingInfo() { HumanBoneName = "LeftToes" ,TargetBoneName= "LeftToeBase" };

        BoneMappingInfo RightUpperLeg = new BoneMappingInfo() { HumanBoneName = "RightUpperLeg" ,TargetBoneName= "RightUpLeg" };
        BoneMappingInfo RightLowerLeg = new BoneMappingInfo() { HumanBoneName = "RightLowerLeg" ,TargetBoneName= "RightLeg" };
        BoneMappingInfo RightFoot = new BoneMappingInfo() { HumanBoneName = "RightFoot" ,TargetBoneName= "RightFoot" };
        BoneMappingInfo RightToes = new BoneMappingInfo() { HumanBoneName = "RightToes" ,TargetBoneName= "RightToeBase" };

        Root.SetChild(Hips);
        Hips.SetChildren(Spine, LeftUpperLeg, RightUpperLeg);
        Spine.SetChild(Chest).SetChild(UpperChest);
        LeftUpperLeg.SetChild(LeftLowerLeg).SetChild(LeftFoot).SetChild(LeftToes);
        RightUpperLeg.SetChild(RightLowerLeg).SetChild(RightFoot).SetChild(RightToes);


        UpperChest.SetChildren(Neck, LeftShoulder, RightShoulder);
        Neck.SetChild(Head);
        LeftShoulder.SetChild(LeftUpperArm).SetChild(LeftLowerArm).SetChild(LeftHand);
        RightShoulder.SetChild(RightUpperArm).SetChild(RightLowerArm).SetChild(RightHand);
     



        #endregion

        #region hand
        BoneMappingInfo LeftThumbProximal = new BoneMappingInfo() { HumanBoneName = "Left Thumb Proximal" ,TargetBoneName= "LeftHandThumb1" };
        BoneMappingInfo LeftThumbIntermediate = new BoneMappingInfo() { HumanBoneName = "Left Thumb Intermediate" ,TargetBoneName= "LeftHandThumb2" };
        BoneMappingInfo LeftThumbDistal = new BoneMappingInfo() { HumanBoneName = "Left Thumb Distal" ,TargetBoneName= "LeftHandThumb3" };

        BoneMappingInfo LeftIndexProximal = new BoneMappingInfo() { HumanBoneName = "Left Index Proximal" ,TargetBoneName= "LeftHandIndex1" };
        BoneMappingInfo LeftIndexIntermediate = new BoneMappingInfo() { HumanBoneName = "Left Index Intermediate" ,TargetBoneName= "LeftHandIndex2" };
        BoneMappingInfo LeftIndexDistal = new BoneMappingInfo() { HumanBoneName = "Left Index Distal" ,TargetBoneName= "LeftHandIndex3" };


        BoneMappingInfo LeftMiddleProximal = new BoneMappingInfo() { HumanBoneName = "Left Middle Proximal" ,TargetBoneName= "LeftHandMiddle1" };
        BoneMappingInfo LeftMiddleIntermediate = new BoneMappingInfo() { HumanBoneName = "Left Middle Intermediate" ,TargetBoneName= "LeftHandMiddle2" };
        BoneMappingInfo LeftMiddleDistal = new BoneMappingInfo() { HumanBoneName = "Left Middle Distal" ,TargetBoneName= "LeftHandMiddle3" };
  

        BoneMappingInfo LeftRingProximal = new BoneMappingInfo() { HumanBoneName = "Left Ring Proximal" ,TargetBoneName= "LeftHandRing1" };
        BoneMappingInfo LeftRingIntermediate = new BoneMappingInfo() { HumanBoneName = "Left Ring Intermediate" ,TargetBoneName= "LeftHandRing2" };
        BoneMappingInfo LeftRingDistal = new BoneMappingInfo() { HumanBoneName = "Left Ring Distal" ,TargetBoneName= "LeftHandRing3" };


        BoneMappingInfo LeftLittleProximal = new BoneMappingInfo() { HumanBoneName = "Left Little Proximal" ,TargetBoneName= "LeftHandPinky1" };
        BoneMappingInfo LeftLittleIntermediate = new BoneMappingInfo() { HumanBoneName = "Left Little Intermediate", TargetBoneName= "LeftHandPinky2" };
        BoneMappingInfo LeftLittleDistal = new BoneMappingInfo() { HumanBoneName = "Left Little Distal" ,TargetBoneName= "LeftHandPinky3" };

        LeftThumbProximal.SetChild(LeftThumbIntermediate).SetChild(LeftThumbDistal);
        LeftIndexProximal.SetChild(LeftIndexIntermediate).SetChild(LeftIndexDistal);
        LeftMiddleProximal.SetChild(LeftMiddleIntermediate).SetChild(LeftMiddleDistal);
        LeftRingProximal.SetChild(LeftRingIntermediate).SetChild(LeftRingDistal);
        LeftLittleProximal.SetChild(LeftLittleIntermediate).SetChild(LeftLittleDistal);

        LeftHand.SetChildren(LeftThumbProximal, LeftIndexProximal, LeftMiddleProximal, LeftRingProximal,LeftLittleProximal);

        //...........................

        BoneMappingInfo RightThumbProximal = new BoneMappingInfo() { HumanBoneName = "Right Thumb Proximal" ,TargetBoneName= "RightHandThumb1" };
        BoneMappingInfo RightThumbIntermediate = new BoneMappingInfo() { HumanBoneName = "Right Thumb Intermediate", TargetBoneName= "RightHandThumb2" };
        BoneMappingInfo RightThumbDistal = new BoneMappingInfo() { HumanBoneName = "Right Thumb Distal" ,TargetBoneName= "RightHandThumb3" };

        BoneMappingInfo RightIndexProximal = new BoneMappingInfo() { HumanBoneName = "Right Index Proximal" ,TargetBoneName= "RightHandIndex1" };
        BoneMappingInfo RightIndexIntermediate = new BoneMappingInfo() { HumanBoneName = "Right Index Intermediate" ,TargetBoneName= "RightHandIndex2" };
        BoneMappingInfo RightIndexDistal = new BoneMappingInfo() { HumanBoneName = "Right Index Distal" ,TargetBoneName= "RightHandIndex3" };

        BoneMappingInfo RightMiddleProximal = new BoneMappingInfo() { HumanBoneName = "Right Middle Proximal" ,TargetBoneName= "RightHandMiddle1" };
        BoneMappingInfo RightMiddleIntermediate = new BoneMappingInfo() { HumanBoneName = "Right Middle Intermediate" ,TargetBoneName= "RightHandMiddle2" };
        BoneMappingInfo RightMiddleDistal = new BoneMappingInfo() { HumanBoneName = "Right Middle Distal" ,TargetBoneName= "RightHandMiddle3" };

        BoneMappingInfo RightRingProximal = new BoneMappingInfo() { HumanBoneName = "Right Ring Proximal" ,TargetBoneName= "RightHandRing1" };
        BoneMappingInfo RightRingIntermediate = new BoneMappingInfo() { HumanBoneName = "Right Ring Intermediate" ,TargetBoneName= "RightHandRing2" };
        BoneMappingInfo RightRingDistal = new BoneMappingInfo() { HumanBoneName = "Right Ring Distal" ,TargetBoneName= "RightHandRing3" };

        BoneMappingInfo RightLittleProximal = new BoneMappingInfo() { HumanBoneName = "Right Little Proximal" ,TargetBoneName= "RightHandPinky1" };
        BoneMappingInfo RightLittleIntermediate = new BoneMappingInfo() { HumanBoneName = "Right Little Intermediate" ,TargetBoneName= "RightHandPinky2" };
        BoneMappingInfo RightLittleDistal = new BoneMappingInfo() { HumanBoneName = "Right Little Distal" ,TargetBoneName= "RightHandPinky3" };

        RightThumbProximal.SetChild(RightThumbIntermediate).SetChild(RightThumbDistal);
        RightIndexProximal.SetChild(RightIndexIntermediate).SetChild(RightIndexDistal);
        RightMiddleProximal.SetChild(RightMiddleIntermediate).SetChild(RightMiddleDistal);
        RightRingProximal.SetChild(RightRingIntermediate).SetChild(RightRingDistal);
        RightLittleProximal.SetChild(RightLittleIntermediate).SetChild(RightLittleDistal);

        RightHand.SetChildren(RightThumbProximal, RightIndexProximal, RightMiddleProximal, RightRingProximal, RightLittleProximal);

        #endregion



    }

    public static AvatarBoneMappingInfo LoadFromJson(string json)
    {
       return JsonUtility.FromJson<AvatarBoneMappingInfo>(json);
    }
    public static void SaveToJson(string savePath, AvatarBoneMappingInfo info )
    {
        string json= JsonMapper.ToJson(info);
        System.IO.File.WriteAllText(savePath,json);
    }

}


public class CustomAvatar
{
    public AvatarBone root;

    public static AvatarBone  Load(string avatarPath)
    {
        string avatarStr= File.ReadAllText(avatarPath);

        return CreateAvatar(avatarStr,0,avatarStr.Length-1);


    }

    public static AvatarBone CreateAvatar(string avatarStr, int start,int end)
    {
        try
        {
            if (start >= end)
                return null;
            AvatarBone Parrent = new AvatarBone();
            Stack<CharIndexInfo> stack = new Stack<CharIndexInfo>();
            for (int i = start; i <= end; i++)
            {
                if (avatarStr[i] == '{')
                {
                    stack.Push(new CharIndexInfo('{', i));
                }
                else if (avatarStr[i] == '}')
                {
                    CharIndexInfo charIndexInfo = stack.Peek();
                    if (charIndexInfo.Char == '{')//Find a avatarBone
                    {
                        stack.Pop();

                        string[] lines =    (i - charIndexInfo.Index > 0) ?
                            avatarStr.Substring(charIndexInfo.Index + 1, i - charIndexInfo.Index).Split('\n', System.StringSplitOptions.RemoveEmptyEntries):
                            null;
                        if (stack.Count == 0)
                        {
                            Parrent.Lines = lines;

                        }
                        else
                        {
                            AvatarBone child = CreateAvatar(avatarStr, charIndexInfo.Index + 1, i - 1);
                            if (child != null)
                            {
                                child.Lines = lines;
                                Parrent.AvatarBones.Add(child);
                            }
                        }
                    }
                }
            }
            return Parrent;
        }
        catch (System.Exception e)
        {
            return null;
        }

    }


    private struct CharIndexInfo
    {
        public CharIndexInfo(char Char, int Index)
        {
            this.Char = Char;
            this.Index = Index;
        }
        public char Char;
        public int Index;
    }
}

public class AvatarBone
{
    public string[] Lines;
    [XmlIgnore]
    private List<AvatarBone> avatarBones;
    public List<AvatarBone> AvatarBones {
        get {
            if (avatarBones == null)
                avatarBones = new List<AvatarBone>();
            return avatarBones;
        
        }
    
    }

}