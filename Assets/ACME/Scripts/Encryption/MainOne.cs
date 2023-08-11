using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Reflection;

public class MainOne : MonoBehaviour
{
	void Start ()
    {
        
    }
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Abc.ChangeScene();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Abc.ChangScenetwo();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(0);
        }
    }

    /// <summary>
    /// 代码 添加 脚本
    /// </summary>
    public static void AddSpreit()
    {
        Debug.Log("tianjia");
        //mygameobj = GameObject.Find("Main Cameraone");
        //mygameobj.AddComponent<Mainone>();
        GameObject go = GameObject.FindGameObjectWithTag("JSS");
        go.AddComponent(typeof(MainOne));
    }
    /// <summary>
    /// 代码 添加 脚本 er er 
    /// </summary>
    public static void AddRegister()
    {
        Debug.Log("tianjia");
        //mygameobj = GameObject.Find("Main Cameraone");
        //mygameobj.AddComponent<Mainone>();
        GameObject go = GameObject.FindGameObjectWithTag("JSS");
        go.AddComponent(typeof(Registry));
    }


}
