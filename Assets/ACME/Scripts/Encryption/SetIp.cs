using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetIp : MonoBehaviour
{
    //                本地IP显示框       本地端口显示框        对方IP显示框      对方端口显示框
    public InputField LocalIpInputText, LocalDuanKouInputText, OtherIpInputText, OtherDuanKouInputText;
    //          本地 旁边          对方旁边
    public Text LocalMessageText, OtherMessageText;

    void Start()
    {
        //先 读取 IP和端口   之后写入IP 和端口
        ReadLocaiip();
        ReadOtherIP();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene("End");
        }
    }
    /// <summary>
    /// 读取txt  本地IP 端口
    /// </summary>
    public void ReadLocaiip()
    {
        //File.ReadAllText(Application.streamingAssetsPath + "\\LocalIP.txt"); 0 ip  1  端口
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "\\IP\\IPLocal.txt");
        string[] strs = sr.ReadToEnd().Split(';');
        LocalIpInputText.text = strs[0];
        LocalDuanKouInputText.text = strs[1];
        sr.Close();
    }
    /// <summary>
    /// 写入 本地IP 端口
    /// </summary>
    public void SetLocalIP()
    {
        File.WriteAllText(@Application.streamingAssetsPath + "\\IP\\IPLocal.txt", LocalIpInputText.text.ToString() + ";" + LocalDuanKouInputText.text.ToString());
        LocalMessageText.text = "设置成功";
    }
    /// <summary>
    /// 读取 txt  对方IP  端口
    /// </summary>
    public void ReadOtherIP()
    {
        //File.ReadAllText(Application.streamingAssetsPath + "\\LocalIP.txt"); 0 ip  1  端口
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "\\IP\\IPRemote.txt");
        string[] strs = sr.ReadToEnd().Split(';');
        OtherIpInputText.text = strs[0];
        OtherDuanKouInputText.text = strs[1];
        sr.Close();
    }
    /// <summary>
    /// 写入  对方IP 端口
    /// </summary>
    public void SetOtherIP()
    {
        File.WriteAllText(@Application.streamingAssetsPath + "\\IP\\IPRemote.txt", OtherIpInputText.text.ToString() + ";" + OtherDuanKouInputText.text.ToString());
        OtherMessageText.text = "设置成功";
    }


}
