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
public enum RoleType
{ 
    A,
    B,
    B2
}

public class RoleManager : MonoBehaviour
{

    public static RoleManager instance;
    public string AniOffsetDataPath;
    [SerializeField] string configFileName = "ManSittingDynamicAvatar";
    [SerializeField] RoleType roleType;
    [SerializeField] RuntimeAnimatorController controller;
    [SerializeField] Avatar avatar;
    [SerializeField] Transform joint;
    [SerializeField] Text text;
    public Dictionary<EFaceType,SkinnedMeshRenderer> SkinDic=new Dictionary<EFaceType, SkinnedMeshRenderer>();
    private string PLeftShoulder = "Armature/Hips/Spine/Spine1/Spine2/LeftShoulder";
    private string PLeftArm = "Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm";
    private string PRightShoulder = "Armature/Hips/Spine/Spine1/Spine2/RightShoulder";
    private string PRightArm = "Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm";

    [SerializeField] bool AdjustJointsBeforeCreateAvatar = false;
    private void Awake()
    {
        instance = this;

        AniOffsetDataPath=System.IO.Path.Combine(Application.dataPath , "../Data/SkeletonModifyData");
    }
    void Start()
    {
        LoadGLB_GLTFUtility.Instance.ImportGLB_GLTFAsync(string.Format(Application.dataPath+ "/../Data/roleModels/{0}.glb",roleType.ToString()), OnFinishAsync);
        //LoadGLB_GLTFUtility.Instance.ImportGLB_GLTFAsync(Application.dataPath+ "/../roleModels/64d495b4651a0d35000406bc.glb", OnFinishAsync);
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
        Transform root = GameObject.Instantiate(joint);
        root.position = new Vector3(0f, 0f, 0);
        root.name = configFileName;

        result.transform.parent = root;
        result.transform.localPosition = new Vector3(0,0,0);

        if (AdjustJointsBeforeCreateAvatar)
        {
            Transform LeftShoulder = root.Find(PLeftShoulder);
            Transform LeftArm = root.Find(PLeftArm);
            Transform RightShoulder = root.Find(PRightShoulder);
            Transform RightArm = root.Find(PRightArm);
            LeftShoulder.localRotation = Quaternion.Euler(76.3827591f, 87.2930984f, 179.000076f);
            LeftArm.localRotation = Quaternion.Euler(345.954071f, 355.431458f, 359.34552f);
            RightShoulder.localRotation = Quaternion.Euler(78.8968277f, 273.109344f, 181.604721f);
            RightArm.localRotation = Quaternion.Euler(349.174835f, 4.60694408f, 0.643833816f);
        }


        Animator animator= root.gameObject.AddComponent<Animator>();
        animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        //animator.avatar = manAvatar;
        animator.runtimeAnimatorController = controller;
        DynamicAvatar dynamicAvatar= root.gameObject.AddComponent<DynamicAvatar>();
        dynamicAvatar.Init(root.gameObject,animator,avatar);


        SkeletonModify skeletonModify = root.gameObject.AddComponent<SkeletonModify>();
        skeletonModify.Init(configFileName,joint, text, animator);

    }


}
