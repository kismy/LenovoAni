using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Text;
public class BlendShapeRetargeting : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] bool UseLiveLinkFace = false;
    [SerializeField] SkinnedMeshRenderer Original;

    [SerializeField] SkinnedMeshRenderer[] Targets;
    [SerializeField] BlendShapeMapping mapping;



    Socket udp;
    byte[] buffer;
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 35;
        //udp = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);
        //udp.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.171"),11111));

        //buffer = new byte[1024];

        //mapping.InitMapIndex(Original, Targets[0]);

    }

    // Update is called once per frame
    void Update()
    {
        if (Original != null)
        {
            //int length = udp.Receive(buffer);//接收数据报
            if (UseLiveLinkFace)
            {
                //if (length > 0)
                //{
                //    //string message = Encoding.ASCII.GetString(buffer, 0, length);
                //    //Debug.Log(message);
                //}

            }
            else
            {
                for (int i = 0; i < Targets[0].sharedMesh.blendShapeCount; i++)
                {
                    float weight = Original.GetBlendShapeWeight(i)/100f;
                    foreach (var target in Targets)
                    {
                        target.SetBlendShapeWeight(i, weight);
                    }
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.F1))
            LoadAni("IdleOfSitDown");
        else if (Input.GetKeyDown(KeyCode.F2))
            LoadAni("ThumbsUp");
        else if (Input.GetKeyDown(KeyCode.F3))
            LoadAni("HandClap");
        else if (Input.GetKeyDown(KeyCode.F4))
            LoadAni("Pray");
        else if (Input.GetKeyDown(KeyCode.F5))
            LoadAni("Sign");
        else if (Input.GetKeyDown(KeyCode.F6))
            LoadAni("Cheer");
        else if (Input.GetKeyDown(KeyCode.F7))
            LoadAni("OK");
        else if (Input.GetKeyDown(KeyCode.F8))
            LoadAni("Wave");
        else if (Input.GetKeyDown(KeyCode.F9))
            LoadAni("Think");
        else if (Input.GetKeyDown(KeyCode.F10))
            LoadAni("Yean");
        else if (Input.GetKeyDown(KeyCode.F11))
            LoadAni("Yes");
        else if (Input.GetKeyDown(KeyCode.F12))
            LoadAni("ThumbsDown");
    }

    private void LoadAni(string aniState)
    {
        animator.SetTrigger("On" + aniState);
    }
}
