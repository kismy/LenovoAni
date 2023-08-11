using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.SceneManagement;
using System;

public class MyBase64 : MonoBehaviour {

    // Use this for initialization
    void Start () {
        ////    http://www.manew.com/thread-115940-1-1.html
        Base64Encode("edit20190605");
        Base64Decode("ZWRpdDIwMTkwNjMw");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {     
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("SetIp");
        }
    }

    /// <summary>  
    /// Base64编码  
    /// </summary>  
    public static string Base64Encode(string message)
    {
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
        return Convert.ToBase64String(bytes);
    }
    /// <summary>  
    /// Base64解码  
    /// </summary>  
    public static string Base64Decode(string message)
    {
        byte[] bytes = Convert.FromBase64String(message);
        return Encoding.GetEncoding("utf-8").GetString(bytes);
    }
}
