using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;

public class BuildApp
{
    private static DateTime now;
    private static string m_SharePath = "D:\\ShareFiles\\Signature\\";
    private static string m_AppName = PlayerSettings.productName;//RealConfig.GetRealFram().m_AppName;
    private static string dir;
 

    [MenuItem("ACME/Build PC包")]
    public static void BuildPC()
    {

        m_SharePath = QuickConfig.instance.JKSharePath;
        now = DateTime.Now;
        BuildPC_WithResolutionDialog();
        BuildPC_WithoutResolutionDialog();
        HandleReadMe();
        WriteBuildName(dir);

    }
    public static void BuildPC_WithResolutionDialog()
    {
        BuildSetting buildSetting = GetBuildSetting();
        buildSetting.DisPlayResolutionDialog = UnityEditor.ResolutionDialogSetting.Enabled;
        SetBuildSetting(buildSetting);

        //生成可执行程序
        dir = m_AppName + "/"+ string.Format("{0:yyyyMMddHHmm}", now);
      
        string savePath = m_SharePath + dir + "/EnabledDialog";

        savePath += string.Format("/{0}.exe", m_AppName);
        BuildPipeline.BuildPlayer(FindEnableEditorrScenes(), savePath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);


    }
    public static void BuildPC_WithoutResolutionDialog()
    {
        BuildSetting buildSetting = GetBuildSetting();
        buildSetting.DisPlayResolutionDialog = UnityEditor.ResolutionDialogSetting.Disabled;
        SetBuildSetting(buildSetting);

        //生成可执行程序
        dir = m_AppName + "/"  + string.Format("{0:yyyyMMddHHmm}", now);
        string savePath = m_SharePath + dir+ "/WithoutDialog";
        savePath += string.Format("/{0}.exe", m_AppName);
        BuildPipeline.BuildPlayer(FindEnableEditorrScenes(), savePath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);

    }

    private static string[] FindEnableEditorrScenes()
    {
        List<string> editorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            editorScenes.Add(scene.path);
        }
        return editorScenes.ToArray();
    }
    public static void WriteBuildName(string name)
    {
        FileInfo fileInfo = new FileInfo(Application.dataPath + "/../buildname.txt");
        StreamWriter sw = fileInfo.CreateText();
        sw.WriteLine(name);
        sw.Close();
        sw.Dispose();
    }
    public static void HandleReadMe()
    {
        if (!File.Exists(Application.dataPath + "/ReadMe.txt"))
            using (File.Create(Application.dataPath + "/ReadMe.txt"))
        if (!Directory.Exists(m_SharePath + dir))
                    Directory.CreateDirectory(m_SharePath + dir);
       
        try {
            File.Copy(Application.dataPath + "/ReadMe.txt", m_SharePath + dir + "/ReadMe.txt", true);
        }catch(Exception e)
        {
            Debug.Log(e.ToString());

        }
    }
    static BuildSetting GetBuildSetting()
    {
        string[] parameters = Environment.GetCommandLineArgs();
        BuildSetting buildSetting = new BuildSetting();
        foreach (var str in parameters)
        {
            if (str.StartsWith("Name"))
            {
                string[] tempStrs = str.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (tempStrs.Length == 2) buildSetting.Name = tempStrs[1].Trim();
            }
            else if (str.StartsWith("FullScreenMode"))
            {
                string[] tempStrs = str.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (tempStrs.Length == 2) buildSetting.FullScreenMode = (FullScreenMode)Enum.Parse(typeof(FullScreenMode), tempStrs[1], true);
            }
            else if (str.StartsWith("DisPlayResolutionDialog"))
            {
                string[] tempStrs = str.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (tempStrs.Length == 2) buildSetting.DisPlayResolutionDialog = (UnityEditor.ResolutionDialogSetting)Enum.Parse(typeof(UnityEditor.ResolutionDialogSetting), tempStrs[1], true);
            }
            else if (str.StartsWith("ScreenWidth"))
            {
                string[] tempStrs = str.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (tempStrs.Length == 2) buildSetting.ScreenWidth = int.Parse(tempStrs[1].Trim());
            }
            else if (str.StartsWith("ScreenHeight"))
            {
                string[] tempStrs = str.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (tempStrs.Length == 2) buildSetting.ScreenHeight = int.Parse(tempStrs[1].Trim());
            }
            else if (str.StartsWith("DefaultIsNativeResolution"))
            {
                string[] tempStrs = str.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (tempStrs.Length == 2) bool.TryParse(tempStrs[1].Trim(), out buildSetting.DefualtNativeResolution);
            }
        }
        return buildSetting;
    }
    static void SetBuildSetting(BuildSetting buildSetting)
    {
        PlayerSettings.productName = buildSetting.Name;
        PlayerSettings.fullScreenMode = buildSetting.FullScreenMode;
        PlayerSettings.displayResolutionDialog = buildSetting.DisPlayResolutionDialog;
        PlayerSettings.defaultIsNativeResolution = buildSetting.DefualtNativeResolution;
        PlayerSettings.defaultScreenWidth = buildSetting.ScreenWidth;
        PlayerSettings.defaultScreenHeight = buildSetting.ScreenHeight;
    }
}



public class BuildSetting
{
    public string Name;
    public UnityEngine.FullScreenMode FullScreenMode;
    public UnityEditor.ResolutionDialogSetting DisPlayResolutionDialog;
    public bool DefualtNativeResolution;
    public int ScreenWidth;
    public int ScreenHeight;
    public override string ToString()
    {
        string outPut = "APPName=" + Name +" || FullScreenMode = " + FullScreenMode + " || DisPlayResolutionDialog=" + DisPlayResolutionDialog  +
                        " || DefualtNativeResolution=" + DefualtNativeResolution + " || ScreenWidth=" + ScreenWidth + "ScreenHeight=" + ScreenHeight;
        return outPut;
    }
}




