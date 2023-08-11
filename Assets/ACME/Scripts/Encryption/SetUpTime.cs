using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Win32;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetUpTime : MonoBehaviour
{
    private static SetUpTime instance;
    public static SetUpTime Instance
    {
        get { return instance; }
    }
    //11111
    TimeSpan days;
    string timing;
    int timings, timespans;
    RegistryKey regkeySetKey;
    public InputField InputField;
    public Text message_text;
    public void Start()
    {
        instance = this;
    }
    public void sure()
    {
        MyBase64.Base64Decode(InputField.text);
       try
            {
                string DateText = (MyBase64.Base64Decode(InputField.text)).Substring(4);
                string Year = DateText.Substring(0, 4);
                string Month = DateText.Substring(4, 2);
                string Days = DateText.Substring(6, 2);
                subtractNum(Year, Month, Days);
            }
            catch (Exception)
            {
                message_text.text = "输入有误，请重新输入 No002";
            }
    }

    private void subtractNum(string year,string month,string day)
    {
        //string[] dates = dateNew.Split('-');
        DateTime departure = DateTime.Now;
        DateTime arrival = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
        DateTime start = Convert.ToDateTime(departure.ToShortDateString());
        DateTime end = Convert.ToDateTime(arrival.ToShortDateString());
        TimeSpan days = end.Subtract(start);
        string dateNew = year + "-" + month + "-" + day;
        //Debug.Log(dateNew);
        RegistryChange(dateNew, (days.Days).ToString());
    }
    //修改键值
    private void RegistryChange(string Ndate, string Nday)
    {
        RegistryKey regkeySetKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows", true).CreateSubKey("LZQ"); //创建了一个test_test子键并有三个子健
        regkeySetKey.SetValue("ByTheTime", Ndate);
        regkeySetKey.SetValue("Timing", Nday);//如果没有也可以直接增加键值
        //LZQ
        //Debug.Log(Ndate + "   " + Nday + "天");
        message_text.text = "输入成功，请重启";
        //SceneManager.LoadScene(0);
        StartCoroutine(LoadMainScene());
    }
    IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(0);
    }
    public void sureButton(String X)
    {
        MyBase64.Base64Decode(X);
        try
        {
            string DateText = (MyBase64.Base64Decode(X)).Substring(4);
            string Year = DateText.Substring(0, 4);
            string Month = DateText.Substring(4, 2);
            string Days = DateText.Substring(6, 2);
            //lZQ
            //Debug.Log(Year + " " + Month + " " + Days);
            subtractNum(Year, Month, Days);
        }
        catch (Exception)
        {
            message_text.text = "输入有误，请重新输入 No001";
        }
    }

}
