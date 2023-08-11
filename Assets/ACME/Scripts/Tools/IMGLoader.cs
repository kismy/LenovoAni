using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using UnityEngine;

public class IMGLoader
{
   public static Texture2D LoadImage(string fullPath)
    {
        Texture2D texture = null;
        WWW www = new WWW(@"file://" + fullPath);
        using (www)
        {
            while (!www.isDone) { }

            if (www.texture != null)
            {
                texture = www.texture;
                texture.name = fullPath;
            }

        }
        www.Dispose();
        www = null;
        GC.Collect();
        Resources.UnloadUnusedAssets();
        return texture;
    }


}