using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using QTween;
using System.Linq;


public class SkeletonModify : MonoBehaviour
{
    [SerializeField] string currentAniState;
    [SerializeField] Transform joint;
    [SerializeField] Text text;
    [SerializeField]
    SkeletonModifyData m_SkeletonModifyData;
    [SerializeField] List<Transform> OffsetJoints;
    [SerializeField] Animator animator;
    [Range(1,10)]
    [SerializeField] float speed = 5f;
    [SerializeField]
    private string configFileName;

    public void Init(string configFileName, Transform joint,Text text,Animator animator)
    {
        this.configFileName = configFileName;
        this.joint = joint;
        this.text = text;
        this.animator = animator;
        OffsetJoints = new List<Transform>();

    }
    void Start()
    {
        QTween.Extension.mono = this;
        m_SkeletonModifyData = new SkeletonModifyData() { roleName=gameObject.name,modifies=new List<ModifyData>()};
        CreateOffsetJoints_OnRuntime();
        animator.SetTrigger("OnIdleOfSitDown");
        text.text = "当前动画：IdleOfSitDown";
        LoadCurAnimationModify("IdleOfSitDown");
    }


    void CreateOffsetJoints_OnRuntime()
    {
        OffsetJoints.Clear();

        string[] lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "SkeletonModifyData", configFileName + ".joints"));
        List<Transform> targets = new List<Transform>();
        foreach (var line in lines)
        {
            string[] strs = line.Split('=');
            if (strs == null || strs.Length != 2) continue;

            Transform target = transform.Find(strs[1]);
            targets.Add(target);
        }
        foreach (var target in targets)
        {
            Transform _Offset = GameObject.Instantiate<Transform>(joint, target.parent);
            _Offset.name = target.name + "_Offset";
            _Offset.localPosition = Vector3.zero;
            _Offset.localRotation = Quaternion.identity;
            target.parent = _Offset;
            OffsetJoints.Add(_Offset);
        }
    }


    public void SaveCurAnimationModify()
    {
        m_SkeletonModifyData.modifies.Clear();
        foreach (var item in OffsetJoints)
        {
            m_SkeletonModifyData.modifies.Add(new ModifyData()
            {
                ModifySkeleton = item.name,
                LocalPos = item.localPosition,
                LocalRotation = item.localRotation.eulerAngles

            });
        }
        SaveJson();

    }

    public void LoadCurAnimationModify(string stateName=null)
    {
       
        if (stateName == null)
            stateName = currentAniState;
        else if (stateName == currentAniState)
            return;
        SkeletonModifyData tempData = LoadJson(stateName);
        if (tempData == null)
        {
            m_SkeletonModifyData.aniStateName = stateName;
            m_SkeletonModifyData.modifies.Clear();
        }
        else
            m_SkeletonModifyData = tempData;

        if (m_SkeletonModifyData != null)
        {
            foreach (var item in OffsetJoints)
            {
                ResetOffset(item, m_SkeletonModifyData.modifies);
            }
        }

    }

    public void ClearCurAnimationModify()
    {
        foreach (var item in OffsetJoints)
        {
            item.localPosition = Vector3.zero;
            item.localRotation = Quaternion.identity;
        }
    }

    private void ResetOffset(Transform target, List<ModifyData> modifies)
    {
       ModifyData data= modifies.Find(item => {
            return item.ModifySkeleton== target.name;
        });

        if (data != null)
        {
            //target.localPosition = data.LocalPos;
            //target.localRotation = Quaternion.Euler(data.LocalRotation);

            target.DoLocalPosition(data.LocalPos);
            target.DoLocalRotation(data.LocalRotation, speed);
        }
        else
        {
            //target.localPosition = Vector3.zero;
            //target.localRotation = Quaternion.identity;

            target.DoLocalPosition(Vector3.zero);
            target.DoLocalRotation(Vector3.zero, speed);
        }
    }

    public void SaveJson()
    {
        string savePath = Path.Combine(Application.streamingAssetsPath, "SkeletonModifyData", configFileName);
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        m_SkeletonModifyData.aniStateName = currentAniState;

        string fileFullPath = Path.Combine(savePath, currentAniState + ".json");
        //string fileFullPath = Path.Combine(savePath, "IdleOfSitDown.json");
        File.WriteAllText(fileFullPath, JsonUtility.ToJson(m_SkeletonModifyData));
        Debug.Log("Save Success:"+ fileFullPath);
    }

    private SkeletonModifyData LoadJson(string StateName)
    {
        AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(0);

        string fileFullPath = string.Format(Application.streamingAssetsPath+"/{0}/{1}/{2}", "SkeletonModifyData", configFileName, StateName+".json");
        if (!File.Exists(fileFullPath))
        {
            Debug.LogWarning("File Not Find:"+ fileFullPath);
            return null;

        }

        string Json= File.ReadAllText(fileFullPath);
        return JsonUtility.FromJson<SkeletonModifyData>(Json);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            LoadAni("IdleOfSitDown");
        else if (Input.GetKeyDown(KeyCode.F2))
            LoadAni("ThumbsUp");
        else if (Input.GetKeyDown(KeyCode.F3))
            LoadAni("HandClap"); 
        else if (Input.GetKeyDown(KeyCode.F4))
            LoadAni("Sign");
        else if (Input.GetKeyDown(KeyCode.F5))
            LoadAni("Cheer");
        else if (Input.GetKeyDown(KeyCode.F6))
            LoadAni("Pray");
        else if (Input.GetKeyDown(KeyCode.F7))
            LoadAni("OK");
        else if (Input.GetKeyDown(KeyCode.F8))
            LoadAni("Wave");
        else if (Input.GetKeyDown(KeyCode.F9))
            LoadAni("Think");
        else if (Input.GetKeyDown(KeyCode.F10))
            LoadAni("Yean");
        else if (Input.GetKeyDown(KeyCode.F11))
            LoadAni("Yes");
        else if (Input.GetKeyDown(KeyCode.F12))
            LoadAni("ThumbsDown");
    }

    private void LoadAni(string aniState)
    {
        animator.SetTrigger("On"+ aniState);
        text.text = "当前动画：" + aniState;
        LoadCurAnimationModify(aniState);
        currentAniState = aniState;
    }
}
