using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DynamicAvatar : MonoBehaviour
{
    [SerializeField] GameObject avatarRoot;
    [SerializeField] Animator animator;
    public bool autoInit = false;

    void Start()
    {
        if(autoInit)
            Init(avatarRoot, animator,null);
    }

    private void AddBone(BoneMappingInfo root)
    {
        HumanSkeletonMap.Add(root.TargetBoneName,root.HumanBoneName);
        if(root.Children!=null)
            foreach (var chid in root.Children)
            {
                if(chid!=null)
                    AddBone(chid);
            }
        if(root.Child!=null)
            AddBone(root.Child);

    }

    public void Init(GameObject root, Animator animator,Avatar avatar)
    {
        this.avatarRoot = root;
        this.animator = animator;
        HumanSkeletonMap.Clear();
        AvatarBoneMappingInfo mapinfo = new AvatarBoneMappingInfo();
        AddBone(mapinfo.Root);
        string savePath = System.IO.Path.Combine(Application.streamingAssetsPath, "SkeletonModifyData" ,"AvatarBoneMappingInfo.json");
        AvatarBoneMappingInfo.SaveToJson(savePath,mapinfo);

        //foreach (var item in skeletonMaps)
        //{
        //    HumanSkeletonMap.Add(item.SkeletonGO.name, item.SkeletonName);
        //}

        /// <summary>
        /// 2.4 创建Avatar
        /// </summary>
        HumanDescription humanDescription = new HumanDescription()
        {
            upperArmTwist = 0.5f,
            lowerArmTwist = 0.5f,
            upperLegTwist = 0.5f,
            lowerLegTwist = 0.5f,
            armStretch = 0.05f,
            legStretch = 0.05f,
            feetSpacing = 0f,
            hasTranslationDoF = false,
            skeleton = CreateSkeletonBone(avatarRoot),
            human = CreateHumanBone(avatarRoot),
        };
        if (animator.avatar == null && avatar == null)
        {
            avatar = AvatarBuilder.BuildHumanAvatar(avatarRoot, humanDescription);
            animator.avatar = avatar;
        }
        animator.enabled = true;

    }

    /// <summary>
    /// 2.2 创建Skeleton数据
    /// </summary>
    /// <param name="avatarRoot"></param>
    /// <returns></returns>
    private static SkeletonBone[] CreateSkeletonBone(GameObject avatarRoot)
    {
        List<SkeletonBone> skeleton = new List<SkeletonBone>();
       Transform[] avatarTransforms = avatarRoot.GetComponentsInChildren<Transform>();
        foreach (Transform avatarTransform in avatarTransforms)
        {
            SkeletonBone bone = new SkeletonBone()
            {
                name = avatarTransform.name,
                position = avatarTransform.localPosition,
                rotation = avatarTransform.localRotation,
                scale = avatarTransform.localScale
            };

            skeleton.Add(bone);
        }
        return skeleton.ToArray();
    }


    /// <summary>
    /// 2.3 创建Human映射关系
    /// </summary>
    public static Dictionary<string, string> HumanSkeletonMap = new Dictionary<string, string>()
    {


    };

    private static HumanBone[] CreateHumanBone(GameObject avatarRoot)
    {
        List<HumanBone> human = new List<HumanBone>();

        Transform[] avatarTransforms = avatarRoot.GetComponentsInChildren<Transform>();
        foreach (Transform avatarTransform in avatarTransforms)
        {
            if (HumanSkeletonMap.TryGetValue(avatarTransform.name, out string humanName))
            {
                HumanBone bone = new HumanBone
                {
                    boneName = avatarTransform.name,
                    humanName = humanName,
                    limit = new HumanLimit() { useDefaultValues = true }
                };

                human.Add(bone);
            }
        }
        return human.ToArray();
    }






}
