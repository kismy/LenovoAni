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
    B2,
    C,
    F1,
    F2
}

public class RoleManager : MonoBehaviour
{
    [SerializeField] Dropdown dropdown;

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

    [SerializeField] BlendShapeRetargeting face;

    [SerializeField] bool driveByAni = true;
    [SerializeField] bool AdjustJointsBeforeCreateAvatar = false;
    [SerializeField] bool isMan = true;
    [SerializeField] Vector3 InitPosOffset = Vector3.zero;
    [SerializeField] bool modifyOffsetWhenPlayIdleAni = true;

    private void Awake()
    {
        instance = this;

        AniOffsetDataPath=System.IO.Path.Combine(Application.dataPath , "../Data/SkeletonModifyData");
        dropdown.onValueChanged.AddListener(OnRoleSwitch);
    }
    void Start()
    {
        LoadGLB_GLTFUtility.Instance.ImportGLB_GLTFAsync(string.Format(Application.dataPath+ "/../Data/roleModels/{0}.glb",roleType.ToString()), OnFinishAsync);
        //LoadGLB_GLTFUtility.Instance.ImportGLB_GLTFAsync(Application.dataPath+ "/../roleModels/64d495b4651a0d35000406bc.glb", OnFinishAsync);
    }
    public void OnRoleSwitch(int index)
    { 
    
    }
    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnRoleSwitch);
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
        root.name = configFileName;

        result.transform.parent = root;
        result.transform.localPosition = new Vector3(0,0,0);
        root.localPosition = InitPosOffset;

        if (AdjustJointsBeforeCreateAvatar)
        {
            Transform LeftShoulder = root.Find(PLeftShoulder);
            Transform LeftArm = root.Find(PLeftArm);
            Transform RightShoulder = root.Find(PRightShoulder);
            Transform RightArm = root.Find(PRightArm);
            if (isMan)
            {
                LeftShoulder.localRotation = Quaternion.Euler(76.4f, 87.3f, 179f);
                LeftArm.localRotation = Quaternion.Euler(346f, 355.4f, 359.3f);
                RightShoulder.localRotation = Quaternion.Euler(78.9f, 273f, 181.6f);
                RightArm.localRotation = Quaternion.Euler(349.2f, 4.6f, 0.644f);
            }
            else
            {
                LeftShoulder.localRotation = Quaternion.Euler(100, -90, 11);
                LeftArm.localRotation = Quaternion.Euler(-8, -2f, -12);
                RightShoulder.localRotation = Quaternion.Euler(100, 90, -11);
                RightArm.localRotation = Quaternion.Euler(-8, 2f, 12f);
            }
        }

        if (driveByAni)
        {
            Animator animator = root.gameObject.AddComponent<Animator>();
            animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
            //animator.avatar = manAvatar;
            animator.runtimeAnimatorController = controller;
            DynamicAvatar dynamicAvatar = root.gameObject.AddComponent<DynamicAvatar>();
            dynamicAvatar.Init(root.gameObject, animator, avatar);


            SkeletonModify skeletonModify = root.gameObject.AddComponent<SkeletonModify>();
            skeletonModify.Init(configFileName, joint, text, animator,modifyOffsetWhenPlayIdleAni);

            face.Init(SkinDic);
        }

    }


}
