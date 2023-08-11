using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      AvatarBone avatar=  CustomAvatar.Load("D:\\WorkSpace\\LenovoAni\\Assets\\StreamingAssets\\SkeletonModifyData\\CustomAvatar.avatar");
        
    }
}
