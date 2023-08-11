using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class TCPClient:MonoBehaviour
{
    object myLock = new object();
    Queue<string> TempMsgQueue;
    Queue<string> msgQueue;
    Thread socketThread = null;
    static Socket clientSocket;
    IPEndPoint ServerIPEndPoint;
    public Action<string> OnReceiveMsg;
    float TIME = 3;
    float time = 3;

    private void Start()
    {
        OnReceiveMsg += (msg) =>
        {
            Debug.Log(msg);
        };
        socketThread = new Thread(StartSocket);
        socketThread.Start();
    }

    void Update()
    {
        //消息处理
        if (msgQueue == null && TempMsgQueue != null)
        {
            lock (myLock)
            {
                msgQueue = TempMsgQueue;
                TempMsgQueue = null;
            }

            while (msgQueue.Count > 0)
            {
                string msg = msgQueue.Dequeue();
                if (OnReceiveMsg != null && !string.IsNullOrEmpty(msg))
                {
                    OnReceiveMsg(msg);
                }
            }
            msgQueue = null;
        } 

        //断线重连
        if (clientSocket.Connected == false)
        {
            if (time > 0)
                time -= Time.deltaTime;
            else
            {
                time = TIME;
                if (socketThread != null)
                {
                    socketThread.Interrupt();
                    socketThread.Abort();
                }
                socketThread = new Thread(StartSocket);
                socketThread.Start();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            SendMessage("I' a Client.");
        }
    }

    private void StartSocket()
    {
        //1）创建客户端Socket
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress serverIPAddress = IPAddress.Parse(QuickConfig.instance.RemoteIP);
        ServerIPEndPoint = new IPEndPoint(serverIPAddress, QuickConfig.instance.RemotePort);
        clientSocket.Connect(ServerIPEndPoint);

        //3）接收来自server的消息,
        byte[] data = new byte[1024];
        while (true)
        {    
            int count = clientSocket.Receive(data);
            string msg = Encoding.UTF8.GetString(data, 0, count);
            lock (myLock)
            {
                if (TempMsgQueue == null)
                    TempMsgQueue = new Queue<string>();
                TempMsgQueue.Enqueue(msg);
            }
        }
    }   

    public static void SokectSend(string msg)
    {
        if (clientSocket != null)
            clientSocket.Send(Encoding.UTF8.GetBytes(msg));
    }      

    private void OnDestroy()
    {
        socketThread.Interrupt();
        socketThread.Abort();
        if (clientSocket!=null)
            clientSocket.Close();
    }
}