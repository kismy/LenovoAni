using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Text;
public class BlendShapeRetargeting : MonoBehaviour
{
    [SerializeField] bool UseLiveLinkFace = false;
    [SerializeField] SkinnedMeshRenderer Original0_51;

    [SerializeField] SkinnedMeshRenderer Face0_50;
    [SerializeField] SkinnedMeshRenderer Tongue51;
    [SerializeField] BlendShapeMapping mapping;



    Socket udp;
    byte[] buffer;
    private void Start()
    {
        //udp = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);
        //udp.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.171"),11111));

        //buffer = new byte[1024];

        mapping.InitMapIndex(Original0_51, Face0_50, Tongue51);

    }

    // Update is called once per frame
    void Update()
    {
        if (Original0_51 != null)
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
                for (int i = 0; i < 52; i++)
                {
                    float weight = Original0_51.GetBlendShapeWeight(mapping.IndexMapping[i].IdFrom);
                    if (mapping.IndexMapping[i].IdTo != -1)
                        Face0_50.SetBlendShapeWeight(mapping.IndexMapping[i].IdTo, weight);
                    else
                    {
                        Debug.Log(mapping.IndexMapping[i].To +" : -1");
                    }
                }
            }
        }
    }
}
