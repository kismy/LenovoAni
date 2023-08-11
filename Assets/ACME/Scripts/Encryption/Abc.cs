using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Abc : MonoBehaviour
{
    //public static GameObject mygameobj;

	void Start ()
    {
        Debug.Log(" Abc  Start");
	}
	void Update ()
    {
        Debug.Log(" Abc  Update");
    }

    //脚本不挂载到游戏对象执行
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void DoSomething()
    {
        //LZQ
        //Debug.Log("It's the start of the game");
    }
    /// <summary>
    /// 切换场景   End
    /// </summary>
    public static void ChangeScene()
    {
        Debug.Log(" ChangeScene  End ");
        SceneManager.LoadScene("End");
    }
    /// <summary>
    /// 切换场景  SetIp  
    /// </summary>
    public static void ChangScenetwo()
    {
        Debug.Log(" ChangScenetwo    SetIp ");
        SceneManager.LoadScene("SetIp");
    }
    /// <summary>
    /// 代码 添加 脚本
    /// </summary>
    public static void AddRegistryjss()
    {
        Debug.Log(" ABC tianjia");
        GameObject go = GameObject.FindGameObjectWithTag("MainCamera");
        go.AddComponent(typeof(Registry));
    }


}
