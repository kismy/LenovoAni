using UnityEngine;
using System.Collections;
//引入库  
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.IO;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UDPClient : MonoBehaviour
{
    private static UDPClient instance;
    public static UDPClient Instance
    {
        get { return instance; }
    }
    Socket socket; //目标socket  
    EndPoint remoteEndPoint; //侦听端口,和发送消息的Target  
    IPEndPoint localIPEndPoint;
    string sendStr; //发送的字符串  
    byte[] recvData = new byte[1024]; //接收的数据，必须为字节  
    byte[] sendData = new byte[1024]; //发送的数据，必须为字节  
    int recvLen; //接收的数据长度  
    Thread connectThread; //连接线程  
    private object mylock=new object();
    private string recvStr;

    public static Queue<string> msgQueueBackgound;
    public static Queue<string> msgQueue;


    //初始化  
    void InitSocket()
    {
        //定义侦听端口,侦听任何IP  
        localIPEndPoint = new IPEndPoint(IPAddress.Parse(QuickConfig.instance.LocalIP), QuickConfig.instance.LocalPort);
        //定义套接字类型,在主线程中定义  
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        uint IOC_IN = 0x80000000;
        uint IOC_VENDOR = 0x18000000;
        uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
        socket.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);

        //服务端需要绑定ip  
        socket.Bind(localIPEndPoint);
        //定义客户端  
        IPEndPoint remote = new IPEndPoint(IPAddress.Parse(QuickConfig.instance.RemoteIP), QuickConfig.instance.RemotePort);
        remoteEndPoint = (EndPoint)remote;
        //print("waiting for UDP dgram");

        //开启一个线程连接，必须的，否则主线程卡死  
        Loom.RunAsync(() => {
            connectThread = new Thread(SocketReceive);
            connectThread.Start();
        });
    }

    public void SocketSend(string sendStr)
    {
        //清空发送缓存  
        sendData = new byte[1024];
        //数据类型转换  
        sendData = Encoding.UTF8.GetBytes(sendStr);
        //发送给指定客户端  
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, remoteEndPoint);
    }
    /// <summary>
    /// 发送16进制数据
    /// </summary>
    /// <param name="sendStr"></param>
    public void SocketHexSend(string sendStr)
    {

        //清空发送缓存  
        sendData = HexStringToByteArray(sendStr);
        //数据类型转换  

        //发送给指定服务端  
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, remoteEndPoint);
    }


    //服务器接收  
    void SocketReceive()
    {
        EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0) as EndPoint;
        //进入接收循环  
        while (true)
        {
            //对data清零  
            recvData = new byte[1024];
            //获取客户端，获取客户端数据，用引用给客户端赋值  
            recvLen = socket.ReceiveFrom(recvData, ref endPoint);
            // print("message from: " + clientEnd.ToString()); //打印客户端信息  
            //输出接收到的数据  

            lock (mylock) {
                recvStr = Encoding.UTF8.GetString(recvData, 0, recvLen);
            }


        }
    }

    //连接关闭  
    void SocketQuit()
    {
        //关闭线程  
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        //最后关闭socket  
        if (socket != null)
            socket.Close();
        //print("disconnect");
    }

    /// <summary>
    /// 字节数组转16进制字符串
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    private static string byteToHexStr(byte[] bytes)
    {
        string returnStr = "";
        if (bytes != null)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                returnStr += bytes[i].ToString("X2");
            }
        }
        return returnStr;
    }

    /// <summary>
    /// 16进制转字节数组
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static byte[] HexStringToByteArray(string s)
    {
        s = s.Replace(" ", "");
        byte[] buffer = new byte[s.Length / 2];
        for (int i = 0; i < s.Length; i += 2)
            buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
        return buffer;
    }


    // Use this for initialization  
    void Awake()
    {
        instance = this;
        InitSocket(); //在这里初始化server 
        OnReceive += PCController.OnReceiveMsg;


    }


    void OnDestroy()
    {
        SocketQuit();
    }

    public Action<String> OnReceive;

    private void Update()
    {
        lock (mylock)
        {
            if (!string.IsNullOrEmpty(recvStr) && OnReceive != null)
            {
                OnReceive(recvStr);
                recvStr = "";

            }
        }
    }


}