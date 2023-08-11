using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Win32;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Registry : MonoBehaviour
{
    string MySetDate;
    string ByTheTime;
    string writenTime; //记录未超过到期时间，且在使用中记录的最接近到期时间的值
    string timing;
    //TimeSpan timespan;
    TimeSpan days;
    int timings, timespans;
    RegistryKey regkeySetKey;
    // Use this for initialization
    bool goyn = true;
    [SerializeField] CanvasGroup canvas;


    void Start()
    {
        StartCoroutine(ShowJieMiHint());
        RegistryRead();
        if (goyn)
        {
           
            if (AlreadyTimeOut())
            {
                SceneManager.LoadScene("End");
            }
            else
            {
                RegistryCreate();
            }
        }
    }





    private void RegistryCreate()
    {
        //RegistryKey regkeySetKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true).CreateSubKey("test_test"); //创建了一个test_test子键并有三个子健
        regkeySetKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows", true).CreateSubKey("LZQ"); //创建了一个test_test子键并有三个子健
        regkeySetKey.SetValue("Timing", (days.Days).ToString());
    }

    //读取键值
    private void RegistryRead()
    {
        //RegistryKey regkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
        //RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
        regkeySetKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows", true).CreateSubKey("LZQ");
        string[] n = regkeySetKey.GetValueNames();
        for (int i = 0; i < n.Length; i++)
        {
            //Response.Write(n[i]+": "+regkey.GetValue(n[i])+"<br >"); 
            //Debug.Log(n[i] + ": " + regkey.GetValue(n[i]) + "\t\n");
            //Debug.Log(n[i] + ": " + regkey.GetValue(n[i]));
        }

        //参考网址 https://blog.csdn.net/cslp517/article/details/78284177
        if (regkeySetKey.GetValue("ByTheTime") != null)
        {
            ByTheTime = regkeySetKey.GetValue("ByTheTime").ToString();
        }
        else
        {
            regkeySetKey.SetValue("ByTheTime", "1970-01-01");

            string writenTime = DateTime.Now.ToString("yyyy-MM-dd");
            regkeySetKey.SetValue("writenTime", writenTime);

            ByTheTime = "";
        }

        if (regkeySetKey.GetValue("writenTime") != null)
        {
            writenTime = regkeySetKey.GetValue("writenTime").ToString();
            DateTime dateTime = GetTime(writenTime);
            if (dateTime < DateTime.Now)
            {
                writenTime = DateTime.Now.ToString("yyyy-MM-dd");
                regkeySetKey.SetValue("writenTime", writenTime);
            }
        }
        else
        {
            writenTime = DateTime.Now.ToString("yyyy-MM-dd");
            regkeySetKey.SetValue("writenTime", writenTime);
            writenTime = "";
        }



        if (regkeySetKey.GetValue("Timing") != null)
        {
            timing = regkeySetKey.GetValue("Timing").ToString();
        }
        else
        {
            regkeySetKey.SetValue("Timing", "0");
            timing = "";
        }
        //LZQ
        //Debug.Log("ByTheTime: " + ByTheTime + "timing: " + timing);
        if (ByTheTime == "" || timing == "" || writenTime == "")//获得的那个值和减去的值相比
        {
            goyn = false;
            SceneManager.LoadScene("End");
        }
    }

    IEnumerator ShowJieMiHint()
    {
        canvas.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        while (canvas.alpha > 0)
        {
            canvas.alpha -= Time.deltaTime;
            yield return null;
        }
        canvas.gameObject.SetActive(false);
        yield break;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str">format 2019-12-31</param>
    /// <returns></returns>
    DateTime GetTime(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return new DateTime(0, 0, 0);
        }

        string[] strs = str.Split('-');
        if (strs.Length >= 3)
        {
            int yyyy = int.Parse(strs[0]);
            int MM = int.Parse(strs[1]);
            int dd = int.Parse(strs[2]);

            return new DateTime(yyyy, MM, dd);
        }

        return new DateTime(0, 0, 0);
    }

    bool AlreadyTimeOut()
    {
        MySetDate = ByTheTime;
        DateTime arrival = GetTime(MySetDate);
        days = arrival.Subtract(DateTime.Now);

        timings = Convert.ToInt32(timing);
        timespans = Convert.ToInt32(days.Days);


        if (timings < timespans || timespans <= 0 ||GetTime(writenTime)>DateTime.Today)
            return true;
        return false;
    }
}
