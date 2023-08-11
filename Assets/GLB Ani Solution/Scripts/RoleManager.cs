using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleManager : MonoBehaviour
{
    [SerializeField] string configFileName = "ManSittingDynamicAvatar";
    [SerializeField] RuntimeAnimatorController controller;
    [SerializeField] Transform joint;
    [SerializeField] Text text;
    void Start()
    {
        LoadGLB_GLTFUtility.Instance.ImportGLB_GLTFAsync(Application.dataPath+ "/../roleModels/6476e63cc16b82b1e6b9760f.glb", OnFinishAsync);
    }

    /// <summary>
    /// ��ɼ���
    /// </summary>
    /// <param name="result">���س���������</param>
    /// <param name="clip">���صĶ���</param>
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
