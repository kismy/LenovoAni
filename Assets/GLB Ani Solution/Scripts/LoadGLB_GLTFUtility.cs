using Siccity.GLTFUtility;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// 加载GLB/GLTF模型
/// </summary>
public class LoadGLB_GLTFUtility : MonoBehaviour
{
    Action<GameObject,AnimationClip[]> OnFinish;
    public GameObject ObjOfImport;
    bool isCreate = true;
    private static LoadGLB_GLTFUtility instance;
    public static LoadGLB_GLTFUtility Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "LoadGLBUtility";
                instance = obj.AddComponent<LoadGLB_GLTFUtility>();

            }
            return instance;
        }
    }

    /// <summary>
    /// 导入网络glb资源
    /// </summary>
    /// <param name="url">模型路径</param>
    /// <param name="action">导入完成事件</param>
    public void ImportGLB_GLTFAsyncByWed(string url, Action<GameObject, AnimationClip[]> OnFinishAsync)
    {
        StartCoroutine(DownLoadFile(url, OnFinishAsync));

    }
    /// <summary>
    /// 导入网络glb资源
    /// </summary>
    /// <param name="url">路径</param>
    public void ImportGLBAsyncByWed(string url)
    {
        StartCoroutine(DownLoadFile(url, null));

    }

    /// <summary>
    /// 下载网络资源到本地
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator DownLoadFile(string url, Action<GameObject, AnimationClip[]> OnFinishAsync)
    {
        yield return new WaitForSeconds(0.5f);
        string directoryPath = Application.persistentDataPath + "/FileCache";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        string downloadFileName = url.Substring(url.LastIndexOf('/') + 1);
        string localpath = directoryPath + "/" + downloadFileName;
        Debug.Log(downloadFileName);
        //如果本地文件已存在 直接加载
        if (File.Exists(localpath))
        {
            ImportGLB_GLTFAsync(localpath, OnFinishAsync);
            yield break;
        }
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.timeout = 60;
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError)
        {
            Debug.Log("Download Error: " + webRequest.error);
            if (File.Exists(localpath))
            {
                File.Delete(localpath);
            }
        }
        else
        {
            var file = webRequest.downloadHandler.data;
            FileStream nFile = new FileStream(localpath, FileMode.Create);
            nFile.Write(file, 0, file.Length);
            nFile.Close();
            ImportGLB_GLTFAsync(localpath, OnFinishAsync);
        }
    }
    /// <summary>
    /// 异步加载 gltf and glb
    /// </summary>
    /// <param name="filepath">路径</param>
    /// <param name="action">回调函数</param>
    public void ImportGLB_GLTFAsync(string filepath, Action<GameObject, AnimationClip[]> OnFinishAsync)
    {
        if (!isCreate) return;
        isCreate = false;
        UnloadAndDestroy(ObjOfImport);
        Importer.LoadFromFileAsync(filepath, new ImportSettings(), this.OnFinishAsync);
        this.OnFinish = OnFinishAsync;
    }

    /// <summary>
    ///异步加载模型
    /// </summary>
    /// <param name="filepath"><路径/param>
    public void ImportGLB_GLTFAsync(string filepath)
    {

        if (!isCreate) return;

        isCreate = false;

        UnloadAndDestroy(ObjOfImport);

        Importer.LoadFromFileAsync(filepath, new ImportSettings(), OnFinish);
    }






    /// <summary>
    /// 删除卸载 所有加载过的模型 清除缓存
    /// </summary>
    /// <param name="obj"></param>
    public void UnloadAndDestroy(GameObject obj)
    {
        if (obj != null)
        {
            GameObject.Destroy(obj);
        }
        Resources.UnloadUnusedAssets();

    }
    /// <summary>
    /// 完成加载
    /// </summary>
    /// <param name="result">加载出来的物体</param>
    /// <param name="clip">加载的动画</param>
    /// <param name="transform"></param>
    /// <param name="name"></param>
    void OnFinishAsync(GameObject result, AnimationClip[] clip)

    {
        ObjOfImport = result;
        isCreate = true;
        Debug.Log("Finished importing " + result.name);
        if (OnFinish != null)
            OnFinish(result,clip);
    }



}