using ReadyPlayerMe.Core;
using UnityEngine;
using UnityEngine.UI;
public class TestAvatarLoader : MonoBehaviour
{
    [SerializeField] Transform TPoseTrans;
    private GameObject m_Root;
    // Add 2 editable fields to store the new masculine and feminine animator controllers
    [SerializeField] private RuntimeAnimatorController masculineController;
    [SerializeField] private RuntimeAnimatorController feminineController;
    [SerializeField] Dropdown leftDropdown;
    [SerializeField] Dropdown rightDropdown;
    [SerializeField] bool modifyBasePose = false;
    [SerializeField] bool drivenByAni = false;

    Animator m_RootAnimator;
    Animator TPoseAnimator;

    private int index=0;
    private void Start()
    {
        TPoseAnimator = TPoseTrans.GetComponent<Animator>();
        TPoseAnimator.enabled = false;

        ApplicationData.Log();
        var avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += (_, args) =>
        {
            OnAvatarLoaderCompleted(args);
        };

        string avatarUrl = "https://api.readyplayer.me/v1/avatars/638df693d72bffc6fa17943c.glb";
        avatarLoader.LoadAvatar(avatarUrl);
    }

    public void OnAvatarLoaderCompleted(CompletionEventArgs args)
    {
        m_Root = args.Avatar;
        Animator animator = m_Root.GetComponent<Animator>();
        DestroyImmediate(animator);
        ModifyBasePose(args);
        BindDynamicAvatar(m_Root);
        if (drivenByAni)
            SetAnimatorController(args.Metadata.OutfitGender); //  <--------------- ADDED

    }

    private void ModifyBasePose(CompletionEventArgs args)
    {
        args.Avatar.transform.position = new Vector3(-0.5f,0,0);
        if (!modifyBasePose)
            return;
        Transform LeftShoulder = args.Avatar.transform.Find("Armature/Hips/Spine/Spine1/Spine2/LeftShoulder");
        Transform LeftArm = args.Avatar.transform.Find("Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm");
        Transform LeftForeArm = args.Avatar.transform.Find("Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm");

        Transform RightShoulder = args.Avatar.transform.Find("Armature/Hips/Spine/Spine1/Spine2/RightShoulder");
        Transform RightArm = args.Avatar.transform.Find("Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm");
        Transform RightForeArm = args.Avatar.transform.Find("Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm");

        Transform TLeftShoulder = TPoseTrans.Find("Armature/Hips/Spine/Spine1/Spine2/LeftShoulder");
        Transform TLeftArm = TPoseTrans.Find("Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm");
        Transform TLeftForeArm = TPoseTrans.Find("Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm");


        Transform TRightShoulder = TPoseTrans.Find("Armature/Hips/Spine/Spine1/Spine2/RightShoulder");
        Transform TRightArm = TPoseTrans.Find("Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm");
        Transform TRightForeArm = TPoseTrans.Find("Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm");
        if (args.Metadata.OutfitGender == OutfitGender.Masculine)
        {


            LeftShoulder.localPosition = TLeftShoulder.localPosition;
            LeftShoulder.localRotation = TLeftShoulder.localRotation;
            LeftArm.localPosition = TLeftArm.localPosition;
            LeftArm.localRotation = TLeftArm.localRotation;
            LeftForeArm.localPosition = TLeftForeArm.localPosition;
            LeftForeArm.localRotation = TLeftForeArm.localRotation;

            RightShoulder.localPosition = TRightShoulder.localPosition;
            RightShoulder.localRotation = TRightShoulder.localRotation;
            RightArm.localPosition = TRightArm.localPosition;
            RightArm.localRotation = TRightArm.localRotation;
            RightForeArm.localPosition = TRightForeArm.localPosition;
            RightForeArm.localRotation = TRightForeArm.localRotation;

            //LeftShoulder.localRotation = Quaternion.Euler(100f, -94f, -3f);
            //LeftArm.localRotation = Quaternion.Euler(-10f, 22.6f, -5.57f);
            //RightShoulder.localRotation = Quaternion.Euler(100, 94f, 2.76f);
            //RightArm.localRotation = Quaternion.Euler(-10f, -22.6f, 4.5f);
        }
        else
        {
            LeftShoulder.localRotation = Quaternion.Euler(100, -90, 11);
            LeftArm.localRotation = Quaternion.Euler(-8, -2f, -12);
            RightShoulder.localRotation = Quaternion.Euler(100, 90, -11);
            RightArm.localRotation = Quaternion.Euler(-8, 2f, 12f);
        }

    }

    private void BindDynamicAvatar(GameObject root)
    {
        DynamicAvatar dynamicAvatar =root.AddComponent<DynamicAvatar>();
        Animator animator = root.AddComponent<Animator>();
        animator.applyRootMotion = true;
        animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        dynamicAvatar.Init(root, animator);
    }
    // This method is used to reassign the appropriate animator controller based on outfit gender
    private void SetAnimatorController(OutfitGender outfitGender)
    {
        m_RootAnimator = m_Root.GetComponent<Animator>();

        // set the correct animator based on outfit gender
        if (m_RootAnimator != null && outfitGender == OutfitGender.Masculine)
        {
            m_RootAnimator.runtimeAnimatorController = masculineController;
        }
        else
        {
            m_RootAnimator.runtimeAnimatorController = feminineController;
        }
        InitUI(rightDropdown);

        //TPoseAnimator= TPoseTrans.GetComponent<Animator>();
        //InitUI(TPoseAnimator, leftDropdown);
        //TPoseAnimator.enabled = true;
    }

    private void OnDestroy()
    {
        if (m_Root != null) Destroy(m_Root);
    }

    void InitUI(Dropdown dropdown)
    {
        if (m_RootAnimator == null|| m_RootAnimator.runtimeAnimatorController == null)
            return;
        dropdown.ClearOptions();
        for (int i = 0; i < m_RootAnimator.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (m_RootAnimator.runtimeAnimatorController.animationClips[i].name.ToLower().Contains("idle"))
                continue;
            dropdown.options.Add(new Dropdown.OptionData(m_RootAnimator.runtimeAnimatorController.animationClips[i].name));
        }
        dropdown.value = index;
        dropdown.RefreshShownValue();
        dropdown.onValueChanged.AddListener( (int index) =>
        {
            m_RootAnimator.SetTrigger(dropdown.options[index].text);
            TPoseAnimator.SetTrigger(dropdown.options[index].text);

        });

        TPoseAnimator.enabled = true;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index++;
            if (index >= rightDropdown.options.Count)
                index = 0;
            rightDropdown.value = index;
            rightDropdown.RefreshShownValue();

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index--;
            if (index < 0)
                index = rightDropdown.options.Count - 1;
            rightDropdown.value = index;
            rightDropdown.RefreshShownValue();

        }
    }
}