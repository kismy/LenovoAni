using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class XMLConfig
{

#if UNITY_EDITOR
    [UnityEditor.MenuItem("ACME/创建QuickConfig")]
    public static void Create()
    {
        if (!Directory.Exists(Application.dataPath + "/StreamingAssets/TOOLS"))
            Directory.CreateDirectory(Application.dataPath + "/StreamingAssets/TOOLS");

        if (!File.Exists(Application.dataPath + "/StreamingAssets/TOOLS/QuickConfig.xml"))
        {
            FileInfo fileInfo = new FileInfo(Application.dataPath + "/StreamingAssets/TOOLS/QuickConfig.xml");

            using (StreamWriter sw = new StreamWriter(fileInfo.Create()))
            {
                QuickConfig config = new QuickConfig() { PngSharePath = @"D:\ShareFiles\", JKSharePath = @"D:\ShareFiles\" };
                string xml = XmlUtil.Serializer(typeof(QuickConfig), config);
                sw.Write(xml);
            }

        }
        UnityEditor.AssetDatabase.Refresh();

    }

    [UnityEditor.MenuItem("ACME/创建带水印_配置文件")]
    public static void CreatePatcher1()
    {
        CreatePatcher(false);
    }
    [UnityEditor.MenuItem("ACME/创建去除水印_配置文件")]
    public static void CreatePatcher2()
    {
        CreatePatcher(true);
    }
    static void CreatePatcher(bool showLogo)
    {

        QuickConfig config = QuickConfig.Load();
        config.UnityPatcher = showLogo;
        byte[] bytes = BinarySerialize.Object2Bytes(config);

        Stream stream = null;
        if (!File.Exists(Application.streamingAssetsPath + "/TOOLS/UnityPatcher.Dll"))
        {
            stream = File.Create(Application.streamingAssetsPath + "/TOOLS/UnityPatcher.Dll");

        }
        else
            stream = (new StreamWriter(Application.streamingAssetsPath + "/TOOLS/UnityPatcher.Dll", false)).BaseStream;
        BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.ASCII);
        binaryWriter.Write(bytes);

        binaryWriter.Close();
        stream.Close();
        stream.Dispose();
        UnityEditor.AssetDatabase.Refresh();
    }
#endif



}

