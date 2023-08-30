using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum EFaceType
{
    EyeLeft,
    EyeRight,
    Face,
    Teeth
}

public class RoleManager : MonoBehaviour
{
    public static RoleManager instance;
    [SerializeField] string configFileName = "ManSittingDynamicAvatar";
    [SerializeField] RuntimeAnimatorController controller;
    [SerializeField] Transform joint;
    [SerializeField] Text text;
    public Dictionary<EFaceType,SkinnedMeshRenderer> SkinDic=new Dictionary<EFaceType, SkinnedMeshRenderer>();
    void Start()
    {
        instance = this;
        LoadGLB_GLTFUtility.Instance.ImportGLB_GLTFAsync(Application.dataPath+ "/../roleModels/6476e63cc16b82b1e6b9760f.glb", OnFinishAsync);
    }

    /// <summary>
    /// 完成加载
    /// </summary>
    /// <param name="result">加载出来的物体</param>
    /// <param name="clip">加载的动画</param>
    /// <param name="transform"></param>
    /// <param name="name"></param>
    void OnFinishAsync(GameObject result, AnimationClip[] clip)
    {
        Transform root= GameObject.Instantiate(joint);
        root.position = new Vector3(0f,0f,0);
        root.name = configFileName;
        SkinnedMeshRenderer[] renderers = result.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var item in renderers)
        {
            item.gameObject.layer = 6;
            if (item.gameObject.name.Contains("EyeLeft"))
                SkinDic.Add(EFaceType.EyeLeft, item);
            else if (item.gameObject.name.Contains("EyeRight"))
                SkinDic.Add(EFaceType.EyeRight, item);
            else if (item.gameObject.name.Contains("Wolf3D_Head"))
                SkinDic.Add(EFaceType.Face, item);
            else if (item.gameObject.name.Contains("Teeth"))
                SkinDic.Add(EFaceType.Teeth, item);
        }
        result.transform.parent = root;
        result.transform.localPosition = new Vector3(0,0,0);

        Animator animator= root.gameObject.AddComponent<Animator>();
        animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        //animator.avatar = manAvatar;
        animator.runtimeAnimatorController = controller;
        DynamicAvatar dynamicAvatar= root.gameObject.AddComponent<DynamicAvatar>();
        dynamicAvatar.Init(root.gameObject,animator);


        SkeletonModify skeletonModify = root.gameObject.AddComponent<SkeletonModify>();
        skeletonModify.Init(configFileName,joint, text, animator);

    }


}
