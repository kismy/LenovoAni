using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImagePlayer : MonoBehaviour {
    private enum PathType
    {
        AbsolutePath,
        RelativeToDataPath,
        RelativeToStreamingAssetPath,
    }
    [SerializeField] PathType pathType;
    [SerializeField] string imagePath;

    [SerializeField] RawImage ImageL;
    [SerializeField] RawImage ImageR;
    [SerializeField] RectTransform ContentRect;
    RectTransform rectTransform;

    private List<Texture2D> textureList;
    private int index = 0;
    private bool isReadyToShowNextImage = true;
    [SerializeField] private float speed = 5;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ContentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.width * 2);
        ContentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height);
        ContentRect.pivot = new Vector2(0, 0.5f);
        ContentRect.anchorMin = new Vector2(0, 0.5f);
        ContentRect.anchorMax = new Vector2(0, 0.5f);
        ContentRect.anchoredPosition = new Vector2(0, 0);

        ImageL.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.width);
        ImageL.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height);
        ImageL.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        ImageL.rectTransform.anchorMin = new Vector2(0.25f, 0.5f);
        ImageL.rectTransform.anchorMax = new Vector2(0.25f, 0.5f);
        ImageL.rectTransform.anchoredPosition = new Vector2(0, 0);

        ImageR.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.width);
        ImageR.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height);
        ImageR.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        ImageR.rectTransform.anchorMin = new Vector2(0.75f, 0.5f);
        ImageR.rectTransform.anchorMax = new Vector2(0.75f, 0.5f);
        ImageR.rectTransform.anchoredPosition = new Vector2(0, 0);

        LoadTextures();
        if (textureList.Count >= 1)
            SetTexture(ImageL, textureList[0]);
        else
            gameObject.SetActive(false);
    }




    void LoadTextures()
    {
        string path = null;
        switch (pathType)
        {
            case PathType.AbsolutePath:
                path = imagePath;
                break;
            case PathType.RelativeToDataPath:
                path = Application.dataPath + "/" + imagePath;
                break;
            case PathType.RelativeToStreamingAssetPath:
                path = Application.streamingAssetsPath + "/" + imagePath;
                break;
            default:
                break;
        }
        textureList = new List<Texture2D>();

        if (!Directory.Exists(path)) return;

        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] fileInfos = directoryInfo.GetFiles();
        if (fileInfos != null && fileInfos.Length > 0)
        {
            foreach (var fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(".png") || fileInfo.Name.EndsWith(".jpg"))
                {

                    Texture2D texture = IMGLoader.LoadImage(fileInfo.FullName);
                    texture.name = fileInfo.Name;
                    textureList.Add(texture);
                }
            }
        }
    }

    public void Next()
    {
        if (textureList == null || textureList.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        if (isReadyToShowNextImage)
        {
            isReadyToShowNextImage = false;
            StartCoroutine(ShowNext());
        }
    }
    IEnumerator ShowNext()
    {
        index++;
        if (index > textureList.Count - 1)
            index = 0;
        SetTexture(ImageR, textureList[index]);
        ContentRect.pivot = new Vector2(0, 0.5f);
        ContentRect.anchoredPosition = Vector2.zero;

        yield return null;
        Vector2 target = new Vector2(-rectTransform.rect.width, ContentRect.anchoredPosition.y);

        while (true)
        {
            if (ContentRect.anchoredPosition.x + rectTransform.rect.width > 0.5f)
                ContentRect.anchoredPosition = Vector2.Lerp(ContentRect.anchoredPosition, target, Time.deltaTime * speed);
            else
            {
                ContentRect.anchoredPosition = Vector2.zero;
                SetTexture(ImageL, textureList[index]);
                isReadyToShowNextImage = true;
                yield break;
            }
            yield return null;
        }
    }

    public void Pre()
    {
        if (textureList == null || textureList.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        if (isReadyToShowNextImage)
        {
            isReadyToShowNextImage = false;
            StartCoroutine(ShowPre());
        }
    }
    IEnumerator ShowPre()
    {
        index--;
        if (index < 0)
            index = textureList.Count - 1;
        SetTexture(ImageR, ImageL.texture);

        SetTexture(ImageL, textureList[index]);
        ContentRect.pivot = new Vector2(0.5f, 0.5f);
        ContentRect.anchoredPosition = Vector2.zero;

        yield return null;
        Vector2 target = new Vector2(rectTransform.rect.width, ContentRect.anchoredPosition.y);
        while (true)
        {
            if (rectTransform.rect.width - ContentRect.anchoredPosition.x > 0.5f)
                ContentRect.anchoredPosition = Vector2.Lerp(ContentRect.anchoredPosition, target, Time.deltaTime * speed);
            else
            {
                ContentRect.anchoredPosition = Vector2.zero;
                SetTexture(ImageR, textureList[index]);
                isReadyToShowNextImage = true;
                yield break;
            }
            yield return null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Pre();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            Next();
    }
    private void SetTexture(RawImage image, Texture texture)
    {
        SetTexture(image,  texture as Texture2D);
    }
    private void SetTexture(RawImage image,Texture2D texture)
    {
        if (image != null && texture != null)
        {
            float aspect = rectTransform.rect.width / (float)rectTransform.rect.height;
            float aspect2 = texture.width / (float)texture.height;
            if (aspect>aspect2)//若宽一样，texture比现实框高，为了现实全所以Y轴不缩放，缩小X轴
            {
                image.rectTransform.localScale = new Vector3(aspect2/aspect, 1,1);
            }
            else
            {
                image.rectTransform.localScale = new Vector3(1, aspect/ aspect2, 1);

            }
            image.texture = texture;
        }
    }
}
