using Siccity.GLTFUtility;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// ����GLB/GLTFģ��
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
    /// ��������glb��Դ
    /// </summary>
    /// <param name="url">ģ��·��</param>
    /// <param name="action">��������¼�</param>
    public void ImportGLB_GLTFAsyncByWed(string url, Action<GameObject, AnimationClip[]> OnFinishAsync)
    {
        StartCoroutine(DownLoadFile(url, OnFinishAsync));

    }
    /// <summary>
    /// ��������glb��Դ
    /// </summary>
    /// <param name="url">·��</param>
    public void ImportGLBAsyncByWed(string url)
    {
        StartCoroutine(DownLoadFile(url, null));

    }

    /// <summary>
    /// ����������Դ������
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
        //��������ļ��Ѵ��� ֱ�Ӽ���
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
    /// �첽���� gltf and glb
    /// </summary>
    /// <param name="filepath">·��</param>
    /// <param name="action">�ص�����</param>
    public void ImportGLB_GLTFAsync(string filepath, Action<GameObject, AnimationClip[]> OnFinishAsync)
    {
        if (!isCreate) return;
        isCreate = false;
        UnloadAndDestroy(ObjOfImport);
        Importer.LoadFromFileAsync(filepath, new ImportSettings(), this.OnFinishAsync);
        this.OnFinish = OnFinishAsync;
    }

    /// <summary>
    ///�첽����ģ��
    /// </summary>
    /// <param name="filepath"><·��/param>
    public void ImportGLB_GLTFAsync(string filepath)
    {

        if (!isCreate) return;

        isCreate = false;

        UnloadAndDestroy(ObjOfImport);

        Importer.LoadFromFileAsync(filepath, new ImportSettings(), OnFinish);
    }






    /// <summary>
    /// ɾ��ж�� ���м��ع���ģ�� �������
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
    /// ��ɼ���
    /// </summary>
    /// <param name="result">���س���������</param>
    /// <param name="clip">���صĶ���</param>
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