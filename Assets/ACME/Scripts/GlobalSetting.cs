using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalSetting : MonoBehaviour {
    [SerializeField] Image logo;
    [SerializeField] RawImage startImage;

    void Awake()
	{        
        Debug.unityLogger.logEnabled = QuickConfig.instance.Debug;

        Cursor.visible = QuickConfig.instance.ShowCursor;

        if (QuickConfig.instance.ScreenControl)
        {
            Screen.SetResolution(QuickConfig.instance.ScreenWidth, QuickConfig.instance.ScreenHeight,FullScreenMode.FullScreenWindow);
        }
        if (QuickConfig.instance != null && QuickConfig.instance.ShowStartImage == true)
        {
            startImage.gameObject.SetActive(true);
            Texture2D texture2D = IMGLoader.LoadImage(Application.streamingAssetsPath + "/TOOLS/Start.ini");
            startImage.texture = texture2D;
            startImage.SetNativeSize();
        }
        else
            startImage.gameObject.SetActive(false);



        QuickConfig patcher = QuickConfig.LoadFromUnityPatcher();
        if (patcher != null && patcher.UnityPatcher == true)
        {
            logo.gameObject.SetActive(false);
        }
        else
            logo.gameObject.SetActive(true);
        
    }
}
