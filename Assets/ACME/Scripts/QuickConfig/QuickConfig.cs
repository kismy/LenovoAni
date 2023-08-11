using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System;
using System.Text;

[Serializable]
[XmlRoot("Root")]
public class QuickConfig {
    private static QuickConfig m_Instance;
    public static QuickConfig instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = Load();
            return m_Instance;
        }
        set { m_Instance = value; }
    }

    [XmlElement("LocalIP")]
    public string LocalIP = "127.0.0.1";
    [XmlElement("LocalPort")]
    public int LocalPort = 8001;
    [XmlElement("RemoteIP")]
    public string RemoteIP = "127.0.0.1";
    [XmlElement("RemotePort")]
    public int RemotePort = 8888;

    [XmlElement("PngSharePath")]
    public string PngSharePath;
    [XmlElement("JKSharePath")]
    public string JKSharePath;

    [XmlElement("Debug")]
    public bool Debug = false;
    [XmlElement("ShowCursor")]
    public bool ShowCursor = false;
    [XmlElement("UnityPatcher")] //用于创建UnityPatcher.dll的二进制文件,程序运行后依据该二进制文件值控制LOGO水印
    public bool UnityPatcher = false;
    [XmlElement("ShowStartImage")]
    public bool ShowStartImage = false;


    [XmlElement("ScreenControl")]
    public bool ScreenControl = false;
    [XmlElement("ScreenWidth")]
    public int ScreenWidth = 1920;
    [XmlElement("ScreenHeight")]
    public int ScreenHeight = 1080;


    private static QuickConfig Create()
    {
        QuickConfig config = null;
        if (!Directory.Exists(Application.dataPath + "/StreamingAssets/TOOLS"))
            Directory.CreateDirectory(Application.dataPath + "/StreamingAssets/TOOLS");

        if (!File.Exists(Application.dataPath + "/StreamingAssets/TOOLS/QuickConfig.xml"))
        {
            FileInfo fileInfo = new FileInfo(Application.dataPath + "/StreamingAssets/TOOLS/QuickConfig.xml");

            using (StreamWriter sw = new StreamWriter(fileInfo.Create()))
            {
                config = new QuickConfig() { PngSharePath = @"D:\ShareFiles\", JKSharePath = @"D:\ShareFiles\" };
                string xml = XmlUtil.Serializer(typeof(QuickConfig), config);
                sw.Write(xml);
            }
        }
        return config;
    }
    public static  QuickConfig Load()
    {
        if (!File.Exists(Application.dataPath + "/StreamingAssets/TOOLS/QuickConfig.xml"))
        {
          return Create();
        }

        if (File.Exists(Application.dataPath + "/StreamingAssets/TOOLS/QuickConfig.xml"))
        {
            FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/TOOLS/QuickConfig.xml", FileMode.Open, FileAccess.Read);
            using (stream)
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string xml = sr.ReadToEnd();
                    return (QuickConfig)XmlUtil.Deserialize(typeof(QuickConfig), xml);
                }
            }
        }
        return null;
    }

    public static QuickConfig LoadFromUnityPatcher()
    {
        if (!File.Exists(Application.streamingAssetsPath + "/TOOLS/UnityPatcher.Dll"))
        {
            UnityEngine.Debug.LogError("StreamingAssets/TOOLS/UnityPatcher.Dll 不存在！");
            return null;
        }

        Stream stream = (new StreamReader(Application.streamingAssetsPath + "/TOOLS/UnityPatcher.Dll", false)).BaseStream;
        BinaryReader br = new BinaryReader(stream, Encoding.ASCII);
        byte[] bytes = br.ReadBytes((int)stream.Length);
        object o = BinarySerialize.Bytes2Object(bytes);

        QuickConfig config = null;
        try
        {
            config = o as QuickConfig;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("bytes 2 QuickConfig错误！"+e.ToString());
            return null;

        }

        br.Close();
        stream.Close();
        stream.Dispose();

        return config;

    }

}

