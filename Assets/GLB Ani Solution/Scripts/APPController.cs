using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APPController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Func());
    }

    IEnumerator Func()
    {
        while (true)
        {

            yield return new WaitForSeconds(3);
            PCController.Zuixiaohua();
            yield return new WaitForSeconds(3);
            PCController.ZuiDahua();
        }
    
    }
}
